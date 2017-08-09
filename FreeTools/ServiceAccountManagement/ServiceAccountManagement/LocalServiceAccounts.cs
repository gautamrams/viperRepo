using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;


namespace ServiceAccountManagement
{
   
    public partial class LocalServiceAccounts : Form
    {
        System.Collections.Generic.List<String> NameList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> DescriptionList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> TypeList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> MachineList = new System.Collections.Generic.List<String>();
        public LocalServiceAccounts()
        {
            InitializeComponent();
        }
        public void recolour1()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.LightGray : Color.White;
            }
        }
        private void LocalServiceAccounts_Load(object sender, EventArgs e)
        {    
                this.MinimumSize = this.Size;
                this.MaximumSize = this.Size;
                listView1.Items.Clear();
                NameList.Clear();
                DescriptionList.Clear();
                TypeList.Clear();
                MachineList.Clear();
                int i = 0;
                if (!string.IsNullOrEmpty(Form1.errorcomputers))
                {
                    label1.Visible = true;
                    linkLabel1.Visible = true;
                }
                foreach (string str in Form1.finalList)
                {
                ListViewItem item = new ListViewItem();
                item.Text = str;
                NameList.Add(str);
                string str1 = "";
                string domainstr = "";
                int index = str.IndexOf("\\");
                if (index > 0)
                    str1 = str.Substring(0, index);
                index = Form1.DomainName.LastIndexOf(".");
                if (index > 0)
                    domainstr = Form1.DomainName.Substring(0, index);
                if (str1.ToUpper().Equals(domainstr.ToUpper()) || str1.ToUpper().Equals(System.Environment.UserDomainName.ToUpper()))
                {
                    item.SubItems.Add("Domain user/Domain Group");
                    DescriptionList.Add("Domain user/Domain Group");
                    item.SubItems.Add("Domain Account");
                    TypeList.Add("Domain Account");
                }
                else
                {
                    try
                    {
                        String adsPath = "WinNT://" + Form1.computersList[i] + ",computer";
                        DirectoryEntry compEntry = new DirectoryEntry(adsPath);
                        string str2 = str.Substring(str.IndexOf('\\') + 1);
                        try
                        {
                            DirectoryEntry userEntry = compEntry.Children.Find(str2, "User");
                            item.SubItems.Add("" + userEntry.InvokeGet("description"));
                            DescriptionList.Add("" + userEntry.InvokeGet("description"));
                            item.SubItems.Add("Local User");
                            TypeList.Add("Local User");
                        }
                        catch (Exception eq)
                        {
                            try
                            {
                                DirectoryEntry groupEntry = compEntry.Children.Find(str2, "Group");
                                item.SubItems.Add("" + groupEntry.InvokeGet("description"));
                                DescriptionList.Add("" + groupEntry.InvokeGet("description"));
                                item.SubItems.Add("Local Group");
                                TypeList.Add("Local Group");
                            }
                            catch (Exception ex)
                            {
                                item.SubItems.Add("");
                                DescriptionList.Add("");
                                item.SubItems.Add("Local Account");
                                TypeList.Add("Local Account");
                            }

                        }
                       


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                item.SubItems.Add(Form1.computersList[i].ToString());
                MachineList.Add(Form1.computersList[i].ToString());
                listView1.Items.Add(item);
                i++;
            }
                
                recolour1();       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count < 1)
            {
                MessageBox.Show("No data to export","Information",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Title = "Choose file to save to",
                    FileName = "Report.csv",
                    Filter = "CSV (*.csv)|*.csv",
                    FilterIndex = 0,
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                //show the dialog + display the results in a msgbox unless cancelled


                if (sfd.ShowDialog() == DialogResult.OK)
                {

                    string[] headers = listView1.Columns
                               .OfType<ColumnHeader>()
                               .Select(header => header.Text.Trim())
                               .ToArray();

                    string[][] items = listView1.Items
                                .OfType<ListViewItem>()
                                .Select(lvi => lvi.SubItems
                                    .OfType<ListViewItem.ListViewSubItem>()
                                    .Select(si => si.Text).ToArray()).ToArray();

                    string table = string.Join(",", headers) + Environment.NewLine;
                    foreach (string[] a in items)
                    {
                        //a = a_loopVariable;
                        table += string.Join(",", a) + Environment.NewLine;
                    }
                    table = table.TrimEnd('\r', '\n');
                    System.IO.File.WriteAllText(sfd.FileName, table);

                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox1.Text.Equals("Search"))
            {
                listView1.Items.Clear();
                for (int i = 0; i < NameList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(NameList[i]);
                    item.SubItems.Add(DescriptionList[i]);
                    item.SubItems.Add(TypeList[i]);
                    item.SubItems.Add(MachineList[i]);
                    listView1.Items.Add(item);
                }
            }
               

            else
            {
                for (int i = listView1.Items.Count - 1; i >= 0; i--)
                {
                    var item = listView1.Items[i];
                    if (!(item.Text.ToLower().Contains(textBox1.Text.ToLower())))
                        listView1.Items.Remove(item);
                }
                
                pictureBox2.Visible = true;
                pictureBox1.Visible = false;
            }
            recolour1();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.ForeColor == Color.Gray)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                pictureBox1_Click(sender, e);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                textBox1.ForeColor = Color.Gray;
                textBox1.Text = "Search";
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            pictureBox1_Click(sender, e);
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Error Occured computers : \n" + Form1.errorcomputers , "Error Computers List");
        }

        private void allAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            for (int i = 0; i < NameList.Count; i++)
            {
                ListViewItem item = new ListViewItem(NameList[i]);
                item.SubItems.Add(DescriptionList[i]);
                item.SubItems.Add(TypeList[i]);
                item.SubItems.Add(MachineList[i]);
                listView1.Items.Add(item);
            }
            recolour1();
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void onlyUsersAndGroupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                var item = listView1.Items[i];
                if ((item.SubItems[2].Text.Equals("Local Account")))
                    listView1.Items.Remove(item);
            }
            recolour1();
            pictureBox2.Visible = false;
            pictureBox1.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point( -166 , btnSender.Height);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            contextMenuStrip1.Show(ptLowerLeft);
        }
    }
}
