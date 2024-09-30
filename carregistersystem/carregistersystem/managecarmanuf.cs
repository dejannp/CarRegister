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

    public partial class managecarmanuf : Form
    {
        int checknamequeryresult = 0, checkserialqueryresult=0;
        SqlConnection conn = new SqlConnection();
        public FormAction modus;
        public string name, serial;
        string username, password, db, provider;
        

        public managecarmanuf()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                switch (modus)
                {
                    case FormAction.CarManufAdd:

                        SqlCommand addquery = conn.CreateCommand();
                        addquery.CommandType = CommandType.Text;
                        addquery.Parameters.AddWithValue("@Name", textBox1.Text);
                        addquery.Parameters.AddWithValue("@Serial", textBox2.Text);
                        addquery.CommandText = "INSERT INTO CarManuf (Name, SerialNum) VALUES (@Name, @Serial)";

                       
                        SqlCommand checknamequery = conn.CreateCommand();
                        checknamequery.CommandType = CommandType.Text;
                        checknamequery.Parameters.AddWithValue("@Name", textBox1.Text);
                        checknamequery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE Name=@Name";

                     
                        SqlCommand checkserialquery = conn.CreateCommand();
                        checkserialquery.CommandType = CommandType.Text;
                        checkserialquery.Parameters.AddWithValue("@Serial", textBox2.Text);
                        checkserialquery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE SerialNum=@Serial";

                       
                        object nameResultObj = checknamequery.ExecuteScalar();
                        object serialResultObj = checkserialquery.ExecuteScalar();

                        int checknamequeryresult = (nameResultObj != null) ? Convert.ToInt32(nameResultObj) : 0;
                        int checkserialqueryresult = (serialResultObj != null) ? Convert.ToInt32(serialResultObj) : 0;

                        
                        if (checknamequeryresult > 0)
                        {
                            MessageBox.Show("Manufacturer name already exists.");
                        }
                        else if (checkserialqueryresult > 0)
                        {
                            MessageBox.Show("Serial number already exists.");
                        }
                        else
                        {
                          
                            addquery.ExecuteNonQuery();
                            MessageBox.Show("Record successfully inserted.");
                        }

                        break;

                    case FormAction.CarManufEdit:
                       
                        break;

                    case FormAction.CarManufDel:
                       
                        break;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }



        private void managecarmanuf_Load(object sender, EventArgs e)
        {
           // debug message MessageBox.Show("WORK MODE" + modus.ToString());
            textBox1.Text = name;
            textBox2.Text = serial;

            IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
            provider = ini.Read("DatabaseConfig", "Server");
            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            db = ini.Read("DatabaseConfig", "dbupiti");

            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";

       
            switch (modus)
            {

                case FormAction.CarManufAdd:

                 
                    button1.Text = "Add";
                
                break;

              case FormAction.CarManufEdit:
                   
                    button1.Text = "Edit";


                    break;

                case FormAction.CarManufDel:
                    button1.Text = "DELETE";
                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                    break;

}
            try
            {
                conn.Open();
            }
            catch(Exception ex)
            {
                //MessageBox.Show("Error" + ex.ToString());
            }









        }
    }
}
