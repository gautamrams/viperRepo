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
using CheckComboBoxTest;
namespace LocalUserManagement
{
    public partial class Form1 : Form
    {
        int ind = 0;
        AddMultipleComputers addMultipleComputers = new AddMultipleComputers();
        ResetPasswordForm RPForm = new ResetPasswordForm();
        String DomainName;
        String UserName;
        String Password;
        String localcomputerName;
        bool defaultUser = true;
        bool is_import = false;
        string uripath = null, path = null;
        System.Collections.Generic.List<String> dcList = new System.Collections.Generic.List<string>();
        System.Collections.Generic.List<String> AllComputersList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> StartList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> AllComputersLocationList = new System.Collections.Generic.List<String>();
        System.Collections.ArrayList UserList = new System.Collections.ArrayList();
        System.Collections.ArrayList GroupList = new System.Collections.ArrayList();
        List<String> finalList = new List<String>();
        int distinguishedname_position = -1;
        
        //        System.Collections.ArrayList GroupArrayList = new System.Collections.ArrayList();

        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
        }
        public static Form1 _Form1;
      
        public void update(string message)
        {
                Constants.computerList.Clear();
                addMultipleComputers.FormClosed += AddMultipleComputersClosed;
                addMultipleComputers.ShowDialog();
        }
        public void recolour()
        {
            for (int item = 0; item < listView2.Items.Count; ++item)
            {
                var items = listView2.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243,243,243) : Color.White;
            }
          /*  int i=0;
            foreach (ListViewItem item in listView2.Items)
            {
                if (item.SubItems[3].Text.Equals("Enabled"))
                {
                    if (i % 2 != 0)
                        item.ImageIndex = 2;
                    else
                        item.ImageIndex = 0;
                }
                else
                {
                    if (i % 2 != 0)
                        item.ImageIndex = 3;
                    else
                        item.ImageIndex = 1;                
                }
                i++;
            
            } */
            
        }
        public bool IsActive(DirectoryEntry de)
        {
            if (de.NativeGuid == null) return false;

            int flags = (int)de.Properties["userAccountControl"].Value;

            return !Convert.ToBoolean(flags & 0x0002);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = 150;
            this.Top = 40;
            Constants.computerList.Clear();
            Constants.checkedComputerList.Clear();
            String domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            localcomputerName = Environment.MachineName;
            DomainName = domainName;
            
            int i = 0;
            listView2.SmallImageList = imageList1;
            listView2.DrawSubItem += new DrawListViewSubItemEventHandler(listview2_DrawSubItem);
            UserName = "User Name";
            Password = "Password";
            button3.Enabled = false;
            StartList.Clear();
            finalList.Clear();
            StartList.Add(localcomputerName);
            label22.Text = "Loading Computers ...";
            uripath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = new Uri(uripath).LocalPath;
            try
            {
                
                
                Domain domain;
                domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domainName));

                foreach (System.DirectoryServices.ActiveDirectory.DomainController dc in domain.DomainControllers)
                {
                    int index = dc.Name.IndexOf('.');
                    String dcName = dc.Name.Substring(0, index);
                    dcList.Add(dcName);
                }
                if (new FileInfo(path + "/conf/save.txt").Length != 0)
                {
                    string result = File.ReadAllText(path + "/conf/save.txt");
                    foreach (string a in result.Split(','))
                    {
                        string temp = a.TrimEnd(' ', '\n');
                        if (temp.Length > 1 && temp != localcomputerName)
                        {
                          //ccbox.Items.Add(new CCBoxItem(temp, 0));
                            StartList.Add(temp);
                            i++;
                        }
                    }
                }
                   String adsPath = "LDAP://" + domainName;
                    DirectoryEntry domainEntry;
                    domainEntry = new DirectoryEntry(adsPath);
                    DirectorySearcher mySearcher = new DirectorySearcher(domainEntry);
                    mySearcher.PageSize = 0;
                    mySearcher.Filter = "(ObjectCategory=computer)";
                    domainEntry.Children.SchemaFilter.Add("computer");
                    foreach (SearchResult result in mySearcher.FindAll())
                    {

                        String name = result.GetDirectoryEntry().Name;
                        String addName = name.Substring(name.IndexOf('=') + 1);
                        bool canDisplay = false;
                        int j;
                        for (j = 0; j < dcList.Count; j++)
                        {
                            if (addName.ToLower().Equals(dcList[j].ToLower()))
                                break;
                        }
                        if (j == dcList.Count)
                            canDisplay = true;
                        if (i < 4 && canDisplay == true)
                        {
                            StartList.Add(addName);
                            i++;
                        }
                        else if ((i - 1) > 4)
                            break;
                    } 
                    Dictionary<String, int> uniqueStore = new Dictionary<String, int>();
                    
                    StartList = StartList.ConvertAll(d => d.ToUpper());
                    foreach (string currValue in StartList)
                    {
                        if (!uniqueStore.ContainsKey(currValue))
                        {
                            uniqueStore.Add(currValue, 0);
                            finalList.Add(currValue);
                        }
                    }
                    for (int ind = 0; ind < finalList.Count; ind ++ )
                        ccbox.Items.Add(new CCBoxItem(finalList[ind], 0));
                ccbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ccbox_ItemCheck);
                ccbox.MaxDropDownItems = 6;
                ccbox.DisplayMember = "Name";
                ccbox.ValueSeparator = ", ";
                ccbox.SetItemChecked(0, true);
            
                listView2.Items.Clear();
                CheckBox chkBox = new CheckBox();
                chkBox.Checked = false;
                chkBox.Name = "ToSelectAll";
                chkBox.Size = new Size(13, 13);
                chkBox.Location = new Point(5, 2);
                chkBox.CheckedChanged += SelectAllUsers;
                listView2.Controls.Add(chkBox);
                try
                   {
                       String ads = "WinNT://" + localcomputerName + ",computer";
                       DirectoryEntry computerEntries;
                       computerEntries = new DirectoryEntry(ads);
                       DirectoryEntries userEntries = computerEntries.Children;
                       ArrayList tempArrayList = new ArrayList();
                       foreach (DirectoryEntry user in userEntries)
                       {
                           
                           if (user.SchemaClassName.Equals("User"))
                           {
                              
                               ListViewItem item = new ListViewItem();
                               item.Text = user.Name;
                               item.Tag = user.Properties["description"].Value.ToString();
                               if(user.Properties["fullname"].Value.ToString().Length == 1)
                               item.SubItems.Add(user.Properties["fullname"].Value.ToString());
                               else
                                   item.SubItems.Add("-");
                              // item.SubItems.Add(user.Properties["description"].Value.ToString());
                               if ((((int)user.Properties["UserFlags"].Value) & 2) <= 0)
                               {
                                   item.SubItems.Add("Enabled");
                                  
                                       item.ImageIndex = 0;
                                 
                               }
                               else
                               {
                                   item.SubItems.Add("Disabled");
                                
                                       item.ImageIndex = 1;
                                 
                               }
                               item.SubItems.Add(localcomputerName);
                               tempArrayList.Add(item);
                               listView2.Items.Add(item);
                           }
                       } // foreach
                       label22.Text = "Loading local users of " + localcomputerName + " ...";
                       Constants.computerList.Add(localcomputerName);
                       Constants.checkedComputerList.Add(localcomputerName);
                       
                       computerEntries.Dispose();
                       button3_Click(this, e);
                       recolour();
                       label22.Text="";
                      
                   }
                   catch (System.UnauthorizedAccessException ex)
                   {
                       MessageBox.Show("You dont have permission to fetch Users in \"" + Constants.computerList[ind] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      
                     //  label21.Visible = true;
                       
                       label22.Text = "";
                       return;
                       
                   }
                   catch (Exception ex)
                   {
                      MessageBox.Show("Unspecified error occurred while fetching Users in  \"" + Constants.computerList[ind] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      label22.Text = "";
                   }
                // for loop - getting User Details
               
               }   // try
                 
            
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show("  The network path was not found ");
                label22.Text = "";
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                label22.Text = "";
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Unspecified Error");
                label22.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure . Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label22.Text = "";
                }
                else
                {
                    MessageBox.Show("Unspecified Error");
                    label22.Text = "";
                }
            }
            
            addMultipleComputers.FormClosed += new FormClosedEventHandler(AddMultipleComputersClosed);

        }


        // To Select All Items in LIst View Items
        private void SelectAllComputers(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;

        }
       
        // Get-Users Button
        private void button3_Click(object sender, EventArgs e)
        {
            Domain domain;
            listView2.Items.Clear();
            
            button3.Enabled = false;
            label22.Text = "Loading User Details .. Please Wait ..";
            if ((UserName.Equals("User Name") && !Password.Equals("Password")) || (!UserName.Equals("User Name") && Password.Equals("Password")))
            {
                MessageBox.Show("Please Enter User Name & Password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button3.Enabled = true;
                return;
            }
            else if (!UserName.Equals("User Name") && !Password.Equals("Password"))
            {
                defaultUser = false;
                addMultipleComputers.defaultUser = defaultUser;
                addMultipleComputers.UserName = UserName;
                addMultipleComputers.Password = Password;
            }
            try
            {
                if (!defaultUser)
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName, UserName, Password));
                else
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName));
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
                MessageBox.Show("Unspecified Error");
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
                    MessageBox.Show("Unspecified Error");
                    button3.Enabled = true;
                    return;
                }
            }
        
               listView2.Items.Clear();
               UserList.Clear();
               ccbox_DropDownClosed(sender, e);       
               if (Constants.computerList.Count == 0)
               {
                   MessageBox.Show("Please select at least one computer","Error Message");
                   button3.Enabled = true;
                   return;
               }
               /*
                System.IO.File.WriteAllText(@"conf/ppm.txt",string.Empty);        
                ColumnHeader header = listView1.Columns[1];
                header.Text = "    Connectivity ";

                for (int i = 0; i < listView1.Items.Count; i++)
                    listView1.Items[i].SubItems[1].Text = "-----";

                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    Constants.checkedComputerList.Add(listView1.CheckedItems[i].Text);
                listView1.Refresh();
                label22.Text = "Fetching users please wait ...";
                */
                Constants.computerList = Constants.computerList.ConvertAll(d => d.ToUpper());
                Dictionary<String, int> uniqueStore = new Dictionary<String, int>();
                List<String> finalList = new List<String>();
               
                    foreach (string currValue in Constants.computerList)
                    {
                        if (!uniqueStore.ContainsKey(currValue))
                        {
                            uniqueStore.Add(currValue, 0);
                            finalList.Add(currValue);
                        }
                    }
                ccbox.DataSource = null;
                backgroundWorker1.RunWorkerAsync(finalList);
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(12);
               
           // } // else
        } // Get-Users Button

        public void LocalUsersDetails()
        {
            listView2.Items.Clear();
            CheckBox chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "ToSelectAll";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllUsers;
            listView2.Controls.Add(chkBox);

            refreshAll();

        }  // LocalUserDetails function

        public void refreshAll()
        {
            UserList.Clear();
            GroupList.Clear();

            progressBar2.Visible = true;
            progressBar2.Location = new Point(171, 52);
            progressBar2.Refresh();
            label18.Location = new Point(143, 25);
            label18.Visible = true;
            label18.Refresh();

            groupBox7.Visible = false;


            button11.Location = new Point(420, 52);
            button11.Visible = true;
            button11.Refresh();

            button4.Enabled = false;
            button4.Refresh();
            button5.Enabled = false;
            button5.Refresh();
            button6.Enabled = false;
            button6.Refresh();
            button8.Enabled = false;
            button8.Refresh();
            button9.Enabled = false;
            button9.Refresh();
            button10.Enabled = false;
            button10.Refresh();

            backgroundWorker2.RunWorkerAsync(Constants.computerList);

        }

        // Displaying All Users in All Computers
        public void displayAllUsers(int computerNumber)
        {
            //for (int i = 0; i < UserList.Count; i++)
            // {
            button20.Enabled = false;
            try
            {
                ArrayList temp = (ArrayList)UserList[computerNumber];
                for (int j = 0; j < temp.Count; j++)
                {
                    ListViewItem item = new ListViewItem();
                    item = (ListViewItem)temp[j];
                    listView2.Items.Add(item);
                }
                recolour();
            }
            catch(Exception eab){ }
            button20.Enabled = true;
            // }
        }  // displayAllUsers function

        public void displaySingleComputerUsers(int index)
        {
            ArrayList temp = (ArrayList)UserList[index - 1];
            for (int j = 0; j < temp.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                item = (ListViewItem)temp[j];
                listView2.Items.Add(item);
            }
            recolour();
        } // displaySingleComputerUsers function

        // To Select All Users 
        private void SelectAllUsers(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = false;

        }


        // Back Button Local User Details Group Box
        private void button7_Click(object sender, EventArgs e)
        {

        }
        // Computer Selection
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                comboBox3.Items.Clear();
                comboBox3.Items.Add("All Groups");
                comboBox3.SelectedItem = "All Groups";

                if (comboBox2.Text.Equals("All Computers"))
                {
                    comboBox3.Items.Add("Administrators");
                    comboBox3.Items.Add("Backup Operators");
                    comboBox3.Items.Add("Guests");
                    comboBox3.Items.Add("Network Configuration Operators");
                    comboBox3.Items.Add("Remote Desktop Users");
                    comboBox3.Items.Add("Replicator");
                    comboBox3.Items.Add("Users");
                    comboBox3.Items.Add("HelpServicesGroup");
                }
                else
                {
                    try
                    {
                        String ads = "WinNT://" + comboBox2.Text + ",computer";
                        DirectoryEntry computerEntries;
                        if (!defaultUser)
                            computerEntries = new DirectoryEntry(ads, UserName, Password);
                        else
                            computerEntries = new DirectoryEntry(ads);
                        DirectoryEntries groupEntries = computerEntries.Children;

                        foreach (DirectoryEntry group in groupEntries)
                        {
                            if (group.SchemaClassName.Equals("Group"))
                            {
                                comboBox3.Items.Add(group.Name);
                            }
                        }
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have permission to fetch Group details in \"" + comboBox2.Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unspecified error occurred while fetching Group Details in  \"" + comboBox2.Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }  // else

                CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll"];
                cb.Checked = true;
                cb.Checked = false;
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }  // Computer Selection

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (progressBar2.Visible == true)
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            if (progressBar2.Visible == true)
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        // Group Selection
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            try
            {

                if (comboBox3.Text.Equals("All Groups"))
                {
                    if (comboBox2.Text.Equals("All Computers"))
                    {
                        for (int i = 0; i < Constants.computerList.Count; i++)
                            displayAllUsers(i);
                    }
                    else
                        displaySingleComputerUsers(comboBox2.SelectedIndex);
                }
                else
                {
                    groupBox7.Visible = false;

                    //progressBar2.Location = new Point(143, 30);
                    progressBar2.Visible = true;
                    progressBar2.Refresh();
                    //label18.Location = new Point(73, 10);
                    label18.Visible = true;
                    label18.Refresh();
                    button7.Location = new Point(420, 52);
                    button7.Visible = true;
                    button7.Refresh();

                    button4.Enabled = false;
                    button4.Refresh();
                    button5.Enabled = false;
                    button5.Refresh();
                    button6.Enabled = false;
                    button6.Refresh();
                    button8.Enabled = false;
                    button8.Refresh();
                    button9.Enabled = false;
                    button9.Refresh();
                    button10.Enabled = false;
                    button10.Refresh();

                    List<String> tempList = new List<String>();

                    if (comboBox2.Text.Equals("All Computers"))
                    {
                        for (int i = 0; i < Constants.computerList.Count; i++)
                        {
                            String names = Constants.computerList[i] + "," + comboBox3.Text;
                            tempList.Add(names);
                        }
                    }
                    else
                    {
                        String names = comboBox2.Text + "," + comboBox3.Text;
                        tempList.Add(names);
                    }
                    displayGroupUsers(tempList);
                }

                CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll"];
                cb.Checked = true;
                cb.Checked = false;
            }
            catch (System.InvalidOperationException ex)
            {
                MessageBox.Show("Connection failed,try to get the local users some time later.", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection failed,try to get the local users some time later.", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        } // Group Selection

        // displayGroupUsers
        public void displayGroupUsers(List<String> tempList)
        {
            backgroundWorker3.RunWorkerAsync(tempList);
        }  //displayGroupUsers

        // Properties Button
        private void button10_Click(object sender, EventArgs e)
        {
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listView2.CheckedItems.Count == 0)
                MessageBox.Show("Please select at least one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView2.CheckedItems.Count > 1)
                MessageBox.Show("Please select only one user", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                ListViewItem item = listView2.CheckedItems[0];
                try
                {
                    ///////// Just Checking whether it is a Domain User or not /////
                    String adsPath = "WinNT://" + item.SubItems[2].Text + ",computer";
                    DirectoryEntry compEntry;
                    if (!defaultUser)
                        compEntry = new DirectoryEntry(adsPath, UserName, Password);
                    else
                        compEntry = new DirectoryEntry(adsPath);

                    DirectoryEntry userEntry = compEntry.Children.Find(item.Text, "User");
                    userEntry.Dispose();
                    compEntry.Dispose();
                    /////////////////////////////////

                    PropertiesForm PropForm = new PropertiesForm();
                    PropForm.user = item.Text;
                    PropForm.computer = item.SubItems[2].Text;
                    PropForm.userName = UserName;
                    PropForm.password = Password;
                    PropForm.defaultUser = defaultUser;
                    PropForm.ShowDialog();
                }
                catch (Exception ex)
                {
                    DomainUserPropertiesForm domainForm = new DomainUserPropertiesForm();
                    domainForm.user = item.Text;
                    domainForm.computer = item.SubItems[2].Text;
                    domainForm.userName = UserName;
                    domainForm.password = Password;
                    domainForm.defaultUser = defaultUser;
                    domainForm.ShowDialog();
                }
                refreshListView();
            } // else
        }  // Properties Button

        // Delete Button
        private void button4_Click(object sender, EventArgs e)
        {
            /*
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ListView.CheckedListViewItemCollection collection = listView2.CheckedItems;

            if (collection.Count < 1)
                MessageBox.Show("Please select atleast one user", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DialogResult result;

                result = MessageBox.Show("Are you sure you want to delete ?", "Confirm User Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;

                String adsPathRemove;
                int temp = 0;

                for (int i = 0; i < collection.Count; i++)
                {
                    ListViewItem item = collection[i];
                    try
                    {
                        adsPathRemove = "WinNT://" + item.SubItems[2].Text + ",computer";
                        DirectoryEntry compEntries;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(adsPathRemove, UserName, Password);
                        else
                            compEntries = new DirectoryEntry(adsPathRemove);
                        DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        compEntries.Children.Remove(myChildEntry);
                        myChildEntry.Dispose();
                        compEntries.Dispose();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have permission to delete the Users in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Domain Users can not be deleted. Local Users only can be deleted. ", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                } // for loop
                if (temp == 1)
                {
                    refreshAll();
                    //                    refreshListView();
                    MessageBox.Show("Users are Removed Succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    cl.executeDll(14);
                }
            } // else*/
                AddMultipleComputers addMultipleComputers = new AddMultipleComputers();
                Constants.computerList.Clear();
                addMultipleComputers.FormClosed += AddMultipleComputersClosed;
                addMultipleComputers.ShowDialog();
        }// Delete Button.    

        public void refreshListView()
        {
            // Refreshing ListView
            listView2.Items.Clear();
            if (comboBox3.Text.Equals("All Groups"))
            {
                if (comboBox2.Text.Equals("All Computers"))
                {
                    for (int i = 0; i < Constants.computerList.Count; i++)
                        displayAllUsers(i);
                }
                else
                    displaySingleComputerUsers(comboBox2.SelectedIndex);
            }
            else
            {

                progressBar2.Visible = true;
                progressBar2.Refresh();
                label18.Visible = true;
                label18.Refresh();
                List<String> tempList = new List<String>();

                if (comboBox2.Text.Equals("All Computers"))
                {
                    for (int i = 0; i < Constants.computerList.Count; i++)
                    {
                        String names = Constants.computerList[i] + "," + comboBox3.Text;
                        tempList.Add(names);
                    }
                }
                else
                {
                    String names = comboBox2.Text + "," + comboBox3.Text;
                    tempList.Add(names);
                }
                displayGroupUsers(tempList);
            }
        }//refresh ListView function

        // Reset Password Button
        private void button9_Click(object sender, EventArgs e)
        {
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listView2.CheckedItems.Count == 0)
                MessageBox.Show("Please select at least one user", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView2.CheckedItems.Count > 1)
                MessageBox.Show("Please select only one user", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                ListViewItem item = listView2.CheckedItems[0];

                ResetPasswordForm RPForm = new ResetPasswordForm();
                RPForm.userName = UserName;
             //   RPForm.password = Password;
                RPForm.user = item.Text;
                RPForm.path = "WinNT://" + item.SubItems[2].Text + ",computer";
                RPForm.defaultUser = defaultUser;
                RPForm.ShowDialog();
            }
        }  // Reset Password Button

        // Enable Button
        private void button6_Click(object sender, EventArgs e)
        {
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                        Path = "WinNT://" + item.SubItems[2].Text + ",computer";
                        DirectoryEntry compEntries;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(Path, UserName, Password);
                        else
                            compEntries = new DirectoryEntry(Path);
                        DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        if ((((int)myChildEntry.Properties["UserFlags"].Value) & 2) > 0)
                            myChildEntry.Properties["UserFlags"].Value = ((int)myChildEntry.Properties["UserFlags"].Value) ^ 2;
                        myChildEntry.CommitChanges();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have permission to enable the Users in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("       Domain Users cant be Enabled.\nFor Local Users only, we can have this operation", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }  // for loop
                if (temp == 1)
                {
                    //refreshAll();
                    // refreshListView();
                    MessageBox.Show("Users are Enabled Succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                } // if
            } //else
        }  // Enable Button

        // Disable Button
        private void button5_Click(object sender, EventArgs e)
        {
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                        Path = "WinNT://" + item.SubItems[2].Text + ",computer";
                        DirectoryEntry compEntries;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(Path, UserName, Password);
                        else
                            compEntries = new DirectoryEntry(Path);
                        DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        if ((((int)myChildEntry.Properties["UserFlags"].Value) & 2) <= 0)
                            myChildEntry.Properties["UserFlags"].Value = ((int)myChildEntry.Properties["UserFlags"].Value) ^ 2;
                        myChildEntry.CommitChanges();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have permission to enable the Users in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("       Domain Users cant be Disabled.\nFor Local Users only, we can have this operation", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }  // for loop
                if (temp == 1)
                {
                    // refreshAll();
                    //  refreshListView();
                    MessageBox.Show("Users are Disabled Succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                } // if
            } //else

        }
        // Disable Button

        // Add Button
        private void button8_Click(object sender, EventArgs e)
        {
            
            
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                AddForm addForm = new AddForm();
                addForm.userName = UserName;
                addForm.password = Password;
                addForm.defaultUser = defaultUser;
                for (int i = 0; i < Constants.computerList.Count; i++)
                {
                    bool canDisplay = false;
                    int j;
                    for (j = 0; j < dcList.Count; j++)
                    {
                        if (Constants.computerList[i].ToLower().Equals(dcList[j].ToLower()))
                            break;
                    }
                    if (j == dcList.Count)
                        canDisplay = true;

                    if (canDisplay == true)
                    {
                        addForm.computerList.Add(Constants.computerList[i]);
                    }
                }
                addForm.ShowDialog();
                if (addForm.isAdded == 1 || addForm.isAdded == 2)
                    refreshAll();
                if(addForm.is_exception==true)
                {
                    
                    //label21.Visible = true;
                    
                }
                addForm.isAdded = 0;
                //refreshListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unspecified Error", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } // Add Button

        private void Label14_TextChanged(object sender, EventArgs e)
        {
            label14.Refresh();
        }

        private void Button3_VisibleChanged(object sender, EventArgs e)
        {
            button3.Refresh();
        }

        private void Button12_VisibleChanged(object sender, EventArgs e)
        {
            //   button12.Refresh();
        }

        private void ListView1_SubItemChanged(object sender, DrawListViewSubItemEventArgs e)
        {
            //    MessageBox.Show("SubItem Changed");
        }

        private void ListView1_ItemChanged(object sender, DrawListViewItemEventArgs e)
        {
            //   MessageBox.Show("Item changed");
        }


        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
           

            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;

            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;

            List<String> ComboList = new List<String>();

            Constants.computerList.Clear();

            for (ind = 0; ind < coll.Count; ind++)
            {

                
                tempBGWorker.ReportProgress(ind, "Checking");

                if (backgroundWorker1.CancellationPending == true)
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
                        
                        //System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        ///////////////////////////////////////// DirectoryEntry Checking
                        try
                        {
                            String ads = "WinNT://" + coll[ind] + ",computer";
                            DirectoryEntry computerEntries;
                            if (!defaultUser)
                                computerEntries = new DirectoryEntry(ads, UserName, Password);
                            else
                                computerEntries = new DirectoryEntry(ads);

                            DirectoryEntries userEntries = computerEntries.Children;

                            foreach (DirectoryEntry user in userEntries)
                            {
                            }

                            tempBGWorker.ReportProgress(ind, "Accessible");
                            Constants.computerList.Add(coll[ind]);
                            //    System.Threading.Thread.Sleep(100);
                        }
                        catch (Exception ex)
                        {
                            tempBGWorker.ReportProgress(ind, "Not Accessible");
                            
                            //      System.Threading.Thread.Sleep(100);
                        } // catch
                        ///////////////////////////////////////// DirectoryEntry Checking
                    } // else
                } //try
                catch (Exception ex)
                {
                    tempBGWorker.ReportProgress(ind, "Not Accessible");
                    
                    //System.Threading.Thread.Sleep(100);
                }
                /////////////////////////////////////////// PingChecking COmputers
            } // for loop
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
           try
           {
                if (e.UserState.GetType().ToString().Equals("System.String"))
                {
                    if (e.UserState.ToString().Equals("Checking"))
                    {
                        //label22.Text = "Checking Accessibility of \"" + finalList[e.ProgressPercentage] + "\"..";
                        label22.Refresh();
                    }
                    if (e.UserState.ToString().Equals("Not Accessible"))
                    {
                        label22.Text = "Unable to connect to \"" + finalList[e.ProgressPercentage] + "\"..";
                        label22.Refresh();
                    }
                }
           }
            catch (Exception et){}
            
        }

        // Stop Button
        private void button12_Click_1(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            label14.Text = "Process is about to stop...";
            label14.Refresh();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                progressBar1.Visible = false;
                progressBar1.Refresh();
                label14.Visible = false;
                label14.Refresh();
                button3.Visible = true;
                button3.Enabled = true;
            }
            if (Constants.computerList.Count < 1)
            {
                progressBar1.Visible = false;
                progressBar1.Refresh();
                label14.Visible = false;
                label14.Refresh();
                button3.Visible = true;
                button12.Visible = false;
                button3.Enabled = true;
                label22.Text = "";
                MessageBox.Show("Selected Computers are not accessible now", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
             }
            else
            {
                progressBar1.Visible = false;
                progressBar1.Refresh();
                label14.Visible = false;
                label14.Refresh();

             //   groupBox1.Visible = false;
                comboBox1.Visible = false;
              // textBox1.Visible = false;
                label12.Visible = false;
              //textBox2.Visible = false;
                label13.Visible = false;
                label8.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                groupBox3.Visible = false;
                groupBox8.Visible = false;

                groupBox4.Size = new System.Drawing.Size(677, 562);
                groupBox4.Location = new System.Drawing.Point(18, 49);
                label22.Text = "";
                LocalUsersDetails();
            }
            button3.Enabled = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (progressBar1.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while...", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            listView1.Items.Clear();
            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";
            listView1.Refresh();

            if (StartList.Count == 0)
            {
                label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label17.Refresh();
                MessageBox.Show("Please Enter Domain Details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text.Equals("") || textBox3.Text.Equals("Computer Name"))
            {
                for (int i = 0; i < StartList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(StartList[i]);
                    if (distinguishedname_position != -1)
                    item.SubItems.Add(AllComputersLocationList[i]);
                    else
                        item.SubItems.Add("-");
                    listView1.Items.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < StartList.Count; i++)
                {
                    if (StartList[i].Contains(textBox3.Text) || StartList[i].ToUpper().Contains(textBox3.Text.ToUpper()))
                    {
                        ListViewItem item = new ListViewItem(StartList[i]);
                        if (distinguishedname_position == -1 && is_import )
                            item.SubItems.Add("-");
                        else
                            item.SubItems.Add(AllComputersLocationList[i]);
                        listView1.Items.Add(item);
                    }
                }
            }
            label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
            label17.Refresh();

            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
            cb.Checked = true;
            cb.Checked = false;

            listView1.Refresh();

        }  // Search Button in Computer Details

        private void textBox3_Click(object sender, EventArgs e)
        {/*
            textBox3.Text = "";
            textBox3.ForeColor = Color.Black;
          */
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals(""))
            {
                textBox3.ForeColor = Color.Gray;
                textBox3.Text = "Computer Name";
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.ForeColor == Color.Gray)
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals(""))
            {
                comboBox1.ForeColor = Color.Black;
                comboBox1.Text = "Domain Name";
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.ForeColor == Color.Gray)
            {
                comboBox1.Text = "";
                comboBox1.ForeColor = Color.Black;
            }
        }

       

      
        

        // Back Button
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            //pictureBox2.Visible = false;
            //pictureBox4.Visible = true;
           // pictureBox3.Visible = true;
           // pictureBox6.Visible = false;

            button3.Visible = true;
            button12.Visible = false;

            groupBox4.Visible = false;

              groupBox8.Visible = true;
          //  groupBox8.Location = new System.Drawing.Point(24, 38);
          //  groupBox8.Size = new System.Drawing.Size(665, 104);

            comboBox1.Visible = true;
           
            label12.Visible = true;
            
            label13.Visible = true;
            label8.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            groupBox3.Visible = true;

            groupBox3.Visible = true;
            groupBox3.Location = new Point(92, 245);
            groupBox3.Size = new Size(518, 366);
        }

        // Front Button
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (progressBar1.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //pictureBox3.Visible = false;
          //  pictureBox6.Visible = true;
          //  pictureBox2.Visible = true;
          //  pictureBox4.Visible = false;

            progressBar1.Visible = false;
            progressBar1.Refresh();
            label14.Visible = false;
            label14.Refresh();

            //groupBox1.Visible = false;
            comboBox1.Visible = false;
            
            label12.Visible = false;
            
            label13.Visible = false;
            label8.Visible = false;
            button1.Visible = false;
            button2.Visible = false;
            groupBox3.Visible = false;
            groupBox8.Visible = false;

            groupBox4.Size = new System.Drawing.Size(677, 562);
            groupBox4.Location = new System.Drawing.Point(18, 49);
            groupBox4.Visible = true;

        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                
            }
        }

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
               
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                pictureBox1_Click(sender, e);
            }
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;
            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;
            
            // Getting User Details
            for (ind = 0; ind < coll.Count; ind++)
            {
                tempBGWorker.ReportProgress(ind, "CheckingUsers");

                if (backgroundWorker2.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                bool canDisplay = false;
                int j;
                for (j = 0; j < dcList.Count; j++)
                {
                    if (coll[ind].ToLower().Equals(dcList[j].ToLower()))
                        break;
                }
                if (j == dcList.Count)
                    canDisplay = true;
                if (canDisplay)
                {
                    try
                    {
                        String ads = "WinNT://" + coll[ind] + ",computer";
                        DirectoryEntry computerEntries;
                        if (!defaultUser)
                            computerEntries = new DirectoryEntry(ads, UserName, Password);
                        else
                            computerEntries = new DirectoryEntry(ads);
                        DirectoryEntries userEntries = computerEntries.Children;

                        ArrayList tempArrayList = new ArrayList();
                        foreach (DirectoryEntry user in userEntries)
                        {
                            if (user.SchemaClassName.Equals("User"))
                            {
                                ListViewItem item = new ListViewItem();
                                item.Text = user.Name;
                                item.Tag = user.Properties["description"].Value.ToString();
                                if (user.Properties["fullname"].Value.ToString().Length == 1)
                                item.SubItems.Add(user.Properties["fullname"].Value.ToString());
                                else
                                item.SubItems.Add("-");
                                item.SubItems.Add(Constants.computerList[ind]);
                                if ((((int)user.Properties["UserFlags"].Value) & 2) <= 0)
                                {
                                    item.SubItems.Add("Enabled");
                                    item.ImageIndex = 0;
                                }
                                else
                                {
                                    item.SubItems.Add("Disabled");
                                    item.ImageIndex = 1;
                                }
                                    tempArrayList.Add(item);
                            }
                        } // foreach
                        tempBGWorker.ReportProgress(ind, tempArrayList);
                        //System.Threading.Thread.Sleep(1000);

                        computerEntries.Dispose();
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        //MessageBox.Show("You dont have permission to fetch Users in \"" + computerList[ind] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        tempBGWorker.ReportProgress(ind, "AccessDenied");
                        //computerList.RemoveAt(ind);
                        //ind--;
                    }
                    catch (Exception ex)
                    {
                        tempBGWorker.ReportProgress(ind, "UnspecifiedError");
                        //MessageBox.Show("Unspecified error occurred while fetching Users in  \"" + computerList[ind] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //computerList.RemoveAt(ind);
                        //ind--;
                    }
                }
            } // for loop - getting User Details
        }

        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //MessageBox.Show("" + e.UserState.GetType().ToString());
           // progressBar2.Value++;
           // progressBar2.PerformStep();
           // progressBar2.Refresh();

           // if (progressBar2.Value > 150)
           //     progressBar2.Value = 5;

            if (e.UserState.GetType().ToString().Equals("System.String"))
            {
                //  MessageBox.Show("Equal to String -- ");
                if (e.UserState.ToString().Equals("CheckingUsers"))
                {
                    //  MessageBox.Show("Checking user");
                    label22.Text = "Loading User Details from \"" + Constants.computerList[e.ProgressPercentage] + "\" ";
                }
                else if (e.UserState.ToString().Equals("AccessDenied"))
                {
                    //  MessageBox.Show("Access Denied user");
                    Constants.computerList.RemoveAt(e.ProgressPercentage);
                    label22.Text = "You dont have permission to fetch Users in \"" + Constants.computerList[e.ProgressPercentage] + "\" ";
                    //ind--;
                    //  MessageBox.Show("You dont have permission to fetch Users in \"" + computerList[e.ProgressPercentage] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.UserState.ToString().Equals("UnspecifiedError"))
                {
                    //  MessageBox.Show("Error in user");
                    Constants.computerList.RemoveAt(e.ProgressPercentage);
                    label22.Text="Unspecified error occurred while fetching Users in  \"" + Constants.computerList[e.ProgressPercentage] + "\" ";
                    //ind--;
                    //    MessageBox.Show("Unspecified error occurred while fetching Users in  \"" + computerList[e.ProgressPercentage] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else if (e.UserState.ToString().Equals("CheckingGroups"))
                {
                    //  MessageBox.Show("Checking group");
                    label22.Text = "Loading Group Details from \"" + Constants.computerList[e.ProgressPercentage] + "\" ";
                }
                else if (e.UserState.ToString().Equals("AccessDeniedGroup"))
                {
                    //  MessageBox.Show("Access denied in group");
                    Constants.computerList.RemoveAt(e.ProgressPercentage);
                    UserList.RemoveAt(e.ProgressPercentage);
                    //  ind--;
                    //  MessageBox.Show("You dont have permission to fetch Users in \"" + computerList[e.ProgressPercentage] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (e.UserState.ToString().Equals("UnspecifiedErrorGroup"))
                {
                    //   MessageBox.Show("Error in group");
                    Constants.computerList.RemoveAt(e.ProgressPercentage);
                    UserList.RemoveAt(e.ProgressPercentage);
                    //  ind--;
                    //  MessageBox.Show("Unspecified error occurred while fetching Users in  \"" + computerList[e.ProgressPercentage] + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (e.UserState.GetType().ToString().Equals("System.Collections.ArrayList"))
            {
                // MessageBox.Show("Equal to arraylist");
                UserList.Add((ArrayList)e.UserState);
                displayAllUsers(e.ProgressPercentage);
            }
            else //if( e.UserState.GetType().ToString().Equals("System.Collections.Generic.List<String>" ) )
            {
                //  MessageBox.Show("equal to list");
                GroupList.Add((List<String>)e.UserState);
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            label22.Text = "";
            if (e.Error != null)
            {
                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();

                // MessageBox.Show("Error occurred while stopping the process\n" + e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (ind != Constants.computerList.Count)
                {
                    for (int i = ind; i < Constants.computerList.Count; )
                        Constants.computerList.RemoveAt(i);
                }

                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();


                groupBox7.Location = new Point(5, 20);
                groupBox7.Size = new Size(670, 65);
                groupBox7.Visible = true;

                button11.Visible = false;
                button11.Refresh();

                button4.Enabled = true;
                button4.Refresh();
                button5.Enabled = true;
                button5.Refresh();
                button6.Enabled = true;
                button6.Refresh();
                button8.Enabled = true;
                button8.Refresh();
                button9.Enabled = true;
                button9.Refresh();
                button10.Enabled = true;
                button10.Refresh();


                // displayAllUsers();

                comboBox2.Items.Clear();
                comboBox3.Items.Clear();
                comboBox2.Items.Add("All Computers");
                comboBox3.Items.Add("All Groups");
                comboBox2.SelectedItem = "All Computers";
                comboBox3.SelectedItem = "All Groups";

                for (int i = 0; i < Constants.computerList.Count; i++)
                    comboBox2.Items.Add(Constants.computerList[i]);

                refreshListView();

            }
            
        }

        private void backgroundWorker3_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            List<String> coll = (List<String>)e.Argument;

            for (int i = 0; i < coll.Count; i++)
            {
                if (backgroundWorker3.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                String names = (String)coll[i];
                String computerName = names.Substring(0, names.IndexOf(','));
                String groupName = names.Substring(names.IndexOf(',') + 1, names.Length - computerName.Length - 1);

                backgroundWorker3.ReportProgress(2, names);
                // System.Threading.Thread.Sleep(100);

                try
                {
                    DirectoryEntry groupEntry = new DirectoryEntry("WinNT://" + computerName + "/" + groupName + ",group");

                    foreach (object member in (System.Collections.IEnumerable)groupEntry.Invoke("Members"))
                    {
                        DirectoryEntry memberEntry = new DirectoryEntry(member);

                        if (memberEntry.Path.Contains(computerName))
                        {
                            ListViewItem item = new ListViewItem(memberEntry.Name);
                            if (memberEntry.Properties["fullname"].Value.ToString().Length == 1)
                                item.SubItems.Add(memberEntry.Properties["fullname"].Value.ToString());
                            else
                                item.SubItems.Add("-");
                            item.SubItems.Add(computerName);
                            item.Tag = memberEntry.Properties["description"].Value.ToString();
                            if ((((int)memberEntry.Properties["UserFlags"].Value) & 2) <= 0)
                            {
                                item.SubItems.Add("Enabled");
                                item.ImageIndex = 0;
                            }
                            else
                            {
                                item.SubItems.Add("Disabled");
                                item.ImageIndex = 1;
                            }
                                item.SubItems.Add(computerName);

                            backgroundWorker3.ReportProgress(0, item);
                            //      System.Threading.Thread.Sleep(300);
                            //  listView2.Items.Add(item);
                        }
                            /*
                        else
                        {
                            
                           
                            String name = memberEntry.Path.Substring(memberEntry.Path.IndexOf('/') + 2);
                            ListViewItem item = new ListViewItem(name);
                            item.SubItems.Add("");
                            item.SubItems.Add(computerName);
                            if (memberEntry.SchemaClassName.Equals("Group"))
                                item.SubItems.Add("Domain Group");
                            else if (memberEntry.SchemaClassName.Equals("User"))
                                item.SubItems.Add("Domain User");

                            
                            //item.ImageIndex = 0;

                            backgroundWorker3.ReportProgress(0, item);
                            //System.Threading.Thread.Sleep(300);

                            // listView2.Items.Add(item);
                        //} 
                        }*/
                        memberEntry.Dispose();
                              
                    } // foreach
                    groupEntry.Dispose();
                }
                catch (Exception ex)
                {
                    backgroundWorker3.ReportProgress(1, names);
                    //System.Threading.Thread.Sleep(300);
                }
            }

        }

        private void backgroundWorker3_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            //MessageBox.Show("e.ProgressPercentage : "+e.ProgressPercentage+"e.UserState.GetType().ToString() : " + e.UserState.GetType().ToString());
          //  progressBar2.Value++;
          //  progressBar2.PerformStep();
         //  progressBar2.Refresh();

        //    if (progressBar2.Value > 150)
        //        progressBar2.Value = 5;
            if (e.UserState.GetType().ToString().Equals("System.String"))
            {
                if (e.ProgressPercentage == 2)
                {
                    String names = (String)e.UserState;
                    String computerName = names.Substring(0, names.IndexOf(','));
                    label18.Text = " Loading Group Members from \"" + computerName + "\"";
                }
                else if (e.ProgressPercentage == 1)
                {
                    String names = (String)e.UserState;
                    String computerName = names.Substring(0, names.IndexOf(','));
                    //MessageBox.Show("Unspecified Error occurred while fetching group Users in \"" + computerName + "\"", " Information ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                // MessageBox.Show("Entered");
                ListViewItem item = (ListViewItem)e.UserState;
                listView2.Items.Add(item);
                listView2.Refresh();
                recolour();
                //     MessageBox.Show("added");
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Completed");
            progressBar2.Visible = false;
            progressBar2.Refresh();
            label18.Visible = false;
            label18.Refresh();
            button7.Visible = false;
            button7.Refresh();

            button4.Enabled = true;
            button4.Refresh();
            button5.Enabled = true;
            button5.Refresh();
            button6.Enabled = true;
            button6.Refresh();
            button8.Enabled = true;
            button8.Refresh();
            button9.Enabled = true;
            button9.Refresh();
            button10.Enabled = true;
            button10.Refresh();
            button20.Enabled = true;
            groupBox7.Visible = true;

            if (listView2.Items.Count == 0)
            {
                MessageBox.Show("No users available in this group", " Information ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void label18_TextChanged(object sender, EventArgs e)
        {
            label18.Refresh();
        }

        // Stop Button in Local User Details
        private void button7_Click_1(object sender, EventArgs e)
        {
            backgroundWorker3.CancelAsync();
            label18.Text = "Process is about to stop...";
            label18.Refresh();

        }

        // Stop Button - IN starting Stage
        private void button11_Click(object sender, EventArgs e)
        {
            backgroundWorker2.CancelAsync();
            label18.Text = "Process is about to stop...";
            label18.Refresh();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.admanagerplus.com");
        }

        private void backgroundWorker4_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                // Getting DC list
                System.Collections.Generic.List<String> dcList = new System.Collections.Generic.List<string>();
                Domain domain;
                if (!defaultUser)
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName, UserName, Password));
                else
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName));
                foreach (System.DirectoryServices.ActiveDirectory.DomainController dc in domain.DomainControllers)
                {
                    if ( backgroundWorker4.CancellationPending == true )
                    {
                        e.Cancel = true;
                        break;
                    }

                    int index = dc.Name.IndexOf('.');
                    String dcName = dc.Name.Substring(0, index);
                    dcList.Add(dcName);
                }

                String adsPath = "LDAP://" + DomainName;
                DirectoryEntry domainEntry;
                backgroundWorker4.ReportProgress(3);
                if (!defaultUser)
                {
                    domainEntry = new DirectoryEntry(adsPath, UserName, Password);
                }
                else
                {
                    domainEntry = new DirectoryEntry(adsPath);
                }

                backgroundWorker4.ReportProgress(3);
                DirectorySearcher mySearcher = new DirectorySearcher(domainEntry);
                mySearcher.Filter = "(ObjectCategory=computer)";
                mySearcher.PageSize = 1000;
                domainEntry.Children.SchemaFilter.Add("computer");
                foreach (SearchResult result in mySearcher.FindAll())
                {
                    if (backgroundWorker4.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    backgroundWorker4.ReportProgress(3);
                    String name = result.GetDirectoryEntry().Name;
                    String addName = name.Substring(name.IndexOf('=') + 1);
                    ListViewItem item = new ListViewItem(addName);
                    
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
                    {
                        String path = result.GetDirectoryEntry().Path;
                        String addPath = path.Substring(path.IndexOf(',') + 1);
                        item.SubItems.Add(addPath);
                        StartList.Add(addName);
                        AllComputersLocationList.Add(addPath);
                        backgroundWorker4.ReportProgress(1, item);
                        System.Threading.Thread.Sleep(1);
                    }

                }

                domainEntry.Dispose();
            }   // try
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                backgroundWorker4.ReportProgress(2, "  The network path was not found ");
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                backgroundWorker4.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                backgroundWorker4.ReportProgress(2, "Unspecified Error");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    backgroundWorker4.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
                }
                else
                {
                    backgroundWorker4.ReportProgress(2, "Unspecified Error");
                }
            }

        }

        private void backgroundWorker4_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
           // progressBar3.Value += 3;
          //  progressBar3.Refresh();

           // if (progressBar3.Value > 180)
          //      progressBar3.Value = 5;

            if (e.ProgressPercentage == 1)
            {
                ListViewItem item = (ListViewItem)e.UserState;
                listView1.Items.Add(item);
                label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label17.Refresh();
            }
            else if (e.ProgressPercentage == 2)
            {
                MessageBox.Show((String)e.UserState, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar3.Visible = false;
            progressBar3.Refresh();
            button13.Visible = false;
            button13.Refresh();
            label20.Visible = false;
            label20.Refresh();

            if (listView1.Items.Count > 0)
            {
                // For Select All Purpose
                textBox3.Visible = true;
                pictureBox1.Visible = true;
                textBox3_Leave(sender, e);

                CheckBox chkBox = new CheckBox();
                chkBox.Checked = false;
                chkBox.Name = "ToSelectAllComputers";
                chkBox.Size = new System.Drawing.Size(13, 13);
                chkBox.Location = new System.Drawing.Point(5, 2);
                chkBox.CheckedChanged += SelectAllComputers;
                listView1.Controls.Add(chkBox);

                CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
                cb.Checked = true;
                cb.Checked = false;

                listView1.Sorting = SortOrder.Ascending;
                listView1.Refresh();

                label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label17.Refresh();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            textBox3.Text = "";
            textBox3_Leave(sender, e);

            listView1.Items.Clear();
            StartList.Clear();
            AllComputersLocationList.Clear();
            Constants.computerList.Clear();

            label17.Text = "Total Number of Computers  : " + 0;

            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";

            //pictureBox6.Visible = true;
            //pictureBox4.Visible = true;
           // pictureBox2.Visible = false;
           // pictureBox3.Visible = false;

        }

        private void button13_Click(object sender, EventArgs e)
        {
            backgroundWorker4.CancelAsync();
            label20.Text = "Process is about to stop...";
            label20.Refresh();
        }
      

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

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
        void listview2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
           // if ((e.ItemState & ListViewItemStates.Focused)==0)
          //      {
         //               e.Graphics.FillRectangle(SystemBrushes.Highlight,e.Bounds);
         //               e.Graphics.DrawString(e.Item.Text, listView2.Font,SystemBrushes.HighlightText, e.Bounds);
        // }
            //  else
            //   {
                e.DrawBackground();
                e.DrawText();
          //     }
   }
        public void displayFilteredUsers(object sender) 
        {
            button20.Enabled = false;
            List<String> tempList = new List<String>();
            ToolStripItem obj = (ToolStripItem)sender;
            for (int i = 0; i < Constants.computerList.Count; i++)
            {
                String names = Constants.computerList[i] + "," + obj.Text;
                tempList.Add(names);
            }
            displayGroupUsers(tempList);
        }
        private void administratorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void allGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            try
            {
                for (int i = 0; i < Constants.computerList.Count; i++)
                    displayAllUsers(i);
            }
            catch(Exception eaaa){}
        }

        private void backupOperatorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void guestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void networkConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void remoteDesktopUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void replicatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
        }

        private void helpServicesGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            displayFilteredUsers(sender);
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

     

        private void ccbox_DropDownClosed(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("Items checked: ");
            foreach (CCBoxItem item in ccbox.CheckedItems)
            {
                Constants.computerList.Add(item.Name);
                sb.Append(item.Name).Append(ccbox.ValueSeparator);
            }
            //Constants.computerList.RemoveAt(Constants.computerList.Count-1);
            sb.Remove(sb.Length - ccbox.ValueSeparator.Length, ccbox.ValueSeparator.Length);            
        }

      

        private void ccbox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Constants.computerList.Clear();
            CCBoxItem item = ccbox.Items[e.Index] as CCBoxItem;
            
        }

        private void ccbox_DropDown(object sender, EventArgs e)
        {
            
            
        }

        private void label21_Click(object sender, EventArgs e)
        {
            SaveForm saveform = new SaveForm();
            saveform.ShowDialog();
            defaultUser = saveform.defaultuser;
            UserName = saveform.username;
            Password = saveform.password;
           
        }

        
        public void AddMultipleComputersClosed(object sender, EventArgs e)
        {
            if (addMultipleComputers.fav_set)
            {
                ccbox.Items.Clear();
                Form1_Load(sender, e);
                addMultipleComputers.fav_set = false;
            }
        }

        private void ccbox_Click(object sender, EventArgs e)
        {
            
        }
       

        private void listView2_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView2_DrawSubItem_1(object sender, DrawListViewSubItemEventArgs e)
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

        private void buttonLastest1_Click(object sender, EventArgs e)
        {
            SaveForm saveform = new SaveForm();
            saveform.ShowDialog();
            defaultUser = saveform.defaultuser;
            UserName = saveform.username;
            Password = saveform.password;
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

                result = MessageBox.Show("Are you sure you want to delete ?", "Confirm User Delete", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;

                String adsPathRemove;
                int temp = 0;

                for (int i = 0; i < collection.Count; i++)
                {
                    ListViewItem item = collection[i];
                    try
                    {
                        adsPathRemove = "WinNT://" + item.SubItems[2].Text + ",computer";
                        DirectoryEntry compEntries;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(adsPathRemove, UserName, Password);
                        else
                            compEntries = new DirectoryEntry(adsPathRemove);
                        DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        compEntries.Children.Remove(myChildEntry);
                        myChildEntry.Dispose();
                        compEntries.Dispose();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have sufficient permission to delete the users in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //label21.Visible = true;

                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Domain Users can not be deleted. Local Users only can be deleted. ", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                } // for loop
                if (temp == 1)
                {
                    refreshAll();
                    //                    refreshListView();
                    MessageBox.Show("User(s) deleted succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    cl.executeDll(14);
                }
            } // else
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                        Path = "WinNT://" + item.SubItems[2].Text + ",computer";
                        DirectoryEntry compEntries;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(Path, UserName, Password);
                        else
                            compEntries = new DirectoryEntry(Path);
                        DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        if ((((int)myChildEntry.Properties["UserFlags"].Value) & 2) > 0)
                            myChildEntry.Properties["UserFlags"].Value = ((int)myChildEntry.Properties["UserFlags"].Value) ^ 2;
                        myChildEntry.CommitChanges();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have permission to enable the Users in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //label21.Visible = true;

                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("       Domain Users cant be Enabled.\nFor Local Users only, we can have this operation", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }  // for loop
                if (temp == 1)
                {
                    //refreshAll();
                    // refreshListView();
                    MessageBox.Show("User(s) Enabled Succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                } // if
                button3_Click(sender, e);
            } //else
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (progressBar2.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                        Path = "WinNT://" + item.SubItems[2].Text + ",computer";
                        DirectoryEntry compEntries;
                        if (!defaultUser)
                            compEntries = new DirectoryEntry(Path, UserName, Password);
                        else
                            compEntries = new DirectoryEntry(Path);
                        DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                        if ((((int)myChildEntry.Properties["UserFlags"].Value) & 2) <= 0)
                            myChildEntry.Properties["UserFlags"].Value = ((int)myChildEntry.Properties["UserFlags"].Value) ^ 2;
                        myChildEntry.CommitChanges();
                        temp = 1;
                    }
                    catch (System.UnauthorizedAccessException ex)
                    {
                        MessageBox.Show("You don't have permission to disable the Users in \"" + item.SubItems[2].Text + "\"", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        //label21.Visible = true;

                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("       Domain Users cant be Disabled.\nFor Local Users only, we can have this operation", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                }  // for loop
                if (temp == 1)
                {
                    // refreshAll();
                    //  refreshListView();
                    MessageBox.Show("User(s) Disabled Succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                } // if
                button3_Click(sender, e);
            } //else
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
                RPForm.FormClosed += RPFormClosed;
                RPForm.ShowDialog();
            }
         }

        public void RPFormClosed(object sender, EventArgs e)
        {
            if (RPForm.is_change)
            {
                ListView.CheckedListViewItemCollection collection = listView2.CheckedItems;
                               
                  for (int i = 0; i < collection.Count; i++)
                  {
                    ListViewItem item = collection[i];
                    try
                    {
                        
                            DirectoryEntry compEntries;
                            if (!defaultUser)
                                compEntries = new DirectoryEntry("WinNT://" + item.SubItems[2].Text + ",computer", UserName, Password);
                            else
                                compEntries = new DirectoryEntry("WinNT://" + item.SubItems[2].Text + ",computer");

                                DirectoryEntry myChildEntry = compEntries.Children.Find(item.Text, "User");
                      
                        
                        myChildEntry.Invoke("SetPassword", RPForm.password);
                        MessageBox.Show("Password is changed successfully for user " + item.Text + " in " + item.SubItems[2].Text, " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                        cl.executeDll(15);
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
                            MessageBox.Show("You don’t have sufficient permission to reset password for "+item.Text+" in "+item.SubItems[2].Text, " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                 
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unspecified Error", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                    
                
                }
            }
        }

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
                    ///////// Just Checking whether it is a Domain User or not /////
                    String adsPath = "WinNT://" + item.SubItems[2].Text + ",computer";
                    DirectoryEntry compEntry;
                    if (!defaultUser)
                        compEntry = new DirectoryEntry(adsPath, UserName, Password);
                    else
                        compEntry = new DirectoryEntry(adsPath);

                    DirectoryEntry userEntry = compEntry.Children.Find(item.Text, "User");
                    userEntry.Dispose();
                    compEntry.Dispose();
                    /////////////////////////////////

                    PropertiesForm PropForm = new PropertiesForm();
                    PropForm.user = item.Text;
                    PropForm.computer = item.SubItems[2].Text;
                    PropForm.userName = UserName;
                    PropForm.password = Password;
                    PropForm.defaultUser = defaultUser;
                    PropForm.ShowDialog();
                    if (PropForm.is_exc == true)
                    {

                        //label21.Visible = true;


                    }
                }
                catch (Exception ex)
                {
                    DomainUserPropertiesForm domainForm = new DomainUserPropertiesForm();
                    domainForm.user = item.Text;
                    domainForm.computer = item.SubItems[2].Text;
                    domainForm.userName = UserName;
                    domainForm.password = Password;
                    domainForm.defaultUser = defaultUser;
                    domainForm.ShowDialog();
                    if (domainForm.is_exc == true)
                    {

                        //label21.Visible = true;

                    }
                }
            }
        }
    } 
}
