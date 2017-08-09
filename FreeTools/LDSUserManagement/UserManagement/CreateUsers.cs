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
   
    public partial class CreateUsers : Form
    {
        
        public String userName;
        public String password;
        public String path;
        public String port;
        public String partition;
        public String dc;

        public ListView.CheckedListViewItemCollection cnitem;
        ListView.ListViewItemCollection grpselected;
        public String present;
        public String past;
        public bool rightUser = false;
        public bool defaultUser = false;
        public int isAdded = 0;
        public bool is_exception = false ;
        public int csvflag = 0;
        public int flag = 0;
        public int closeflag = 0;
        public bool fromcsv = false;
        public System.Collections.Generic.List<String> computerList = new System.Collections.Generic.List<string>();
        //public PropertyValueCollection ValueCollection ;
        public System.DirectoryServices.PropertyValueCollection ValueCollection;
        public ListView.ListViewItemCollection createusergroup = new ListView.ListViewItemCollection(new ListView());
        //public System.Collections.ArrayList userList=new System.Collections.ArrayList();
        //AddMultipleComputers addSingleUserForm;
        AddMultipleUserForm addMultipleUserForm;

        public CreateUsers()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            this.Left = 200;
            this.Top = 70;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            
            CheckBox chkBox = new CheckBox();

            
            present = partition;
//-------------------------------------------------------------------------------------------

            
//---------------------------------------------------------------------------------------------
            chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "SelectAllGroups";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllGroups;
            //listView2.Controls.Add(chkBox);
//--------------------------------------------------------------------------------------------------
            
//---------------------------------------------------------------------------------------------------
            // For "Users to be added " ListView
            chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "SelectAllUsers";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllUsers;
            listView3.Controls.Add(chkBox);

            ListViewItem item2 = new ListViewItem("");
            item2.SubItems.Add("");
            item2.SubItems.Add("");
            item2.SubItems.Add("");
            listView3.Items.Add(item2);

            listView3.Scroll += new EventHandler(listView3_Scroll);
            DirectoryEntry de1;
            if (!defaultUser)
            {
                de1 = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
            }
            else
            {
                de1 = new DirectoryEntry(path);
            }
            //DirectoryEntry de1 = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure);
            ValueCollection = de1.Properties["memberOf"];
        }
        
        public void AddSingleUserFormClosed(object sender, EventArgs e)
        {
        //    if (addSingleUserForm.rightUser == true)
        //    {
        //        ListViewItem item = new ListViewItem(addSingleUserForm.user);
        //        item.SubItems.Add(addSingleUserForm.fullName);
        //        item.SubItems.Add(addSingleUserForm.desc);
        //        item.SubItems.Add(addSingleUserForm.pass);
        //        listView3.Items.Add(item);

        //        CheckBox cb = (CheckBox)listView3.Controls["SelectAllUsers"];
        //        cb.Checked = false;
        //        cb.Checked = true;
        //    }
        }
        // To check whether the CSV File is a valid file or not. It should be in the format of User Name,Full Name,Description,Password. Its checked with the help of comma seperator.
        public bool isValidFile(String myString)
        {
            int comma = 0;
            String[] subString = myString.Split('\n','\0'); // Splitting the CSV file into single-single row.
            for (int j = 0; j < subString.Length - 1; j++)
            {

                String item = subString[j];
                comma = 0;      // Reset the no. of commas to 0 for each and every single row.
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i].Equals(','))
                    {
                        comma = comma + 1;
                    }
                }

                if (comma != 3 && comma != 0)    // Checking for the no. of attributes(4) with the help of comma.
                    return false;
            }
            if (comma != 0)
                return true;
            else
                return false;

        }

        public void AddMultipleUserFormClosed(object sender, EventArgs e)
        {
            
            if (addMultipleUserForm.filePath == null)
            {
            }
            else if (addMultipleUserForm.isImport == false)
            {
            }
            else
            {
                
                System.Collections.Generic.List<String> userDetails = new System.Collections.Generic.List<string>();
                try
                {
                    System.IO.StreamReader myFile = new System.IO.StreamReader(addMultipleUserForm.filePath);
                }
                catch(Exception ev)
                {
                    if (ev.Message.Contains("because it is being used by another process"))
                    {
                        MessageBox.Show("The CSV file is open. Please close the file and try again ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                try
                {
                    System.IO.StreamReader myFile = new System.IO.StreamReader(addMultipleUserForm.filePath);
                    int i = 0, j = 0;
                    String myString = myFile.ReadToEnd();
                    ListViewItem item = new ListViewItem();
                    if (isValidFile(myString) == false)
                    {
                        MessageBox.Show("Invalid header in the CSV file.\nPlease provide the following attributes as its header: \n \tUser Name,UPN,Description,Password", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        myFile.Close();
                    }
                    else
                    {
                        myString = myString.Substring(myString.IndexOf('\n') + 1);
                        for (i = 0, j = 0; i < myString.Length; i++)
                        {
                            if (myString[i].Equals('\n'))
                            {

                                userDetails.Add(myString.Substring(j, i - j - 1));
                                i = i + 2;
                                j = i - 1;

                            }
                            else if (myString[i].Equals(','))
                            {

                                userDetails.Add(myString.Substring(j, i - j));
                                j = i + 1;

                            }
                        }
                        userDetails.Add(myString.Substring(j, myString.Length - j));  // For Last word in CSV File

                        for (i = 0; i < userDetails.Count; i++)
                        {
                            if (i % 4 == 0)
                            {
                                item = new ListViewItem();
                                item.Text = userDetails[i];
                            }
                            else if (i % 4 == 1)
                                item.SubItems.Add(userDetails[i]);
                            else if (i % 4 == 2)
                                item.SubItems.Add(userDetails[i]);
                            else
                            {
                                item.SubItems.Add(userDetails[i]);
                                listView3.Items.Add(item);

                                CheckBox cb = (CheckBox)listView3.Controls["SelectAllUsers"];
                                cb.Checked = false;
                                cb.Checked = true;
                            }
                        } // for loop
                    } // Inner else
                    myFile.Close();
                    fromcsv = true;
                } // Outer else

                catch (Exception)
                {
                    MessageBox.Show("The file does not exist"," Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        
        public void SelectAllGroups(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)listView2.Controls["SelectAllGroups"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = false;
        }
        public void SelectAllUsers(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)listView3.Controls["SelectAllUsers"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView3.Items.Count; i++)
                    listView3.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView3.Items.Count; i++)
                    listView3.Items[i].Checked = false;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        
        public void addUser()
        {
            //ListView.CheckedListViewItemCollection containerCollection = listView1.CheckedItems;
            ListView.ListViewItemCollection groupCollection = listView2.Items;
            //ListView.CheckedListViewItemCollection groupCollection = listView2.CheckedItems;
            ListView.CheckedListViewItemCollection userCollection = listView3.CheckedItems;
            //ListView.SelectedListViewItemCollection removegrp = listView2.SelectedItems;


                    try
                    {
                        //ListViewItem cont = containerCollection[0];
                        
                                          
                        //var grp = new DirectoryEntry(result.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
//----------------------------------------------------------------------------------------------------------------------------------
                        String contpath = "LDAP://" + dc + ":" + port + "/" + textBox5.Text.Trim();
                        //MessageBox.Show(contpath);
                        DirectoryEntry computerEntry;
                        if (!defaultUser)
                            computerEntry = new DirectoryEntry(contpath, userName, password,AuthenticationTypes.Secure);
                        
                        else
                            computerEntry = new DirectoryEntry(contpath);


                        for (int us = 0; us < userCollection.Count; us++)
                        {
                            
                            ListViewItem user = userCollection[us];
                            if (user.Text == "")
                            {
                                
                                if (!fromcsv)
                                {
                                    MessageBox.Show("Cannot add user without the username");
                                    isAdded = 2;
                                }
                                continue;
                            }
                            String username = "CN="+user.Text;
                            //MessageBox.Show(username);
                            if (!username.Equals(""))
                            {
                                String groupName = "";
                                
                                try
                                {
                                    DirectoryEntry userEntry = computerEntry.Children.Add(username, "User");
                                    
                                    userEntry.Properties["userPrincipalName"].Value = user.SubItems[1].Text;
                                    if (user.SubItems[2].Text != "")
                                    {
                                        userEntry.Properties["description"].Value = user.SubItems[2].Text;
                                    }
                                    userEntry.CommitChanges();
                                    
//-------------------------------------------------------------------------------------------------------------------------
                                    const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
                                    const long ADS_OPTION_PASSWORD_METHOD = 7;

                                    //const int ADS_PASSWORD_ENCODE_REQUIRE_SSL = 0;
                                    const int ADS_PASSWORD_ENCODE_CLEAR = 1;
                                    DirectoryEntry compEntries1;
                                    int intPort = 0;
                                    intPort = Int32.Parse(port);
                                    AuthenticationTypes authtype = AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure;
                                    if (!defaultUser)
                                    {
                                        compEntries1 = new DirectoryEntry(userEntry.Path, userName, password, authtype);

                                    }
                                    else
                                    {
                                        compEntries1 = new DirectoryEntry(userEntry.Path);
                                    }

                                    compEntries1.Invoke("SetOption", new object[] { ADS_OPTION_PASSWORD_PORTNUMBER, intPort });
                                    compEntries1.Invoke("SetOption", new object[]
                                                        {ADS_OPTION_PASSWORD_METHOD,
                                                         ADS_PASSWORD_ENCODE_CLEAR});


                                    compEntries1.Invoke("SetPassword", new object[] { user.SubItems[3].Text });
                                    compEntries1.CommitChanges();
                                    if ((bool)compEntries1.Properties["msDS-UserAccountDisabled"].Value == true)
                                        compEntries1.Properties["msDS-UserAccountDisabled"].Value = false;
                                    compEntries1.CommitChanges();
                                   
//-------------------------------------------------------------------------------------------------------------------------
                                    if (groupCollection != null)
                                    {
                                        for (int gro = 0; gro < groupCollection.Count; gro++)
                                        {
                                            ListViewItem group = groupCollection[gro];
                                            groupName = group.Text;
                                            DirectoryEntry groupEntries;
                                            if (!defaultUser)
                                            {
                                                groupEntries = new DirectoryEntry(path, userName, password, authtype);
                                            }
                                            else
                                            {
                                                groupEntries = new DirectoryEntry(path);
                                            }
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
                                            String tee = compEntries1.Properties["distinguishedName"].Value.ToString();

                                            grp.Properties["member"].Add(tee);
                                            grp.CommitChanges();

                                        }
                                    }
                                    isAdded = 1;
                                    
                                }
                                catch (System.UnauthorizedAccessException ex)
                                {
                                    isAdded = 2;
                                    MessageBox.Show("You dont have sufficient permissions to add user(s) ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    is_exception = true;
                                }
                                catch (Exception ex)
                                {
                                    isAdded = 2;
                                    if (ex.Message.Contains("The password does not meet the password policy requirements."))
                                        MessageBox.Show("The password does not meet the password policy requirements for user '" + (String)username + "'", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (ex.Message.Contains("The account already exists."))
                                        MessageBox.Show("\"" + (String)username + "\" already exists ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (ex.Message.Contains("An unknown directory object was requested"))
                                        MessageBox.Show("Error occurred while including " + (String)username + "\" at \"" + groupName + "\" Group ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (ex.Message.Contains("Exception has been thrown"))
                                        MessageBox.Show("Password does not meet password policy. User " + (String)username + " added as disabled", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else
                                    {
                                        MessageBox.Show("Error occurred while adding " + (String)username + " User. ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        
                                        if (ex.Message.Contains("Exception has been thrown"))
                                        {
                                            MessageBox.Show("The password does not meet password policy. User added as disabled", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                        
                                }
                                
                            }
                            
                        }// "For" loop for UserList.
                    }
                    catch (Exception ex)
                    {
                        isAdded = 2;
                        MessageBox.Show("Error in Getting Connection ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (isAdded == 1)
                    {
                        MessageBox.Show("User(s) Added Successfully ");
                        //isAdded = 0;
                    }
                    
                //} // "For" loop for ComputerCollection

            if ( isAdded == 0 || isAdded == 1 )
            {
                //ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                //cl.executeDll(13);
                this.Close();
                
            }
            else
            {
                if(listView3.CheckedItems.Count == 0 || (listView3.CheckedItems.Count == 0 && listView3.Items[0].Checked == true))
                MessageBox.Show("Please Select at least one User", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            
        } // addUser function
            // Add Button
        private void button1_Click(object sender, EventArgs e)
        {
            if ((!textBox1.Text.Equals("") && !textBox4.Text.Equals("")))
                pictureBox1_Click(sender, e);
            if (textBox5.Text=="")
                MessageBox.Show("Please Select a Container or OU", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            
            //else if (listView2.Items.Count == 0)
            //    MessageBox.Show("Please Select at least one Group", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView3.Items.Count <= 1 && (textBox1.Text.Equals("") && textBox4.Text.Equals("")))
                MessageBox.Show("Please create atleast one User", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView3.CheckedItems.Count == 0 || (listView3.CheckedItems.Count == 0 && listView3.Items[0].Checked == true))
                MessageBox.Show("Please Select atleast one User", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                addUser();
        } // Add Button

            // AddSingleUser Button 
        private void button6_Click(object sender, EventArgs e)
        {
            //addSingleUserForm = new AddMultipleComputers();
            //addSingleUserForm.FormClosed += AddSingleUserFormClosed;
            //addSingleUserForm.ShowDialog();
        } // AddSingleUser Button 

	
            // AddMultipleUser Button 
        private void button3_Click(object sender, EventArgs e)
        {
            addMultipleUserForm = new AddMultipleUserForm();
            addMultipleUserForm.FormClosed += AddMultipleUserFormClosed;
            addMultipleUserForm.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            String user, pass, fullName, desc;
            if (textBox1.Text.Equals(""))
                //MessageBox.Show("User name is a mandatory field", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Please enter the username", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBox4.Text.Equals(""))
                //MessageBox.Show("Password is a mandatory field", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Please enter the password", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBox2.Text.Equals(""))
                //MessageBox.Show("Password is a mandatory field", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Please enter the UPN", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                user = textBox1.Text;
                fullName = textBox2.Text;
                desc = textBox3.Text;
                pass = textBox4.Text;
                ListViewItem item = new ListViewItem(user);
                item.SubItems.Add(fullName);
                item.SubItems.Add(desc);
                item.SubItems.Add(pass);
                listView3.Items.Add(item);
                item.Checked = true;

                
                textBox2.Text="";
                textBox3.Text="";
                textBox4.Text="";
                textBox1.Text = "";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView3.CheckedItems[1];
            item.BeginEdit();
            //item.EnsureVisible();
        }

        ListViewItem.ListViewSubItem SelectedLSI;
        
        private void HideTextEditor()
        {
            
            TxtEdit.Visible = false;
            if (SelectedLSI != null)
            {
                
                SelectedLSI.Text = TxtEdit.Text;
            }
            
            SelectedLSI = null;       
            //TxtEdit.Text = "hari";
            
        }

        private void listView3_MouseUp(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("mouse up");
            ListViewHitTestInfo i = listView3.HitTest(e.X, e.Y);
            if (i.SubItem == null)
                return;
            SelectedLSI = i.SubItem;           
            

            int border = 0;
            switch (listView3.BorderStyle)
            {
                case BorderStyle.FixedSingle:
                    border = 1;
                    break;
                case BorderStyle.Fixed3D:
                    border = 2;
                    break;
            }

            int CellWidth = SelectedLSI.Bounds.Width;
            int CellHeight = SelectedLSI.Bounds.Height;
            int CellLeft = border + listView3.Left + i.SubItem.Bounds.Left;
            int CellTop = listView3.Top + i.SubItem.Bounds.Top;
            // First Column
            if (i.SubItem == i.Item.SubItems[0])
                CellWidth = listView3.Columns[0].Width;
            if (e.X > 15)
            {
                TxtEdit.Location = new Point(CellLeft + 10, CellTop + 75);
                TxtEdit.Size = new Size(124, CellHeight);
                TxtEdit.Visible = true;
                TxtEdit.BringToFront();
                TxtEdit.Text = i.SubItem.Text;
                
                TxtEdit.Select();
                TxtEdit.SelectAll();
            }
        }

        private void listView3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X > 15)
            {
                //MessageBox.Show("Firing mousedown");
                //HideTextEditor();
            }
            
        }
        private void listView3_Scroll(object sender, EventArgs e)
        {
            
            HideTextEditor();
        }

        private void TxtEdit_Leave(object sender, EventArgs e)
        {
            //MessageBox.Show(TxtEdit.Text);
            HideTextEditor();
        }

        private void TxtEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                
                HideTextEditor();
            }
        }

        private void listView3_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView3.Columns[e.ColumnIndex].Width;
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

                using (var headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular))
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
                sf.Alignment = StringAlignment.Near;

                using (var headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular))
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

                using (var headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Regular))
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

        private void button7_Click(object sender, EventArgs e)
        {
            String cn="";
            cn = textBox5.Text;
            ContainerTree cs = new ContainerTree();
            cs.userName = userName;
            cs.password = password;
            cs.partition = partition;
            cs.port = port;
            cs.dc = dc;
            cs.path = path;
            cs.defaultUser = defaultUser;
            //cs.moveuserflag = 0;
            //cs.defaultUser = defaultUser;
            cs.ShowDialog();
            //cnitem = cs.cnitem;
            String containerDN = cs.dnofcn;
            if (containerDN != "")
            {
                textBox5.Text = containerDN;
            }
            else
            {
                textBox5.Text = cn;
            }
            //listView1.Items.Clear();
            //if(cnitem.Count!=0)
            //{
            //    listView1.Items.Add((ListViewItem)cnitem[0].Clone());
            //}
        }
        public void PropForm1_FormClosed(object sender, EventArgs e)
        {
            closeflag = 1;
        }
        private void button8_Click(object sender, EventArgs e)
        {
            //try
            //{
                label3.Text = "Loading ... Please wait ...";
                label3.Refresh();
               
                MemberSelectionForm mf = new MemberSelectionForm();
                mf.userName = userName;
                mf.password = password;
                mf.partition = partition;
                mf.port = port;
                mf.dc = dc;
                mf.path = path;
                mf.defaultUser = defaultUser;
                mf.FormClosed += new FormClosedEventHandler(PropForm1_FormClosed);

                ValueCollection.Clear();
                foreach (ListViewItem vi in listView2.Items)
                {
                    ValueCollection.Add(vi.Text);
                }
                //mf.ValueCollection = ValueCollection;
                mf.ValueCollection = ValueCollection;

                mf.ShowDialog();
                flag = mf.flag;
                if (flag == 1)
                {
                    closeflag = 0;
                }
                if (closeflag == 0)
                {
                    grpselected = mf.groupCollection;
                    listView2.Items.Clear();
                //}
                    label3.Text = "";
                    label3.Refresh();
                    if (grpselected.Count < 1)
                    {

                        //MessageBox.Show("You have not selected any group");
                    }
                    else
                    {
                        listView2.Items.Clear();
                        foreach (ListViewItem lvi in grpselected)
                        {
                            listView2.Items.Add((ListViewItem)lvi.Clone());
                            lvi.Checked = true;
                        }

                    }
                }
            //}
            //catch (NullReferenceException exx)
            //{
            //    MessageBox.Show("yo");
            //    MessageBox.Show("You have not selected any group");
            //}

        }

       
        private void button4_Click_1(object sender, EventArgs e)
        {
            foreach(ListViewItem v in listView2.SelectedItems)
            {
                listView2.Items.Remove(v);
            }
        }
        
    }
}
