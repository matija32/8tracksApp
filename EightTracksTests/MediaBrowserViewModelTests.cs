using System;
using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics;
using System.Threading;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using EightTracksPlayer.ViewModels;
using EightTracksPlayer.ViewModels.Factories;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using ReactiveUI;
using ReactiveUI.Testing;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class MediaBrowserViewModelTests
    {
        private const string applicationTag = Tags.Application;
        private const string mediaBrowsingTag = Tags.MediaLibraryBrowsing;
        private Feature browsingAvailableMixes = Features.BrowsingAvailableMixes;
//        private TestScheduler testScheduler;
//        private IScheduler origSched;
        private ManualResetEvent lockObject;
        private RequestExecutorStub requestExecutor;
        private MediaBrowserViewModel mediaBrowserViewModel;
        private AudioPlayerStub audioPlayerStub;
        private IMediaLibraryBrowser mediaLibraryBrowser;
        private Authenticator authenticator;
        private PlaybackController playbackController;
        private string mixName1 = "mixName1";
        private string mixName2 = "mixName2";

        private MixesResponse mixesResponse = new MixesResponse()
        {
            MixesElement = new MixElement[]
                                {
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() },
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() },
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() }
                                }
        };

        private MixesResponse likedMixesResponse = new MixesResponse()
        {
            MixesElement = new MixElement[]
                                {
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() },
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() }
                                }
        };

        private MixesResponse feedMixesResponse = new MixesResponse()
        {
            MixesElement = new MixElement[]
                                {
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() },
                                    new MixElement() { CoverUrls = new CoverUrlsElement(), UserElement = new UserElement() }
                                }
        };

        private LoginFormViewModel loginFormViewModel;

        [SetUp]
        public void SetUp()
        {
            //            testScheduler = new TestScheduler();
            //            origSched = RxApp.DeferredScheduler;
            //            RxApp.DeferredScheduler = testScheduler;

            lockObject = new ManualResetEvent(false);
            requestExecutor = new RequestExecutorStub();
            audioPlayerStub = new AudioPlayerStub();
            playbackController = new PlaybackController(audioPlayerStub, requestExecutor);
            authenticator = new Authenticator(requestExecutor, new SettingsStub());
            mediaLibraryBrowser = new MediaLibraryBrowser(requestExecutor);


            requestExecutor.Responses.Add(new LoginResponse()
            {
                LoggedIn = false,
                CurrentUserElement = new CurrentUserElement()
                {
                    Login = "displayName",
                    Slug = "userSlug"
                }
            });

            loginFormViewModel = new LoginFormViewModel(authenticator);
            
            mediaBrowserViewModel = new MediaBrowserViewModel(mediaLibraryBrowser, 
                                                              new SettingsStub(),
                                                              loginFormViewModel,
                                                              new MixViewModelFactory(playbackController, mediaLibraryBrowser, loginFormViewModel.UserLoggedInObservable));
        }

        [TearDown]
        public void TearDown()
        {
            //            RxApp.DeferredScheduler = origSched;
        }

 

        [Test]
        public void TestDisplaysFeedMixes()
        {

            mediaBrowserViewModel.GetFeedMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            requestExecutor.Responses.Add(feedMixesResponse);
            browsingAvailableMixes
                .WithScenario("displaying feed mixes").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUser_LoggedIn, true)
                    .And(TheUserHasAFeedMixCalled_, mixName1, 0)
                    .And(TheUserHasAFeedMixCalled_, mixName2, 1)
                .When(TheUserIssuesACommandToDisplayFeedMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AMixCalled_WillBeShown, mixName1, 0)
                    .And(AMixCalled_WillBeShown, mixName2, 1)
                .ExecuteWithReport();
        }

        [Test]
        public void TestDisplaysLikedMixes()
        {           
            mediaBrowserViewModel.GetLikedMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            requestExecutor.Responses.Add(likedMixesResponse);
            browsingAvailableMixes
                .WithScenario("displaying liked mixes").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUser_LoggedIn, true)
                    .And(TheUserLikedAMixCalled_, mixName1, 0)
                    .And(TheUserLikedAMixCalled_, mixName2, 1)
                .When(TheUserIssuesACommandToDisplayLikedMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AMixCalled_WillBeShown, mixName1, 0)
                    .And(AMixCalled_WillBeShown, mixName2, 1)
                .ExecuteWithReport();
        }

        private void TheCommandIsProcessed([Silent] bool isCommandAsync)
        {

            if (isCommandAsync)
            {
                lockObject.WaitOne(20000);
            }
            //testScheduler.RunToMilliseconds(2000);
            Thread.Sleep(1000);
        }

        private void AMixCalled_WillBeShown(string mixName, [Silent] int index)
        {
            Assert.That(mediaBrowserViewModel.BrowsedMixes[index].Name, Is.EqualTo(mixName));
        }

        private void TheUserIssuesACommandToDisplayLikedMixes()
        {
            mediaBrowserViewModel.GetLikedMixes.Execute(null);
        }

        private void TheUserIssuesACommandToDisplayFeedMixes()
        {
            mediaBrowserViewModel.GetLikedMixes.Execute(null);
        }

        private void TheUserHasAFeedMixCalled_(string mixName, [Silent] int index)
        {
            feedMixesResponse.MixesElement[index].Name = mixName;
        }

        private void TheUserLikedAMixCalled_(string mixName, [Silent] int index)
        {
            likedMixesResponse.MixesElement[index].Name = mixName;
        }

        [Test]
        public void TestAllowsGettingLikedAndFeedMixesOnlyIfUserIsLoggedIn()
        {
            browsingAvailableMixes
                .WithScenario("enabling getting liked and feed mixes if the user is logged in").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUser_LoggedIn, false)
                .When(TheUserLogsIn)
                .Then(TheUser_BeAbleToGetLikedMixes, true)
                    .And(TheUser_BeAbleToGetFeedMixes, true)

                .WithScenario("disabling getting liked and feed mixes if the user is not logged in").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUser_LoggedIn, true)
                .When(TheUserLogsOut)
                .Then(TheUser_BeAbleToGetLikedMixes, false)
                    .And(TheUser_BeAbleToGetFeedMixes, false)
                .ExecuteWithReport();
        }

        private void TheUser_BeAbleToGetLikedMixes([BooleanParameterFormat("will", "will not")]bool canGetLikedMixes)
        {
            Assert.That(mediaBrowserViewModel.GetLikedMixes.CanExecute(null), Is.EqualTo(canGetLikedMixes));
        }

        private void TheUser_BeAbleToGetFeedMixes([BooleanParameterFormat("will", "will not")]bool canGetLikedMixes)
        {
            Assert.That(mediaBrowserViewModel.GetFeedMixes.CanExecute(null), Is.EqualTo(canGetLikedMixes));
        }

        private void TheUserLogsOut()
        {
            TheUser_LoggedIn(false);
        }

        private void TheUserLogsIn()
        {
            TheUser_LoggedIn(true);
        }

        private void TheUser_LoggedIn([BooleanParameterFormat("is", "is not")] bool isLoggedIn)
        {
            ((ReplaySubject<UserData>)loginFormViewModel.UserDataObservable).OnNext(isLoggedIn
                                                                 ? new UserData("", "", "", "")
                                                                 : UserData.NoUser);
        }

        [Test]
        public void TestDisplaysRecentlyMadeMixes()
        {

            mediaBrowserViewModel.GetRecentlyMadeMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            requestExecutor.Responses.Add(mixesResponse);
            browsingAvailableMixes
                .WithScenario("displaying liked mixes").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(AnyConditions)
                .When(TheUserIssuesACommandToDisplayRecentlyMadeMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOfMixesWillBeShown)
                .ExecuteWithReport();
        }

        [Test]
        public void TestDisplaysHotMixes()
        {

            mediaBrowserViewModel.GetHotMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            requestExecutor.Responses.Add(mixesResponse);
            browsingAvailableMixes
                .WithScenario("displaying liked mixes").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(AnyConditions)
                .When(TheUserIssuesACommandToDisplayHotMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOfMixesWillBeShown)
                .ExecuteWithReport();
        }

        [Test]
        public void TestDisplaysPopularMixes()
        {

            mediaBrowserViewModel.GetPopularMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            requestExecutor.Responses.Add(mixesResponse);
            browsingAvailableMixes
                .WithScenario("displaying popular mixes").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(AnyConditions)   
                .When(TheUserIssuesACommandToDisplayPopularMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOfMixesWillBeShown)
                .ExecuteWithReport();
        }

        [Test]
        public void TestDisplaysFilteredMixesByTagsAndKeywords()
        {
            mediaBrowserViewModel.GetPopularMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);
            browsingAvailableMixes
                .WithScenario("displaying popular mixes filtered by tags and keywords").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheMixFilterIs_, "[jazz][hiphop][cover] summer hit")
                .When(TheUserIssuesACommandToDisplayPopularMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOfMixesWillBeShown)
                    .And(TheMixBrowsingMessageShouldBe_, "Displaying 3 of Popular mixes, tagged: jazz hiphop cover, including \"summer hit\"")

                .WithScenario("displaying popular mixes filtered by keywords").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheMixFilterIs_, "summer hit")
                .When(TheUserIssuesACommandToDisplayPopularMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOfMixesWillBeShown)
                    .And(TheMixBrowsingMessageShouldBe_, "Displaying 3 of Popular mixes, including \"summer hit\"")

                .WithScenario("displaying popular mixes filtered by tags").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheMixFilterIs_, "[jazz][hiphop][cover]")
                .When(TheUserIssuesACommandToDisplayPopularMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOfMixesWillBeShown)
                    .And(TheMixBrowsingMessageShouldBe_, "Displaying 3 of Popular mixes, tagged: jazz hiphop cover")
                
                .ExecuteWithReport();
        }

        private void TheMixFilterIs_(string filter)
        {
            mediaBrowserViewModel.Filter = filter;
        }

        private void TheMixBrowsingMessageShouldBe_(string browsingMessage)
        {
            Assert.That(mediaBrowserViewModel.MixBrowsingMessage, Is.EqualTo(browsingMessage));
        }

        private void AListOfMixesWillBeShown()
        {
            Assert.That(mediaBrowserViewModel.BrowsedMixes.Count, Is.EqualTo(mixesResponse.MixesElement.Length));
        }

        private void TheUserIssuesACommandToDisplayRecentlyMadeMixes()
        {
            mediaBrowserViewModel.GetRecentlyMadeMixes.Execute(null);
        }

        private void TheUserIssuesACommandToDisplayHotMixes()
        {
            mediaBrowserViewModel.GetHotMixes.Execute(null);
        }

        private void TheUserIssuesACommandToDisplayPopularMixes()
        {
            mediaBrowserViewModel.GetPopularMixes.Execute(null);
        }

        private void AnyConditions()
        {
        }

        [Test]
        public void TestGetsMoreThanOnePageOfMixes()
        {
            mediaBrowserViewModel.GetHotMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            mediaBrowserViewModel.GetMoreMixes.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());

            requestExecutor.Responses.Add(mixesResponse);
            requestExecutor.Responses.Add(mixesResponse);

            browsingAvailableMixes
                .WithScenario("getting more then one 'page' of mixes ").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheApplicationLoaded_HotMixes, mixesResponse.MixesElement.Length)
                .When(TheUserIssuesACommandToDisplayMoreMixes)
                    .And(TheCommandIsProcessed, true)
                .Then(AListOf_MixesWillBeShown, 2 * mixesResponse.MixesElement.Length)
                .ExecuteWithReport();
        }

        private void AListOf_MixesWillBeShown(int totalNumberOfMixes)
        {
            Assert.That(mediaBrowserViewModel.BrowsedMixes.Count, Is.EqualTo(totalNumberOfMixes));
        }

        private void TheUserIssuesACommandToDisplayMoreMixes()
        {
            mediaBrowserViewModel.GetMoreMixes.Execute(null);
        }

        private void TheApplicationLoaded_HotMixes(int obj)
        {
            TheUserIssuesACommandToDisplayHotMixes();
            TheCommandIsProcessed(true);
        }
    }
}