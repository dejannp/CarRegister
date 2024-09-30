using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace carregistersystem
{
    public partial class carmanuf : Form
    {
        string username, password, provider, db;
        public string name, serial;

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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        SqlConnection conn = new SqlConnection();
       

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedRow = dataGridView1.SelectedRows[0];

                    if (selectedRow.Cells[0].Value != null)
                    {
                        Name = selectedRow.Cells[0].Value.ToString();
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

        public carmanuf()
        {
            InitializeComponent();
        }

        private void carmanuf_Load(object sender, EventArgs e)
        {

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
               

                string carmanufquerytext = "SELECT Name as 'Name', SerialNum as 'Serial' FROM CarManuf ";
                SqlDataAdapter carmanufda = new SqlDataAdapter(carmanufquerytext, conn);
                DataTable carmanufdt = new DataTable();
                carmanufda.Fill(carmanufdt);
                dataGridView1.DataSource = carmanufdt;





            }

            catch (Exception ex)
            {

            }











        }

        private void ShowForm(FormAction modus)
        {

            managecarmanuf mcf = new managecarmanuf();
            if (modus == FormAction.CarManufDel | modus == FormAction.CarManufEdit)
            {
                if (Name == "" | serial == "")
                {
                    MessageBox.Show("You must select car manufacturer");
                    return;


                }

                else
                {

                    mcf.name = Name;
                    mcf.serial = serial;
                   mcf.modus=modus;
                    mcf.ShowDialog();
                }


            }

            if (modus == FormAction.CarManufAdd)
            {
                mcf.modus = modus;

                mcf.ShowDialog();
            }

        }








    }
}
