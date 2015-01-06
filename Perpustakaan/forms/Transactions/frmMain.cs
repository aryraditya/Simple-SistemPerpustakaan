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
    public partial class frmMain : Form
    {
        dbPerpus db = new dbPerpus();

        #region custom function
        private void initGridView(){
            DataGridViewImageButtonDeleteColumn columnDelete = new DataGridViewImageButtonDeleteColumn();
            DataGridViewImageButtonUpdateColumn columnUpdate = new DataGridViewImageButtonUpdateColumn();
            DataGridViewImageButtonViewColumn columnView = new DataGridViewImageButtonViewColumn();
            DataGridViewImageButtonReturnColumn columnReturn = new DataGridViewImageButtonReturnColumn();
            columnView.Width = 20;
            columnReturn.Width = 20;
            columnDelete.Width = 20;
            columnUpdate.Width = 20;
            dataGridView1.Columns.Add("loan_id", "ID Transaksi");
            dataGridView1.Columns.Add("loan_date", "Tanggal");
            dataGridView1.Columns.Add("loan_name", "Peminjam");
            dataGridView1.Columns.Add("loan_bukuName", "Nama Buku");
            dataGridView1.Columns.Add("loan_bukuPenulis", "Penulis");
            dataGridView1.Columns.Add("loan_phone", "No Telp");
            dataGridView1.Columns.Add("loan_status", "Status");
            dataGridView1.Columns.Add("loan_returnDate", "Tanggal Pengembalian");
            dataGridView1.Columns.Add(columnReturn);
            dataGridView1.Columns.Add(columnView);
            dataGridView1.Columns.Add(columnUpdate);
            dataGridView1.Columns.Add(columnDelete);
            dataGridView1.Columns["loan_id"].Width = 75;
            dataGridView1.Columns["loan_date"].Width = 100;
            dataGridView1.Columns["loan_name"].Width = 175;
            dataGridView1.Columns["loan_bukuName"].Width = 300;
            dataGridView1.Columns["loan_bukuPenulis"].Width = 150;
            dataGridView1.Columns["loan_phone"].Width = 100;
            dataGridView1.Columns["loan_returnDate"].Width = 100;
        }

        private void fillGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            initGridView();
            List<BookLoan> bl = BookLoan.findAll();
            foreach (BookLoan bls in bl)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["loan_id"].Value = bls.getId();
                dataGridView1.Rows[n].Cells["loan_date"].Value = bls.date.ToShortDateString();
                dataGridView1.Rows[n].Cells["loan_name"].Value = bls.name;
                dataGridView1.Rows[n].Cells["loan_bukuName"].Value = bls.book.name;
                dataGridView1.Rows[n].Cells["loan_bukuPenulis"].Value = bls.book.author;
                dataGridView1.Rows[n].Cells["loan_phone"].Value = bls.phone;
                dataGridView1.Rows[n].Cells["loan_status"].Value = bls.isReturned() ? "Returned" : "Borrowed";
                dataGridView1.Rows[n].Cells["loan_returnDate"].Value = bls.Return.returnDate;
            }
        }

        private void frm_frmClosed(object sender, EventArgs e)
        {
            fillGridView();
        }
        #endregion

        public frmMain()
        {
            InitializeComponent();
            fillGridView();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idPeminjaman = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

            if (e.ColumnIndex == 9)
            {
            }
            else if (e.ColumnIndex == 10)
            {
                BookLoan loan = BookLoan.findByPk(idPeminjaman);
                if (loan.isReturned())
                {
                    MessageBox.Show("Tidak bisa merubah transaksi, Buku untuk transaksi ini sudah dikembalikan");
                }
                else
                {
                    forms.peminjaman.frmBorrow frm = new forms.peminjaman.frmBorrow(loan.bookId, loan.getId());
                    frm.Show();
                    frm.FormClosed += new FormClosedEventHandler(frm_frmClosed);
                }
            }
            else if (e.ColumnIndex == 11) //delete
            {
                if (MessageBox.Show("Are you sure want to delete this transaction ?", "Delete transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    BookLoan loan = BookLoan.findByPk(idPeminjaman);
                    loan.delete();
                    fillGridView();
                }
            }
        }
    }
}
