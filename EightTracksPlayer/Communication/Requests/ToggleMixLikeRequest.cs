using System;
using System.IO;
using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class ToggleMixLikeRequest : RequestBase
    {
        private readonly int mixId;
        private readonly string userToken;
        private HttpWebRequest httpWebRequest;

        public ToggleMixLikeRequest(string baseUri, string apiKey, string userToken, int mixId) : base(baseUri, apiKey)
        {
            this.userToken = userToken;
            this.mixId = mixId;
            Url = String.Format(@"{0}mixes/{1}/toggle_like.xml?user_token={2}&api_key={3}", BaseUri, MixId, UserToken,
                                ApiKey);
            this.httpWebRequest = CreateHttpWebRequest();
        }

        public string UserToken
        {
            get { return userToken; }
        }

        public int MixId
        {
            get { return mixId; }
        }

        private HttpWebRequest CreateHttpWebRequest()
        {
            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(Url);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(String.Empty);
            webRequest.Method = "POST";

            webRequest.Timeout = 30000;
            webRequest.ContentLength = buffer.Length;
            Stream writer = webRequest.GetRequestStream();
            writer.Write(buffer, 0, buffer.Length);
            writer.Close();

            return webRequest;
        }

        public override HttpWebRequest GetHttpWebRequest()
        {
            return httpWebRequest;
        }
    }
}