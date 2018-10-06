using System;
using System.Concurrency;
using System.Runtime.InteropServices;
using EightTracksPlayer;
using EightTracksPlayer.Media;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NMock2;
using NUnit.Framework;
using QuartzTypeLib;
using ReactiveUI;
using StoryQ;
using StoryQ.Formatting.Parameters;
using Is = NUnit.Framework.Is;

namespace EightTracksTests
{
    [TestFixture]
    public class StatefullAudioPlayerTests
    {
        private Feature playingAMix = Features.PlayingAMix;
        private Feature controllingPlayback = Features.ControllingPlayback;
        private StatefullAudioPlayer statefullAudioPlayer;
        private const string audioPlaybackTag = Tags.AudioPlayback;
        private const string infrastructureTag = Tags.Infrastructure;
        private string properAudioFilePath;
        private string audioFilePath;
        private IScheduler origSched;

        [SetUp]
        public void SetUp()
        {

            origSched = RxApp.DeferredScheduler;
            RxApp.DeferredScheduler = new TestScheduler();
            
            statefullAudioPlayer = new StatefullAudioPlayer(new AudioPlayerStub());
            properAudioFilePath = @"..\..\..\..\Data\Audio files\XAqxNO.48k.v3.m4a";
        }

        [TearDown]
        public void TearDown()
        {
            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestPlaysMusicFromAnAudioFile()
        {
            playingAMix
                .WithScenario("Playing audio").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(AProperPathToAnAudioFile)
                .When(TheApplicationIssuesACommandToOpenAFile)
                .Then(TheStateOfThePlayerShouldBe_, MediaStatus.Running)
                .ExecuteWithReport();

        }

        [Test]
        public void TestStopsPlayback()
        {
            controllingPlayback
                .WithScenario("Stopping the playback").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(ThereIsAnOngoingPlayback)
                .When(TheApplicationIssuesACommandToStopThePlayback)
                .Then(TheStateOfThePlayerShouldBe_, MediaStatus.Stopped)
                    .And(TheCurrentPositionOfThePlaybackShouldBe_, 0)
                .ExecuteWithReport();
        }

        [Test]
        public void TestPausesPlayback()
        {
            controllingPlayback
                .WithScenario("Pausing the playback").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(ThereIsAnOngoingPlayback)
                .When(TheApplicationIssuesACommandToPauseThePlayback)
                .Then(TheStateOfThePlayerShouldBe_, MediaStatus.Paused)
                .ExecuteWithReport();
        }


        [Test]
        public void TestUnpausesPlayback()
        {
            controllingPlayback
                .WithScenario("Unpausing the playback").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(ThereIsAnOngoingPlayback)
                    .And(ThePlaybackHasBeenPaused)
                .When(TheApplicationIssuesACommandToUnpauseThePlayback)
                .Then(TheStateOfThePlayerShouldBe_, MediaStatus.Running)
                .ExecuteWithReport();
        }


        [Test]
        public void TestChangesTheVolume()
        {
            controllingPlayback
                .WithScenario("Changing the volume").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(ThereIsAnOngoingPlayback)
                    .And(TheVolumeIsAt_Percent, 50)
                .When(TheApplicationIssuesACommandToChangeTheVolumeTo_Percent, 80)
                .Then(TheVolumeShouldBecome_Percent, 80)
                .ExecuteWithReport();
        }

        [Test]
        public void TestSkipsToPosition()
        {
            controllingPlayback
                .WithScenario("Skipping to a position").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(ThereIsAnOngoingPlayback)
                    .And(TheTrackDurationIs_Seconds, 300)
                    .And(ThePositionAt_Seconds, 100)
                .When(TheApplicationIssuesACommandToSkipTo_Second, 200)
                .Then(TheCurrentPositionOfThePlaybackShouldBe_, 200)

                .WithScenario("Not skipping to an invalid positon").Tag(audioPlaybackTag).Tag(infrastructureTag)
                .Given(ThereIsAnOngoingPlayback)
                    .And(TheTrackDurationIs_Seconds, 300)
                    .And(ThePositionAt_Seconds, 100)
                .When(TheApplicationIssuesACommandToSkipTo_Second, 400)
                .Then(TheCurrentPositionOfThePlaybackShouldBe_, 400)

                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToSkipTo_Second(int newPosition)
        {
            statefullAudioPlayer.SkipTo(newPosition);
        }

        private void TheTrackDurationIs_Seconds(int duration)
        {
            ((AudioPlayerStub) (statefullAudioPlayer.BaseAudioPlayer)).CurrentTrackDuration = duration;
        }

        private void ThePositionAt_Seconds(int position)
        {
            statefullAudioPlayer.BaseAudioPlayer.SkipTo(position);
        }

        private void TheVolumeShouldBecome_Percent(int volume)
        {
            Assert.That(((AudioPlayerStub)statefullAudioPlayer.BaseAudioPlayer).Volume, Is.EqualTo(volume));
        }

        private void TheApplicationIssuesACommandToChangeTheVolumeTo_Percent(int volume)
        {
            statefullAudioPlayer.SetVolume(volume);
        }

        private void TheVolumeIsAt_Percent(int volume)
        {
            statefullAudioPlayer.SetVolume(volume);
        }

        private void TheApplicationIssuesACommandToPauseThePlayback()
        {
            statefullAudioPlayer.Pause();
        }

        private void TheApplicationIssuesACommandToUnpauseThePlayback()
        {
            statefullAudioPlayer.Play();
        }

        private void ThePlaybackHasBeenPaused()
        {
            statefullAudioPlayer.Pause();
        }

        private void TheCurrentPositionOfThePlaybackShouldBe_(int position)
        {
            Assert.That(statefullAudioPlayer.CurrentPosition, Is.EqualTo(position));
        }

        private void TheApplicationIssuesACommandToStopThePlayback()
        {
            statefullAudioPlayer.Stop();
        }

        private void ThereIsAnOngoingPlayback()
        {
            statefullAudioPlayer.Open(properAudioFilePath);
            ((AudioPlayerStub)(statefullAudioPlayer.BaseAudioPlayer)).CurrentPosition = 100;
        }

        private void TheStateOfThePlayerShouldBe_(MediaStatus mediaStatus)
        {
            Assert.That(statefullAudioPlayer.Status, Is.EqualTo(mediaStatus));
        }

        private void TheApplicationIssuesACommandToOpenAFile()
        {
            statefullAudioPlayer.Open(audioFilePath);
        }

        private void AProperPathToAnAudioFile()
        {
            audioFilePath = properAudioFilePath;
        }
    }
}