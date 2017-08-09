using System;
using System.DirectoryServices;
using System.Collections;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ADLDSManagement
{
    public partial class PropertiesForm : Form
    {
        public String path;
        public String user;
        public String port;
        public String partition;
        public String dc;
        public String userName;
        public String password;
        public int flag = 0;
        public int focusflag = 0;
        public int memberselectedflag = 0;
        public string ounamechanged;
        public string upnchanged;
        public string descriptionchanged;
        public int closeflag = 0;
        public ListView.ListViewItemCollection retcollection;
        
        public ListView.ListViewItemCollection refinedvaluegroup = new ListView.ListViewItemCollection(new ListView());
        public ListView.ListViewItemCollection remcollection = new ListView.ListViewItemCollection(new ListView());
        public ListView.ListViewItemCollection removegroup = new ListView.ListViewItemCollection(new ListView());
        public PropertyValueCollection ValueCollection;
        String userpath;
        
        public bool defaultUser = false;
        public bool is_exc = false;
        
        
        public PropertiesForm()
        {
            InitializeComponent();
        }
        
        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;

                using (var headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void PropertiesForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Left = 350;
                this.Top = 150;
                this.MinimumSize = this.Size;
                this.MaximumSize = this.Size;
               
                button1.Enabled = true;
                userpath = "LDAP://" + dc + ":" + port + "/" + user;
                listView1.Items.Clear();
                if (flag == 1)
                {
                    tabPage2.Focus();
                }
                if (focusflag == 1)
                {
                    tabPage2.Focus();
                }
                ///////////////// Finding Groups
                if (flag == 0)
                {
                    //MessageBox.Show("test flag 0");
                    DirectoryEntry de1;
                    if (!defaultUser)
                    {
                        de1 = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure);
                    }
                    else
                    {
                        de1 = new DirectoryEntry(userpath);
                    }
                    //DirectoryEntry de1 = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure);
                    ValueCollection = de1.Properties["memberOf"];

                    foreach (String strgrp in ValueCollection)
                    {


                        ListViewItem item = new ListViewItem();
                        //getting groupName from the DN
                        String temp = strgrp.Remove(0, 3);
                        //MessageBox.Show(temp);
                        int start;
                        start = temp.IndexOf(",");

                        String finalstr;
                        finalstr = temp.Substring(0, start);
                        item.Text = finalstr;
                        listView1.Items.Add(item);

                    }
                    
                    //foreach (String sr in ValueCollection)
                    //{
                    //    MessageBox.Show(sr);
                    //}
                }
                else
                {
                    
                    listView1.Items.Clear();
                    if (retcollection != null)
                    {
                        
                        foreach (ListViewItem strgrp1 in retcollection)
                        {
                            ListViewItem item = new ListViewItem();
                            item.Text = strgrp1.Text;
                            listView1.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                    
                    if (refinedvaluegroup != null)
                    {
                        
                        foreach (ListViewItem strgrp1 in refinedvaluegroup)
                        {
                            //MessageBox.Show("check "+strgrp1.Text);
                            ListViewItem item1=null;
                            if (listView1.Items.Count > 0)
                            {
                                item1 = listView1.FindItemWithText(strgrp1.Text.Trim(), false, 0, false);
                            }
                            if (item1 == null)
                            {
                                //MessageBox.Show("not found");
                                listView1.Items.Add(strgrp1.Text);
                            }
                        }
                    }
                    //MessageBox.Show("test flag 3");
                    //foreach(ListViewItem li in listView1.Items)
                    //{
                    //    MessageBox.Show(li.Text);
                    //}

                }


                // User Entries

                DirectoryEntry userEntry;
                if (!defaultUser)
                {
                    userEntry = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure);
                }
                else
                {
                    userEntry = new DirectoryEntry(userpath);
                }
                String str = userEntry.Name;
                textBox1.Text = str.Replace("CN=", "");
                textBox2.Text = "" + userEntry.Properties["userPrincipalName"].Value;
                textBox3.Text = "" + userEntry.Properties["description"].Value;
                String str1 = userEntry.Properties["distinguishedName"].Value.ToString();
                String removestr = userEntry.Name + ",";
                textBox4.Text = str1.Replace(removestr, "");
                
                button3.Enabled = true; // Apply Button in tab-1


            }
            catch (Exception exx)
            {
                
                MessageBox.Show(exx.Message);
            }
        }

            // Apply Button in tab-1
        private void button3_Click(object sender, EventArgs e)
        {
            
            DirectoryEntry computerEntry;
            if( !defaultUser )
                computerEntry = new DirectoryEntry(userpath, userName, password);
            else
                computerEntry = new DirectoryEntry(userpath);

            try
            {

                computerEntry.Properties["userPrincipalName"].Value = textBox2.Text;                
                computerEntry.Properties["description"].Value = textBox3.Text;

                if (textBox2.Text == "")
                {
                    computerEntry.Properties["userPrincipalName"].Clear();
                }
                if(textBox3.Text=="")
                {
                    computerEntry.Properties["description"].Clear();
                }

                computerEntry.CommitChanges();                
                String containerpath = "LDAP://" + dc + ":" + port + "/" + textBox4.Text.Trim().ToString();
                DirectoryEntry theNewParent;
                if (!defaultUser)
                {
                    theNewParent = new DirectoryEntry(containerpath, userName, password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                }
                else
                {
                    theNewParent = new DirectoryEntry(containerpath);
                }
                
                DirectoryEntry theObjectToMove;
                if (!defaultUser)
                {
                    theObjectToMove = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                }
                else
                {
                    theObjectToMove = new DirectoryEntry(userpath);
                }
                
                
                theObjectToMove.MoveTo(theNewParent);
                userpath = "LDAP://" + dc + ":" + port + "/CN=" +textBox1.Text.Trim()+","+textBox4.Text.Trim() ;
                user = "CN=" + textBox1.Text.Trim() + "," + textBox4.Text.Trim();
                //MessageBox.Show(userpath);
                MessageBox.Show("The user details have been modified succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                button1_Click_1(sender,e);
                this.Close();
                
            }
            catch (System.UnauthorizedAccessException ex)
            {
                MessageBox.Show("You don't have permission to modify user details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                is_exc = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please provide valid details ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
            button3.Enabled = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            
            button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }                 

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection itemcollection1 = listView1.SelectedItems;
            
            foreach(ListViewItem lv in itemcollection1)
            {
                listView1.Items.Remove(lv);
                remcollection.Add(lv);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /////////////////////////////////////////////// User is added into the group
            //ListView.CheckedListViewItemCollection groupCollection = listView1.CheckedItems;
            //ListView.SelectedListViewItemCollection itemcollection = listView1.SelectedItems;
            ListView.ListViewItemCollection itemcollection = listView1.Items;
            try
            {

                for (int gro = 0; gro < itemcollection.Count; gro++)
                {
                    ListViewItem group = itemcollection[gro];
                    String groupName = group.Text;
                    DirectoryEntry groupEntries;
                    if (!defaultUser)
                    {
                        groupEntries = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
                    }
                    else
                    {
                        groupEntries = new DirectoryEntry(path);
                    }
                    DirectorySearcher ds2 = new DirectorySearcher(groupEntries);

                    ds2.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                    ds2.Filter = "(&(objectClass=group)(CN=" + groupName + "))";

                    SearchResult result1 = ds2.FindOne();

                    var grp = new DirectoryEntry(result1.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
                    grp.Properties["member"].Add(user);
                    grp.CommitChanges();
                }
                if (remcollection != null)
                {
                    for (int gro = 0; gro < remcollection.Count; gro++)
                    {
                        ListViewItem group = remcollection[gro];
                        String groupName = group.Text;
                        DirectoryEntry groupEntries;
                        if (!defaultUser)
                        {
                            groupEntries = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
                        }
                        else
                        {
                            groupEntries = new DirectoryEntry(path);
                        }
                        //DirectoryEntry groupEntries = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);

                        DirectorySearcher ds2 = new DirectorySearcher(groupEntries);

                        ds2.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                        ds2.Filter = "(&(objectClass=group)(CN=" + groupName + "))";

                        SearchResult result1 = ds2.FindOne();
                        if (result1 == null)
                        {
                            continue;
                        }
                        DirectoryEntry grp;
                        if (!defaultUser)
                        {
                            grp = new DirectoryEntry(result1.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
                        }
                        else
                        {
                            grp = new DirectoryEntry(result1.GetDirectoryEntry().Path);
                        }
                        //var grp = new DirectoryEntry(result1.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
                        grp.Properties["member"].Remove(user);
                        grp.CommitChanges();
                    }
                }
                //MessageBox.Show("Removed user from groups");
                PropertiesForm_Load(sender, e);
                //tabPage2.Focus(); 
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                try
                {
                    throw ex.InnerException;
                }
                catch (System.UnauthorizedAccessException ex1)
                {
                    MessageBox.Show("You don't have permission to modify User details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("Unspecified Group", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The specified account name is already a member of the local group."))
                    MessageBox.Show("This User already belongs to the specified group --"+ ex.Message, " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Unspecified Group found while performing the action", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Close();
            }

            try
            {

                for (int gro = 0; gro < removegroup.Count; gro++)
                {
                    ListViewItem group = removegroup[gro];
                    String groupName = group.Text;
                    DirectoryEntry groupEntries;
                    if (!defaultUser)
                    {
                        groupEntries = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
                    }
                    else
                    {
                        groupEntries = new DirectoryEntry(path);
                    }
                    //DirectoryEntry groupEntries = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);

                    DirectorySearcher ds2 = new DirectorySearcher(groupEntries);

                    ds2.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                    ds2.Filter = "(&(objectClass=group)(CN=" + groupName + "))";

                    SearchResult result1 = ds2.FindOne();
                    DirectoryEntry grp;
                    if (!defaultUser)
                    {
                        grp = new DirectoryEntry(result1.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
                    }
                    else
                    {
                        grp = new DirectoryEntry(result1.GetDirectoryEntry().Path);
                    }
                    //var grp = new DirectoryEntry(result1.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
                    grp.Properties["member"].Remove(user);
                    grp.CommitChanges();

                }

            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                try
                {
                    throw ex.InnerException;
                }
                catch (System.UnauthorizedAccessException ex1)
                {
                    MessageBox.Show("You don't have permission to modify user details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("Unspecified Group", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The specified account name is already a member of the local group."))
                    MessageBox.Show("This User already belongs to the specified group --" + ex.Message, " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Unspecified Group found while performing the action", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.Close();
            }
        }
        public void PropForm1_FormClosed(object sender, EventArgs e)
        {
            closeflag = 1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            upnchanged = textBox2.Text.Trim();
            descriptionchanged = textBox3.Text.Trim();
            ounamechanged = textBox4.Text.Trim();

            label6.Text = "Loading ... Please Wait....";
            label6.Refresh();
            MemberSelectionForm PropForm1 = new MemberSelectionForm();
            PropForm1.path = path;
            PropForm1.dc = dc;
            PropForm1.port = port;
            PropForm1.partition = partition;
            PropForm1.user = user;
            PropForm1.userName = userName;
            PropForm1.password = password;
            PropForm1.defaultUser = defaultUser;

            PropForm1.FormClosed+=new FormClosedEventHandler(PropForm1_FormClosed);
            //------------------------------------------------
            //Adding items in listview to Valuecollection
            ValueCollection.Clear();
            foreach (ListViewItem vi in listView1.Items)
            {
                ValueCollection.Add(vi.Text);
            }
            //------------------------------------------------
            PropForm1.ValueCollection = ValueCollection;
            PropForm1.ShowDialog();
            //closeflag = PropForm1.closeflag;
            flag = PropForm1.flag;
            if (flag == 1)
            {
                closeflag = 0;
            }
            if (closeflag == 0)
            {
                retcollection = PropForm1.groupCollection;
                removegroup = PropForm1.removegroup;
                refinedvaluegroup = PropForm1.refinedvaluegroup;
                flag = PropForm1.flag;
                //flag = 1;
                PropertiesForm_Load(sender, e);
                //MessageBox.Show(ounamechanged);
            }
            textBox4.Text = ounamechanged;
            textBox2.Text = upnchanged;
            textBox3.Text = descriptionchanged;
            label6.Text = "";
            label6.Refresh();
            tabPage2.Focus();
            
        }

        private void label5_Click(object sender, EventArgs e)
        {
            ContainerTree ct = new ContainerTree();
            ct.path = path;
            ct.dc = dc;
            ct.port = port;
            ct.partition = partition;
            ct.propertiesflag = 1;
            ct.userName = userName;
            ct.password = password;
            ct.defaultUser = defaultUser;
            ct.ShowDialog();
            if (ct.dnofcn != "")
            {
                textBox4.Text = ct.dnofcn;
                //ounamechanged = ct.dnofcn.Trim();
            }
        }
       
        
    }
}
