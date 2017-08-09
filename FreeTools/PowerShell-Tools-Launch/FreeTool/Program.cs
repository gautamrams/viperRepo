using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FreeTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message, "Free AD Tool Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
  
     }
}