using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using System.ComponentModel;
using System.Configuration.Install;
 

namespace ActiveDirectoryReplicationManager
{
    [Cmdlet ("get","replicationprocess")]
    public class ReplicaProcess : Cmdlet 
    {
        protected override void ProcessRecord()
        {
            try
            {
                Form1 frm = new Form1();
                //ReplicationManager rm = new ReplicationManager();
                frm.BringToFront();
                frm.ShowDialog();
                //rm.BringToFront();
                //rm.ShowDialog();
            }
            catch (Exception eeex)
            {
                WriteObject(eeex.Message.ToString());
            }
        }

    }

    [RunInstaller(true)]
    public class mysnappin : PSSnapIn
    {
        public override string Name
        {
            get { return "ReplicationManager"; }
        }

        public override string Vendor
        {
            get { return "Zoho"; }
        }

        public override string Description
        {
            get { return "Manually Force The Replication Process Between Domain Controllers In The Domain"; }
        }
    }
}
