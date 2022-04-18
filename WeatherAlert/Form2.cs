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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            foreach (string file in Directory.GetFiles("data", "*.csv"))
            {
                //This works, but i have to add everything in array in foreach
                if (txt_box_username.Text == "" || txt_box_username.Text + ".csv" == Path.GetFileName(file))
                {
                    MessageBox.Show("Invalid name!");
                    return;
                }
            }

            File.Create(@"data\" + txt_box_username.Text + ".csv");
            this.Close();
            return;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
