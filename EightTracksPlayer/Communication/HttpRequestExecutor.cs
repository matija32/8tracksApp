using System;
using System.Configuration;
using System.Net;
using EightTracksPlayer.Communication.Requests;
using EightTracksPlayer.Communication.Responses;
using EightTracksPlayer.Communication.Serialization;
using log4net;
using ReactiveUI;
using ILog = log4net.ILog;

namespace EightTracksPlayer.Communication
{
    public class HttpRequestExecutor : IRequestExecutor, IEnableLogger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private AggregatedResponseDeserializer deserializer = new AggregatedResponseDeserializer();

        #region IRequestExecutor Members

        public IResponse Execute(IRequest request)
        {
            HttpWebRequest httpWebRequest = request.GetHttpWebRequest();
            WebResponse webResponse = null;

            bool isIEProxyUsed = false;
            if (ConfigurationManager.AppSettings["isIEProxyUsed"] != null)
            {
                try
                {
                    isIEProxyUsed = Boolean.Parse(ConfigurationManager.AppSettings["isIEProxyUsed"]);
                }
                catch (Exception)
                {
                }
            }
            try
            {
                if (isIEProxyUsed)
                {
                    httpWebRequest.Proxy = WebRequest.GetSystemWebProxy();
                }
                webResponse = httpWebRequest.GetResponse();
            }
            catch (WebException webException)
            {
                log.Info("Error received while executing a request", webException);
                webResponse = webException.Response;
            }

            return deserializer.Deserialize(request.GetType(), (HttpWebResponse) webResponse);
        }

        #endregion
    }
}