using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Perpustakaan.forms.Buku
{
    public partial class CategoryForm : Form
    {

        BookCategory bc;
        private int Id;

        public CategoryForm(int _id=0)
        {
            InitializeComponent();
            Id = _id;
            if (Id != 0)
            {
                bc = BookCategory.findByPk(Id);
                TxtKategori.Text = bc.name;
                this.Text = "Update Kategori - "+bc.id;
            }
            else
            {
                bc = new BookCategory();
                this.Text = "Tambah Kategori Baru";
            }
        }

        private void CategoryForm_Load(object sender, EventArgs e)
        {

        }

        private void BtnSimpan_Click(object sender, EventArgs e)
        {
            bc.name = TxtKategori.Text;
            bc.save();
            MessageBox.Show("Data Sudah Disimpan");
            if(System.Windows.Forms.Application.OpenForms["CategoryMain"] != null)
                (System.Windows.Forms.Application.OpenForms["CategoryMain"] as CategoryMain).fillGridView();

            this.Close();
        }
    }
}
