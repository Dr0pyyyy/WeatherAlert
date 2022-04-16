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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        DataTable table = new DataTable();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Setting up default column names
            table.Columns.Add("Username", typeof(string));
            table.Columns.Add("Place", typeof(string));
            table.Columns.Add("Longitude", typeof(string));
            table.Columns.Add("Latitude", typeof(string));

            dataGridView1.DataSource = table;

            if (!Directory.Exists("data"))
            {
                Directory.CreateDirectory("data");
            }

            //reads every file
            foreach (string file in Directory.GetFiles("data", "*.csv"))
            {
                //reads every line of each file
                string[] lines = File.ReadAllLines(file);
                string[] values;

                for (int i = 0; i < lines.Length; i++)
                {
                    //Fills row of database with file data
                    string[] username = Path.GetFileName(file).Split('.');
                    lines[i] = username[0] + ";" + lines[i];
                    values = lines[i].ToString().Split(';');
                    string[] row = new string[values.Length];

                    for (int j = 0; j < values.Length; j++)
                    {
                        row[j] = values[j].Trim();
                    }

                    table.Rows.Add(row);
                }
            }
        }

        private void btn_new_profile_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_rename_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
        }

        private void btn_load_Click(object sender, EventArgs e)
        {

        }
    }
}
