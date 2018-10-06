using System;
using System.Collections.Generic;
using System.Concurrency;
using EightTracksPlayer;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using ReactiveUI;
using StoryQ;
using StoryQ.Formatting.Parameters;


//TODO: add tests for ApplyFilter command and browsing mixes with filter
namespace EightTracksTests
{
    [TestFixture]
    public class MediaLibraryBrowserTests
    {
        private Feature browsingMixes = Features.BrowsingAvailableMixes;
        private Feature favoritingItemsInTheMediaLibrary = Features.FavoritingItemsInTheMediaLibrary;
        
        private const string mediaLibraryBrowsingTag = Tags.MediaLibraryBrowsing;
        private const string domainTag = Tags.Domain;
        
        private List<Mix> mixesList;
        private IScheduler origSched;
        private Mix mix;

        private MixFilter mixFilter;
        
        private RequestExecutorStub requestExecutor;
        private IMediaLibraryBrowser mediaLibraryBrowser;

        private IResponse mixLikedResponse = new ToggleMixLikeResponse() {LoggedIn=true, Mix = new MixElement() {LikedByCurrentUser = true}};
        private IResponse mixUnlikedResponse = new ToggleMixLikeResponse() { LoggedIn = true, Mix = new MixElement() { LikedByCurrentUser = false } };

        private IResponse mixesResponse = new MixesResponse()
        {
            MixesElement = new MixElement[]
                                {
                                    new MixElement(){CoverUrls = new CoverUrlsElement(), UserElement = new UserElement()},
                                    new MixElement(){CoverUrls = new CoverUrlsElement(), UserElement = new UserElement()},
                                    new MixElement(){CoverUrls = new CoverUrlsElement(), UserElement = new UserElement()}
                                }
        };

//        private Mix similarMix;


        [SetUp]
        public void SetUp()
        {

            origSched = RxApp.DeferredScheduler;
            RxApp.DeferredScheduler = new TestScheduler();

            requestExecutor = new RequestExecutorStub();
            mediaLibraryBrowser = new MediaLibraryBrowser(requestExecutor);
            mix = new Mix(new MixElement(), 0);
            mixFilter = new MixFilter();
        }

