using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Perpustakaan
{
	class Book
	{
        private static dbPerpus db = new dbPerpus();

        private int idBuku = 0;

        public String name;
        public String isbn;
        public string author;
        public string publisher;
        public DateTime publishDate;
        public string publishCity;
        public DateTime entryDate;
        public int quantity;
        public string caseCode;
        public string description;
        public int categoryId;
        public string category;

        public int getId()
        {
            return idBuku;
        }

        public Boolean isNewRecord()
        {
            if (idBuku == 0)
                return true;
            return false;
        }

        public void fillAttributes(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                reader.Read();
                idBuku = int.Parse(reader["book_id"].ToString());
                name = reader["book_name"].ToString();
                isbn = reader["book_isbn"].ToString();
                author = reader["book_author"].ToString();
                publisher = reader["book_publisher"].ToString();
                publishDate = DateTime.Parse(reader["book_pub_date"].ToString());
                publishCity = reader["book_pub_city"].ToString();
                entryDate = DateTime.Parse(reader["book_entry_date"].ToString());
                quantity = int.Parse(reader["book_qty"].ToString());
                caseCode = reader["book_casecode"].ToString();
                description = reader["book_description"].ToString();
                categoryId = int.Parse(reader["category_id"].ToString());
                category = reader["category_name"].ToString();
            }
        }

        public void save()
        {
            db.openConnection();

            String query;
            if(isNewRecord()){
                    query = "INSERT INTO book (book_name, book_isbn, book_author, book_publisher, book_entry_date, book_pub_date, book_pub_city, book_qty, book_casecode,book_description, category_id) values " +
                    "(@book_name,@isbn, @author, @publisher, @entry, @pubdate, @pubcity, @qty, @casecode, @desc, @category)";
               }
                else
                {
                    query = "UPDATE book SET book_name=@book_name, book_isbn=@isbn, book_author=@author, book_publisher=@publisher, book_pub_date=@pubdate, book_pub_city=@pubcity, book_qty=@qty, book_casecode=@casecode, book_description=@desc, category_id=@category " +
                        "WHERE book_id=@book_id";
                }

            SqlCommand cmd = new SqlCommand(query, db.conn);
            cmd.Parameters.AddWithValue("@book_name", name);
            cmd.Parameters.AddWithValue("@isbn", isbn);
            cmd.Parameters.AddWithValue("@author", author);
            cmd.Parameters.AddWithValue("@publisher", publisher);
            cmd.Parameters.AddWithValue("@pubdate", publishDate);
            cmd.Parameters.AddWithValue("@pubcity", publishCity);
            cmd.Parameters.AddWithValue("@qty", quantity);
            cmd.Parameters.AddWithValue("@entry", DateTime.Now.ToString());
            cmd.Parameters.AddWithValue("@casecode", caseCode);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@category", categoryId);
            cmd.Parameters.AddWithValue("@book_id", idBuku);

            cmd.ExecuteNonQuery();

            db.closeConnection();
        }

        public static Book findByPk(int IdBuku)
        {
            db.openConnection();

            String sql = "SELECT * FROM book AS b " +
                        "LEFT OUTER JOIN ref_category AS c ON b.category_id = c.category_id " +
                        "WHERE book_id=@book_id";

            SqlCommand cmd = new SqlCommand(sql, db.conn);
            cmd.Parameters.AddWithValue("@book_id", IdBuku);
            SqlDataReader reader = cmd.ExecuteReader();
            Book book = new Book();
            book.fillAttributes(reader);
            reader.Close();

            return book;
        }
	}
}
