using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.ServiceProcess;
using System.Management;
using System.Text.RegularExpressions;
using System.DirectoryServices.ActiveDirectory;
using System.Net.NetworkInformation;

namespace ServiceAccountManagement
{

    public partial class Addcomputers : Form
    {
       
        System.Collections.Generic.List<String> AllComputersList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> AllComputersLocationList = new System.Collections.Generic.List<String>();
        public static string DomainName;
        public bool is_load = false;
        public bool fav_set = false;
        string path = null, uripath = null;
        public Addcomputers()
        {
            InitializeComponent();
        }
        public void recolour1()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243,243,243) : Color.White;
            }
        }
        private void LocalServiceAccounts_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            is_load = true;
            listView1.Items.Clear();
            comboBox1.Items.Clear();
            Constants.checkedComputerList.Clear();
            try
            {
                System.DirectoryServices.ActiveDirectory.Forest currentForest = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();
                System.DirectoryServices.ActiveDirectory.DomainCollection dc = currentForest.Domains;
                foreach (System.DirectoryServices.ActiveDirectory.Domain d in dc)
                {
                    comboBox1.Items.Add(d.Name);
                }
                comboBox1.SelectedItem = comboBox1.Items[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while getting Domain details", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DomainName = comboBox1.Text;
            button5.Visible = true;
            button4.Visible = false;
            button1.Visible = false;
            comboBox1.Enabled = false;
            button3_Click(this, e);
            label2.Text = "";
            recolour1();
        }
        private void SelectAllSA(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllSA"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;

        }

        

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            

            listView1.Items.Clear();
            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";
            listView1.Refresh();
            
            if (AllComputersList.Count == 0)
            {
                label1.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label1.Refresh();
                MessageBox.Show("Please Enter Computer Details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
            if (textBox1.Text.Equals("") || textBox1.Text.Equals("Computer Name"))
            {

                for (int i = 0; i < AllComputersList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(AllComputersList[i]);
                    item.SubItems.Add(AllComputersLocationList[i]);
                    listView1.Items.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < AllComputersList.Count; i++)
                {
                    if (AllComputersList[i].Contains(textBox1.Text) || AllComputersList[i].ToUpper().Contains(textBox1.Text.ToUpper()))
                    {
                        ListViewItem item = new ListViewItem(AllComputersList[i]);
                        item.SubItems.Add(AllComputersLocationList[i]);
                        listView1.Items.Add(item);
                    }
                }
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
            }
            label1.Text = "Total Number of Computers  : " + listView1.Items.Count;
       

            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllSA"];
            cb.Checked = true;
            cb.Checked = false;
            
            recolour1();
            listView1.Refresh();
            
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.ForeColor == Color.Gray)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // pictureBox1.Focus();
                pictureBox1_Click(sender, e);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                textBox1.ForeColor = Color.Gray;
                textBox1.Text = "Search";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            pictureBox1_Click(sender, e);
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Error Occured computers : \n" + Form1.errorcomputers, "Error Computers List");
        }
      

        private void button2_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(-116, btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            contextMenuStrip1.Show(ptLowerLeft);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           // bool itemflag = true;
          /*  ServiceList.Clear();
            ServiceAccountList.Clear();
            ComputerList.Clear();
            
            button4.Visible = true;

            if (listView1.CheckedItems.Count == 0)
                MessageBox.Show("Please select atleast one Service Account", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                backgroundWorker1.RunWorkerAsync();
           */
            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";
            
            AllComputersList.Clear();
            AllComputersLocationList.Clear();
            listView1.Items.Clear();
            button3.Enabled = false;
            // DomainName = comboBox2.Text;
           // label4.Text = "Loading Computers...";
            button5.Visible = false;
            pictureBox1.Enabled = false;
            button4.Visible = true;
            backgroundWorker1.RunWorkerAsync();
            recolour1();

        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
          

           try
            {
                // Getting DC list
                System.Collections.Generic.List<String> dcList = new System.Collections.Generic.List<string>();
                Domain domain;
                if (!Form1.defaultUser)
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, comboBox1.Text, Form1.UserName, Form1.Password));
                else
                {
                   //DomainName = comboBox1.Text;
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, comboBox1.Text));
                }
                foreach (System.DirectoryServices.ActiveDirectory.DomainController dc in domain.DomainControllers)
                {
                    if (backgroundWorker1.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    int index = dc.Name.IndexOf('.');
                    String dcName = dc.Name.Substring(0, index);
                    dcList.Add(dcName);
                }

                String adsPath = "LDAP://" + comboBox1.Text;
                DirectoryEntry domainEntry;
                backgroundWorker1.ReportProgress(3);
                if (!Form1.defaultUser)
                {
                    domainEntry = new DirectoryEntry(adsPath, Form1.UserName, Form1.Password);
                }
                else
                {
                    domainEntry = new DirectoryEntry(adsPath);
                }

                backgroundWorker1.ReportProgress(3);


                DirectorySearcher mySearcher = new DirectorySearcher(domainEntry);
                mySearcher.Filter = "(ObjectCategory=computer)";
                mySearcher.PageSize = 1000;
                domainEntry.Children.SchemaFilter.Add("computer");
                foreach (SearchResult result in mySearcher.FindAll())
                {
                    if (backgroundWorker1.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    backgroundWorker1.ReportProgress(3);
                    String name = result.GetDirectoryEntry().Name;
                    String addName = name.Substring(name.IndexOf('=') + 1);
                    ListViewItem item = new ListViewItem(addName);
                    /*
                    bool canDisplay = false;
                    int i;
                    for (i = 0; i < dcList.Count; i++)
                    {
                        if (addName.ToLower().Equals(dcList[i].ToLower()))
                            break;
                    }
                    if (i == dcList.Count)
                        canDisplay = true;

                    if (canDisplay == true)
                    { */
                    String path = result.GetDirectoryEntry().Path;
                    String addPath = path.Substring(path.IndexOf(',') + 1);
                    item.SubItems.Add(addPath);
                    AllComputersList.Add(addName);
                    AllComputersLocationList.Add(addPath);
                    backgroundWorker1.ReportProgress(1, item);
                    System.Threading.Thread.Sleep(1);
                    // }

                }

                domainEntry.Dispose();
            }   // try
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                backgroundWorker1.ReportProgress(2, "  The network path was not found ");
                button3.Enabled = true;
                comboBox1.Enabled = true;
                pictureBox1.Enabled = true;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                backgroundWorker1.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
                button3.Enabled = true;
                comboBox1.Enabled = true;
                pictureBox1.Enabled = true;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                backgroundWorker1.ReportProgress(2, "Unspecified Error");
                button3.Enabled = true;
                comboBox1.Enabled = true;
                pictureBox1.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    backgroundWorker1.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
                }
                else
                {
                    backgroundWorker1.ReportProgress(2, "Unspecified Error");
                }
                button3.Enabled = true;
                comboBox1.Enabled = true;
                pictureBox1.Enabled = true;
            }
        }
        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
          
            if (e.ProgressPercentage == 1)
            {
                ListViewItem item = (ListViewItem)e.UserState;
                listView1.Items.Add(item);
                label1.Text = "Total Number of Computers  : " + listView1.Items.Count;
                if (is_load && listView1.Items.Count > 99)
                {
                    is_load = false;
                    backgroundWorker1.CancelAsync();
                }
                label1.Refresh();
            }
            else if (e.ProgressPercentage == 2)
            {
                MessageBox.Show((String)e.UserState, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
          
            if (listView1.Items.Count > 0)
            {


                CheckBox chkBox = new CheckBox();
                chkBox.Checked = false;
                chkBox.Name = "ToSelectAllSA";
                chkBox.Size = new System.Drawing.Size(13, 13);
                chkBox.Location = new System.Drawing.Point(5, 2);
                chkBox.CheckedChanged += SelectAllSA;
                listView1.Controls.Add(chkBox);

                CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllSA"];
                cb.Checked = true;
                cb.Checked = false;

                listView1.Sorting = SortOrder.Ascending;
                listView1.Refresh();

                label1.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label1.Refresh();
            }
            button3.Enabled = true;
            button4.Visible = false;
            button5.Visible = true;
            pictureBox1.Enabled = true;
            comboBox1.Enabled = true;
            recolour1();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            backgroundWorker1.CancelAsync();          
        }
     

        private void button6_Click(object sender, EventArgs e)
        {
            uripath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = new Uri(uripath).LocalPath;
            System.IO.StreamWriter file = new System.IO.StreamWriter(path + "/conf/service.txt");
            System.Collections.Generic.List<String> tempList = new System.Collections.Generic.List<String>();

            if (listView1.CheckedItems.Count > 0)
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    tempList.Add((listView1.CheckedItems[i].Text));
                string s = string.Join(",", tempList.ToArray());
                s = s + ',';
                file.WriteLine(s);
                file.Close();
                MessageBox.Show("Your selection has been saved. Selected computers appears in your favourite list", "Success");
                fav_set = true;
                this.Close();
            }
            else
                MessageBox.Show("Please select atleast one computer", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count < 1)
            {
                MessageBox.Show("Please select atleast one computer", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //button1.Enabled = true;
                return;
            }
            recolour1();
            ColumnHeader header = listView1.Columns[1];
            label2.Text = " Checking Connectivity  Please Wait ... ";
            header.Text = "    Connectivity ";
            button1.Visible = true;
            button5.Visible = false;
            button6.Visible = false;
            for (int i = 0; i < listView1.Items.Count; i++)
                listView1.Items[i].SubItems[1].Text = "-----";

            for (int i = 0; i < listView1.CheckedItems.Count; i++)
                Constants.checkedComputerList.Add(listView1.CheckedItems[i].Text);

            listView1.Refresh();
            // MessageBox.Show(checkedComputerList.Count.ToString());
            Form1.S_SelectedDomain = comboBox1.Text;
            backgroundWorker2.RunWorkerAsync(Constants.checkedComputerList);

        }
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            /*
            try
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    checkedComputerList.Add(listView1.CheckedItems[i].Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
             */

            //System.Collections.Generic.List<String> coll =new System.Collections.Generic.List<String>();
            //for (int i = 0; i < checkedComputerList.Count; i++)
            //    coll.Add(checkedComputerList[i]);

            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;

            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;



            List<String> ComboList = new List<String>();

            Constants.computerList.Clear();

            for (int ind = 0; ind < coll.Count; ind++)
            {


                tempBGWorker.ReportProgress(ind, "Checking");

                if (backgroundWorker2.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                /////////////////////////////////////////// PingChecking Computers
                try
                {

                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();
                    options.DontFragment = true;
                    String data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
                    int timeout = 60;

                    PingReply reply = pingSender.Send(coll[ind], timeout, buffer, options);

                    if (reply.Status != IPStatus.Success)
                    {
                        tempBGWorker.ReportProgress(ind, "Not Accessible");
                        // MessageBox.Show(coll[ind] + " is Not Accessible", "Warning");
                        //System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        try
                        {
                            String ads = "WinNT://" + coll[ind] + ",computer";
                            DirectoryEntry computerEntries;
                            if (!Form1.defaultUser)
                                computerEntries = new DirectoryEntry(ads, Form1.UserName, Form1.Password);
                            else
                                computerEntries = new DirectoryEntry(ads);

                            DirectoryEntries userEntries = computerEntries.Children;

                            foreach (DirectoryEntry user in userEntries)
                            {
                            }

                            tempBGWorker.ReportProgress(ind, "Accessible");
                            
                        }
                        catch (Exception ex)
                        {
                            tempBGWorker.ReportProgress(ind, "Not Accessible");
                           
                        } // catch
                                  
                   } // else
                } //try
                catch (Exception ex)
                {
                    tempBGWorker.ReportProgress(ind, "Not Accessible");
                    // MessageBox.Show(coll[ind] + " is Not Accessible", "Warning");
                    //System.Threading.Thread.Sleep(100);
                }
                /////////////////////////////////////////// PingChecking COmputers
            } // for loop
        }

        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {




            //     if (progressBar1.Value > 150)
            //           progressBar1.Value = 5;
            if(e.ProgressPercentage<Constants.checkedComputerList.Count)
            {


            if (e.UserState.ToString().Equals("Checking"))
                label2.Text = "Checking accessibility of \"" + Constants.checkedComputerList[e.ProgressPercentage] + "\" ";
            else
            {
                listView1.CheckedItems[e.ProgressPercentage].SubItems.RemoveAt(1);
                listView1.CheckedItems[e.ProgressPercentage].UseItemStyleForSubItems = false;
                listView1.CheckedItems[e.ProgressPercentage].SubItems.Add("" + e.UserState);
                /* if(e.ProgressPercentage % 2 == 0)
                 listView1.CheckedItems[e.ProgressPercentage].SubItems[1].BackColor = Color.LightGray;
                 else
                 listView1.CheckedItems[e.ProgressPercentage].SubItems[1].BackColor = Color.White;*/
                if (e.UserState.Equals("Accessible"))
                    listView1.CheckedItems[e.ProgressPercentage].SubItems[1].ForeColor = Color.Green;
                else
                    listView1.CheckedItems[e.ProgressPercentage].SubItems[1].ForeColor = Color.Red;
                recolour1();

            }
                listView1.Refresh();
            }


            if (e.UserState.ToString().Equals("Accessible"))
            {
                //  MessageBox.Show("i : " + e.ProgressPercentage + "state : " + e.UserState + "listView1.CheckedItems[e.ProgressPercentage].Text : " + listView1.CheckedItems[e.ProgressPercentage].Text);
                
                
                Constants.computerList.Add(listView1.CheckedItems[e.ProgressPercentage].Text);
                //  MessageBox.Show("computerList.Count : " + computerList.Count);
            }

        }



        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //  MessageBox.Show("Completed..... computerlist.count : " + computerList.Count);
            if (e.Error != null)
            {
                MessageBox.Show("Error occurred while stopping the process\n" + e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (Constants.computerList.Count < 1)
            {
                MessageBox.Show("Selected Computers are not accessible now", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label2.Text = "";
            }
            button5.Visible = true;
            button6.Visible = true;
            button1.Visible = false;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label2.Text = "Process about to stop ..";
            button1.Visible = false;
            backgroundWorker2.CancelAsync(); 
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

       

       

    }
}

