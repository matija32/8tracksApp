using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using EightTracksPlayer.Communication;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.Domain
{
    public class MediaLibraryBrowser : IMediaLibraryBrowser
    {
        private RequestExecutionAdapter requestExecutor;
        private RequestFactory requestFactory = RequestFactory.Instance;
        private BrowsingSessionData sessionData = new BrowsingSessionData();

        public MediaLibraryBrowser()
            : this(new HttpRequestExecutor())
        {
        }

        public MediaLibraryBrowser(IRequestExecutor requestExecutor)
        {
            this.requestExecutor = new RequestExecutionAdapter(requestExecutor);
        }

        #region IMediaLibraryBrowser Members

        public List<Mix> GetRecentlyMadeMixes(MixFilter mixFilter)
        {
            return GetMixes(mixFilter, MixesViewType.Recent, 1);
        }

        public List<Mix> GetHotMixes(MixFilter mixFilter)
        {
            return GetMixes(mixFilter, MixesViewType.Hot, 1);
        }

        public List<Mix> GetPopularMixes(MixFilter mixFilter)
        {
            return GetMixes(mixFilter, MixesViewType.Popular, 1);
        }

        public List<Mix> GetLikedMixes(MixFilter mixFilter, string userSlug)
        {
            return GetMixes(mixFilter, MixesViewType.Liked, 1, userSlug);
        }

        public List<Mix> GetFeedMixes(MixFilter mixFilter, string userSlug)
        {
            return GetMixes(mixFilter, MixesViewType.Feed, 1, userSlug);
        }

        public List<Mix> GetHistoryMixes(MixFilter mixFilter, string userSlug)
        {
            return GetMixes(mixFilter, MixesViewType.Listened, 1, userSlug);
        }

        public List<Mix> GetRecommendedMixes(MixFilter mixFilter, string userSlug)
        {
            return GetMixes(mixFilter, MixesViewType.Recommended, 1, userSlug);
        }

        public List<Mix> GetMoreMixes()
        {
            if (sessionData.IsEmpty) return new List<Mix>();

            MixesRequest mixesRequest = requestFactory.CreateMoreMixesRequest(sessionData.LastMixesRequest);
            MixesResponse mixesResponse = requestExecutor.ExecuteMixesRequest(mixesRequest);

            sessionData.Update(mixesRequest, mixesResponse);

            return ExtractMixes(mixesResponse);
        }


        public string UserToken
        {
            get { return requestFactory.UserToken; }
            set
            {
                requestFactory.UserToken = value;
            }
        }

        public int PageSize
        {
            get { return requestFactory.PageSize; }
            set { requestFactory.PageSize = value; }
        }

        public void ToggleLike(Mix mix)
        {
            ToggleMixLikeRequest toggleMixLikeRequest = requestFactory.CreateToggleMixLikeRequest(mix.MixId);
            ToggleMixLikeResponse toggleMixLikeReponse =
                requestExecutor.ExecuteToggleMixLikeRequest(toggleMixLikeRequest);
            if (toggleMixLikeReponse.LoggedIn)
            {
                mix.LikedByCurrentUser = toggleMixLikeReponse.Mix.LikedByCurrentUser;
            }
        }

        public BrowsingSessionData SessionData
        {
            get { return sessionData; }
        }

        #endregion

        private List<Mix> ExtractMixes(MixesResponse mixesResponse)
        {
            return mixesResponse.MixesElement
                .Select(mixElement => new Mix(mixElement, mixesResponse.Id)).ToList();
        }

        private List<Mix> GetMixes(MixFilter mixFilter, MixesViewType viewType, int pageNumber, string userSlug)
        {
            MixesRequest mixesRequest = requestFactory.CreateMixesRequest(mixFilter, viewType, pageNumber, userSlug);
            MixesResponse mixesResponse = requestExecutor.ExecuteMixesRequest(mixesRequest);

            sessionData.Update(mixesRequest, mixesResponse);

            return ExtractMixes(mixesResponse);
        }


        private List<Mix> GetMixes(MixFilter mixFilter, MixesViewType viewType, int pageNumber)
        {
            return GetMixes(mixFilter, viewType, pageNumber, String.Empty);
        }
    }
}