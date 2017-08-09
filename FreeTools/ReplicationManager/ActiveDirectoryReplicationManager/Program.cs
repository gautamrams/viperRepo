using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Configuration.Install;
using System.Management.Automation;

namespace ActiveDirectoryReplicationManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new ReplicationManager()); 
            //Application.Run(new Form1());
        }
    }

    [RunInstaller(true)]
    public class TestPSSnapin : PSSnapIn
    {
        public override string Name
        {
            get
            {
                return "TerminalSessionManager";
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
                return "This cmdlet helps to list,Disconnect and Logoff the Terminal Sessions";
            }
        }
    } // TestPSSnapin Class


    [Cmdlet(VerbsCommon.Get, "TerminalSessionManager")]
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
