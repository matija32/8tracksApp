using System;
using System.Collections.Generic;
using System.Concurrency;
using System.Linq;
using System.Threading;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils.Browsing;
using EightTracksPlayer.ViewModels;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using ReactiveUI;
using ReactiveUI.Xaml;
using ReactiveUI.Testing;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class MixViewModelTests
    {
        private const string mediaBrowsingTag = Tags.MediaLibraryBrowsing;
        private const string applicationTag = Tags.Application;
        private const string audioPlaybackTag = Tags.AudioPlayback;

        private Feature favoritingItemsInTheMediaLibrary = Features.FavoritingItemsInTheMediaLibrary;
        private Feature playingAMix = Features.PlayingAMix;
        private RequestExecutorStub requestExecutor;
        private AudioPlayerStub audioPlayer;
        private IScheduler origSched;
        private PlaybackController playbackController;
        private Mix aMix;
        private MixViewModel mixViewModel;
        private MediaLibraryBrowser mediaLibraryBrowser;
        private Subject<bool> userLoggedInSubject;

        private IResponse playSongResponse = new PlaySongResponse() { SetElement = new SetElement() { TrackElement = new TrackElement() { Url = "url1" } } };
        private IResponse newPlayTokenResponse = new NewPlayTokenResponse();
        private IResponse mixLikedResponse = new ToggleMixLikeResponse() { LoggedIn = true, Mix = new MixElement() { LikedByCurrentUser = true } };
        private IResponse mixUnlikedResponse = new ToggleMixLikeResponse() { LoggedIn = true, Mix = new MixElement() { LikedByCurrentUser = false } };
        private TestScheduler testScheduler;
        private ManualResetEvent lockObject;
        private Feature downloadingMediaContent;
        private FileSystemBrowserStub fileSystemBrowserStub;
        private LoginFormViewModel loginFormViewModel;


        [SetUp]
        public void SetUp()
        {
            fileSystemBrowserStub = new FileSystemBrowserStub();
            downloadingMediaContent = Features.DownloadingMediaContent;

            testScheduler = new TestScheduler();
            origSched = RxApp.DeferredScheduler;
            RxApp.DeferredScheduler = testScheduler;
            
            aMix = new Mix(new MixElement(), 0);
   
            lockObject = new ManualResetEvent(false);

            audioPlayer = new AudioPlayerStub();
            requestExecutor = new RequestExecutorStub();
            playbackController = new PlaybackController(audioPlayer, requestExecutor);
            mediaLibraryBrowser = new MediaLibraryBrowser(requestExecutor);
            userLoggedInSubject = new Subject<bool>();
            mixViewModel = new MixViewModel(aMix, playbackController, mediaLibraryBrowser, userLoggedInSubject);
            mixViewModel.FileSystemBrowser = fileSystemBrowserStub;
            mixViewModel.WebAccessProxy = new WebAccessProxyStub();
            lockObject.Reset();

        }

        [TearDown]
        public void TearDown()
        {
            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestPlayAMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            aMix.CurrentTrackIndexObservable.Where(x => x==0).Subscribe(_ => lockObject.Set());
            
            playingAMix
                .WithScenario("the user plays a mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AnyConditions)
                .When(TheUserIssuesACommandToPlayTheMix)
                    .And(TheCommandIsProcessed, false)
                .Then(TheCurrentTrackShouldBeNumber_, 1)
                    .And(TheAudioPlaybackShouldStart)
                .ExecuteWithReport();
        }

        [Test]
        public void TestPlaysTheNextSongInTheMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(playSongResponse);

            aMix.CurrentTrackIndexObservable.Where(x => x == 1).Subscribe(_ => lockObject.Set());
            
            playingAMix
                .WithScenario("playing the next song in the mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(TheMixIsPlaying)
                    .And(TheCurrentTrackIsNumber_, 1)
                .When(TheUserIssuesACommandToGoToNextTrack)
                    .And(TheCommandIsProcessed, true)
                .Then(TheCurrentTrackShouldBeNumber_, 2)
                    .And(TheAudioPlaybackShouldStart)
                .ExecuteWithReport();
        }

        private void TheMixIsPlaying()
        {
            playbackController.Play(aMix);
        }

        private void TheUserIssuesACommandToGoToNextTrack()
        {
            mixViewModel.NextSong.Execute(null);
        }

        private void TheCurrentTrackIsNumber_(int trackIndex)
        {
            aMix.CurrentTrackIndex = trackIndex-1;
        }

        private void TheAudioPlaybackShouldStart()
        {
            Assert.That(audioPlayer.IsOpenIssued, Is.EqualTo(true));
        }

        private void TheCurrentTrackShouldBeNumber_(int expectedTrackIndex)
        {
            Assert.That(mixViewModel.CurrentTrackIndex, Is.EqualTo(expectedTrackIndex-1));
        }

        private void TheUserIssuesACommandToPlayTheMix()
        {
            mixViewModel.Play.Execute(null);
        }

        private void AnyConditions()
        {
            
        }

        [Test]
        public void TestLikesAMix()
        {
            requestExecutor.Responses.Add(mixLikedResponse);
            aMix.LikedByCurrentUser = false;
            aMix.LikedByCurrentUserObservable.Where(x => x).Subscribe(_ => lockObject.Set());

            favoritingItemsInTheMediaLibrary
                .WithScenario("the user likes a mix").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUserIs_LoggedIn, true)
                    .And(TheMixIs_LikedByTheUser, false)
                .When(TheUserIssuesACommandToToggleLikeTheMix)
                    .And(TheCommandIsProcessed, true)
                .Then(TheMixNow_LikedByTheUser, true)
                .ExecuteWithReport();
        }

        private void TheCommandIsProcessed([Silent] bool isCommandAsync)
        {

            // wait for the command to finish, if it's async (if it's sync it's already executed).
            if (isCommandAsync) lockObject.WaitOne();
            Thread.Sleep(1000);

            // give a bit of time for the command results to be propagated upwards to the view-model
            testScheduler.RunToMilliseconds(2000);
            
            
        }

        private void TheUserIssuesACommandToToggleLikeTheMix()
        {   
            mixViewModel.ToggleLike.Execute(null);
        }

        private void TheMixNow_LikedByTheUser([BooleanParameterFormat("should be", "should not be")] bool isLiked)
        {
            Assert.That(mixViewModel.LikedByCurrentUser, Is.EqualTo(isLiked));
        }

        [Test]
        public void TestUnlikesAMix()
        {
            requestExecutor.Responses.Add(mixUnlikedResponse);
            aMix.LikedByCurrentUser = true;
            aMix.LikedByCurrentUserObservable.Where(x => !x).Subscribe(_ => lockObject.Set());

            favoritingItemsInTheMediaLibrary
                .WithScenario("the user likes a mix").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUserIs_LoggedIn, true)
                    .And(TheMixIs_LikedByTheUser, true)
                .When(TheUserIssuesACommandToToggleLikeTheMix)
                    .And(TheCommandIsProcessed, true)
                .Then(TheMixNow_LikedByTheUser, false)
                .ExecuteWithReport();
        }

        [Test]
        public void TestAllowsTogglingLikeOnlyIfUserIsLoggedIn()
        {
            favoritingItemsInTheMediaLibrary
                .WithScenario("disable toggling liking a mix if the user is not logged in").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUserIs_LoggedIn, true)
                .When(TheUserLogsOut)
                .Then(TheUserShould_BeAbleToToggleLikeMix, false)

                .WithScenario("enabling toggling liking a mix if the user is logged in").Tag(applicationTag).Tag(mediaBrowsingTag)
                .Given(TheUserIs_LoggedIn, false)
                .When(TheUserLogsIn)
                .Then(TheUserShould_BeAbleToToggleLikeMix, true)
                .ExecuteWithReport();
        }

        private void TheUserLogsIn()
        {
            userLoggedInSubject.OnNext(true);
        }

        private void TheUserShould_BeAbleToToggleLikeMix([BooleanParameterFormat("", "not")] bool ableToExecute)
        {
            Assert.That(mixViewModel.ToggleLike.CanExecute(null), Is.EqualTo(ableToExecute));
        }

        private void TheUserLogsOut()
        {
            userLoggedInSubject.OnNext(false);
        }

        private void TheMixIs_LikedByTheUser([BooleanParameterFormat("", "not")] bool isLiked)
        {
            aMix.LikedByCurrentUser = isLiked;
        }

        private void TheUserIs_LoggedIn([BooleanParameterFormat("", "not")] bool shouldBeLoggedIn)
        {
            userLoggedInSubject.OnNext(shouldBeLoggedIn);
        }

