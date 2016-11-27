using GalaSoft.MvvmLight;
using System;
using System.IO;
using System.Linq;
using System.Net;

namespace DownloadMaster
{
    public class Download : ObservableObject, IDisposable
    {
        private WebClient _webClient;
        private int _progressValue;

        public Download(string url, string path)
        {
            _webClient = new WebClient();
            Url = url;
            Name = CreateUniqueName(Path.Combine(path, Url.Split('/').Last()));
        }

        public string Url { get; set; }
        public string Name { get; set; }

        public int ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                RaisePropertyChanged("ProgressValue");
            }
        }

        public void Start()
        {
            _webClient.DownloadProgressChanged += (s, es) => { ProgressValue = es.ProgressPercentage; };
            _webClient.DownloadFileCompleted += (s, es) => { if (es.Cancelled) { File.Delete(Name); } };
            _webClient.DownloadFileAsync(new Uri(Url), Name);
        }

        public void Stop()
        {
            _webClient.CancelAsync();
        }

        private string CreateUniqueName(string name)
        {
            if (!File.Exists(name))
            {
                return name;
            }
            else
            {
                return Path.Combine(Path.GetDirectoryName(name), string.Concat(Path.GetFileNameWithoutExtension(name), 
                    $"({new Random().Next(0, 50000)})", Path.GetExtension(name))); 
            }
        }

        public void Dispose()
        {
            _webClient.Dispose();
        }
    }
}
