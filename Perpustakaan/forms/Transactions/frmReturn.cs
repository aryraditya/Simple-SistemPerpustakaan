using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Perpustakaan.forms.Transactions
{
    public partial class frmReturn : Form
    {
        BookLoan bl;

        private AppSettings denda = AppSettings.findByName(AppSettings.Denda);

        public frmReturn(int idPeminjaman)
        {
            InitializeComponent();
            this.CenterToScreen();
            bl = BookLoan.findByPk(idPeminjaman);
            txtIdBuku.Text = bl.book.name;
            txtJudulBuku.Text = bl.book.getId().ToString();
            txtTahun.Text = bl.book.publishDate.Year.ToString();
            txtNama.Text = bl.name;
            txtAlamat.Text = bl.address;
            txtNoTelp.Text = bl.phone;
            if (bl.gender)
                radioButton2.Checked = true;
            else
                radioButton1.Checked = true;
            cmbIdType.SelectedItem = bl.cardType;
            txtIdNo.Text = bl.cardNo;
        }

        private void frmReturn_Load(object sender, EventArgs e)
        {

            dateTimePicker1.Value = bl.date;
            dateTimePicker2.Value = bl.dateExp;
            dateTimePicker3.Value = DateTime.Now;
            calculatePenalty();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bl.save();

            bl.Return.penalty = double.Parse(txt_denda.Text);
            bl.Return.returnDate = dateTimePicker3.Value;
            bl.Return.id = bl.getId();


            if (bl.Return.Save())
            {
                bl.book.quantity = bl.book.quantity + 1;
                bl.book.save();

                MessageBox.Show("Data Telah Disimpan", "SUCCESS!");

                if (Application.OpenForms["frmMainT"] != null)
                    (Application.OpenForms["frmMainT"] as forms.Transactions.frmMainT).fillGridView();

                if (Application.OpenForms["frmBuku"] != null)
                    (Application.OpenForms["frmBuku"] as frmBuku).fillBookGridView();

                this.Close();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void calculatePenalty()
        {
            double dateDiff = Math.Floor(double.Parse((dateTimePicker3.Value - bl.dateExp).TotalDays.ToString()));
            if (dateDiff > 0)
            {
                int ddn;
                if (string.IsNullOrWhiteSpace(denda.Value))
                    ddn = 0;
                else
                    ddn = int.Parse(denda.Value);

                txt_denda.Text = (dateDiff * ddn).ToString();
            }
            else
                txt_denda.Text = "0";
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

            calculatePenalty();
        }
    }
}
