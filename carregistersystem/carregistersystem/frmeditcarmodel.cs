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
   
    public partial class frmeditcarmodel : Form
    {
        public FormAction modus;
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

            switch(modus)

            {
                case FormAction.CarModelAdd:

                    button1.Text = "ADD";
                    break;

                case FormAction.CarModelEdit:
                    button1.Text = "EDIT";
                    break;

                case FormAction.CarModelDel:
                    button1.Text = "DELETE";
                    break;





            }











        }
    }
}
