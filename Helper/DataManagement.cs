using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using AdsRunAway.Model;
using Newtonsoft.Json;

namespace AdsRunAway.Helper
{
    class DataManagement
    {
        public List<HostsSource> View()
        {
            string MyData = File.ReadAllText(PathInfo.Source);
            List<HostsSource> CurrentSource = JsonConvert.DeserializeObject <List<HostsSource>> (MyData);
            return CurrentSource;
        }
        public void Changed(bool ChangedValue)
        {
           
        }
        public void BulkSave(List<string> value)
        {
            List<HostsSource> hostsSources = new List<HostsSource>();
            for(int i = 0; i < value.Count - 1; i++)
            {
                HostsSource source = new HostsSource();
                source.Source = value[i];
                source.Checked = true;
                hostsSources.Add(source);
            }
            var JSONData = JsonConvert.SerializeObject(hostsSources);
            File.WriteAllText(PathInfo.Source, JSONData);
        }
        public void Insert(string value)
        {
            HostsSource hostsSource = new HostsSource();
            hostsSource.Source = value;
            hostsSource.Checked = true;
            var DataFile = File.ReadAllText(PathInfo.Source);
            if (DataFile.Length > 0)
            {
                List<HostsSource> CurrentSource = View();
                CurrentSource.Add(hostsSource);
                var JSONData = JsonConvert.SerializeObject(CurrentSource);
                File.WriteAllText(PathInfo.Source, JSONData);
            }
            else
            {
                List<HostsSource> source = new List<HostsSource>();
                source.Add(hostsSource);
                var JSONData = JsonConvert.SerializeObject(source);
                File.WriteAllText(PathInfo.Source, JSONData);
            }

        }
        public void Create()
        {
            if(File.Exists(PathInfo.Source) == false)
            {
                if(Directory.Exists(Path.GetDirectoryName(PathInfo.Source)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(PathInfo.Source));
                }
                File.Create(PathInfo.Source).Close();
                List<HostsSource> sources = new List<HostsSource>();
                //adaway
                HostsSource AdAwaySource = new HostsSource();
                AdAwaySource.Source = Properties.Resources.AdAway;
                AdAwaySource.Checked = true;
                //hostsfile.net
                HostsSource HostsFileNet = new HostsSource();
                HostsFileNet.Source = Properties.Resources.Host_File_Net;
                HostsFileNet.Checked = true;
                //pglyoyo
                HostsSource pglYoyo = new HostsSource();
                pglYoyo.Source = Properties.Resources.Pgl_Yoyo;
                pglYoyo.Checked = true;
                sources.AddRange(new HostsSource[] { AdAwaySource, HostsFileNet, pglYoyo });
                var JSONData = JsonConvert.SerializeObject(sources);
                File.WriteAllText(PathInfo.Source, JSONData);
             
            }
        }
    }
}
