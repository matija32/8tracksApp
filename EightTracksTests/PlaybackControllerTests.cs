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

namespace EightTracksTests
{
    [TestFixture]
    public class PlaybackControllerTests
    {
        private Feature playingAMix = Features.PlayingAMix;
        private Feature controllingPlayback = Features.ControllingPlayback;

        private Feature continuousAudioPlayback = Features.ContinuousAudioPlayback;
        private IScheduler origSched;
        
        private const string audioPlaybackTag = Tags.AudioPlayback;
        private const string domainTag = Tags.Domain;

        private IPlaybackController playbackController;
        private AudioPlayerStub audioPlayer;
        private RequestExecutorStub requestExecutor;

        private Mix currentMix;
        private Mix aMix;

        private IResponse playSongResponse = new PlaySongResponse() { SetElement = new SetElement() { TrackElement = new TrackElement() { Url = "url1" } } };
        private IResponse newPlayTokenResponse = new NewPlayTokenResponse() {PlayToken = "qwerty"};
        

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {

        }

        [SetUp]
        public void SetUp()
        {

            origSched = RxApp.DeferredScheduler;
            RxApp.DeferredScheduler = new TestScheduler();

            audioPlayer = new AudioPlayerStub();

            aMix = new Mix(new MixElement(), 0);

            requestExecutor = new RequestExecutorStub();
            playbackController = new PlaybackController(audioPlayer, requestExecutor);
            playbackController.CurrentMixObservable.Subscribe(mix => currentMix = mix);

        }

        [TearDown]
        public void TearDown()
        {
            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestPlaysAMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            playingAMix
                .WithScenario("playing a mix").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(ACertainMixToBePlayed)
                .When(TheApplicationIssuesACommandToPlayTheMix)
                .Then(TheControllerExecutesA_, typeof(PlayMixRequest), 1)
                    .And(TheControllerPlaysTheSongNumber_OfTheMix, 1)
                    .And(TheCurrentMixContains_Song, 1)
                .ExecuteWithReport();
        }

        private void TheControllerExecutesA_([TypeParameterFormat] Type requestType, [Silent] int requestIndex)
        {
            Assert.That(requestExecutor.Requests[requestIndex], Is.TypeOf(requestType));
        }

        [Test]
        public void TestPlaysNextSongInAMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(playSongResponse);
                        
            controllingPlayback
                .WithScenario("skipping to the next song in the current mix").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheSongBeingPlayedIsNumber_OfTheMix, 1)
                .When(TheApplicationIssuesACommandToGoToNextTrack)
                .Then(TheControllerExecutesA_, typeof(NextSongRequest), 2)
                    .And(TheControllerPlaysTheSongNumber_OfTheMix, 2)
                    .And(TheCurrentMixContains_Song, 2)
                    .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToGoToNextTrack()
        {
            playbackController.NextSong(false);
        }

        private void TheCurrentMixContains_Song(int numberOfSongs)
        {
            Assert.That(currentMix.Tracks.Count, Is.EqualTo(numberOfSongs));
        }

        private void TheApplicationIssuesACommandToPlayTheMix()
        {
            playbackController.Play(currentMix);
        }

        private void ACertainMixToBePlayed()
        {
            currentMix = aMix;
        }

        [Test]
        public void TestAutomaticallyPlaysNextSongOfATrack()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(playSongResponse);
                        
