using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management.Automation;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace ActiveDirectoryReplicationManager
{
    public partial class Form1 : Form
    {
        public Form1()
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

                br = new SolidBrush(Color.LightYellow);

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

                br = new SolidBrush(Color.LemonChiffon);

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

                br1 = new SolidBrush(Color.LightYellow);

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

                br1 = new SolidBrush(Color.LemonChiffon);

                // Change this to your preference

                g1.FillRectangle(br1, e.Bounds);

                br1 = new SolidBrush(Color.Black);

                g1.DrawString(strTitle, tabControl2.Font, br1, r1, sf1);

            }

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("form-load");
            comboBox1.Enabled = false;
            radioButton1.Checked = true;
           // button5.Enabled = false;
            groupBox2.Visible = false;
            //groupBox3.Visible = false;
            button12.Enabled = false;
            groupBox5.Visible = false;
            //button10.Enabled = false;




            Domain domain;

            try
            {
                domain = Domain.GetCurrentDomain();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                Domain parentDomain = domain.Parent;
                if (parentDomain != null)
                {
                    comboBox1.Items.Add(domain.Parent);
                }

                foreach (Domain childDomain in domain.Children)
                {
                    comboBox1.Items.Add(childDomain.Name);

                }

                foreach (DomainController dc in domain.DomainControllers)
                {
                    comboBox2.Items.Add(dc.Name);
                    comboBox3.Items.Add(dc.Name);
                }
                foreach (DomainController dc2 in parentDomain.DomainControllers)
                {
                    comboBox2.Items.Add(dc2.Name);
                    comboBox3.Items.Add(dc2.Name);
                }
            }
            catch (Exception)
            {

            }
            ListDomains();

        }

        public void ListDomains()
        {
            MessageBox.Show("ListDomains funs called");
            try
            {
                DirectoryEntry en = new DirectoryEntry("LDAP://");
                DirectorySearcher srch = new DirectorySearcher("objectCategory=Domain");
                SearchResultCollection coll = srch.FindAll();
                String domainPath = null, domainPath1 = null;
                int c = 0;
                foreach (SearchResult rs in coll)
                {
                    ResultPropertyCollection rpc = rs.Properties;
                    c++;
                    foreach (object domainName in rpc["distinguishedName"])
                    {
                        String strDomain = domainName.ToString();
                        String[] dom = strDomain.Split("DC=".ToCharArray());
                        for (int j = 0; j < dom.Length; j += 3)
                        {
                            if (j == (dom.Length - 1))
                                domainPath = domainPath + dom[j];
                            else
                                domainPath = domainPath + dom[j];
                        }
                        domainPath1 = domainPath.Replace(',', '.');
                        this.comboBox1.Items.Add(domainPath1);
                    }
                }

            }
            catch (System.Runtime.InteropServices.COMException coe)
            {

                MessageBox.Show("" + coe.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("button - clicked");
/*
            if (radioButton1.Checked)
                MessageBox.Show("radio1");
            if (radioButton2.Checked)
                MessageBox.Show("radio2");


            this.listView1.Items.Clear();

            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (radioButton2.Checked == true)
                {
                    MessageBox.Show("1");
                    if (this.comboBox1.Text.Equals(""))
                    {
                        MessageBox.Show("Please Select Domain", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }
                    toolStripStatusLabel6.Text = "Replicating '" + comboBox1.Text + "' wait...";
                    MessageBox.Show("2");
                    String domaintext = comboBox1.Text;
                    Application.DoEvents();
                    DirectoryContextType contexttype = new DirectoryContextType();
                    DirectoryContext context = new DirectoryContext(contexttype, domaintext);

                    Domain domain = Domain.GetDomain(context);
                    MessageBox.Show("3");
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
                                dc.SyncReplicaFromAllServers(partitionName.ToString(),
                                                    SyncFromAllServersOptions.AbortIfServerUnavailable
                                                    | SyncFromAllServersOptions.CrossSite
                                                    );
                            }
                        }
                        catch (SyncFromAllServersOperationException em)
                        {
                            MessageBox.Show("1\n"+em.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("2\n" + ex);
                        }
                    } // foreach loop
                    MessageBox.Show("4");
                    toolStripStatusLabel6.Text = "Done";
                    MessageBox.Show("Ok");
                } // if 
                /*
                if (radioButton1.Checked == true)
                {
                    toolStripStatusLabel6.Text = "Replicating 'Entire Forest' wait...";

                    this.Cursor = Cursors.WaitCursor;

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
                            MessageBox.Show("" + ex);
                            return;
                        }

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
                                    MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("" + ex);
                                }

                                Domain parentDomain = domain1.Parent;

                                foreach (DomainController dc2 in parentDomain.DomainControllers)
                                {
                                    toolStripStatusLabel1.Text = "Replicating 'Entire Forest' wait...";
                                    dc2.CheckReplicationConsistency();
                                    dc2.SyncFromAllServersCallback = SyncFromAllServersCallbackDelegate1;
                                    try
                                    {
                                        foreach (string partitionName in dc2.Partitions)
                                        {
                                            dc2.SyncReplicaFromAllServers(partitionName.ToString(),
                                                                SyncFromAllServersOptions.AbortIfServerUnavailable
                                                                | SyncFromAllServersOptions.CrossSite
                                                                );
                                        }
                                    }
                                    catch (SyncFromAllServersOperationException eexe)
                                    {
                                        MessageBox.Show(eexe.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("" + ex);
                                    }
                                } // foreach
                            } // foereach
                        }
                        catch (UnauthorizedAccessException)
                        {
                            MessageBox.Show("You Have No Permission To Replicate", "Replication Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("" + ex);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    this.Cursor = Cursors.Default;
                    toolStripStatusLabel6.Text = "Done";
                    //button10.Enabled = true;

                } // if
                */
/*
            }
            catch (Exception ex)
            {
                MessageBox.Show("3\n"+ex.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
            if (listView1.Items.Count == 0)
            {
                button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
            }
*/
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

        private void button3_Click(object sender, EventArgs e)
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
            button12.Enabled = false;
            toolStripStatusLabel6.Text = "Ready...";
            toolStripStatusLabel7.Text = "Ready..."; 
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

        private void button5_Click_1(object sender, EventArgs e)
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
                                MessageBox.Show(exx.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                MessageBox.Show(exx.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        MessageBox.Show("Record Saved in the table '" + textBox6.Text + "' Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //MessageBox.Show("Table ' " + textBox6.Text.ToString () + " ' was created Successfully and Records are Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    }
                    ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    cl.executeDll(18);

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
                MessageBox.Show(eee.Message.ToString() + " Check your Login Credentials");
                return;
            }
            groupBox2.Visible = false;
          //  button5.Enabled = false;
            toolStripStatusLabel6.Text = "Done";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel7.Text = "Replicating wait...";
            this.Cursor = Cursors.WaitCursor;

            this.listView7.Items.Clear();

            try
            {

                string targetServer = comboBox3.Text;
                string sourceServer = comboBox2.Text;
                if (comboBox2.Text == comboBox3.Text)
                {
                    MessageBox.Show("Please Select Different Source & Destination Dc's", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                    if (comboBox2.Text == null || comboBox3.Text == null || comboBox2.Text.Length == 0 || comboBox3.Text.Length == 0)
                    {
                        MessageBox.Show("Please Select Dc's", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        try
                        {
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
                        catch (Exception eexxx)
                        {
                            MessageBox.Show(eexxx.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //MessageBox.Show("Replication Process Between Source Server & Destination Server Successfully Done", "Successful Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            if (listView7.Items.Count == 0)
            {
                button12.Enabled = false;
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
            this.textBox7.Text = "";
            this.textBox8.Text = "";
            this.textBox9.Text = "";
            this.textBox10.Text = "";
            this.textBox15.Text = "";
            groupBox5.Visible = true; 
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
                                MessageBox.Show(exx.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                MessageBox.Show(exx.Message.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        MessageBox.Show("Record Saved in the table '" + textBox10.Text + "' Successfully", "Success Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        groupBox2.Visible = true;
                    }
                    ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    cl.executeDll(19);
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
                MessageBox.Show(eee.Message.ToString() + " Check your Login Credentials");
                return;
            }
            groupBox5.Visible = false;
            button12.Enabled = false;
            toolStripStatusLabel7.Text = "Done";

        }

        private void button14_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = false;
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
                MessageBox.Show(ex.ToString(), "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            this.Cursor = Cursors.Default;
        }
    }
}