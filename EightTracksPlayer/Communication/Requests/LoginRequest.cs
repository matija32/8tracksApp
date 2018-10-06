using System;
using System.IO;
using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class LoginRequest : RequestBase
    {
        private readonly HttpWebRequest httpWebRequest;
        private readonly string password;
        private readonly string username;

        public LoginRequest(string host, string apiKey, string username, string password)
            : base(host, apiKey)
        {
            this.password = password;
            this.username = username;
            Url = String.Format(@"{0}sessions.xml?api_key={1}", BaseUri, ApiKey);

            this.httpWebRequest = CreateHttpWebRequest();
        }

        public string Password
        {
            get { return password; }
        }

        public string Username
        {
            get { return username; }
        }


        private HttpWebRequest CreateHttpWebRequest()
        {
            string credentials = String.Format(@"login={0}&password={1}", Username, Password);

            HttpWebRequest webRequest = (HttpWebRequest) WebRequest.Create(Url);

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(credentials);
            webRequest.Method = "POST";

            webRequest.ContentLength = buffer.Length;
            webRequest.Timeout = 30000;
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