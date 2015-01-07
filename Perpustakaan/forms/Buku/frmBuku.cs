using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Perpustakaan.forms
{
    public partial class frmBuku : Form
    {
        dbPerpus db = new dbPerpus();

        #region custom function

        void bukuInputFormClosed(object sender, EventArgs e)
        {
            fillBookGridView();
        }

        void frm_formClosed(object sender, EventArgs e)
        {
            fillBookGridView();
        }
        
        #endregion

        public frmBuku()
        {
            InitializeComponent();
        }

        private void frmBuku_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            fillBookGridView();
        }

        public void fillBookGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            db.openConnection();

            String sql = "SELECT * FROM book AS b "+
                        "LEFT OUTER JOIN ref_category AS c ON b.category_id = c.category_id "+
                        "WHERE book_name LIKE '%"+textBox1.Text+"%'"+
                        " AND category_name LIKE '%" + textBox5.Text + "%'"+
                        " AND book_author LIKE '%" + textBox2.Text + "%'"+
                        " AND book_publisher LIKE '%" + textBox3.Text + "%'";

            if (textBox4.Text != "")
                sql += " AND year(book_pub_date)=@year";
            SqlCommand cmd = new SqlCommand(sql,db.conn);
            cmd.Parameters.AddWithValue("@year", textBox4.Text);
            SqlDataReader reader = cmd.ExecuteReader();
            initGridViewColumn();
            if(reader.HasRows){
                while(reader.Read()){
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = reader["book_id"].ToString();
                    dataGridView1.Rows[n].Cells[1].Value = reader["book_name"].ToString();
                    dataGridView1.Rows[n].Cells[2].Value = reader["category_name"].ToString();
                    dataGridView1.Rows[n].Cells[3].Value = reader["book_author"].ToString();
                    dataGridView1.Rows[n].Cells[4].Value = reader["book_publisher"].ToString();
                    dataGridView1.Rows[n].Cells[5].Value = reader["book_qty"].ToString();
                    dataGridView1.Rows[n].Cells[6].Value = reader["book_casecode"].ToString();
                }

            }
            reader.Close();
            db.closeConnection();
        }

        private void initGridViewColumn()
        {
            DataGridViewImageButtonDeleteColumn columnDelete = new DataGridViewImageButtonDeleteColumn();
            DataGridViewImageButtonUpdateColumn columnUpdate = new DataGridViewImageButtonUpdateColumn();
            DataGridViewImageButtonViewColumn columnView = new DataGridViewImageButtonViewColumn();
            DataGridViewImageButtonTransactionColumn columnTransaction = new DataGridViewImageButtonTransactionColumn();
            columnView.Width = 20;
            columnTransaction.Width = 20;
            columnDelete.Width = 20;
            columnUpdate.Width =  20;
            dataGridView1.Columns.Add("book_id", "ID Buku");
            dataGridView1.Columns.Add("book_name", "Judul Buku");
            dataGridView1.Columns.Add("book_category", "Kategori");
            dataGridView1.Columns.Add("book_author", "Penulis");
            dataGridView1.Columns.Add("book_publisher", "Penerbit");
            dataGridView1.Columns.Add("book_qty", "Stok");
            dataGridView1.Columns.Add("book_casecode", "Rak Buku");
            dataGridView1.Columns.Add(columnTransaction);
            dataGridView1.Columns.Add(columnView);
            dataGridView1.Columns.Add(columnUpdate);
            dataGridView1.Columns.Add(columnDelete);
            dataGridView1.Columns["book_id"].Width = 50;
            dataGridView1.Columns["book_name"].Width = 300;
            dataGridView1.Columns["book_category"].Width = 110;
            dataGridView1.Columns["book_author"].Width = 150;
            dataGridView1.Columns["book_publisher"].Width = 150;
            dataGridView1.Columns["book_qty"].Width = 50;
            dataGridView1.Columns["book_casecode"].Width = 50;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            int idBuku = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            int qtyBuku = int.Parse(dataGridView1.Rows[e.RowIndex].Cells["book_qty"].Value.ToString());

            if (e.ColumnIndex == 7) //transaction
            {
                if (qtyBuku <= 0)
                    MessageBox.Show("Stok buku tidak ada. Tidak dapat melakukan transaksi untuk buku ini","Stok buku kosong");
                else
                {
                    forms.peminjaman.frmBorrow frm = new forms.peminjaman.frmBorrow(idBuku);
                    frm.Show();
                    //frm.FormClosed += new FormClosedEventHandler(frm_formClosed);
                }
            }
            else if (e.ColumnIndex == 8) //view
            {
                forms.Buku.BukuView frmView = new forms.Buku.BukuView(idBuku);
                frmView.Show();
            }
            else if (e.ColumnIndex == 9) //update
            {
                forms.Buku.BukuCreateUpdate frmCR = new forms.Buku.BukuCreateUpdate(idBuku);
                frmCR.Show();

                //frmCR.FormClosed += new FormClosedEventHandler(bukuInputFormClosed);
            }
            else if (e.ColumnIndex == 10) //delete
            {
                if (MessageBox.Show("Semua Transaksi untuk buku ini akan ikut terhapus. Apakah anda yakin untuk menghapus buku ini ?", "Delete book", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    db.openConnection();
                    string sql = "DELETE FROM book WHERE book_id=@id";
                    SqlCommand cmd = new SqlCommand(sql, db.conn);
                    cmd.Parameters.AddWithValue("@id", idBuku);
                    cmd.ExecuteNonQuery();

                    fillBookGridView();
                    db.closeConnection();
                }

            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            fillBookGridView();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {

            //fillBookGridView();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fillBookGridView();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            fillBookGridView();
        }
    }
}
