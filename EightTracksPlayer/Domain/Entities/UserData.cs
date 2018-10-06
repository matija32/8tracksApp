using System;

namespace EightTracksPlayer.Domain.Entities
{
    public class UserData
    {
        private static readonly UserData noUser = new UserData(String.Empty, String.Empty, String.Empty, String.Empty);

        public UserData(string username, string userSlug, string displayName, string userToken)
        {
            Username = username;
            UserSlug = userSlug;
            DisplayName = displayName;
            UserToken = userToken;
        }

        public string Username { get; private set; }
        public string UserSlug { get; private set; }
        public string DisplayName { get; private set; }
        public string UserToken { get; private set; }

        public static UserData NoUser
        {
            get { return noUser; }
        }
    }
}