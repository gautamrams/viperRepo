using System;
using System.Collections.Generic;
//using System.Linq;
using System.Windows.Forms;
using System.Management.Automation;
using System.ComponentModel;
using System.Configuration.Install;

namespace LocalUserManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
       //     Application.EnableVisualStyles();
       //     Application.SetCompatibleTextRenderingDefault(false);
       //     Application.Run(new Form1());
            
        }
    }

    [RunInstaller(true)]
    public class TestPSSnapin : PSSnapIn
    {
        public override string Name
        {
            get
            {
                return "LocalUserManagement";
            }
        }

        // Vendor information for the PowerShell snap-in.
        public override string Vendor
        {
            get
            {
                return "ManageEngine";
            }
        }

        // Description of the PowerShell snap-in
        public override string Description
        {
            get
            {
                return "This cmdlet helps to add,remove,set properties for the Local Users";
            }
        }
    } // TestPSSnapin Class


    [Cmdlet(VerbsCommon.Get, "LocalUserManagement")]
    public class TestCmdlet : Cmdlet
    {
        protected override void ProcessRecord()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    } // TestCmdlet Class
}
