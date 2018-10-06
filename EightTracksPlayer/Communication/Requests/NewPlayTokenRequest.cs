using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class NewPlayTokenRequest : RequestBase
    {
        public NewPlayTokenRequest(string host, string apiKey)
            : base(host, apiKey)
        {
            Url = BaseUri + @"sets/new.xml?api_key=" + ApiKey;
        }

        public override HttpWebRequest GetHttpWebRequest()
        {
            return (HttpWebRequest) WebRequest.Create(Url);
        }
    }
}