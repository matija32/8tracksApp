using System;
using System.Collections.Generic;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Media;

namespace EightTracksPlayer.Domain
{
    /// <summary>
    /// PlaybackController controls the current audio playback and is responsible for automatic switching to the next item in the playlist.
    /// </summary>
    public interface IPlaybackController
    {
        bool IsMixPresent { get; }

        IObservable<double> CurrentDurationObservable { get; }
        IObservable<double> CurrentPositionObservable { get; }
        IObservable<Mix> CurrentMixObservable { get; }
        IObservable<MediaStatus> PlayerStateObservable { get; }
        void Play(Mix mix);

        void NextSong(bool isUserRequested);
        void Pause();
        void Continue();

        void NextMix();
        void Stop();
        void SkipTo(double positionInSeconds);
        void SetVolume(int volume);
        void GoToTrack(int trackIndex);
        
    }
}