            continuousAudioPlayback
                .WithScenario("going to the next song upon finishing playing a song").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheSongBeingPlayedIsNumber_OfTheMix, 1)
                .When(TheSongFinishes)
                .Then(TheControllerExecutesA_, typeof(NextSongRequest), 2)
                    .And(TheControllerPlaysTheSongNumber_OfTheMix, 2)
                    .And(TheCurrentMixContains_Song, 2)
                    .ExecuteWithReport();
        }

        private void TheControllerPlaysTheSongNumber_OfTheMix(int index)
        {
            Assert.That(currentMix.CurrentTrackIndex, Is.EqualTo(index-1));
        }

        private void TheSongFinishes()
        {
            audioPlayer.InvokeEndOfStreamReachedEvent();
        }

        private void TheSongBeingPlayedIsNumber_OfTheMix(int index)
        {
            currentMix.CurrentTrackIndex = index-1;
        }

        private void AMixIsBeingPlayed()
        {
            currentMix = new Mix(new MixElement(), 0);
            playbackController.Play(currentMix);
        }

        [Test]
        public void TestPlaysTheNextMixInTheSet()
        {
            int mixId = 5060;

            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(new NextMixResponse() { NextMix = new MixElement() { Id = mixId } });
            requestExecutor.Responses.Add(playSongResponse);
            
            continuousAudioPlayback
                .WithScenario("going to the next mix upon finishing playing a mix").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                .When(TheApplicationIssuesACommandToGetASimilarMix)
                .Then(TheControllerExecutesA_, typeof(NextMixRequest), 2)
                    .And(TheControllerPlaysTheSongNumber_OfTheMix, 1)
                    .And(TheCurrentMixContains_Song, 1)
                    .And(TheCurrentMixHasId_, mixId)
                .ExecuteWithReport();
        }

        [Test]
        public void TestAddsNewTracksToTheMix()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            playingAMix
                .WithScenario("adding new tracks to the mix").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                .When(TheApplicationIssuesACommandToPlayTheSameMix)
                .Then(TheControllerShouldNotSendAdditionalRequests)
                .ExecuteWithReport();
        }

        private void TheControllerShouldNotSendAdditionalRequests()
        {
            Assert.That(requestExecutor.Requests.Count, Is.EqualTo(requestExecutor.Responses.Count));
        }

        private void TheApplicationIssuesACommandToPlayTheSameMix()
        {
            TheApplicationIssuesACommandToPlayTheMix();
        }

        [Test]
        public void TestAutomaticallyGoesToTheNextMix()
        {
            int mixId1 = 5061;
            int mixId2 = 5062;

            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(new NextMixResponse() { NextMix = new MixElement() { Id = mixId1 } });
            requestExecutor.Responses.Add(playSongResponse);

            //token is already there because of the previous scenario
            requestExecutor.Responses.Add(playSongResponse);
            requestExecutor.Responses.Add(new NextMixResponse() { NextMix = new MixElement() { Id = mixId2 } });
            requestExecutor.Responses.Add(playSongResponse);

            continuousAudioPlayback
                .WithScenario("going to the next mix upon finishing playing a mix").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheSongIsTheLastOneOfTheMix)
                .When(TheSongFinishes)
                .Then(TheControllerExecutesA_, typeof(NextMixRequest), 2)
                    .And(TheControllerPlaysTheSongNumber_OfTheMix, 1)
                    .And(TheCurrentMixContains_Song, 1)
                    .And(TheCurrentMixHasId_, mixId1)

                .WithScenario("going to the next mix when trying to skip over the last song of the mix").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheSongIsTheLastOneOfTheMix)
                .When(TheApplicationIssuesACommandToGoToNextTrack)
                .Then(TheControllerExecutesA_, typeof(NextMixRequest), 2)
                    .And(TheControllerPlaysTheSongNumber_OfTheMix, 1)
                    .And(TheCurrentMixContains_Song, 1)
                    .And(TheCurrentMixHasId_, mixId2)

                .ExecuteWithReport();
        }

        private void TheSongIsTheLastOneOfTheMix()
        {
            currentMix.GetCurrentTrack().IsLast = true;
        }

        private void TheCurrentMixHasId_(int mixId)
        {
            Assert.That(currentMix.MixId, Is.EqualTo(mixId));
        }

        private void TheApplicationIssuesACommandToGetASimilarMix()
        {
            playbackController.NextMix();
        }

        [Test]
        public void TestPausesThePlayback()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("pausing the playback").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                .When(TheApplicationIssuesACommandToPauseTheMix)
                .Then(TheAudioPlayerShouldPauseThePlayback)
                .ExecuteWithReport();

        }

        private void TheAudioPlayerShouldPauseThePlayback()
        {
            Assert.That(audioPlayer.IsPauseIssued, Is.True);
        }

        private void TheApplicationIssuesACommandToPauseTheMix()
        {
            playbackController.Pause();
        }

        [Test]
        public void TestStopsThePlayback()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("stopping the playback").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                .When(TheApplicationIssuesACommandToStopTheMix)
                .Then(TheAudioPlayerShouldStopThePlayback)
                .ExecuteWithReport();
        }

        private void TheAudioPlayerShouldStopThePlayback()
        {
            Assert.That(audioPlayer.IsStopIssued, Is.True);
        }

        private void TheApplicationIssuesACommandToStopTheMix()
        {
            playbackController.Stop();
        }

        [Test]
        public void TestContinuesThePlayback()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

