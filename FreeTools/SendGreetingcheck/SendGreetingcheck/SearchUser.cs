using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
namespace SendGreetingcheck
{
    public partial class LastLogonTool : Form
    {
        public LastLogonTool()
        {
            InitializeComponent();
            UserSearchlist.View = View.Details;
            UserSearchlist.GridLines = true;
            UserSearchlist.CheckBoxes = true;
            UserSearchlist.Columns.Add("FirstName", 132, HorizontalAlignment.Left);
            UserSearchlist.Columns.Add("CommmonName", 120, HorizontalAlignment.Left);
            UserSearchlist.Columns.Add("DistinguishedName", 150, HorizontalAlignment.Left);
            UserSearchlist.Columns.Add("SamAccountName", 150, HorizontalAlignment.Left);
            UserSearchlist.Columns.Add("UserLogonName", 170, HorizontalAlignment.Left);
            this.AcceptButton = Search;
            AddCurrentDomain();
            //DomainNamedcomboBox.Items.Add("admpw2k8.com");
            //DomainNamedcomboBox.Items.Add("admp.com");
            
        }
        
        public void recolour()
        {
            for (int item = 0; item < UserSearchlist.Items.Count; ++item)
            {
                var items = UserSearchlist.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(244, 244, 244) : Color.White;
            }
        }

        public void AddCurrentDomain()
        {
            //ArrayList alDomains = new ArrayList();
            Forest currentForest = Forest.GetCurrentForest();
            DomainCollection myDomains = currentForest.Domains;
            foreach (Domain objDomain in myDomains)
            {
                //  alDomains.Add(objDomain.Name);
                DomainNamedcomboBox.Items.Add(objDomain.Name);
                
            }
            //return alDomains;

        }
        private void Search_Click(object sender, EventArgs e)
        {
            UserSearchlist.Items.Clear();
            SearchUser(txtUserName.Text);
            recolour();
        }
        public void SearchUser(String userName)
        {
            string[] dcArray = DomainNamedcomboBox.Text.Split(new char[] { '.' });
            string resultdc = "";
            int counter = 0;
            txtUserName.Clear();
            foreach (string dc in dcArray)
            {
                resultdc = resultdc + "DC=" + dc;
                counter++;
                if (dcArray.Length != counter)
                    resultdc = resultdc + ",";

            }
            if (DomainNamedcomboBox.Text.Trim().Length == 0)
            {
                MessageBox.Show("Cannot complete search without domain name.!", " Try Again!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DirectoryEntry dir = new DirectoryEntry("LDAP://" + resultdc);
            try
            {
                
                DirectorySearcher searcher = new DirectorySearcher();
                searcher.PageSize = 1000;
                searcher.SearchRoot = dir;

               // searcher.Filter = "(&(&(objectClass=user)(objectClass=person))(|(givenName=" + userName + "*)                    (userPrincipalName=" + userName + "*)(sn=" + userName + "*)(sAMAccountName=" + userName + "*)                (displayName=" + userName + "*)))";
                searcher.Filter = "(&(&(objectClass=user)(objectCategory=person))(|(givenName=" + userName + "*)                    (userPrincipalName=" + userName + "*)(sn=" + userName + "*)(sAMAccountName=" + userName + "*)                (displayName=" + userName + "*)))";


                searcher.SearchScope = SearchScope.Subtree;
                searcher.PropertiesToLoad.Add("cn");
                searcher.PropertiesToLoad.Add("givenName");
                searcher.PropertiesToLoad.Add("displayName");
                searcher.PropertiesToLoad.Add("sAMAccountName");
                searcher.PropertiesToLoad.Add("distinguishedName");
                searcher.PropertiesToLoad.Add("userPrincipalName");
                searcher.Sort = new SortOption("givenName", SortDirection.Ascending);
                SearchResultCollection results = searcher.FindAll();
                if (results.Count > 0)
                {
                    foreach (SearchResult result in results)
                    {
                        if (result.Properties["givenName"].Count > 0)
                            UserSearchlist.Items.Add("" + result.Properties["givenName"][0]);
                        else
                            UserSearchlist.Items.Add("-");

                        UserSearchlist.Items[UserSearchlist.Items.Count - 1].SubItems.Add("" + result.Properties["cn"][0]);
                        UserSearchlist.Items[UserSearchlist.Items.Count - 1].SubItems.Add("" + result.Properties["distinguishedName"][0]);
                        UserSearchlist.Items[UserSearchlist.Items.Count - 1].SubItems.Add("" + result.Properties["sAMAccountName"][0]);

                        if (result.Properties["userPrincipalName"].Count > 0)
                            UserSearchlist.Items[UserSearchlist.Items.Count - 1].SubItems.Add("" + result.Properties["userPrincipalName"][0]);
                        else
                            UserSearchlist.Items[UserSearchlist.Items.Count - 1].SubItems.Add("-");

                    }
                }
                
                else
                    MessageBox.Show("Record Not Found!", " Try Again!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            { MessageBox.Show("Domain can't be contacted!", " Try Again!", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem CurrentItem in UserSearchlist.Items)
                CurrentItem.Checked = true;
        }

        private void DeSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem CurrentItem in UserSearchlist.Items)
                CurrentItem.Checked = false;
        }

        private void GetLastLogonDetails_Click(object sender, EventArgs e)
        {
            int itemCount = UserSearchlist.CheckedItems.Count;
            int flag = 0;
            ListView.CheckedListViewItemCollection checkedItems = UserSearchlist.CheckedItems;
            string[] userSamACNames = new string[checkedItems.Count];
            int i = 0;
            foreach (ListViewItem item in checkedItems)
            {
                userSamACNames[i] = item.SubItems[3].Text;
                flag = 1;
                i++;
            }
            string dcName = DomainNamedcomboBox.Text;
            if (flag == 0)
            {
                if (DomainNamedcomboBox.Text.Length == 0)
                    MessageBox.Show(" Enter Domain Name!", " Try Again!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                   MessageBox.Show(" Select at least one user!", " Try Again!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            ListDC listDC = new ListDC();

            if (checkedItems.Count > 0)
            {
                listDC.getDCNamesSearch(dcName, userSamACNames, checkedItems.Count);
                listDC.Show();
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(11);

            }
        }

        private void DomainNamedcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
   

        private void UserSearchlist_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif",9, FontStyle.Bold))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }

        private void UserSearchlist_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void UserSearchlist_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }
    }

}