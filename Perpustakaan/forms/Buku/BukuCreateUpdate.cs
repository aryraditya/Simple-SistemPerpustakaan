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
    public partial class BukuCreateUpdate : Form
    {
        dbPerpus db = new dbPerpus();

        private int _idBuku = 0;

        private int defaultCategory = 0;
        private int defaultCategoryIndex = 0;


        #region "Custom Function"
        private void fillCategory()
        {
            db.openConnection();
            string sql = "SELECT * FROM ref_category WHERE ";
            SqlCommand cmd = new SqlCommand(sql, db.conn);
            SqlDataReader reader = cmd.ExecuteReader();
            int no = 0;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ComboItem categoryItem = new ComboItem();
                    categoryItem.Text = reader["category_name"].ToString();
                    categoryItem.Value = reader["category_id"].ToString();
                    comboBox1.Items.Add(categoryItem);

                    if (this.defaultCategory == int.Parse(reader["category_id"].ToString()))
                    {
                        this.defaultCategoryIndex = no;
                    }
                    no++;
                }

            }
            comboBox1.SelectedIndex = this.defaultCategoryIndex;
            db.closeConnection();
        }


        private Boolean validateForm()
        {
            string judulBuku = textBox1.Text.ToString();
            string penulis = textBox3.Text.ToString();
            string penerbit = textBox4.Text.ToString();
            string kotaTerbit = textBox5.Text.ToString();
            string rakBuku = textBox6.Text.ToString();
            Boolean results = true;

            if (judulBuku == "" || penulis == "" || penerbit == "" || kotaTerbit == "" || rakBuku == "")
            {
                MessageBox.Show("Please fill the required fields", "Empty!");
                results = false;
            }

            if (rakBuku.Length < 4)
            {
                MessageBox.Show("Rak buku should be 4 characters", "Invalid length!");
                results =  false;
            }

            return results;
        }

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
                textBox1.Text = reader["book_name"].ToString();
                textBox2.Text = reader["book_isbn"].ToString();
                textBox3.Text = reader["book_author"].ToString();
                textBox4.Text = reader["book_publisher"].ToString();
                textBox5.Text = reader["book_pub_city"].ToString();
                textBox6.Text = reader["book_casecode"].ToString();
                this.defaultCategory = int.Parse(reader["category_id"].ToString());
                dateTimePicker1.Value = DateTime.Parse(reader["book_pub_date"].ToString());
                numericUpDown1.Value = int.Parse(reader["book_qty"].ToString());
                richTextBox1.Text = reader["book_description"].ToString();
            }
            else
            {
                MessageBox.Show("Invalid book id", "error!");
                this.Close();
            }
            db.closeConnection();
        }
        #endregion

        public BukuCreateUpdate(int idBuku=0)
        {
            InitializeComponent();
            this.CenterToScreen();
            this._idBuku = idBuku;


            if (this._idBuku == 0)
                this.Text = "Tambah Buku Baru";
            else
            {
                this.Text = "Update Buku - " + this._idBuku.ToString();
                fillForm();
            }

            fillCategory();

            dateTimePicker1.MaxDate = DateTime.Now.Date;
            //sMessageBox.Show((comboBox1.Items as ComboItem).IndexOf("Sejarah").ToString());
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void BukuUpdate_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!validateForm())
            {
                return;
            }
            string judulBuku = textBox1.Text.ToString();
            string isbn = textBox2.Text.ToString();
            string penulis = textBox3.Text.ToString();
            string penerbit = textBox4.Text.ToString();
            string pub_date = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string pub_city = textBox5.Text.ToString();
            int qty = int.Parse(numericUpDown1.Value.ToString());
            string caseCode = textBox6.Text.ToString();
            string desc = richTextBox1.Text.ToString();
            int categoryId = int.Parse((comboBox1.SelectedItem as ComboItem).Value.ToString());
            string sql = null;
            string msgBox = null;

            db.openConnection();
            if (this._idBuku == 0){
                sql = "INSERT INTO book (book_name, book_isbn, book_author, book_publisher, book_entry_date, book_pub_date, book_pub_city, book_qty, book_casecode,book_description, category_id) values " +
                "(@book_name,@isbn, @author, @publisher, @entry, @pubdate, @pubcity, @qty, @casecode, @desc, @category)";
                msgBox = "New book have been added";
            }else{
                sql = "UPDATE book SET book_name=@book_name, book_isbn=@isbn, book_author=@author, book_publisher=@publisher, book_pub_date=@pubdate, book_pub_city=@pubcity, book_qty=@qty, book_casecode=@casecode, book_description=@desc, category_id=@category "+
                    "WHERE book_id=@book_id";
                msgBox = "Book have been updated";
            }
            
            SqlCommand cmd = new SqlCommand(sql, db.conn);
            cmd.Parameters.AddWithValue("@book_name", judulBuku);
            cmd.Parameters.AddWithValue("@isbn", isbn);
            cmd.Parameters.AddWithValue("@author", penulis);
            cmd.Parameters.AddWithValue("@publisher", penerbit);
            cmd.Parameters.AddWithValue("@pubdate", pub_date);
            cmd.Parameters.AddWithValue("@pubcity", pub_city);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@entry", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@casecode", caseCode);
            cmd.Parameters.AddWithValue("@desc", desc);
            cmd.Parameters.AddWithValue("@category", categoryId);

            if (this._idBuku != 0)
                cmd.Parameters.AddWithValue("@book_id", this._idBuku);

            cmd.ExecuteNonQuery();

            MessageBox.Show(msgBox, "SUCCESS!");

            db.closeConnection();

            if(Application.OpenForms["frmBuku"] != null)
                (Application.OpenForms["frmBuku"] as frmBuku).fillBookGridView();

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this._idBuku == 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                richTextBox1.Clear();
                dateTimePicker1.Value = DateTime.Now.Date;
            }
            else
            {
                fillForm();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.KeyChar = Char.ToUpper(e.KeyChar);
        }


    }
}