//            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("continues the playback if it's paused").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(AMixIsPaused)
                .When(TheApplicationIssuesACommandToContinueTheMix)
                .Then(TheAudioPlayerShouldUnpauseThePlayback)

                .WithScenario("continues the playback if it's stopped").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(AMixIsStopped)
                .When(TheApplicationIssuesACommandToContinueTheMix)
                .Then(TheAudioPlayerShouldPlayThePlayback)
                            
                .ExecuteWithReport();

        }

        private void AMixIsStopped()
        {
            playbackController.Stop();
        }

        private void TheAudioPlayerShouldPlayThePlayback()
        {
            Assert.That(audioPlayer.IsPlayIssued, Is.True);
        }

        private void TheAudioPlayerShouldUnpauseThePlayback()
        {
            Assert.That(audioPlayer.IsPauseIssued, Is.True);
        }

        private void TheApplicationIssuesACommandToContinueTheMix()
        {
            playbackController.Continue();
        }

        private void AMixIsPaused()
        {
            playbackController.Pause();
        }

        [Test]
        public void TestChangesTheVolume()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

            controllingPlayback
                .WithScenario("changing the volume").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheVolumeIsSetAt_Percent, 50)
                .When(TheApplicationIssuesACommandToChangeTheVolumeTo_Percent, 80)
                .Then(TheVolumeShouldBecome_Percent, 80)
                .ExecuteWithReport();
        }

        private void TheVolumeShouldBecome_Percent(int volume)
        {
            Assert.That(audioPlayer.Volume, Is.EqualTo(volume));
        }

        private void TheApplicationIssuesACommandToChangeTheVolumeTo_Percent(int volume)
        {
            playbackController.SetVolume(volume);
        }

        private void TheVolumeIsSetAt_Percent(int volume)
        {
            playbackController.SkipTo(volume);
        }

        [Test]
        public void TestSkipsToPosition()
        {
            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);

//            requestExecutor.Responses.Add(newPlayTokenResponse);
            requestExecutor.Responses.Add(playSongResponse);
            
            controllingPlayback
                .WithScenario("skipping to a position").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheTrackDurationIs_Seconds, 300)
                    .And(ThePositionAt_Seconds, 100)
                .When(TheApplicationIssuesACommandToSkipTo_Second, 200)
                .Then(TheCurrentPositionOfThePlaybackShouldBe_, 200)

                .WithScenario("Not skipping to an invalid positon").Tag(audioPlaybackTag).Tag(domainTag)
                .Given(AMixIsBeingPlayed)
                    .And(TheTrackDurationIs_Seconds, 300)
                    .And(ThePositionAt_Seconds, 100)
                .When(TheApplicationIssuesACommandToSkipTo_Second, 400)
                .Then(TheCurrentPositionOfThePlaybackShouldBe_, 400)

                .ExecuteWithReport();
        }

        private void TheCurrentPositionOfThePlaybackShouldBe_(int position)
        {
            Assert.That(audioPlayer.CurrentPosition, Is.EqualTo(position));
        }

        private void TheApplicationIssuesACommandToSkipTo_Second(int newPosition)
        {
            playbackController.SkipTo(newPosition);
        }

        private void ThePositionAt_Seconds(int position)
        {
            audioPlayer.CurrentPosition = position;
        }

        private void TheTrackDurationIs_Seconds(int duration)
        {
            audioPlayer.CurrentTrackDuration = duration;
        }
    }
}