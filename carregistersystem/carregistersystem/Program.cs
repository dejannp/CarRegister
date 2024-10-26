using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace carregistersystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //  Application.Run(new login());


          //first open loginfrm
            login loginForm = new login();
            if (loginForm.ShowDialog() == DialogResult.OK)  // Ako je login uspješan
            {
                // if  db login succed open selectmenuform 
                selectmenu selectMenuForm = new selectmenu();
                if (selectMenuForm.ShowDialog() == DialogResult.OK)
                {
                  // open carmanufform
                    carmanuf carmanufForm = new carmanuf();
                    carmanufForm.ShowDialog();
                }
            }



        }
    }
}
