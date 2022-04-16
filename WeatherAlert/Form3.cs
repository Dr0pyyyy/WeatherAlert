using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeatherAlert
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_yes_Click(object sender, EventArgs e)
        {
            //figure out how to delete data from gridview and than how to delete file as well
            Form1 form1 = new Form1();
            try
            {
                foreach (DataGridViewRow item in form1.dataGridView.SelectedRows)
                {
                    //form1.dataGridView.Rows.RemoveAt(item.Index);
                }
            }
            catch
            {
                MessageBox.Show("No data selected");
            }
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
