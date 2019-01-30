using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using AdsRunAway.Helper;
using System.IO;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;
using AdsRunAway.Model;

namespace AdsRunAway
{
    public partial class Main : Form
    {
        DataManagement DataMgmt = new DataManagement();
        StringBuilder builder = new StringBuilder();
        Downloader downloader = new Downloader();
        public Main()
        {
            InitializeComponent();
         
        }
        protected override void WndProc(ref Message m)
        {
            if(m.Msg == NativeMethods.WM_SHOWME)
            {
                ShowMe();
            }
            base.WndProc(ref m);
        }
        private void ShowMe()
        {
            if(WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            bool top = TopMost;
            TopMost = true;
            TopMost = top;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DNSClient.Enable();
            MessageBox.Show(this, "The DNSClient has been successfully enabled, you must restart your computer to take effect", "Enabled", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
        }
        void GetHostsSource()
        {
            List<bool> chck = new List<bool>();
            var data = DataMgmt.View();

            foreach(HostsSource item in data)
            {
                checkedListBox1.Items.Add(item.Source);
                chck.Add(item.Checked);
            }

            //checkedstate
            for(int i=0; i <= checkedListBox1.Items.Count - 1; i++)
            {
                checkedListBox1.SetItemChecked(i, chck[i]);
            }
          
        }
      

        #region hostsProcess
        void Finish()
        {
            LblStatus.Text = "Ready";
            LblReceived.Visible = false;
            LblTotal.Visible = false;
            HostsProcess.Visible = false;
        }
        void Start()
        {
            LblReceived.Visible = true;
            LblTotal.Visible = true;
            HostsProcess.Visible = true;
        }
        private async void DownloadFile()
        {
            Start();
            WebClient client = new WebClient();
            client.DownloadFileCompleted += Client_DownloadFileCompleted;
            client.DownloadProgressChanged += Client_DownloadProgressChanged;
            foreach(string item in checkedListBox1.Items)
            {
                LblStatus.Text = "Downloading..";
                await client.DownloadFileTaskAsync(new Uri(item), PathInfo.Downloaded);
            }
           
            LblStatus.Text = "Merging...";
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + @"\merged.txt", builder.ToString());
            Finish();
            if (File.Exists(PathInfo.Hosts) == true)
            {

                    File.Copy(PathInfo.Hosts, PathInfo.Hosts + ".bak", true);
            }

            try
            {
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"\merged.txt", PathInfo.Hosts, true);
                    Properties.Settings.Default.Enabled = true;
                    Properties.Settings.Default.Save();
                    MessageBox.Show(this, "Action Successfully Applied, You must restart your computer to take effect!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Check();

            }catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            LblReceived.Text = bytesIn + " / " + totalBytes;
            HostsProcess.Value = e.ProgressPercentage;


        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            builder.Append(File.ReadAllText(PathInfo.Downloaded));
            builder.AppendLine();
        }

        #endregion
      

        private void BtnTabBlock_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            PanelSelector.Location = new Point(9, 91);
            PanelSelector.Size = new Size(50, 3);
            BtnTabHost.ForeColor = Color.Silver;
            BtnTabOther.ForeColor = Color.Silver;
            if (Properties.Settings.Default.Dark == true){
                BtnTabBlock.ForeColor = Color.White;
            }
            else {
                BtnTabBlock.ForeColor = Color.Black;
            }
           
            
        }

        private void BtnTabHost_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
            PanelSelector.Location = new Point(75, 91);
            PanelSelector.Size = new Size(100, 3);
            if(Properties.Settings.Default.Dark == true)
            {
                BtnTabHost.ForeColor = Color.White;
            }
            else
            {
                BtnTabHost.ForeColor = Color.Black;
            }
           
            BtnTabBlock.ForeColor = Color.Silver;
            BtnTabOther.ForeColor = Color.Silver;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DataMgmt.Create();
            CheckForIllegalCrossThreadCalls = false;
            GetHostsSource();
            Check();
        }
        #region EnabledOrDisabled
        void Check()
        {
            switch (Properties.Settings.Default.Enabled)
            {
                case true:
                    HostsEnabled();
                    break;
                case false:
                    HostsDisabled();
                    break;

            }
        }
        void HostsEnabled()
        {
            BtnApply.Text = "DISABLE";
            LblHostsStatus.Text = "Enabled";
            LblSummary.Text = "Adblocker is enabled. enjoy your browsing without ads";
            LblHostsStatus.ForeColor = Color.Green;

        }
        void HostsDisabled()
        {
            BtnApply.Text = "ENABLE";
            LblHostsStatus.Text = "Not Enabled";
            LblSummary.Text = "Adblocker is currently not enabled, if you want to enable it, click on enable button below";
            LblHostsStatus.ForeColor = Color.Red;
        }
        #endregion

