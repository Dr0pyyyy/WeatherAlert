using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WeatherAlert
{
    public partial class DeleteForm : Form
    {
        public DeleteForm()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public DataGridView Dgv { get; set; }

        private void btn_yes_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in Dgv.SelectedRows)
                {
                    //Need to handle exception when is no data in gridview
                    string filePath = @"data\" + row.Cells[0].Value.ToString() + ".csv";
                    Dgv.Rows.RemoveAt(row.Index);
                    File.Delete(filePath);
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
