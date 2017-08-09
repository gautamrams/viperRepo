using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace GetDuplicates
{
    [RunInstaller(true)]
    public class GetDuplicatesSnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "GetDuplicates"; }
        }
        public override string Vendor
        {
            get { return "AdventNet Inc"; }
        }
        public override string VendorResource
        {
            get { return "PowerShell,AdventNet Inc"; }
        }
        public override string Description
        {
            get { return "Get all the duplicate names"; }
        }
        public override string DescriptionResource
        {
            get { return "GetDuplicates,Registers the CmdLets and Providers in this assembly"; }
        }
    }
}
