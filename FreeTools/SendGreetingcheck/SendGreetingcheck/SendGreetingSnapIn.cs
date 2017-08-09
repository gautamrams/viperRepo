using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text.RegularExpressions;
using System.Configuration.Install;
using System.Collections.ObjectModel;
using System.Collections;
using System.ComponentModel;
#region Snap-in
namespace SendGreetingcheck
{
    [RunInstaller(true)]

    public class SendGreetingSnapIn : CustomPSSnapIn
    {
        private Collection<CmdletConfigurationEntry> cmdlets = new Collection<CmdletConfigurationEntry>();
     private Collection<ProviderConfigurationEntry> providers = new Collection<ProviderConfigurationEntry>();
     private Collection<TypeConfigurationEntry> types = new Collection<TypeConfigurationEntry>();
     private Collection<FormatConfigurationEntry> formats = new Collection<FormatConfigurationEntry>();

        public SendGreetingSnapIn()
            : base()
     {
          cmdlets.Add(new CmdletConfigurationEntry("Get-LastLogon", typeof(Sendgreeting), null));
     }

     public override string Name
     {
         get { return "GetLastLogon"; }
     }

     public override string Vendor
     {
          get { return "BdsSoft"; }
     }

     public override string Description
     {
         get { return "This snap-in contains the Get-LastLogon cmdlet."; }
     }

     public override Collection<CmdletConfigurationEntry> Cmdlets
     {
          get { return cmdlets; }
     }

     public override Collection<ProviderConfigurationEntry> Providers
     {
          get { return providers; }
     }

     public override Collection<TypeConfigurationEntry> Types
     {
          get { return types; }
     }

     public override Collection<FormatConfigurationEntry> Formats
     {
          get { return formats; }
     }
    }
}
#endregion
