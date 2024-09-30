using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carregistersystem
{
    public partial class managecarmanuf : Form
    {
        public FormAction modus;
        public string name, serial;


        public managecarmanuf()
        {
            InitializeComponent();
        }

        private void managecarmanuf_Load(object sender, EventArgs e)
        {
            MessageBox.Show("WORK MODE" + modus.ToString());
            textBox1.Text = name;
            textBox2.Text = serial;
        }
    }
}
