using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using EightTracksPlayer.Utils.Browsing;
using log4net;
using ReactiveUI;
using ReactiveUI.Xaml;
using ILog = log4net.ILog;

namespace EightTracksPlayer.ViewModels
{
    public class MixViewModel : ReactiveValidatedObject, IEnableLogger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static MixViewModel NoMixViewModelAvailable = new MixViewModel(
            new Mix(new MixElement()
                        {
                            Description = "No description available",
                            Name = "No name available",
                            TagListCache = "No tags available",
                            UserElement = new UserElement() {Slug = "No author available"},
                            CoverUrls = new CoverUrlsElement() {sq250 = @"..\..\Resources\NoImageAvailable.png"}
                        }, 0), null, null, null);

        private int _CurrentTrackIndex;
        private bool _IsBeingRecorded;
        private bool _LikedByCurrentUser;
        private string _RecordingPath;
        private DispatchedReactiveCollection<TrackViewModel> _Tracks;
        //private ObservableAsPropertyHelper<int> _TotalNumberOfTracks;
        private string _CurrentTrackIndexAsString;
        private bool isUserRequested = true;


        public MixViewModel(Mix mixModel, IPlaybackController playbackController,
                            IMediaLibraryBrowser mediaLibraryBrowser,IObservable<bool> userLoggedInObservable)
        {

            mixModel.CurrentTrackIndexObservable.Subscribe(x =>
                                                               {
                                                                   // if we're playing a track that has already been added
                                                                   CurrentTrackIndex = x;
                                                                   UpdateCurrentTrackIndicator();
                                                               });
            mixModel.LikedByCurrentUserObservable.Subscribe(x => LikedByCurrentUser = x);

            this.Model = mixModel;

            Play = ReactiveCommand.Create(_ => true);
            Play.Subscribe(_ =>playbackController.Play(Model));


            ToggleLike = Model != Mix.NoMixAvailable
                             ? new ReactiveAsyncCommand(userLoggedInObservable, 1)
                             : new ReactiveAsyncCommand(Observable.Return(false), 1);

            ToggleLike.RegisterAsyncAction(_ => mediaLibraryBrowser.ToggleLike(Model));

            this.isUserRequested = ConfigurationManager.AppSettings["AudioPlayer"] == null || !ConfigurationManager.AppSettings["AudioPlayer"].Equals("DirectX WMI 3.14169");
            ReplaySubject<bool> skippingSongAllowed = new ReplaySubject<bool>(1);

            //skipping not allowed if there are no tracks in the mix
            Model.CurrentTrackIndexObservable.Select(trackIndex => trackIndex < Model.Tracks.Count && trackIndex >= 0
                                                                       ? Model.Tracks[trackIndex].IsSkipAllowed
                                                                       : false).Subscribe(skippingSongAllowed.OnNext);

            NextSong = new ReactiveAsyncCommand(skippingSongAllowed, 1);
            NextSong.RegisterAsyncAction(_ =>
                                             {
                                                 try
                                                 {
                                                     playbackController.NextSong(isUserRequested);
                                                 }
                                                 catch (Exception e)
                                                 {
                                                     log.Error("Unable to go to the next song", e);
                                                 }
                                             });

            Tracks = new DispatchedReactiveCollection<TrackViewModel>();

            // merge current items and future ones
            Observable.Merge(Model.Tracks.ToObservable(), Model.Tracks.ItemsAdded)
                .Select(CreateTrackViewModel)
                .Subscribe(trackViewModel =>
                               {
                                   Tracks.Add(trackViewModel);
                                   UpdateCurrentTrackIndicator();
                               });

            Download = ReactiveCommand.Create(_ => Model != Mix.NoMixAvailable);
            Download.Subscribe(_ =>
                                   {
                                       string destinationFolder = FileSystemBrowser.GetSaveToDirectory();

                                       if (String.IsNullOrEmpty(destinationFolder))
                                       {
                                           return;
                                       }

                                       destinationFolder += Path.DirectorySeparatorChar + Model.Name;
                                       FileSystemBrowser.TryCreateDirectory(destinationFolder);

                                       Tracks
//                                           .Where(
//                                               trackViewModel =>
//                                               trackViewModel.TrackLocation.Equals(FileLocation.Online.ToString()))
                                           .ToObservable()
                                           .Subscribe(trackViewModel => SaveTrack(trackViewModel, destinationFolder));
                                   });

            FileSystemBrowser = new FileSystemBrowser();
            WebAccessProxy = new WebAccessProxy();

//            _TotalNumberOfTracks = Tracks.CollectionCountChanged
//                .ToProperty(this, x => x.TotalNumberOfTracks);
            CurrentTrackIndexAsString = "0";

            skippingSongAllowed.OnNext(false);
        }

