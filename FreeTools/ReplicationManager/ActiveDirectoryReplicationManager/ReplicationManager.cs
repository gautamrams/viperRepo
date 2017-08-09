using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Threading;


namespace ActiveDirectoryReplicationManager
{
    public partial class ReplicationManager : Form
    {
        public ReplicationManager()
        {
            InitializeComponent();


            tabControl1.DrawItem += new DrawItemEventHandler(OnDrawItem);
            tabControl2.DrawItem += new DrawItemEventHandler(OnDrawItem1);
        }
        private void OnDrawItem(object sender, DrawItemEventArgs e)
        {

            Graphics g = e.Graphics;

            TabPage tp = tabControl1.TabPages[e.Index];

            Brush br = default(Brush);

            StringFormat sf = new StringFormat();

            RectangleF r = new RectangleF(e.Bounds.X, e.Bounds.Y + 0, e.Bounds.Width, e.Bounds.Height - 0);

            sf.Alignment = StringAlignment.Center;

            string strTitle = tp.Text;

            //If the current index is the Selected Index, change the color

            if (tabControl1.SelectedIndex == e.Index)
            {

                //this is the background color of the tabpage header

                br = new SolidBrush(Color.White);

                // chnge to your choice


                g.FillRectangle(br, e.Bounds);

                //this is the foreground color of the text in the tab header

                br = new SolidBrush(Color.Black);

                // change to your choice

                g.DrawString(strTitle, tabControl1.Font, br, r, sf);

            }

            else
            {

                //these are the colors for the unselected tab pages

                br = new SolidBrush(Color.FromArgb(244, 244, 244));

                // Change this to your preference

                g.FillRectangle(br, e.Bounds);

                br = new SolidBrush(Color.Black);

                g.DrawString(strTitle, tabControl1.Font, br, r, sf);

            }

        }


        private void OnDrawItem1(object sender, DrawItemEventArgs e)
        {
            Graphics g1 = e.Graphics;

            TabPage tp1 = tabControl2.TabPages[e.Index];

            Brush br1 = default(Brush);

            StringFormat sf1 = new StringFormat();

            //RectangleF r = new RectangleF(e.Bounds.X, e.Bounds.Y -0, e.Bounds.Width, e.Bounds.Height +0);
            RectangleF r1 = new RectangleF(e.Bounds.X, e.Bounds.Y + 0, e.Bounds.Width, e.Bounds.Height - 0);
            sf1.Alignment = StringAlignment.Center;

            string strTitle = tp1.Text;

            //If the current index is the Selected Index, change the color

            if (tabControl2.SelectedIndex == e.Index)
            {

                //this is the background color of the tabpage header

                br1 = new SolidBrush(Color.FromArgb(250, 250, 250));

                // chnge to your choice

                g1.FillRectangle(br1, e.Bounds);

                //this is the foreground color of the text in the tab header

                br1 = new SolidBrush(Color.Black);

                // change to your choice

                g1.DrawString(strTitle, tabControl2.Font, br1, r1, sf1);

            }

            else
            {

                //these are the colors for the unselected tab pages

                br1 = new SolidBrush(Color.FromArgb(244, 244, 244));

                // Change this to your preference

                g1.FillRectangle(br1, e.Bounds);

                br1 = new SolidBrush(Color.Black);

                g1.DrawString(strTitle, tabControl2.Font, br1, r1, sf1);

            }

        }


