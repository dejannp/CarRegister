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

namespace carregistersystem
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void login_Load(object sender, EventArgs e)
        {
         
            IniFile ini = new IniFile(@"..\\..\\include\\config.ini");
           
             /*INI FILE PATH CHECK
            string read;
            read = ini.Read("DatabaseConfig", "username");
            label1.Text = read;
           */






        }
    }
}
