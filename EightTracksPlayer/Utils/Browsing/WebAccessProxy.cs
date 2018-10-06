using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows.Navigation;
using log4net;

namespace EightTracksPlayer.Utils.Browsing
{
    public class WebAccessProxy : IWebAccessProxy
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ReplaySubject<FileLocation> fileLocationObservable = new ReplaySubject<FileLocation>(1);
        private WebClient webClient = new WebClient();

        public WebAccessProxy()
        {
            RequestNavigateHandler = this.OpenWebBrowser;
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
            ResetFileLocation();
        }

        #region IWebAccessProxy Members

        public EventHandler<RequestNavigateEventArgs> RequestNavigateHandler { get; private set; }

        public void ResetFileLocation()
        {
            fileLocationObservable.OnNext(FileLocation.Online);
        }

        public IObservable<FileLocation> FileLocationObservable
        {
            get { return fileLocationObservable; }
        }

        public void Download(string sourceUri, string destinationUri, string performer, string title)
        {
            try
            {
                Dictionary<String, String> paramters = new Dictionary<string, string>();

                paramters.Add("destination", destinationUri);
                paramters.Add("performer", performer);
                paramters.Add("title", title);

                fileLocationObservable.OnNext(FileLocation.Downloading);
                webClient.DownloadFileAsync(new Uri(sourceUri), destinationUri, paramters);
            }
            catch (Exception e)
            {
                fileLocationObservable.OnNext(FileLocation.Online);
                log.Error(String.Format("Error received while trying to download a file from {0} to {1}", sourceUri,
                                  destinationUri), e);
            }
        }

        public void OpenWebBrowser(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        public object Clone()
        {
            return new WebAccessProxy();
        }

        #endregion

        private void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {

            if (e.Cancelled || e.Error != null)
            {
                fileLocationObservable.OnNext(FileLocation.Online);
                log.Error(String.Format("Error received while trying to download a file", e.Error));
            }

            fileLocationObservable.OnNext(FileLocation.Downloaded);

            Dictionary<String, String> paramters = (Dictionary<string, string>) e.UserState;

            SetID3Tags(paramters);

        }

        private void SetID3Tags(Dictionary<string, string> paramters)
        {
            try
            {
                TagLib.File f = TagLib.File.Create(paramters["destination"]);
                f.Tag.Performers = new String[] { paramters["performer"] };
                f.Tag.Title = paramters["title"];
                f.Save();
            }
            catch (Exception e)
            {
                log.Error(String.Format("Error received while trying set ID3 tags for downloaded file at {0}", paramters["destination"]), e);
            }

        }
    }
}