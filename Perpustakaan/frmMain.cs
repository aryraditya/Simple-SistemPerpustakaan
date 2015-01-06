using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Perpustakaan
{
    public partial class frmMain : Form
    {
        dbPerpus db = new dbPerpus();
        public forms.frmBuku frmBuku = null;
        public forms.Transactions.frmMain frmPeminjaman = null;
        public forms.Buku.CategoryMain frmCategory = null;
        public forms.App.Settings frmSettings = null; 

        #region "Child Form Handler"
        void frmBuku_FormClosed(object sender, EventArgs e)
        {
            frmBuku = null;
        }

        void frmPeminjaman_FormClosed(object sender, EventArgs e)
        {
            frmPeminjaman = null;
        }

        void frmCategory_formClosed(object sender, EventArgs e)
        {
            frmCategory = null;
        }
        
        void bukuInputFormClosed(object sender, EventArgs e)
        {
            frmBuku = null;
            //daftarBukuToolStripMenuItem_Click(sender, e);
        }
        void frmSettingsFormClosed(object sender, EventArgs e)
        {
            frmSettings = null;
            //daftarBukuToolStripMenuItem_Click(sender, e);
        }

        private void closeMDI()
        {
            if(ActiveMdiChild != null){
                ActiveMdiChild.Close();
            }
        }
        #endregion


        public frmMain()
        {
            InitializeComponent();
            this.Text = user.name + " - " + this.Text;
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new frmLogin().Show();
            user.DestroySession();
            this.Hide();
        }

        private void daftarBukuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBuku = null;
            if (frmBuku == null)
            {
                frmBuku = new forms.frmBuku();
                frmBuku.MdiParent = this;
                frmBuku.FormClosed += new FormClosedEventHandler(frmBuku_FormClosed);
                frmBuku.Show();
            }
            else
            {
                frmBuku.Activate();
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void inputBukuBaruToolStripMenuItem_Click(object sender, EventArgs e)
        {
            forms.Buku.BukuCreateUpdate frmBuku = new forms.Buku.BukuCreateUpdate();
            frmBuku.Show();
            frmBuku.FormClosing += new FormClosingEventHandler(bukuInputFormClosed);
        }

        private void kategoriBukuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmCategory == null)
            {
                frmCategory = new forms.Buku.CategoryMain();
                frmCategory.MdiParent = this;
                frmCategory.FormClosed += new FormClosedEventHandler(frmCategory_formClosed);
                frmCategory.Show();
            }
            else
            {
                frmCategory.Activate();
            }
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPeminjaman = null;
            if (frmPeminjaman == null)
            {
                frmPeminjaman = new forms.Transactions.frmMain();
                frmPeminjaman.MdiParent = this;
                frmPeminjaman.FormClosed += new FormClosedEventHandler(frmPeminjaman_FormClosed);
                frmPeminjaman.Show();
            }
            else
            {
                frmPeminjaman.Activate();
            }
        }

        private void applicationSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (frmSettings == null)
            {
                frmSettings = new forms.App.Settings();
                frmSettings.Show();
                frmSettings.FormClosed += new FormClosedEventHandler(frmSettingsFormClosed);
            }
            else
            {
                frmSettings.Activate();
            }
        }

    }
}
