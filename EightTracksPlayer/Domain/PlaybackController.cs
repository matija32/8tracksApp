using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using EightTracksPlayer.Communication;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Media;
using EightTracksPlayer.Media.Events;
using log4net;

namespace EightTracksPlayer.Domain
{
    public class PlaybackController : IPlaybackController
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private StatefullAudioPlayer audioPlayerProxy;
        private ReplaySubject<double> currentDurationObservable = new ReplaySubject<double>(1);
        private Mix currentMix = null;
        private ReplaySubject<Mix> currentMixObservable = new ReplaySubject<Mix>(1);
        private string playToken = "";
        private RequestExecutionAdapter requestExecutor;
        private RequestFactory requestFactory = RequestFactory.Instance;

        public PlaybackController()
            : this(new WmpAudioPlayerAdapter(), new HttpRequestExecutor())
        {
        }

        public PlaybackController(IAudioPlayer audioPlayer, IRequestExecutor requestExecutor)
        {
            this.audioPlayerProxy = new StatefullAudioPlayer(audioPlayer);
            this.requestExecutor = new RequestExecutionAdapter(requestExecutor);

            audioPlayerProxy.EndOfStreamReached +=
                new EventHandler<EndOfStreamReachedEventArgs>(audioPlayerProxy_EndOfStreamReached);

            //bring the position to zero if we reach the end of stream
            audioPlayerProxy.CurrentPositionObservable
                .Where(_ => audioPlayerProxy.Status == MediaStatus.Running)
                .Where(currentPosition => currentPosition == audioPlayerProxy.CurrentTrackDuration)
                .Subscribe(_ => audioPlayerProxy.SkipTo(0));


            audioPlayerProxy.CurrentPositionObservable
                .Where(_ => audioPlayerProxy.Status == MediaStatus.Running)
                .Select(_ => audioPlayerProxy.CurrentTrackDuration)
                .Where(duration => duration > 0)
                .DistinctUntilChanged()
                .Subscribe(duration =>
                               {
                                   currentDurationObservable.OnNext(duration);
                                   currentMix.GetCurrentTrack().Duration = audioPlayerProxy.CurrentTrackDuration;
                               });

            currentMixObservable.OnNext(Mix.NoMixAvailable);
        }

        #region IPlaybackController Members

        public void Play(Mix mix)
        {
            if (playToken.Length == 0)
            {
                playToken = GetPlayToken();
            }

            if (mix.Tracks.Count == 0)
            {
                PlayMixRequest playMixRequest = requestFactory.CreatePlayMixRequest(playToken, mix.MixId);
                PlaySongResponse playSongReponse = requestExecutor.ExecutePlayMixRequest(playMixRequest);

                Track openingTrack = new Track(playSongReponse.SetElement.TrackElement,
                                               playSongReponse.SetElement.AtLastTrack,
                                               playSongReponse.SetElement.SkipAllowed);
                if (String.IsNullOrEmpty(openingTrack.Uri))
                {
                    return;
                }

                mix.AddTrack(openingTrack);
            }

            // in case we want to play the same mix again, 
            // it means we don't change anything in the UI
            // just reset the counter and start again
            if (currentMix==null || !mix.RestfulUri.Equals(currentMix.RestfulUri))
            {
                currentMix = mix;
                currentMixObservable.OnNext(currentMix);
            }

            currentMix.ResetTrackIndex();
            PlayCurrentTrack(currentMix);
        }

        public void NextSong(bool isUserRequested)
        {
            if (!IsMixPresent)
            {
                return;
            }

            if (currentMix.GetCurrentTrack().IsLast)
            {
                NextMix();
                return;
            }

            if (currentMix.CurrentTrackIndex == currentMix.Tracks.Count - 1)
            {
                NextSongRequest nextSongRequest = requestFactory.CreateNextSongRequest(playToken, currentMix.MixId, isUserRequested);
                PlaySongResponse playSongReponse = requestExecutor.ExecuteNextSongRequest(nextSongRequest);

                currentMix.AddTrack(new Track(playSongReponse.SetElement.TrackElement,
                                              playSongReponse.SetElement.AtLastTrack,
                                              playSongReponse.SetElement.SkipAllowed));
            }

            currentMix.MoveToNextTrack();

            if (audioPlayerProxy.Status == MediaStatus.Running)
            {
                PlayCurrentTrack(currentMix);
            }
        }

        public bool IsMixPresent
        {
            get { return currentMix != null; }
        }

        public void Pause()
        {
            audioPlayerProxy.Pause();
        }

        public void Continue()
        {
            if (audioPlayerProxy.Status == MediaStatus.Stopped)
            {
                PlayCurrentTrack(currentMix);
            }
            else if (audioPlayerProxy.Status == MediaStatus.Paused)
            {
                audioPlayerProxy.Play();
            }
        }

        public IObservable<double> CurrentPositionObservable
        {
            get { return this.audioPlayerProxy.CurrentPositionObservable; }
        }

        public IObservable<Mix> CurrentMixObservable
        {
            get { return currentMixObservable; }
        }

        public IObservable<MediaStatus> PlayerStateObservable
        {
            get { return audioPlayerProxy.PlayerStateObservable; }
        }

        public IObservable<double> CurrentDurationObservable
        {
            get { return currentDurationObservable; }
        }

        public void NextMix()
        {
            NextMixRequest nextMixRequest = requestFactory.CreateNextMixRequest(currentMix.MixId, currentMix.SetId,
                                                                                playToken);
            NextMixResponse nextMixResponse = requestExecutor.ExecuteNextMixRequest(nextMixRequest);
            Mix mix = new Mix(nextMixResponse.NextMix, currentMix.SetId);
            Play(mix);
        }

        public void Stop()
        {
            audioPlayerProxy.Stop();
        }

        public void SkipTo(double positionInSeconds)
        {
            audioPlayerProxy.SkipTo((int) positionInSeconds);
        }

        public void SetVolume(int volume)
        {
            audioPlayerProxy.SetVolume(volume);
        }

        public void GoToTrack(int trackIndex)
        {
            this.Stop();
            currentMix.GoToTrack(trackIndex);
            this.Continue();
        }

        #endregion

        private void PlayCurrentTrack(Mix mix)
        {
            Track track = mix.GetCurrentTrack();
            audioPlayerProxy.Open(track.Uri);
        }

        private void audioPlayerProxy_EndOfStreamReached(object sender, EndOfStreamReachedEventArgs e)
        {
            if (currentMix.GetCurrentTrack().IsLast)
            {
                NextMix();
            }
            else
            {
                NextSong(false);
            }
        }

        private string GetPlayToken()
        {
            NewPlayTokenRequest newPlayTokenRequest = requestFactory.CreateNewPlayTokenRequest();
            NewPlayTokenResponse newPlayTokenResponse = requestExecutor.ExecuteNewPlayTokenRequest(newPlayTokenRequest);

            return newPlayTokenResponse.PlayToken;
        }
    }
}