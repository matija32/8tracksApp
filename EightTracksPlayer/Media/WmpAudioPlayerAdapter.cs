using System;
using EightTracksPlayer.Media.Events;
using log4net;
using ReactiveUI;
using WMPLib;
using ILog = log4net.ILog;

namespace EightTracksPlayer.Media
{
    public class WmpAudioPlayerAdapter : IAudioPlayer, IEnableLogger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private bool isMediaEnded = false;
        private bool isMediaLoaded = false;
        private WindowsMediaPlayer windowsMediaPlayer = new WindowsMediaPlayer();

        public WmpAudioPlayerAdapter()
        {
            windowsMediaPlayer.PlayStateChange +=
                new _WMPOCXEvents_PlayStateChangeEventHandler(windowsMediaPlayer_PlayStateChange);
        }

        #region IAudioPlayer Members

        public event EventHandler<EndOfStreamReachedEventArgs> EndOfStreamReached;

        public void SetVolume(int volume)
        {
            windowsMediaPlayer.settings.volume = volume;
        }

        public void Open(string audioUri)
        {
            windowsMediaPlayer.URL = audioUri;
            isMediaLoaded = true;
            Play();
        }

        public void Pause()
        {
            windowsMediaPlayer.controls.pause();
        }

        public void Play()
        {
            windowsMediaPlayer.controls.play();
        }

        public void Stop()
        {
            windowsMediaPlayer.controls.stop();
            windowsMediaPlayer.controls.currentPosition = 0;
//            windowsMediaPlayer.URL = "";
            isMediaLoaded = false;
        }

        public void SkipTo(int positionInSeconds)
        {
            if (CanSkipTo(positionInSeconds))
            {
                windowsMediaPlayer.controls.currentPosition = positionInSeconds;
            }
        }

        public double CurrentPosition
        {
            get
            {
                if (windowsMediaPlayer == null || NoTrackLoaded())
                {
                    return 0;
                }
                try
                {
                    return windowsMediaPlayer.controls.currentPosition;
                }
                catch (Exception e)
                {
                    log.Info("Error received from the underlying windows media player, setting the current position to 0", e);
                    return 0;
                }
            }
        }

        public double CurrentTrackDuration
        {
            get
            {
                if (windowsMediaPlayer == null || NoTrackLoaded())
                {
                    return 0;
                }

                try
                {
                    return windowsMediaPlayer.controls.currentItem.duration;
                }
                catch (Exception e)
                {
                    log.Info(
                        "Error received from the underlying windows media player, setting the current track duration to 0", e);
                    return 0;
                }
            }
        }

        #endregion

        private void windowsMediaPlayer_PlayStateChange(int NewState)
        {
            if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                isMediaEnded = true;
            }
            else if (windowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsTransitioning)
            {
                if (isMediaEnded)
                {
                    isMediaEnded = false;
                    // End of stream occurs here. Do what ever you want.
                    OnEndOfStreamReached();
                }
            }
        }

        private void OnEndOfStreamReached()
        {
            if (EndOfStreamReached != null)
            {
                EndOfStreamReached(this, new EndOfStreamReachedEventArgs());
            }
        }

        private bool CanSkipTo(int positionInSeconds)
        {
            return positionInSeconds > 0
                   && positionInSeconds < CurrentTrackDuration;
        }

        private bool NoTrackLoaded()
        {
            return !isMediaLoaded;
        }
    }
}