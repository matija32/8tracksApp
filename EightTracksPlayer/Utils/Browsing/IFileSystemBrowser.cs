namespace EightTracksPlayer.Utils.Browsing
{
    public interface IFileSystemBrowser
    {
        string GetSaveAsLocation(string defaultFilename);
        string GetSaveToDirectory();
        void TryCreateDirectory(string path);
    }
}