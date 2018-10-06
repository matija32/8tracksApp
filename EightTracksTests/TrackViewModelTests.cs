using System;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils.Browsing;
using EightTracksPlayer.ViewModels;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class TrackViewModelTests
    {

        private const string applicationTag = Tags.Application;
        private const string downloadingMediaContentTag = Tags.DownloadingMediaContent;

        private Track track;
        private FileSystemBrowserStub fileSystemBrowser;
        private WebAccessProxyStub webAccessProxy;
        private TrackViewModel trackViewModel;
        private Feature downloadingMediaContent;
        private string destinationFilename;

        [SetUp]
        public void SetUp()
        {
            destinationFilename = "anArtist - aName.ext";
            track = new Track(new TrackElement(), false, true);
            fileSystemBrowser = new FileSystemBrowserStub() { DestinationFilename = destinationFilename };
            webAccessProxy = new WebAccessProxyStub();
            downloadingMediaContent = Features.DownloadingMediaContent;

            trackViewModel = new TrackViewModel(track, fileSystemBrowser, webAccessProxy);
        }

        [Test]
        public void TestDownloadsATrack()
        {
            string trackUri = "domain.org/subdomain/some-track.mp3";
            string trackName = "some-track-name";
            string trackPerformer = "some-track-performer";
            string defaultFilename = String.Format("{0} - {1}{2}", trackPerformer, trackName, ".mp3");

            downloadingMediaContent
                .WithScenario("downloading a song").Tag(applicationTag).Tag(downloadingMediaContentTag)
                .Given(TheTrackUriIs_, trackUri)
                    .And(TheTrackNameIs_, trackName)
                    .And(TheTrackPerformerIs_, trackPerformer)
                .When(TheApplicationIssuesACommandToDownloadTheTrack)
                    .And(TheCommandIsProcessed)
                .Then(TheDefaultFilenameIs_, defaultFilename)
                    .And(TheTrackShouldBeDownloadedFrom_To_, trackUri, destinationFilename)
                    .And(TheLocationOfTheTrackIs_, FileLocation.Downloaded)
                .ExecuteWithReport();
                
        }

        private void TheCommandIsProcessed()
        {
            webAccessProxy.PushNewFileLocation(FileLocation.Downloaded);
        }

        private void TheLocationOfTheTrackIs_(FileLocation fileLocation)
        {
            Assert.That(trackViewModel.TrackLocation, Is.EqualTo(fileLocation.ToString()));
        }

        private void TheTrackShouldBeDownloadedFrom_To_(string trackUri, string destinationFilename)
        {
            Assert.That(webAccessProxy.SourceUri, Is.EqualTo(trackUri));
            Assert.That(webAccessProxy.DestinationUri, Is.EqualTo(destinationFilename));
        }

        private void TheDefaultFilenameIs_(string defaultFilename)
        {
            Assert.That(fileSystemBrowser.SourceFilename, Is.EqualTo(defaultFilename));
        }

        private void TheTrackPerformerIs_(string trackPerformer)
        {
            track.Performer = trackPerformer;
        }

        private void TheTrackNameIs_(string trackName)
        {
            track.Name = trackName;
        }

        private void TheApplicationIssuesACommandToDownloadTheTrack()
        {
            trackViewModel.Download.Execute(null);
        }

        private void TheTrackUriIs_(string trackUri)
        {
            track.Uri = trackUri;
        }

        [Test]
        public void TestDoesNotAllowMoreThanOneOngoingTrackDownload()
        {
            downloadingMediaContent
                .WithScenario("enables downloading a track if the track is not currently being downloaded").Tag(applicationTag).Tag(downloadingMediaContentTag)
                .Given(TheTrackNotDownloaded)
                .When(TheTrackDownloadProcessStarts)
                .Then(TheApplication_DownloadTheTrack, false)

                .WithScenario("enables downloading a track if the track is not currently being downloaded").Tag(applicationTag).Tag(downloadingMediaContentTag)
                .Given(TheTrackDownloadIsOngoing)
                .When(TheTrackDownloadProcessFinishes)
                .Then(TheApplication_DownloadTheTrack, true)
                .ExecuteWithReport();
        }

        private void TheApplication_DownloadTheTrack([BooleanParameterFormat("can", "can not")]bool canDownload)
        {
            Assert.That(trackViewModel.Download.CanExecute(null), Is.EqualTo(canDownload));
        }

        private void TheTrackDownloadProcessFinishes()
        {
            webAccessProxy.PushNewFileLocation(FileLocation.Downloaded);
        }

        private void TheTrackDownloadProcessStarts()
        {
            webAccessProxy.PushNewFileLocation(FileLocation.Downloading);
        }

        private void TheTrackDownloadIsOngoing()
        {
            webAccessProxy.PushNewFileLocation(FileLocation.Downloading);
        }

        private void TheTrackNotDownloaded()
        {
            webAccessProxy.PushNewFileLocation(FileLocation.Online);
        }
    }
}