        private void ReplicationManager_Load(object sender, EventArgs e)
        {

            comboBox1.Enabled = false;
            radioButton1.Checked = true;
            // button5.Enabled = false;
            groupBox2.Visible = false;
            //groupBox3.Visible = false;
            //  button12.Enabled = false;
            groupBox5.Visible = false;
            //button10.Enabled = false;


            comboBox1.Items.Clear();

            try
            {
                System.DirectoryServices.ActiveDirectory.Forest currentForest = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();
                System.DirectoryServices.ActiveDirectory.DomainCollection dc = currentForest.Domains;
                foreach (System.DirectoryServices.ActiveDirectory.Domain d in dc)
                {
                    comboBox1.Items.Add(d.Name);

                    foreach (DomainController dc1 in d.DomainControllers)
                    {
                        comboBox2.Items.Add(dc1.Name);
                        comboBox3.Items.Add(dc1.Name);
                    }
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
            //ListDomains();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();


            try
            {
                if (radioButton2.Checked == true)
                {

                    if (this.comboBox1.Text.Equals(""))
                    {
                        MessageBox.Show("Please Select Domain", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }
                    toolStripStatusLabel6.Text = "Replicating '" + comboBox1.Text + "' wait...";

                    this.Cursor = Cursors.WaitCursor;

                    String domaintext = comboBox1.Text;
                    Application.DoEvents();
                    DirectoryContextType contexttype = new DirectoryContextType();
                    DirectoryContext context = new DirectoryContext(contexttype, domaintext);

                    Domain domain = Domain.GetDomain(context);
                    foreach (DomainController dc in DomainController.FindAll(context))
                    {
                        //Domain domain;
                        domain = Domain.GetCurrentDomain();
                        dc.CheckReplicationConsistency();
                        dc.SyncFromAllServersCallback = SyncFromAllServersCallbackDelegate;
                        try
                        {

                            foreach (string partitionName in dc.Partitions)
                            {
                                //MessageBox.Show(" DC name : " + dc.Name + "... Partion : " + partitionName);
                                dc.SyncReplicaFromAllServers(partitionName.ToString(),
                                                    SyncFromAllServersOptions.AbortIfServerUnavailable
                                                    | SyncFromAllServersOptions.CrossSite
                                                    );
                            }
                        }
                        catch (SyncFromAllServersOperationException em)
                        {
                            toolStripStatusLabel6.Text = "Done";
                            MessageBox.Show("Error occurred when synchronizing the Server '" + dc.Name + "'", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;

                        }

                        catch (Exception ex)
                        {
                            toolStripStatusLabel6.Text = "Done";
                            MessageBox.Show("Unspecified Error" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    toolStripStatusLabel6.Text = "Done";
                } // if (radioButton2.Checked == true)

                if (radioButton1.Checked == true)
                {
                    toolStripStatusLabel6.Text = "Replicating 'Entire Forest' wait...";

                    this.Cursor = Cursors.WaitCursor;
                    Application.DoEvents();

                    try
                    {
                        this.listView1.Items.Clear();

                        Domain domain1;
                        try
                        {

                            domain1 = Domain.GetCurrentDomain();
                        }

                        catch (ActiveDirectoryObjectNotFoundException ex)
                        {
                            toolStripStatusLabel6.Text = "Done";
                            MessageBox.Show("Unspecified error occurred" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        //////////////////////////////////////
                        try
                        {
                            foreach (DomainController dc in domain1.DomainControllers)
                            {
                                Domain domain;
                                domain = Domain.GetCurrentDomain();
                                dc.CheckReplicationConsistency();
                                dc.SyncFromAllServersCallback = SyncFromAllServersCallbackDelegate1;
                                try
                                {
                                    foreach (string partitionName in dc.Partitions)
                                    {
                                        dc.SyncReplicaFromAllServers(partitionName.ToString(),
                                                            SyncFromAllServersOptions.AbortIfServerUnavailable
                                                            | SyncFromAllServersOptions.CrossSite
                                                            );
                                    }
                                }
                                catch (SyncFromAllServersOperationException ex)
                                {
                                    toolStripStatusLabel6.Text = "Done";
                                    MessageBox.Show("Error occurred when synchronizing the Server '" + dc.Name + "'", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                                catch (Exception ex)
                                {
                                    toolStripStatusLabel6.Text = "Done";
                                    MessageBox.Show("Unspecified Error" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    this.Cursor = Cursors.Default;
                                    return;
                                }

                                System.DirectoryServices.ActiveDirectory.Forest currentForest = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();
                                System.DirectoryServices.ActiveDirectory.DomainCollection domainCollection = currentForest.Domains;

                                foreach (System.DirectoryServices.ActiveDirectory.Domain dom in domainCollection)
                                {
                                    foreach (DomainController dc2 in dom.DomainControllers)
                                    {
                                        if (!dc.Name.Equals(dc2.Name))
                                        {
                                            toolStripStatusLabel1.Text = "Replicating 'Entire Forest' wait...";
                                            dc2.CheckReplicationConsistency();
                                            dc2.SyncFromAllServersCallback = SyncFromAllServersCallbackDelegate1;
                                            try
                                            {
                                                foreach (string partitionName in dc2.Partitions)
                                                {
                                                    //               MessageBox.Show(" DC name : " + dc.Name + "... dc2 : " + dc2.Name + ".... par : " + partitionName);
                                                    dc2.SyncReplicaFromAllServers(partitionName.ToString(),
                                                                        SyncFromAllServersOptions.AbortIfServerUnavailable
                                                                        | SyncFromAllServersOptions.CrossSite
                                                                        );
                                                }
                                            }
                                            catch (SyncFromAllServersOperationException eexe)
                                            {
                                                toolStripStatusLabel6.Text = "Done";
                                                MessageBox.Show("Error occured when replicating from '" + dc.Name + "' to '" + dc2.Name + "'", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                this.Cursor = Cursors.Default;
                                                return;
                                            }
                                            catch (Exception ex)
                                            {
                                                toolStripStatusLabel6.Text = "Done";
                                                MessageBox.Show("Unspecified error" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                this.Cursor = Cursors.Default;
                                                return;
                                            }
                                        } // if
                                    } // foreach
                                    // } // if
                                } // for each
                            } // foreach
                        }
                        catch (UnauthorizedAccessException)
                        {
                            toolStripStatusLabel6.Text = "Done";
                            MessageBox.Show("You dont have permission for Replication", "Replication Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        catch (ActiveDirectoryServerDownException ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show("The following error occurred during the attempt to contact the domain controller '" + ex.Name + "':\n\nThe RPC Server is unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        catch (Exception ex)
                        {
                            toolStripStatusLabel6.Text = "Done";
                            MessageBox.Show("Unspecified error" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        ////////////////////////////////////////////////
                    }
                    catch (Exception ex)
                    {
                        toolStripStatusLabel6.Text = "Done";
                        MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Cursor = Cursors.Default;
                    }
                    this.Cursor = Cursors.Default;
                    toolStripStatusLabel6.Text = "Done";
                    //button10.Enabled = true;

                } // if 

            }
            catch (UnauthorizedAccessException)
            {
                toolStripStatusLabel6.Text = "Done";
                MessageBox.Show("You dont have permission for Replication", "Replication Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            catch (ActiveDirectoryServerDownException ex)
            {
                this.Cursor = Cursors.Default;
                toolStripStatusLabel7.Text = "Done";
                MessageBox.Show("The following error occurred during the attempt to contact the domain controller '" + ex.Name + "':\n\nThe RPC Server is unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                toolStripStatusLabel6.Text = "Done";
                MessageBox.Show("Unspecified Error" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            this.Cursor = Cursors.Default;
            if (listView1.Items.Count == 0)
            {
                //  button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
            }

        }

        private bool SyncFromAllServersCallbackDelegate(
                                SyncFromAllServersEvent et,
                                string ts,
                                string ss,
                                SyncFromAllServersOperationException eex
                            )
        {


            string a, b, c, d;

            a = et.ToString();


            listView1.Items.Add(a.ToString());

            if (ss != null)
            {
                b = ss.ToString();
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(b.ToString());
            }
            else
            {
                b = "-----";
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(b.ToString());
            }
            if (ts != null)
            {
                c = ts.ToString();
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(c.ToString());

            }
            else
            {
                c = "-----";
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(c.ToString());
            }
            return true;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Nothing to save", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.textBox3.Text = "";
            this.textBox4.Text = "";
            this.textBox5.Text = "";
            this.textBox6.Text = "";
            this.textBox13.Text = "";
            groupBox2.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {

            string dt = DateTime.Now.ToString();

            if (textBox5.Text == null || textBox6.Text == null || textBox13.Text == null || textBox5.Text.Length == 0 || textBox6.Text.Length == 0 || textBox13.Text.Length == 0)
            {
                MessageBox.Show("Please Enter All The Fields", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            toolStripStatusLabel6.Text = "Saving the content to the database, please wait. .";
            //toolStripStatusLabel1.Text = "Saving your data please wait..."; 
            SqlConnection cn = new SqlConnection("Data Source=\'" + (textBox5.Text + ("\';Initial Catalog=\'" + (textBox13.Text + ("\';User ID=\'" + (textBox3.Text + ("\';Password=\'" + (textBox4.Text + ("\';Networ" + "k Library=dbmssocn")))))))));
            try
            {
                //if (checkBox1.Checked != true)
                //{
                SqlDataReader dr;
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SYS.DATABASES where name = '" + textBox13.Text + "'", cn);
                dr = cmd.ExecuteReader();
                if (dr.Read() == true)
                {
                    //MessageBox.Show("Database Name = " + dr["name"]);
                    dr.Close();
                    SqlCommand cmd1 = new SqlCommand("select *from sys.tables where name = '" + textBox6.Text + "'", cn);
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    if (dr1.Read() == true)
                    {

                        //MessageBox.Show("Table Name = " + dr1["name"]);
                        dr1.Close();
                        ListViewItem lv;
                        for (int i = 0; i <= listView1.Items.Count - 1; i++)
                        {
                            try
                            {
                                lv = listView1.Items[i];
                                SqlCommand cmd3 = new SqlCommand("insert into " + textBox6.Text + " values(\'" + lv.ToString() + ("\', \'" + (lv.SubItems[1].ToString() + ("\', \'" + (lv.SubItems[2].ToString() + ("\', \'" + (dt + ("\')"))))))), cn);
                                //SqlCommand cmd = new SqlCommand("insert into replicat_by_domain values(\'" + lv.ToString () + ("\', \'" + (lv.SubItems[1].ToString ()  + ("\', \'" + (lv.SubItems[2].ToString () + ("\')"))))), cn);
                                cmd3.ExecuteNonQuery();

                            }
                            catch (ArgumentOutOfRangeException exx)
                            {
                                MessageBox.Show(exx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        MessageBox.Show("Record Saved in the table '" + textBox6.Text + "' Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        groupBox2.Visible = true;
                    }
                    else
                    {
                        dr1.Close();
                        //MessageBox.Show("Table Not Found");
                        SqlCommand cmd2 = new SqlCommand("create table " + textBox6.Text + "(sno smallint IDENTITY(1,1)PRIMARY KEY CLUSTERED,event_type varchar(100),source_server varchar(500),destination_server varchar(500),replicate_date_time datetime)", cn);
                        cmd2.ExecuteNonQuery();
                        //MessageBox.Show("Table ' " + textBox6.Text + " ' Created Successfully");
                        ListViewItem lv;
                        for (int i = 0; i <= listView1.Items.Count - 1; i++)
                        {
                            try
                            {
                                lv = listView1.Items[i];
                                SqlCommand cmd4 = new SqlCommand("insert into " + textBox6.Text + " values(\'" + lv.ToString() + ("\', \'" + (lv.SubItems[1].ToString() + ("\', \'" + (lv.SubItems[2].ToString() + ("\', \'" + (dt + ("\')"))))))), cn);
                                cmd4.ExecuteNonQuery();
                            }
                            catch (ArgumentOutOfRangeException exx)
                            {
                                MessageBox.Show(exx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        MessageBox.Show("Record Saved in the table '" + textBox6.Text + "' Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //MessageBox.Show("Table ' " + textBox6.Text.ToString () + " ' was created Successfully and Records are Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                }
                else
                {

                    dr.Close();
                    MessageBox.Show("Database Not Found");


                }


                cn.Close();

            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message + " Check your Login Credentials", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            groupBox2.Visible = false;
            //button5.Enabled = false;
            toolStripStatusLabel6.Text = "Done";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //this.Hide ();
            //Form fff = new FreeTool.Form1();
            //fff.Show();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            //listView6.Items.Clear();
            listView7.Items.Clear();
            // button5.Enabled = false;
            //button10.Enabled = false;
            // button12.Enabled = false;
        }


        private bool SyncFromAllServersCallbackDelegate1(
                                SyncFromAllServersEvent eventType,
                                string targetServer,
                                string sourceServer,
                                SyncFromAllServersOperationException e
                            )
        {

            toolStripStatusLabel1.Text = "Replicating 'Entire Forest' wait...";
            string a, b, c;

            a = eventType.ToString();

            listView1.Items.Add(a.ToString());

            if (sourceServer != null)
            {
                b = sourceServer.ToString();
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(b.ToString());

            }
            else
            {
                b = "-----";
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(b.ToString());
            }


            if (targetServer != null)
            {
                c = targetServer.ToString();
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(c.ToString());

            }
            else
            {
                c = "-----";
                listView1.Items[listView1.Items.Count - 1].SubItems.Add(c.ToString());
            }


            return true;

        }




        private void button9_Click(object sender, EventArgs e)
        {
            //groupBox3.Visible = false; 
        }

        private void button11_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = "Replicating wait...";


            this.listView7.Items.Clear();
            Application.DoEvents();


            try
            {

                string targetServer = comboBox3.Text;
                string sourceServer = comboBox2.Text;
                if (comboBox2.Text == comboBox3.Text)
                {
                    this.Cursor = Cursors.Default;
                    toolStripStatusLabel7.Text = "Done";
                    MessageBox.Show("Please Select Different Source & Destination Dc's", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                    if (comboBox2.Text == null || comboBox3.Text == null || comboBox2.Text.Length == 0 || comboBox3.Text.Length == 0)
                    {
                        this.Cursor = Cursors.Default;
                        toolStripStatusLabel7.Text = "Done";
                        MessageBox.Show("Please Select Dc's", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        try
                        {
                            this.Cursor = Cursors.WaitCursor;
                            DirectoryContext targetContext = new DirectoryContext(DirectoryContextType.DirectoryServer, targetServer);
                            DomainController targetDc = DomainController.GetDomainController(targetContext);
                            targetDc.CheckReplicationConsistency();
                            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain);
                            DomainController dc = DomainController.FindOne(context);
                            foreach (string partition in dc.Partitions)
                            {
                                targetDc.SyncReplicaFromServer(partition, sourceServer);
                                listView7.Items.Add(partition.ToString());
                                listView7.Items[listView7.Items.Count - 1].SubItems.Add(sourceServer.ToString());
                                listView7.Items[listView7.Items.Count - 1].SubItems.Add(targetServer.ToString());
                            }
                        }
                        catch (ActiveDirectoryOperationException ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show("The following error occurred during the attempt to synchronize naming context from domain controller '" + comboBox2.Text + "' to domain controller '" + comboBox3.Text + "':\n\nThe RPC Server is unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (ActiveDirectoryServerDownException ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show("The following error occurred during the attempt to contact the domain controller '" + ex.Name + "':\n\nThe RPC Server is unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show("You don't have permission for Replication", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (ArgumentException ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show("The following error occurred during the attempt to contact the domain controller '" + comboBox2.Text + "':\n\nThe RPC Server is unavailable", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (ActiveDirectoryObjectNotFoundException ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show("Domain controller '" + comboBox2.Text + " 'does not exist or cannot be contacted", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            this.Cursor = Cursors.Default;
                            toolStripStatusLabel7.Text = "Done";
                            MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
            }
            //MessageBox.Show("Replication Process Between Source Server & Destination Server Successfully Done", "Successful Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            if (listView7.Items.Count == 0)
            {
                // button12.Enabled = false;
            }
            else
            {
                button12.Enabled = true;
            }
            this.Cursor = Cursors.Default;
            toolStripStatusLabel7.Text = "Done";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (listView7.Items.Count == 0)
            {
                MessageBox.Show("Nothing to save", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.textBox7.Text = "";
            this.textBox8.Text = "";
            this.textBox9.Text = "";
            this.textBox10.Text = "";
            this.textBox15.Text = "";
            groupBox5.Visible = true;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = false;
        }

        private void button13_Click(object sender, EventArgs e)
        {

            string dt = DateTime.Now.ToString();
            if (textBox7.Text == null || textBox10.Text == null || textBox15.Text == null || textBox7.Text.Length == 0 || textBox10.Text.Length == 0 || textBox15.Text.Length == 0)
            {
                MessageBox.Show("Please Enter All The Fields", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            toolStripStatusLabel7.Text = "Saving the content to the database, please wait. .";
            SqlConnection cn = new SqlConnection("Data Source=\'" + (textBox7.Text + ("\';Initial Catalog=\'" + (textBox15.Text + ("\';User ID=\'" + (textBox8.Text + ("\';Password=\'" + (textBox9.Text + ("\';Networ" + "k Library=dbmssocn")))))))));
            try
            {
                SqlDataReader dr;
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM SYS.DATABASES where name = '" + textBox15.Text + "'", cn);
                dr = cmd.ExecuteReader();
                if (dr.Read() == true)
                {

                    //MessageBox.Show("Database Name = " + dr["name"]);
                    dr.Close();
                    SqlCommand cmd1 = new SqlCommand("select *from sys.tables where name = '" + textBox10.Text + "'", cn);
                    SqlDataReader dr1 = cmd1.ExecuteReader();
                    if (dr1.Read() == true)
                    {

                        //MessageBox.Show("Table Name = " + dr1["name"]);
                        dr1.Close();
                        ListViewItem lv;
                        for (int i = 0; i <= listView7.Items.Count - 1; i++)
                        {
                            try
                            {
                                cn.Open();
                                lv = listView7.Items[i];

                                SqlCommand cmd3 = new SqlCommand("insert into " + textBox10.Text + " values(\'" + lv.ToString() + ("\', \'" + (lv.SubItems[1].ToString() + ("\', \'" + (lv.SubItems[2].ToString() + ("\', \'" + (dt + ("\')"))))))), cn);
                                //SqlCommand cmd = new SqlCommand("insert into replicat_by_domain values(\'" + lv.ToString () + ("\', \'" + (lv.SubItems[1].ToString ()  + ("\', \'" + (lv.SubItems[2].ToString () + ("\')"))))), cn);
                                cmd3.ExecuteNonQuery();
                                cn.Close();
                            }
                            catch (ArgumentOutOfRangeException exx)
                            {
                                MessageBox.Show(exx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        MessageBox.Show("Record Saved in the table '" + textBox10.Text + "' Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        dr1.Close();
                        //MessageBox.Show("Table Not Found");
                        SqlCommand cmd4 = new SqlCommand("create table " + textBox10.Text + "(sno smallint IDENTITY(1,1)PRIMARY KEY CLUSTERED,event_type varchar(100),source_server varchar(500),destination_server varchar(500),replicate_date_time datetime)", cn);
                        cmd4.ExecuteNonQuery();
                        //MessageBox.Show("Table ' " + textBox10.Text + " ' Created Successfully");
                        ListViewItem lv;
                        for (int i = 0; i <= listView7.Items.Count - 1; i++)
                        {
                            try
                            {
                                lv = listView7.Items[i];

                                SqlCommand cmd5 = new SqlCommand("insert into " + textBox10.Text + " values(\'" + lv.ToString() + ("\', \'" + (lv.SubItems[1].ToString() + ("\', \'" + (lv.SubItems[2].ToString() + ("\', \'" + (dt + ("\')"))))))), cn);
                                cmd5.ExecuteNonQuery();
                            }
                            catch (ArgumentOutOfRangeException exx)
                            {
                                MessageBox.Show(exx.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        MessageBox.Show("Record Saved in the table '" + textBox10.Text + "' Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        groupBox2.Visible = true;
                    }
                }

                else
                {
                    dr.Close();
                    MessageBox.Show("Database Not Found");

                }
                cn.Close();

            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message + " Check your Login Credentials", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            groupBox5.Visible = false;
            //  button12.Enabled = false;
            toolStripStatusLabel7.Text = "Done";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            this.listView2.Items.Clear();
            this.listView3.Items.Clear();
            this.listView4.Items.Clear();
            this.listView5.Items.Clear();

            try
            {
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain);
                DomainController dc = DomainController.FindOne(context);
                foreach (string partition in dc.Partitions)
                {
                    listView2.Items.Add(partition);

                    foreach (ReplicationCursor cursor in dc.GetReplicationCursors(partition))
                    {

                        listView2.Items[listView2.Items.Count - 1].SubItems.Add(cursor.SourceServer);

                        listView2.Items[listView2.Items.Count - 1].SubItems.Add(cursor.UpToDatenessUsn.ToString());

                        listView2.Items[listView2.Items.Count - 1].SubItems.Add(cursor.SourceInvocationId.ToString());

                        listView2.Items[listView2.Items.Count - 1].SubItems.Add(cursor.LastSuccessfulSyncTime.ToString());


                    }
                }

                foreach (string partition in dc.Partitions)
                {
                    listView3.Items.Add(partition);
                    foreach (ReplicationNeighbor neighbor in dc.GetReplicationNeighbors(partition))
                    {
                        listView3.Items[listView3.Items.Count - 1].SubItems.Add(neighbor.SourceServer);
                        listView3.Items[listView3.Items.Count - 1].SubItems.Add(neighbor.UsnAttributeFilter.ToString());
                        listView3.Items[listView3.Items.Count - 1].SubItems.Add(neighbor.ReplicationNeighborOption.ToString());
                        listView3.Items[listView3.Items.Count - 1].SubItems.Add(neighbor.LastSuccessfulSync.ToString());
                    }
                }


                foreach (ReplicationConnection con in dc.InboundConnections)
                {
                    listView4.Items.Add(con.Name);
                    listView4.Items[listView4.Items.Count - 1].SubItems.Add(con.SourceServer);
                    listView4.Items[listView4.Items.Count - 1].SubItems.Add(con.DestinationServer);
                    listView4.Items[listView4.Items.Count - 1].SubItems.Add(con.TransportType.ToString());
                    listView4.Items[listView4.Items.Count - 1].SubItems.Add(con.ReplicationScheduleOwnedByUser.ToString());
                }


                foreach (ReplicationConnection con1 in dc.OutboundConnections)
                {
                    listView5.Items.Add(con1.Name);
                    listView5.Items[listView5.Items.Count - 1].SubItems.Add(con1.SourceServer);
                    listView5.Items[listView5.Items.Count - 1].SubItems.Add(con1.DestinationServer);
                    listView5.Items[listView5.Items.Count - 1].SubItems.Add(con1.TransportType.ToString());
                    listView5.Items[listView5.Items.Count - 1].SubItems.Add(con1.ReplicationScheduleOwnedByUser.ToString());
                }
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(20);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            this.Cursor = Cursors.Default;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            //listView1.Items.Clear();
            //listView2.Items.Clear();
            //listView3.Items.Clear();
            //listView4.Items.Clear();
            //listView5.Items.Clear();
            ////listView6.Items.Clear();
            //listView7.Items.Clear();
            //button5.Enabled = false;
            ////button10.Enabled = false;
            //button12.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
            listView4.Items.Clear();
            listView5.Items.Clear();
            //listView6.Items.Clear();
            listView7.Items.Clear();
            //button5.Enabled = false;
            //button10.Enabled = false;
            // button12.Enabled = false;
            //toolStripStatusLabel6.Text = "Ready...";
            //toolStripStatusLabel7.Text = "Ready..."; 
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void tabPage11_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_2(object sender, EventArgs e)
        {
            Form1 rm = new Form1();
            rm.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void listView7_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView7_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView7_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void listView2_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void listView2_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView3_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void listView3_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView3_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView5_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }

        }

        private void listView5_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView5_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView4_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void listView4_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView4_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void statusStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }



    }
}