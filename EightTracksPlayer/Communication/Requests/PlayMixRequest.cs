using System;
using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class PlayMixRequest : RequestBase
    {
        private readonly int mixId;
        private readonly string playToken;

        public PlayMixRequest(string host, string apiKey, string playToken, int mixId, string userToken)
            : this(host, apiKey, playToken, mixId)
        {
            Url += "&user_token=" + userToken;
        }

        public PlayMixRequest(string host, string apiKey, string playToken, int mixId) : base(host, apiKey)
        {
            this.playToken = playToken;
            this.mixId = mixId;
            Url = String.Format(@"{0}sets/{1}/play.xml?mix_id={2}&api_key={3}", BaseUri, PlayToken, MixId, ApiKey);
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