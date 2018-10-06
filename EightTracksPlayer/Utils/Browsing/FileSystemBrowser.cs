using System;
using System.IO;
using System.Windows.Forms;

namespace EightTracksPlayer.Utils.Browsing
{
    public class FileSystemBrowser : IFileSystemBrowser
    {
        #region IFileSystemBrowser Members

        public string GetSaveAsLocation(string defaultFilename)
        {
            // Show the FolderBrowserDialog.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = defaultFilename;
            saveFileDialog.Filter = @"All Files (*.*)|*.*";
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return String.Empty;
        }

        public string GetSaveToDirectory()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog {ShowNewFolderButton = true};
            folderBrowserDialog.Description = "Select root directory for the mixes";
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                return folderBrowserDialog.SelectedPath;
            }
            return String.Empty;
        }

        public void TryCreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #endregion
    }
}