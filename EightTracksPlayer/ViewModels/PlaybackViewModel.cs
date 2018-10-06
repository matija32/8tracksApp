using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Media;
using EightTracksPlayer.Utils;
using EightTracksPlayer.ViewModels.Factories;
using log4net;
using ReactiveUI;
using ReactiveUI.Xaml;
using ILog = log4net.ILog;

namespace EightTracksPlayer.ViewModels
{
    public class PlaybackViewModel : ReactiveValidatedObject
    {
        private readonly IPlaybackController playbackController;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private ObservableAsPropertyHelper<MixViewModel> _CurrentMixViewModel;
        private ObservableAsPropertyHelper<double> _CurrentPosition;
        private ObservableAsPropertyHelper<string> _CurrentPositionAsString;
        private ObservableAsPropertyHelper<double> _CurrentTrackDuration;
        private ObservableAsPropertyHelper<string> _CurrentTrackDurationAsString;
        private ObservableAsPropertyHelper<bool> _IsPositionSliderEnabled;
        private ObservableAsPropertyHelper<IReactiveCommand> _NextSong;

        private int _SelectedTrackIndex;
        private double _Volume;
        private Settings settings;

        private string volumeKey = "8t_volume";

        public PlaybackViewModel(IPlaybackController playbackController, Settings settings,
                                 IMixViewModelFactory mixViewModelFactory)
        {
            this.playbackController = playbackController;
            this.settings = settings;
            var volumeString = (string) settings[volumeKey] ?? "50";
            Volume = Double.Parse(volumeString);

            _CurrentPosition = playbackController.CurrentPositionObservable
                .ToProperty(this, x => x.CurrentPosition);

            _CurrentPositionAsString = playbackController.CurrentPositionObservable
                .Select(TimeSpan.FromSeconds)
                .Select(DurationToString)
                .ToProperty(this, x => x.CurrentPositionAsString);

            // connecting current mix being played to the viewmodel

            IObservable<MixViewModel> currentMixViewModelObservable = playbackController.CurrentMixObservable.Select(mixViewModelFactory.CreateMixViewModel);

            _CurrentMixViewModel = currentMixViewModelObservable
                .ToProperty(this, x => x.CurrentMixViewModel);


            IObservable<bool> _CurrentMixViewModelPresent = 
                this.WhenAny(
                    x => x.CurrentMixViewModel, 
                    mixViewModel => mixViewModel.Value != null 
                        && mixViewModel.Value.Model != Mix.NoMixAvailable);


            _CurrentTrackDurationAsString = playbackController.CurrentDurationObservable
                .Select(duration => DurationToString(TimeSpan.FromSeconds(duration)))
                .ToProperty(this, x => x.CurrentTrackDurationAsString);

            _CurrentTrackDuration = playbackController.CurrentDurationObservable
                .ToProperty(this, x => x.CurrentTrackDuration);


            // commands            
            
            // going to the next song only if there is a mix currently loaded
            _NextSong = currentMixViewModelObservable
                .Select(x => x.NextSong)
                .ToProperty(this, x => x.NextSong);


            var audioPlayerState = playbackController.PlayerStateObservable;

            // both having a current mix and not running
            var audioPlayerNotRunning = audioPlayerState.Select(status =>
                                                                status == MediaStatus.Stopped ||
                                                                status == MediaStatus.Paused).CombineLatest(
                                                                    _CurrentMixViewModelPresent, (x, y) => x && y);

            Continue = new ReactiveAsyncCommand(audioPlayerNotRunning, 1);
            Continue.Subscribe(_ => playbackController.Continue());

            var audioPlayerRunning = audioPlayerState.Select(status => status == MediaStatus.Running);
            Pause = new ReactiveAsyncCommand(audioPlayerRunning, 1);
            Pause.Subscribe(_ => playbackController.Pause());

            var audioPlayerRunningOrPaused = audioPlayerState.Select(status =>
                                                                     status == MediaStatus.Running ||
                                                                     status == MediaStatus.Paused);
            Stop = new ReactiveAsyncCommand(audioPlayerRunningOrPaused, 1);
            Stop.Subscribe(_ => playbackController.Stop());

            NextMix = new ReactiveAsyncCommand(_CurrentMixViewModelPresent, 1);
            NextMix.RegisterAsyncAction(_ =>
                                            {
                                                try
                                                {
                                                    playbackController.NextMix();
                                                }
                                                catch(Exception e)
                                                {
                                                    log.Error("Unable to go to the next mix", e);
                                                }
                                            }
                );

            bool debugModeOn = ConfigurationManager.AppSettings["AudioPlayer"] != null && ConfigurationManager.AppSettings["AudioPlayer"].Equals("DirectX WMI 3.14169");

            _IsPositionSliderEnabled = audioPlayerRunningOrPaused
                .Select(a => a && debugModeOn)
                .ToProperty(this, x => x.IsPositionSliderEnabled);

            GoToTrack = new ReactiveAsyncCommand(_CurrentMixViewModelPresent, 1);
            GoToTrack.Subscribe(_ =>
                                    {
                                        if (debugModeOn)
                                        {
                                            try
                                            {
                                                playbackController.GoToTrack(SelectedTrackIndex);
                                            }
                                            catch(Exception e)
                                            {
                                                log.Error("Unable to go to track " + SelectedTrackIndex, e);
                                            }
                                        }
                                    });

            Skip = new ReactiveAsyncCommand(audioPlayerRunningOrPaused, 1);
            Skip.Subscribe(step =>
                               {
                                   double newPosition = CurrentPosition + Double.Parse((string) step);
                                   if (newPosition >= 0 && newPosition <= CurrentTrackDuration)
                                   {
                                       CurrentPosition = newPosition;
                                   }
                               });

            ChangeVolume = new ReactiveAsyncCommand(null, 1);
            ChangeVolume.Subscribe(step =>
                                       {
                                           double newVolume = Volume + Double.Parse((string) step);
                                           if (newVolume >= 0 && newVolume <= 100)
                                           {
                                               Volume = newVolume;
                                           }
                                       });
        }

        
        public ReactiveAsyncCommand Continue { get; protected set; }
        public ReactiveAsyncCommand Pause { get; protected set; }
        public ReactiveAsyncCommand Stop { get; protected set; }
        public ReactiveAsyncCommand NextMix { get; protected set; }
        public ReactiveAsyncCommand GoToTrack { get; protected set; }
        public ReactiveAsyncCommand Skip { get; protected set; }
        public ReactiveAsyncCommand ChangeVolume { get; protected set; }

