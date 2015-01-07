using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Perpustakaan
{
	class BookReturn
	{
        private static dbPerpus db = new dbPerpus();

        private int _id;

        public DateTime? returnDate = null;
        public double penalty;

        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private void fillAttributes(SqlDataReader reader)
        {
            //_id = int.Parse(reader["loan_id"].ToString());
            returnDate = DateTime.Parse(reader["return_date"].ToString());
            penalty = double.Parse(reader["return_penalty"].ToString());
        }

        public Boolean Save()
        {
            db.openConnection();

            string qr = "INSERT INTO book_return (loan_id,return_date,return_penalty) VALUES (@loan_id,@date,@penalty)";
            SqlCommand cmd = new SqlCommand(qr, db.conn);
            cmd.Parameters.AddWithValue("@loan_id", _id);
            cmd.Parameters.AddWithValue("@date", returnDate);
            cmd.Parameters.AddWithValue("@penalty", penalty);

            try
            {
                cmd.ExecuteNonQuery();
                db.closeConnection();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                db.closeConnection();
                return false;
            }
        }

        public static BookReturn findByPk(int idPeminjaman)
        {
            db.openConnection();

            BookReturn br = new BookReturn();
            string query = "SELECT * FROM book_return WHERE loan_id=@loan_id";
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@loan_id", idPeminjaman);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                br.fillAttributes(reader);
            }

            reader.Close();
            db.closeConnection();
            return br;
        }
	}
}
