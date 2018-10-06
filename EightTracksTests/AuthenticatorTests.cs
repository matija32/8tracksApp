using System;
using System.Concurrency;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Responses.SupportingElements;
using EightTracksPlayer.Domain;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using EightTracksTests.Stubs;
using EightTracksTests.TestUtils;
using NUnit.Framework;
using ReactiveUI;
using StoryQ;
using StoryQ.Formatting.Parameters;

namespace EightTracksTests
{
    [TestFixture]
    public class AuthenticatorTests
    {

        private const string domainTag = Tags.Domain;
        private const string authenticationTag = Tags.Authentication;

        private Feature authenticateAnUser = Features.AuthenticatingAnUser;

        private IAuthenticator authenticator;
        private IScheduler origSched;
        private RequestExecutorStub requestExecutor;

        private string userToken = "aUserToken";
        private LoginResponse successfulLoginResponse;
        private LoginResponse unsucessfulLoginResponse;
        private string username;

        public AuthenticatorTests()
        {
            CurrentUserElement userElement = new CurrentUserElement()
                                                 {
                                                     Login = "aLogin"
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
            origSched = RxApp.DeferredScheduler;
            RxApp.DeferredScheduler = new TestScheduler();
            username = "aUsername";
            requestExecutor = new RequestExecutorStub();
            authenticator = new Authenticator(requestExecutor, new SettingsStub());
        }

        [TearDown]
        public void TearDown()
        {
            RxApp.DeferredScheduler = origSched;
        }

        [Test]
        public void TestLogsInTheUserWithTheValidCredentials()
        {
            authenticateAnUser
                .WithScenario("logging in a user with valid user credentials").Tag(domainTag).Tag(authenticationTag)
                .Given(TheUserProvidesCorrectUsernameAndPassword)
                .When(TheApplicationIssuesACommandToLogInTheUser)
                .Then(TheAuthenticatorExecutesALoginRequest)
                    .And(TheUser_LoggedIn, true)
                    .And(TheUserTokenIs_, userToken)
                .ExecuteWithReport();
        }

        private void TheAuthenticatorExecutesALoginRequest()
        {
            Assert.That(requestExecutor.Requests[0], Is.TypeOf(typeof(LoginRequest)));
        }

        [Test]
        public void TestDoesNotLogInTheUserWithInvalidCredentials()
        {
            authenticateAnUser
                 .WithScenario("logging in a user with invalid user credentials").Tag(domainTag).Tag(authenticationTag)
                 .Given(TheUserProvidesInorrectUsernameAndPassword)
                 .When(TheApplicationIssuesACommandToLogInTheUser)
                 .Then(TheAuthenticatorExecutesALoginRequest)
                    .And(TheUser_LoggedIn, false)
                    .And(TheUserTokenDoesNotExist)

                 .WithScenario("logging in a user with no credentials").Tag(domainTag).Tag(authenticationTag)
                 .Given(AnyConditions)
                 .When(TheApplicationDoesNotSetThePassword)
                    .And(TheApplicationIssuesACommandToLogInTheUser)
                 .Then(TheUser_LoggedIn, false)
                    .And(TheUserTokenDoesNotExist)
                 .ExecuteWithReport();
        }

        private void TheApplicationDoesNotSetThePassword()
        {
            username = String.Empty;
        }

        private void TheUserTokenIs_(string userToken)
        {
            Assert.That(authenticator.UserData.UserToken, Is.EqualTo(userToken));
        }

        private void TheApplicationIssuesACommandToLogInTheUser()
        {
            authenticator.Login(username, "aPassword");
        }

        private void TheUserTokenDoesNotExist()
        {
            Assert.That(authenticator.UserData.UserToken.Length, Is.EqualTo(0));
        }

        private void TheUser_LoggedIn([BooleanParameterFormat("should be", "should not be")] bool shouldBeLogged)
        {
            Assert.That(authenticator.UserData != UserData.NoUser, Is.EqualTo(shouldBeLogged));
        }

        private void TheUserProvidesInorrectUsernameAndPassword()
        {
            requestExecutor.Responses.Add(unsucessfulLoginResponse);
        }

        private void TheUserProvidesCorrectUsernameAndPassword()
        {
            requestExecutor.Responses.Add(successfulLoginResponse);
        }

        [Test]
        public void TestLogsOutTheUser()
        {
            authenticateAnUser
                .WithScenario("logging out a user").Tag(domainTag).Tag(authenticationTag)
                .Given(AnyConditions)
                .When(TheApplicationIssuesACommandToLogOutTheUser)
                .Then(TheUser_LoggedIn, false)
                    .And(TheUserTokenDoesNotExist)
                .ExecuteWithReport();
        }

        private void TheApplicationIssuesACommandToLogOutTheUser()
        {
            authenticator.Logout();
        }

        private void AnyConditions()
        {
            
        }
    }
}