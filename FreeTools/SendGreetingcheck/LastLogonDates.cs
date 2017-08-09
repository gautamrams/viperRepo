using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
namespace SendGreetingcheck
{
    public partial class LastLogonDates : Form
    {
        public LastLogonDates()
        {
            InitializeComponent();
            LastLogonDateslist.View = View.Details;
            LastLogonDateslist.GridLines = true;
            this.SuspendLayout();
            this.Location = new System.Drawing.Point(300, 268);
            this.ResumeLayout();
            
            //LastLogonDateslist.Columns.Add("User Name", 150, HorizontalAlignment.Center);
        }
        public void createColumnHeaders(string[] dcNames)
        {
           
            if (dcNames.Length >= 2)
            {
                LastLogonDateslist.Columns.Add("User Name", 150);
                foreach (string colnHeader in dcNames)
                    LastLogonDateslist.Columns.Add(colnHeader, 205, HorizontalAlignment.Center);
                SetWindowParams(this);
            }
            else if (dcNames.Length < 2)
            {
                LastLogonDateslist.Columns.Add("User Name", 165);
                foreach (string colnHeader in dcNames)
                    LastLogonDateslist.Columns.Add(colnHeader, 240, HorizontalAlignment.Center);
                
            }
        }
        private void SetWindowParams(Form aForm)
        {
           /* short nTop = 1100;
            short nLeft = 1200;
            short nHeight = 400;
            short nWidth = 600;

            aForm.SuspendLayout();
            //aForm.Location = new System.Drawing.Point(nLeft,nTop);
            aForm.Top = nTop;
            aForm.Left = nLeft;*/
            aForm.Size = new System.Drawing.Size(590, 392);
            aForm.ResumeLayout(false);
            //LastLogondateslist.Location = new Point(1500,2000);
            //groupBox1.Location = new Point(50, 50);
            groupBox1.Size = new System.Drawing.Size(565, 295);
            //groupBox1.ResumeLayout(false);
            LastLogonDateslist.Size = new System.Drawing.Size(540, 265);
            //LastLogonDateslist.ResumeLayout(false);
            Close.Location = new Point(255, 342);
            //Close.ResumeLayout(false);

        }
        public void LastLogonlastLogonTimestamp(string username, string domain, string[] dcNames)
        {
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, domain);
            DateTime latestLogon = DateTime.MinValue;
            DateTime[] lastlogonarray = new DateTime[10];
            string servername = null;
            DomainControllerCollection dcc = DomainController.FindAll(context);
            int i = 0;
            foreach (DomainController dc in dcc)
            {
                // latestLogon = DateTime.MinValue;
                DirectorySearcher ds;
                foreach (string reqdcnames in dcNames)
                {
                    using (dc)
                        if (reqdcnames.Equals(dc.ToString()))
                        {
                            using (ds = dc.GetDirectorySearcher())
                            {
                                ds.Filter = String.Format("(sAMAccountName={0})", username);
                                ds.PropertiesToLoad.Add("lastLogonTimestamp");
                                ds.SizeLimit = 1;
                                SearchResult sr = ds.FindOne();
                                if (sr != null)
                                {
                                    DateTime lastLogon = DateTime.MinValue;
                                    if (sr.Properties.Contains("lastLogonTimestamp"))
                                    {
                                        long var = (long)sr.Properties["lastLogonTimestamp"][0];
                                        lastLogon = DateTime.FromFileTime((long)sr.Properties["lastLogonTimestamp"][0]);
                                    }
                                    if (DateTime.Compare(lastLogon, DateTime.MinValue) > 0)
                                    {
                                        lastlogonarray[i] = lastLogon;
                                        i++;
                                        servername = dc.Name;
                                    }
                                }
                            }
                        }
                }
            }
            LastLogonDateslist.Items.Add(username);
            foreach (DateTime last in lastlogonarray)
            {
                long ticks = 0;
                if ((last.Equals(DateTime.FromFileTime(0)) || ticks == last.Ticks))
                    LastLogonDateslist.Items[LastLogonDateslist.Items.Count - 1].SubItems.Add("NO Data");
                else
                {
                    LastLogonDateslist.Items[LastLogonDateslist.Items.Count - 1].SubItems.Add("" + last.ToString());
                }
            }
            

        }
        public void LastLogon(string username, string domain, string[] dcNames)
        {
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, domain);
            DateTime latestLogon = DateTime.MinValue;
            DateTime[] lastlogonarray = new DateTime[10];
            string servername = null;
            DomainControllerCollection dcc = DomainController.FindAll(context);
            int i = 0;
            foreach (DomainController dc in dcc)
            {
                // latestLogon = DateTime.MinValue;
                DirectorySearcher ds;
                foreach (string reqdcnames in dcNames)
                {
                    using (dc)
                        if (reqdcnames.Equals(dc.ToString()))
                        {
                            using (ds = dc.GetDirectorySearcher())
                            {
                                ds.Filter = String.Format("(sAMAccountName={0})", username);
                                ds.PropertiesToLoad.Add("lastLogon");
                                ds.SizeLimit = 1;
                                SearchResult sr = ds.FindOne();
                                if (sr != null)
                                {
                                    DateTime lastLogon = DateTime.MinValue;
                                    if (sr.Properties.Contains("lastLogon"))
                                    {
                                        long var = (long)sr.Properties["lastLogon"][0];
                                        lastLogon = DateTime.FromFileTime((long)sr.Properties["lastLogon"][0]);
                                    }
                                    if (DateTime.Compare(lastLogon, DateTime.MinValue) > 0)
                                    {
                                        lastlogonarray[i] = lastLogon;
                                        i++;
                                        servername = dc.Name;
                                    }
                                }
                            }
                        }
                }
            }
            LastLogonDateslist.Items.Add(username);
            foreach (DateTime last in lastlogonarray)
            {
                long ticks = 0;
                if ((last.Equals(DateTime.FromFileTime(0)) || ticks == last.Ticks))
                    LastLogonDateslist.Items[LastLogonDateslist.Items.Count - 1].SubItems.Add("NO Data");
                else
                {
                    LastLogonDateslist.Items[LastLogonDateslist.Items.Count - 1].SubItems.Add("" + last.ToString());
                }
            }

        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}