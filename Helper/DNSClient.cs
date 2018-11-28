using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace AdsRunAway.Helper
{
    class DNSClient
    {
        public static RegistryKey RegKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\services\Dnscache", true);
        public static void Disable()
        {
            try
            {
                RegKey.SetValue("Start", 4);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
        }
        public static void Enable()
        {
            try
            {
                RegKey.SetValue("Start", 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.Message);
            }
           
        }
    }
}
