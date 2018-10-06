using System;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using log4net;

namespace EightTracksPlayer.Communication
{
    public class RequestExecutionAdapter : IRequestExecutor
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IRequestExecutor requestExecutor;

        public RequestExecutionAdapter(IRequestExecutor requestExecutor)
        {
            this.requestExecutor = requestExecutor;
        }

        #region IRequestExecutor Members

        public IResponse Execute(IRequest request)
        {
            try
            {
                return requestExecutor.Execute(request);
            }
            catch (Exception e)
            {
                log.Error(String.Format("Error while executing a certain {0}", request.GetType()), e);
                return null;
            }
        }

        #endregion

        public MixesResponse ExecuteMixesRequest(MixesRequest mixesRequest)
        {
            return (MixesResponse) (Execute(mixesRequest) ?? Activator.CreateInstance(typeof (MixesResponse)));
        }

        public NewPlayTokenResponse ExecuteNewPlayTokenRequest(NewPlayTokenRequest newPlayTokenRequest)
        {
            return
                (NewPlayTokenResponse)
                (Execute(newPlayTokenRequest) ?? Activator.CreateInstance(typeof (NewPlayTokenResponse)));
        }

        public PlaySongResponse ExecutePlayMixRequest(PlayMixRequest playMixRequest)
        {
            return (PlaySongResponse) (Execute(playMixRequest) ?? Activator.CreateInstance(typeof (PlaySongResponse)));
        }

        public PlaySongResponse ExecuteNextSongRequest(NextSongRequest nextSongRequest)
        {
            return (PlaySongResponse) (Execute(nextSongRequest) ?? Activator.CreateInstance(typeof (PlaySongResponse)));
        }

        public LoginResponse ExecuteLoginRequest(LoginRequest loginRequest)
        {
            return (LoginResponse) (Execute(loginRequest) ?? Activator.CreateInstance(typeof (LoginResponse)));
        }

        public ToggleMixLikeResponse ExecuteToggleMixLikeRequest(ToggleMixLikeRequest toggleMixLikeRequest)
        {
            return
                (ToggleMixLikeResponse)
                (Execute(toggleMixLikeRequest) ?? Activator.CreateInstance(typeof (MixesResponse)));
        }

        public NextMixResponse ExecuteNextMixRequest(NextMixRequest nextMixRequest)
        {
            return (NextMixResponse) (Execute(nextMixRequest) ?? Activator.CreateInstance(typeof (NextMixResponse)));
        }

        public ResponseBase ExecuteReportMixRequest(IRequest reportMixRequest)
        {
            return (ResponseBase)(Execute(reportMixRequest) ?? Activator.CreateInstance(typeof(ResponseBase)));
        }
    }
}