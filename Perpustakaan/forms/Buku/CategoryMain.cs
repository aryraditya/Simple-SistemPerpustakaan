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
    public partial class CategoryMain : Form
    {

        dbPerpus db = new dbPerpus();
        private void initGridView()
        {
            DataGridViewImageButtonDeleteColumn columnDelete = new DataGridViewImageButtonDeleteColumn();
            DataGridViewImageButtonUpdateColumn columnUpdate = new DataGridViewImageButtonUpdateColumn();
            columnDelete.Width = 20;
            columnUpdate.Width = 20;
            dataGridView1.Columns.Add("category_id", "Id Category");
            dataGridView1.Columns.Add("category_name", "Category Name");
            dataGridView1.Columns.Add(columnUpdate);
            dataGridView1.Columns.Add(columnDelete);
            dataGridView1.Columns["category_id"].Width = 100;
            dataGridView1.Columns["category_name"].Width = 330;
        }
        public void fillGridView()
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            initGridView();
            List<BookCategory> bl = BookCategory.findAll(textBox1.Text);
            foreach (BookCategory bls in bl)
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells["category_id"].Value = bls.id;
                dataGridView1.Rows[n].Cells["category_name"].Value = bls.name;
            }
        }
        private void frm_closed(object sender, EventArgs e)
        {
            fillGridView();
        }

        public CategoryMain()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            fillGridView();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int idCategory = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            if (e.ColumnIndex == 2)
            {
                forms.Buku.CategoryForm frm = new forms.Buku.CategoryForm(idCategory);
                frm.Show();
                frm.FormClosed += new FormClosedEventHandler(frm_closed);
            }
            else
            {
                if (MessageBox.Show("Are you sure want to delete this item ?", "Delete category", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    BookCategory model = BookCategory.findByPk(idCategory);
                    model.delete();
                    fillGridView();
                }
            }
        }

        private void CategoryMain_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            forms.Buku.CategoryForm frm = new forms.Buku.CategoryForm();
            frm.Show();
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            fillGridView();
        }

    }
}
