using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Perpustakaan.forms.Buku
{
    public partial class BukuView : Form
    {
        dbPerpus db = new dbPerpus();
        private int _idBuku = 0;

        private void fillForm()
        {
            db.openConnection();
            string sql = "SELECT * FROM book b JOIN ref_category c ON b.category_id=c.category_id WHERE book_id=@bookId";
            SqlCommand cmd = new SqlCommand(sql, db.conn);
            cmd.Parameters.AddWithValue("@bookId", this._idBuku);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                lblJdl.Text = this.Text = reader["book_name"].ToString();
                lblIsdbn.Text = reader["book_isbn"].ToString();
                lblPenulis.Text = reader["book_author"].ToString();
                lblPenerbit.Text = reader["book_publisher"].ToString();
                lblKotaTerbit.Text = reader["book_pub_city"].ToString();
                lblRak.Text = reader["book_casecode"].ToString();
                lblCategory.Text = reader["category_name"].ToString();
                lblTglTerbit.Text = DateTime.Parse(reader["book_pub_date"].ToString()).ToLongDateString();
                lblKuantity.Text = reader["book_qty"].ToString();
                lblDesc.Text = reader["book_description"].ToString();
            }
            else
            {
                MessageBox.Show("Invalid book id", "error!");
                this.Close();
            }
            db.closeConnection();
        }

        public BukuView(int idBuku)
        {
            InitializeComponent();
            this._idBuku = idBuku;
            fillForm();
        }

        private void BukuView_Load(object sender, EventArgs e)
        {

        }
    }
}