        public Mix Model { get; private set; }

        public IReactiveCommand Play { get; protected set; }
        public ReactiveAsyncCommand ToggleLike { get; protected set; }
        public ReactiveAsyncCommand NextSong { get; protected set; }
        public ReactiveCommand Download { get; protected set; }

        public IFileSystemBrowser FileSystemBrowser { get; set; }
        public IWebAccessProxy WebAccessProxy { get; set; }

        public int TotalNumberOfTracks
        {
            get { return Model.TotalNumberOfTracks; }
        }

        public String TotalDurationAsString
        {
            get
            {
                if (Model.TotalDuration.Hours == 0)
                {
                    return String.Format("{0}min", Model.TotalDuration.Minutes);
                }
                else
                {
                    return String.Format("{0}h{1}m", Model.TotalDuration.Hours, Model.TotalDuration.Minutes);
                }
            }
        }

        public string CoverUri
        {
            get { return Model.CoverUri; }
            set { this.RaiseAndSetIfChanged(x => x.CoverUri, value); }
        }

        public string Name
        {
            get { return Model.Name; }
            set { this.RaiseAndSetIfChanged(x => x.Name, value); }
        }

        public int CurrentTrackIndex
        {
            get { return _CurrentTrackIndex; }
            set
            {
                this.RaiseAndSetIfChanged(x => x.CurrentTrackIndex, value);
                
                //referencing the count directly because the track viewmodels are updated after the current index is increased
                CurrentTrackIndexAsString = (Model.Tracks.Count > 0 ? value + 1 : 0).ToString();
            }
        }

        public string CurrentTrackIndexAsString
        {
            get { return _CurrentTrackIndexAsString; }
            set { this.RaiseAndSetIfChanged(x => x.CurrentTrackIndexAsString, value); }
        }

        public string RecordingPath
        {
            get { return _RecordingPath; }
            set { this.RaiseAndSetIfChanged(x => x.RecordingPath, value); }
        }

        public bool IsBeingRecorded
        {
            get { return _IsBeingRecorded; }
            set { this.RaiseAndSetIfChanged(x => x.IsBeingRecorded, value); }
        }

        public bool LikedByCurrentUser
        {
            get { return _LikedByCurrentUser; }
            set { this.RaiseAndSetIfChanged(x => x.LikedByCurrentUser, value); }
        }

        public string Author
        {
            get { return Model.Author; }
            set { this.RaiseAndSetIfChanged(x => x.Author, value); }
        }

        public DispatchedReactiveCollection<TrackViewModel> Tracks
        {
            get { return _Tracks; }
            set { this.RaiseAndSetIfChanged(x => x.Tracks, value); }
        }

        public string FirstPublishedAt
        {
            get { return Model.FirstPublishedAt.ToLocalTime().ToString("dd MMM yyyy @ hh:mm"); }
            set { this.RaiseAndSetIfChanged(x => x.FirstPublishedAt, value); }
        }

        public int PlaysCount
        {
            get { return Model.PlaysCount; }
            set { this.RaiseAndSetIfChanged(x => x.PlaysCount, value); }
        }

        public int LikesCount
        {
            get { return Model.LikesCount; }
            set { this.RaiseAndSetIfChanged(x => x.LikesCount, value); }
        }

        public string Description
        {
            get { return Model.Description; }
            set { this.RaiseAndSetIfChanged(x => x.Description, value); }
        }

        public string RestfulUri
        {
            get { return Model.RestfulUri; }
            set { this.RaiseAndSetIfChanged(x => x.RestfulUri, value); }
        }

        public string TagList
        {
            get { return StringUtils.TagListToString(Model.TagListCache); }
            set { this.RaiseAndSetIfChanged(x => x.TagList, value); }
        }

        private void UpdateCurrentTrackIndicator()
        {
            Tracks.Select((track, index) =>
                              {
                                  return new KeyValuePair<TrackViewModel, bool>(track,
                                                                                index == CurrentTrackIndex);
                              })
            .ToObservable()
                .Subscribe(pair => pair.Key.IsCurrentlyPlayed = pair.Value);
        }

        private void SaveTrack(TrackViewModel trackViewModel, string destinationFolder)
        {
            string fileName = trackViewModel.GetFileName();

            string destinationPath = destinationFolder + Path.DirectorySeparatorChar + fileName;

            if (!File.Exists(destinationPath))
            {
                trackViewModel.SaveTo(destinationPath);
            }
        }

        private TrackViewModel CreateTrackViewModel(Track track)
        {
            return new TrackViewModel(track, FileSystemBrowser, (IWebAccessProxy) WebAccessProxy.Clone())
                       {SortIndex = -Tracks.Count};
        }
    }
}