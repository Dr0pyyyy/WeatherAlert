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
            //It works, but when user add new file, it wont show in datagridview, bcs theres no data init
            try
            {
                foreach (string file in Directory.GetFiles("data", "*.csv"))
                {
                    Path.GetFileName(file);
                    if (txt_box_username.Text == "" || txt_box_username.Text == Path.GetFileName(file))
                    {
                        MessageBox.Show("Invalid username");
                        return;
                    }
                    else
                    {
                        File.Create("data/" + txt_box_username.Text + ".csv");
                        this.Close();
                    }
                }

                /*
                if (txt_box_username.Text == "")
                {
                    MessageBox.Show("Invalid username!");
                }
                else
                {
                    File.Create("data/" + txt_box_username.Text + ".csv");
                    this.Close();
                }*/
            }
            catch 
            {
                MessageBox.Show("This file already exist!");
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
