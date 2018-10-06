using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public interface IRequest
    {
        string BaseUri { get; }

        string ApiKey { get; }

        string Url { get; }

        HttpWebRequest GetHttpWebRequest();
    }
}