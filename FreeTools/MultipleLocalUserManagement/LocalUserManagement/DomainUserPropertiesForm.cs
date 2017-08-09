using System;
using System.DirectoryServices;

//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;

namespace LocalUserManagement
{
    public partial class DomainUserPropertiesForm : Form
    {
        public String user;
        public String computer;
        public String userName;
        public String password;
        public bool defaultUser = false;
        public bool is_exc = false;
        public System.Collections.Generic.List<String> groupList = new System.Collections.Generic.List<string>();

        public DomainUserPropertiesForm()
        {
            InitializeComponent();
        }

        private void DomainUserPropertiesForm_Load(object sender, EventArgs e)
        {
            this.Left = 350;
            this.Top = 150;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            DirectoryEntry compEntry;
            String adsPath;

            // Finding Groups
            try
            {
                adsPath = "WinNT://" + computer + ",computer";
                if( !defaultUser )
                    compEntry = new DirectoryEntry(adsPath, userName, password);
                else
                    compEntry = new DirectoryEntry(adsPath);

                DirectoryEntries groupEntries = compEntry.Children;

                foreach (DirectoryEntry group in groupEntries)
                {
                    if (group.SchemaClassName.Equals("Group"))
                    {
                        comboBox1.Items.Add(group.Name);
                        groupList.Add(group.Name);
                    }
                } // foreach

                // User Entries
                DirectoryEntry userEntry = new DirectoryEntry("WinNT://" + user);
                if (userEntry.SchemaClassName.Equals("Group"))
                {
                    MessageBox.Show("This selection is not an User.\nPlease Select an User for its Properties", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

                button1.Enabled = false; // Apply Button

                bool temp = true;
                DirectoryEntry admGroup, user_groups;
                object members;

                try
                {
                    for (int i = 0; i < groupList.Count; i++)
                    {
                        admGroup = new DirectoryEntry("WinNT://" + computer + "/" + groupList[i]);
                        members = admGroup.Invoke("Members", null);
                        foreach (object member in (System.Collections.IEnumerable)members)
                        {
                            user_groups = new DirectoryEntry(member);
                            if (userEntry.Path.Equals(user_groups.Path))
                            {
                                temp = false;
                                if (groupList[i].Equals("Power Users"))
                                {
                                    radioButton1.Checked = true;
                                    comboBox1.Enabled = false;
                                }
                                else if (groupList[i].Equals("Users"))
                                {
                                    radioButton2.Checked = true;
                                    comboBox1.Enabled = false;
                                }
                                else
                                {
                                    radioButton3.Checked = true;
                                    comboBox1.Enabled = true;
                                    comboBox1.SelectedItem = groupList[i];
                                }
                                button1.Enabled = false; // Apply Button in tab-2
                                break;
                            }
                        } // foreach
                        if (temp == false)
                            break;
                    } // outer "for" loop
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in Getting Group of the User", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            } // try
            catch (Exception ex)
            {
                
                MessageBox.Show("Unspecified Error", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                comboBox1.Enabled = true;
            else
                comboBox1.Enabled = false;

            button1.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                comboBox1.Enabled = true;
            else
                comboBox1.Enabled = false;

            button1.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                comboBox1.Enabled = true;
            else
                comboBox1.Enabled = false;

            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked==true && comboBox1.Text.Equals(""))
            {
                MessageBox.Show("Please Select any one of Group", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            /////////////////////////////////////////////// User is being Removed
            try
            {
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(15);
                DirectoryEntry computerEntry;
                if( !defaultUser )
                    computerEntry = new DirectoryEntry("WinNT://" + computer + ",computer", userName, password);
                else
                    computerEntry = new DirectoryEntry("WinNT://" + computer + ",computer");

                DirectoryEntry admGroup, user_groups;
                object members;
                DirectoryEntry userEntry = new DirectoryEntry("WinNT://" + user);

                for (int i = 0; i < groupList.Count; i++)
                {
                    admGroup = new DirectoryEntry("WinNT://" + computer + "/" + groupList[i] + ",Group");
                    members = admGroup.Invoke("Members", null);
                    foreach (object member in (System.Collections.IEnumerable)members)
                    {
                        user_groups = new DirectoryEntry(member);
                        if (userEntry.Path.Equals(user_groups.Path))
                        {
                            admGroup.Invoke("Remove", userEntry.Path);
                            admGroup.CommitChanges();
                        } //if
                    } // foreach
                } // outer "for" loop
                ///////////////////////////////////////////////

                // Adding into Group

                String adsPath = "WinNT://" + computer + ",computer";
                if (radioButton1.Checked == true)
                {
                    userEntry = new DirectoryEntry("WinNT://" + user);
                    DirectoryEntry groupEntries = computerEntry.Children.Find("Power Users", "Group");
                    groupEntries.Invoke("Add", userEntry.Path);
                    groupEntries.CommitChanges();
                    MessageBox.Show("User is added in \"Power Users\" Group", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else if (radioButton2.Checked == true)
                {
                    userEntry = new DirectoryEntry("WinNT://" + user);
                    DirectoryEntry groupEntries = computerEntry.Children.Find("Users", "Group");
                    groupEntries.Invoke("Add", userEntry.Path);
                    groupEntries.CommitChanges();
                    MessageBox.Show("User is added in \"Users\" Group", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    if (comboBox1.Text.Equals(""))
                    {
                        MessageBox.Show("Please Select any one of Group", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        userEntry = new DirectoryEntry("WinNT://" + user);
                        DirectoryEntry groupEntries = computerEntry.Children.Find((String)comboBox1.SelectedItem, "Group");
                        groupEntries.Invoke("Add", userEntry.Path);
                        groupEntries.CommitChanges();
                        MessageBox.Show("User is added in \"" + (String)comboBox1.SelectedItem + "\" Group", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);                       
                    }
                } // else
              
                this.Close();
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                try
                {
                    throw ex.InnerException;
                }
                catch (System.UnauthorizedAccessException ex1)
                {
                    MessageBox.Show("You dont have permission to modify User's details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    is_exc = true;
                }
                catch (Exception ex1)
                {
                    MessageBox.Show("Unspecified Error" , " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The specified account name is already a member of the local group."))
                    MessageBox.Show("This User is already belongs to that specified group", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Unspecified Error" , " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);

                this.Close();
            }
        }
    }
}
