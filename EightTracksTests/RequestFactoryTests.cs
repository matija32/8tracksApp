using System;
using System.Concurrency;
using System.IO;
using System.Net;
using EightTracksPlayer.Communication;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Domain.Entities;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using ReactiveUI;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class RequestFactoryTests
    {

        private IRequest request;
        
        private Feature playingAMixFeature;
        private Feature userAuthenticationFeature;
        
        private string httpWebRequestsTag;
        private string infrastructureTag;
        private RequestFactory requestFactory;

        private string apiKeyValue = "ABC123";
        private int mixId;
        private string playToken;
        private IScheduler origSched;
        private Feature favoritingItemsInTheMediaLibrary;
        private string userToken;
        private Feature browsingAvailableMixes;
        private MixesViewType viewType;
        private int pageNumber;
        private string userSlug;
        private Feature continuousAudioPlayback;
        private long mixSetId;
        private MixFilter mixFilter;


        [TestFixtureSetUp]
        public void Initialize()
        {
            playingAMixFeature = Features.PlayingAMix;
            userAuthenticationFeature = Features.AuthenticatingAnUser;
            favoritingItemsInTheMediaLibrary = Features.FavoritingItemsInTheMediaLibrary;
            browsingAvailableMixes = Features.BrowsingAvailableMixes;
            continuousAudioPlayback = Features.ContinuousAudioPlayback;

            infrastructureTag = Tags.Infrastructure;
            httpWebRequestsTag = Tags.HttpWebRequests;

            
        }

        [SetUp]
        public void ResetFields()
        {
            request = null;
            origSched = RxApp.DeferredScheduler;
            RxApp.DeferredScheduler = new TestScheduler();
            mixFilter = new MixFilter();

            requestFactory = new RequestFactory(apiKey: apiKeyValue);

            mixId = 500;
            mixSetId = 150000;
            playToken = "1234509876";
            userToken = "abcdefg";
            viewType = MixesViewType.Feed;
            pageNumber = 1;
            requestFactory.PageSize = 20;
        }

        [TearDown]
        public void TearDown()
        {
            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestCreatesMixesRequest()
        {
            int pageSize = 15;
            int pageNumber = 3;
            string expectedUriNotLoggedIn = String.Format(@"http://8tracks.com/mixes.xml?api_key={0}&sort=recent", apiKeyValue);
            string expectedUriLoggedIn = expectedUriNotLoggedIn + "&user_token=" + userToken;
            string defaultPagination = String.Format("&per_page={0}&page={1}", requestFactory.PageSize, this.pageNumber);
            string expectedUriPaginatedLoggedIn = expectedUriLoggedIn + "&per_page=" + pageSize + "&page=" + pageNumber;
            browsingAvailableMixes
                .WithScenario("creating a request for obtaining latest mixes when no user is logged in").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                    .And(TheUserTokenIs_, String.Empty)
                    .And(TheViewTypeIs_, MixesViewType.Recent)
                .When(TheApplicationIssuesACommandToCreateMixesRequest)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUriNotLoggedIn+defaultPagination)
                
                .WithScenario("creating a request for obtaining latest mixes when a user is logged in").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                        .And(TheUserTokenIs_, userToken)
                        .And(TheViewTypeIs_, MixesViewType.Recent)
                .When(TheApplicationIssuesACommandToCreateMixesRequest)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUriLoggedIn+defaultPagination)

                .WithScenario("creating a request for obtaining second set of mixes").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                        .And(TheUserTokenIs_, userToken)
                        .And(TheViewTypeIs_, MixesViewType.Recent)
                        .And(ThePageNumerIs_With_MixesPerPage, pageNumber, pageSize)
                .When(TheApplicationIssuesACommandToCreateMixesRequest)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUriPaginatedLoggedIn)

                .ExecuteWithReport();
        }

        [Test]
        public void TestCreateFilteredMixesRequest()
        {
            string tags = "jazz hiphop cover";
            string query = "summer hit";
            string expectedUri = String.Format(@"http://8tracks.com/mixes.xml?api_key={0}&sort=recent&per_page=20&page=1&tags={1}&q={2}", apiKeyValue, tags, query);

            browsingAvailableMixes
                .WithScenario("filtering available mixes by tags and other keywords").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(TheViewTypeIs_, MixesViewType.Recent)
                    .And(TheTagsAre_And_And_, "jazz", "hiphop", "cover")
                    .And(TheQueryIs_, "summer hit")
                .When(TheApplicationIssuesACommandToCreateMixesRequest)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUri)
                .ExecuteWithReport();

        }

        private void TheQueryIs_(string query)
        {
            mixFilter.Query = query;
        }

        private void TheTagsAre_And_And_(string tag1, string tag2, string tag3)
        {
            mixFilter.Tags.Add(tag1);
            mixFilter.Tags.Add(tag2);
            mixFilter.Tags.Add(tag3);
        }

        private void ThePageNumerIs_With_MixesPerPage(int pageNumber, int pageSize)
        {
            this.pageNumber = pageNumber;
            requestFactory.PageSize = pageSize;
        }

        private void TheApplicationIssuesACommandToCreateMixesRequest()
        {
            request = requestFactory.CreateMixesRequest(mixFilter, viewType, pageNumber, userSlug);
        }

        [Test]
        public void TestCreatesNewPlayToken()
        {
            string expectedUri = @"http://8tracks.com/sets/new.xml?api_key=" + apiKeyValue;
            playingAMixFeature
                .WithScenario("creating a request for obtaining the play token needed to start streaming music").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                .When(TheApplicationIssuesACommandToCreateNewPlayTokenRequest)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUri)
                .ExecuteWithReport();
        }

        private void TheUriShouldBe_(string expectedUri)
        {
            Assert.That(request.Url, Is.EqualTo(expectedUri));
        }

        private void ApiKeyIs_(string apiKeyValue)
        {
            Assert.That(requestFactory.ApiKey, Is.EqualTo(apiKeyValue));
        }

        private void The_RequestShouldBeProperlyCreated(string method)
        {
            Assert.That(request, Is.Not.EqualTo(null));
            Assert.That(request.GetHttpWebRequest().Method, Is.EqualTo(method));
        }

        private void TheApplicationIssuesACommandToCreateNewPlayTokenRequest()
        {
            request = requestFactory.CreateNewPlayTokenRequest();
        }

        [Test]
        public void TestCreatesPlayMixRequest()
        {
            string expectedUri = String.Format(@"http://8tracks.com/sets/{0}/play.xml?mix_id={1}&api_key={2}", playToken, mixId, apiKeyValue);
            playingAMixFeature
                .WithScenario("creating a request to start playing a certain mix").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                .When(TheApplicationIssuesACommandToCreatePlayMixRequestForMixWithId_UsingAToken_, mixId, playToken)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUri)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToCreatePlayMixRequestForMixWithId_UsingAToken_(int mixId, string playToken)
        {
            request = requestFactory.CreatePlayMixRequest(playToken, mixId);
        }

        [Test]
        public void TestCreatesNextSongRequest()
        {
            string expectedUri = String.Format(@"http://8tracks.com/sets/{0}/next.xml?mix_id={1}&api_key={2}", playToken, mixId, apiKeyValue);
            playingAMixFeature
                .WithScenario("creating a request to play the next song in the mix").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                .When(TheApplicationIssuesACommandToCreateNextSongRequestForMixWithId_UsingAToken_, mixId, playToken)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUri)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToCreateNextSongRequestForMixWithId_UsingAToken_(int mixId, string playToken)
        {
            request = requestFactory.CreateNextSongRequest(playToken, mixId, false);
        }

        [Test]
        public void TestCreatesLoginRequest()
        {
            string username = "myUsername";
            string password = "myPassword";
            string expectedPostData = String.Format(@"login={0}&password={1}", username, password);
            string expectedUri = "http://8tracks.com/sessions.xml?api_key="+apiKeyValue;

            userAuthenticationFeature
                .WithScenario("creating a request to log in").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                .When(TheApplicationIssuesACommandToCreateALoginRequestWithUsername_AndPassword_, username, password)
                .Then(The_RequestShouldBeProperlyCreated, "POST")
                    .And(TheUriShouldBe_, expectedUri)
                    .And(ThePostDataShouldBe_, expectedPostData)
                .ExecuteWithReport();

        }

        private void ThePostDataShouldBe_(string expectedPostData)
        {
            HttpWebRequest httpWebRequest = request.GetHttpWebRequest();
            Assert.That(httpWebRequest.ContentLength, Is.EqualTo(expectedPostData.Length));
        }

        private void TheApplicationIssuesACommandToCreateALoginRequestWithUsername_AndPassword_(string username, string password)
        {
            request = requestFactory.CreateLoginRequest(username, password);
        }

        [Test]
        public void TestCreateToggleMixLikeRequest()
        {
            string expectedUri = String.Format("http://8tracks.com/mixes/{0}/toggle_like.xml?user_token={1}&api_key={2}",mixId, userToken ,apiKeyValue);
            
            favoritingItemsInTheMediaLibrary
                .WithScenario("Creating a request to toggle liking a mix").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                    .And(TheUserTokenIs_, userToken)
                .When(TheApplicationIssuesACommandToCreateAToggleLikeMixRequestForMixId_, mixId)
                .Then(The_RequestShouldBeProperlyCreated, "POST")
                    .And(TheUriShouldBe_, expectedUri)
                    .And(ThePostDataShouldBe_, String.Empty)
                .ExecuteWithReport();
                
        }

        private void TheApplicationIssuesACommandToCreateAToggleLikeMixRequestForMixId_(int mixId)
        {
            request = requestFactory.CreateToggleMixLikeRequest(mixId);
        }

        private void TheUserTokenIs_(string userToken)
        {
            requestFactory.UserToken = userToken;
        }

        [Test]
        public void TestCreateUserMixesRequest()
        {
            string userSlug = "MrJohnDoe";
            string expectedUri = String.Format("http://8tracks.com/users/{0}/mixes.xml?api_key={1}&user_token={2}&view=liked", userSlug, apiKeyValue, userToken);
            string expectedNextPageUri = expectedUri + "&per_page=" + requestFactory.PageSize + "&page=" + (pageNumber+1);
            string defaultPagination = String.Format("&per_page={0}&page={1}", requestFactory.PageSize, pageNumber);
            MixesViewType viewType = MixesViewType.Liked;

            browsingAvailableMixes
                .WithScenario("creating a request for obtaining latest liked mixes of an user").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                    .And(TheUserTokenIs_, userToken)
                    .And(TheUserIdentifierIs_, userSlug)
                    .And(TheViewTypeIs_, viewType)
                .When(TheApplicationIssuesACommandToCreateAUser_MixesRequestForUser_, viewType, userSlug)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUri + defaultPagination)

            .WithScenario("creating a request for obtaining second set of mixes").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                    .And(TheUserTokenIs_, userToken)
                    .And(TheViewTypeIs_, viewType)
                    .And(TheUserIdentifierIs_, userSlug)
                    .And(ThePageNumerIs_With_MixesPerPage, pageNumber, requestFactory.PageSize)
                .When(TheApplicationIssuesACommandToCreateMoreMixesRequest)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedNextPageUri)

                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToCreateMoreMixesRequest()
        {
            request = requestFactory.CreateMoreMixesRequest((MixesRequest) request);
        }

        private void TheUserIdentifierIs_(string userSlug)
        {
            this.userSlug = userSlug;
        }

        private void TheViewTypeIs_(MixesViewType viewType)
        {
            this.viewType = viewType;
        }

        private void TheApplicationIssuesACommandToCreateAUser_MixesRequestForUser_(MixesViewType viewType, string userSlug)
        {
            request = requestFactory.CreateMixesRequest(mixFilter, viewType, pageNumber, userSlug);
        }

        [Test]
        public void TestCreatesNextMixRequest()
        {
            string expectedUri = String.Format(@"http://8tracks.com/sets/{0}/next_mix.xml?mix_id={1}&api_key={2}&mix_set_id={3}", playToken, mixId, apiKeyValue, mixSetId);

            continuousAudioPlayback
                .WithScenario("creating a request to get the next mix in the mix set").Tag(httpWebRequestsTag).Tag(infrastructureTag)
                .Given(ApiKeyIs_, apiKeyValue)
                .When(TheApplicationIssuesACommandToCreateNextSongRequestForMixWithId_WithinASet_UsingAToken_, mixId, mixSetId, playToken)
                .Then(The_RequestShouldBeProperlyCreated, "GET")
                    .And(TheUriShouldBe_, expectedUri)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToCreateNextSongRequestForMixWithId_WithinASet_UsingAToken_(int mixId, long mixSetId, string playToken)
        {
            request = requestFactory.CreateNextMixRequest(mixId, mixSetId, playToken);
        }
    }
}