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
            bookLoan = BookLoan.findByPk(idPeminjaman);

            this.Text = bookLoan.getId() + " - " + bookLoan.book.name;
        }

        private void frmView_Load(object sender, EventArgs e)
        {

        }
    }
}
