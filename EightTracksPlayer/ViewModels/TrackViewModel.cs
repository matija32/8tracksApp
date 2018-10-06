using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using EightTracksPlayer.Utils.Browsing;
using log4net;
using ReactiveUI;
using ReactiveUI.Xaml;
using ILog = log4net.ILog;

namespace EightTracksPlayer.ViewModels
{
    public class TrackViewModel : ReactiveValidatedObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); 
        private ObservableAsPropertyHelper<string> _Duration;
        private bool _IsCurrentlyPlayed;
        private ObservableAsPropertyHelper<string> _TrackLocation;

        public TrackViewModel(Track model, IFileSystemBrowser fileSystemBrowser, IWebAccessProxy webAccessProxy)
        {
            this.Model = model;
            FileSystemBrowser = fileSystemBrowser;
            WebAccessProxy = webAccessProxy;

            var noOngoingDownloadObservable =
                WebAccessProxy.FileLocationObservable.Select(x => x != FileLocation.Downloading);

            _TrackLocation = WebAccessProxy.FileLocationObservable
                .Select(x => x.ToString()).ToProperty(this, x => x.TrackLocation);

            Download = new ReactiveAsyncCommand(noOngoingDownloadObservable, 1);
            Download.Subscribe(_ =>
                                   {

                                       string fileName = GetFileName();

                                       string destinationPath = FileSystemBrowser.GetSaveAsLocation(fileName);

                                       if (String.IsNullOrEmpty(destinationPath))
                                       {
                                           log.Info("Download cancelled - " + Model.Uri);
                                           return;
                                       }

                                       log.Info(String.Format("Download started. From {0} to {1}", Model.Uri,
                                                                 destinationPath));
                                       SaveTo(destinationPath);
                                   });

            _Duration = Model.DurationObservable.Select(DurationToString).ToProperty(this, x => x.Duration);
        }

        public string GetFileName()
        {
            string sourceUri = Model.Uri;
            string extension = Path.GetExtension(sourceUri);
            // soundcloud API provides a sourceUri without an extension -> in case extension is an empty String,
            // set extension to .mp3
            if (extension == String.Empty) extension = ".mp3";

            string fileName = String.Format("{0} - {1}{2}", Model.Performer, Model.Name, extension);

            return RemoveInvalidCharacters(fileName);
        }

        private string RemoveInvalidCharacters(string fileName)
        {
            string resultFilename = "";

            List<char> invalidChars = Path.GetInvalidFileNameChars().ToList();
            invalidChars.Add(':');

            resultFilename = new string(fileName.Select(c => invalidChars.Contains(c) ? ' ' : c).ToArray());

            return resultFilename;
        }

        public Track Model { get; private set; }
        public ReactiveAsyncCommand Download { get; private set; }

        public IFileSystemBrowser FileSystemBrowser { get; set; }
        public IWebAccessProxy WebAccessProxy { get; set; }

        public string TrackLocation
        {
            get { return _TrackLocation.Value; }
        }

        public bool IsCurrentlyPlayed
        {
            get { return _IsCurrentlyPlayed; }
            set { this.RaiseAndSetIfChanged(x => x.IsCurrentlyPlayed, value); }
        }

        public bool IsLast
        {
            get { return Model.IsLast; }
            set { this.RaiseAndSetIfChanged(x => x.IsLast, value); }
        }

        public string Name
        {
            get { return Model.Name; }
            set { this.RaiseAndSetIfChanged(x => x.Name, value); }
        }

        public string Performer
        {
            get { return Model.Performer; }
            set { this.RaiseAndSetIfChanged(x => x.Performer, value); }
        }

        [Required]
        public string Duration
        {
            get { return _Duration.Value; }
        }

        [Required]
        public int SortIndex { get; set; }

        public void SaveTo(string destinationPath)
        {
            try
            {
                WebAccessProxy.Download(Model.Uri, destinationPath, Performer, Name);
            }
            catch (Exception e)
            {
                log.Error("Unable to download the track", e);
            }
            
        }

        private string DurationToString(double duration)
        {
            var durationTimeSpan = TimeSpan.FromSeconds(duration);
            return ((int) durationTimeSpan.TotalMinutes).ToString("D2") + ":" + durationTimeSpan.Seconds.ToString("D2");
        }
    }
}