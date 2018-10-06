using System;
using System.Collections.Generic;
using System.Windows.Navigation;
using EightTracksPlayer.Utils.Browsing;

namespace EightTracksTests.Stubs
{
    public class WebAccessProxyStub : IWebAccessProxy
    {
        private ReplaySubject<FileLocation> fileLocationObservable = new ReplaySubject<FileLocation>(1);

        public WebAccessProxyStub()
        {
            ResetFileLocation();
        }

        public void Download(string sourceUri, string destinationUri, string performer, string title)
        {
            SourceUri = sourceUri;
            DestinationUri = destinationUri;
        }

        public string DestinationUri { get; set; }
        public string SourceUri { get; set; }

        public void OpenWebBrowser(object sender, RequestNavigateEventArgs e)
        {
            
        }

        public EventHandler<RequestNavigateEventArgs> RequestNavigateHandler
        {
            get { throw new NotImplementedException(); }
        }

        public void ResetFileLocation()
        {
            fileLocationObservable.OnNext(FileLocation.Online);
        }

        public IObservable<FileLocation> FileLocationObservable
        {
            get { return fileLocationObservable; }
        }

        public void PushNewFileLocation(FileLocation fileLocation)
        {
            fileLocationObservable.OnNext(fileLocation);
        }

        public object Clone()
        {
            return new WebAccessProxyStub();
        }
    }
}