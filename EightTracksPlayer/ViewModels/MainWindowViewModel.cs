using System;
using System.Linq;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using EightTracksPlayer.ViewModels.Factories;
using ReactiveUI;
using ReactiveUI.Xaml;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace EightTracksPlayer.ViewModels
{
    public class MainWindowViewModel : ReactiveValidatedObject
    {


        public MainWindowViewModel()
            : this(new PlaybackController(), new MediaLibraryBrowser(), new Authenticator(), new Settings())
        {
        }

        public MainWindowViewModel(IPlaybackController playbackController, IMediaLibraryBrowser mediaLibraryBrowser,
                                   IAuthenticator authenticator, Settings settings)
        {
            LoginFormViewModel = new LoginFormViewModel(authenticator);
            LoginFormViewModel.UserDataObservable.Subscribe(
                userData => mediaLibraryBrowser.UserToken = userData.UserToken);

            
            IMixViewModelFactory mixViewModelFactory = new MixViewModelFactory(playbackController, mediaLibraryBrowser,
                                                                               LoginFormViewModel.UserLoggedInObservable);
            this.MediaBrowserViewModel = new MediaBrowserViewModel(mediaLibraryBrowser, settings, LoginFormViewModel,
                                                                   mixViewModelFactory);

            this.PlaybackViewModel = new PlaybackViewModel(playbackController, settings, mixViewModelFactory);


            new TrackReporter(playbackController.CurrentMixObservable,
                                                   playbackController.CurrentPositionObservable);
        }

        public ReactiveAsyncCommand NextSong { get; protected set; }

        public MediaBrowserViewModel MediaBrowserViewModel { get; protected set; }
        public LoginFormViewModel LoginFormViewModel { get; private set; }

        public PlaybackViewModel PlaybackViewModel { get; private set; }
    }
}