using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Collections;

namespace ADLDSManagement
{
    public partial class containerselection : Form
    {
        public String userName;
        public String password;
        public String path;
        public String port;
        public String partition;
        public String dc;
        public ListView.CheckedListViewItemCollection useritem;
        public String present;
        public String past;
        public bool defaultUser = false;
        public int moveuserflag;
        public ListView.CheckedListViewItemCollection cnitem;

        public containerselection()
        {
            InitializeComponent();
        }

        

        private void button3_Click(object sender, EventArgs e)
        {
            cnitem = listView1.CheckedItems;
            this.Close();
        }

        private void containerselection_Load(object sender, EventArgs e)
        {
            if(moveuserflag==0)
            {
                button1.Enabled = false;
            }
            if(moveuserflag == 1)
            {
                button1.Enabled = true;
                button3.Enabled = false;
            }
            try
            {
                button4.Enabled = false;
                present = partition;
                //for listing containers and ou
                
                //------------------------------------------------------------------------------------------------
                DirectoryEntry de1 = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);

                DirectorySearcher ds = new DirectorySearcher(de1);

                ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
                ds.Filter = "(|(objectClass=container)(objectClass=organizationalUnit))";
                ds.PageSize = 100;

                SearchResultCollection results = ds.FindAll();
                
                label3.Text = partition;

                foreach (SearchResult sr in results)
                {
                    var ou = new DirectoryEntry(sr.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);

                    ListViewItem item = new ListViewItem();


                    item.Text = ou.Name;

                    item.SubItems.Add(ou.Properties["distinguishedName"].Value.ToString());
                    listView1.Items.Add(item);
                }

            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show(" The network path was not found ");
                //button3.Enabled = true;
                return;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //button3.Enabled = true;
                return;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Unspecified Error");
                //button3.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //button3.Enabled = true;
                    return;
                }
                else
                {
                    //MessageBox.Show("test UE");
                    MessageBox.Show("Unspecified Error - "+ex.Message);
                    //button3.Enabled = true;
                    return;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            ListView.CheckedListViewItemCollection containerCollection = listView1.CheckedItems;
            try
            {
                ListViewItem cont = containerCollection[0];
                past = present;
                present = cont.SubItems[1].Text;
                String contpath = "LDAP://" + dc + ":" + port + "/" + present;
                DirectoryEntry computerEntry;
                if (!defaultUser)
                    computerEntry = new DirectoryEntry(contpath, userName, password, AuthenticationTypes.Secure);

                else
                    computerEntry = new DirectoryEntry(contpath);


                DirectorySearcher ds = new DirectorySearcher(computerEntry);
                ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
                ds.Filter = "(|(objectClass=container)(objectClass=organizationalUnit))";
                ds.PageSize = 100;

                SearchResultCollection results = ds.FindAll();
                String display = cont.Text;
                label3.Text = display;

                listView1.Items.Clear();
                foreach (SearchResult sr in results)
                {
                    var ou = new DirectoryEntry(sr.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);

                    ListViewItem item = new ListViewItem();


                    item.Text = ou.Name;

                    item.SubItems.Add(ou.Properties["distinguishedName"].Value.ToString());
                    listView1.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("InvalidArgument"))
                {
                    MessageBox.Show("Users not available in this CN/OU. Please go back to previous CN/OU");
                }

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int endflag = 0;
            try
            {
                //ListViewItem cont = containerCollection[0];

                String contpath = "LDAP://" + dc + ":" + port + "/" + past;
                present = past;
                if (past.Trim().Equals(partition.Trim()))
                {
                    past = "";
                    button4.Enabled = false;
                    endflag = 1;
                }
                else
                {
                    if (present.Trim().ToLower().Equals(partition.Trim().ToLower()))
                    {
                        //MessageBox.Show("check");
                        button4.Enabled = false;
                        endflag = 1;
                    }
                    else
                    {
                        int end = present.IndexOf(",");
                        past = present.Remove(0, end + 1);
                        //MessageBox.Show(past);
                    }
                }
                DirectoryEntry computerEntry;
                if (!defaultUser)
                    computerEntry = new DirectoryEntry(contpath, userName, password, AuthenticationTypes.Secure);

                else
                    computerEntry = new DirectoryEntry(contpath);


                DirectorySearcher ds = new DirectorySearcher(computerEntry);
                ds.SearchScope = System.DirectoryServices.SearchScope.OneLevel;
                ds.Filter = "(|(objectClass=container)(objectClass=organizationalUnit))";
                ds.PageSize = 100;

                SearchResultCollection results = ds.FindAll();
                if (endflag == 0)
                {
                    String display = computerEntry.Name;
                    label3.Text = display;
                }
                if (endflag == 1)
                {
                    label3.Text = partition;
                }

                listView1.Items.Clear();
                foreach (SearchResult sr in results)
                {
                    var ou = new DirectoryEntry(sr.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);

                    ListViewItem item = new ListViewItem();


                    item.Text = ou.Name;

                    item.SubItems.Add(ou.Properties["distinguishedName"].Value.ToString());
                    listView1.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            ListView.CheckedListViewItemCollection contcollec = listView1.CheckedItems;
            if (listView1.CheckedItems.Count == 0)
            {
                MessageBox.Show("Select a container");
            }
            if (listView1.CheckedItems.Count > 1)
            {
                MessageBox.Show("Cannot move user to more than one container");
            }
            else
            {
                try
                {
                    String containerpath = "LDAP://" + dc + ":" + port + "/" + contcollec[0].SubItems[1].Text;
                    DirectoryEntry theNewParent = new DirectoryEntry(containerpath, userName, password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                    foreach (ListViewItem useritem1 in useritem)
                    {

                        String userpath = "LDAP://" + dc + ":" + port + "/" + useritem1.SubItems[3].Text;
                        //MessageBox.Show(userpath);
                        DirectoryEntry theObjectToMove = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                        theObjectToMove.MoveTo(theNewParent);
                    }
                    MessageBox.Show("User(s) Moved successfully !");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
