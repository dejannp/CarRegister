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
   
    public partial class frmEditCarModel : Form
    {
        SqlConnection conn = new SqlConnection();

        string username, password, db, provider, vinNumber;
        public FormAction modus;
        string carmodelname, fueltype, carmanufname;
        string selectedcarmanuf, selectedcarmanufname;
        int selectedcarmanufid=0,checkname=0;
        public bool UpdateFlag { get; set; } = false;
        public int selectedCarModelId { get; set; } = 0;
        bool changesmade = false;





        private void frmeditcarmodel_Load(object sender, EventArgs e)
        {
            cmbManuf.DropDownStyle = ComboBoxStyle.DropDownList;

            //    MessageBox.Show(carmodelid.ToString());

            ConfigConnection.Initialize();
            username = ConfigConnection.username;
            password = ConfigConnection.password;
            db = ConfigConnection.database;
            provider = ConfigConnection.provider;



            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";

            try
            {
                CmbInitalization();
                
                if (modus != FormAction.CarModelAdd)
                {
                    readData();
                }
                else
                {
                    vinNumber = GenerateVIN();
                    txtVIN.Text = vinNumber;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            switch (modus)

            {
                case FormAction.CarModelAdd:

                    this.Text += "-Add";
                    btnSave.Text = "ADD";
                    break;

                case FormAction.CarModelEdit:
                    btnSave.Text = "EDIT";
                    this.Text += "-Edit";
                    break;

                case FormAction.CarModelDel:

                    this.Text += "-Delete";
                    btnRefreshVIN.Enabled = false;
                    rbDiesel.Enabled = false;
                    rbPetrol.Enabled = false;
                    txtName.Enabled = false;
                    cmbManuf.Enabled = false;
                    btnSave.Text = "DELETE";
                    txtVIN.Enabled = false;
                    break;
            }

            fueltype = rbDiesel.Checked ? "Diesel" : "Petrol";

        }

        private void readData()
        {
            string sQ = @"SELECT  cm.[Id] As CarModelId
                                          , ISNULL(cm.[CarManufId], 0) As CarManufId
                                          ,ISNULL(cm.[Name], '') As Name
                                          ,ISNULL(cm.[FuelType],'') As FuelType
                                          ,ISNULL(cm.[VIN], '') As VIN
                                            , ISNULL (cmf.[Name],'') As CarManufName 
                                      FROM [CarRegister].[dbo].[CarModel] cm 
                                    LEFT JOIN CarRegister.dbo.CarManuf cmf ON cmf.Id= cm.CarManufId
                                      WHERE cm.Id=@Id";

            SqlCommand cmdCarManufComboSelection = conn.CreateCommand();
            cmdCarManufComboSelection.CommandType = CommandType.Text;
            cmdCarManufComboSelection.CommandText = sQ;
            cmdCarManufComboSelection.Parameters.AddWithValue("@Id", carmodelid);








            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }


            SqlDataAdapter carmanufda = new SqlDataAdapter(cmdCarManufComboSelection);
            DataTable carmanufDT = new DataTable();
            carmanufda.Fill(carmanufDT);

            if (carmanufDT.Rows.Count > 0)
            {

                txtName.Text = carmanufDT.Rows[0]["Name"].ToString();
                cmbManuf.SelectedValue = carmanufDT.Rows[0]["CarManufId"];
                txtVIN.Text = carmanufDT.Rows[0]["VIN"].ToString();
                carmanufname = carmanufDT.Rows[0]["CarManufName"].ToString();
                carmodelname = carmanufDT.Rows[0]["Name"].ToString();
                vinNumber = carmanufDT.Rows[0]["VIN"].ToString();

             

                switch (carmanufDT.Rows[0]["FuelType"].ToString())
                {
                    case "Diesel":
                        rbDiesel.Checked = true;
                        break;

                    case "Petrol":
                        rbPetrol.Checked = true;
                        break;

                    default:

                        throw new NotImplementedException();
                }
            }





        }

        private void button1_Click(object sender, EventArgs e)
        {

            switch (modus)

            {
                case FormAction.CarModelAdd:

              

                    string addquerytext = "INSERT INTO CarModel  (CarManufId,Name,FuelType,VIN) VALUES (@CarManufId,@Name,@FuelType,@VIN)";
                    SqlCommand addquery = conn.CreateCommand();
                    addquery.CommandType = CommandType.Text;
                    addquery.CommandText = addquerytext;

                    selectedcarmanuf = cmbManuf.Text;


                    selectedcarmanufname = selectedcarmanuf.Substring(selectedcarmanuf.IndexOf("-") + 1);

                    // MessageBox.Show(selectedcarmanufid + "-" + selectedcarmanufname);


                    int checkname = this.checknamefunction();

                    /// MessageBox.Show(checkname.ToString());




                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("You have not entered a model name");
                        return;
                    }
                  
                    if (string.IsNullOrEmpty(cmbManuf.Text))
                    {
                        MessageBox.Show("You have did not choose a model car manufacturer");
                        return;
                    }
                    if (checkname > 0)
                    {
                        MessageBox.Show("Car model with that name already exists!");
                        MessageBox.Show(checkname.ToString());
                        return;
                    }




                    else
                    {
                        if (rbDiesel.Checked == true)
                        {
                            fueltype = "Diesel";
                        }
                        if (rbPetrol.Checked == true)
                        {
                            fueltype = "Petrol";
                        }

                        selectedcarmanufid = getselectedcarmanufid();

                        addquery.Parameters.AddWithValue("@CarManufId", selectedcarmanufid);
                        // MessageBox.Show(selectedcarmanufid);
                        addquery.Parameters.AddWithValue("@Name", txtName.Text);
                        addquery.Parameters.AddWithValue("@FuelType", fueltype);
                        addquery.Parameters.AddWithValue("VIN", vinNumber);

                        addquery.ExecuteNonQuery();
                        changesmade = true;
                       
                        this.Close();
                    }




                    break;

                case FormAction.CarModelEdit:
                    bool editmade = false;

                    editmade = false;


                     checkname= checknamefunction();
                   // MessageBox.Show(checkname.ToString());
                    string editquerytext = "UPDATE CarModel SET Name=@Name, FuelType=@FuelType,CarManufId=@CarManufId,VIN=@VIN WHERE Id=@ID ";

                    SqlCommand editquery = conn.CreateCommand();
                    editquery.CommandType = CommandType.Text;
                    editquery.CommandText = editquerytext;

                     selectedcarmanufid = getselectedcarmanufid();


                    string querytext = "SELECT Name FROM CarManuf WHERE Id=@Id";
                    SqlCommand carmanufqueryname = conn.CreateCommand();
                    carmanufqueryname.CommandType = CommandType.Text;
                    carmanufqueryname.CommandText = querytext;
                    carmanufqueryname.Parameters.AddWithValue("@Id", selectedcarmanufid);
                    string curentcarmanufname=(string)carmanufqueryname.ExecuteScalar();

                  
                    // MessageBox.Show(checkname.ToString());

                    string curentfueltype = " ";

                    if (rbDiesel.Checked == true)
                    {
                        curentfueltype = "Diesel";
                    }
                    if (rbPetrol.Checked == true)
                    {
                        curentfueltype = "Petrol";
                    }

                    editquery.Parameters.AddWithValue("@CarManufId", selectedcarmanufid);
                    editquery.Parameters.AddWithValue("@Name", txtName.Text);
                    editquery.Parameters.AddWithValue("@Id", carmodelid);
                    editquery.Parameters.AddWithValue("@FuelType", curentfueltype);
                    editquery.Parameters.AddWithValue("@VIN", txtVIN.Text);


                    if ((!txtName.Text.Equals(carmodelname)  || !cmbManuf.Text.Contains("-" + carmanufname)   || !curentfueltype.Equals(fueltype))&&checkname==0||!txtVIN.Text.Equals(vinNumber))
                    {
                       
                        editmade = true;
                    }
                    else
                    {
                        if (checkname > 0)
                        {
                            MessageBox.Show(" The new name already belongs to an existing record");
                        }
                        else
                        {
                            MessageBox.Show("In order to edit car model you must change any property!");
                            
                        }
                        editmade = false;
                    }

                    if (editmade )
                    {

                        editquery.ExecuteNonQuery();
                      
                        changesmade = true;
                        editmade = false;
                        this.Close();

                    }
                    break;

                case FormAction.CarModelDel:

                    //debug purposes MessageBox.Show(carmodelid.ToString());
                    string deletecarmodelquerytext = "DELETE FROM CarModel WHERE Id=@Id";
                    SqlCommand deletecarmodelquery = conn.CreateCommand();
                    deletecarmodelquery.CommandType = CommandType.Text;
                    deletecarmodelquery.CommandText = deletecarmodelquerytext;
                    deletecarmodelquery.Parameters.AddWithValue("@Id", carmodelid);
                    deletecarmodelquery.ExecuteNonQuery();
                    changesmade = true;
                    this.Close();

                    break;

            }

        }

        private string GenerateVIN()
        {
            Random randomVIN = new Random();

            //using this way to generate 13 numbar vin because random doesn t accept long number as argument
            string vinNumber = randomVIN.Next(1000000, 9999999).ToString() + randomVIN.Next(100000, 999999).ToString();
            //Debug info  MessageBox.Show(vinNumber);


            string checkvin = "SELECT * FROM CarModel WHERE VIN=@vin";
            SqlCommand checkvinquery = conn.CreateCommand();
            checkvinquery.CommandType = CommandType.Text;

            checkvinquery.CommandText = checkvin;

            checkvinquery.Parameters.AddWithValue("@vin", vinNumber);

            //ensuring that same VIN doesen t exist
            while ((int)checkvinquery.ExecuteNonQuery() > 0)
            {
                vinNumber = randomVIN.Next(1000000, 9999999).ToString() + randomVIN.Next(100000, 999999).ToString();
                break;
            }

            return vinNumber;
        }

        private void btnRefreshVIN_Click(object sender, EventArgs e)
        {
            txtVIN.Text = GenerateVIN();
        }

        private void txtName_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(txtName, true, true, true, false);

            }
        }

        private void cmbManuf_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectNextControl(cmbManuf, true, true, true, false);

            }
        }

        private void frmeditcarmodel_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (changesmade)
            {
                selectedCarModelId = carmodelid;
                UpdateFlag = true;
            }





        }

        private int checknamefunction()
        {
            checkname = 0; 

            string chekcnamequerytext = "SELECT COUNT(*) FROM CarModel WHERE Name=@Name"; 

            if (modus == FormAction.CarModelEdit)
            {
                chekcnamequerytext += " AND Id != @Id"; 
            }

            SqlCommand checknamequery = conn.CreateCommand();
            checknamequery.CommandType = CommandType.Text;
            checknamequery.CommandText = chekcnamequerytext;
            checknamequery.Parameters.AddWithValue("@Name", txtName.Text);

            if (modus == FormAction.CarModelEdit)
            {
                checknamequery.Parameters.AddWithValue("@Id", carmodelid);
            }

         
            int count = (int)checknamequery.ExecuteScalar(); 

            return count; 
        }

        private int getselectedcarmanufid()
        {
            selectedcarmanuf = cmbManuf.Text;

            if (string.IsNullOrEmpty(selectedcarmanuf))
            {
                MessageBox.Show("You have to choose car manufacturer!");
            }
            else
            {

                selectedcarmanufname = selectedcarmanuf.Substring(selectedcarmanuf.IndexOf("-") + 1);
                string selectedcarmanufidquerytext = "SELECT Id FROM CarManuf WHERE Name = @Name";

                SqlCommand selectedcarmanufidquery = conn.CreateCommand();

                selectedcarmanufidquery.CommandType = CommandType.Text;
                selectedcarmanufidquery.CommandText = selectedcarmanufidquerytext;
                selectedcarmanufidquery.Parameters.AddWithValue("@Name", selectedcarmanufname);

                selectedcarmanufid = (int)selectedcarmanufidquery.ExecuteScalar();
                

            }
            return selectedcarmanufid;
        }

        public int carmodelid, carmanufid;
        public frmEditCarModel()
        {

            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void CmbInitalization()
        {
            SqlCommand cmdCarManufComboSelection = conn.CreateCommand();
            cmdCarManufComboSelection.CommandType = CommandType.Text;
            cmdCarManufComboSelection.CommandText = "Select Id As CarManufId, SerialNum + '-' + Name As SerialNumName FROM CarManuf WHERE Active=1";

            conn.Open();

            SqlDataAdapter carmanufda = new SqlDataAdapter(cmdCarManufComboSelection);
            DataTable carmanufDT = new DataTable();
            carmanufda.Fill(carmanufDT);

            cmbManuf.DataSource = carmanufDT;
            cmbManuf.DisplayMember = "SerialNumName";
            cmbManuf.ValueMember = "CarManufId";
        }
    }
}
