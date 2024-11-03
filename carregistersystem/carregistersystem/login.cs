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
using System.IO;

namespace carregistersystem
{
    public partial class login : Form
    {
        IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
        string username, provider, password, db;
        bool databaseCreated;

        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            db = ini.Read("DatabaseConfig", "Database");
            provider = ini.Read("DatabaseConfig", "Server");

            textBox1.Text = provider;
            textBox2.Text = username;
            textBox3.Text = password;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ini.UpdateProperty("Server", textBox1.Text, "DatabaseConfig");
            ini.UpdateProperty("Username", textBox2.Text, "DatabaseConfig");
            ini.UpdateProperty("Password", textBox3.Text, "DatabaseConfig");

            provider = ini.Read("DatabaseConfig", "Server");
            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            db = ini.Read("DatabaseConfig", "Database");

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";

            try
            {
                conn.Open();

                databaseCreated = Convert.ToBoolean(ini.Read("DatabaseConfig", "DatabaseCreated"));

                if (!databaseCreated)
                {
                    
                    string createdbquerytext = File.ReadAllText(@"..\\..\\include\\createdb.txt");
                    string createtablesquerytext = File.ReadAllText(@"..\\..\\include\\createtables.txt");

                    SqlCommand createdbquery = new SqlCommand(createdbquerytext, conn);
                    SqlCommand createtablesquery = new SqlCommand(createtablesquerytext, conn);

                    
                    createdbquery.ExecuteNonQuery();
                    createtablesquery.ExecuteNonQuery();

                    
                    ini.UpdateProperty("DatabaseCreated", "true", "DatabaseConfig");
                }

                conn.Close();

                // Is database created?
                bool databaseAvailable = false;
                for (int i = 0; i < 5; i++)  // checking
                {
                    if (IsDatabaseAvailable())
                    {
                        databaseAvailable = true;
                        break;
                    }
                    System.Threading.Thread.Sleep(2000); // pause
                }

                if (databaseAvailable)
                {
                    
                    

                   

                    this.DialogResult = DialogResult.OK;  
                    this.Close();  



                }
                else
                {
                    MessageBox.Show("Database is not available yet. Please try again.");
                }
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

        private bool IsDatabaseAvailable()
        {
            try
            {
                // Connection creation
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = $"Data Source={provider};Initial Catalog={db};User id={username};Password={password};";
                    conn.Open();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
