using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace carregistersystem
{
    public partial class carmodel : Form
    {
        string username, password, provider, db;
        int carmodelid, carmanufid, generatevin;
        public string name, vin,fueltype, CarManufName;

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarModelDel);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(CarManufName + name  + vin + fueltype);
            ShowForm(FormAction.CarModelEdit);
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
                        CarManufName = selectedRow.Cells[0].Value.ToString();
                    }
                    if (selectedRow.Cells[1].Value != null)
                    {
                        name = selectedRow.Cells[1].Value.ToString();
                    }

                    if (selectedRow.Cells[2].Value != null)
                    {
                        vin = selectedRow.Cells[2].Value.ToString();
                    }
                    if (selectedRow.Cells[3].Value != null)
                    {
                        fueltype = selectedRow.Cells[3].Value.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarModelAdd);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }



      

        
         








       

        public bool update = false;
        SqlConnection conn = new SqlConnection();

        public carmodel()
        {
            InitializeComponent();
        }

        private void carmodel_Load(object sender, EventArgs e)
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
            deleteToolStripMenuItem.Image = Bitmap.FromFile(@"..\\..\\include\\iks.bmp");
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

                string carmodelquerytext = "SELECT CarManufName as 'Car manufacturer', Name as 'Name', VIN as 'VIN', FuelType as 'Fuel type'  FROM CarModel ";
                SqlDataAdapter carmodelda = new SqlDataAdapter(carmodelquerytext, conn);
                DataTable carmodeldt = new DataTable();
                carmodelda.Fill(carmodeldt);
                dataGridView1.DataSource = carmodeldt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }











        }


        


        private void ShowForm(FormAction modus)
        {
            frmeditcarmodel cm = new frmeditcarmodel();

            
           

            if (modus == FormAction.CarModelDel || modus == FormAction.CarModelEdit)
            {
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(vin))
                {
                    MessageBox.Show("You must select a car model.");
                    return;
                }
                else
                {
                    cm.modus = modus;
                    cm.ShowDialog();

                }
            }

            // if is adding?
            if (modus == FormAction.CarModelAdd)
            {
                cm.modus = modus;
                cm.ShowDialog();

                
            }
        }




    }
}
