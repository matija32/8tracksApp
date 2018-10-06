using System;
using EightTracksPlayer.Media.DxMessaging;
using EightTracksPlayer.Media.Events;
using QuartzTypeLib;

namespace EightTracksPlayer.Media
{
    public class DxAudioPlayerAdapter : IAudioPlayer
    {
        protected IBasicAudio basicAudio = null;
        protected FilgraphManager filgraphManager = null;
        protected IMediaControl mediaControl = null;
        protected IMediaEvent mediaEvent = null;
        protected IMediaEventEx mediaEventEx = null;
        protected IMediaPosition mediaPosition = null;
        private DxMessageHandler messageHandler = new DxMessageHandler();

        public DxAudioPlayerAdapter()
        {
            messageHandler.PlaybackStopped += new EventHandler<DxMessageEventArgs>(messageHandler_PlaybackStopped);
        }

        #region IAudioPlayer Members

        public event EventHandler<EndOfStreamReachedEventArgs> EndOfStreamReached;

        public void SetVolume(int volume)
        {
            if (!IsMediaControlPresent()) return;
            basicAudio.Volume = volume;
        }

        public double CurrentPosition
        {
            get
            {
                if (mediaPosition == null)
                {
                    return 0;
                }

                return (int) mediaPosition.CurrentPosition;
            }
        }

        public double CurrentTrackDuration
        {
            get
            {
                if (mediaPosition == null)
                {
                    return 0;
                }
                return mediaPosition.Duration;
            }
        }


        public void Open(string audioUri)
        {
            CleanUp();
            //Console.WriteLine(audioUri);
            filgraphManager = new FilgraphManager();
            filgraphManager.RenderFile(audioUri);

            SetPlayerCollaborators();

            mediaControl.Run();
        }

        public void Pause()
        {
            if (!IsMediaControlPresent()) return;
            mediaControl.Pause();
        }

        public void Play()
        {
            if (!IsMediaControlPresent()) return;
            mediaControl.Run();
        }

        public void Stop()
        {
            if (!IsMediaControlPresent()) return;
            mediaControl.Stop();
            mediaPosition.CurrentPosition = 0;
        }

        public void SkipTo(int positionInSeconds)
        {
            if (CanSkipTo(positionInSeconds))
            {
                mediaPosition.CurrentPosition = positionInSeconds;
            }
        }

        #endregion

        private void messageHandler_PlaybackStopped(object sender, DxMessageEventArgs e)
        {
            if (EndOfStreamReached != null)
            {
                EndOfStreamReached(this, new EndOfStreamReachedEventArgs());
            }
        }

        protected virtual void SetPlayerCollaborators()
        {
            basicAudio = filgraphManager as IBasicAudio;

            mediaPosition = filgraphManager as IMediaPosition;

            mediaControl = filgraphManager as IMediaControl;

            mediaEvent = filgraphManager as IMediaEvent;

            mediaEventEx = filgraphManager as IMediaEventEx;

            messageHandler.MediaEventEx = mediaEventEx;
        }

        private void CleanUp()
        {
            if (mediaControl != null)
            {
                mediaControl.StopWhenReady();
            }

            mediaControl = null;
            mediaPosition = null;
            basicAudio = null;
            filgraphManager = null;
        }

        private bool IsMediaControlPresent()
        {
            return mediaControl != null;
        }

        private bool CanSkipTo(int positionInSeconds)
        {
            return mediaPosition != null
                   && positionInSeconds > 0
                   && positionInSeconds < mediaPosition.Duration;
        }
    }
}