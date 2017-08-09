using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace GetDuplicates
{
    public partial class Form1 : Form
    {
        public Form1()
        {

            InitializeComponent();
            
        }
        public void recolour()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {

            
            listBox1.Items.Clear();
            //listBox2.Items.Clear();
            //listBox3.Items.Clear();
            listView1.Items.Clear();
            bool flag = false;
            if (this.comboBox2.Text.Equals("")/*.Items.Count.Equals(0)*/)
            {
                MessageBox.Show("Select Domain", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
                //Application.Exit();
            }
            if (comboBox2.Text.Equals("Select Domain"))
            {
                MessageBox.Show("INVALID Selection on Domains", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
                //Application.Exit();
            }

            //        else
            String domaintext = comboBox2.Text;

            if (comboBox1.Text.Equals("Select an Item"))
            {

                MessageBox.Show("INVALID Selection on Items", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
                //Application.Exit();
            }
            //Form1 frm = new Form1();
            //public static 
            button1.Visible = false;
            button3.Visible = true;
            int y = 0;
            //a:
            if (y == 0)
            {
                y++;
                //MessageBox.Show("cvbnm");
                button4.Visible = true;
                Application.DoEvents();


                Console.WriteLine("");
                //  goto a;
                //MessageBox.Show("It will take a few seconds..","Get-Duplicates");             
            }
            
            try
            {
                String check = comboBox1.SelectedItem.ToString();
                String strdomain = domaintext; //Domain.GetCurrentDomain().ToString();
                //for (i = 0; i < strdomain.Length; i++)
                //{
                //    if(strdomain.Split('.'
                //    count++;
                //}
                int gN = 0;
                String[] dom = strdomain.Split('.');
                String domainPath = "LDAP://";
                //MessageBox.Show(dom.Length.ToString());
                for (int j = 0; j < dom.Length; j++)
                    if(j==0)
                    domainPath = domainPath + "DC=" + dom[j];
                    else
                    domainPath = domainPath + ",DC=" + dom[j];
             //   MessageBox.Show(domainPath);
                button4.Visible = true;
                //String domainPath = "LDAP://" + domain1.ToString() + "/CN=Users,DC=" + domain1.ToString() + ",DC=com";
                DirectoryEntry entry = new DirectoryEntry(domainPath); //new DirectoryEntry("LDAP://" + domain1.ToString() + "/CN=Users,DC=" + domain1.ToString() + ",DC=com");

                //DirectoryEntry entry = new DirectoryEntry("LDAP://DC=Domain.GetCurrentDomain()");

                DirectorySearcher search = new DirectorySearcher(entry);//, ("(objectClass=User)"));

                SearchResultCollection results = search.FindAll();
                String[] gNarray = new String[results.Count];
                //String[] sNarray = new String[results.Count];
                foreach (SearchResult rs in results)
                {
                    ResultPropertyCollection resultPropColl = rs.Properties;

                    foreach (object givenName in resultPropColl[check])
                    {
                        gNarray[gN] = givenName.ToString();
                        this.listBox1.Items.Add(gNarray[gN++]);
                    }


                    /*foreach (object samName in resultPropColl["sAMAccountName"])
                    {
                        sNarray[sN] = samName.ToString();
                        //this.listBox2.Items.Add(sNarray[sN++]);
                    }*/


                }
                // MessageBox.Show(gNarray.Length + "  " + listBox1.Items.Count);
                // MessageBox.Show(gNarray[29]);
                /*   int len=gNarray.Length;
                   for (int i = 0; i <len ; i++)
                   {
                       if (gNarray[i].Equals(""))
                           --len;
                   }*/
                //MessageBox.Show(gNarray.Length + "  " + listBox1.Items.Count);
                /*for (int a = 0; a < gNarray.Length; a++)
                {
                    if (gNarray[a].Length.Equals(0))
                        gNarray[a].Remove(0);
                }*/
                ListView lv = new ListView();
                //MessageBox.Show(gNarray[24]);
      // int count = 0;
      //          for (int j = 0; j < listBox1.Items.Count; j++)
      //          {
      //              for (int k = j + 1; k < listBox1.Items.Count/*gNarray.Length*/; k++)
      //              {
      //                  if (gNarray[j].Equals(gNarray[k]))
       //                 {
        //                    flag = true;
                           //this.listBox3.Items.Add(gNarray[j]);
         //                   count++;
                            // if(!(gNarray[j].Equals("test-test"))||(gNarray[j].Equals("vimala")))
          //                  FetchSAM(check, gNarray[j], search);
                            //break;
         //               }

                        //foreach (SearchResult rs in results)
                        //{
                        //    ResultPropertyCollection resultPropColl = rs.Properties;
                        //    if (resultPropColl[check].Equals(gNarray[j]))
                        //    {
                        //        foreach (object samName in resultPropColl["sAMAccountName"])
                        //        {
                        //            this.listBox3.Items.Add(samName.ToString());
                        //        }
                        //    }
                        //}
          //          }

                    //search.Filter = "(givenName=" + gNarray[j] + ")";
                    //SearchResultCollection results1 = search.FindAll();


                    ////if(count>0)
                    ////{
                    //foreach (SearchResult rs in results1)
                    //{
                    //    ResultPropertyCollection resultPropColl = rs.Properties;
                    //  //  foreach (object givenName in entry.Properties[check].Value = gNarray[j])
                    //   // {
                    //        foreach (object samName in resultPropColl["sAMAccountName"])
                    //        {
                    //            this.listBox3.Items.Add(samName.ToString());
                    //        }
                    //    //}
                    //}
                    //}
                    /*SearchResult[] resultsarray = new SearchResult[results.Count];
                    for (int i = 0; i < results.Count; i++)
                    {
                        results.CopyTo(resultsarray, i);
                        this.listBox1.Items.Add(resultsarray[i].GetDirectoryEntry().InvokeGet(check));

                    }*/




//                }
                //MessageBox.Show(listView1.Items.Count.ToString());
        Dictionary<string, int> d = new Dictionary<string, int>();
        String s;
        int i = 0;
        while (i < listBox1.Items.Count)
        {
            s = gNarray[i];
            if (d.ContainsKey(s))
            {
                int value = (int)d[s];
                if (value == 1)
                {
                    flag = true;
                    FetchSAM(check, gNarray[i] , search);
                    d.Remove(s);
                    d.Add(s, 2);
                }
            }
            else
            {
                d.Add(s, 1);
            }

            i++;
        }
        //Console.ReadKey();

                visible();
                if (!flag)
                {
                    MessageBox.Show("No Duplicates Found", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listBox1.Items.Clear();
                    listView1.Items.Clear();
                    button4.Visible = false;

                }
                else
                {
                    ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    cl.executeDll(10);
                    recolour();
                }
                button4.Visible = false;
            }
            
            catch (ActiveDirectoryObjectNotFoundException ex)
            {
                button4.Visible = false;
                MessageBox.Show("Domain Not Found", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (ActiveDirectoryServerDownException exc)
            {
                button4.Visible = false;
                MessageBox.Show("Server is unavailable to respond", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Security.Authentication.AuthenticationException except)
            {
                button4.Visible = false;
                MessageBox.Show("Do not have the permission", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException excep)
            {
                button4.Visible = false;
                MessageBox.Show("Domain not found", "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Runtime.InteropServices.COMException coe)
            {
                button4.Visible = false;
                MessageBox.Show("" + coe.Message, "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception error)
            {
                button4.Visible = false;
                MessageBox.Show("" + error.Message, "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button3.Visible = false;
            button1.Visible = true;
            
           }
        public void FetchSAM(String check, String str, DirectorySearcher search1)
        {
           
            search1.Filter = "( " + check + "=" + str + ")";
            SearchResultCollection results1 = search1.FindAll();
            
            int i = 0;
            ListViewItem itemsamName;
            
            foreach (SearchResult rs in results1)
            {
                
                ResultPropertyCollection resultPropColl = rs.Properties;
                 // foreach (object samName in entry.Properties[check].Value = gNarray[j])
                // {

                foreach (object samName in resultPropColl["sAMAccountName"])
                {
                    
                  //  string samName = resultPropColl["sAMAccountName"][0].ToString();
                 if(resultPropColl[check][0].ToString().Equals(str))
                 {
                 
                     if (++i != 1)
                    {
                        itemsamName = new ListViewItem("", 0);
                        itemsamName.SubItems.Add(samName.ToString());
                    }
                   
                     else
                    {
                        itemsamName = new ListViewItem(str, 0);
                        itemsamName.SubItems.Add(samName.ToString());
                    }

                   listView1.Items.AddRange(new ListViewItem[] { itemsamName });
                }
                }
            }
          //  MessageBox.Show(i.ToString());
            itemsamName = new ListViewItem("", 0);
            itemsamName.SubItems.Add("");
            listView1.Items.AddRange(new ListViewItem[] { itemsamName });
        }
        private void visible()
        {
          //if(listView1.Items.Count>0)
            button4.Visible = false;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // button4.Visible = false;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            button4.Visible = false;
            comboBox1.Text = "Select an Item";
            comboBox2.Text = "Select Domain";
        }

        
        private void Form1_Load_1(object sender, EventArgs e)
        {
            //int y = 0;
            try
            {
                ListDomains();
            }
            catch (Exception exe)
            {
                MessageBox.Show("" + exe.Message, "Free AD Tools Error Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ListDomains()
        {
            try
            {
                /*
                //DirectoryEntry en = new DirectoryEntry("LDAP://");

                // Search for objectCategory type "Domain"
                DirectorySearcher srch = new DirectorySearcher("objectCategory=Domain");
                //MessageBox.Show("First Testing");
                //listView1.Items.Add("1");
                //listView1.Items.Add("2");
                //listView1.Columns[0].ListView.Items.Add("sdfgh");
                //listView1.Columns[1].ListView.Items.Add("asdf");
                //srch.PropertiesToLoad.Add("cn");
                SearchResultCollection coll = srch.FindAll();
                //MessageBox.Show("Final Testing");
                // MessageBox.Show(coll.Count.ToString());
                // Enumerate over each returned domain.
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
                        //MessageBox.Show(dom.Length.ToString());

                        for (int j = 0; j < dom.Length; j += 3)
                        {
                            if (j == (dom.Length - 1))
                                domainPath = domainPath + dom[j];
                            else
                                domainPath = domainPath + dom[j];
                        }
                        domainPath1 = domainPath.Replace(',', '.');
                        /* String strDomain1 = domainPath;
                         String[] dom1 = strDomain1.Split(',');
                         for(int j=0;j<dom1.Length;j++)
                         {
                             if (j == (dom1.Length - 1))
                                 domainPath1 = domainPath1 + dom1[j];
                             else
                                 domainPath = dom[j] + ".";
                         }*/


                        //MessageBox.Show(domainPath1);
                    /*    this.comboBox2.Items.Add(domainPath1);

                        //this.comboBox2.Items.Add(((object)("india.adventnet.com")).ToString());
                    }
                } //MessageBox.Show(c.ToString());
                //ArrayList alDomains = new ArrayList();
                /* Forest currentForest = Forest.GetCurrentForest();
                 DomainCollection myDomains = currentForest.Domains;
                 foreach (Domain objDomain in myDomains)
                 {
                     //alDomains.Add(objDomain.Name);
                     this.comboBox2.Items.Add(objDomain);
                 } */
                //return alDomains;

                Forest currentForest = Forest.GetCurrentForest();
                DomainCollection myDomains = currentForest.Domains;
                foreach (Domain objDomain in myDomains)
                {
                    //  alDomains.Add(objDomain.Name);
                    comboBox2.Items.Add(objDomain.Name);
                }
        
            }
            catch (ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show("Domain Not Found", "GetDuplicates");
            }
            catch (ActiveDirectoryServerDownException exc)
            {
                MessageBox.Show("Server is unavailable to respond", "GetDuplicates");
            }
            catch (System.Security.Authentication.AuthenticationException except)
            {
                MessageBox.Show("Do not have the permission", "GetDuplicates");
            }
        }

        private void button4_VisibleChanged(object sender, EventArgs e)
        {
            Console.WriteLine("");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //button4.CanFocus = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
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