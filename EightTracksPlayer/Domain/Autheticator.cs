using System;
using EightTracksPlayer.Communication;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;
using log4net;

namespace EightTracksPlayer.Domain
{
    public class Authenticator : IAuthenticator
    {
        private static readonly ILog log = LogManager.GetLogger(
          System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); private string passwordKey = "8t_password";
        private RequestExecutionAdapter requestExecutor;
        private RequestFactory requestFactory = RequestFactory.Instance;
        private Settings settings;

        private UserData userData = UserData.NoUser;
        private string usernameKey = "8t_username";

        public Authenticator(IRequestExecutor requestExecutor, Settings settings)
        {
            this.requestExecutor = new RequestExecutionAdapter(requestExecutor);
            this.settings = settings;
        }

        public Authenticator(IRequestExecutor requestExecutor)
            : this(requestExecutor, new Settings())
        {
        }

        public Authenticator() : this(new HttpRequestExecutor())
        {
        }

        #region IAuthenticator Members

        public bool Login(string username, string password, bool shouldRememberLoginData)
        {
            if (String.IsNullOrEmpty(username) || String.IsNullOrEmpty(password)) return false;

            LoginRequest loginRequest = requestFactory.CreateLoginRequest(username, password);

            LoginResponse loginResponse = requestExecutor.ExecuteLoginRequest(loginRequest);

            if (loginResponse.LoggedIn)
            {
                this.userData = new UserData(
                    username,
                    loginResponse.CurrentUserElement.Slug,
                    loginResponse.CurrentUserElement.Login,
                    loginResponse.UserToken);

                if (shouldRememberLoginData)
                {
                    settings[usernameKey] = username;
                    settings[passwordKey] = password;
                }
                log.Info(String.Format("User {0} is now logged in.", username));
            }
            else
            {
                log.Error(String.Format("Unable to log in the user {0}. Error received: {1}", username,
                             loginResponse.ErrorsElement.Value));
            }

            return loginResponse.LoggedIn;
        }

        public bool TryLoginFromStoredCredentials()
        {
            string username = (string) settings[usernameKey];
            string password = (string) settings[passwordKey];
            return Login(username, password);
        }

        public bool Login(string username, string password)
        {
            return Login(username, password, false);
        }

        public void Logout()
        {
            log.Info(String.Format("User {0} is now logged out.", userData.Username));
            
            userData = UserData.NoUser;
            settings[usernameKey] = String.Empty;
            settings[passwordKey] = String.Empty;
        }

        public UserData UserData
        {
            get { return userData; }
        }

        #endregion
    }
}