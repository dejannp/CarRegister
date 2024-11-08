using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace carregistersystem
{
    public partial class frmEditCarManuf : Form
    {
        int checknamequeryresult = 0, checkserialqueryresult = 0;
        SqlConnection conn = new SqlConnection();
        public FormAction modus;
        public string name, serial;
        string username, password, db, provider;
        SqlCommand checknamequery, checkserialquery;
        bool changesmade = false;
        bool errorcheckvalidation=false;
        public bool UpdateFlag { get; set; } = false;

        public frmEditCarManuf()
        {
            InitializeComponent();
        }

        private void carmanufexistscheck(out int checkquerynameresult, out int checkserialqueryresult)
        {
            // when opening managecarmanuf form via add menustrip from carmanuf form, values name and serial are null
            if (modus == FormAction.CarManufAdd)
            {
                name = txtName.Text;
                serial = txtSerial.Text;

                // checking if name exists
                checknamequery = conn.CreateCommand();
                checknamequery.CommandType = CommandType.Text;
                checknamequery.Parameters.AddWithValue("@Name", name);
                checknamequery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE Name=@Name";

                // checking if serial exists
                checkserialquery = conn.CreateCommand();
                checkserialquery.CommandType = CommandType.Text;
                checkserialquery.Parameters.AddWithValue("@Serial", serial);
                checkserialquery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE SerialNum=@Serial";

                object nameResultObj = checknamequery.ExecuteScalar();
                object serialResultObj = checkserialquery.ExecuteScalar();

                checkquerynameresult = (nameResultObj != null) ? Convert.ToInt32(nameResultObj) : 0;
                checkserialqueryresult = (serialResultObj != null) ? Convert.ToInt32(serialResultObj) : 0;
            }
            else
            {
                checkquerynameresult = 0;
                checkserialqueryresult = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (modus == FormAction.CarManufAdd || modus == FormAction.CarManufEdit)
            {
                // Check if serial number is valid
                if (string.IsNullOrEmpty(txtSerial.Text) || txtSerial.Text.Length !=5)
                {
                    MessageBox.Show("The serial number cannot be empty and must be 5 characters long.");
                    return;
                }

                carmanufexistscheck(out checknamequeryresult, out checkserialqueryresult);
            }

            try
            {
                switch (modus)
                {
                    case FormAction.CarManufAdd:
                        addcarmanuf();
                        break;

                    case FormAction.CarManufEdit:
                        editcarmanuf();
                        if (!errorcheckvalidation)
                        {
                            this.Close();
                        }
                        break;

                    case FormAction.CarManufDel:
                        deletcarmanuf();
                        if (!errorcheckvalidation)
                        {
                            this.Close();
                        }
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
            // Perform a fresh check to see if the manufacturer exists before deleting
            SqlCommand checkQuery = conn.CreateCommand();
            checkQuery.CommandType = CommandType.Text;
            checkQuery.Parameters.AddWithValue("@Name", name);
            checkQuery.Parameters.AddWithValue("@Serial", serial);
            checkQuery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE Name=@Name AND SerialNum=@Serial";
            object resultObj = checkQuery.ExecuteScalar();
            int result = (resultObj != null) ? Convert.ToInt32(resultObj) : 0;

            if (result > 0)
            {
                // Manufacturer found, proceed to deactivate
                SqlCommand deleteQuery = conn.CreateCommand();
                deleteQuery.CommandType = CommandType.Text;
                deleteQuery.Parameters.AddWithValue("@Name", name);
                deleteQuery.Parameters.AddWithValue("@Serial", serial);
                deleteQuery.Parameters.AddWithValue("@Inactive", false);
                deleteQuery.CommandText = "UPDATE CarManuf SET Active=@Inactive WHERE Name=@Name AND SerialNum=@Serial";
                deleteQuery.ExecuteNonQuery();
                changesmade = true;
                //MessageBox.Show("Car manufacturer deleted successfully!");
            }
            else
            {
                // Manufacturer not found
                MessageBox.Show("The car manufacturer you are trying to delete is not found in the database.");
                errorcheckvalidation = true;
            }
        }

        private void editcarmanuf()
        {
            // variables for checking changes
            bool isNameChanged = txtName.Text != name;
            bool isSerialChanged = txtSerial.Text != serial;

            if (isNameChanged || isSerialChanged)
            {
                // checking new name if changed
                if (isNameChanged)
                {
                    checknamequery = conn.CreateCommand();
                    checknamequery.CommandType = CommandType.Text;
                    checknamequery.Parameters.AddWithValue("@Name", txtName.Text);
                    checknamequery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE Name=@Name";
                    object nameResultObj = checknamequery.ExecuteScalar();
                    checknamequeryresult = (nameResultObj != null) ? Convert.ToInt32(nameResultObj) : 0;

                    if (checknamequeryresult > 0)
                    {
                        MessageBox.Show("Manufacturer name already exists.");
                        errorcheckvalidation = true;
                        return;
                    }
                }

                // checking new serial if changed
                if (isSerialChanged)
                {
                    checkserialquery = conn.CreateCommand();
                    checkserialquery.CommandType = CommandType.Text;
                    checkserialquery.Parameters.AddWithValue("@Serial", txtSerial.Text);
                    checkserialquery.CommandText = "SELECT COUNT(*) FROM CarManuf WHERE SerialNum=@Serial";
                    object serialResultObj = checkserialquery.ExecuteScalar();
                    checkserialqueryresult = (serialResultObj != null) ? Convert.ToInt32(serialResultObj) : 0;

                    if (checkserialqueryresult > 0)
                    {
                        MessageBox.Show("Serial number already exists.");
                        errorcheckvalidation = true;
                        return;
                    }
                }

                SqlCommand editquery = conn.CreateCommand();
                editquery.CommandType = CommandType.Text;

                if (isNameChanged && !isSerialChanged)
                {
                    // changing only name
                    editquery.Parameters.AddWithValue("@Nameedit", txtName.Text);
                    editquery.Parameters.AddWithValue("@Name", name);
                    editquery.CommandText = "UPDATE CarManuf SET Name=@Nameedit WHERE Name=@Name";
                }
                else if (!isNameChanged && isSerialChanged)
                {
                    // changing only serial
                    editquery.Parameters.AddWithValue("@serialedit", txtSerial.Text);
                    editquery.Parameters.AddWithValue("@serial", serial);
                    editquery.CommandText = "UPDATE CarManuf SET SerialNum=@serialedit WHERE SerialNum=@serial";
                }
                else
                {
                    // changing both
                    editquery.Parameters.AddWithValue("@Nameedit", txtName.Text);
                    editquery.Parameters.AddWithValue("@serialedit", txtSerial.Text);
                    editquery.Parameters.AddWithValue("@Name", name);
                    editquery.Parameters.AddWithValue("@serial", serial);
                    editquery.CommandText = "UPDATE CarManuf SET Name=@Nameedit, SerialNum=@serialedit WHERE Name=@Name AND SerialNum=@serial";
                }

                editquery.ExecuteNonQuery();
                changesmade = true;
                MessageBox.Show("Car manufacturer edited successfully!");

//  unnecesary              txtName.ReadOnly = true;
  //              txtSerial.ReadOnly = true;
            }
        }

        private void addcarmanuf()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtSerial.Text))
            {
                MessageBox.Show("All fields must be filled!");
                return;
            }

            SqlCommand addquery = conn.CreateCommand();
            addquery.CommandType = CommandType.Text;
            addquery.Parameters.AddWithValue("@Name", txtName.Text);
            addquery.Parameters.AddWithValue("@Serial", txtSerial.Text);
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
            else
            {
                addquery.ExecuteNonQuery();
                MessageBox.Show("New car manufacturer added successfully!");
                changesmade = true;
            }
        }

        private void managecarmanuf_Load(object sender, EventArgs e)
        {
            txtName.Text = name;
            txtSerial.Text = serial;

            ConfigConnection.Initialize();
            username = ConfigConnection.username;
            password = ConfigConnection.password;
            db = ConfigConnection.database;
            provider = ConfigConnection.provider;

            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";

            switch (modus)
            {
                case FormAction.CarManufAdd:
                    btnAction.Text = "Add";
                    break;

                case FormAction.CarManufEdit:
                    btnAction.Text = "Edit";
                    break;

                case FormAction.CarManufDel:
                    btnAction.Focus();
                    btnAction.Text = "Delete";
                    txtName.ReadOnly = true;
                    txtSerial.ReadOnly = true;
                    break;
            }

            try
            {
                conn.Open();
                carmanufexistscheck(out checknamequeryresult, out checkserialqueryresult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void managecarmanuf_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (changesmade)
            {
                UpdateFlag = true;
            }
            }
    }
}
