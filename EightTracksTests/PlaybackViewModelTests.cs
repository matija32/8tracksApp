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
using EightTracksPlayer.Media;
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
    public class PlaybackViewModelTests
    {
        private const string applicationTag = Tags.Application;
        private const string audioPlaybackTag = Tags.AudioPlayback;

        private Feature playingAMix = Features.PlayingAMix;
        private Feature controllingPlayback = Features.ContinuousAudioPlayback;
//        private TestScheduler testScheduler;
//        private IScheduler origSched;
        private ManualResetEvent lockObject;
        private RequestExecutorStub requestExecutor;
        private PlaybackViewModel playbackViewModel;
        private AudioPlayerStub audioPlayerStub;
        private IMediaLibraryBrowser mediaLibraryBrowser;
        private Authenticator authenticator;
        private PlaybackController playbackController;
        private string mixName1 = "mixName1";
        private string coverUri1 = "coverUri1";

        private PlaySongResponse playSongResponse;
        private NewPlayTokenResponse newPlayTokenResponse;
        private LoginFormViewModel loginFormViewModel;

        [SetUp]
        public void SetUp()
        {
//            testScheduler = new TestScheduler();
//            origSched = RxApp.DeferredScheduler;
//            RxApp.DeferredScheduler = testScheduler;
            playSongResponse = new PlaySongResponse() { SetElement = new SetElement() { TrackElement = new TrackElement() { Url = "some url" } } };
            newPlayTokenResponse = new NewPlayTokenResponse() {PlayToken = "a-token"};
            
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

            playbackViewModel = new PlaybackViewModel(playbackController, new SettingsStub(),
                new MixViewModelFactory(playbackController, mediaLibraryBrowser, loginFormViewModel.UserLoggedInObservable));


        }

        [TearDown]
        public void TearDown()
        {
//            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestDisplaysInfoAboutTheCurrentMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            playingAMix
                .WithScenario("displaying information about the current mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(NoMixesArePlayed)
                .When(AMixStartsPlaying)
                    .And(ABitOfTimePasses)
                .Then(TheCoverImageOfTheMixIsShown)
                    .And(TheNameOfTheMixIsShown)
                .ExecuteWithReport();
        }

        [Test]
        public void TestUpdatesControlAvailability()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("updates the availability of the controls when no mix is played").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(NoMixesArePlayed)
                .When(TheUserIsIdle)
                .Then(TheUser_BeAbleToStopTheMix, false)
                    .And(TheUser_BeAbleToPauseTheMix, false)
                    .And(TheUser_BeAbleToGoToNextMix, false)
                    .And(TheUser_BeAbleToGoToNextSong, false)
                    .And(TheUser_BeAbleToContinueTheMix, false)
                    .And(TheUser_BeAbleToRecordTheMix, false)

                .WithScenario("updates the availability of the controls when the playback is ongoing").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixStartsPlaying)
                .When(TheUserIsIdle)
                .Then(TheUser_BeAbleToStopTheMix, true)
                    .And(TheUser_BeAbleToPauseTheMix, true)
                    .And(TheUser_BeAbleToGoToNextMix, true)
                    .And(TheUser_BeAbleToGoToNextSong, true)
                    .And(TheUser_BeAbleToContinueTheMix, false)
                    .And(TheUser_BeAbleToRecordTheMix, true)

                .WithScenario("updates the availability of the controls when the playback is paused").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPaused)
                .When(TheUserIsIdle)
                .Then(TheUser_BeAbleToStopTheMix, true)
                    .And(TheUser_BeAbleToPauseTheMix, false)
                    .And(TheUser_BeAbleToGoToNextMix, true)
                    .And(TheUser_BeAbleToGoToNextSong, true)
                    .And(TheUser_BeAbleToContinueTheMix, true)
                    .And(TheUser_BeAbleToRecordTheMix, true)

                .WithScenario("updates the availability of the controls when the playback is stopped").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsStopped)
                .When(TheUserIsIdle)
                .Then(TheUser_BeAbleToStopTheMix, false)
                    .And(TheUser_BeAbleToPauseTheMix, false)
                    .And(TheUser_BeAbleToGoToNextMix, true)
                    .And(TheUser_BeAbleToGoToNextSong, true)
                    .And(TheUser_BeAbleToContinueTheMix, true)
                    .And(TheUser_BeAbleToRecordTheMix, true)


                .ExecuteWithReport();
        }

        private void TheUser_BeAbleToRecordTheMix([BooleanParameterFormat("should", "shouldn't")]bool isEnabled)
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.Download.CanExecute(null), Is.EqualTo(isEnabled));
        }

        private void TheUser_BeAbleToStopTheMix([BooleanParameterFormat("should", "shouldn't")]bool isEnabled)
        {
            Assert.That(playbackViewModel.Stop.CanExecute(null), Is.EqualTo(isEnabled));
        }

        private void TheUser_BeAbleToPauseTheMix([BooleanParameterFormat("should", "shouldn't")]bool isEnabled)
        {
            Assert.That(playbackViewModel.Pause.CanExecute(null), Is.EqualTo(isEnabled));
        }

        private void TheUser_BeAbleToContinueTheMix([BooleanParameterFormat("should", "shouldn't")]bool isEnabled)
        {
            Assert.That(playbackViewModel.Continue.CanExecute(null), Is.EqualTo(isEnabled));
        }

        private void TheUser_BeAbleToGoToNextMix([BooleanParameterFormat("should", "shouldn't")]bool isEnabled)
        {
            Assert.That(playbackViewModel.NextMix.CanExecute(null), Is.EqualTo(isEnabled));
        }

        private void TheUser_BeAbleToGoToNextSong([BooleanParameterFormat("should", "shouldn't")]bool isEnabled)
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.NextSong.CanExecute(null), Is.EqualTo(isEnabled));
        }

        private void TheUserIsIdle()
        {
            
        }

        [Test]
        public void TestDisplaysNoInfoInCaseOfInvalidMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            playingAMix
                .WithScenario("not displaying information in case of an invalid mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixDoesNotHaveAOpeningSongUri)
                    .And(NoMixesArePlayed)
                .When(AMixStartsPlaying)
                    .And(ABitOfTimePasses)
                .Then(StillNoMixesShouldBePlayed)
                .ExecuteWithReport();
        }

        [Test]
        public void TestPausesTheMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("pausing the current mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixStartsPlaying)
                .When(TheUserIssuesACommandToPauseTheMix)
                    .And(TheCommandIsProcessed, false)
                .Then(ThePlaybackShouldBePaused)
                .ExecuteWithReport();
        }

        private void ThePlaybackShouldBePaused()
        {
            Assert.That(audioPlayerStub.IsPauseIssued, Is.True);
        }

        private void TheUserIssuesACommandToPauseTheMix()
        {
            playbackViewModel.Pause.Execute(null);
        }

        private void AMixIsPlayed()
        {
            playbackController.Play(new Mix(new MixElement(), 0) { CoverUri = coverUri1, Name = mixName1 });
        }

        [Test]
        public void TestUnpausesTheMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            
            controllingPlayback
                .WithScenario("unpausing the current mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixIsPaused)
                .When(TheUserIssuesACommandToContinueTheMix)
                    .And(TheCommandIsProcessed, false)
                .Then(ThePlaybackShouldBeUnpaused)

                .ExecuteWithReport();
        }

        private void TheSecondTrackShouldBePlayed()
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.CurrentTrackIndex, Is.EqualTo(1));
        }

        private void TheSecondTrackIsBeingPlayed()
        {
            playbackController.NextSong(false);
        }

        private void AMixIsStopped()
        {
            playbackController.Stop();
        }

        private void ThePlaybackShouldBeUnpaused()
        {
            Assert.That(audioPlayerStub.IsPlayIssued, Is.True);
        }

        private void TheUserIssuesACommandToContinueTheMix()
        {
            playbackViewModel.Continue.Execute(null);
        }

        private void AMixIsPaused()
        {
            playbackController.Pause();
        }

        [Test]
        public void TestStopsTheMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("stopping the current mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixStartsPlaying)
                .When(TheUserIssuesACommandToStopTheMix)
                    .And(TheCommandIsProcessed, false)
                .Then(ThePlaybackShouldBeStopped)
                .ExecuteWithReport();
        }

        private void ThePlaybackShouldBeStopped()
        {
            Assert.That(audioPlayerStub.IsStopIssued, Is.True);
        }

        private void ThePlaybackShouldBeOpened()
        {
            Assert.That(audioPlayerStub.IsOpenIssued, Is.True);
        }

        private void TheUserIssuesACommandToStopTheMix()
        {
            playbackViewModel.Stop.Execute(null);
        }

        [Test]
        public void TestPlaysAStoppedMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(new PlaySongResponse() { SetElement = new SetElement() { TrackElement = new TrackElement() { Url = "some-url-1" } } });
            requestExecutor.Responses.Add(new PlaySongResponse() { SetElement = new SetElement() { TrackElement = new TrackElement() { Url = "some-url-2" } } });
            

            controllingPlayback
                .WithScenario("playing a stopped current mix").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(TheSecondTrackIsBeingPlayed)
                    .And(AMixIsStopped)
                .When(TheUserIssuesACommandToContinueTheMix)
                    .And(TheCommandIsProcessed, false)
                .Then(ThePlaybackShouldBeOpened)
                    .And(TheSecondTrackShouldBePlayed)
                .ExecuteWithReport();

        }

        [Test]
        public void TestChangesTheVolume()
        {
//            requestExecutor.Responses.Add(newPlayTokenResponse);
//            requestExecutor.Responses.Add(playSongResponse);
//            
            controllingPlayback
                .WithScenario("changing the volume").Tag(applicationTag)
                .Given(AnyConditions)
                .When(TheUserIssuesACommandToChangeTheVolumeTo_Percent, 80)
                    .And(ABitOfTimePasses)
                .Then(TheAudioVolumeShouldBeAt_Percent, 80)
                .ExecuteWithReport();
        }

        private void TheAudioVolumeShouldBeAt_Percent(int volume)
        {
            Assert.That(audioPlayerStub.Volume, Is.EqualTo(volume));
        }

        private void TheUserIssuesACommandToChangeTheVolumeTo_Percent(int volume)
        {
            playbackViewModel.Volume = 80;
        }

        private void AnyConditions()
        {
            
        }

        [Test]
        public void TestChangesTheTrackPosition()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("changing the position").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixStartsPlaying)
                    .And(TheCurrentTrackIs_SecondsLong, 240)
                .When(TheUserIssuesACommandToChangeThePositionTo_Seconds, 30)
                    .And(ABitOfTimePasses)
                .Then(TheAudioPlaybackShouldBeAt_Seconds, 30)
                .ExecuteWithReport();
        }

        private void TheAudioPlaybackShouldBeAt_Seconds(int newPosition)
        {
            Assert.That(playbackViewModel.CurrentPosition, Is.EqualTo(newPosition));
        }

        private void TheCurrentTrackIs_SecondsLong(int duration)
        {
            audioPlayerStub.CurrentTrackDuration = duration;
        }

        private void TheUserIssuesACommandToChangeThePositionTo_Seconds(int newPosition)
        {
            playbackViewModel.CurrentPosition = newPosition;
        }

        private void StillNoMixesShouldBePlayed()
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.Model, Is.EqualTo(Mix.NoMixAvailable));
        }

        private void AMixDoesNotHaveAOpeningSongUri()
        {
            playSongResponse.SetElement.TrackElement.Url = String.Empty;
        }

        private void ABitOfTimePasses()
        {
            Thread.Sleep(2000);
        }

        private void TheCoverImageOfTheMixIsShown()
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.CoverUri, Is.EqualTo(coverUri1));
        }

        private void TheNameOfTheMixIsShown()
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.Name, Is.EqualTo(mixName1));
        }

        private void AMixStartsPlaying()
        {
            playbackController.Play(new Mix(new MixElement(), 0){CoverUri = coverUri1, Name = mixName1});
        }

        private void NoMixesArePlayed()
        {
            
        }

        [Test]
        public void TestGoesToTheNextMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(new NextMixResponse() { NextMix = new MixElement() { Name = mixName1} });
            requestExecutor.Responses.Add(playSongResponse);
            playbackViewModel.NextMix.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());

            controllingPlayback
                .WithScenario("going to the next mix in the set").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixStartsPlaying)
                .When(TheUserIssuesACommandToGoToTheNextMixInTheSet)
                    .And(TheCommandIsProcessed, true)
                .Then(TheCurrentMixIsNamed_, mixName1);

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

        private void TheUserIssuesACommandToGoToTheNextMixInTheSet()
        {
            playbackViewModel.NextMix.Execute(null);
        }

        private void TheCurrentMixIsNamed_(string mixName)
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.Name, Is.EqualTo(mixName));
        }

        [Test]
        public void TestGoesToTheNextSong()
        {
            playbackViewModel.CurrentMixViewModel.NextSong.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());

            string trackPerformer = "some-artist";
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(new PlaySongResponse(){SetElement = new SetElement(){TrackElement = new TrackElement(){Url = trackPerformer}}});

            controllingPlayback
                .WithScenario("going to the next mix in the set").Tag(applicationTag).Tag(audioPlaybackTag)
                .Given(AMixIsPlayed)
                    .And(AMixStartsPlaying)
                .When(TheUserIssuesACommandToGoToTheNextSongInTheMix)
                    .And(TheCommandIsProcessed, true)
                .Then(TheCurrentTrackPerformerIs_, trackPerformer);

        }

        private void TheUserIssuesACommandToGoToTheNextSongInTheMix()
        {
            playbackViewModel.CurrentMixViewModel.NextSong.Execute(null);
        }

        private void TheCurrentTrackPerformerIs_(string trackUrl)
        {
            Assert.That(playbackViewModel.CurrentMixViewModel.Tracks[playbackViewModel.CurrentMixViewModel.CurrentTrackIndex].Performer, Is.EqualTo(trackUrl));
        }
    }
}