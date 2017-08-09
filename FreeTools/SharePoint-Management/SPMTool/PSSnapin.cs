using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;
using System.Windows.Forms;

namespace SPMTool
{
    [RunInstaller(true)]
    public class SPMToolSnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "SPMTool"; }
        }
        public override string Vendor
        {
            get { return "adventnet"; }
        }
        public override string VendorResource
        {
            get { return "SPMTool,adventnet"; }
        }
        public override string Description
        {
            get { return "It will provide important reports to the administrator"; }
        }
        public override string DescriptionResource
        {
            get { return "SPMTool,Registers the CmdLets and Providers in this assembly"; }
        }
    }
    [Cmdlet(VerbsCommon.Get, "SPMTool", SupportsShouldProcess = true)]
    public class SPMTool : Cmdlet
    {

        #region Parameters
        /*
        [Parameter(Position = 0,
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Help Text")]
        [ValidateNotNullOrEmpty]
        public string Name
        {
            
        }
 */
        #endregion

        protected override void ProcessRecord()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form2());                
            }
            catch (Exception e)
            {
                MessageBox.Show("Problem in running form: "+e);
            }
        }
    }

}
