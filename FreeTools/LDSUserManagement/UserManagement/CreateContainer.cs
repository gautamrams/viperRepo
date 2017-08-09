using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;

namespace ADLDSManagement
{
    public partial class CreateContainer : Form
    {
        public String userName;
        public String password;
        public String path;
        public String port;
        public String partition;
        public String dc;
        public String dnofcn;
        public bool defaultUser;
        public int createdflag = 0;
        public int flag = 0;

        public CreateContainer()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createdflag = 0;
            flag = 1;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dnofcn == "")
                {
                    MessageBox.Show("Select a container for creating the OU");
                }
                else
                {
                    
                    path = "LDAP://" + dc + ":" + port + "/" + dnofcn;
                    //MessageBox.Show(path);
                    DirectoryEntry de;
                    if (!defaultUser)
                    {
                        de = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
                    }
                    else
                    {
                        de = new DirectoryEntry(path);
                    }
                    DirectorySearcher ds = new DirectorySearcher(de);
                    ds.Filter = "(|(&(objectCategory=organizationalUnit)(OU=" + textBox1.Text.Trim() + "))((&(objectCategory=container)(cn=" + textBox1.Text.Trim()+"))))";
                    ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
                    SearchResultCollection result = ds.FindAll();
                    if (result.Count > 0)
                    {
                        MessageBox.Show("There is already an OU/Container with the same name");
                        result.Dispose();
                    }
                    else
                    {
                        if (textBox1.Text.Trim().Length > 64)
                        {
                            MessageBox.Show("OU name cannot exceed 64 characters");
                        }
                        else
                        {
                            DirectoryEntry userEntry = de.Children.Add("OU=" + textBox1.Text.Trim().ToString(), "OrganizationalUnit");
                            userEntry.CommitChanges();
                            MessageBox.Show("The OU has been added successfully");
                            createdflag = 1;
                            this.Close();
                        }
                    }

                }
            }
            catch(Exception ex)
            {
                if (ex.Message.Contains("(0x80005000)"))
                {
                    MessageBox.Show("Please select an OU", "ERROR");
                    this.Close();
                }
                if (ex.Message.Contains("naming violation"))
                {
                    MessageBox.Show("Cannot create OU inside a Container", "ERROR");
                    this.Close();
                }
                
            }
        }
    }
}
