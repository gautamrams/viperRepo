using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace weak_password_finder
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetProcessDPIAware();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Environment.OSVersion.Version.Major >=6)
            SetProcessDPIAware();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            
            Application.Run(new Form1());
        }
        
    }
}
