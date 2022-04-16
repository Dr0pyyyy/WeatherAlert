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
    public partial class Form4 : Form
    {
        public Form4()
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
                //Works fine, but need to change file name also
                File.Move(@"data\" + row.Cells[0].Value.ToString() + ".csv",@"data\" + txt_box_new_username.Text + ".csv");
                row.Cells[0].Value = txt_box_new_username.Text;
                this.Close();
            }
        }
    }
}
