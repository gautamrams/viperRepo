using System.Management.Automation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SendGreetingcheck
{
    [Cmdlet(VerbsCommon.Get, "LastLogon", SupportsShouldProcess = true)]
    public class Sendgreeting : Cmdlet
    {

        /*[Parameter(Mandatory = true, Position = 0)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string name;*/



        protected override void ProcessRecord()
        {
            try
            {
                //WriteObject("Hello " + name + "!");
                LastLogonTool objForm = new LastLogonTool();
                objForm.BringToFront();
                objForm.ShowDialog();
            }
            catch (Exception)
            {
            }
        }
    }
}