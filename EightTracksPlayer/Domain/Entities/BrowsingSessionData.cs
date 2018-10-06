using System;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;

namespace EightTracksPlayer.Domain.Entities
{
    public class BrowsingSessionData
    {
        public MixesRequest LastMixesRequest { get; set; }

        public long MixSetId { get; set; }
        public int TotalEntries { get; set; }
        public int TotalPages { get; set; }


        public bool IsEmpty
        {
            get { return LastMixesRequest == null; }
        }

        public void Update(MixesRequest mixesRequest, MixesResponse mixesResponse)
        {
            this.LastMixesRequest = mixesRequest;
            this.MixSetId = mixesResponse.Id;
            this.TotalEntries = mixesResponse.TotalEntries;
            this.TotalPages = mixesResponse.TotalPages;
        }
    }
}