//        [Test]
//        public void TestDownloadsAllTracksInAMix()
//        {
//
//            string trackName1 = "trackName1";
//            string trackName2 = "trackName2";
//            string performer1 = "performer1";
//            string performer2 = "performer2";
//            string url1 = "domain.org/subdomain/some-track.mp3";
//            string url2 = "seconddomain.net/some-other-track.m4a";
//            string mixName = "mixNameX";
//            string destinationFolder = @"C:\mymixes\coolmixes";
//            string location1 = destinationFolder + "\\" + mixName + "\\" + trackName1 + " - " + performer1 + ".mp3";
//            string location2 = destinationFolder + "\\" + mixName + "\\" + trackName2 + " - " + performer2 + ".m4a";
//
//            
//
//            downloadingMediaContent
//                .WithScenario("downloading all tracks in a mix")
//                .Given(TheMixHasATrackNamed_MadeBy_WithUrl_, trackName1, performer1, url1)
//                    .And(TheMixHasATrackNamed_MadeBy_WithUrl_, trackName2, performer2, url2)
//                    .And(TheMixNameIs_, mixName)
//                .When(TheUserIssuesACommandToDownloadTheMixTo_, destinationFolder)
//                    .And(TheCommandIsProcessed, false)
//                    .And(TracksAreDownloaded)
//                .Then(ATrackShouldBe_To_, FileLocation.Downloaded, location1, 0)
//                    .And(ATrackShouldBe_To_, FileLocation.Downloaded, location2, 1)
//                .ExecuteWithReport();
//
//        }
//
//        private void TracksAreDownloaded()
//        {
//            testScheduler.RunToMilliseconds(4000);
//            Thread.Sleep(4000);
//            testScheduler.RunToMilliseconds(4000);
//
//            mixViewModel.Tracks.ToObservable().Subscribe(x =>
//                                                             {
//                                                                 ((WebAccessProxyStub) x.WebAccessProxy).PushNewFileLocation(FileLocation.Downloaded);
//                                                             });
//            testScheduler.RunToMilliseconds(4000);
//            Thread.Sleep(4000);
//            testScheduler.RunToMilliseconds(4000);
//        }
//
//        private void ATrackShouldBe_To_(FileLocation fileLocation, string filepath, [Silent]int trackIndex)
//        {
//            testScheduler.RunToMilliseconds(4000);
//            Thread.Sleep(4000);
//            testScheduler.RunToMilliseconds(4000);
//            
//            Assert.That(((FileSystemBrowserStub)mixViewModel.Tracks[trackIndex].FileSystemBrowser).DestinationFilename, Is.EqualTo(filepath));
//            Assert.That(mixViewModel.Tracks[trackIndex].TrackLocation, Is.EqualTo(fileLocation.ToString()));
//        }
//
//        private void TheUserIssuesACommandToDownloadTheMixTo_(string destinationFolder)
//        {
//            fileSystemBrowserStub.DestinationFolder = destinationFolder;
//            mixViewModel.Download.Execute(null);
//        }
//
//        private void TheMixNameIs_(string name)
//        {
//            aMix.Name = name;
//        }
//
//        private void TheMixHasATrackNamed_MadeBy_WithUrl_(string trackName, string performer, string url)
//        {
//            ManualResetEvent tempLock = new ManualResetEvent(false);
//            mixViewModel.Tracks.CollectionCountChanged.Subscribe(_ => tempLock.Set());
//            aMix.AddTrack(new Track(new TrackElement(){Url = url, Name = trackName, Performer = performer}, false));
//            tempLock.WaitOne();
//            tempLock.Reset();
//            testScheduler.RunToMilliseconds(2000);
//            Thread.Sleep(2000);
//            mixViewModel.Tracks[mixViewModel.Tracks.Count-1].FileSystemBrowser = mixViewModel.FileSystemBrowser;
//            mixViewModel.Tracks[mixViewModel.Tracks.Count-1].WebAccessProxy = new WebAccessProxyStub();
//
//        }
    }
}