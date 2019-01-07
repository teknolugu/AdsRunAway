using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AdsRunAway
{
    public partial class Starting : Form
    {
        public Starting()
        {
            InitializeComponent();
        }

        private void Starting_Load(object sender, EventArgs e)
        {
            BackColor = ColorTranslator.FromHtml("#e74c3c");
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FirstLaunch = false;
            Properties.Settings.Default.Save();
            Main frm = new Main();
            Hide();
            frm.Show();
        }
    }
}