        [TearDown]
        public void TearDown()
        {
            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestGetsLatestMixes()
        {
            requestExecutor.Responses.Add(mixesResponse);
            browsingMixes
                .WithScenario("getting recent mixes").Tag(mediaLibraryBrowsingTag).Tag(domainTag)
                .Given(AnyConditions)
                .When(TheApplicationIssuesACommandToGetTheRecentMixes)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
                    .And(TheRequestQueriesFor_Mixes, MixesViewType.Recent)
                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
                .ExecuteWithReport();
        }

        [Test]
        public void TestGetsPopularMixes()
        {
            requestExecutor.Responses.Add(mixesResponse);
            browsingMixes
                .WithScenario("getting popular mixes").Tag(mediaLibraryBrowsingTag).Tag(domainTag)
                .Given(AnyConditions)
                .When(TheApplicationIssuesACommandToGetThePopularMixes)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
                    .And(TheRequestQueriesFor_Mixes, MixesViewType.Popular)
                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToGetThePopularMixes()
        {
            mixesList = mediaLibraryBrowser.GetPopularMixes(mixFilter);
        }

        [Test]
        public void TestGetsHotMixes()
        {
            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);
            browsingMixes
                .WithScenario("getting hot mixes").Tag(mediaLibraryBrowsingTag).Tag(domainTag)
                .Given(AnyConditions)
                .When(TheApplicationIssuesACommandToGetTheHotMixes)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
                    .And(TheRequestQueriesFor_Mixes, MixesViewType.Hot)
                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToGetTheHotMixes()
        {
            mixesList = mediaLibraryBrowser.GetHotMixes(mixFilter);
        }

        private void TheMediaLibraryBrowserExecutesA_(Type requestType)
        {
            Assert.That(requestExecutor.Requests[0], Is.TypeOf(requestType));
        }

        private void TheMediaLibraryBrowserReturnsAListOfMixes()
        {
            Assert.That(mixesList.Count, Is.EqualTo(3));
        }

        private void TheApplicationIssuesACommandToGetTheRecentMixes()
        {
            mixesList = mediaLibraryBrowser.GetRecentlyMadeMixes(mixFilter);
        }

        private void AnyConditions()
        {

        }

        [Test]
        public void TestLikesACertainMix()
        {
            requestExecutor.Responses.Add(mixLikedResponse);
            favoritingItemsInTheMediaLibrary
                .WithScenario("Liking a mix")
                .Given(AMixIs_LikedByCurrentUser, false)
                .When(TheApplicationIssuesACommandToToggleLikingAMix)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(ToggleMixLikeRequest))
                    .And(TheMix_LikedByTheCurrentUser, true)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToToggleLikingAMix()
        {
            mediaLibraryBrowser.ToggleLike(mix);
        }

        private void TheMix_LikedByTheCurrentUser([BooleanParameterFormat("should be", "should not be")] bool shouldBeLikedByTheUser)
        {
            Assert.That(mix.LikedByCurrentUser, Is.EqualTo(shouldBeLikedByTheUser));
        }

        private void AMixIs_LikedByCurrentUser([BooleanParameterFormat("", "not")]bool isLikedByCurrentUser)
        {
            mix.LikedByCurrentUser = isLikedByCurrentUser;
        }

        [Test]
        public void TestUnlikesACertainMix()
        {
            requestExecutor.Responses.Add(mixUnlikedResponse);
            favoritingItemsInTheMediaLibrary
                .WithScenario("Unliking a mix")
                .Given(AMixIs_LikedByCurrentUser, true)
                .When(TheApplicationIssuesACommandToToggleLikingAMix)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(ToggleMixLikeRequest))
                    .And(TheMix_LikedByTheCurrentUser, false)
                .ExecuteWithReport();
        }

        [Test]
        public void TestGetsLikedMixes()
        {
            requestExecutor.Responses.Add(mixesResponse);
            browsingMixes
                .WithScenario("getting liked mixes").Tag(mediaLibraryBrowsingTag).Tag(domainTag)
                .Given(AnyConditions)
                .When(TheApplicationIssuesACommandToGetTheLikedMixes)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
                    .And(TheRequestQueriesFor_Mixes, MixesViewType.Liked)
                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
                .ExecuteWithReport();
        }

        [Test]
        public void TestGetsFeedMixes()
        {
            requestExecutor.Responses.Add(mixesResponse);
            browsingMixes
                .WithScenario("getting mixes from the feed").Tag(mediaLibraryBrowsingTag).Tag(domainTag)
                .Given(AnyConditions)
                .When(TheApplicationIssuesACommandToGetTheFeedMixes)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
                    .And(TheRequestQueriesFor_Mixes, MixesViewType.Feed)
                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
                .ExecuteWithReport();
        }

        private void TheRequestQueriesFor_Mixes(MixesViewType viewType)
        {
            IRequest lastRequest = requestExecutor.Requests[requestExecutor.Requests.Count - 1];
            Assert.That(lastRequest, Is.TypeOf(typeof (MixesRequest)));
            Assert.That(((MixesRequest)lastRequest).ViewType, Is.EqualTo(viewType));
        }

        private void TheApplicationIssuesACommandToGetTheFeedMixes()
        {
            mixesList = mediaLibraryBrowser.GetFeedMixes(mixFilter, "aUserSlug");
        }

        [Test]
        public void TestGetsMixesByTags()
        {
//            browsingMixes
//                .WithScenario("getting mixes that match certain tags")
//                .Given(ASetOfTags_, tagList)
//                .When(TheApplicationIssuesACommandToGetTheRecentMixes)
//                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
//                    .And(TheRequestQueriesFor_Mixes, MixesViewType.Recent)
//                    .And(ThatRequestContainsTags_, tagList)
//                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
//                
//
//                .WithScenario("getting mixes wihtout specifying tags")
//                .Given(AnEmptySetOfTags)
//                .When(TheApplicationIssuesACommandToGetTheRecentMixes)
//                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
//                    .And(TheRequestQueriesFor_Mixes.MixesSortEnum.Recent)
//                    .And(ThatRequestContainsNoTags)
//                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
//                .ExecuteWithReport();

        }

        public void TestAppliesFilterOnCurrentlyBrowsedTypeOfMixes()
        {
            // testing the ApplyFilter command
        }

        [Test]
        public void TestGetsTheNextSetOfMixResults()
        {
            int pageSize = 3;
            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);

            browsingMixes
                .WithScenario("getting the next set of mix results")
                .Given(TheMediaLibraryBrowserHadAtSomePointRetrieved_MostRecentMixes, pageSize)
                    .And(AfterThatItRetrieved_MostHotMixes, pageSize)
                .When(TheApplicationIssuesAComandToGetTheNextSetOfMixes)
                .Then(TheMediaLibraryBrowserExecutesA_, typeof(MixesRequest))
                    .And(TheRequestQueriesForPage_Of_Mixes, 2, MixesViewType.Hot)
                    .And(TheMediaLibraryBrowserReturnsAListOfMixes)
                .ExecuteWithReport();
        }

        private void TheRequestQueriesForPage_Of_Mixes(int pageNumber, MixesViewType viewType)
        {
            Assert.That(((MixesRequest)requestExecutor.Requests[2]).PageNumber, Is.EqualTo(pageNumber));
            Assert.That(((MixesRequest)requestExecutor.Requests[2]).ViewType, Is.EqualTo(viewType));
        }

        private void TheApplicationIssuesAComandToGetTheNextSetOfMixes()
        {
            mixesList = mediaLibraryBrowser.GetMoreMixes();
        }

        private void AfterThatItRetrieved_MostHotMixes(int pageSize)
        {
            mediaLibraryBrowser.PageSize = pageSize;
            mediaLibraryBrowser.GetHotMixes(mixFilter);
        }

        private void TheMediaLibraryBrowserHadAtSomePointRetrieved_MostRecentMixes(int pageSize)
        {
            mediaLibraryBrowser.PageSize = pageSize;
            mediaLibraryBrowser.GetRecentlyMadeMixes(mixFilter);
        }

        private void TheApplicationIssuesACommandToGetTheLikedMixes()
        {
            mixesList = mediaLibraryBrowser.GetLikedMixes(mixFilter, "aUserSlug");
        }
    }
}