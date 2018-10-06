using System;
using System.Linq;
using System.Net;
using EightTracksPlayer.Domain.Entities;
using EightTracksPlayer.Utils;

namespace EightTracksPlayer.Communication.Requests
{
    public class MixesRequest : PaginatedRequest
    {
        private readonly string userSlug;
        private readonly string userToken;
        private readonly MixesViewType viewType;
        private MixFilter mixFilter;

        public MixesRequest(string host, string apiKey, string userToken, MixesViewType view, string userSlug,
                            int pageNumber, int pageSize, MixFilter mixFilter)
            : base(host, apiKey, pageNumber, pageSize)
        {
            this.userToken = userToken;
            this.viewType = view;
            this.userSlug = userSlug;
            this.mixFilter = mixFilter;

            if (view == MixesViewType.Feed || view == MixesViewType.Liked)
            {
                Url = String.Format("{0}users/{1}/mixes.xml?api_key={2}&user_token={3}&view={4}", BaseUri, userSlug,
                                    ApiKey, userToken, view.ToString().ToLower());
            }
            else if (view == MixesViewType.Listened || view == MixesViewType.Recommended)
            {
                Url = String.Format("{0}mix_sets/{1}.xml?include=mixes&api_key={2}&user_token={3}", BaseUri, view.ToString().ToLower(),
                                    ApiKey, userToken);
            }
            else
            {
                Url = String.Format("{0}mixes.xml?api_key={1}&sort={2}", BaseUri, ApiKey, view.ToString().ToLower());
                if (userToken.Length > 0)
                {
                    Url += "&user_token=" + userToken;
                }
            }

            Url += "&per_page=" + pageSize;
            Url += "&page=" + pageNumber;

            if (mixFilter.Tags.Count > 0)
            {
                string tags = StringUtils.TagListToString(mixFilter.Tags, "%2B");
                Url += "&tags=" + tags;
            }
            if (mixFilter.Query.Length > 0)
            {
                Url += "&q=" + mixFilter.Query;
            }
        }

        public MixFilter MixFilter
        {
            get { return mixFilter; }
        }

        public MixesViewType ViewType
        {
            get { return viewType; }
        }

        public override HttpWebRequest GetHttpWebRequest()
        {
            return (HttpWebRequest) WebRequest.Create(Url);
        }

        public override PaginatedRequest GetNextPageRequest()
        {
            return new MixesRequest(BaseUri, ApiKey, userToken, viewType, userSlug, PageNumber + 1, PageSize, MixFilter);
        }
    }
}