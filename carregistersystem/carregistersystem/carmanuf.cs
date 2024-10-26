using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.IO;

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
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
             
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
            ///
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
            update = false;
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarManufEdit);
            update = false;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarManufDel);
            update = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string search = textBox1.Text;
            List<string> searchContent = search.Split(' ').ToList();
            int counter = searchContent.Count;

           
            string searchQueryText = "SELECT Name, SerialNum As 'Serial' FROM CarManuf WHERE Active = 1 AND (";

            // making parameters list from search for SQL query
            List<string> nameParameters = new List<string>();
            List<string> serialNumParamters = new List<string>();

            for (int i = 0; i < counter; i++)
            {
                nameParameters.Add($"Name LIKE @search{i}");
                serialNumParamters.Add($"SerialNum LIKE @search{i}");
            }

            
            searchQueryText += "(" + string.Join(" OR ", nameParameters) + ") OR (" + string.Join(" OR ", serialNumParamters) + "))";

            SqlCommand searchquery = conn.CreateCommand();
            searchquery.CommandType = CommandType.Text;
            searchquery.CommandText = searchQueryText;

                //Adding parameters for search
                for (int i = 0; i < counter; i++)
                {
                    //MessageBox.Show(searchContent[i].ToString());
                    searchquery.Parameters.AddWithValue($"@search{i}", "%" + searchContent[i] + "%");
                }

                SqlDataAdapter searchDa = new SqlDataAdapter(searchquery);
                DataTable searchDt = new DataTable();
                searchDa.Fill(searchDt);
                dataGridView1.DataSource = searchDt;
            

           //DEBUG File.WriteAllText("beton",searchQueryText.ToString());
            //File.WriteAllText("beton", searchQuery);
     










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
            frmEditCarManuf mcf = new frmEditCarManuf();

         
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

          // if is adding?
            if (modus == FormAction.CarManufAdd)
            {
                mcf.modus = modus;
                mcf.ShowDialog();
            }
        }

        private void managecarmanuf_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmEditCarManuf mcmf = sender as frmEditCarManuf;
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
