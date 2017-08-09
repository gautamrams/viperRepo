using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Windows.Forms;


namespace GetDomains
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListDomains();

        }
        public void recolour()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
        }
        public void ListDomains()
        {
            try
            {
               /* DirectoryEntry en = new DirectoryEntry("LDAP://");
                DirectorySearcher srch = new DirectorySearcher("objectCategory=Domain");
                //srch.PropertiesToLoad.Add("cn");
                SearchResultCollection coll = srch.FindAll();
                String domainPath = null, domainPath1 = null;
                int c = 0;
                foreach (SearchResult rs in coll)
                {
                    ResultPropertyCollection rpc = rs.Properties;
                    c++;
                    foreach (object domainName in rpc["distinguishedName"])
                    {
                        String strDomain = domainName.ToString();
                        String[] dom = strDomain.Split("DC=".ToCharArray());
                        for (int j = 0; j < dom.Length; j += 3)
                        {
                            if (j == (dom.Length - 1))
                                domainPath = domainPath + dom[j];
                            else
                                domainPath = domainPath + dom[j];
                        }
                        domainPath1 = domainPath.Replace(',', '.');
                        this.comboBox1.Items.Add(domainPath1);
                    }
                }
                /* Forest currentForest = Forest.GetCurrentForest();
                DomainCollection myDomains = currentForest.Domains;
                foreach (Domain objDomain in myDomains)
                {
                       this.comboBox1.Items.Add(objDomain);
                } */
                Forest currentForest = Forest.GetCurrentForest();
                DomainCollection myDomains = currentForest.Domains;
                foreach (Domain objDomain in myDomains)
                {
                    //  alDomains.Add(objDomain.Name);
                    comboBox1.Items.Add(objDomain.Name);
                }
        

            }
            catch (System.Runtime.InteropServices.COMException coe)
            {
                button4.Visible = false;
                MessageBox.Show("" + coe.Message, "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message, "AD Free Tool Information", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.listView1.Items.Clear();
            if (this.comboBox1.Text.Equals(""))
            {
                MessageBox.Show("Please Select a Domain", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (comboBox1.Text.Equals("Select Domain"))
            {
                MessageBox.Show("Please Select a Domain", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            String domaintext = comboBox1.Text;
            Application.DoEvents();
            button4.Visible = true;
            //MessageBox.Show("");

            ListDomainControllers(domaintext);
        }
        public void ListDomainControllers(String domaintext)
        {
            ListView lv = new ListView();
            try
            {
                DirectoryContextType contexttype = new DirectoryContextType();
                DirectoryContext context = new DirectoryContext(contexttype, domaintext);
                Domain domain = Domain.GetDomain(context);

                foreach (DomainController dc in domain.DomainControllers)
                {
                    bool flag = false;
                    ListViewItem itemGC;// = new ListViewItem(dc.Name, 0);
                    //MessageBox.Show(dc.Name.ToString(),"Get Domain");
                    try
                    {
                        // listBox1.Items.Add(dc.Name.ToString());
                        if (dc.IsGlobalCatalog())
                        {
                            flag = true;
                            itemGC = new ListViewItem(dc.Name, 0);
                            itemGC.SubItems.Add("GlobalCatalog");
                            listView1.Items.AddRange(new ListViewItem[] { itemGC });
                            //listBox1.Items.Add("Global Catalog");
                        }

                        foreach (ActiveDirectoryRole role in dc.Roles)
                        {
                            if (flag == false)
                                itemGC = new ListViewItem(dc.Name, 0);
                            else
                                itemGC = new ListViewItem("", 0);
                            itemGC.SubItems.Add(role.ToString());
                            flag = true;
                            listView1.Items.AddRange(new ListViewItem[] { itemGC });
                            //listBox1.Items.Add(role.ToString());
                        }
                        //for (int i = 0; i < 100000000; i++) ;
                        button4.Visible = false;
                    }
                    catch (NullReferenceException e)
                    {
                        MessageBox.Show("" + e.Message, "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        button4.Visible = false;
                    }
                    if (flag == false)
                    {
                        //for (int i = 0; i < 100000000; i++) ;
                        itemGC = new ListViewItem(dc.Name, 0);
                        itemGC.SubItems.Add("   ----");
                        listView1.Items.AddRange(new ListViewItem[] { itemGC });

                        itemGC = new ListViewItem("", 0);
                        itemGC.SubItems.Add("");
                        listView1.Items.AddRange(new ListViewItem[] { itemGC });
                        //listBox1.Items.Add("---------");
                        button4.Visible = false;
                    }
                    else
                    {
                        itemGC = new ListViewItem("", 0);
                        itemGC.SubItems.Add("");
                        listView1.Items.AddRange(new ListViewItem[] { itemGC });
                    }
                }
                recolour();
                ClassLibrary1.Class1 cl =new  ClassLibrary1.Class1();
                cl.executeDll(9);
            }
            catch (ActiveDirectoryObjectNotFoundException e)
            {
                MessageBox.Show("Domain Not Found", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button4.Visible = false;
            }
            catch (ActiveDirectoryServerDownException exc)
            {
                MessageBox.Show("Server is unavailable to respond", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button4.Visible = false;
            }
            catch (System.Security.Authentication.AuthenticationException except)
            {
                MessageBox.Show("Do not have the permission", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button4.Visible = false;
            }
            catch (Exception error)
            {
                MessageBox.Show(""+error.Message,"Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button4.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button4.Visible = false;
            this.listView1.Items.Clear();
            this.comboBox1.Text = "Select Domain";
        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;

                using (var headerFont = new Font("Microsoft Sans Serif", 10, FontStyle.Bold))
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

       
    }
}