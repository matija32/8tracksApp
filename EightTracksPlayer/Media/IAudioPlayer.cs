using System;
using EightTracksPlayer.Media.Events;

namespace EightTracksPlayer.Media
{
    public interface IAudioPlayer
    {
        double CurrentPosition { get; }

        double CurrentTrackDuration { get; }
        void Open(string audioUri);

        void Pause();

        void Play();

        void Stop();

        void SkipTo(int positionInSeconds);

        event EventHandler<EndOfStreamReachedEventArgs> EndOfStreamReached;

        void SetVolume(int volume);
    }
}