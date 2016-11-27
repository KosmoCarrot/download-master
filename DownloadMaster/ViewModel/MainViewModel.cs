using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using DownloadMaster.Services;
using System.IO;
using System.Collections.ObjectModel;
using static System.Net.ServicePointManager;
using MahApps.Metro.Controls.Dialogs;

namespace DownloadMaster.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogCoordinator _dialogService;
        private readonly IOService _ioService;
        private string _selectedPath;
        private string _currentUrl;
        private Download _currentDownload;

        public MainViewModel(IDialogCoordinator dialogService, IOService ioService)
        {
            DefaultConnectionLimit = 10;

            _dialogService = dialogService;
            _ioService = ioService;

            SetFolderCommand = new RelayCommand(SetFolder);
            DownloadCommand = new RelayCommand(DownloadFile);
            RemoveDownloadCommand = new RelayCommand(RemoveDownload);
        }

        public ObservableCollection<Download> Downloads { get; set; } = new ObservableCollection<Download>();

        public RelayCommand SetFolderCommand { get; set; }
        public RelayCommand DownloadCommand { get; set; }
        public RelayCommand RemoveDownloadCommand { get; set; }

        public string SelectedPath
        {
            get
            {
                return _selectedPath ??
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            }
            set
            {
                _selectedPath = value;
         
                RaisePropertyChanged("SelectedPath");
            }
        }

        public string CurrentUrl
        {
            get { return _currentUrl; }
            set
            {
                _currentUrl = value;

                RaisePropertyChanged("CurrentUrl");
            }
        }

        public Download CurrentDownload
        {
            get { return _currentDownload; }
            set
            {
                _currentDownload = value;

                RaisePropertyChanged("CurrentDownload");
            }
        }

        private void SetFolder()
        {
            string _folder = _ioService.OpenFileDialog(); 

            if (_folder != string.Empty)
                SelectedPath = _folder;
        }

        private void DownloadFile()
        {
            try
            {
                Download download = new Download(CurrentUrl, SelectedPath);
                download.Start();
                Downloads.Add(download);
                CurrentUrl = null;
            }
            catch (Exception)
            {
                _dialogService.ShowMessageAsync(this, "ERROR", "Please make sure all fields are set properly!");
            }
        }

        private void RemoveDownload()
        {
            CurrentDownload.Stop();
            Downloads.Remove(CurrentDownload);
        }
    }
}