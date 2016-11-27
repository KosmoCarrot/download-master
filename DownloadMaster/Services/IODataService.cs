using System.Windows.Forms;

namespace DownloadMaster.Services
{
    class IODataService : IOService
    {
        public string OpenFileDialog()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();

            return string.IsNullOrWhiteSpace(fbd.SelectedPath)? string.Empty : fbd.SelectedPath;
        }
    }
}
