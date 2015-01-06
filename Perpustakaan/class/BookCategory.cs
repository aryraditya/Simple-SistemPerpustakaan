using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Perpustakaan
{
	class BookCategory
	{
        public static dbPerpus db = new dbPerpus();

        private int _id;

        public string name;

        public int id
        {
            get { return _id; }
        }

        public Boolean isNewRecord()
        {
            if (id != 0)
                return false;

            return true;
        }

        private void fillAttributes(SqlDataReader reader){
            _id = int.Parse(reader["category_id"].ToString());
            name = reader["category_name"].ToString();
        }

        public void delete()
        {
            db.openConnection();
            
            string query = "DELETE from ref_category WHERE category_id=@category";
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@category", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Error!");
            }
            db.closeConnection();
        }

        public void save()
        {
            db.openConnection();
            string query;
            if (isNewRecord())
            {
                query = "INSERT INTO ref_category (category_name) VALUES(@category)";
            }
            else
            {
                query = "UPDATE ref_category SET category_name=@category WHERE category_id=@category_id";
            }
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@category_id", id);
            cmd.Parameters.AddWithValue("@category", name);
            cmd.ExecuteNonQuery();

            
            db.closeConnection();

        }

        public static BookCategory findByPk(int __id){
            db.openConnection();
            BookCategory bc = new BookCategory();

            string query = "SELECT * FROM ref_category WHERE category_id=@id";
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@id", __id);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                bc.fillAttributes(reader);
            }
            else
            {
                MessageBox.Show("Invalid Category", "Alert");
            }

            db.closeConnection();

            return bc;
        }

        public static List<BookCategory> findAll(string _cn=null)
        {
            db.openConnection();

            List<BookCategory> bcList = new List<BookCategory>();


            string query = "SELECT * FROM ref_category WHERE category_name LIKE '%"+_cn+"%'";
            SqlCommand cmd = new SqlCommand(query, db.conn);

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BookCategory bc = new BookCategory();
                    bc.fillAttributes(reader);
                    bcList.Add(bc);
                }
            }

            reader.Close();
            db.closeConnection();

            return bcList;
        }
	}
}
