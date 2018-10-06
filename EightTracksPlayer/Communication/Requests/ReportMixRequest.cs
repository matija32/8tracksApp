using System;
using System.Net;

namespace EightTracksPlayer.Communication.Requests
{
    public class ReportMixRequest : RequestBase
    {
        private readonly int mixId;
        private readonly long trackId;

        public ReportMixRequest(string baseUri, string apiKey, int mixId, long trackId)
            : base(baseUri, apiKey)
        {
            this.mixId = mixId;
            this.trackId = trackId;
            Url = String.Format(@"{0}sets/{1}/report.xml?mix_id={2}&track_id={3}&api_key={4}", BaseUri, MixId, MixId, TrackId, ApiKey);
        }

        public long TrackId
        {
            get { return trackId; }
        }

        public int MixId
        {
            get { return mixId; }
            
        }

        public override HttpWebRequest GetHttpWebRequest()
        {
            return (HttpWebRequest)WebRequest.Create(Url);
        }
    }
}