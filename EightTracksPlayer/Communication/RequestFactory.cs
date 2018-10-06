using System;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Domain.Entities;

namespace EightTracksPlayer.Communication
{
    public class RequestFactory
    {
        private string apiKey = "";
        private string baseUri = "";
        private int pageSize = 20;
        private string userToken = "";
        private static RequestFactory instance = new RequestFactory();

        public RequestFactory(string baseUri = "http://8tracks.com/",
                              string apiKey = "707648e21c23fd8c3521becab247d8e12006162a")
        {
            this.apiKey = apiKey;
            this.baseUri = baseUri;
        }

        public string BaseUri
        {
            get { return baseUri; }
            set { baseUri = value; }
        }

        public string ApiKey
        {
            get { return apiKey; }
            set { apiKey = value; }
        }

        public string UserToken
        {
            get { return userToken; }
            set { userToken = value; }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        public NewPlayTokenRequest CreateNewPlayTokenRequest()
        {
            return new NewPlayTokenRequest(baseUri, apiKey);
        }

        public MixesRequest CreateMixesRequest(MixFilter mixFilter, MixesViewType view, int pageNumber, string userSlug)
        {
            return new MixesRequest(baseUri, apiKey, userToken, view, userSlug, pageNumber, pageSize, mixFilter);
        }

        public MixesRequest CreateMixesRequest(MixFilter mixFilter, MixesViewType view, int pageNumber)
        {
            return new MixesRequest(baseUri, apiKey, userToken, view, String.Empty, pageNumber, pageSize, mixFilter);
        }

        public PlayMixRequest CreatePlayMixRequest(string playToken, int mixId)
        {
            return userToken.Length > 0
                       ? new PlayMixRequest(baseUri, apiKey, playToken, mixId, userToken)
                       : new PlayMixRequest(baseUri, apiKey, playToken, mixId);
        }

        public NextSongRequest CreateNextSongRequest(string playToken, int mixId, bool isUserRequested)
        {
            return new NextSongRequest(baseUri, apiKey, playToken, mixId, isUserRequested);
        }

        public LoginRequest CreateLoginRequest(string username, string password)
        {
            return new LoginRequest(baseUri, apiKey, username, password);
        }

        public ToggleMixLikeRequest CreateToggleMixLikeRequest(int mixId)
        {
            return new ToggleMixLikeRequest(baseUri, apiKey, userToken, mixId);
        }

        public MixesRequest CreateMoreMixesRequest(MixesRequest mixesRequest)
        {
            return (MixesRequest) mixesRequest.GetNextPageRequest();
        }


        public NextMixRequest CreateNextMixRequest(int mixId, long mixSetId, string playToken)
        {
            return new NextMixRequest(baseUri, apiKey, mixId, mixSetId, playToken);
        }

        public IRequest CreateReportMixRequest(int mixId, long trackId)
        {
            return new ReportMixRequest(baseUri, apiKey, mixId, trackId);
        }

        public static RequestFactory Instance
        {
            get { return instance; }
        }

        public void recreateInstance()
        {
            instance = new RequestFactory();
        }
    }
}