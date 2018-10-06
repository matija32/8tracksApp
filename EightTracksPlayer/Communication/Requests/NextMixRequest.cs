using System;
using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class NextMixRequest : RequestBase
    {
        private readonly int mixId;
        private readonly long mixSetId;
        private readonly string playToken;

        public NextMixRequest(string baseUri, string apiKey, int mixId, long mixSetId, string playToken)
            : base(baseUri, apiKey)
        {
            this.mixId = mixId;
            this.mixSetId = mixSetId;
            this.playToken = playToken;
            Url = String.Format(@"{0}sets/{1}/next_mix.xml?mix_id={2}&api_key={3}&mix_set_id={4}", BaseUri, PlayToken,
                                MixId, ApiKey, MixSetId);
        }

        public int MixId
        {
            get { return mixId; }
        }

        public long MixSetId
        {
            get { return mixSetId; }
        }

        public string PlayToken
        {
            get { return playToken; }
        }

        public override HttpWebRequest GetHttpWebRequest()
        {
            return (HttpWebRequest) WebRequest.Create(Url);
        }
    }
}