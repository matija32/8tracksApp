using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EightTracksPlayer.Media.Events;
using ReactiveUI;

namespace EightTracksPlayer.Media
{
    public class StatefullAudioPlayer : IAudioPlayer
    {
        private IAudioPlayer baseAudioPlayer;
        private IObservable<double> currentPositionObservable;
        private ReplaySubject<MediaStatus> playerStateObservable = new ReplaySubject<MediaStatus>(1);

        private MediaStatus status;

        public StatefullAudioPlayer(IAudioPlayer baseAudioPlayer)
        {
            this.BaseAudioPlayer = baseAudioPlayer;
            Status = MediaStatus.Unknown;

            //user to have RxApp.DeferredScheduler
            CurrentPositionObservable = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(0.5))
                .Select(_ => BaseAudioPlayer.CurrentPosition)
                .DistinctUntilChanged();

            Status = MediaStatus.Stopped;
            this.BaseAudioPlayer.EndOfStreamReached +=
                new EventHandler<EndOfStreamReachedEventArgs>(BaseAudioPlayer_EndOfStreamReached);
        }

        public MediaStatus Status
        {
            get { return status; }
            private set
            {
                playerStateObservable.OnNext(value);
                status = value;
            }
        }

        public IAudioPlayer BaseAudioPlayer
        {
            get { return baseAudioPlayer; }
            set { baseAudioPlayer = value; }
        }

        public IObservable<double> CurrentPositionObservable
        {
            get { return currentPositionObservable; }
            private set { currentPositionObservable = value; }
        }

        public IObservable<MediaStatus> PlayerStateObservable
        {
            get { return playerStateObservable; }
        }

        #region IAudioPlayer Members

        public event EventHandler<EndOfStreamReachedEventArgs> EndOfStreamReached;

        public void Open(string audioUri)
        {
            BaseAudioPlayer.Open(audioUri);
            Status = MediaStatus.Running;
        }

        public void Pause()
        {
            BaseAudioPlayer.Pause();
            Status = MediaStatus.Paused;
        }

        public void Play()
        {
            BaseAudioPlayer.Play();
            Status = MediaStatus.Running;
        }

        public void Stop()
        {
            BaseAudioPlayer.Stop();
            Status = MediaStatus.Stopped;
        }

        public void SkipTo(int positionInSeconds)
        {
            if (Status == MediaStatus.Running || Status == MediaStatus.Paused)
            {
                BaseAudioPlayer.SkipTo(positionInSeconds);
            }
        }

        public double CurrentPosition
        {
            get { return BaseAudioPlayer.CurrentPosition; }
        }

        public double CurrentTrackDuration
        {
            get { return BaseAudioPlayer.CurrentTrackDuration; }
        }

        public void SetVolume(int volume)
        {
            BaseAudioPlayer.SetVolume(volume);
        }

        #endregion

        private void BaseAudioPlayer_EndOfStreamReached(object sender, EndOfStreamReachedEventArgs e)
        {
            OnEndOfStreamReached();
        }

        private void OnEndOfStreamReached()
        {
            BaseAudioPlayer.Stop();
            if (EndOfStreamReached != null)
            {
                EndOfStreamReached(this, new EndOfStreamReachedEventArgs());
            }
        }
    }
}