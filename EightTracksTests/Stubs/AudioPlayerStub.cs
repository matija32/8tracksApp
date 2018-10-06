
using System;
using EightTracksPlayer.Media;
using EightTracksPlayer.Media.Events;
using QuartzTypeLib;

namespace EightTracksTests.Stubs
{
    public class AudioPlayerStub : IAudioPlayer
    {

        public bool IsOpenIssued { get; set; }
        public bool IsPauseIssued { get; set; }
        public bool IsPlayIssued { get; set; }
        public bool IsStopIssued { get; set; }


        public void Open(string audioUri)
        {
            CurrentTrackDuration = 100;
            IsOpenIssued = true;
        }

        public void Pause()
        {
            IsPauseIssued = true;
        }

        public void Play()
        {
            CurrentPosition = 1;
            IsPlayIssued = true;
        }

        public void Stop()
        {
            CurrentPosition = 0;
            CurrentTrackDuration = 0;
            IsStopIssued = true;
        }

        public void SkipTo(int positionInSeconds)
        {
            if (CurrentPosition >= 0 && CurrentPosition <= CurrentTrackDuration)
            {
                CurrentPosition = positionInSeconds;
            }
        }

        public double CurrentPosition { get; set; }

        public double CurrentTrackDuration { get; set; }

        public event EventHandler<EndOfStreamReachedEventArgs> EndOfStreamReached;

        public void SetVolume(int volume)
        {
            Volume = volume;
        }

        public int Volume { get; set; }

        public void InvokeEndOfStreamReachedEvent()
        {
            if (EndOfStreamReached != null)
            {
                EndOfStreamReached(this, new EndOfStreamReachedEventArgs());
            }
        }
    }
}