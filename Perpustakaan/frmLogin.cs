using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Perpustakaan
{
    public partial class frmLogin : Form
    {

        dbPerpus db = new dbPerpus();

        public frmLogin()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            db.openConnection();
            String username, password;
            username = textBox1.Text;
            password = textBox2.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Please fill username and password", "Login Failed");
                return;
            }

            String sqlQuery = "SELECT * FROM tbl_user WHERE user_name=@username and user_password=@password and user_status=1";
            SqlCommand cmd = new SqlCommand(sqlQuery, db.conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", db.CalculateMD5Hash(password));
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                user.name = reader["user_name"].ToString();
                user.id = int.Parse(reader["user_id"].ToString());
                frmMain fmain = new frmMain();
                fmain.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Login failed");
            }

            db.closeConnection();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Application.Exit();
        }
    }
}
