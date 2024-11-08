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
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace carregistersystem
{
    public partial class frmCarModel : Form
    {
        public bool update = false;
        string username, password, provider, db;
        public int carmodelid, carmanufid;
        public string name, vin,fueltype, CarManufName;
        SqlConnection conn = new SqlConnection();

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(FormAction.CarModelDel);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
           //debug msg MessageBox.Show(CarManufName + name  + vin + fueltype);
            ShowForm(FormAction.CarModelEdit);
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    carmodelid = (int)dataGridView1.SelectedRows[0].Cells["clmnCarModelId"].Value;
                    carmanufid = (int)dataGridView1.SelectedRows[0].Cells["clmnCarManufId"].Value;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //search to do

            string sFilter;
           sFilter = GetSearchFilter();

            FillDataGridWithData(sFilter);
        }

        private string GetSearchFilter()
        {
         
            string search = txtSearch.Text.Trim();
            search = Regex.Replace(search, @"['""@$(){}[+?!#]", "");

           
            List<string> searchContent = search.Split(' ').ToList();
            List<string> searchConditions = new List<string>();

          
            foreach (string keyword in searchContent)
            {
                
                if (string.IsNullOrWhiteSpace(keyword)) continue;

                
                List<string> conditions = new List<string>
        {
            $"cmf.Name LIKE '%{keyword}%'",
            $"cmf.SerialNum LIKE '%{keyword}%'",
            $"(cmf.SerialNum + '-' + cmf.Name) LIKE '%{keyword}%'",
            $"cm.[Name] LIKE '%{keyword}%'",
            $"cm.[FuelType] LIKE '%{keyword}%'",
            $"cm.[VIN] LIKE '%{keyword}%'"
        };

                
                searchConditions.Add("(" + string.Join(" OR ", conditions) + ")");
            }

          
            string searchQueryText = searchConditions.Count > 0
                ? "AND " + string.Join(" AND ", searchConditions)
                : "";

            return searchQueryText;
        }


        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                button1_Click(sender,e);

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

        public frmCarModel()
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

            ConfigConnection.Initialize();
            username = ConfigConnection.username;
            password = ConfigConnection.password;
            db = ConfigConnection.database;
            provider = ConfigConnection.provider;

            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";

            FillDataGridWithData();
        }


        


        private void ShowForm(FormAction modus)
        {
            frmEditCarModel cm = new frmEditCarModel();

            cm.FormClosed += new FormClosedEventHandler(frmeditcarmodel_FormClosed);

            if (modus == FormAction.CarModelDel || modus == FormAction.CarModelEdit)
            {
                if (carmodelid == 0)
                {
                    MessageBox.Show("You must select a car model.");
                    return;
                }
                else
                {
                    cm.carmodelid = carmodelid;
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

            //FillDataGridWithData();
        }



        private void frmeditcarmodel_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmEditCarModel cm = sender as frmEditCarModel;

            if (cm != null)
            {
                if (cm.UpdateFlag)
                {
                    FillDataGridWithData();

                }

            }
        }

        private void FillDataGridWithData(string searchFilter = "")
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            string carmodelquerytext = $@"
        SELECT  
            cm.[Id] AS CarModelId,
            ISNULL(cm.[CarManufId], 0) AS CarManufId,
            ISNULL(cm.[Name], '') AS Name,
            ISNULL(cm.[FuelType], '') AS FuelType,
            ISNULL(cm.[VIN], '') AS VIN,
            cmf.SerialNum + '-' + cmf.Name AS CarManufName 
        FROM 
            [CarRegister].[dbo].[CarModel] cm
        LEFT JOIN 
            CarRegister.dbo.CarManuf cmf ON cmf.Id = cm.CarManufId
        WHERE 
            cmf.Active = 1 {searchFilter}";

            SqlDataAdapter carmodelda = new SqlDataAdapter(carmodelquerytext, conn);
            DataTable carmodeldt = new DataTable();
            carmodelda.Fill(carmodeldt);
            dataGridView1.DataSource = carmodeldt;
        }
    }
}
