using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;

namespace Perpustakaan
{
    static class user
    {
        public static int id
        {
            get;
            set;
        }

        public static String name
        {
            get;
            set;
        }
        public static void DestroySession()
        {
            user.name = null;
            user.id = 0;
        }
    }
    class dbPerpus
    {
        public SqlConnection conn = new SqlConnection(Properties.Settings.Default.perpusConnectionString);

        public void openConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        public void closeConnection()
        {
            conn.Close();
        }



        public string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
