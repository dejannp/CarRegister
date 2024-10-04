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
        int checknamequeryresult = 0, checkserialqueryresult = 0;
        SqlConnection conn = new SqlConnection();
        public FormAction modus;
        public string name, serial;
        string username, password, db, provider;
        SqlCommand checknamequery, checkserialquery;
        public bool UpdateFlag { get; set; } = false;

        private void carmanufexistscheck(out int checkquerynameresult, out int checkserialqueryresult)
        {
            switch (modus)
            {
                case FormAction.CarManufAdd:
                    name = textBox1.Text;
                    serial = textBox2.Text;
                    break;
            }

            checknamequery = conn.CreateCommand();
            checknamequery.CommandType = CommandType.Text;
            checknamequery.Parameters.AddWithValue("@Name", name);
            checknamequery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE Name=@Name";

            checkserialquery = conn.CreateCommand();
            checkserialquery.CommandType = CommandType.Text;
            checkserialquery.Parameters.AddWithValue("@Serial", serial);
            checkserialquery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE SerialNum=@Serial";

            object nameResultObj = checknamequery.ExecuteScalar();
            object serialResultObj = checkserialquery.ExecuteScalar();

            checkquerynameresult = (nameResultObj != null) ? Convert.ToInt32(nameResultObj) : 0;
            checkserialqueryresult = (serialResultObj != null) ? Convert.ToInt32(serialResultObj) : 0;
        }

        public managecarmanuf()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void managecarmanuf_FormClosed(object sender, FormClosedEventArgs e)
        {

            UpdateFlag = true;



        }

        private void button1_Click(object sender, EventArgs e)
        {
            carmanufexistscheck(out checknamequeryresult, out checkserialqueryresult);

            try
            {
                switch (modus)
                {
                    case FormAction.CarManufAdd:

                        if (textBox2.Text.Length > 5)
                        {
                            MessageBox.Show("The serial number cannot be more than 5 characters long.");
                        }
                        else
                        {
                            addcarmanuf();
                        }
                        break;

                    case FormAction.CarManufEdit:
                        editcarmanuf();
                        break;

                    case FormAction.CarManufDel:
                        //debug msg MessageBox.Show(checkserialqueryresult.ToString() + checkserialqueryresult.ToString());

                        deletcarmanuf();
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

        private void deletcarmanuf()
        {
            if (checknamequeryresult > 0 && checkserialqueryresult > 0)
            {
                SqlCommand deletequery = conn.CreateCommand();
                deletequery.CommandType = CommandType.Text;
                deletequery.Parameters.AddWithValue("@Name", name);
                deletequery.Parameters.AddWithValue("@serial", serial);
                deletequery.Parameters.AddWithValue("@inactive", false);
                deletequery.CommandText = "UPDATE CarManuf SET Active=@inactive Where Name=@Name AND SerialNum=@serial";
                deletequery.ExecuteNonQuery();

                MessageBox.Show("Car manufacturer deleted successfully!");
            }
            else
            {
                MessageBox.Show("The car manufacturer you are trying to delete is not found in the database.");
            }
        }

        private void editcarmanuf()
        {
            if (checknamequeryresult > 0 && checkserialqueryresult > 0)
            {
                editcheckmethod();

                if (checknamequeryresult > 0)
                {
                    MessageBox.Show("Manufacturer name already exists.");
                }
                else if (checkserialqueryresult > 0)
                {
                    MessageBox.Show("Serial number already exists.");
                }
                else if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("All fields must be filled!");
                }
                else
                {
                   //Debug message MessageBox.Show(checknamequeryresult.ToString() + " " + checkserialqueryresult.ToString());

                    SqlCommand editquery = conn.CreateCommand();
                    editquery.CommandType = CommandType.Text;
                    editquery.Parameters.AddWithValue("@Name", name);
                    editquery.Parameters.AddWithValue("@serial", serial);
                    editquery.Parameters.AddWithValue("@Nameedit", textBox1.Text);
                    editquery.Parameters.AddWithValue("@serialedit", textBox2.Text);

                    editquery.CommandText = "UPDATE CarManuf SET Name=@Nameedit, SerialNum=@serialedit Where Name=@Name AND SerialNum=@serial";
                    editquery.ExecuteNonQuery();

                    MessageBox.Show("Car manufacturer edited successfully!");

                    textBox1.ReadOnly = true;
                    textBox2.ReadOnly = true;
                }
            }
            else
            {
                MessageBox.Show("The car manufacturer you are trying to edit is not found in the database.");
            }
        }

        private void addcarmanuf()
        {
            

            SqlCommand addquery = conn.CreateCommand();
            addquery.CommandType = CommandType.Text;
            addquery.Parameters.AddWithValue("@Name", textBox1.Text);
            addquery.Parameters.AddWithValue("@Serial", textBox2.Text);
            addquery.Parameters.AddWithValue("@Active", true);
            addquery.CommandText = "INSERT INTO CarManuf (Name, SerialNum, Active) VALUES (@Name, @Serial, @Active)";

            if (checknamequeryresult > 0)
            {
                MessageBox.Show("Manufacturer name already exists.");
            }
            else if (checkserialqueryresult > 0)
            {
                MessageBox.Show("Serial number already exists.");
            }
            else if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("All fields must be filled!");
            }
            else
            {
                addquery.ExecuteNonQuery();
                MessageBox.Show("New car manufacturer added successfully!");
            }
        }

        private void editcheckmethod()
        {
            checknamequery = conn.CreateCommand();
            checknamequery.CommandType = CommandType.Text;
            checknamequery.Parameters.AddWithValue("@Name", textBox1.Text);
            checknamequery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE Name=@Name";

            checkserialquery = conn.CreateCommand();
            checkserialquery.CommandType = CommandType.Text;
            checkserialquery.Parameters.AddWithValue("@Serial", textBox2.Text);
            checkserialquery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE SerialNum=@Serial";

            object nameResultObj = checknamequery.ExecuteScalar();
            object serialResultObj = checkserialquery.ExecuteScalar();

            checknamequeryresult = (nameResultObj != null) ? Convert.ToInt32(nameResultObj) : 0;
            checkserialqueryresult = (serialResultObj != null) ? Convert.ToInt32(serialResultObj) : 0;
        }

        private void managecarmanuf_Load(object sender, EventArgs e)
        {
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
                carmanufexistscheck(out checknamequeryresult, out checkserialqueryresult);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error" + ex.ToString());
            }
        }
    }
}
