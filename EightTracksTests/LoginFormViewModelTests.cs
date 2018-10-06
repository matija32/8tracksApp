using System;
using System.Collections.Generic;
using System.Concurrency;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.ViewModels;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using ReactiveUI;
using ReactiveUI.Testing;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class LoginFormViewModelTests
    {

        private const string applicationTag = Tags.Application;
        private Feature authenticateAnUser = Features.AuthenticatingAnUser;
        private Authenticator authenticator;
        private ManualResetEvent lockObject;
        //private IScheduler origSched;
        //private TestScheduler testScheduler;
        private LoginFormViewModel loginFormViewModel;
        private RequestExecutorStub requestExecutor;
        private LoginResponse successfulLoginResponse;
        private LoginResponse unsucessfulLoginResponse;
        private string userToken = "aUserToken";
        private string userSlug = "userSlug";
        private string displayName = "aDisplayName";
        private string authenticationTag = Tags.Authentication;

        public LoginFormViewModelTests()
        {
            CurrentUserElement userElement = new CurrentUserElement()
            {
                Login = displayName,
                Slug = userSlug
            };
            successfulLoginResponse = new LoginResponse()
            {
                LoggedIn = true,
                UserToken = userToken,
                CurrentUserElement = userElement
            };

            unsucessfulLoginResponse = new LoginResponse()
            {
                LoggedIn = false,
                CurrentUserElement = userElement
            };
        }

        [SetUp]
        public void SetUp()
        {
            //testScheduler = new TestScheduler();
            //origSched = RxApp.DeferredScheduler;
            //RxApp.DeferredScheduler = testScheduler;
            
            lockObject = new ManualResetEvent(false);
            requestExecutor = new RequestExecutorStub();
            // because it'll try to invoke it in the beginning, while loading the credentials from the registry
            requestExecutor.Responses.Add(unsucessfulLoginResponse);

            authenticator = new Authenticator(requestExecutor, new SettingsStub());
            loginFormViewModel = new LoginFormViewModel(authenticator);
            loginFormViewModel.UsernameField = "username";
            loginFormViewModel.PasswordField = "password";
        }

        [TearDown]
        public void TearDown()
        {
            //RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestLogsInTheUserWithValidCredentials()
        {
            
            loginFormViewModel.LogIn.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            authenticateAnUser
                .WithScenario("logging in with valid user credentials").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheUser_CurrentlyLoggedIn, false)
                    .And(TheCredentialsTheUserEntered_Valid, true)
                .When(TheUserIssuesAComandToLogIn)
                    .And(TheCommandIsProcessed, true)
                .Then(TheUser_LoggedInTheSystem, true)
                    .And(TheLoginForm_Expanded, false)
                    .And(TheUserIdentifier_ShownOnTheLoginForm, true)
                .ExecuteWithReport();
        }

        private void TheCommandIsProcessed([Silent] bool isCommandAsync)
        {
            if (isCommandAsync) lockObject.WaitOne();
            Thread.Sleep(1000);
        }

        private void TheUserIdentifier_ShownOnTheLoginForm([BooleanParameterFormat("should be", "sould not be")] bool isIdentifierShown)
        {
            if (isIdentifierShown)
            {
                Assert.That(loginFormViewModel.UserData.UserSlug, Is.EqualTo(userSlug));    
            }
            else
            {
                Assert.That(loginFormViewModel.UserData.UserSlug, Is.Not.StringMatching(userSlug));
            }
            
        }

        private void TheLoginForm_Expanded([BooleanParameterFormat("should be", "sould not be")] bool isFormExpanded)
        {
            Assert.That(loginFormViewModel.IsLoginFormExpanded, Is.EqualTo(isFormExpanded));
        }

        private void TheUser_LoggedInTheSystem([BooleanParameterFormat("should be", "sould not be")] bool isLoggedIn)
        {
            Assert.That(loginFormViewModel.UserLoggedIn, Is.EqualTo(isLoggedIn));
        }

        private void TheUserIssuesAComandToLogIn()
        {
            loginFormViewModel.LogIn.Execute(null);
        }

        private void TheCredentialsTheUserEntered_Valid([BooleanParameterFormat("are", "are not")] bool areCredentialsValid)
        {
            requestExecutor.Responses.Add(areCredentialsValid ? successfulLoginResponse : unsucessfulLoginResponse);
        }

        private void TheUser_CurrentlyLoggedIn([BooleanParameterFormat("is", "is not")] bool isCurrentlyLoggedIn)
        {
            loginFormViewModel.IsLoginFormExpanded = !isCurrentlyLoggedIn;

            ((ReplaySubject<UserData>)loginFormViewModel.UserDataObservable).OnNext(isCurrentlyLoggedIn
                                                                             ? new UserData("aUsername", userSlug, displayName, userToken)
                                                                             : UserData.NoUser);
            
        }

        [Test]
        public void TestDoesNotLogInTheUserWithInvalidCredentials()
        {
            //waiting for the second "not logged in", the first one is in Given, the 
            loginFormViewModel.LogIn.AsyncCompletedNotification.Subscribe(_ => lockObject.Set());
            
            authenticateAnUser
                .WithScenario("logging in with invalid user credentials").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheUser_CurrentlyLoggedIn, false)
                    .And(TheCredentialsTheUserEntered_Valid, false)
                .When(TheUserIssuesAComandToLogIn)
                    .And(TheCommandIsProcessed, true)
                .Then(TheUser_LoggedInTheSystem, false)
                    .And(TheLoginForm_Expanded, true)
                    .And(TheUserIdentifier_ShownOnTheLoginForm, false)
                    .And(TheUsernameField_Focused, true)
                .ExecuteWithReport();
        }

        private void TheUsernameField_Focused([BooleanParameterFormat("should be", "shouldn't be")] bool shouldBeFocused)
        {
            Assert.That(loginFormViewModel.IsUsernameFieldFocused, Is.EqualTo(shouldBeFocused));
        }

        [Test]
        public void TestLogsOutTheUser()
        {
            authenticateAnUser
               .WithScenario("logging out").Tag(applicationTag).Tag(authenticationTag)
               .Given(TheUser_CurrentlyLoggedIn, true)
               .When(TheUserIssuesAComandToLogOut)
                   .And(TheCommandIsProcessed, false)
               .Then(TheUser_LoggedInTheSystem, false)
                   .And(TheLoginForm_Expanded, false)
                   .And(TheUserIdentifier_ShownOnTheLoginForm, false)
                   .And(ThePasswordField_Focused, true)
               .ExecuteWithReport();   
        }

        private void ThePasswordField_Focused([BooleanParameterFormat("should be", "shouldn't be")] bool shouldBeFocused)
        {
            Assert.That(loginFormViewModel.IsPasswordFieldFocused, Is.EqualTo(shouldBeFocused));
        }

        private void TheUserIssuesAComandToLogOut()
        {
            loginFormViewModel.LogOut.Execute(null);
        }

        [Test]
        public void TestAllowsLoggingOutOnlyIfTheUserIsLoggedIn()
        {
            authenticateAnUser
                .WithScenario("enable logging in when user is logged out").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheUser_CurrentlyLoggedIn, true)
                .When(TheUserLogs_, false)
                .Then(TheUser_BeAbleToLogIn, true)

                .WithScenario("disable logging in when user is logged in").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheUser_CurrentlyLoggedIn, false)
                .When(TheUserLogs_, true)
                .Then(TheUser_BeAbleToLogIn, false)

                .ExecuteWithReport();

        }

        private void TheUserLogs_([BooleanParameterFormat("in", "out")] bool isGoingToLogIn)
        {
            ((ReplaySubject<UserData>) loginFormViewModel.UserDataObservable).OnNext(isGoingToLogIn
                                                                                         ? new UserData("x", "x", "x","x")
                                                                                         : UserData.NoUser);
        }

        private void TheUser_BeAbleToLogIn([BooleanParameterFormat("will", "will not")] bool canLogIn)
        {
            Assert.That(loginFormViewModel.LogIn.CanExecute(null), Is.EqualTo(canLogIn));
        }

        [Test]
        public void TestAllowsLoggingInOnlyIfTheUserIsLoggedOut()
        {
            authenticateAnUser
                .WithScenario("enable logging out when user is logged in").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheUser_CurrentlyLoggedIn, true)
                .When(TheUserLogs_, true)
                .Then(TheUser_BeAbleToLogOut, true)

                .WithScenario("disable logging out when user is logged out").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheUser_CurrentlyLoggedIn, true)
                .When(TheUserLogs_, false)
                .Then(TheUser_BeAbleToLogOut, false)

                .ExecuteWithReport();

        }

        private void TheUser_BeAbleToLogOut([BooleanParameterFormat("will", "will not")] bool canLogOut)
        {
            Assert.That(loginFormViewModel.LogOut.CanExecute(null), Is.EqualTo(canLogOut));
        }

        [Test]
        public void TestFocusesOnUsernameFieldUponExpansion()
        {
            authenticateAnUser
                .WithScenario("seeting the form focus to username field upon expansion").Tag(applicationTag).Tag(authenticationTag)
                .Given(TheForm_CurrentlyExpanded, false)
                .When(TheUserExpandsTheForm)
                .Then(TheFocus_SetOnTheUsernameField, true)
                .ExecuteWithReport();
        }

        private void TheFocus_SetOnTheUsernameField([BooleanParameterFormat("should be", "shouldn't be")] bool shouldBeFocused)
        {
            Assert.That(loginFormViewModel.IsUsernameFieldFocused, Is.EqualTo(shouldBeFocused));
        }

        private void TheUserExpandsTheForm()
        {
            loginFormViewModel.IsLoginFormExpanded = true;
        }

        private void TheForm_CurrentlyExpanded([BooleanParameterFormat("is", "is not")] bool isCurrentlyExpanded)
        {
            loginFormViewModel.IsLoginFormExpanded = false;
        }
    }
}