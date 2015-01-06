using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Perpustakaan
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            dbPerpus db = new dbPerpus();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           // Application.Run(new forms.Buku.BukuCreateUpdate(23));
            Application.Run(new frmLogin());
        }
    }
}
