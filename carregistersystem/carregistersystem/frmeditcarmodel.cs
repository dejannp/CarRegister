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
   
    public partial class frmeditcarmodel : Form
    {
        SqlConnection conn = new SqlConnection();
      
        string username, password, db, provider;
        public FormAction modus;
        public string carmodelname, fueltype, carmanufname;
        string selectedcarmanuf, selectedcarmanufname;
        int selectedcarmanufid=0,checkname=0;
        public bool UpdateFlag { get; set; } = false;
        bool changesmade = false;


        private void button1_Click(object sender, EventArgs e)
        {

            switch (modus)

            {
                case FormAction.CarModelAdd:

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


                    string addquerytext = "INSERT INTO CarModel  (CarManufId,Name,FuelType,VIN,CarManufName) VALUES (@CarManufId,@Name,@FuelType,@VIN,@CarManufName)";
                    SqlCommand addquery = conn.CreateCommand();
                    addquery.CommandType = CommandType.Text;
                    addquery.CommandText = addquerytext;

                    selectedcarmanuf = comboBox1.Text;


                    selectedcarmanufname = selectedcarmanuf.Substring(selectedcarmanuf.IndexOf("-") + 1);

                    // MessageBox.Show(selectedcarmanufid + "-" + selectedcarmanufname);


                    int checkname = this.checknamefunction();

                    /// MessageBox.Show(checkname.ToString());




                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        MessageBox.Show("You have not entered a model name");
                        return;
                    }
                    if ((radioButton1.Checked == false && radioButton2.Checked == false))
                    {
                        MessageBox.Show("You have to choose the fuel type!");
                        return;
                    }
                    if (string.IsNullOrEmpty(comboBox1.Text))
                    {
                        MessageBox.Show("You have did not choose a model car manufacturer");
                        return;
                    }
                    if (checkname > 0)
                    {
                        MessageBox.Show("Car model with that name already exists!");
                        return;
                    }




                    else
                    {
                        if (radioButton1.Checked == true)
                        {
                            fueltype = "Diesel";
                        }
                        if (radioButton2.Checked == true)
                        {
                            fueltype = "Petrol";
                        }

                         selectedcarmanufid = getselectedcarmanufid();

                        addquery.Parameters.AddWithValue("@CarManufId", selectedcarmanufid);
                        // MessageBox.Show(selectedcarmanufid);
                        addquery.Parameters.AddWithValue("@Name", textBox1.Text);
                        addquery.Parameters.AddWithValue("@FuelType", fueltype);
                        addquery.Parameters.AddWithValue("VIN", vinNumber);
                        addquery.Parameters.AddWithValue("CarManufName", selectedcarmanufname);

                        addquery.ExecuteNonQuery();
                        changesmade = true;
                        MessageBox.Show("Car model succesfully added!");
                        this.Close();
                    }




                    break;

                case FormAction.CarModelEdit:


                    if (radioButton1.Checked == true)
                    {
                        fueltype = "Diesel";
                    }
                    if (radioButton2.Checked == true)
                    {
                        fueltype = "Petrol";
                    }


                     checkname= checknamefunction();
                   // MessageBox.Show(checkname.ToString());
                    string editquerytext = "UPDATE CarModel SET Name=@Name, FuelType=@FuelType, CarManufName=@CarManufName, CarManufId=@CarManufId WHERE Id=@ID ";

                    SqlCommand editquery = conn.CreateCommand();
                    editquery.CommandType = CommandType.Text;
                    editquery.CommandText = editquerytext;

                     selectedcarmanufid = getselectedcarmanufid();


                    string querytext = "SELECT Name FROM CarManuf WHERE Id=@Id";
                    SqlCommand carmanufqueryname = conn.CreateCommand();
                    carmanufqueryname.CommandType = CommandType.Text;
                    carmanufqueryname.CommandText = querytext;
                    carmanufqueryname.Parameters.AddWithValue("@Id", selectedcarmanufid);
                    string carmanufname=(string)carmanufqueryname.ExecuteScalar();

                   // MessageBox.Show(checkname.ToString());

                    editquery.Parameters.AddWithValue("@CarManufId", selectedcarmanufid);
                    editquery.Parameters.AddWithValue("@Name", textBox1.Text);
                    editquery.Parameters.AddWithValue("@Id", carmodelid);
                    editquery.Parameters.AddWithValue("@FuelType", fueltype);
                    editquery.Parameters.AddWithValue("CarManufName", carmanufname);

                    if (checkname > 0)
                    {
                        if (textBox1.Text == carmodelname)
                        {
                            MessageBox.Show("No changes were made!");
                        }
                        else
                        {

                            MessageBox.Show("Edited name can not be existing!");
                        }
                        }

                    else
                    {
                        editquery.ExecuteNonQuery();
                        MessageBox.Show("Edit succesfull!");
                        changesmade = true;
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

        private void frmeditcarmodel_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (changesmade)
            {
                UpdateFlag = true;
            }





        }

        private int checknamefunction()
        {
             checkname = -1;

            string chekcnamequerytext = "SELECT * FROM CarModel WHERE Name=@Name";
            SqlCommand checknamequery = conn.CreateCommand();
            checknamequery.CommandType = CommandType.Text;
            checknamequery.CommandText = chekcnamequerytext;
            checknamequery.Parameters.AddWithValue("@Name", textBox1.Text);


            var resultchecknamequery = checknamequery.ExecuteScalar();
            checkname = resultchecknamequery != null ? (int)resultchecknamequery : -1;
            return checkname;
        }

        private int getselectedcarmanufid()
        {
            selectedcarmanuf = comboBox1.Text;

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
        public frmeditcarmodel()
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

        private void frmeditcarmodel_Load(object sender, EventArgs e)
        {

        //    MessageBox.Show(carmodelid.ToString());
          
            IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
            provider = ini.Read("DatabaseConfig", "Server");
            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            db = ini.Read("DatabaseConfig", "dbupiti");

            //added MultipleActiveResultSets=True for reading two readers in same time
            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};MultipleActiveResultSets=True";
            


            string carmanufnamequerytext = "SELECT Name FROM CarManuf WHERE Active=1";
            string carmanufserialnumquerytext = "SELECT SerialNum FROM CarManuf  WHERE Active=1";

            
            SqlCommand carmanufnamequery = conn.CreateCommand();
            SqlCommand carmanufserialnumquery = conn.CreateCommand();

            carmanufnamequery.CommandType = CommandType.Text;
            carmanufserialnumquery.CommandType = CommandType.Text;

            carmanufnamequery.CommandText = carmanufnamequerytext;
            carmanufserialnumquery.CommandText = carmanufserialnumquerytext;
            



            try
            {
                conn.Open();

                SqlDataReader namereader = carmanufnamequery.ExecuteReader();
                SqlDataReader serialnumreader = carmanufserialnumquery.ExecuteReader();

                while (namereader.Read() && serialnumreader.Read())
                {
                    string combocontent = serialnumreader["SerialNum"].ToString() + "-" + namereader["Name"].ToString();
                    comboBox1.Items.Add(combocontent);
                  
                    bool check = false;

                   


                    //MessageBox.Show(combocontent + " " + " " + carmanufname );
                   //  check = combocontent.Contains("-" + carmanufname);

                    if (check)
                    {
                       // MessageBox.Show("HEJ");
                        //comboBox1.SelectedItem = combocontent;

                    }



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            






            switch (modus)

            {
                case FormAction.CarModelAdd:


                    









                    button1.Text = "ADD";
                    break;

                case FormAction.CarModelEdit:
                    button1.Text = "EDIT";
                    textBox1.Text = carmodelname;
                    if (fueltype == "Diesel")
                    {
                        radioButton1.Checked = true;
                    }

                    if (fueltype == "Petrol")
                    {
                        radioButton2.Checked = true;
                    }

                    foreach (var item in comboBox1.Items)
                    {
                        string comboboxitem = item.ToString();
                        if (comboboxitem.Contains(carmanufname))
                        {
                            comboBox1.Text = comboboxitem;
                            break;
                        }

                    }

                    break;

                case FormAction.CarModelDel:

                    foreach (var item in comboBox1.Items)
                    {
                        string comboboxitem = item.ToString();
                        if (comboboxitem.Contains(carmanufname))
                        {
                            comboBox1.Text = comboboxitem;
                            break;
                        }

                    }
                    textBox1.Text = carmodelname;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    textBox1.Enabled = false;
                    comboBox1.Enabled = false;
                    button1.Text = "DELETE";

                    if (fueltype == "Diesel")
                    {
                        radioButton1.Checked = true;
                    }

                    if (fueltype == "Petrol")
                    {
                        radioButton2.Checked = true;
                    }

                    break;





            }











        }
    }
}
