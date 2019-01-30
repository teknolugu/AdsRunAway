using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using AdsRunAway.Model;

namespace AdsRunAway.Helper
{
    internal class Downloader
    {
        private StringBuilder builder = new StringBuilder();

        internal delegate void DownloadProgressDelegate(int ProgressPercentage);

        internal event DownloadProgressDelegate DownloadProgress;

        public async void DownloadFile(string url)
        {
            WebClient client = new WebClient();
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            await Task.Run(() =>
            {
                client.DownloadFileAsync(new Uri(url), PathInfo.Downloaded);
            });
            return;
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            string MergedHostsPath = AppDomain.CurrentDomain.BaseDirectory + @"\merged.txt";
            builder.Append(File.ReadAllText(PathInfo.Downloaded));
            StreamWriter sw = new StreamWriter(MergedHostsPath, append: true);
            sw.WriteLine(builder.ToString());
            sw.Close();
            sw.Dispose();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            Main form = new Main();
            //form.SetProgress(int.Parse(Math.Truncate(percentage).ToString()));
            //form.SetProgress(e.ProgressPercentage);
        }
    }
}