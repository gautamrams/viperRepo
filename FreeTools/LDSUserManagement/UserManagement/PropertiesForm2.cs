using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;

namespace ADLDSManagement
{
    public partial class PropertiesForm2 : Form
    {
        public String path;
        public String user;
        public String port;
        public String partition;
        public String dc;
        public String userName;
        public String password;
        public String userpath;
        public int flag;
        public PropertyValueCollection ValueCollection;
        public ListView.CheckedListViewItemCollection groupCollection;

        public bool defaultUser = false;
        public PropertiesForm2()
        {
            InitializeComponent();
        }

        public void SelectAllGroups(object sender, EventArgs e)
        {

            CheckBox cb = (CheckBox)listView1.Controls["SelectAllGroups"];

            if (cb.Checked == true)
            {

                for (int i = 0; i < this.listView1.Items.Count; i++)
                {

                    listView1.Items[i].Checked = true;
                }
            }
            else
            {
                for (int i = 0; i < this.listView1.Items.Count; i++)
                {
                    listView1.Items[i].Checked = false;
                }
            }
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
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PropertiesForm2_Load_1(object sender, EventArgs e)
        {
            
            this.Left = 350;
            this.Top = 150;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            CheckBox chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "SelectAllGroups";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllGroups;
            listView1.Controls.Add(chkBox);
            
            userpath = "LDAP://" + dc + ":" + port + "/" + user;
            
            // Finding Groups
            DirectoryEntry de1;
            if (!defaultUser)
            {
                de1 = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);
            }
            else
            {
                de1 = new DirectoryEntry(path);
            }
            //DirectoryEntry de1 = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);

            DirectorySearcher ds = new DirectorySearcher(de1);

            ds.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            ds.Filter = "(objectClass=group)";
            ds.PageSize = 100;

            ds.PropertiesToLoad.Add("cn");

            ds.PropertyNamesOnly = true;

            SearchResultCollection results = ds.FindAll();

            listView1.Items.Clear();
            foreach (SearchResult sr in results)
            {
                DirectoryEntry user1;
                if (!defaultUser)
                {
                    user1 = new DirectoryEntry(sr.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);
                }
                else
                {
                    user1 = new DirectoryEntry(sr.GetDirectoryEntry().Path);
                }
                //var user1 = new DirectoryEntry(sr.GetDirectoryEntry().Path, userName, password, AuthenticationTypes.Secure);

                ListViewItem item = new ListViewItem();

                String grpstring = user1.Properties["distinguishedName"].Value.ToString().Trim();

                
                String text = user1.Name;
                int length1 = text.Length;
                item.Text = text.Remove(0, 3);
                listView1.Items.Add((ListViewItem)item.Clone());
            }
            if (ValueCollection.Count > 0)
            {
                foreach (String val in ValueCollection)
                {
                    String temp = val.Remove(0, 3);
                    //MessageBox.Show(temp);
                    int start;
                    start = temp.IndexOf(",");

                    String finalstr;
                    finalstr = temp.Substring(0, start);
                    ListViewItem item = listView1.FindItemWithText(finalstr.Trim());
                    if (item == null)
                    {
                        MessageBox.Show("Null");
                    }
                    else
                    {
                        item.Checked = true;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            groupCollection = listView1.CheckedItems;
            
            //PropertiesForm prop1 = new PropertiesForm();
            //prop1.path = path;
            //prop1.dc = dc;
            //prop1.port = port;
            //prop1.partition = partition;
            //prop1.user = user;
            //prop1.userName = userName;
            //prop1.password = password;
            //prop1.defaultUser = defaultUser;
            //prop1.retcollection = groupCollection;
            //prop1.flag = 1;
            //prop1.focusflag = 1;
            this.Close();
            //prop1.ShowDialog();
            
            //this.Dispose();
            //this.Close();

        }
    }
}
