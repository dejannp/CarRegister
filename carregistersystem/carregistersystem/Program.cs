using System;
using System.Windows.Forms;

namespace carregistersystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            
            login loginForm = new login();

            if (loginForm.ShowDialog() == DialogResult.OK)  // if login succed
            {
                while (true)
                {
                    
                    selectmenu selectMenuForm = new selectmenu();

                    var result = selectMenuForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        ///open carmanuf
                        carmanuf carmanufForm = new carmanuf();
                        
                            carmanufForm.ShowDialog();
                        
                    }
                    else if (result == DialogResult.Ignore)
                    {
                       //open carmodel
                        carmodel carmodelform = new carmodel();
                        
                            carmodelform.ShowDialog();
                        
                    }
                    else
                    {
                        //break loop
                        break;
                    }
                }
            }
        }
    }
}
