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
    public partial class MemberSelectionForm : Form
    {
        public String userName;
        public String password;
        public String path;
        public String port;
        public String partition;
        public String dc;
        public String user;
        public bool defaultUser = false;
        public int startindexg;
        public int endindexg;
        public int issearchset;
        public String filtertext;
        public int grpcount=0;
        public int grpflag = 0;
        public int flag = 0;

        public int goandclearsearchflag = 0;
        public int goandclearsearchcount = 0;
        public int closeflag = 0;
        public ListView.ListViewItemCollection createusergroup = new ListView.ListViewItemCollection(new ListView());
        public ListView.ListViewItemCollection addgrp = new ListView.ListViewItemCollection(new ListView());
        public ListView.ListViewItemCollection groupCollection = new ListView.ListViewItemCollection(new ListView());
        public ListView.ListViewItemCollection itemcoll= new ListView.ListViewItemCollection(new ListView());
        public PropertyValueCollection ValueCollection;
        public ListView.ListViewItemCollection removegroup = new ListView.ListViewItemCollection(new ListView());
        public ListView.ListViewItemCollection refinedvaluegroup = new ListView.ListViewItemCollection(new ListView());
        SearchResultCollection results1;
        
        int loadflag;

        public MemberSelectionForm()
        {
            InitializeComponent();
            Shown+=new EventHandler(MemberSelectionForm_Shown);
        }
        public void recolour1()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }


        }
        void listView2_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            //Console.Write("Column Resizing");
            e.NewWidth = this.listView2.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
            
        }
        private void MemberSelectionForm_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("triggered");
            loadflag = 1;
            label3.Text = "Loading ... Please wait ...";
            MemberSelectionForm.ActiveForm.Text = "Loading ....";
            listView2.Items.Clear();
            try
            {
                DirectoryEntry de1;
                if (!defaultUser)
                {
                    de1 = new DirectoryEntry(path, userName, password, AuthenticationTypes.Secure);

                }
                else
                {
                    de1 = new DirectoryEntry(path);
                    //de.Dispose();
                }

                //------------------------------------------------------------------------------------------------

                de1.RefreshCache(new string[] { "canonicalName" });
                DirectorySearcher ds1 = new DirectorySearcher(de1);

                ds1.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                if (issearchset != 1)
                {
                    ds1.Filter = "(objectClass=group)";
                }
                else
                {
                    ds1.Filter = filtertext;
                }
                ds1.PageSize = 100;
                ds1.PropertiesToLoad.Add("canonicalName");
                ds1.PropertiesToLoad.Add("cn");

                //ds.PropertiesToLoad.Add("userPrincipalName");

                //ds1.PropertyNamesOnly = true;
                ds1.Sort.Direction = SortDirection.Ascending;
                ds1.Sort.PropertyName = "cn";
                results1 = ds1.FindAll();
                if (results1.Count == 0)
                {
                    MessageBox.Show("Cannot find items matching your search query");
                    textBox1.Text = "";
                    issearchset = 0;
                    MemberSelectionForm_Load(sender, e);
                    MemberSelectionForm_Shown(sender, e);
                }
                else
                {
                    //MessageBox.Show(ValueCollection.Count.ToString());
                    de1.Dispose();

                    int go1 = 100;
                    if (results1.Count < 100)
                    {
                        go1 = results1.Count;

                    }


                    listView1.BeginUpdate();

                    for (int i = 0; i < go1; i++)
                    {

                        ListViewItem item = new ListViewItem();

                        SearchResult sr1 = results1[i];
                        String[] data1 = { "-" };

                        if (sr1 != null)
                        {

                            ResultPropertyCollection myResultPropColl1;
                            myResultPropColl1 = sr1.Properties;


                            foreach (string myKey in myResultPropColl1.PropertyNames)
                            {

                                if (myKey.Trim().Equals("canonicalname"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        data1[0] = myCollection.ToString();


                                    }
                                }



                                if (myKey.Trim().Equals("cn"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        item.Text = myCollection.ToString();

                                    }
                                }
                                if (myKey.Trim().Equals("adspath"))
                                {
                                    foreach (Object myCollection in myResultPropColl1[myKey])
                                    {
                                        //do nothing

                                    }
                                }
                            }
                            item.SubItems.Add(data1[0]);
                            listView1.Items.Add(item);
                        }
                        listView1.EndUpdate();
                        startindexg = 0;
                        endindexg = 100;
                        button5.Enabled = true;
                        if (results1.Count < 100)
                        {

                            button5.Enabled = false;
                            button6.Enabled = false;
                            button22.Enabled = false;
                            button23.Enabled = false;
                            endindexg = results1.Count;
                        }
                        button6.Enabled = false;
                        button22.Enabled = false;
                        label8.Text = (startindexg + 1).ToString() + " - " + endindexg.ToString() + " of " + results1.Count.ToString();

                    }
                    recolour1();
                    loadflag = 0;
                    if (ValueCollection == null)
                    {
                        //
                    }
                    else
                    {
                        //Adding items as checked for every items in valuecollection which "member of" groups and intermediary groups when clicking add
                        foreach (String val in ValueCollection)
                        {
                            //String temp = val.Remove(0, 3);
                            ////MessageBox.Show(temp);
                            //int start;
                            //start = temp.IndexOf(",");

                            //String finalstr;
                            //finalstr = temp.Substring(0, start);
                            ListViewItem item1 = listView1.FindItemWithText(val.Trim(), false, 0, false);
                            if (item1 == null)
                            {
                                //MessageBox.Show("cant find");

                            }
                            else
                            {
                                item1.Checked = true;
                            }
                        }
                        if (itemcoll != null)
                        {
                            foreach(ListViewItem it in itemcoll)
                            {
                                ListViewItem item1 = listView1.FindItemWithText(it.Text.Trim(), false, 0, false);
                                if (item1 == null)
                                {
                                    //MessageBox.Show("cant find");

                                }
                                else
                                {
                                    item1.Checked = true;
                                }
                            }
                        }
                    }
                    MemberSelectionForm.ActiveForm.Text = "Select Groups";

                }
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show(" The network path was not found ");
                button5.Enabled = true;
                return;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button5.Enabled = true;
                return;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Error");
                button5.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    button5.Enabled = true;
                    return;
                }
                else
                {
                //-----
                }
            }
            label3.Text = "";
            
            if (ValueCollection != null)
            {
                //adding items from valuecollection to refinedvalue which contains the same items
                refinedvaluegroup.Clear();
                foreach (String strr in ValueCollection)
                {
                    //String temp = strr.Remove(0, 3);
                    ////MessageBox.Show(temp);
                    //int start;
                    //start = temp.IndexOf(",");

                    //String finalstr;
                    //finalstr = temp.Substring(0, start);
                    refinedvaluegroup.Add(strr);
                    //MessageBox.Show(finalstr);
                }
            }
            grpcount = refinedvaluegroup.Count;
            
            if(goandclearsearchflag==1)
            {
                grpcount = goandclearsearchcount;
            }
            label1.Text = "groups selected : " + grpcount.ToString();
            
        }
      
        private void MemberSelectionForm_Load(object sender, EventArgs e)
        {
            listView2.DrawSubItem += new DrawListViewSubItemEventHandler(listview1_DrawSubItem);
            //this.listView1.ItemCheck += new ItemCheckEventHandler(listView1_ItemCheck1);
            this.listView1.ItemChecked += new ItemCheckedEventHandler(listView1_ItemCheck1);
            listView2.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listView2_ColumnWidthChanging);
            listView1.Items.Clear();

            CheckBox chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "ToSelectAll";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);          
            chkBox.Visible = true;
            checkBox1.CheckedChanged += SelectAllGroups;
            checkBox1.Name = "ToSelectAll1";
            
            button6.Enabled = true;
            button5.Enabled = true;
            button22.Enabled = false;
            button23.Enabled = true;

            //if (createusergroup.Count != 0)
            //{
            //    ValueCollection.Clear();
            //    foreach (ListViewItem lv in createusergroup)
            //    {
            //        ValueCollection.Add(lv.Text);
            //    }
            //}

            

        }
        private void SelectAllGroups(System.Object sender, System.EventArgs e)
        {
            //CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll1"];
            CheckBox cb = (CheckBox)sender;

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;

        }
        void listview1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.LightGray, e.Bounds);
            e.DrawText();
        }
        private void listView1_DrawColumnHeader_1(object sender, DrawListViewColumnHeaderEventArgs e)
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
        void listview1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {

            e.DrawBackground();
            e.DrawText();

        }
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                loadflag = 1;

                listView1.Items.Clear();
                button23.Enabled = true;
                listView1.BeginUpdate();
                startindexg = startindexg - 100;

                if (endindexg == results1.Count)
                {
                    button5.Enabled = true;
                    int teeemp = endindexg / 100;

                    endindexg = teeemp * 100;
                    if(startindexg==0)
                    {
                        button6.Enabled = false;
                        
                        button22.Enabled = false;
                    }

                }
                else if (startindexg == 0)
                {
                    button6.Enabled = false;
                    button5.Enabled = true;
                    button22.Enabled = false;
                    endindexg = endindexg - 100;
                }
                else
                {
                    button5.Enabled = true;
                    endindexg = endindexg - 100;
                }


                //MessageBox.Show(startindexg.ToString() + "--" + endindexg.ToString());
                for (int i = startindexg; i < endindexg; i++)
                {

                    ListViewItem item = new ListViewItem();

                    SearchResult sr1 = results1[i];
                    String[] data1 = { "-" };

                    if (sr1 != null)
                    {

                        ResultPropertyCollection myResultPropColl1;
                        myResultPropColl1 = sr1.Properties;


                        foreach (string myKey in myResultPropColl1.PropertyNames)
                        {

                            if (myKey.Trim().Equals("canonicalname"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    data1[0] = myCollection.ToString();


                                }
                            }



                            if (myKey.Trim().Equals("cn"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    item.Text = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("adspath"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    //do nothing

                                }
                            }

                        }
                        item.SubItems.Add(data1[0]);
                        listView1.Items.Add(item);

                    }

                    label8.Text = (startindexg + 1).ToString() + " - " + endindexg.ToString() + " of " + results1.Count.ToString();
                    listView1.EndUpdate();



                }
                recolour1();
                loadflag = 0;
                if (ValueCollection != null)
                {
                    foreach (String val in ValueCollection)
                    {
                        //String temp = val.Remove(0, 3);
                        ////MessageBox.Show(temp);
                        //int start;
                        //start = temp.IndexOf(",");

                        //String finalstr;
                        //finalstr = temp.Substring(0, start);
                        ListViewItem item1 = listView1.FindItemWithText(val.Trim(), false, 0, false);
                        if (item1 == null)
                        {
                            //MessageBox.Show("Null");

                        }
                        else
                        {
                            item1.Checked = true;
                        }
                    }
                }
                if (itemcoll != null)
                {
                    foreach (ListViewItem j in itemcoll)
                    {
                        //MessageBox.Show("items in itemcoll : "+j.Text);
                        ListViewItem item1 = listView1.FindItemWithText(j.Text.Trim(), false, 0, false);
                        if (item1 == null)
                        {
                            //MessageBox.Show("Null");

                        }
                        else
                        {
                            item1.Checked = true;
                        }
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform the action", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                loadflag = 1;
                listView1.Items.Clear();
                button6.Enabled = true;
                button22.Enabled = true;
                button23.Enabled = true;
                //label22.Text = "Loading User Details .. Please Wait ..  results";

                startindexg += 100;
                endindexg += 100;
                if (endindexg > results1.Count)
                {
                    endindexg = results1.Count;
                    button23.Enabled = false;
                }
                if (endindexg == results1.Count)
                {
                    button5.Enabled = false;
                }
                listView1.BeginUpdate();
                //MessageBox.Show(startindexg.ToString() + "--" + endindexg.ToString());
                for (int i = startindexg; i < endindexg; i++)
                {
                    ListViewItem item = new ListViewItem();

                    SearchResult sr1 = results1[i];
                    String[] data1 = { "-" };

                    if (sr1 != null)
                    {

                        ResultPropertyCollection myResultPropColl1;
                        myResultPropColl1 = sr1.Properties;


                        foreach (string myKey in myResultPropColl1.PropertyNames)
                        {

                            if (myKey.Trim().Equals("canonicalname"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    data1[0] = myCollection.ToString();


                                }
                            }



                            if (myKey.Trim().Equals("cn"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    item.Text = myCollection.ToString();

                                }
                            }
                            if (myKey.Trim().Equals("adspath"))
                            {
                                foreach (Object myCollection in myResultPropColl1[myKey])
                                {
                                    //do nothing

                                }
                            }

                        }
                        item.SubItems.Add(data1[0]);
                        listView1.Items.Add(item);


                    }
                    listView1.EndUpdate();

                    label8.Text = (startindexg + 1).ToString() + " - " + endindexg.ToString() + " of " + results1.Count.ToString();

                }
                recolour1();
                loadflag = 0;
                if (ValueCollection != null)
                {
                    foreach (String val in ValueCollection)
                    {

                        //String temp = val.Remove(0, 3);
                        ////MessageBox.Show(temp);
                        //int start;
                        //start = temp.IndexOf(",");

                        //String finalstr;
                        //finalstr = temp.Substring(0, start);
                        ListViewItem item1 = listView1.FindItemWithText(val.Trim(), false, 0, false);
                        if (item1 == null)
                        {
                            //MessageBox.Show("Null");

                        }
                        else
                        {

                            item1.Checked = true;
                        }
                    }
                }

                if (itemcoll != null)
                {

                    foreach (ListViewItem it in itemcoll)
                    {
                        ListViewItem item1 = listView1.FindItemWithText(it.Text, false, 0, false);

                        if (item1 == null)
                        {
                            //MessageBox.Show("Null");

                        }
                        else
                        {
                            item1.Checked = true;
                            //MessageBox.Show("found");
                        }
                    }
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Cannot perform the action", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            if (itemcoll != null)
            {
                foreach (ListViewItem s in itemcoll)
                {
                     
                    if (!groupCollection.Contains(s))
                    {
                        groupCollection.Add((ListViewItem)s.Clone());
                       
                    }

                }
            }
            
            flag = 1;
            refinedvaluegroup.Clear();
            if (ValueCollection != null)
            {
                foreach (String strr in ValueCollection)
                {
                    //String temp = strr.Remove(0, 3);
                    ////MessageBox.Show(temp);
                    //int start;
                    //start = temp.IndexOf(",");

                    //String finalstr;
                    //finalstr = temp.Substring(0, start);
                    refinedvaluegroup.Add(strr);
                    //MessageBox.Show(finalstr);
                }
            }
            
            ListView.ListViewItemCollection dupeCollection = new ListView.ListViewItemCollection(new ListView());
            if (refinedvaluegroup != null)
            {
                foreach (ListViewItem lii in refinedvaluegroup)
                {
                    dupeCollection.Add((ListViewItem)lii.Clone());
                }
            }
            
            if (removegroup != null)
            {
                foreach (ListViewItem lvi in removegroup)
                {
                    foreach (ListViewItem lii in dupeCollection)
                    {
                        if (lvi.Text == lii.Text)
                        {
                            refinedvaluegroup.Remove(lii);
                        }
                    }
                }
            }
            //foreach (ListViewItem lii in refinedvaluegroup)
            //{
            //    MessageBox.Show(lii.Text);
            //}

            
            this.Close();
            
        }

        private void button22_Click(object sender, EventArgs e)
        {
            startindexg = 100;
            endindexg = 200;
            button6_Click(sender, e);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            int temp = results1.Count % 100;
            endindexg = results1.Count - temp;
            startindexg = endindexg - 100;

            button5_Click(sender, e);
        }
        private void listView1_ItemCheck1(object sender,
                System.Windows.Forms.ItemCheckedEventArgs e)
        {
            if (e.Item.Checked ==false)
            {
                bool found = false;
                bool propfound = false;

                if (ValueCollection != null)
                {
                    foreach (String it in ValueCollection)
                    {
                        //String temp = it.Remove(0, 3);
                        
                        //int start;
                        //start = temp.IndexOf(",");

                        //String finalstr;
                        //finalstr = temp.Substring(0, start);
                        //MessageBox.Show("checking "+finalstr);
                        if (e.Item.Text == it.Trim())
                        {
                            propfound = true;
                        }
                    }
                    if (propfound)
                    {
                        //MessageBox.Show("yes");
                        //ValueCollection.Remove((ListViewItem)e.Item.Clone());
                        String torem=null;
                        foreach (String it in ValueCollection)
                        {
                            //String temp = it.Remove(0, 3);

                            //int start;
                            //start = temp.IndexOf(",");

                            //String finalstr;
                            //finalstr = temp.Substring(0, start);
                            if (e.Item.Text == it)
                            {
                                if (loadflag == 1)
                                {
                                    //MessageBox.Show("load flag setttttttttttt");
                                }
                                else
                                {
                                    //grpcount--;
                                    //MessageBox.Show("removing from valcoll"+finalstr);
                                    label1.Text = "groups selected : " + grpcount;
                                    torem = it;
                                    //ValueCollection.Remove(it);
                                    removegroup.Add((ListViewItem)e.Item.Clone());
                                    //foreach (String temp11 in ValueCollection)
                                    //{
                                    //    MessageBox.Show("items in valcol  "+temp11);
                                    //}
                                }
                            }
                        }
                        if (torem != null)
                        {
                            ValueCollection.Remove(torem);
                            foreach(ListViewItem li in refinedvaluegroup)
                            {
                                if (li.Text == torem)
                                {
                                    refinedvaluegroup.Remove(li);
                                }
                            }
                            //refinedvaluegroup.Remove(torem);
                        }
                        
                    }
                    
                }
                if (itemcoll != null)
                {
                    foreach (ListViewItem it in itemcoll)
                    {
                        //MessageBox.Show("checking "+it.Text);
                        //MessageBox.Show("1");
                        if (e.Item.Text == it.Text)
                        {
                            found = true;
                        }
                    }
                    if (found)
                    {
                        //MessageBox.Show("yes");
                        //MessageBox.Show("2");
                        itemcoll.Remove((ListViewItem)e.Item.Clone());
                        foreach (ListViewItem it in itemcoll)
                        {

                            if (e.Item.Text == it.Text)
                            {
                                if (loadflag == 1)
                                {
                                    //MessageBox.Show("load flag setttttttttttt");
                                }
                                else
                                {
                                    grpcount--;
                                    label1.Text = "groups selected : " + grpcount;
                                    itemcoll.RemoveAt(it.Index);
                                    foreach (ListViewItem lv in refinedvaluegroup)
                                    {
                                        if (e.Item.Text == lv.Text)
                                        {
                                            //MessageBox.Show("triggered uncheckk");
                                            grpflag++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            else if ((e.Item.Checked == true))
            {
                
                bool found = false;
                //if (grpcount > 0)
                //{
                    
                    

                    if(itemcoll!=null)
                    {
                        foreach (ListViewItem it in itemcoll)
                        {
                            
                            if (e.Item.Text == it.Text)
                            {
                                found = true;
                            }
                        }
                        if(!found)
                        {
                            foreach(ListViewItem lv in refinedvaluegroup)
                            {
                                if (e.Item.Text == lv.Text)
                                {
                                    //MessageBox.Show("triggered");
                                    if (grpflag == 0)
                                    {
                                        grpcount--;
                                    }
                                    else
                                    {
                                        grpflag--;
                                    }
                                }
                            }
                            grpcount++;
                            label1.Text = "groups selected : " + grpcount;
                            label1.Refresh();
                            //MessageBox.Show("triggered");
                            itemcoll.Add((ListViewItem)e.Item.Clone());
                        }
                        
                        //foreach (ListViewItem it in itemcoll)
                        //{
                        //    MessageBox.Show("-----"+ it.Text);
                        //}
                        
                    }
                //}
            }

            
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            listView1.Location = new System.Drawing.Point(8, 101);
            textBox1.Visible = true;
            button2.Visible = true;
            //MessageBox.Show(listView1.Location.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "")
            {
                issearchset = 0;
                //MessageBox.Show("Enter any search terms");
                goandclearsearchflag = 1;
                goandclearsearchcount = grpcount;
                MemberSelectionForm_Load(sender, e);
                MemberSelectionForm_Shown(sender, e);
            }
            else
            {
                issearchset = 1;
                String appendtext = "";
                filtertext = "(objectClass=group)";
                if (textBox1.Text != "")
                {
                    appendtext += "(cn=*" + textBox1.Text + "*)";

                }
                filtertext = "(&" + filtertext + appendtext + ")";
                //MessageBox.Show(filtertext);
                goandclearsearchflag = 1;
                goandclearsearchcount = grpcount;
                MemberSelectionForm_Load(sender, e);
                MemberSelectionForm_Shown(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            closeflag = 1;
            this.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            issearchset = 0;
            textBox1.Text = "";
            goandclearsearchflag = 1;
            goandclearsearchcount = grpcount;
            MemberSelectionForm_Load(sender,e);
            MemberSelectionForm_Shown(sender, e);
        }        
    }
}
