using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CheckComboBoxTest;
namespace ServiceAccountManagement
{
    public partial class AssociatedServices : Form
    {
        System.Collections.Generic.List<String> ServiceAccountList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> ComputerList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> ServicesList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> FilterServiceAccountList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> FilterComputerList = new System.Collections.Generic.List<String>();
        
        public AssociatedServices()
        {
            InitializeComponent();
            
        }
        public void recolour1()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
        }

        private void AssociatedServices_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            listView1.Items.Clear();
            ServiceAccountList.Clear();
            ComputerList.Clear();
            ServicesList.Clear();
            ccbox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ccbox1_ItemCheck);
            ccbox2.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ccbox2_ItemCheck);


            for(int i = 0 ; i < Form1.S_ServiceAccountList.Count ; i++ )
            {
                ListViewItem item = new ListViewItem();
                item.Text = Form1.S_ServiceAccountList[i].ToString();
                ServiceAccountList.Add(Form1.S_ServiceAccountList[i].ToString());
                item.SubItems.Add(Form1.S_ComputerList[i].ToString());
                ComputerList.Add(Form1.S_ComputerList[i].ToString());
                item.SubItems.Add(Form1.S_ServiceList[i].ToString());
                ServicesList.Add(Form1.S_ServiceList[i].ToString());
                listView1.Items.Add(item);
                
            }
            Dictionary<String, int> uniqueStore1 = new Dictionary<String, int>();
            foreach (string currValue1 in ServiceAccountList)
            {
                if (!uniqueStore1.ContainsKey(currValue1))
                {
                    uniqueStore1.Add(currValue1, 0);
                    ccbox1.Items.Add(new CCBoxItem(currValue1, 0));
                }
            }

           Dictionary<String, int> uniqueStore2 = new Dictionary<String, int>();
           foreach (string currValue2 in ComputerList)
            {
                if (!uniqueStore2.ContainsKey(currValue2))
                {
                    uniqueStore2.Add(currValue2, 0);
                    ccbox2.Items.Add(new CCBoxItem(currValue2, 0));
                }
            }
           ccbox1.MaxDropDownItems = 6;
           ccbox1.DisplayMember = "Name";
           ccbox1.ValueSeparator = ", ";
           for (int i = 0; i < ccbox1.Items.Count; i++)
                ccbox1.SetItemChecked(i, true);
           
           ccbox2.MaxDropDownItems = 6;
           ccbox2.DisplayMember = "Name";
           ccbox2.ValueSeparator = ", ";
           for (int i = 0; i < ccbox2.Items.Count; i++)
               ccbox2.SetItemChecked(i, true);

           recolour1();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button3.Visible = false;
          //  panel2.Visible = false; 
            panel1.Visible = true;            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ccbox1_DropDownClosed(sender, e);
            ccbox2_DropDownClosed(sender, e);
            listView1.Items.Clear();
            FilterComputerList.Clear();
            FilterServiceAccountList.Clear();
            /* foreach (CCBoxItem serviceaccount in ccbox1.Items)
            {
                j = 0;
                foreach (CCBoxItem computer in ccbox2.Items)
                {
                    if (serviceaccount.Equals(ServiceAccountList[i]) && computer.Equals(ComputerList[j]))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Text = ServiceAccountList[i].ToString();
                        item.SubItems.Add(LocalServiceAccounts.ComputerList[j].ToString());
                        item.SubItems.Add(LocalServiceAccounts.ServiceList[j].ToString());
                        listView1.Items.Add(item);
                    
                    }
                    j++;
                }
                i++; */
            ccbox1_DropDownClosed(sender,e);
            ccbox2_DropDownClosed(sender, e);

            

            
            for (int i = 0; i < ServiceAccountList.Count; i++)
            { 
              if(FilterServiceAccountList.Any( s => s.Equals(ServiceAccountList[i])) && FilterComputerList.Any( d => d.Equals(ComputerList[i]))) 
              {
                ListViewItem item = new ListViewItem();
                item.Text = ServiceAccountList[i].ToString();
                item.SubItems.Add(ComputerList[i].ToString());
                item.SubItems.Add(ServicesList[i].ToString());
                listView1.Items.Add(item);
              }
            }
            recolour1();
            //button3.Visible = true;
            label1.Visible = true;
            panel1.Visible = false;
            panel2.Visible = true; 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ccbox1_DropDownClosed(sender, e);
            ccbox2_DropDownClosed(sender, e); 
            button3.Visible = true;
            panel1.Visible = false;
            panel2.Visible = true; 
        }

        private void ccbox1_DropDownClosed(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("Items checked: ");
            foreach (CCBoxItem item in ccbox1.CheckedItems)
            {
                FilterServiceAccountList.Add(item.Name);
                sb.Append(item.Name).Append(ccbox1.ValueSeparator);
            }
            sb.Remove(sb.Length - ccbox1.ValueSeparator.Length, ccbox1.ValueSeparator.Length);
        }



        private void ccbox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
           
            CCBoxItem item = ccbox1.Items[e.Index] as CCBoxItem;
            
        }
        private void ccbox2_DropDownClosed(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("Items checked: ");
            foreach (CCBoxItem item in ccbox2.CheckedItems)
            {
                FilterComputerList.Add(item.Name);
                sb.Append(item.Name).Append(ccbox2.ValueSeparator);
            }
            sb.Remove(sb.Length - ccbox2.ValueSeparator.Length, ccbox2.ValueSeparator.Length);
        }



        private void ccbox2_ItemCheck(object sender, ItemCheckEventArgs e)
        {

            CCBoxItem item = ccbox2.Items[e.Index] as CCBoxItem;

        }

        private void label1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            
            listView1.Items.Clear();
            for (int i = 0; i < ServiceAccountList.Count; i++)
            {
                    ListViewItem item = new ListViewItem();
                    item.Text = ServiceAccountList[i].ToString();
                    item.SubItems.Add(ComputerList[i].ToString());
                    item.SubItems.Add(ServicesList[i].ToString());
                    listView1.Items.Add(item);
            }
          
            for (int i = 0; i < ccbox1.Items.Count; i++)
                ccbox1.SetItemChecked(i, true);
         
            for (int i = 0; i < ccbox2.Items.Count; i++)
                ccbox2.SetItemChecked(i, true);

            button3.Visible = true;
            recolour1();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count < 1)
            {
                MessageBox.Show("No data to export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog
                {
                    Title = "Choose file to save to",
                    FileName = "Services.csv",
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
