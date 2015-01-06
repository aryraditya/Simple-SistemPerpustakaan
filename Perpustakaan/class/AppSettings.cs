using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Perpustakaan
{
	class AppSettings
	{
        public const string Denda = "denda";

        public static dbPerpus db = new dbPerpus();

        private Boolean _isNewRecord;

        public Boolean isNewRecord
        {
            get { return _isNewRecord; }
        }

        public string Name;
        public string Value;

        public void save()
        {
            db.openConnection();
            string qr;
            if (isNewRecord)
            {
                qr = "INSERT INTO setting (setting_name, setting_value) VALUES (@name,@value)";
            }
            else
            {
                qr = "UPDATE setting SET setting_value=@value WHERE setting_name=@name";
            }

            SqlCommand cmd = new SqlCommand(qr, db.conn);
            cmd.Parameters.AddWithValue("@value", Value);
            cmd.Parameters.AddWithValue("@name", Name);

            try
            {
                cmd.ExecuteNonQuery();
                _isNewRecord = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            db.closeConnection();
        }

        public static AppSettings findByName(string name){
            AppSettings s = new AppSettings();
            db.openConnection();
            string qr = "SELECT * FROM setting WHERE setting_name=@name";

            SqlCommand cmd = new SqlCommand(qr, db.conn);
            cmd.Parameters.AddWithValue("@name", name);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                s.Name = reader["setting_name"].ToString();
                s.Value = reader["setting_value"].ToString();
                s._isNewRecord = false;
            }
            else
            {
                s.Name = name;
                s._isNewRecord = true;
            }

            reader.Close();
            db.closeConnection();
            return s;
        }
	}
}
