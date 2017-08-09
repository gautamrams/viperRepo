using System;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Collections.Generic; // for 'List'
using System.Collections; // for 'ArrayList'
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.IO;
using System.Text;
//using CheckComboBoxTest;


namespace ADLDSManagement
{
    public partial class Form1 : Form
    {
        //int ind = 0;
        //AddMultipleComputers addMultipleComputers = new AddMultipleComputers();
        //ResetPasswordForm RPForm = new ResetPasswordForm();
       // SaveForm sv = new SaveForm();
        
       // String DomainName;
        String UserName;
        String Password;
        String path1;
        String partition;
        String port1;
        String dc1;
        
        bool defaultUser = true;
        SearchResultCollection results;
        SearchResultCollection results1;
        int startindex;
        int endindex;
        int startindexg;
        int endindexg;

        int issearchset;
        int issearchsetg;
        String filtertext;
        int quicksearchflagu = 0;
        int quicksearchflagg = 0;
        int searchflagu = 0;
        int searchflagg = 0;
        int fromgo = 0;
        int fromgrpgo = 0;
        //        System.Collections.ArrayList GroupArrayList = new System.Collections.ArrayList();
        
        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
        }
        public static Form1 _Form1;
      
        
        public void recolour()
        {
            for (int item = 0; item < listView2.Items.Count; ++item)
            {
                var items = listView2.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243,243,243) : Color.White;
            }
          
            
        }
        public void recolour1()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }


        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.Left = 150;
            this.Top = 40;
            tabPage1.Enabled = false;
            tabPage2.Enabled = false;

            //this.listView3.ColumnWidthChanged += new ColumnWidthChangedEventHandler(listview3_columnwidthchanged);
            
            listView2.DrawSubItem += new DrawListViewSubItemEventHandler(listview2_DrawSubItem);
            listView1.DrawSubItem += new DrawListViewSubItemEventHandler(listview1_DrawSubItem);
            listView3.DrawSubItem += new DrawListViewSubItemEventHandler(listView3_DrawSubItem);
            listView4.DrawSubItem += new DrawListViewSubItemEventHandler(listView4_DrawSubItem);
            listView4.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listView4_ColumnWidthChanging);
            listView3.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listView3_ColumnWidthChanging);
            listView1.MouseMove +=new MouseEventHandler(listView1_MouseMove);
            listView1.MouseLeave +=new EventHandler(listView1_MouseLeave);
            UserName = "";
            Password = "";
            button3.Enabled = true;
            button14.Enabled = false;
            button21.Enabled = false;
            label22.Visible = true;
            
            label22.Text = "";


            
                

                listView3.Items.Clear();
                CheckBox chkBox0 = new CheckBox();
                chkBox0.Checked = false;
                chkBox0.Name = "ToSelectAll";
                chkBox0.Size = new Size(13, 13);
                chkBox0.Location = new Point(5, 2);
                chkBox0.CheckedChanged += SelectAllUsers;
                listView3.Controls.Add(chkBox0);

                listView4.Items.Clear();
                CheckBox chkBox2 = new CheckBox();
                chkBox2.Checked = false;
                chkBox2.Name = "ToSelectAll";
                chkBox2.Size = new Size(13, 13);
                chkBox2.Location = new Point(5, 2);
                chkBox2.CheckedChanged += SelectAllGroups;
                listView4.Controls.Add(chkBox2);

                

                
          
        }

        // Get-Users Button
        private void button3_Click(object sender, EventArgs e)
        {
            
            listView2.Items.Clear();
            listView2.SmallImageList = this.imageList1;
            button3.Enabled = true;
            //button21.Enabled = true;
            label22.Text = "Loading User Details .. Please Wait ..";

            
            
            label22.Refresh();
            
            if ((UserName.Equals("") && !Password.Equals("")) || (!UserName.Equals("") && Password.Equals("")))
            {
                MessageBox.Show("Please enter the username & password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button3.Enabled = true;
                return;
            }
            
            try
            {
                string check = "LDAP://" + dc1 + ":" + port1 + "/RootDSE";
                DirectoryEntry de;
                if (!defaultUser)
                {
                    
                    de = new DirectoryEntry(path1, UserName, Password, AuthenticationTypes.Secure);
                    if (de != null)
                    {
                       // MessageBox.Show("CONNECTED");

                    }
                }
                else
                {
                    de = new DirectoryEntry(path1);

                }             
       //------------------------------------------------------------------------------------------------
                
                de.RefreshCache(new string[] { "canonicalName" });
                
                DirectorySearcher ds = new DirectorySearcher(de);
                
                //String time = DateTime.Now.ToString();
                //System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\test.txt", true);
                //file.WriteLine(time);

                //file.Close();
                
                ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                
                
                if (issearchsetg != 1)
                {
                    ds.Filter = "(objectClass=user)";
                }
                else
                {
                    //MessageBox.Show(filtertext);
                    if (filtertext != "")
                    {
                        ds.Filter = filtertext;
                        filtertext = "";
                        
                    }
                    else
                    {
                        filtertext = "";
                        ds.Filter = "(objectClass=user)";
                    }
                    quicksearchflagu = 1;
                }             
                ds.PageSize = 100;
                ds.PropertiesToLoad.Add("canonicalName");
                ds.PropertiesToLoad.Add("cn");
                ds.PropertiesToLoad.Add("msDS-UserAccountDisabled");
                ds.PropertiesToLoad.Add("userPrincipalName");
                ds.PropertiesToLoad.Add("distinguishedName");
                //ds.PropertyNamesOnly = true;
                ds.Sort.Direction = SortDirection.Ascending;
                ds.Sort.PropertyName = "cn";
                
                results = ds.FindAll();
                if (results.Count == 0)
                {
                    if (fromgo != 2)
                    {
                        MessageBox.Show("Cannot find the objects");
                        label12.Text = "0 - 0 of 0";
                        label22.Text = "";
                        startindex = 0;
                        endindex = 0;
                    }
                    issearchsetg = 0;
                    textBox1.Text = "";
                    textBox2.Text = "";
                    if (fromgo == 1)
                    {
                        fromgo = 2;
                        button3_Click(sender, e);
                    }
                }
                else
                {

                    de.Dispose();

                    int go = 100;
                    if (results.Count < 100)
                    {
                        go = results.Count;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        button14.Enabled = false;
                        button21.Enabled = false;
                    }
                    if (results.Count > 100)
                    {
                        button1.Enabled = true;
                        button21.Enabled = true;
                    }


                    listView2.BeginUpdate();

                    for (int i = 0; i < go; i++)
                    {
                        ListViewItem item = new ListViewItem();

                        SearchResult sr = results[i];
                        String[] data = { "-", "-", "-" };

                        if (sr != null)
                        {

                            ResultPropertyCollection myResultPropColl;
                            myResultPropColl = sr.Properties;


                            foreach (string myKey in myResultPropColl.PropertyNames)
                            {

                                if (myKey.Trim().Equals("userprincipalname"))
                                {
                                    foreach (Object myCollection in myResultPropColl[myKey])
                                    {
                                        data[0] = myCollection.ToString();


                                    }
                                }
                                if (myKey.Trim().Equals("canonicalname"))
                                {
                                    foreach (Object myCollection in myResultPropColl[myKey])
                                    {
                                        data[1] = myCollection.ToString();

                                    }
                                }
                                if (myKey.Trim().Equals("distinguishedname"))
                                {
                                    foreach (Object myCollection in myResultPropColl[myKey])
                                    {
                                        data[2] = myCollection.ToString();

                                    }
                                }
                                if (myKey.Trim().Equals("msds-useraccountdisabled"))
                                {
                                    foreach (Object myCollection in myResultPropColl[myKey])
                                    {
                                        if (myCollection.ToString().ToLower().Equals("true"))
                                        {
                                            item.ImageIndex = 1;

                                        }
                                        if (myCollection.ToString().ToLower().Equals("false"))
                                        {

                                            item.ImageIndex = 0;
                                        }

                                    }
                                }
                                if (myKey.Trim().Equals("cn"))
                                {
                                    foreach (Object myCollection in myResultPropColl[myKey])
                                    {
                                        item.Text = myCollection.ToString();

                                    }
                                }
                                if (myKey.Trim().Equals("adspath"))
                                {
                                    foreach (Object myCollection in myResultPropColl[myKey])
                                    {
                                        //do nothing

                                    }
                                }
                            }

                            for (int j = 0; j < 3; j++)
                            {

                                item.SubItems.Add(data[j]);
                            }
                            listView2.Items.Add(item);

                        }
                    }
                    listView2.EndUpdate();

                    //time = DateTime.Now.ToString();
                    ////file = new System.IO.StreamWriter("d:\\test.txt", true);
                    //file.WriteLine(time);
                    //file.Close();

                    startindex = 0;
                    endindex = 100;
                    //button1.Enabled = true;
                    if (results.Count < 100)
                    {
                        button1.Enabled = false;
                        endindex = results.Count;
                    }
                    if (startindex == 0)
                    {
                        button2.Enabled = false;
                        button14.Enabled = false;
                    }
                    button2.Enabled = false;

                    label12.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();

                    label22.Text = "";
                    label22.Refresh();
                    //label18.Visible = false;
                    recolour();
                    //MessageBox.Show("test3");
                }
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show(" The network path was not found ");
                button3.Enabled = true;
                return;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button3.Enabled = true;
                return;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Error");
                button3.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button3.Enabled = true;
                    return;
                }
                else
                {
                    //MessageBox.Show("test UE");
                    MessageBox.Show("Error - " +ex.Message);
                    button3.Enabled = true;
                    return;
                }
            }
        
               
        } // Get-Users Button

        

        // To Select All Users 
        private void SelectAllUsers(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView3.Controls["ToSelectAll"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = false;

        }
        private void SelectAllGroups(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView4.Controls["ToSelectAll"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;

        }
        void listView4_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            //Console.Write("Column Resizing");
            e.NewWidth = this.listView4.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }
        void listView3_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            //Console.Write("Column Resizing");
            e.NewWidth = this.listView3.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }


        
        private void button4_Click(object sender, EventArgs e)
        {
            
        }// Delete Button.    

        
        private void button8_Click(object sender, EventArgs e)
        {
            
            try
            {
                CreateUsers addForm = new CreateUsers();
                addForm.userName = UserName;
                addForm.password = Password;
                addForm.path = path1;
                addForm.port = port1;
                addForm.partition = partition;
                addForm.dc = dc1;
                addForm.defaultUser = defaultUser;

                
                addForm.ShowDialog();
                if (addForm.isAdded == 1 || addForm.isAdded == 2)
                    //refreshAll();
                if(addForm.is_exception==true)
                {
                    
                    //label21.Visible = true;
                    
                }
                addForm.isAdded = 0;
                //refreshListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // Add Button

        

        private void textBox3_Click(object sender, EventArgs e)
        {/*
            textBox3.Text = "";
            textBox3.ForeColor = Color.Black;
          */
        }

        
        private void button14_Click(object sender, EventArgs e)
        {
        Button btnSender = (Button)sender;
        Point ptLowerLeft = new Point(0, btnSender.Height);
        ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
       
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
           
        }

        
   

        void listview2_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }
        void listView3_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }
        void listView4_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }
        void listview1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }
        void listview2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
           
                e.DrawBackground();
                e.DrawText();
          
        }
        void listView3_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            e.DrawBackground();
            e.DrawText();

        }
        void listView4_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            e.DrawBackground();
            e.DrawText();

        }
        void listview1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            e.DrawBackground();
            e.DrawText();

        }
        

        private void listView2_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ListViewItem item = listView.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    item.Selected = true;
                    item.Checked = true;
                    
                }

            }
        }
        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            ListView listView = sender as ListView;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ListViewItem item = listView.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    item.Selected = true;
                    item.Checked = true;

                }

            }
        }

      


        ToolTip mTooltip = new ToolTip();
        Point mLastPos = new Point(-1, -1);
       
        private void listView2_MouseMove(object sender, MouseEventArgs e)
        {
            
            try
            {
                ListViewItem item = listView2.GetItemAt(e.X, e.Y);
                ListViewHitTestInfo info = listView2.HitTest(e.X, e.Y);
                
                if (mLastPos != e.Location)
                {
                    if (info.Item != null && info.SubItem != null)
                    {
                        
                        mTooltip.Show(info.Item.Tag.ToString(), info.Item.ListView, e.X + 15, e.Y + 15, 20000);
                        
                    }
                    else
                    {
                        mTooltip.SetToolTip(listView2, string.Empty);
                    }
                }

                mLastPos = e.Location;
            }
            catch(Exception eaa){
            mTooltip.SetToolTip(listView2, string.Empty);
            }
        }

        private void listView2_MouseLeave(object sender, EventArgs e)
        {
            
            mTooltip.Hide(listView2);
        }


        ToolTip mTooltip1 = new ToolTip();
        Point mLastPos1 = new Point(-1, -1);

        private void listView1_MouseMove(object sender, MouseEventArgs e)
        {
            
            try
            {
                ListViewItem item = listView1.GetItemAt(e.X, e.Y);
                
                ListViewHitTestInfo info1 = listView1.HitTest(e.X, e.Y);
                
                
                if (mLastPos1 != e.Location)
                {
                    if (info1.Item != null && info1.SubItem != null)
                    {
                        //MessageBox.Show("hi");
                        mTooltip1.Show(info1.Item.Tag.ToString(), info1.Item.ListView, e.X + 15, e.Y + 15, 20000);
                        
                    }
                    else
                    {
                        //MessageBox.Show("Hide");
                        mTooltip1.SetToolTip(listView1, string.Empty);
                    }
                }

                mLastPos1 = e.Location;
            }
            catch (Exception eaa)
            {
                mTooltip1.SetToolTip(listView1, string.Empty);
            }
        }

        private void listView1_MouseLeave(object sender, EventArgs e)
        {
            
            mTooltip1.Hide(listView1);
        }



        private void label21_Click(object sender, EventArgs e)
        {
            //SaveForm saveform = new SaveForm();
            //saveform.ShowDialog();
            //defaultUser = saveform.defaultuser;
            //UserName = saveform.username;
            //Password = saveform.password;
           
        }

        
        

        private void ccbox_Click(object sender, EventArgs e)
        {
            
        }
       

        private void listView2_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void listView3_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void listView4_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView2_DrawSubItem_1(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        private void listView1_DrawSubItem_1(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView2_DrawColumnHeader_1(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 11, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }
        private void listView3_DrawColumnHeader_1(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 11, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }
        private void listView4_DrawColumnHeader_1(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 11, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }
        private void listView1_DrawColumnHeader_1(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 11, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void buttonLastest1_Click(object sender, EventArgs e)
        {
            //SaveForm saveform = new SaveForm();
            //saveform.ShowDialog();
            //defaultUser = saveform.defaultuser;
            //UserName = saveform.username;
            //Password = saveform.password;
            //path1 = saveform.path;
            //port1 = saveform.port;
            //partition = saveform.partition;
            //dc1 = saveform.dc;

        }

        private void button20_Click(object sender, EventArgs e)
        {
            Button Sender = (Button)sender;
            Point ptLowerRight = new Point(Sender.Width - 251, Sender.Height);
            ptLowerRight = Sender.PointToScreen(ptLowerRight);
            contextMenuStrip1.Show(ptLowerRight);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ListView.CheckedListViewItemCollection collection = listView2.CheckedItems;

            if (collection.Count < 1)
                MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DialogResult result;

                result = MessageBox.Show("Are you sure you want to delete the user(s) ?", "Confirm User(s) Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;

                String adsPathRemove;
                int temp = 0;

                for (int i = 0; i < collection.Count; i++)
                {
                    ListViewItem item = collection[i];
                    try
                    {
                        adsPathRemove = "LDAP://" + dc1 + ":" + port1 + "/" + item.SubItems[3].Text;
                        DirectoryEntry compEntries;
                        AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(adsPathRemove, UserName, Password,authtype);
                        else
                            compEntries = new DirectoryEntry(adsPathRemove);
                        DirectoryEntry ou = compEntries.Parent;
                        ou.Children.Remove(compEntries);
                        ou.CommitChanges();
                        //DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        //compEntries.Children.Remove(myChildEntry);
                        //compEntries.CommitChanges();
                        //myChildEntry.Dispose();
                        //compEntries.Dispose();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have sufficient permission to delete the user(s) ", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //label21.Visible = true;

                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("User(s) cannot be deleted.  ", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                } // for loop
                if (temp == 1)
                {
                    //refreshAll();
                    //                    refreshListView();
                    MessageBox.Show("User(s) deleted succesfully ", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    button3.Enabled = true;
                    button3_Click(sender,e);
                    //ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    //cl.executeDll(14);
                }
            } // else
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
                const long ADS_OPTION_PASSWORD_METHOD = 7;

                //const int ADS_PASSWORD_ENCODE_REQUIRE_SSL = 0;
                const int ADS_PASSWORD_ENCODE_CLEAR = 1;
                int intPort = 0;
                intPort = Int32.Parse(port1);


                ListView.CheckedListViewItemCollection collection = listView2.CheckedItems;

                if (collection.Count < 1)
                    MessageBox.Show("Please select at least one User", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    String Path;
                    int temp = 0;

                    for (int i = 0; i < collection.Count; i++)
                    {
                        ListViewItem item = collection[i];
                        try
                        {
                            Path = "LDAP://" + dc1 + ":" + port1 + "/" + item.SubItems[3].Text;
                            //MessageBox.Show("enabling user " + Path);
                            DirectoryEntry compEntries;
                            AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
                            if (!defaultUser)
                            {
                                compEntries = new DirectoryEntry(Path, UserName, Password, authtype);

                            }
                            else
                            {
                                compEntries = new DirectoryEntry(Path);
                            }

                            compEntries.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_PORTNUMBER, intPort });
                            compEntries.Invoke("SetOption", new object[]
                    {ADS_OPTION_PASSWORD_METHOD,
                     ADS_PASSWORD_ENCODE_CLEAR});

                            if ((bool)compEntries.Properties["msDS-UserAccountDisabled"].Value == true)
                                compEntries.Properties["msDS-UserAccountDisabled"].Value = false;
                            compEntries.CommitChanges();
                            temp = 1;
                        }
                        catch (System.UnauthorizedAccessException ex)
                        {
                            MessageBox.Show("You don't have permission to enable the User(s) in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //label21.Visible = true;

                            //break;
                        }
                        catch (System.ArgumentNullException exx)
                        {
                            MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (Exception ex)
                        {
                            String excep = ex.Message;
                            if (excep.Contains("constraint violation occurred") == true)
                            {
                                MessageBox.Show(item.SubItems[0].Text + " - Reset Password before enabling the object ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //break;
                            }
                            else
                            {
                                MessageBox.Show(item.SubItems[0].Text + " Cannot perform the operation ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //break;
                            }
                        }

                    }  // for loop
                    if (temp == 1)
                    {
                        //refreshAll();
                        // refreshListView();
                        MessageBox.Show("User(s) Enabled Succesfully ", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        button3.Enabled = true;
                        button3_Click(sender, e);
                    } // if

                } //else
            }
            catch (System.ArgumentNullException exx)
            {
                MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
                const long ADS_OPTION_PASSWORD_METHOD = 7;

                //const int ADS_PASSWORD_ENCODE_REQUIRE_SSL = 0;
                const int ADS_PASSWORD_ENCODE_CLEAR = 1;
                int intPort = 0;
                intPort = Int32.Parse(port1);



                ListView.CheckedListViewItemCollection collection = listView2.CheckedItems;

                if (collection.Count < 1)
                    MessageBox.Show("Please select at least one user", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    String Path;
                    int temp = 0;

                    for (int i = 0; i < collection.Count; i++)
                    {
                        ListViewItem item = collection[i];
                        try
                        {
                            Path = "LDAP://" + dc1 + ":" + port1 + "/" + item.SubItems[3].Text;
                            //MessageBox.Show("Deleting user " + Path);
                            DirectoryEntry compEntries;
                            AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
                            if (!defaultUser)
                            {
                                compEntries = new DirectoryEntry(Path, UserName, Password, authtype);

                            }
                            else
                            {
                                compEntries = new DirectoryEntry(Path);
                            }

                            compEntries.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_PORTNUMBER, intPort });
                            compEntries.Invoke("SetOption", new object[]
                    {ADS_OPTION_PASSWORD_METHOD,
                     ADS_PASSWORD_ENCODE_CLEAR});

                            if ((bool)compEntries.Properties["msDS-UserAccountDisabled"].Value == false)
                                compEntries.Properties["msDS-UserAccountDisabled"].Value = true;
                            compEntries.CommitChanges();
                            temp = 1;
                        }
                        catch (System.UnauthorizedAccessException ex)
                        {
                            MessageBox.Show("You don't have permission to disable the User(s) in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            //label21.Visible = true;

                            break;
                        }
                        catch (System.ArgumentNullException exx)
                        {
                            MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }  // for loop
                    if (temp == 1)
                    {
                        // refreshAll();
                        //  refreshListView();
                        MessageBox.Show("User(s) Disabled Succesfully ", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        button3.Enabled = true;
                        button3_Click(sender, e);
                    } // if
                    //MessageBox.Show("Click Get Users to update user list");
                }//else
            }
            catch (System.ArgumentNullException exx)
            {
                MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
          
            if (listView2.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one user", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                //RPForm.FormClosed += RPFormClosed;
                ResetPasswordForm RPForm = new ResetPasswordForm();
                RPForm.ShowDialog();
                if (RPForm.is_change)
                {
                    ListView.CheckedListViewItemCollection collection = new ListView.CheckedListViewItemCollection(new ListView());
                    collection = listView2.CheckedItems;

                    const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
                    const long ADS_OPTION_PASSWORD_METHOD = 7;

                    //const int ADS_PASSWORD_ENCODE_REQUIRE_SSL = 0;
                    const int ADS_PASSWORD_ENCODE_CLEAR = 1;
                    int intPort = 0;
                    intPort = Int32.Parse(port1);

                    for (int i = 0; i < collection.Count; i++)
                    {
                        //MessageBox.Show("collecton count: " + collection.Count.ToString());
                        ListViewItem item = collection[i];
                        try
                        {

                            String Path;
                            Path = "LDAP://" + dc1 + ":" + port1 + "/" + item.SubItems[3].Text;
                            //MessageBox.Show(item.SubItems[3].Text);
                            DirectoryEntry compEntries;
                            AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
                            if (!defaultUser)
                            {
                                compEntries = new DirectoryEntry(Path, UserName, Password, authtype);

                            }
                            else
                            {
                                compEntries = new DirectoryEntry(Path);
                            }

                            compEntries.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_PORTNUMBER, intPort });
                            compEntries.Invoke("SetOption", new object[]
                    {ADS_OPTION_PASSWORD_METHOD,
                     ADS_PASSWORD_ENCODE_CLEAR});


                            compEntries.Invoke("SetPassword", new object[] { RPForm.password });
                            MessageBox.Show("The password has been changed successfully for user " + item.Text + " in " + item.SubItems[2].Text, " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                        }

                        catch (System.Reflection.TargetInvocationException ex)
                        {
                            try
                            {
                                throw ex.InnerException;
                            }
                            catch (System.Runtime.InteropServices.COMException ex1)
                            {
                                MessageBox.Show("The password does not meet the password policy requirements", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            catch (System.UnauthorizedAccessException ex1)
                            {
                                MessageBox.Show("You don’t have sufficient permissions to reset passwords  ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }



                    }

                }
                //RPFormClosed(sender,e);
            }
         }

        //public void RPFormClosed(object sender, EventArgs e)
        //{
        //    if (RPForm.is_change)
        //    {
        //        ListView.CheckedListViewItemCollection collection = new ListView.CheckedListViewItemCollection(new ListView());
        //         collection = listView2.CheckedItems;
                
        //        const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
        //        const long ADS_OPTION_PASSWORD_METHOD = 7;

        //        //const int ADS_PASSWORD_ENCODE_REQUIRE_SSL = 0;
        //        const int ADS_PASSWORD_ENCODE_CLEAR = 1;
        //        int intPort = 0;
        //        intPort = Int32.Parse(port1);
            
        //          for (int i = 0; i < collection.Count; i++)
        //          {
        //            MessageBox.Show("collecton count: "+collection.Count.ToString());
        //            ListViewItem item = collection[i];
        //            try
        //            {
                        
        //                String Path;
        //                Path = "LDAP://" + dc1 + ":" + port1 + "/" + item.SubItems[3].Text;
        //                //MessageBox.Show(item.SubItems[3].Text);
        //                DirectoryEntry compEntries;
        //                AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
        //                if (!defaultUser)
        //                {
        //                    compEntries = new DirectoryEntry(Path, UserName, Password, authtype);

        //                }
        //                else
        //                {
        //                    compEntries = new DirectoryEntry(Path);
        //                }

        //                compEntries.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_PORTNUMBER, intPort });
        //                compEntries.Invoke("SetOption", new object[]
        //            {ADS_OPTION_PASSWORD_METHOD,
        //             ADS_PASSWORD_ENCODE_CLEAR});


        //                compEntries.Invoke("SetPassword", new object[] { RPForm.password });
        //                MessageBox.Show("Password is changed successfully for user " + item.Text + " in " + item.SubItems[2].Text, " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

        //            }

        //            catch (System.Reflection.TargetInvocationException ex)
        //            {
        //                try
        //                {
        //                    throw ex.InnerException;
        //                }
        //                catch (System.Runtime.InteropServices.COMException ex1)
        //                {
        //                    MessageBox.Show("The password does not meet the password policy requirements", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return;
        //                }
        //                catch (System.UnauthorizedAccessException ex1)
        //                {
        //                    MessageBox.Show("You don’t have sufficient permission to reset password  ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Unspecified Error", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

        //            }
                    
                    
                
        //        }
                  
        //    }
        //}

        private void button18_Click(object sender, EventArgs e)
        {
            if (listView2.CheckedItems.Count == 0)
                MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView2.CheckedItems.Count > 1)
                MessageBox.Show("Please select only one user", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                ListViewItem item = listView2.CheckedItems[0];
                try
                {
                    
                    /////////////////////////////////

                    PropertiesForm PropForm = new PropertiesForm();
                    PropForm.path = path1;
                    PropForm.dc = dc1;
                    PropForm.port = port1;
                    PropForm.partition = partition;
                    PropForm.user = item.SubItems[3].Text;
                    PropForm.userName = UserName;
                    PropForm.password = Password;
                    PropForm.defaultUser = defaultUser;
                    PropForm.ShowDialog();
                    if (PropForm.is_exc == true)
                    {

                        //label21.Visible = true;


                    }
                    
                    button3_Click(sender,e);
                    
                }
                catch (Exception ex)
                {
                    
                    if (ex.Message.Contains("There is no such object on the server"))
                    {
                        MessageBox.Show("The selected object has either been moved or deleted. Refresh the users and try again");
                    }
                    else
                    {
                        
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                listView2.Items.Clear();
                button2.Enabled = true;
                button14.Enabled = true;
                button21.Enabled = true;
                //label22.Text = "Loading User Details .. Please Wait ..";
                label22.Text = "Loading User Details .. Please Wait ..";
                startindex += 100;
                endindex += 100;
                if (endindex > results.Count)
                {
                    endindex = results.Count;
                    button21.Enabled = false;
                }
                if (endindex == results.Count)
                {
                    button1.Enabled = false;
                }
                listView2.BeginUpdate();
                for (int i = startindex; i < endindex; i++)
                {
                    ListViewItem item = new ListViewItem();

                    SearchResult sr = results[i];
                    String[] data = { "-", "-", "-" };

                    if (sr != null)
                    {

                        ResultPropertyCollection myResultPropColl;
                        myResultPropColl = sr.Properties;


                        foreach (string myKey in myResultPropColl.PropertyNames)
                        {

                            if (myKey.Trim().Equals("userprincipalname"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    data[0] = myCollection.ToString();


                                }
                            }
                            if (myKey.Trim().Equals("canonicalname"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    data[1] = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("distinguishedname"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    data[2] = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("msds-useraccountdisabled"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    if (myCollection.ToString().ToLower().Equals("true"))
                                    {
                                        item.ImageIndex = 1;

                                    }
                                    if (myCollection.ToString().ToLower().Equals("false"))
                                    {

                                        item.ImageIndex = 0;
                                    }
                                }
                            }
                            if (myKey.Trim().Equals("cn"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    item.Text = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("adspath"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    //do nothing

                                }
                            }
                        }

                        for (int j = 0; j < 3; j++)
                        {

                            item.SubItems.Add(data[j]);
                        }
                        listView2.Items.Add(item);

                    }

                }
                listView2.EndUpdate();
                //String display = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                //MessageBox.Show(display);
                //label8.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                label12.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                //label22.Text = "Done !";
                //MessageBox.Show(startindex.ToString()+"...."+endindex.ToString());
                label22.Text = "Done !";
                recolour();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform the action", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                listView2.Items.Clear();
                label22.Text = "Loading User Details .. Please Wait ..";
                listView2.BeginUpdate();
                startindex = startindex - 100;
                button21.Enabled = true;

                if (endindex == results.Count)
                {
                    //MessageBox.Show("1");
                    button1.Enabled = true;
                    int teeemp = endindex / 100;
                    endindex = teeemp * 100;
                    if(startindex==0)
                    {
                        button2.Enabled = false;
                        button1.Enabled = true;
                        button14.Enabled = false;
                    }

                }
                else if (startindex == 0)
                {
                    //MessageBox.Show("2");
                    button2.Enabled = false;
                    button1.Enabled = true;
                    button14.Enabled = false;
                    endindex = endindex - 100;
                }
                else
                {
                    //MessageBox.Show("2");
                    button1.Enabled = true;
                    endindex = endindex - 100;
                }



                for (int i = startindex; i < endindex; i++)
                {

                    ListViewItem item = new ListViewItem();

                    SearchResult sr = results[i];
                    String[] data = { "-", "-", "-" };

                    if (sr != null)
                    {

                        ResultPropertyCollection myResultPropColl;
                        myResultPropColl = sr.Properties;


                        foreach (string myKey in myResultPropColl.PropertyNames)
                        {

                            if (myKey.Trim().Equals("userprincipalname"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    data[0] = myCollection.ToString();


                                }
                            }
                            if (myKey.Trim().Equals("canonicalname"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    data[1] = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("distinguishedname"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    data[2] = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("msds-useraccountdisabled"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    if (myCollection.ToString().ToLower().Equals("true"))
                                    {
                                        item.ImageIndex = 1;

                                    }
                                    if (myCollection.ToString().ToLower().Equals("false"))
                                    {

                                        item.ImageIndex = 0;
                                    }
                                }
                            }
                            if (myKey.Trim().Equals("cn"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    item.Text = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("adspath"))
                            {
                                foreach (Object myCollection in myResultPropColl[myKey])
                                {
                                    //do nothing

                                }
                            }
                        }

                        for (int j = 0; j < 3; j++)
                        {

                            item.SubItems.Add(data[j]);
                        }
                        listView2.Items.Add(item);

                    }

                }
                //label8.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                label12.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                //label22.Text = "Done !";
                //MessageBox.Show(startindex.ToString() + "...." + endindex.ToString());
                label22.Text = "Done !";
                recolour();
                listView2.EndUpdate();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform the action", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            
            button4.Enabled = true;
            button7.Enabled = true;
            button6.Enabled = true;
            button5.Enabled = true;
            button22.Enabled = true;
            button23.Enabled = true;
            label22.Text = "Loading Group Details .. Please Wait ..";
            label22.Refresh();
            
            
            if ((UserName.Equals("") && !Password.Equals("")) || (!UserName.Equals("") && Password.Equals("")))
            {
                MessageBox.Show("Please enter the username & password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = true;
                return;
            }
            try
            {
                DirectoryEntry de1;
                if (!defaultUser)
                {
                    de1 = new DirectoryEntry(path1, UserName, Password, AuthenticationTypes.Secure);
                    
                }
                else
                {
                    de1 = new DirectoryEntry(path1);
                    //de.Dispose();
                }

                //------------------------------------------------------------------------------------------------

                de1.RefreshCache(new string[] { "canonicalName" });
                DirectorySearcher ds1 = new DirectorySearcher(de1);

                ds1.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                if (issearchset != 1)
                {
                    ds1.Filter = "(objectClass=group)";
                }
                else
                {
                    //MessageBox.Show(filtertext);
                    quicksearchflagg = 1;
                    if (filtertext != "")
                    {
                        ds1.Filter = filtertext;
                        filtertext = "";
                    }
                    else
                    {
                        filtertext = "";
                        ds1.Filter = "(objectClass=group)";

                    }
                }
                //MessageBox.Show(filtertext);
                ds1.PageSize = 100;
                ds1.PropertiesToLoad.Add("canonicalName");
                ds1.PropertiesToLoad.Add("cn");
                ds1.PropertiesToLoad.Add("groupType");
                //ds.PropertiesToLoad.Add("userPrincipalName");
                ds1.PropertiesToLoad.Add("distinguishedName");
                //ds1.PropertyNamesOnly = true;
                ds1.Sort.Direction = SortDirection.Ascending;
                ds1.Sort.PropertyName = "cn";
                results1 = ds1.FindAll();

                if (results1.Count == 0)
                {
                    if (fromgrpgo != 2)
                    {
                        MessageBox.Show("Cannot find the objects");
                        label8.Text = "0 - 0 of 0";
                        startindexg = 0;
                        endindexg = 0;
                        label22.Text = "";
                    }
                    textBox5.Text = "";
                    textBox4.Text="";
                    textBox3.Text = "";
                    issearchset = 0;
                    if (fromgrpgo == 1)
                    {
                        fromgrpgo = 2;
                        button4_Click_1(sender, e);
                    }
                }
                else
                {
                    de1.Dispose();

                    int go1 = 100;
                    if (results1.Count < 100)
                    {
                        go1 = results1.Count;
                        button5.Enabled = false;
                        button6.Enabled = false;
                        button22.Enabled = false;
                        button23.Enabled = false;
                    }


                    listView1.BeginUpdate();

                    for (int i = 0; i < go1; i++)
                    {

                        ListViewItem item = new ListViewItem();

                        SearchResult sr1 = results1[i];
                        String[] data1 = { "-", "-", "-", "-" };

                        if (sr1 != null)
                        {

                            ResultPropertyCollection myResultPropColl1;
                            myResultPropColl1 = sr1.Properties;


                            foreach (string myKey in myResultPropColl1.PropertyNames)
                            {

                                if (myKey.Trim().Equals("canonicalname"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        data1[2] = myCollection.ToString();


                                    }
                                }

                                if (myKey.Trim().Equals("distinguishedname"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        data1[3] = myCollection.ToString();

                                    }
                                }

                                if (myKey.Trim().Equals("cn"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        item.Text = myCollection.ToString();

                                    }
                                }
                                if (myKey.Trim().Equals("adspath"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        //do nothing

                                    }
                                }
                                if (myKey.Trim().Equals("grouptype"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        if (myCollection.ToString() == "-2147483640")
                                        {
                                            data1[0] = "Universal Group";
                                            data1[1] = "Security Group";
                                        }
                                        if (myCollection.ToString() == "-2147483644")
                                        {
                                            data1[0] = "Domain Local Group";
                                            data1[1] = "Security Group";
                                        }
                                        if (myCollection.ToString() == "-2147483646")
                                        {
                                            data1[0] = "Global Group";
                                            data1[1] = "Security Group";
                                        }
                                        if (myCollection.ToString() == "8")
                                        {
                                            data1[0] = "Universal Group";
                                            data1[1] = "Distribution Group";
                                        }
                                        if (myCollection.ToString() == "4")
                                        {
                                            data1[0] = "Domain Local Group";
                                            data1[1] = "Distribution Group";
                                        }
                                        if (myCollection.ToString() == "2")
                                        {
                                            data1[0] = "Global Group";
                                            data1[1] = "Distribution Group";
                                        }

                                    }
                                }
                            }

                            for (int j = 0; j < 4; j++)
                            {

                                item.SubItems.Add(data1[j]);
                            }
                            listView1.Items.Add(item);

                        }
                        listView1.EndUpdate();
                        startindexg = 0;
                        endindexg = 100;
                        //MessageBox.Show("test3");

                        button5.Enabled = true;
                        if (results1.Count < 100)
                        {
                            button5.Enabled = false;
                            button6.Enabled = false;
                            button22.Enabled = false;
                            button23.Enabled = false;
                            endindexg = results1.Count;
                        }
                        button6.Enabled = false;
                        button22.Enabled = false;
                        label8.Text = (startindexg + 1).ToString() + " - " + endindexg.ToString() + " of " + results1.Count.ToString();
                        label8.Refresh();
                        recolour1();
                        //MessageBox.Show("test3");
                    }
                    label22.Text = "";
                    label22.Refresh();
                    //label18.Visible = false;
                }
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show(" The network path was not found ");
                button3.Enabled = true;
                return;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter The Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button3.Enabled = true;
                return;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Error ");
                button3.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure. Please enter the username & password correctly", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button3.Enabled = true;
                    return;
                }
                else
                {
                    //MessageBox.Show("test UE");
                    MessageBox.Show("Error - " + ex.Message);
                    button3.Enabled = true;
                    return;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                button6.Enabled = true;
                button22.Enabled = true;
                button23.Enabled = true;
                //label22.Text = "Loading User Details .. Please Wait ..  results";
                label22.Text = "Loading Group Details .. Please Wait ..";
                startindexg += 100;
                endindexg += 100;
                if (endindexg > results1.Count)
                {
                    endindexg = results1.Count;
                    button23.Enabled = false;
                }
                if (endindexg == results1.Count)
                {
                    button5.Enabled = false;
                }
                listView1.BeginUpdate();
                for (int i = startindexg; i < endindexg; i++)
                {
                    ListViewItem item = new ListViewItem();

                    SearchResult sr1 = results1[i];
                    String[] data1 = { "-", "-", "-", "-" };

                    if (sr1 != null)
                    {

                        ResultPropertyCollection myResultPropColl1;
                        myResultPropColl1 = sr1.Properties;


                        foreach (string myKey in myResultPropColl1.PropertyNames)
                        {

                            if (myKey.Trim().Equals("canonicalname"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    data1[2] = myCollection.ToString();


                                }
                            }

                            if (myKey.Trim().Equals("distinguishedname"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    data1[3] = myCollection.ToString();

                                }
                            }

                            if (myKey.Trim().Equals("cn"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    item.Text = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("adspath"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    //do nothing

                                }
                            }
                            if (myKey.Trim().Equals("grouptype"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    if (myCollection.ToString() == "-2147483640")
                                    {
                                        data1[0] = "Universal Group";
                                        data1[1] = "Security Group";
                                    }
                                    if (myCollection.ToString() == "-2147483644")
                                    {
                                        data1[0] = "Domain Local Group";
                                        data1[1] = "Security Group";
                                    }
                                    if (myCollection.ToString() == "-2147483646")
                                    {
                                        data1[0] = "Global Group";
                                        data1[1] = "Security Group";
                                    }
                                    if (myCollection.ToString() == "8")
                                    {
                                        data1[0] = "Universal Group";
                                        data1[1] = "Distribution Group";
                                    }
                                    if (myCollection.ToString() == "4")
                                    {
                                        data1[0] = "Domain Local Group";
                                        data1[1] = "Distribution Group";
                                    }
                                    if (myCollection.ToString() == "2")
                                    {
                                        data1[0] = "Global Group";
                                        data1[1] = "Distribution Group";
                                    }

                                }
                            }
                        }

                        for (int j = 0; j < 4; j++)
                        {

                            item.SubItems.Add(data1[j]);
                        }
                        listView1.Items.Add(item);

                    }
                    listView1.EndUpdate();
                    //String display = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                    //MessageBox.Show(display);
                    //label8.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                    label8.Text = (startindexg + 1).ToString() + " - " + endindexg.ToString() + " of " + results1.Count.ToString();
                    //label22.Text = "Done !";
                    label22.Text = "Done !";
                    recolour1();
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform the action", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                listView1.Items.Clear();
                button23.Enabled = true;
                label22.Text = "Loading User Details .. Please Wait ..";
                listView1.BeginUpdate();
                startindexg = startindexg - 100;

                if (endindexg == results1.Count)
                {
                    button5.Enabled = true;
                    int teeemp = endindexg / 100;

                    endindexg = teeemp * 100;
                    if(startindexg==0)
                    {
                        button6.Enabled = false;
                        button22.Enabled = false;
                    }

                }
                else if (startindexg == 0)
                {
                    button6.Enabled = false;
                    button5.Enabled = true;
                    button22.Enabled = false;
                    endindexg = endindexg - 100;
                }
                else
                {
                    button5.Enabled = true;
                    endindexg = endindexg - 100;
                }



                for (int i = startindexg; i < endindexg; i++)
                {

                    ListViewItem item = new ListViewItem();

                    SearchResult sr1 = results1[i];
                    String[] data1 = { "-", "-", "-", "-" };

                    if (sr1 != null)
                    {

                        ResultPropertyCollection myResultPropColl1;
                        myResultPropColl1 = sr1.Properties;


                        foreach (string myKey in myResultPropColl1.PropertyNames)
                        {

                            if (myKey.Trim().Equals("canonicalname"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    data1[2] = myCollection.ToString();


                                }
                            }

                            if (myKey.Trim().Equals("distinguishedname"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    data1[3] = myCollection.ToString();

                                }
                            }

                            if (myKey.Trim().Equals("cn"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    item.Text = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("adspath"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    //do nothing

                                }
                            }
                            if (myKey.Trim().Equals("grouptype"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    if (myCollection.ToString() == "-2147483640")
                                    {
                                        data1[0] = "Universal Group";
                                        data1[1] = "Security Group";
                                    }
                                    if (myCollection.ToString() == "-2147483644")
                                    {
                                        data1[0] = "Domain Local Group";
                                        data1[1] = "Security Group";
                                    }
                                    if (myCollection.ToString() == "-2147483646")
                                    {
                                        data1[0] = "Global Group";
                                        data1[1] = "Security Group";
                                    }
                                    if (myCollection.ToString() == "8")
                                    {
                                        data1[0] = "Universal Group";
                                        data1[1] = "Distribution Group";
                                    }
                                    if (myCollection.ToString() == "4")
                                    {
                                        data1[0] = "Domain Local Group";
                                        data1[1] = "Distribution Group";
                                    }
                                    if (myCollection.ToString() == "2")
                                    {
                                        data1[0] = "Global Group";
                                        data1[1] = "Distribution Group";
                                    }

                                }
                            }
                        }

                        for (int j = 0; j < 4; j++)
                        {

                            item.SubItems.Add(data1[j]);
                        }
                        listView1.Items.Add(item);

                    }
                    //label8.Text = (startindex + 1).ToString() + " - " + endindex.ToString() + " of " + results.Count.ToString();
                    label8.Text = (startindexg + 1).ToString() + " - " + endindexg.ToString() + " of " + results1.Count.ToString();
                    //label22.Text = "Done !";
                    label22.Text = "Done !";
                    recolour1();
                    listView1.EndUpdate();
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform the action", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ListView.CheckedListViewItemCollection collection = listView1.CheckedItems;
            button4.Enabled = false;
            if (collection.Count < 1)
                MessageBox.Show("Please select at least one group", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DialogResult result;

                result = MessageBox.Show("Are you sure you want to delete the group(s) ?", "Confirm Group(s) Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;

                String adsPathRemove;
                int temp = 0;

                for (int i = 0; i < collection.Count; i++)
                {
                    ListViewItem item = collection[i];
                    try
                    {
                        adsPathRemove = "LDAP://" + dc1 + ":" + port1 + "/" + item.SubItems[4].Text;
                        
                        DirectoryEntry compEntries;
                        AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(adsPathRemove, UserName, Password, authtype);
                        else
                            compEntries = new DirectoryEntry(adsPathRemove);
                        DirectoryEntry ou = compEntries.Parent;
                        ou.Children.Remove(compEntries);
                        
                        ou.CommitChanges();
                        //DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        //compEntries.Children.Remove(myChildEntry);
                        //compEntries.CommitChanges();
                        //myChildEntry.Dispose();
                        //compEntries.Dispose();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have sufficient permission to delete the user(s) in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //label21.Visible = true;

                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Group(s) cannot be deleted. "+ex.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                } // for loop
                if (temp == 1)
                {
                    //refreshAll();
                    //                    refreshListView();
                    MessageBox.Show("Groups(s) deleted succesfully, Refresh the page ", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    button4.Enabled = true;
                    button4_Click_1(sender,e);
                    //ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    //cl.executeDll(14);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            ContainerTree cs1 = new ContainerTree();
            
            cs1.userName = UserName;
            
            cs1.password = Password;
            
            cs1.path = path1;
            cs1.port = port1;
            cs1.partition = partition;
            cs1.defaultUser = defaultUser;
            cs1.dc = dc1;
            
            cs1.moveuserflag = 1;
            ListView.CheckedListViewItemCollection useritem = listView2.CheckedItems;
            cs1.useritem = useritem;
            if (useritem.Count > 0)
            {
                cs1.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select at least one User", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            button3_Click(sender,e);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            listView2.Location = new System.Drawing.Point(7,102);
            textBox1.Visible = true;
            textBox2.Visible = true;
            
            button11.Visible = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "" && textBox2.Text == "" )
            {
                MessageBox.Show("Enter a search term");
                issearchsetg = 0;
                fromgo = 1;
                button3_Click(sender,e);
                
            }
            else
            {
                issearchsetg = 1;
                String appendtext="";
                filtertext = "(objectClass=user)";
                if (textBox1.Text != "")
                {
                    appendtext += "(cn=*"+textBox1.Text+"*)";
                    
                }
                if (textBox2.Text != "")
                {
                    appendtext += "(userPrincipalName=*" + textBox2.Text + "*)";

                }
                
                
                filtertext = "(&" + filtertext + appendtext + ")";
                //MessageBox.Show(filtertext);
                fromgo = 1;
                button3_Click(sender, e);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            listView1.Location = new System.Drawing.Point(7, 102);
            textBox3.Visible = true;
            textBox4.Visible = true;
            textBox5.Visible = true;
            button13.Visible = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            
            if (textBox3.Text == "" && textBox4.Text == "" && textBox5.Text == "")
            {
                MessageBox.Show("Enter a search term");
                issearchset = 0;
                fromgrpgo = 1;
                button4_Click_1(sender, e);
            }
            else
            {
                issearchset = 1;
                String appendtext = "";
                filtertext = "(objectClass=group)";
                if (textBox3.Text != "")
                {
                    
                    appendtext += "(cn=*" + textBox3.Text + "*)";
                }
                if(textBox5.Text.ToLower().Contains("security") && textBox4.Text == "" )
                {
                    
                    appendtext += "(groupType:1.2.840.113556.1.4.803:=2147483648)";
                }
                if (textBox5.Text.ToLower().Contains("distribution") && textBox4.Text == "")
                {
                    
                    appendtext += "(!(groupType:1.2.840.113556.1.4.803:=2147483648))";
                }
                if (textBox5.Text.ToLower().Contains("security") && textBox4.Text != "")
                {
                    if (textBox4.Text.ToLower().Contains("global"))
                    {
                        
                        appendtext += "(groupType:1.2.840.113556.1.4.803:=-2147483646)";
                    }
                    if (textBox4.Text.ToLower().Contains("universal"))
                    {
                        
                        appendtext += "(groupType:1.2.840.113556.1.4.803:=-2147483640)";
                    }
                    if (textBox4.Text.ToLower().Contains("domain"))
                    {
                        
                        appendtext += "(groupType:1.2.840.113556.1.4.803:=-2147483644)";
                    }
                }
                if (textBox5.Text.ToLower().Contains("distribution") && textBox4.Text != "")
                {
                    if (textBox4.Text.ToLower().Contains("global"))
                    {
                        
                        appendtext += "(groupType=2)";
                    }
                    if (textBox4.Text.ToLower().Contains("universal"))
                    {
                        
                        appendtext += "(groupType=8)";
                    }
                    if (textBox4.Text.ToLower().Contains("domain"))
                    {
                        
                        appendtext += "(groupType=4)";
                    }
                }
                if (textBox5.Text == "" && textBox4.Text.ToLower().Contains("global"))
                {
                    
                    appendtext += "(groupType:1.2.840.113556.1.4.803:=2)";
                }
                if (textBox5.Text == "" && textBox4.Text.ToLower().Contains("domain"))
                {
                    
                    appendtext += "(groupType:1.2.840.113556.1.4.803:=4)";
                }
                if (textBox5.Text == "" && textBox4.Text.ToLower().Contains("universal"))
                {
                    
                    appendtext += "(groupType:1.2.840.113556.1.4.803:=8)";
                }
                if (!(textBox5.Text.ToLower().Equals("") || textBox5.Text.ToLower().Contains("security") || textBox5.Text.ToLower().Contains("distribution")))
                {
                    
                    MessageBox.Show("Enter valid terms as search query for group type");
                    textBox5.Text = "";
                }
                if (!(textBox4.Text.ToLower().Equals("") || textBox4.Text.ToLower().Contains("domain") || textBox4.Text.ToLower().Contains("global") || textBox4.Text.ToLower().Contains("universal")))
                {
                    
                    MessageBox.Show("Enter valid terms as search query for group scope");
                    textBox4.Text = "";
                }
                filtertext = "(&" + filtertext + appendtext + ")";
                //MessageBox.Show(filtertext);
                fromgrpgo = 1;
                button4_Click_1(sender, e);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            int temp = results.Count % 100;
            endindex = results.Count - temp;
            startindex = endindex - 100;
            
            button1_Click(sender,e);
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            startindex = 100;
            endindex = 200;
            button2_Click_1(sender,e);
        }

        private void button22_Click(object sender, EventArgs e)
        {
            startindexg = 100;
            endindexg = 200;
            button6_Click(sender,e);

        }

        private void button23_Click(object sender, EventArgs e)
        {
            int temp = results1.Count % 100;
            endindexg = results1.Count - temp;
            startindexg = endindexg - 100;

            button5_Click(sender, e);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            //defaultUser = saveform.defaultuser;
            int domainuser = 0;
            UserName = textBox9.Text.Trim();
            Password = textBox10.Text.Trim();
            listView1.Items.Clear();
            listView2.Items.Clear();
            
            port1 = textBox7.Text.Trim();
            partition = textBox8.Text.Trim();
            dc1 = textBox6.Text.Trim();
            if(checkBox1.Checked==true)
            {
                defaultUser = true;
                UserName = "";
                Password = "";
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox9.Refresh();
                textBox10.Refresh();
            }
            else
            {
                defaultUser = false;
                textBox9.Enabled = true;
                textBox10.Enabled = true;
                textBox9.Refresh();
                textBox10.Refresh();
            }
            path1 = "LDAP://" + dc1 + ":" + port1 + "/" + partition;
            string check = "LDAP://" + dc1 + ":" + port1 + "/RootDSE";
            string config="";
            try
            {
                DirectoryEntry de;
                if (checkBox1.Checked == false)
                {
                    de = new DirectoryEntry(check, UserName, Password, AuthenticationTypes.Secure);
                }
                else
                {
                    de = new DirectoryEntry(check);
                }
                PropertyCollection myResultPropColl;
                myResultPropColl = de.Properties;


                foreach (string myKey in myResultPropColl.PropertyNames)
                {

                    if (myKey.Trim().Equals("configurationNamingContext"))
                    {
                        foreach (Object myCollection in myResultPropColl[myKey])
                        {
                            //MessageBox.Show(myCollection.ToString());
                            config = myCollection.ToString();


                        }
                    }
                }
                check = "LDAP://" + dc1 + ":" + port1 + "/" + config;
                if (checkBox1.Checked == false)
                {
                    de = new DirectoryEntry(check, UserName, Password, AuthenticationTypes.Secure);
                }
                else
                {
                    de = new DirectoryEntry(check);
                }
                de.RefreshCache(new string[] { "allowedAttributesEffective" });
                myResultPropColl = de.Properties;
                foreach (string myKey in myResultPropColl.PropertyNames)
                {


                    if (myKey.Trim().Equals("allowedAttributesEffective"))
                    {

                        foreach (Object myCollection in myResultPropColl[myKey])
                        {
                            if (myCollection.ToString().Trim().Equals("gPLink"))
                            {
                                MessageBox.Show("Please provide the AD LDS credentials");
                                domainuser = 1;
                            }


                        }
                    }

                }
                if (domainuser == 0)
                {
                    tabControl1.SelectedTab = tabPage1;
                    tabPage1.Enabled = true;
                    tabPage2.Enabled = true;
                    
                    button3_Click(sender,e);
                    //button4_Click_1(sender, e);
                    label20.Text = "Connected to LDS instance: " + dc1 + ": " + port1 ;
                    label21.Text = "Connected to LDS instance: " + dc1 + ": " + port1 ;
                    label20.Refresh();
                    label21.Refresh();
                }
            }
            catch (Exception eu)
            {
                MessageBox.Show(eu.Message);
            }

            
            
        }

        private void label18_Click(object sender, EventArgs e)
        {
            if (searchflagg == 0)
            {
                listView1.Location = new System.Drawing.Point(0, 147);
                listView1.Size = new System.Drawing.Size(728, 281);
                textBox3.Visible = true;
                textBox4.Visible = true;
                textBox5.Visible = true;
                button13.Visible = true;
                searchflagg++;
            }
            else
            {
                listView1.Location = new System.Drawing.Point(0, 115);
                listView1.Size = new System.Drawing.Size(728, 316);
                textBox3.Visible = false;
                textBox3.Text = "";
                textBox4.Visible = false;
                textBox4.Text = "";
                textBox5.Visible = false;
                textBox5.Text = "";
                button13.Visible = false;
                searchflagg--;
                if(quicksearchflagg==1)
                {
                    issearchset = 0;
                    button4_Click_1(sender, e);
                }
            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            if (searchflagu == 0)
            {
                listView2.Location = new System.Drawing.Point(0, 147);
                listView2.Size = new System.Drawing.Size(728, 281);
                textBox1.Visible = true;
                textBox2.Visible = true;
                button11.Visible = true;
                searchflagu++;
            }
            else
            {
                listView2.Location = new System.Drawing.Point(0, 115);
                listView2.Size = new System.Drawing.Size(728, 316);
                textBox1.Visible = false;
                textBox1.Text = "";
                textBox2.Visible = false;
                textBox2.Text = "";
                button11.Visible = false;
                searchflagu--;
                if(quicksearchflagu==1)
                {
                    issearchsetg = 0;
                    button3_Click(sender,e);
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                textBox9.Enabled = false;
                textBox10.Enabled = false;
                textBox9.Refresh();
                textBox10.Refresh();
            }
            else
            {
                textBox9.Enabled = true;
                textBox10.Enabled = true;
                textBox9.Refresh();
                textBox10.Refresh();
            }
        }
        private void listview3_columnwidthchanged(Object sender, ColumnWidthChangedEventArgs e)
        {
            columnHeader2.Width = columnHeader9.Width;
            columnHeader3.Width = columnHeader10.Width;
            columnHeader4.Width = columnHeader11.Width;
            columnHeader5.Width = columnHeader12.Width;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            label19_Click(sender,e);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            label18_Click(sender,e);
        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        

        

        

   

      
    } 
}
