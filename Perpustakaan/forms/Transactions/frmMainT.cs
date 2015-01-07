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
    public partial class frmMainT : Form
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
            //dataGridView1.Columns.Add("loan_bukuPenulis", "Penulis");
            dataGridView1.Columns.Add("loan_phone", "No Telp");
            dataGridView1.Columns.Add("loan_status", "Status Pengembalian");
            //dataGridView1.Columns.Add("loan_returnDate", "Tanggal Pengembalian");
            dataGridView1.Columns.Add(columnReturn);
            dataGridView1.Columns.Add(columnView);
            dataGridView1.Columns.Add(columnUpdate);
            dataGridView1.Columns.Add(columnDelete);
            dataGridView1.Columns["loan_id"].Width = 75;
            dataGridView1.Columns["loan_date"].Width = 120;
            dataGridView1.Columns["loan_name"].Width = 150;
            dataGridView1.Columns["loan_bukuName"].Width = 330;
            //dataGridView1.Columns["loan_bukuPenulis"].Width = 150;
            dataGridView1.Columns["loan_status"].Width = 75;
            dataGridView1.Columns["loan_phone"].Width = 100;
            //dataGridView1.Columns["loan_returnDate"].Width = 100;
        }

        public void fillGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            initGridView();

            int? _idPeminjaman= null;
            DateTime? _date=null;
            Boolean? _st = null;

            if (textBox3.Text != "")
                _idPeminjaman = int.Parse(textBox3.Text);

            if (checkBox1.Checked)
                _date = dateTimePicker1.Value;

            if (comboBox1.SelectedIndex == 1)
                _st = false;
            else if(comboBox1.SelectedIndex == 2)
                _st = true;

            List<BookLoan> bl = BookLoan.findAll(_idPeminjaman,textBox1.Text,textBox5.Text,_date,_st);
            foreach (BookLoan bls in bl)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["loan_id"].Value = bls.getId();
                dataGridView1.Rows[n].Cells["loan_date"].Value = bls.date.ToString();
                dataGridView1.Rows[n].Cells["loan_name"].Value = bls.name;
                dataGridView1.Rows[n].Cells["loan_bukuName"].Value = bls.book.name;
                //dataGridView1.Rows[n].Cells["loan_bukuPenulis"].Value = bls.book.author;
                dataGridView1.Rows[n].Cells["loan_phone"].Value = bls.phone;
                dataGridView1.Rows[n].Cells["loan_status"].Value = bls.isReturned() ? "Sudah" : "Belum";
                //dataGridView1.Rows[n].Cells["loan_returnDate"].Value = bls.Return.returnDate;
            }
        }

        private void frm_frmClosed(object sender, EventArgs e)
        {
            fillGridView();
        }
        #endregion

        public frmMainT()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            fillGridView();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idPeminjaman = 0 ;
            if (e.RowIndex < 0)
                return;

            idPeminjaman = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

            if (e.ColumnIndex == 6)
            {
                BookLoan loan = BookLoan.findByPk(idPeminjaman);
                if (loan.isReturned())
                {
                    MessageBox.Show("Tidak bisa melakukan transaksi pengembalian, Buku untuk transaksi ini sudah dikembalikan");
                }
                else
                {
                    forms.Transactions.frmReturn frm = new forms.Transactions.frmReturn(idPeminjaman);
                    frm.Show();
                }
            }
            else if (e.ColumnIndex == 7) //view
            {
                forms.Transactions.frmView frm = new forms.Transactions.frmView(idPeminjaman);
                frm.Show();
            }
            else if (e.ColumnIndex == 8) //update
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
                    //frm.FormClosed += new FormClosedEventHandler(frm_frmClosed);
                }
            }
            else if (e.ColumnIndex == 9) //delete
            {
                if (MessageBox.Show("Are you sure want to delete this transaction ?", "Delete transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    BookLoan loan = BookLoan.findByPk(idPeminjaman);
                    loan.delete();
                    fillGridView();
                    if (Application.OpenForms["frmBuku"] != null)
                        (Application.OpenForms["frmBuku"] as frmBuku).fillBookGridView();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fillGridView();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox3.Clear();
            textBox5.Clear();
            checkBox1.Checked = false;
            dateTimePicker1.Value = DateTime.Now;
            comboBox1.SelectedIndex = 0;
            fillGridView();
        }
    }
}
