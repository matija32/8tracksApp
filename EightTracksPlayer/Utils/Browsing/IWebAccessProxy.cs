using System;
using System.Windows.Navigation;

namespace EightTracksPlayer.Utils.Browsing
{
    public interface IWebAccessProxy : ICloneable
    {
        EventHandler<RequestNavigateEventArgs> RequestNavigateHandler { get; }
        IObservable<FileLocation> FileLocationObservable { get; }
        void Download(string sourceUri, string destinationUri, string performer, string title);
        void OpenWebBrowser(object sender, RequestNavigateEventArgs e);
        void ResetFileLocation();
    }
}