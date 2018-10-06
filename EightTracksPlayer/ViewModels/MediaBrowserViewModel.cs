using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reactive;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using EightTracksPlayer.ViewModels.Factories;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace EightTracksPlayer.ViewModels
{
    public class MediaBrowserViewModel : ReactiveValidatedObject
    {
        private DispatchedReactiveCollection<MixViewModel> _BrowsedMixes; 
        private string _Filter;
        private string _MixBrowsingMessage;
        private MixDisplayMode _MixDisplayMode;
        private string _MixesBatchSize;
        private string _MixesViewTypeAsString;
        private IMediaLibraryBrowser mediaLibraryBrowser;
        private IMixViewModelFactory mixViewModelFactory;
        private string mixdisplayModeKey = "8t_mixDisplayMode";
        private Settings settings;

        public MediaBrowserViewModel(LoginFormViewModel loginFormViewModel, IMixViewModelFactory mixViewModelFactory)
            : this(new MediaLibraryBrowser(), new Settings(), loginFormViewModel, mixViewModelFactory)
        {
        }

        public MediaBrowserViewModel(IMediaLibraryBrowser mediaLibraryBrowser, Settings settings,
                                     LoginFormViewModel loginFormViewModel, IMixViewModelFactory mixViewModelFactory)
        {
            this.settings = settings;
            this.mixViewModelFactory = mixViewModelFactory;
            this.mediaLibraryBrowser = mediaLibraryBrowser;

            GetRecentlyMadeMixes = new ReactiveAsyncCommand(null, 1);
            GetRecentlyMadeMixes.RegisterAsyncFunction(
                _ => mediaLibraryBrowser.GetRecentlyMadeMixes(CreateMixFilter(Filter)))
                .Subscribe(SetBrowsedMixes);

            GetHotMixes = new ReactiveAsyncCommand(null, 1);
            GetHotMixes.RegisterAsyncFunction(_ => mediaLibraryBrowser.GetHotMixes(CreateMixFilter(Filter))).Subscribe(
                SetBrowsedMixes);

            GetPopularMixes = new ReactiveAsyncCommand(null, 1);
            GetPopularMixes.RegisterAsyncFunction(_ => mediaLibraryBrowser.GetPopularMixes(CreateMixFilter(Filter)))
                .Subscribe(SetBrowsedMixes);

            // command is enabled only if the user is logged in
            GetLikedMixes = new ReactiveAsyncCommand(loginFormViewModel.UserLoggedInObservable, 1);
            GetLikedMixes.RegisterAsyncFunction(_ =>
                                                    {
                                                        var mixFilter = CreateMixFilter(Filter);
                                                        string userSlug = loginFormViewModel.UserData.UserSlug;
                                                        return mediaLibraryBrowser.GetLikedMixes(mixFilter, userSlug);
                                                    }).Subscribe(SetBrowsedMixes);

            // command is enabled only if the user is logged in
            GetFeedMixes = new ReactiveAsyncCommand(loginFormViewModel.UserLoggedInObservable, 1);
            GetFeedMixes.RegisterAsyncFunction(_ =>
                                                   {
                                                       var mixFilter = CreateMixFilter(Filter);
                                                       string userSlug = loginFormViewModel.UserData.UserSlug;
                                                       return mediaLibraryBrowser.GetFeedMixes(mixFilter, userSlug);
                                                   }).Subscribe(SetBrowsedMixes);

            GetRecommendedMixes = new ReactiveAsyncCommand(loginFormViewModel.UserLoggedInObservable, 1);
            GetRecommendedMixes.RegisterAsyncFunction(_ =>
                                                    {
                                                        var mixFilter = CreateMixFilter(Filter);
                                                        string userSlug = loginFormViewModel.UserData.UserSlug;
                                                        return mediaLibraryBrowser.GetRecommendedMixes(mixFilter, userSlug);
                                                    }).Subscribe(SetBrowsedMixes);

            GetHistoryMixes = new ReactiveAsyncCommand(loginFormViewModel.UserLoggedInObservable, 1);
            GetHistoryMixes.RegisterAsyncFunction(_ =>
                                                    {
                                                        var mixFilter = CreateMixFilter(Filter);
                                                        string userSlug = loginFormViewModel.UserData.UserSlug;
                                                        return mediaLibraryBrowser.GetHistoryMixes(mixFilter, userSlug);
                                                    }).Subscribe(SetBrowsedMixes);

            
            BrowsedMixes = new DispatchedReactiveCollection<MixViewModel>();

            ReplaySubject<bool> browsedMixesExistObservable = new ReplaySubject<bool>(1);
            BrowsedMixes.CollectionCountChanged.Select(count => count > 0).Subscribe(browsedMixesExistObservable.OnNext);
            browsedMixesExistObservable.OnNext(false);

            GetMoreMixes = new ReactiveAsyncCommand(browsedMixesExistObservable, 1);
            GetMoreMixes.RegisterAsyncFunction(_ => mediaLibraryBrowser.GetMoreMixes())
                .Subscribe(AddBrowsedMixes);

            // set can execute only if there is something in the list
            SetMixDisplayMode = new ReactiveCommand(browsedMixesExistObservable);
            SetMixDisplayMode.Subscribe(mode => MixDisplayMode = (MixDisplayMode) mode);


            ApplyFilter = new ReactiveCommand();
            ApplyFilter.Where(_ => Filter.Length > 0)
                .Subscribe(_ =>
                               {
                                   // if we only started up the application, by search for Hot mixes
                                   if (mediaLibraryBrowser.SessionData.IsEmpty)
                                   {
                                       GetHotMixes.Execute(null);
                                       return;
                                   }

                                   // otherwise, take the same type of mixes as they were before
                                   switch (mediaLibraryBrowser.SessionData.LastMixesRequest.ViewType)
                                   {
                                       case MixesViewType.Hot:      GetHotMixes.Execute(null); break;
                                       case MixesViewType.Popular:  GetPopularMixes.Execute(null); break;
                                       case MixesViewType.Recent:   GetRecentlyMadeMixes.Execute(null); break;
                                       case MixesViewType.Liked:    GetLikedMixes.Execute(null); break;
                                       case MixesViewType.Feed:     GetFeedMixes.Execute(null); break;
                                       case MixesViewType.Listened: GetHistoryMixes.Execute(null); break;
                                       case MixesViewType.Recommended: GetRecommendedMixes.Execute(null); break;
                                       default:
                                           // throw new ArgumentOutOfRangeException();
                                           break;
                                   }
                               });
            

            MixDisplayMode mixDisplayMode;
            string storedMixDisplayModeString = (string) settings[mixdisplayModeKey];
            Enum.TryParse<MixDisplayMode>(storedMixDisplayModeString, out mixDisplayMode);
            MixDisplayMode = mixDisplayMode;

            MixesViewTypeAsString = String.Empty;
            Filter = String.Empty;
            MixBrowsingMessage = String.Empty;
        }


        public ReactiveAsyncCommand GetRecentlyMadeMixes { get; protected set; }
        public ReactiveAsyncCommand GetPopularMixes { get; protected set; }
        public ReactiveAsyncCommand GetHotMixes { get; protected set; }
        public ReactiveAsyncCommand GetLikedMixes { get; protected set; }
        public ReactiveAsyncCommand GetFeedMixes { get; protected set; }
        public ReactiveAsyncCommand GetRecommendedMixes { get; protected set; }
        public ReactiveAsyncCommand GetHistoryMixes { get; protected set; }
        public ReactiveAsyncCommand GetMoreMixes { get; protected set; }
        public IReactiveCommand ApplyFilter { get; protected set; }
        public IReactiveCommand SetMixDisplayMode { get; protected set; }

        public DispatchedReactiveCollection<MixViewModel> BrowsedMixes
        {
            get { return _BrowsedMixes; }
            set { this.RaiseAndSetIfChanged(x => x.BrowsedMixes, value); }
        }

        public MixDisplayMode MixDisplayMode
        {
            get { return _MixDisplayMode; }
            set
            {
                settings[mixdisplayModeKey] = value;
                this.RaiseAndSetIfChanged(x => x.MixDisplayMode, value);
            }
        }

        [Required]
        public string MixesBatchSize
        {
            get { return _MixesBatchSize; }
            set
            {
                mediaLibraryBrowser.PageSize = Int32.Parse(value);
                this.RaiseAndSetIfChanged(x => x.MixesBatchSize, value);
            }
        }

        [Required]
        public string MixBrowsingMessage
        {
            get { return _MixBrowsingMessage; }
            set { this.RaiseAndSetIfChanged(x => x.MixBrowsingMessage, value); }
        }

        [Required]
        public string MixesViewTypeAsString
        {
            get { return _MixesViewTypeAsString; }
            set { this.RaiseAndSetIfChanged(x => x.MixesViewTypeAsString, value); }
        }

        [Required]
        public string Filter
        {
            get { return _Filter; }
            set { this.RaiseAndSetIfChanged(x => x.Filter, value); }
        }

        private void SetBrowsedMixes(List<Mix> mixViewModels)
        {
            BrowsedMixes.Clear();
            MixesViewTypeAsString = mediaLibraryBrowser.SessionData.LastMixesRequest.ViewType.ToString();
            AddBrowsedMixes(mixViewModels);
        }

        private void SetBrowserMessage()
        {
            MixBrowsingMessage = String.Format("Displaying {0} of {1} mixes", BrowsedMixes.Count, MixesViewTypeAsString);
            MixFilter mixFilter = mediaLibraryBrowser.SessionData.LastMixesRequest.MixFilter;
            
            // 8tracks service deos not filter history adn recommended
            if (MixesViewTypeAsString.Equals(MixesViewType.Listened.ToString())) return;
            if (MixesViewTypeAsString.Equals(MixesViewType.Recommended.ToString())) return;

            if (mixFilter.Tags.Count > 0)
            {
                MixBrowsingMessage += String.Format(", tagged: {0}", StringUtils.TagListToString(mixFilter.Tags));
            }
            if (mixFilter.Query.Length > 0)
            {
                MixBrowsingMessage += String.Format(", including \"{0}\"", mixFilter.Query);
            }
        }

        private void AddBrowsedMixes(List<Mix> mixViewModels)
        {
            mixViewModels.Select(mixViewModelFactory.CreateMixViewModel).ToObservable().Subscribe(
                mixViewModel => BrowsedMixes.Add(mixViewModel));
            SetBrowserMessage();
        }


        private MixFilter CreateMixFilter(string filterText)
        {
            MixFilter mixFilter = new MixFilter();
            while (true)
            {
                string[] result = StringUtils.GetStringInBetween("[", "]", filterText, false, false);
                if (result[0].Length == 0) break;
                mixFilter.Tags.Add(result[0]);
                filterText = filterText.Replace("[" + result[0] + "]", " ");
            }
            mixFilter.Query = filterText.Replace("  ", " ").Trim();
            return mixFilter;
        }
    }
}