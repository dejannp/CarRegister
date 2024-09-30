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
using System.Data.SqlClient;
using System.IO;

namespace carregistersystem
{

    
    public partial class login : Form
    {
        IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
        string username, provider, password,db;
        bool databaseCreated;

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

                selectmenu sm = new selectmenu();
                sm.ShowDialog();

            }
            catch
            {
                MessageBox.Show("Error!");

            }

            databaseCreated = Convert.ToBoolean(ini.Read("DatabaseConfig", "DatabaseCreated"));

            if (!databaseCreated)



            {
                //kreirajbazu
                string createdbquerytext,createtablesquerytext;
                createdbquerytext = File.ReadAllText(@"..\\..\\include\\createdb.txt");
                createtablesquerytext = File.ReadAllText(@"..\\..\\include\\createtables.txt");


                SqlCommand createdbquery = new SqlCommand(createdbquerytext,conn);
                SqlCommand createtablesquery = new SqlCommand(createtablesquerytext, conn);

                createdbquery.ExecuteNonQuery();
                createtablesquery.ExecuteNonQuery();




                ini.UpdateProperty("DatabaseCreated", "true", "DatabaseConfig");



                conn.Close();







            }

            conn.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
         
            

            /*INI FILE PATH CHECK
           string read;
           read = ini.Read("DatabaseConfig", "username");
           label1.Text = read;
          */

            username = ini.Read("DatabaseConfig", "Username");
            password = ini.Read("DatabaseConfig", "Password");
            db = ini.Read("DatabaseConfig", "Database");
            provider = ini.Read("DatabaseConfig","Server");

            textBox1.Text = provider;
            textBox2.Text = username;
            textBox3.Text = password;




        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
