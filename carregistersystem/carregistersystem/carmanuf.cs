using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace carregistersystem
{
    public partial class carmanuf : Form
    {
        string username, password, provider, db;
        public string name, serial;
        public bool update = false;
        SqlConnection conn = new SqlConnection();

        public carmanuf()
        {
            InitializeComponent();
        }

        private void carmanuf_Load(object sender, EventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            // MessageBox.Show(update.ToString());


            addToolStripMenuItem.Image = Bitmap.FromFile(@"..\\..\\include\\plus.bmp");
            editToolStripMenuItem.Image = Bitmap.FromFile(@"..\\..\\include\\pen.bmp");
            deleteToolStripMenuItem.Image=Bitmap.FromFile(@"..\\..\\include\\iks.bmp");
            exitToolStripMenuItem.Image = Bitmap.FromFile(@"..\\..\\include\\exit.bmp");

            IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
            provider = ini.Read("DatabaseConfig", "Server");
            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            db = ini.Read("DatabaseConfig", "dbupiti");

            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";

            try
            {
                conn.Close();
                conn.Open();

                string carmanufquerytext = "SELECT Name as 'Name', SerialNum as 'Serial' FROM CarManuf WHERE Active=1";
                SqlDataAdapter carmanufda = new SqlDataAdapter(carmanufquerytext, conn);
                DataTable carmanufdt = new DataTable();
                carmanufda.Fill(carmanufdt);
                dataGridView1.DataSource = carmanufdt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarManufAdd);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarManufEdit);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarManufDel);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            string searchquery = "SELECT Name, SerialNum FROM CarManuf WHERE ( ";
            List<string> searchcontent = search.Split(' ').ToList();
            int counter = searchcontent.Count;




            for (int o = 0; o < counter; o++)
            {
                searchquery = searchquery + " Name LIKE '%" + searchcontent[o] + "%'  ";

                if (o < counter - 1)
                {
                    searchquery = searchquery + " OR";
                }

            }
            searchquery = searchquery + ") AND (";



            for (int o = 0; o < counter; o++)
            {
                searchquery = searchquery + " SerialNum LIKE '%" + searchcontent[o] + "%'";

                if (o < counter - 1)
                {
                    searchquery = searchquery + " OR";
                }

            }
            searchquery = searchquery + ") AND Active=1";

            
           

            SqlDataAdapter da = new SqlDataAdapter(searchquery, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;









        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dataGridView1.SelectedRows[0];

                    if (selectedRow.Cells[0].Value != null)
                    {
                        name = selectedRow.Cells[0].Value.ToString();
                    }
                    if (selectedRow.Cells[1].Value != null)
                    {
                        serial = selectedRow.Cells[1].Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void ShowForm(FormAction modus)
        {
            managecarmanuf mcf = new managecarmanuf();

         
            mcf.FormClosed += new FormClosedEventHandler(managecarmanuf_FormClosed);

            if (modus == FormAction.CarManufDel || modus == FormAction.CarManufEdit)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(serial))
                {
                    MessageBox.Show("You must select a car manufacturer.");
                    return;
                }
                else
                {
                    mcf.name = name;
                    mcf.serial = serial;
                    mcf.modus = modus;
                    mcf.ShowDialog();
                }
            }

            // Ako je dodavanje
            if (modus == FormAction.CarManufAdd)
            {
                mcf.modus = modus;
                mcf.ShowDialog();
            }
        }

        private void managecarmanuf_FormClosed(object sender, FormClosedEventArgs e)
        {
            managecarmanuf mcmf = sender as managecarmanuf;
            if (mcmf != null)
            {
           
                if (mcmf.UpdateFlag)
                {
                    update = true;  
                   // MessageBox.Show(" " + update.ToString());

                  
                    string carmanufquerytext = "SELECT Name as 'Name', SerialNum as 'Serial' FROM CarManuf WHERE Active=1";
                    SqlDataAdapter carmanufda = new SqlDataAdapter(carmanufquerytext, conn);
                    DataTable carmanufdt = new DataTable();
                    carmanufda.Fill(carmanufdt);
                    dataGridView1.DataSource = carmanufdt;
                }
            }
        }
    }
}