        private void BtnApply_Click(object sender, EventArgs e)
        {
            switch (BtnApply.Text)
            {
                case "ENABLE":
                    {
                        switch (MessageBox.Show(this, "The DNS Client Service must be disabled to prevent your internet access from being slow, Would you like to disable it?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                        {
                            case DialogResult.Yes:
                                {
                                    DNSClient.Disable();
                                    try
                                    {
                                        HostsManagement.Backup();
                                    } catch (Exception ex)
                                    {
                                        MessageBox.Show(this, ex.Message, "An error occured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    DownloadFile();
                                    break;
                                }
                            case DialogResult.No:
                                {
                                    MessageBox.Show(this, "Action Canceled by the user", "Canceled", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    Finish();
                                    break;
                                }
                        }
                        break;
                    }
                case "DISABLE":
                    {
                        DNSClient.Enable();
                        try
                        {
                            HostsManagement.Restore();
                        }
                        catch(Exception ex)
                        {
                            DNSClient.Disable();
                            MessageBox.Show(this, "There is an error " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        
                        Properties.Settings.Default.Enabled = false;
                        Properties.Settings.Default.Save();
                        Check();
                        break;
                    }
            }
          
        }

        private void TxtNewSource_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BtnDone.PerformClick(); 
            }
           
        }

        private void LinkEdit_Click(object sender, EventArgs e)
        {
            EditMode(true);
        }
        void EditMode(bool Enabled)
        {
            switch (Enabled)
            {
                case true:
                    LinkDelete.Visible = true;
                    LinkFinish.Visible = true;
                    checkedListBox1.Enabled = true;
                    LinkEdit.Enabled = false;
                    LinkNew.Enabled = false;
                    break;
                case false:
                    LinkNew.Enabled = true;
                    LinkDelete.Visible = false;
                    LinkFinish.Visible = false;
                    checkedListBox1.Enabled = false;
                    LinkEdit.Enabled = true;
                    break;
            }
        }

        private void LinkFinish_Click(object sender, EventArgs e)
        {
            List<HostsSource> Sources = new List<HostsSource>();
            for(int i = 0; i<=checkedListBox1.Items.Count - 1; i++)
            {
                HostsSource hostsSource = new HostsSource();
                hostsSource.Source = checkedListBox1.Items[i].ToString();
                hostsSource.Checked = checkedListBox1.GetItemChecked(i);
                Sources.Add(hostsSource);
            }
            var JSONData = JsonConvert.SerializeObject(Sources);
            File.WriteAllText(PathInfo.Source, JSONData);
            EditMode(false);
        }

        private void LinkDelete_Click(object sender, EventArgs e)
        {
            switch(MessageBox.Show(this, "Are you sure want to delete " + checkedListBox1.SelectedItem + "?","Confirmation",MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);
                    break;
            }
            
        }

        private void LinkNew_Click(object sender, EventArgs e)
        {
            TxtNewSource.Visible = true;
            BtnDone.Visible = true;
            BtnCancel.Visible = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void LinkSettings_Click(object sender, EventArgs e)
        {
            contextMenuStrip1.Show(LinkSettings, 0, LinkSettings.Height);
        }

        private void LinkAbout_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog(this);
        }

        private void BtnTabOther_Click(object sender, EventArgs e)
        {
            PanelSelector.Location = new Point(190, 91);
            PanelSelector.Size = new Size(50, 3);
            tabControl1.SelectedTab = tabPage3;
            if(Properties.Settings.Default.Dark == true)
            {
                BtnTabOther.ForeColor = Color.White;
            }
            else
            {
                BtnTabOther.ForeColor = Color.Black;
            }
            
            BtnTabBlock.ForeColor = Color.Silver;
            BtnTabHost.ForeColor = Color.Silver;
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            if (TxtNewSource.Text == "")
            {
                MessageBox.Show(this, "Value cannot be null", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DataMgmt.Insert(TxtNewSource.Text);
                checkedListBox1.Items.Clear();
                GetHostsSource();
            }
            TxtNewSource.Visible = false;
            BtnDone.Visible = false;
            BtnCancel.Visible = false;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            TxtNewSource.Visible = false;
            BtnDone.Visible = false;
            BtnCancel.Visible = false;
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void ApplyTheme(ThemeType theme)
        {
            ThemeManager themeManager = new ThemeManager();
            List<Color> darkColor = themeManager.ApplyTheme(theme);
            //Apply Theme
            BackColor = darkColor[0];
            PanelMain.DarkTheme = true;
            PanelHeader.DarkTheme = true;

            tabPage1.BackColor = darkColor[0];
            tabPage2.BackColor = darkColor[0];
            tabPage3.BackColor = darkColor[0];

            ForeColor = Color.White;
            lineShape2.BorderColor = darkColor[0];

            BtnApply.BGColor = "#2D2D2D";
            PanelSelector.BackColor = ColorTranslator.FromHtml("#3498db");

            statusStrip1.BackColor = ColorTranslator.FromHtml("#2D2D2D");
            lineShape3.BorderColor = lineShape2.BorderColor;

            LblTitle.ForeColor = Color.WhiteSmoke;
            label3.ForeColor = Color.White;

            LinkSettings.Image = Properties.Resources.baseline_settings_white_18dp;
            LinkAbout.Image = Properties.Resources.baseline_help_white_24dp;
            LblSummary.ForeColor = Color.White;

            //save settings
            Properties.Settings.Default.Dark = true;
            Properties.Settings.Default.Save();
        }
    }
}
