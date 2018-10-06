using System;
using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class NextSongRequest : RequestBase
    {
        private readonly int mixId;
        private readonly bool isUserRequested;
        private readonly string playToken;

        public NextSongRequest(string host, string apiKey, string playToken, int mixId, bool isUserRequested)
            : base(host, apiKey)
        {
            this.playToken = playToken;
            this.mixId = mixId;
            this.isUserRequested = isUserRequested;
            string targetPage = isUserRequested ? "skip" : "next";
            Url = String.Format(@"{0}sets/{1}/{2}.xml?mix_id={3}&api_key={4}", BaseUri, PlayToken, targetPage, MixId, ApiKey);
        }

        protected object IsUserRequested
        {
            get { return isUserRequested; }
        }

        public string PlayToken
        {
            get { return playToken; }
        }

        public int MixId
        {
            get { return mixId; }
        }

        public override HttpWebRequest GetHttpWebRequest()
        {
            return (HttpWebRequest) WebRequest.Create(Url);
        }
    }
}