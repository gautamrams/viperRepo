using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;

namespace GetDomains
{
    [RunInstaller(true)]
    public class GetDomainsSnapIn : PSSnapIn
    {
        public override string Name
        {
            get { return "GetDomains"; }
        }
        public override string Vendor
        {
            get { return "AdventNet Inc"; }
        }
        public override string VendorResource
        {
            get { return "GetDomains,AdventNet Inc"; }
        }
        public override string Description
        {
            get { return "Get all the domain names and domain controllers"; }
        }
        public override string DescriptionResource
        {
            get { return "GetDomains,Registers the CmdLets and Providers in this assembly"; }
        }
    }
}
