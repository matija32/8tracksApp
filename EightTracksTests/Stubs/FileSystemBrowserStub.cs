using System;
using EightTracksPlayer.Utils.Browsing;

namespace EightTracksTests.Stubs
{
    public class FileSystemBrowserStub : IFileSystemBrowser
    {
        public string SourceFilename { get; set; }
        public string DestinationFilename { get; set; }
        
        public string GetSaveAsLocation(string defaultFilename)
        {
            SourceFilename = defaultFilename;
            return DestinationFilename;
        }

        public string GetSaveToDirectory()
        {
            return DestinationFolder;
        }

        public string DestinationFolder { get; set; }

        public void TryCreateDirectory(string path)
        {
            CreatedDirectory = path;
        }

        public string CreatedDirectory { get; set; }
    }
}