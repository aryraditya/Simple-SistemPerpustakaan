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
        }

        private void fillAttributes(SqlDataReader reader)
        {
            returnDate = DateTime.Parse(reader["return_date"].ToString());
            penalty = double.Parse(reader["return_penalty"].ToString());
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
