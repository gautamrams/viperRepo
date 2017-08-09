using System;
using System.DirectoryServices;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace LocalUserManagement
{
   
    public partial class AddForm : Form
    {
        public String userName;
        public String password;
        public bool rightUser = false;
        public bool defaultUser = false;
        public int isAdded = 0;
        public bool is_exception = false ; 
        public System.Collections.Generic.List<String> computerList = new System.Collections.Generic.List<string>();
        //public System.Collections.ArrayList userList=new System.Collections.ArrayList();
        AddMultipleComputers addSingleUserForm;
        AddMultipleUserForm addMultipleUserForm;
        public AddForm()
        {
            InitializeComponent();
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            this.Left = 200;
            this.Top = 70;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            // For Computers ListView
            // For Select All Purpose in Computers
            CheckBox chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "SelectAllComputers";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllComputers;
            listView1.Controls.Add(chkBox);

            Dictionary<String, int> uniqueStore = new Dictionary<String, int>();
            List<String> finalList = new List<String>();
            computerList = computerList.ConvertAll(d => d.ToUpper());
            foreach (string currValue in computerList)
            {
                if (!uniqueStore.ContainsKey(currValue))
                {
                    uniqueStore.Add(currValue, 0);
                    finalList.Add(currValue);
                }
            }

            for (int i = 0; i < finalList.Count; i++)
            {
                ListViewItem item = new ListViewItem(finalList[i]);
                listView1.Items.Add(item);
            }
            if(listView1.Items.Count != 0)
            chkBox.Checked = true;

            // For Groups ListView
            chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "SelectAllGroups";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllGroups;
            listView2.Controls.Add(chkBox);

            listView2.Items.Add("Administrators");
            listView2.Items.Add("Backup Operators");
            listView2.Items.Add("Guests");
            listView2.Items.Add("Network Configuration Operators");
            listView2.Items.Add("Remote Desktop Users");
            listView2.Items.Add("Replicator");
            listView2.Items.Add("Users");
            listView2.Items.Add("HelpServicesGroup");

            listView2.Items[6].Checked = true; // "Users" Group

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
        }
        
        public void AddSingleUserFormClosed(object sender, EventArgs e)
        {
            if (addSingleUserForm.rightUser == true)
            {
                ListViewItem item = new ListViewItem(addSingleUserForm.user);
                item.SubItems.Add(addSingleUserForm.fullName);
                item.SubItems.Add(addSingleUserForm.desc);
                item.SubItems.Add(addSingleUserForm.pass);
                listView3.Items.Add(item);

                CheckBox cb = (CheckBox)listView3.Controls["SelectAllUsers"];
                cb.Checked = false;
                cb.Checked = true;
            }
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
                System.IO.StreamReader myFile = new System.IO.StreamReader(addMultipleUserForm.filePath);
                try
                {
                    int i = 0, j = 0;
                    String myString = myFile.ReadToEnd();
                    ListViewItem item = new ListViewItem();
                    if (isValidFile(myString) == false)
                        MessageBox.Show("Invalid header in CSV file.\nPlease provide the following attributes as its header: \n \tUser Name,Full Name,Description,Password", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                } // Outer else

                catch (Exception)
                {
                    MessageBox.Show("File does not Exist"," Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        public void SelectAllComputers(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)listView1.Controls["SelectAllComputers"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;
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
            ListView.CheckedListViewItemCollection computerCollection = listView1.CheckedItems;
            ListView.CheckedListViewItemCollection groupCollection = listView2.CheckedItems;
            ListView.CheckedListViewItemCollection userCollection = listView3.CheckedItems;
            

            for (int com = 0; com < computerCollection.Count; com++)
            {
                ListViewItem computer = computerCollection[com];
                String computerName = computer.Text;
                    try
                    {
                        String adsPath = "WinNT://" + computerName + ",computer";
                        DirectoryEntry computerEntry;
                        if (!defaultUser)
                            computerEntry = new DirectoryEntry(adsPath, userName, password);
                        else
                            computerEntry = new DirectoryEntry(adsPath);


                        for (int us = 0; us < userCollection.Count; us++)
                        {
                            
                            ListViewItem user = userCollection[us];
                            String username = user.Text;
                            if (!username.Equals(""))
                            {
                                String groupName = "";
                                
                                try
                                {
                                    DirectoryEntry userEntry = computerEntry.Children.Add(username, "User");
                                    userEntry.InvokeSet("fullName", user.SubItems[1].Text);
                                    userEntry.InvokeSet("description", user.SubItems[2].Text);
                                    userEntry.Invoke("SetPassword", user.SubItems[3].Text);
                                    userEntry.CommitChanges();
                                    

                                    for (int gro = 0; gro < groupCollection.Count; gro++)
                                    {
                                        ListViewItem group = groupCollection[gro];
                                        groupName = group.Text;

                                        DirectoryEntry groupEntries = computerEntry.Children.Find(groupName);
                                        groupEntries.Invoke("Add", userEntry.Path);
                                        groupEntries.CommitChanges();
                                    }
                                    isAdded = 1;
                                    
                                }
                                catch (System.UnauthorizedAccessException ex)
                                {
                                    isAdded = 2;
                                    MessageBox.Show("You dont have sufficient permission to add user in \"" + computerName + "\" ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    is_exception = true;
                                }
                                catch (Exception ex)
                                {
                                    isAdded = 2;
                                    if (ex.Message.Contains("The password does not meet the password policy requirements."))
                                        MessageBox.Show("The password does not meet the password policy requirements for user '" + (String)username + "'", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (ex.Message.Contains("The account already exists."))
                                        MessageBox.Show("\"" + (String)username + "\" already exists in \"" + computerName + "\" Computer\n", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else if (ex.Message.Contains("An unknown directory object was requested"))
                                        MessageBox.Show("Error occurred while including \"" + (String)username + "\" at \"" + groupName + "\" Group in \"" + computerName + "\" computer", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    else
                                        MessageBox.Show("Unspecified Error occurred while adding \"" + (String)username + "\" User in \"" + computerName + "\" ", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                
                            }
                            
                        }// "For" loop for UserList.
                    }
                    catch (Exception ex)
                    {
                        isAdded = 2;
                        MessageBox.Show("Unspecified Error in Getting Connection to \"" + computerName + "\" Computer\n", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (isAdded == 1)
                    {
                        MessageBox.Show("User(s) Added Successfully in '" + computerName +"'");
                        //isAdded = 0;
                    }
                    
                } // "For" loop for ComputerCollection

            if ( isAdded == 0 || isAdded == 1 )
            {
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(13);
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
            if (listView1.CheckedItems.Count == 0)
                MessageBox.Show("Please Select at least one Computer", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView2.CheckedItems.Count == 0)
                MessageBox.Show("Please Select at least one Group", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView3.Items.Count <= 1 && (textBox1.Text.Equals("") && textBox4.Text.Equals("")))
                MessageBox.Show("Please Create at least one User", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView3.CheckedItems.Count == 0 || (listView3.CheckedItems.Count == 0 && listView3.Items[0].Checked == true))
                MessageBox.Show("Please Select at least one User", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
                addUser();
        } // Add Button

            // AddSingleUser Button 
        private void button6_Click(object sender, EventArgs e)
        {
            addSingleUserForm = new AddMultipleComputers();
            addSingleUserForm.FormClosed += AddSingleUserFormClosed;
            addSingleUserForm.ShowDialog();
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
                MessageBox.Show("Please Enter User Name", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBox4.Text.Equals(""))
                //MessageBox.Show("Password is a mandatory field", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Please Enter Password", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        }

        ListViewItem.ListViewSubItem SelectedLSI;
        
        private void HideTextEditor()
        {
            TxtEdit.Visible = false;
            if (SelectedLSI != null)
                SelectedLSI.Text = TxtEdit.Text;
            SelectedLSI = null;
            TxtEdit.Text = "";
        }

        private void listView3_MouseUp(object sender, MouseEventArgs e)
        {
            ListViewHitTestInfo i = listView3.HitTest(e.X, e.Y);
            SelectedLSI = i.SubItem;
            if (SelectedLSI == null)
                return;

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
            HideTextEditor();
        }
        private void listView3_Scroll(object sender, EventArgs e)
        {
            HideTextEditor();
        }

        private void TxtEdit_Leave(object sender, EventArgs e)
        {
            HideTextEditor();
        }

        private void TxtEdit_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            HideTextEditor();
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
                sf.Alignment = StringAlignment.Center;

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
    }
}
