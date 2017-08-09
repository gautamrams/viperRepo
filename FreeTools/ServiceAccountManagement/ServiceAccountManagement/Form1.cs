using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using Tools;
using System.Runtime.InteropServices;
using System.Security.Principal;
using CheckComboBoxTest;
using System.IO;
using System.ServiceProcess;
using System.Management;
using System.Text.RegularExpressions;

namespace ServiceAccountManagement
{

    public partial class Form1 : Form
    {

        const int NO_ERROR = 0;
        const int ERROR_INSUFFICIENT_BUFFER = 122;

        enum SID_NAME_USE
        {
            SidTypeUser = 1,
            SidTypeGroup,
            SidTypeDomain,
            SidTypeAlias,
            SidTypeWellKnownGroup,
            SidTypeDeletedAccount,
            SidTypeInvalid,
            SidTypeUnknown,
            SidTypeComputer
        }

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool LookupAccountSid(
          string lpSystemName,
          [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
          StringBuilder lpName,
          ref uint cchName,
          StringBuilder ReferencedDomainName,
          ref uint cchReferencedDomainName,
          out SID_NAME_USE peUse);
        

        System.Collections.Generic.List<String> StartList = new System.Collections.Generic.List<String>();


        string path = null, uripath = null;
        System.Collections.Generic.List<String> NameList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> DescriptionList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> TypeList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> MachineList = new System.Collections.Generic.List<String>();
        public static System.Collections.ArrayList S_ServiceList = new System.Collections.ArrayList();
        public static System.Collections.ArrayList S_ServiceAccountList = new System.Collections.ArrayList();
        public static System.Collections.ArrayList S_ComputerList = new System.Collections.ArrayList();




        List<String> final_List = new List<String>();
        public static System.Collections.ArrayList List = new System.Collections.ArrayList();
        public static System.Collections.ArrayList computersList = new System.Collections.ArrayList();
        public static System.Collections.ArrayList finalList = new System.Collections.ArrayList();

        public static String DomainName;
        public static String SelectedDomain;
        public static String S_SelectedDomain;
        public static String UserName;
        public static String Password;
        public static bool defaultUser = true;
        public static string resultdc = "";
        public static string errorcomputers = "";
        Addcomputers addMultipleComputers = new Addcomputers();
        public Form1()
        {
            
            InitializeComponent();
            this.Left = 863;
            this.Top = 761;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            _Form1 = this;

            AddCurrentDomain();
            CheckComboData();            
        }
       
        public static Form1 _Form1;
        public void CheckComboData()
        {
            Constants.computerList.Clear();
            String domainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            String localcomputerName = Environment.MachineName;
            DomainName = domainName;
            S_SelectedDomain = domainName;
            uripath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = new Uri(uripath).LocalPath;
            final_List.Clear();
            StartList.Clear();
            StartList.Add(localcomputerName);
            int i = 0;
            try
            {

                System.Collections.Generic.List<String> dcList = new System.Collections.Generic.List<string>();
                Domain domain;
                domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domainName));

                foreach (System.DirectoryServices.ActiveDirectory.DomainController dc in domain.DomainControllers)
                {
                    int index = dc.Name.IndexOf('.');
                    String dcName = dc.Name.Substring(0, index);
                    dcList.Add(dcName);
                }
                if (new FileInfo(path + "/conf/service.txt").Length != 0)
                {
                    string result = File.ReadAllText(path + "/conf/service.txt");
                    foreach (string a in result.Split(','))
                    {
                        string temp = a.TrimEnd(' ', '\n');
                        if (temp.Length > 1 && temp != localcomputerName)
                        {
                            //ccbox.Items.Add(new CCBoxItem(temp, 0));
                            StartList.Add(temp);
                            i++;
                        }
                    }
                }
                String adsPath = "LDAP://" + domainName;
                DirectoryEntry domainEntry;
                domainEntry = new DirectoryEntry(adsPath);
                DirectorySearcher mySearcher = new DirectorySearcher(domainEntry);
                mySearcher.Filter = "(ObjectCategory=computer)";
                domainEntry.Children.SchemaFilter.Add("computer");
                foreach (SearchResult result in mySearcher.FindAll())
                {

                    String name = result.GetDirectoryEntry().Name;
                    String addName = name.Substring(name.IndexOf('=') + 1);
                    bool canDisplay = false;
                    int j;
                    for (j = 0; j < dcList.Count; j++)
                    {
                        if (addName.ToLower().Equals(dcList[j].ToLower()))
                            break;
                    }
                    if (j == dcList.Count)
                        canDisplay = true;
                    if (i < 4 && canDisplay == true)
                    {
                        StartList.Add(addName);
                        i++;
                    }
                    else if ((i - 1) > 4)
                        break;
                }
                Dictionary<String, int> uniqueStore = new Dictionary<String, int>();

                StartList = StartList.ConvertAll(d => d.ToUpper());
                foreach (string currValue in StartList)
                {
                    if (!uniqueStore.ContainsKey(currValue))
                    {
                        uniqueStore.Add(currValue, 0);
                        final_List.Add(currValue);
                    }
                }
                for (int ind = 0; ind < final_List.Count; ind++)
                    ccbox.Items.Add(new CCBoxItem(final_List[ind], 0));
                ccbox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ccbox_ItemCheck);
                ccbox.MaxDropDownItems = 6;
                ccbox.DisplayMember = "Name";
                ccbox.ValueSeparator = ", ";
                ccbox.SetItemChecked(0, true);
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show("  The network path was not found ");
                //label22.Text = "";
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // label22.Text = "";
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Unspecified Error");
                // label22.Text = "";
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure . Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //  label22.Text = "";
                }
                else
                {
                    MessageBox.Show("Unspecified Error");
                    // label22.Text = "";
                }
            }
        }
        public void update(string message)
        {
            Constants.computerList.Clear();
            
            addMultipleComputers.FormClosed += AddMultipleComputersClosed;
            addMultipleComputers.ShowDialog();
        }
        public void AddCurrentDomain()
        {
            try
            {
                System.DirectoryServices.ActiveDirectory.Forest currentForest = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();
                System.DirectoryServices.ActiveDirectory.DomainCollection dc = currentForest.Domains;
                foreach (System.DirectoryServices.ActiveDirectory.Domain d in dc)
                {
                comboBox1.Items.Add(d.Name);              
                }
                comboBox1.SelectedItem = comboBox1.Items[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while getting Domain details", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CheckBox chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "ToSelectAllMSA";
            chkBox.Size = new System.Drawing.Size(13, 13);
            chkBox.Location = new System.Drawing.Point(5, 2);
            chkBox.CheckedChanged += SelectAllMSA;
            listView1.Controls.Add(chkBox);

            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllMSA"];
            cb.Checked = true;
            cb.Checked = false;
            listView1.Sorting = SortOrder.Ascending;
            resultdc = "";
            string[] dcArray = comboBox1.Text.Split(new char[] { '.' });
            int counter = 0;
            foreach (string dc in dcArray)
            {
                resultdc = resultdc + "DC=" + dc;
                counter++;
                if (dcArray.Length != counter)
                    resultdc = resultdc + ",";

            }
        }
        public void recolour1()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
        }
        public void recolour2()
        {
            for (int item = 0; item < listView2.Items.Count; ++item)
            {
                var items = listView2.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            listView1.Items.Clear();
            string[] dcArray = comboBox1.Text.Split(new char[] { '.' });
            int counter = 0;
            resultdc = "";
            foreach (string dc in dcArray)
            {
                resultdc = resultdc + "DC=" + dc;
                counter++;
                if (dcArray.Length != counter)
                    resultdc = resultdc + ",";

            }
            if (comboBox1.Text.Trim().Length == 0)
            {
                MessageBox.Show("Cannot complete search without domain name..", " Error ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DirectoryEntry dir = new DirectoryEntry("LDAP://" + resultdc);
            if (!defaultUser)
            {
                try
                {
                    using (new Impersonator(UserName, DomainName, Password))
                    {
                        try
                        {

                            DirectorySearcher searcher = new DirectorySearcher();
                            searcher.PageSize = 1000;
                            searcher.SearchRoot = dir;

                            // searcher.Filter = "(&(&(objectClass=user)(objectClass=person))(|(givenName=" + userName + "*)                    (userPrincipalName=" + userName + "*)(sn=" + userName + "*)(sAMAccountName=" + userName + "*)                (displayName=" + userName + "*)))";
                            searcher.Filter = "(&(|(&(objectClass=computer)(objectCategory=msDs-GroupManagedServiceAccount))(&(objectClass=computer)(objectCategory=msDs-ManagedServiceAccount)))(|(givenName=*)(userPrincipalName=*)(sn=*)(sAMAccountName=*)(displayName=*)))";
                            searcher.SearchScope = SearchScope.Subtree;
                            searcher.PropertiesToLoad.Add("cn");
                            searcher.PropertiesToLoad.Add("displayName");
                            searcher.PropertiesToLoad.Add("Description");
                            searcher.PropertiesToLoad.Add("sAMAccountName");
                            searcher.PropertiesToLoad.Add("distinguishedName");
                            searcher.PropertiesToLoad.Add("msDs-HostServiceAccountBL");
                            searcher.PropertiesToLoad.Add("userPrincipalName");
                            searcher.Sort = new SortOption("cn", SortDirection.Ascending);
                            SearchResultCollection results = searcher.FindAll();
                            string hostcomputers = "";
                            if (results.Count > 0)
                            {
                                foreach (SearchResult result in results)
                                {


                                    if (result.Properties["cn"].Count > 0)
                                        listView1.Items.Add("" + result.Properties["cn"][0]);
                                    else
                                        listView1.Items.Add("-");

                                    listView1.Items[listView1.Items.Count - 1].SubItems.Add("" + result.Properties["sAMAccountName"][0]);
                                    if (result.Properties["Description"].Count > 0)
                                        listView1.Items[listView1.Items.Count - 1].SubItems.Add("" + result.Properties["Description"][0]);
                                    else
                                        listView1.Items[listView1.Items.Count - 1].SubItems.Add("");
                                    if (result.Properties["msDS-HostServiceAccountBL"].Count > 0)
                                    {
                                        for (int i = 0; i < result.Properties["msDS-HostServiceAccountBL"].Count; i++)
                                        {
                                            string cn = result.Properties["msDs-HostServiceAccountBL"][i].ToString();
                                            int startIndex = cn.IndexOf("=");
                                            int endIndex = cn.IndexOf(",");
                                            if (endIndex - startIndex > 1)
                                                cn = cn.Substring(startIndex + 1, endIndex - startIndex - 1);
                                            hostcomputers = hostcomputers + cn + ";";
                                        }
                                        listView1.Items[listView1.Items.Count - 1].SubItems.Add(hostcomputers.TrimEnd(hostcomputers[hostcomputers.Length - 1]));
                                        hostcomputers = "";
                                    }
                                    else
                                        listView1.Items[listView1.Items.Count - 1].SubItems.Add("");


                                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, comboBox1.SelectedItem.ToString()))
                                    {
                                        if (context == null)
                                        {
                                            throw new ApplicationException("Domain not found.");
                                        }
                                        ComputerPrincipal TargetComputer = ComputerPrincipal.FindByIdentity(context, result.Properties["cn"][0].ToString());
                                        if (TargetComputer.Enabled == true)
                                            listView1.Items[listView1.Items.Count - 1].SubItems.Add("Enabled");
                                        else
                                            listView1.Items[listView1.Items.Count - 1].SubItems.Add("Disabled");
                                    }

                                }
                            }

                            else
                                MessageBox.Show("No Managed service accounts available", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Domain can't be contacted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception eq)
                {
                    MessageBox.Show(eq.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else 
            {
                try
                {

                    DirectorySearcher searcher = new DirectorySearcher();
                    searcher.PageSize = 1000;
                    searcher.SearchRoot = dir;

                    // searcher.Filter = "(&(&(objectClass=user)(objectClass=person))(|(givenName=" + userName + "*)                    (userPrincipalName=" + userName + "*)(sn=" + userName + "*)(sAMAccountName=" + userName + "*)                (displayName=" + userName + "*)))";
                    searcher.Filter = "(&(|(&(objectClass=computer)(objectCategory=msDs-GroupManagedServiceAccount))(&(objectClass=computer)(objectCategory=msDs-ManagedServiceAccount)))(|(givenName=*)(userPrincipalName=*)(sn=*)(sAMAccountName=*)(displayName=*)))";
                    searcher.SearchScope = SearchScope.Subtree;
                    searcher.PropertiesToLoad.Add("cn");
                    searcher.PropertiesToLoad.Add("displayName");
                    searcher.PropertiesToLoad.Add("Description");
                    searcher.PropertiesToLoad.Add("sAMAccountName");
                    searcher.PropertiesToLoad.Add("distinguishedName");
                    searcher.PropertiesToLoad.Add("msDs-HostServiceAccountBL");
                    searcher.PropertiesToLoad.Add("userPrincipalName");
                    searcher.Sort = new SortOption("cn", SortDirection.Ascending);
                    SearchResultCollection results = searcher.FindAll();
                    string hostcomputers = "";
                    if (results.Count > 0)
                    {
                        foreach (SearchResult result in results)
                        {


                            if (result.Properties["cn"].Count > 0)
                                listView1.Items.Add("" + result.Properties["cn"][0]);
                            else
                                listView1.Items.Add("-");

                            listView1.Items[listView1.Items.Count - 1].SubItems.Add("" + result.Properties["sAMAccountName"][0]);
                            if (result.Properties["Description"].Count > 0)
                                listView1.Items[listView1.Items.Count - 1].SubItems.Add("" + result.Properties["Description"][0]);
                            else
                                listView1.Items[listView1.Items.Count - 1].SubItems.Add("");
                            if (result.Properties["msDS-HostServiceAccountBL"].Count > 0)
                            {
                                for (int i = 0; i < result.Properties["msDS-HostServiceAccountBL"].Count; i++)
                                {
                                    string cn = result.Properties["msDs-HostServiceAccountBL"][i].ToString();
                                    int startIndex = cn.IndexOf("=");
                                    int endIndex = cn.IndexOf(",");
                                    if (endIndex - startIndex > 1)
                                        cn = cn.Substring(startIndex + 1, endIndex - startIndex - 1);
                                    hostcomputers = hostcomputers + cn + ";";
                                }
                                listView1.Items[listView1.Items.Count - 1].SubItems.Add(hostcomputers.TrimEnd(hostcomputers[hostcomputers.Length - 1]));
                                hostcomputers = "";
                            }
                            else
                                listView1.Items[listView1.Items.Count - 1].SubItems.Add("");


                            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, comboBox1.SelectedItem.ToString()))
                            {
                                if (context == null)
                                {
                                    throw new ApplicationException("Domain not found.");
                                }
                                ComputerPrincipal TargetComputer = ComputerPrincipal.FindByIdentity(context, result.Properties["cn"][0].ToString());
                                if (TargetComputer.Enabled == true)
                                    listView1.Items[listView1.Items.Count - 1].SubItems.Add("Enabled");
                                else
                                    listView1.Items[listView1.Items.Count - 1].SubItems.Add("Disabled");
                            }

                        }
                    }

                    else
                        MessageBox.Show("No Managed service accounts available", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Domain can't be contacted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
            cl.executeDll(25);
            recolour1();
        }

        private void button4_Click(object sender, EventArgs e)
        {
                     
            List.Clear();
            finalList.Clear();
            computersList.Clear();
            errorcomputers = "";
            button6.Visible = true;
            
            linkLabel3.Visible = false;
            label6.Visible = false;

            listView2.Items.Clear();
            ccbox_DropDownClosed(sender, e);
            if (Constants.computerList.Count == 0)
            {
                MessageBox.Show("Please select atleast one Computer", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                button4.Enabled = true;
                button6.Visible = false;
                return;
            }
            else
            {

                Constants.computerList = Constants.computerList.ConvertAll(d => d.ToUpper());
                Dictionary<String, int> uniqueStore = new Dictionary<String, int>();
                List<String> final = new List<String>();

                foreach (string currValue in Constants.computerList)
                {
                    if (!uniqueStore.ContainsKey(currValue))
                    {
                        uniqueStore.Add(currValue, 0);
                        final.Add(currValue);
                    }
                }
                
                    backgroundWorker2.RunWorkerAsync(final);
                    ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                    cl.executeDll(26);               
            }           
        }

        private void button3_Click(object sender, EventArgs e)
        {

           S_ServiceList.Clear();
           S_ServiceAccountList.Clear();
           S_ComputerList.Clear();
            
           button5.Visible = true;
           button3.Enabled = false;

           if (listView2.CheckedItems.Count == 0)
           {
               MessageBox.Show("Please select atleast one Service Account", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               button5.Visible = false;
               button3.Enabled = true;
           }
           else
               backgroundWorker1.RunWorkerAsync();
               ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
               cl.executeDll(27);  
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {

            try
            {
                if (this.backgroundWorker1.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                for (int i = 0; i < listView2.CheckedItems.Count; i++)
                {

                    if (this.backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }
                    backgroundWorker1.ReportProgress(1, "Retrieving Services .. Please Wait ..");
                    ListViewItem item = listView2.CheckedItems[i];
                    ServiceController[] services = ServiceController.GetServices(item.SubItems[3].Text);
                    backgroundWorker1.ReportProgress(1, "Retrieving Services for " + listView2.CheckedItems[i].Text + " in " + item.SubItems[3].Text + " ..");
                    foreach (ServiceController service in services)
                    {
                        if (this.backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        ManagementScope scope = new ManagementScope("\\\\" + item.SubItems[3].Text + "\\root\\cimv2");
                        ManagementPath path = new ManagementPath("Win32_Service.Name='" + service.ServiceName + "'");
                        ObjectGetOptions opt = new ObjectGetOptions(null, TimeSpan.MaxValue, true);
                        ManagementObject wmiService;
                        wmiService = new ManagementObject(scope, path, opt);
                        wmiService.Get();
                        string compare;
                        if (wmiService["StartName"] != null)
                        {
                            compare = wmiService["StartName"].ToString();
                            if (compare.Contains('@'))
                            {
                                string user = compare.Split('@', '.')[0];
                                string domain = compare.Split('@', '.')[1];
                                compare = domain + '\\' + user;
                            }
                        }
                        else
                            compare = null;
                        // if (wmiService["StartName"] != null && wmiService["StartName"].ToString().ToLower().Equals(item.Text.ToLower()))
                        if (compare != null && String.Equals(Regex.Replace(compare.ToString(), @"\s+", String.Empty), Regex.Replace(item.Text, @"\s+", String.Empty), StringComparison.OrdinalIgnoreCase))
                        {
                            // if(itemflag)
                            S_ServiceAccountList.Add(item.Text);
                            //  else
                            //  ServiceAccountList.Add(' ');
                            //  itemflag = false;
                            S_ComputerList.Add(item.SubItems[3].Text);
                            S_ServiceList.Add(service.DisplayName);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button3.Enabled = true;
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {


           if (e.ProgressPercentage == 1)
           {
               label5.Text = e.UserState.ToString();
               label5.Refresh();            
           } 
        }
        private void SelectAllComputers(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView2.Controls["ToSelectAllComputers"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = false;

        }
        private void SelectAllMSA(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllMSA"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

                       button5.Visible = false;
                       label5.Text = "";
                       button3.Enabled = true;
                       if (S_ServiceAccountList.Count > 0)
                       {
                           AssociatedServices associatedservices = new AssociatedServices();
                           associatedservices.ShowDialog();
                       }
                       else
                       {
                           MessageBox.Show("No Service associated with selected Service Account(s)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                           return;
                       }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Credentials credform = new Credentials();
            credform.ShowDialog();
            defaultUser = credform.defaultuser;
            if (!defaultUser)
            {
                try
                {
                    String[] parts = credform.username.Split(new[] { '\\' });
                    DomainName = parts[0];
                    UserName = parts[1];

                }
                catch (Exception eq)
                { 
                    DomainName = "";
                    UserName = "User Name";
                }
            }
            Password = credform.password;            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewAccount newform = new NewAccount();
            SelectedDomain = comboBox1.Text;
            newform.ShowDialog();
            if (NewAccount.isadded)
            {
                button1_Click(sender, e);
                NewAccount.isadded = false;
            }
            
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Credentials credform = new Credentials();
            credform.ShowDialog();
            defaultUser = credform.defaultuser;
            if (!defaultUser)
            {
                try
                {
                    String[] parts = credform.username.Split(new[] { '\\' });
                    DomainName = parts[0];
                    UserName = parts[1];

                }
                catch (Exception eq)
                {
                    DomainName = "";
                    UserName = "User Name";
                }
            }
            Password = credform.password;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            button10.Visible = false;
            button8.Visible = true;

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listView2_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
           
        }

        private void listView2_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            
        }

        private void listView1_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
           
        }

        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.ForeColor == Color.Gray)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
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

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                pictureBox3_Click(sender, e);
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.ForeColor == Color.Gray)
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                textBox2.ForeColor = Color.Gray;
                textBox2.Text = "Search";
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                pictureBox4_Click(sender, e);
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            button5.Visible = false;
            recolour2();
        }
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (this.backgroundWorker2.CancellationPending)
            {
                e.Cancel = true;
                return;
            }
            backgroundWorker2.ReportProgress(1,"Retrieving Service accounts .. Please Wait");
            

            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;
            Constants.computerList.Clear();
            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;
            for (int i = 0; i < coll.Count; i++)
            {

                //ListViewItem item = listView2.CheckedItems[i];
                
                tempBGWorker.ReportProgress(1, "Retrieving accounts from " + coll[i]);
                
                List.Clear();
                if (tempBGWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }
                if (defaultUser)
                {
                    try
                    {
                        using (ServiceAccountManagement.LocalSecurityAuthority.LsaWrapper lsa = new ServiceAccountManagement.LocalSecurityAuthority.LsaWrapper(coll[i]))
                        {
                            tempBGWorker.ReportProgress(1, "Collecting Information from " + coll[i]);
                            List = lsa.ReadPrivilege("SeServiceLogonRight");
                        }
                    }
                    catch (Exception eu)
                    {
                        tempBGWorker.ReportProgress(1, "Error occured in " + coll[i]);
                        MessageBox.Show("Following error occured in " + coll[i] + ":\n" + eu.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorcomputers += coll[i] + ",";
                    }
                }
                else
                {
                    try
                    {
                        using (new Impersonator(UserName, DomainName, Password))
                        {
                            try
                            {
                                using (ServiceAccountManagement.LocalSecurityAuthority.LsaWrapper lsa = new ServiceAccountManagement.LocalSecurityAuthority.LsaWrapper(coll[i]))
                                {
                                    tempBGWorker.ReportProgress(1, "Collecting Information from " + coll[i]);
                                    List = lsa.ReadPrivilege("SeServiceLogonRight");
                                }
                            }
                            catch (Exception eu)
                            {
                                tempBGWorker.ReportProgress(1, "Error occured in" + coll[i]);
                                MessageBox.Show("Following error occured in " + coll[i] + ":\n" + eu.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                errorcomputers += coll[i] + ",";
                            }
                        }
                    }
                    catch (Exception eq)
                    {
                        tempBGWorker.ReportProgress(1, "Error occured in" + coll[i]);
                        MessageBox.Show("Following error occured in " + coll[i] + ":\n" + eq.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        errorcomputers += coll[i] + ",";
                    }
                }
                for (int j = 0; j < List.Count; j++)
                {
                    StringBuilder name = new StringBuilder();
                    uint cchName = (uint)name.Capacity;
                    StringBuilder referencedDomainName = new StringBuilder();
                    uint cchReferencedDomainName = (uint)referencedDomainName.Capacity;
                    SID_NAME_USE sidUse;
                    var Sid = new SecurityIdentifier(List[j].ToString());
                    byte[] bytes = new byte[Sid.BinaryLength];
                    Sid.GetBinaryForm(bytes, 0);
                    int err = NO_ERROR;
                    if (!LookupAccountSid(coll[i], bytes, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                    {
                        err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        if (err == ERROR_INSUFFICIENT_BUFFER)
                        {
                            name.EnsureCapacity((int)cchName);
                            referencedDomainName.EnsureCapacity((int)cchReferencedDomainName);
                            err = NO_ERROR;
                            if (!LookupAccountSid(coll[i], bytes, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                                err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        }
                    }
                    if (err == 0)
                    {
                        if (referencedDomainName.ToString().ToLower().Equals(coll[i].ToLower()))
                        finalList.Add("."+ "\\" + name.ToString());
                        else
                        finalList.Add(referencedDomainName.ToString() + "\\" + name.ToString());
                        
                        computersList.Add(coll[i]);
                    }
                }


            }
            //label4.Text = "";
           
        }
        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 1)
            {
                label5.Text = e.UserState.ToString();
                label5.Refresh();
            }
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            button6.Visible = false;
         //   backgroundWorker2.ReportProgress(1, "Collecting information .. Please wait ..");
            if (finalList.Count > 0)
            {
                listView2.Items.Clear();
                NameList.Clear();
                DescriptionList.Clear();
                TypeList.Clear();
                MachineList.Clear();
                int i = 0;
                if (!string.IsNullOrEmpty(errorcomputers))
                {
                    label6.Visible = true;
                    linkLabel3.Visible = true;
                }
                foreach (string str in Form1.finalList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = str;
                    NameList.Add(str);
                 
                        
                        PrincipalContext ctx = new PrincipalContext(ContextType.Domain,S_SelectedDomain);
                        Principal myObject = Principal.FindByIdentity(ctx, item.Text);
                        if (myObject is UserPrincipal && item.Text[0]!='.')
                        {
                            item.SubItems.Add(myObject.Description);
                            DescriptionList.Add(myObject.Description);
                            item.SubItems.Add("Domain User");
                            TypeList.Add("Domain User");
                        }
                        else if (myObject is GroupPrincipal && item.Text[0]!='.')
                        {

                            item.SubItems.Add(myObject.Description);
                            DescriptionList.Add(myObject.Description);
                            item.SubItems.Add("Domain Group");
                            TypeList.Add("Domain Group");
                        }
                        else if (myObject != null && item.Text[0] != '.')
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
                    listView2.Items.Add(item);
                    i++;
                }
                CheckBox chkBox = new CheckBox();
                chkBox.Checked = false;
                chkBox.Name = "ToSelectAllComputers";
                chkBox.Size = new System.Drawing.Size(13, 13);
                chkBox.Location = new System.Drawing.Point(5, 2);
                chkBox.CheckedChanged += SelectAllComputers;
                listView2.Controls.Add(chkBox);
                label5.Text = "";
                CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
                recolour2();
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            backgroundWorker2.ReportProgress(1, "Process about to Stop ..");
            button6.Visible = false;
            backgroundWorker2.CancelAsync();
            backgroundWorker2.ReportProgress(1, "Collecting Information ..");
        }
        private void ccbox_DropDownClosed(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder("Items checked: ");
            foreach (CCBoxItem item in ccbox.CheckedItems)
            {
                Constants.computerList.Add(item.Name);
                sb.Append(item.Name).Append(ccbox.ValueSeparator);
            }
            //Constants.computerList.RemoveAt(Constants.computerList.Count-1);
            sb.Remove(sb.Length - ccbox.ValueSeparator.Length, ccbox.ValueSeparator.Length);
        }



        private void ccbox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            Constants.computerList.Clear();
            CCBoxItem item = ccbox.Items[e.Index] as CCBoxItem;

        }

        private void ccbox_DropDown(object sender, EventArgs e)
        {


        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Error Occured computers : \n" + Form1.errorcomputers, "Error Computers List");
        }
        public void AddMultipleComputersClosed(object sender, EventArgs e)
        {
            if (addMultipleComputers.fav_set)
            {
                ccbox.Items.Clear();
                Constants.computerList.Clear();
                CheckComboData();
                addMultipleComputers.fav_set = false;
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
          
        }

        
        private void allAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allAccountsToolStripMenuItem.Image = global::ServiceAccountManagement.Properties.Resources.correct;
            onlyUsersToolStripMenuItem.Image = null;
            onlyGroupsToolStripMenuItem.Image = null;
            listView2.Items.Clear();
            for (int i = 0; i < NameList.Count; i++)
            {
                ListViewItem item = new ListViewItem(NameList[i]);
                item.SubItems.Add(DescriptionList[i]);
                item.SubItems.Add(TypeList[i]);
                item.SubItems.Add(MachineList[i]);
                listView2.Items.Add(item);
            }
            recolour2();
            button11.Visible = false;
            button12.Visible = true;
        }

        private void onlyUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            allAccountsToolStripMenuItem.Image = null;
            onlyUsersToolStripMenuItem.Image = global::ServiceAccountManagement.Properties.Resources.correct;
            onlyGroupsToolStripMenuItem.Image = null;
            listView2.Items.Clear();
            for (int i = 0; i < NameList.Count; i++)
            {
                if (TypeList[i].Equals("Domain User") || TypeList[i].Equals("Local User"))
                {
                    ListViewItem item = new ListViewItem(NameList[i]);
                    item.SubItems.Add(DescriptionList[i]);
                    item.SubItems.Add(TypeList[i]);
                    item.SubItems.Add(MachineList[i]);
                    listView2.Items.Add(item);
                }

            }
            recolour2();
            button11.Visible = false;
            button12.Visible = true;
        } 
        
    private void onlyGroupsToolStripMenuItem_Click(object sender, EventArgs e)
    {
        onlyUsersToolStripMenuItem.Image = null;
        allAccountsToolStripMenuItem.Image = null;
        onlyGroupsToolStripMenuItem.Image = global::ServiceAccountManagement.Properties.Resources.correct;
        listView2.Items.Clear();
        for (int i = 0; i < NameList.Count; i++)
        {
            if (TypeList[i].Equals("Domain Group") || TypeList[i].Equals("Local Group"))
            {
                ListViewItem item = new ListViewItem(NameList[i]);
                item.SubItems.Add(DescriptionList[i]);
                item.SubItems.Add(TypeList[i]);
                item.SubItems.Add(MachineList[i]);
                listView2.Items.Add(item);
            }

        }
        recolour2();
        button11.Visible = false;
        button12.Visible = true;
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

    private void listView2_DrawItem(object sender, DrawListViewItemEventArgs e)
    {
        e.DrawDefault = true;
    }

    private void listView2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
    {
        e.DrawDefault = true;
    }

    private void listView2_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
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

    private void button9_Click(object sender, EventArgs e)
    {
        bool isdeleted = false;
        ListView.CheckedListViewItemCollection collection = listView1.CheckedItems;

        if (collection.Count < 1)
        {
            MessageBox.Show("Please select atleast one account", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        else
        {
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to delete ?", "Confirm Delete", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
                return;

            for (int i = 0; i < collection.Count; i++)
            {
                ListViewItem item = collection[i];
                try
                {
                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain, comboBox1.Text);

                    // find the computer in question
                    ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, item.Text);

                    // if found - delete it
                    if (defaultUser)
                    {
                        if (computer != null)
                        {
                            computer.Delete();
                            isdeleted = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            using (new Impersonator(UserName, DomainName, Password))
                            {
                                if (computer != null)
                                {
                                    computer.Delete();
                                    isdeleted = true;
                                }

                            }
                        }
                        catch (Exception ew)
                        {
                            MessageBox.Show(ew.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            if (isdeleted)
                MessageBox.Show("Managed Service Account(s) are Removed Succesfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        button1_Click(sender, e);
    }

    private void button7_Click(object sender, EventArgs e)
    {
        if (listView1.CheckedItems.Count == 0)
            MessageBox.Show("Please select atleast one account", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        else if (listView1.CheckedItems.Count > 1)
            MessageBox.Show("Please select one account", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        else
        {
            ListViewItem item = listView1.CheckedItems[0];

            EditServiceAccount editform = new EditServiceAccount();
            editform.name = item.Text;
            editform.samaccountname = item.SubItems[1].Text;
            editform.description = item.SubItems[2].Text;
            editform.hostcomputers = item.SubItems[3].Text;
            try
            {
                PrincipalContext ctx = new PrincipalContext(ContextType.Domain, comboBox1.Text);
                ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, item.Text);
                if (computer != null)
                {
                    editform.distinguishedname = computer.DistinguishedName;
                    editform.guid = computer.Guid.ToString();
                    if (computer.Enabled == true)
                        editform.isenabled = true;
                    else
                        editform.isenabled = false;
                }
                else
                {
                    MessageBox.Show("Unspecified Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ez)
            {
                MessageBox.Show(ez.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            editform.ShowDialog();
            if (editform.isedited)
            {
                try
                {
                    using (PrincipalContext context = new PrincipalContext(ContextType.Domain, comboBox1.Text))
                    {
                        if (context == null)
                        {
                            throw new ApplicationException("Domain not found.");
                        }
                        using (ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(context, item.Text))
                        {
                            if (computer == null)
                            {
                                throw new ApplicationException("Computer not found.");
                            }
                            using (DirectoryEntry entry = (DirectoryEntry)computer.GetUnderlyingObject())
                            {
                                if (defaultUser)
                                {
                                    entry.Rename("CN=" + editform.name);
                                    entry.InvokeSet("sAMAccountName", editform.samaccountname);
                                    if (!string.IsNullOrWhiteSpace(editform.description))
                                        entry.InvokeSet("description", editform.description);
                                    else
                                        entry.Properties["description"].Value = null;
                                    entry.CommitChanges();
                                }
                                else
                                {
                                    try
                                    {
                                        using (new Impersonator(UserName, DomainName, Password))
                                        {
                                            entry.Rename("CN=" + editform.name);
                                            entry.InvokeSet("sAMAccountName", editform.samaccountname);
                                            if (!string.IsNullOrWhiteSpace(editform.description))
                                                entry.InvokeSet("description", editform.description);
                                            else
                                                entry.Properties["description"].Value = null;
                                            entry.CommitChanges();

                                        }
                                    }
                                    catch (Exception ew)
                                    {
                                        MessageBox.Show(ew.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }

                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain, comboBox1.Text);
                    ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, editform.name);
                    if (computer != null)
                    {
                        if (editform.isenabled)
                            computer.Enabled = true;
                        else
                            computer.Enabled = false;
                        computer.Save();
                    }
                    else
                    {
                        MessageBox.Show("Unspecified Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ez)
                {
                    MessageBox.Show(ez.Message, "Information");
                }
                editform.isedited = false;
            }
            button1_Click(sender, e);
        }
    }

    private void button8_Click(object sender, EventArgs e)
    {
        if (textBox1.Text.Equals("") || textBox1.Text.Equals("Search"))
            button1_Click(sender, e);

        else
        {
            for (int i = listView1.Items.Count - 1; i >= 0; i--)
            {
                var item = listView1.Items[i];
                if (!(item.Text.ToLower().Contains(textBox1.Text.ToLower())))
                    listView1.Items.Remove(item);
            }
            recolour1();
            button10.Visible = true;
            button8.Visible = false;
        }
                

           
    }

    private void button10_Click(object sender, EventArgs e)
    {
        textBox1.Text = "";
        button8_Click(sender, e);
        button10.Visible = false;
        button8.Visible = true;
    }

    private void button11_Click(object sender, EventArgs e)
    {
        textBox2.Text = "";
        button12_Click(sender, e);
        button11.Visible = false;
        button12.Visible = true;
    }

    private void button12_Click(object sender, EventArgs e)
    {
        if (textBox2.Text.Equals("") || textBox2.Text.Equals("Search"))
        {
            listView2.Items.Clear();
            for (int i = 0; i < NameList.Count; i++)
            {
                ListViewItem item = new ListViewItem(NameList[i]);
                item.SubItems.Add(DescriptionList[i]);
                item.SubItems.Add(TypeList[i]);
                item.SubItems.Add(MachineList[i]);
                listView2.Items.Add(item);
            }
        }


        else
        {
            for (int i = listView2.Items.Count - 1; i >= 0; i--)
            {
                var item = listView2.Items[i];
                if (!(item.Text.ToLower().Contains(textBox2.Text.ToLower())))
                    listView2.Items.Remove(item);
            }

            button11.Visible = true;
            button12.Visible = false;
        }
        recolour2(); 
    }

    private void button13_Click(object sender, EventArgs e)
    {
        if (NameList.Count < 1)
        {
            MessageBox.Show("No Data to Filter", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        else
        {
            Button btnSender = (Button)sender;
            Point ptLowerLeft = new Point(-112, btnSender.Height - 2);
            ptLowerLeft = btnSender.PointToScreen(ptLowerLeft);
            contextMenuStrip1.Show(ptLowerLeft);
        }
    }

    private void button14_Click(object sender, EventArgs e)
    {
        if (listView2.Items.Count < 1)
        {
            MessageBox.Show("No Data To Export", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                string[] headers = listView2.Columns
                           .OfType<ColumnHeader>()
                           .Select(header => header.Text.Trim())
                           .ToArray();

                string[][] items = listView2.Items
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

    private void pictureBox2_Click_1(object sender, EventArgs e)
    {
        this.Close();
    }
     
        }
        }