        public IReactiveCommand NextSong
        {
            get { return _NextSong.Value; } 
        }

        public double CurrentPosition
        {
            get { return _CurrentPosition.Value; }
            set { playbackController.SkipTo(value); }
        }

        public int SelectedTrackIndex
        {
            get { return _SelectedTrackIndex; }
            set { _SelectedTrackIndex = value; }
        }

        public string CurrentTrackDurationAsString
        {
            get { return _CurrentTrackDurationAsString.Value; }
        }

        public double CurrentTrackDuration
        {
            get { return _CurrentTrackDuration.Value; }
        }

        public string CurrentPositionAsString
        {
            get { return _CurrentPositionAsString.Value; }
        }


        public MixViewModel CurrentMixViewModel
        {
            get { return _CurrentMixViewModel.Value; }
            //            set { this.RaiseAndSetIfChanged(x => x.CurrentMixViewModel, value); }
        }

        public bool IsPositionSliderEnabled
        {
            get { return _IsPositionSliderEnabled.Value; }
        }

        public double Volume
        {
            get { return _Volume; }
            set
            {
                //_Volume = value;
                playbackController.SetVolume((int) value);
                settings[volumeKey] = value;

                this.RaiseAndSetIfChanged(x => x.Volume, value);
            }
        }

        private string DurationToString(TimeSpan duration)
        {
            return ((int) duration.TotalMinutes).ToString("D2") + ":" + duration.Seconds.ToString("D2");
        }
    }
}