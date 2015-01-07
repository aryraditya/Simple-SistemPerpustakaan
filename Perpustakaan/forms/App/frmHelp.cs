using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Perpustakaan.forms.App
{
    public partial class frmHelp : Form
    {
        public frmHelp()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            linkLabel1.Links.Add(0,55,"https://github.com/aryraditya/Simple-SistemPerpustakaan");
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
    }
}
