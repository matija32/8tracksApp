using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public abstract class RequestBase : IRequest
    {
        private string apiKey;
        private string baseUri;

        protected RequestBase(string baseUri, string apiKey)
        {
            this.baseUri = baseUri;
            this.apiKey = apiKey;
        }

        #region IRequest Members

        public string BaseUri
        {
            get { return baseUri; }
        }

        public string ApiKey
        {
            get { return apiKey; }
        }

        public string Url { get; protected set; }

        public abstract HttpWebRequest GetHttpWebRequest();

        #endregion
    }
}