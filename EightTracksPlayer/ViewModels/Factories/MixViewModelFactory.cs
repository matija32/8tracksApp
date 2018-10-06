using System;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.ViewModels.Factories
{
    public class MixViewModelFactory : IMixViewModelFactory
    {
        private readonly IMediaLibraryBrowser mediaLibraryBrowser;
        private readonly IPlaybackController playbackController;
        private readonly IObservable<bool> userLoggedInObservable;

        public MixViewModelFactory(IPlaybackController playbackController, IMediaLibraryBrowser mediaLibraryBrowser,
                                   IObservable<bool> userLoggedInObservable)
        {
            this.playbackController = playbackController;
            this.mediaLibraryBrowser = mediaLibraryBrowser;
            this.userLoggedInObservable = userLoggedInObservable;
        }

        #region IMixViewModelFactory Members

        public MixViewModel CreateMixViewModel(Mix mix)
        {
            if (mix == Mix.NoMixAvailable)
            {
                return new MixViewModel(mix, playbackController, mediaLibraryBrowser, null);
            }
            else
            {
                return new MixViewModel(mix, playbackController, mediaLibraryBrowser, userLoggedInObservable);
            }
        }

        #endregion
    }
}