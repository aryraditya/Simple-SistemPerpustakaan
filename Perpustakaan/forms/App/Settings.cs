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
    public partial class Settings : Form
    {
        AppSettings denda = AppSettings.findByName(AppSettings.Denda);

        public Settings()
        {
            InitializeComponent();
            if (denda.Value == null)
                denda.Value = "0";

            textBox1.Text = denda.Value;
        }

        private void Settings_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            denda.Value = textBox1.Text;
            denda.save();
            MessageBox.Show("Changes have been saved");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
