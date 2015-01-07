using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Perpustakaan.forms.Transactions
{
    public partial class frmView : Form
    {
        dbPerpus db = new dbPerpus();

        BookLoan bookLoan;

        public frmView(int idPeminjaman)
        {
            InitializeComponent();
            this.CenterToScreen();
            bookLoan = BookLoan.findByPk(idPeminjaman);
            lbl_nama.Text = bookLoan.name;
            lbl_jk.Text = bookLoan.gender ? "Laki-Laki" : "Perempuan";
            lbl_alamat.Text = bookLoan.address;
            lbl_telepon.Text = bookLoan.phone;
            lbl_identitas.Text = bookLoan.cardType;
            lbl_no_identitas.Text = bookLoan.cardNo;

            lbl_nama_buku.Text = bookLoan.book.name;
            lbl_thn_terbit.Text = bookLoan.book.publishDate.Year.ToString();
            lbl_pengarang.Text = bookLoan.book.author;
            lbl_penerbit.Text = bookLoan.book.publisher;

            if (bookLoan.Return.returnDate != null)
            {
                lbl_pengembalian.Text = bookLoan.Return.returnDate.Value.ToLongDateString();
                lbl_pengembalian.ForeColor = Color.Blue;
            }
            else
            {
                lbl_pengembalian.Text = "BELUM DIKEMBALIKAN";
                lbl_pengembalian.ForeColor = Color.Red;
            }

            lbl_denda.Text = "Rp. "+bookLoan.Return.penalty.ToString();


            this.Text = bookLoan.getId() + " - " + bookLoan.book.name;
        }

        private void frmView_Load(object sender, EventArgs e)
        {

        }
    }
}
