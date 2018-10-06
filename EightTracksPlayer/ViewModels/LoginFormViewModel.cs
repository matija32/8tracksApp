using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Windows.Controls;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using ReactiveUI;
using ReactiveUI.Xaml;

namespace EightTracksPlayer.ViewModels
{
    public class LoginFormViewModel : ReactiveValidatedObject
    {
        private bool _IsLoginFormExpanded;
        private bool _IsPasswordFieldFocused;
        private bool _IsUsernameFieldFocused;
        private string _PasswordField;
        private ObservableAsPropertyHelper<UserData> _UserData;
        private ObservableAsPropertyHelper<bool> _UserLoggedIn;
        private string _UsernameField;
        private ReplaySubject<UserData> userDataSubject = new ReplaySubject<UserData>(1);

        public LoginFormViewModel()
            : this(new Authenticator())
        {
        }

        public LoginFormViewModel(IAuthenticator authenticator)
        {
            // push the initial values in the observables
            // so that everyone subscribing before the user logs in for the first time
            // sees that the user is not there

            bool initialCredentialsOK = authenticator.TryLoginFromStoredCredentials();
            if (initialCredentialsOK)
            {
                userDataSubject.OnNext(authenticator.UserData);
                UsernameField = authenticator.UserData.Username;
            }
            else
            {
                userDataSubject.OnNext(UserData.NoUser);
                UsernameField = "";
            }


            UserLoggedInObservable = UserDataObservable.Select(x => x != UserData.NoUser);
            _UserLoggedIn = UserLoggedInObservable.ToProperty(this, x => x.UserLoggedIn);

            LogIn = new ReactiveAsyncCommand(UserLoggedInObservable.Select(x => !x), 1);
            LogIn.RegisterAsyncFunction(_ =>
                                            {
                                                bool logicSuccessful = authenticator.Login(UsernameField, PasswordField,
                                                                                           true);

                                                if (logicSuccessful)
                                                {
                                                    //collapse if the login was successfull
                                                    IsLoginFormExpanded = false;
                                                }
                                                else
                                                {
                                                    // if login failed, return the focus to the username field
                                                    IsUsernameFieldFocused = true;
                                                }

                                                return authenticator.UserData;
                                            }).Subscribe(userDataSubject.OnNext);

            LogOut = new ReactiveAsyncCommand(UserLoggedInObservable, 1);
            LogOut.RegisterAsyncFunction(_ =>
                                             {
                                                 authenticator.Logout();
                                                 IsPasswordFieldFocused = true;
                                                 return authenticator.UserData;
                                             }).Subscribe(userDataSubject.OnNext);

            _UserData = UserDataObservable.ToProperty(this, x => x.UserData);
        }


        [Required]
        public bool UserLoggedIn
        {
            get { return _UserLoggedIn.Value; }
        }

        public IObservable<bool> UserLoggedInObservable { get; private set; }

        [Required]
        public UserData UserData
        {
            get { return _UserData.Value; }
        }

        public IObservable<UserData> UserDataObservable
        {
            get { return userDataSubject; }
        }


        [Required]
        public string UsernameField
        {
            get { return _UsernameField; }
            set { this.RaiseAndSetIfChanged(x => x.UsernameField, value); }
        }

        [Required]
        public bool IsUsernameFieldFocused
        {
            get { return _IsUsernameFieldFocused; }
            set { this.RaiseAndSetIfChanged(x => x.IsUsernameFieldFocused, value); }
        }

        [Required]
        public bool IsPasswordFieldFocused
        {
            get { return _IsPasswordFieldFocused; }
            set { this.RaiseAndSetIfChanged(x => x.IsPasswordFieldFocused, value); }
        }

        [Required]
        public string PasswordField
        {
            get { return _PasswordField; }
            set { this.RaiseAndSetIfChanged(x => x.PasswordField, value); }
        }

        [Required]
        public bool IsLoginFormExpanded
        {
            get { return _IsLoginFormExpanded; }
            set
            {
                if (value) IsUsernameFieldFocused = true;
                this.RaiseAndSetIfChanged(x => x.IsLoginFormExpanded, value);
            }
        }

        public ReactiveAsyncCommand LogIn { get; protected set; }
        public ReactiveAsyncCommand LogOut { get; protected set; }
    }
}