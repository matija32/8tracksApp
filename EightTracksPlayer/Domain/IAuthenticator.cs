using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.Domain
{
    public interface IAuthenticator
    {
        UserData UserData { get; }
        bool TryLoginFromStoredCredentials();
        bool Login(string username, string password);
        bool Login(string username, string password, bool shouldRememberLoginData);
        void Logout();
    }
}