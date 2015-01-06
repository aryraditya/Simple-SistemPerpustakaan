using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Perpustakaan
{
	class BookLoan
	{
        private static dbPerpus db = new dbPerpus();

        private int idPeminjaman = 0;

        public DateTime date;
        public string name;
        public Boolean gender;
        public string address;
        public string phone;
        public string cardType;
        public string cardNo;
        public DateTime dateExp;
        public int bookId;
        public BookReturn Return;

        //public DateTime? returnDate = null;
        //public float returnPinalty;
        public Book book;

        public Boolean isNewRecord(){
            if(idPeminjaman==0)
                return true;
            else
                return false;
        }

        public int getId()
        {
            return idPeminjaman;
        }

        public Boolean validate()
        {
            if(name=="" ||  address=="" || phone=="" || cardType=="" || cardNo==""){
                MessageBox.Show("Please fill all field","Alert");
                return false;
            }
            return true;
        }

        public Boolean isReturned()
        {
            if (Return.returnDate==null)
                return false;

            return true;
        }

        public void delete()
        {
            db.openConnection();
            string query = "DELETE FROM book_loan WHERE loan_id=@id";
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@id", idPeminjaman);
            cmd.ExecuteNonQuery();

            db.closeConnection();
        }

        public void save()
        {
            db.openConnection();
            String query = null;
            if (idPeminjaman == 0)
                query = "INSERT INTO book_loan (loan_date, loan_name, loan_gender, loan_address, loan_phone, loan_card_type, loan_card_no, loan_exp_date, book_id) values (@date, @name, @gender, @address, @phone, @cardType, @cardNo, @expDate, @bookId)";
            else{
                query = "UPDATE book_loan SET " +
                        "loan_date = @date, " +
                        "loan_name = @name, " +
                        "loan_gender = @gender, " +
                        "loan_address = @address, " +
                        "loan_phone = @phone, " +
                        "loan_card_type = @cardType, " +
                        "loan_card_no = @cardNo, " +
                        "loan_exp_date = @expDate, " +
                        "book_id = @bookId " +
                        "WHERE loan_id=@idPeminjaman";
            }
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@cardType", cardType);
            cmd.Parameters.AddWithValue("@cardNo", cardNo);
            cmd.Parameters.AddWithValue("@expDate", dateExp);
            cmd.Parameters.AddWithValue("@bookId", bookId);
            cmd.ExecuteNonQuery();
            db.closeConnection();
        }


        private void fillAttributes(SqlDataReader reader)
        {
            idPeminjaman = int.Parse(reader["loan_id"].ToString());
            date = DateTime.Parse(reader["loan_date"].ToString());
            name = reader["loan_name"].ToString();
            address = reader["loan_address"].ToString();
            gender = Boolean.Parse(reader["loan_gender"].ToString());
            phone = reader["loan_phone"].ToString();
            cardType = reader["loan_card_type"].ToString();
            cardNo = reader["loan_card_no"].ToString();
            phone = reader["loan_phone"].ToString();
            dateExp = DateTime.Parse(reader["loan_exp_date"].ToString());
            bookId = int.Parse(reader["book_id"].ToString());
            book = Book.findByPk(bookId);
            Return = BookReturn.findByPk(idPeminjaman);

            //returnPinalty = string.IsNullOrWhiteSpace(reader["return_penalty"].ToString()) ? 0 : float.Parse(reader["return_penalty"].ToString());

            //if (!string.IsNullOrWhiteSpace(reader["return_date"].ToString()))
            //    returnDate = DateTime.Parse(reader["return_date"].ToString());
        }

        public static BookLoan findByPk(int _idPeminjaman)
        {
            db.openConnection();
            String query = "SELECT * FROM book_loan l " +
                            "JOIN book b ON l.book_id=b.book_id " +
                            "LEFT JOIN book_return r ON l.loan_id=r.loan_id "+
                            "WHERE l.loan_id=@idPeminjaman";
            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@idPeminjaman", _idPeminjaman);
            SqlDataReader reader = cmd.ExecuteReader();

            BookLoan bl = new BookLoan();
            
            if (reader.HasRows)
            {
                reader.Read();
                bl.fillAttributes(reader);
            }
            else
            {
                MessageBox.Show("Invalid Data Peminjaman");
            }
            reader.Close();
            db.closeConnection();
            return bl;
        }

        public static List<BookLoan> findAll(string _name = null, string _bookname = null)
        {
            db.openConnection();
            List<BookLoan> results = new List<BookLoan>();
            String sql = "SELECT * FROM book_loan l " +
                    "INNER JOIN book b ON l.book_id=b.book_id " +
                    "LEFT JOIN book_return r ON l.loan_id=r.loan_id " +
                    "WHERE loan_name LIKE '%" + _name + "%' AND book_name LIKE '%" + _bookname + "%'";
            SqlCommand cmd = new SqlCommand(sql, db.conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    BookLoan tmpData = new BookLoan();
                    tmpData.fillAttributes(reader);
                    results.Add(tmpData);
                }
            }

            reader.Close();
            db.closeConnection();

            return results;
        }
	}
}
