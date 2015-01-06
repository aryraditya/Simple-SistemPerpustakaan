using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Perpustakaan.forms.peminjaman
{
    public partial class frmBorrow : Form
    {
        private int idBuku;
        private int idPeminjaman;
        BookLoan bookLoan = null;
        Book book = null;

        public frmBorrow(int _idBuku,int _idPeminjaman=0)
        {
            InitializeComponent();
            idBuku = _idBuku;
            idPeminjaman = _idPeminjaman;

            cmbIdType.SelectedIndex = 0;
            lblTitle.Text = this.Text = "Form Peminjaman Buku";

            book = Book.findByPk(idBuku);
            fillDataBuku();

            if (idPeminjaman == 0)
                bookLoan = new BookLoan();
            else
            {
                bookLoan = BookLoan.findByPk(idPeminjaman);
                txtNama.Text = bookLoan.name;
                txtAlamat.Text = bookLoan.address;
                txtNoTelp.Text = bookLoan.phone;

                if (bookLoan.gender)
                    radioButton2.Checked = true;
                else
                    radioButton1.Checked = true;

                cmbIdType.SelectedItem = bookLoan.cardType;
                txtIdNo.Text = bookLoan.cardNo;
            }
        }

        #region custom function
        void fillDataBuku()
        {
                txtJudulBuku.Text = book.name;
                txtIdBuku.Text = book.getId().ToString();
                txtTahun.Text = book.publishDate.Year.ToString();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            
            bookLoan.date = dateTimePicker1.Value;
            bookLoan.dateExp = dateTimePicker2.Value;
            bookLoan.name = txtNama.Text;
            bookLoan.address = txtAlamat.Text;
            bookLoan.cardNo = txtIdNo.Text;
            bookLoan.cardType = cmbIdType.SelectedItem.ToString();
            if (radioButton1.Checked)
                bookLoan.gender = false;
            else
                bookLoan.gender = true;
            bookLoan.bookId = idBuku;
            bookLoan.phone = txtNoTelp.Text;


            if(bookLoan.validate())
            {
                bookLoan.save();
                if (bookLoan.isNewRecord())
                {
                    book.quantity = book.quantity - 1;
                    book.save();
                }
                MessageBox.Show("Transaksi Peminjaman berhasil disimpan", "Sukses");

                this.Close();
            }
        }

        private void frmForm_Load(object sender, EventArgs e)
        {
            if (bookLoan.isNewRecord())
                dateTimePicker2.Value = dateTimePicker1.Value.AddDays(7);
            else
            {
                dateTimePicker1.Value = bookLoan.date;
                dateTimePicker2.Value = bookLoan.dateExp;
            }
        }
    }
}
