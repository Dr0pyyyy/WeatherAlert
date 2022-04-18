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
    public partial class RenameForm : Form
    {
        public RenameForm()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        public DataGridView Dgv { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in Dgv.SelectedRows)
            {
                foreach (string file in Directory.GetFiles("data", "*.csv"))
                {
                    //This works, but i have to add everything in array in foreach
                    if (txt_box_new_username.Text == "")
                    {
                        MessageBox.Show("Invalid name!");
                        return;
                    }
                    else if (txt_box_new_username.Text + ".csv" == Path.GetFileName(file))
                    {
                        MessageBox.Show("This username already exist!");
                        return;
                    }
                }
                File.Move(@"data\" + row.Cells[0].Value.ToString() + ".csv",@"data\" + txt_box_new_username.Text + ".csv");
                row.Cells[0].Value = txt_box_new_username.Text;
                this.Close();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
