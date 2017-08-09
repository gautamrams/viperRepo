using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices.ActiveDirectory;


namespace SendGreetingcheck
{
    public partial class ListDC : Form
    {
        string domName = "";
        string[] userSamACNames;
        LastLogonDates lastLogonDates = null;
        int counter=0;
        public ListDC()
        {
            InitializeComponent();
            this.SuspendLayout();
            this.Location = new System.Drawing.Point(320, 325);
            this.ResumeLayout();
            //aForm.Location = new System.Drawing.Point(nLeft,nTop);
        }
        public void getDCNamesSearch(String domainName, string[] userSamNames, int count)
        {
            domName = domainName;
            this.userSamACNames = new string[count];
            this.userSamACNames = userSamNames;
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, domainName); DomainControllerCollection controller = System.DirectoryServices.ActiveDirectory.DomainController.FindAll(context);
            DomainControllerlist.View = View.Details;
            DomainControllerlist.GridLines = true;
            DomainControllerlist.CheckBoxes = true;
            DomainControllerlist.Columns.Add("Domain Controllers", 457, HorizontalAlignment.Center);
            foreach (Object dc in controller)
            {
                DomainControllerlist.Items.Add("" + dc.ToString());

            }
            foreach (ListViewItem CurrentItem in DomainControllerlist.Items)
            {
                CurrentItem.Checked = true;
                counter++;
            }
        }
        private void ListDC_Load(object sender, EventArgs e)
        {

        }

        private void GenerateReport_Click(object sender, EventArgs e)
        {
            lastLogonDates = new LastLogonDates();
            ListView.CheckedListViewItemCollection checkedItems = DomainControllerlist.CheckedItems;
            string[] DCNames = new string[checkedItems.Count];
            int i = 0;
            foreach (ListViewItem item in checkedItems)
            {
                DCNames[i] = item.SubItems[0].Text;
                i++;
            }

            lastLogonDates.createColumnHeaders(DCNames);
            if (LastLogonradioButton.Checked && checkedItems.Count > 0)
            {
                foreach (String usrSamName in userSamACNames)

                    lastLogonDates.LastLogon(usrSamName, domName, DCNames);
                lastLogonDates.Show();
                this.Hide();
            }
            else if (LastLogonTimestampradioButton.Checked && checkedItems.Count > 0)
            {
                foreach (String usrSamName in userSamACNames)

                    lastLogonDates.LastLogonlastLogonTimestamp(usrSamName, domName, DCNames);
                lastLogonDates.Show();
                this.Hide();
            }
            else if(DomainControllerlist.CheckedItems.Count!=counter)
                    MessageBox.Show("Note: It is advisable you select all the domain controllers in order to get accurate data.Press 'OK' to select DCs or 'Cancel' to proceed.",
            " Try Againg!",MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            else
                MessageBox.Show("Please Select Any one of the  DC Name to retrieve the LastLogon Date!", " Try Againg!", MessageBoxButtons.OK, MessageBoxIcon.Information);
         
        }
    }
}