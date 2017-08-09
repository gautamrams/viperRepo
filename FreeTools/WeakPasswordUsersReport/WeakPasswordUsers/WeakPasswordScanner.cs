using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Security;
using DSInternals;
using DSInternals.Common;
using DSInternals.Common.Cryptography;
using DSInternals.Common.Data;
using DSInternals.Common.Interop;
using DSInternals.Replication;
using DSInternals.Replication.Model;
using ActiveDs;
using WeakPasswordUsers;
namespace weak_password_finder
{

    public partial class Form1 : Form
    {

        public int userAccountsCount = 0;
        public String password_dictionary_path = "PasswordPatterns.txt";
        public String domain_name = null;
        public String domain_controller = null;
        public String user_name = null;
        public String password = null;
        public String DomainDistinguishedName;
        public String log_path;
        public String fileName;
        public String connectedDomain="";
        public NetworkCredential admin;
        public bool weakness_found = false;
        public int data_grid_index = 0;
        public String EmptyHashNt = null;
        Dictionary<String, accountDetails[]> hashmap = new Dictionary<String, accountDetails[]>();
        Attributes attribute = new Attributes();
        //accountDetails - To store User account details obtained from ReplicateUserDetails()
        public struct accountDetails
        {
           
            public String samAccountName;
            public String userPrincipalName;
            public String DistinguishedName;
            public String AccountStatus;
            public bool has_weak_password;
            public String Department;
            public String PasswordExpiryDate;
            public String AccountExpiryDate;
            public bool EmptyPassword;
            public String Description;
            public String EmailAddress;
            public String EmployeeID;
            public String FirstName;
            public String FullName;
            public String HomePage;
            public String NtHash;
            public void accout_details()
            {
                this.samAccountName = "";
                this.userPrincipalName = "";
                this.Department = "";
                this.DistinguishedName = "";
                this.AccountStatus = "";
                this.PasswordExpiryDate = "";
                this.AccountExpiryDate = "";
                this.Description = "";
                this.EmailAddress = "";
                this.EmployeeID = "";
                this.FirstName = "";
                this.FullName = "";
                this.HomePage = "";
                this.EmptyPassword = false;
            }
            public String getData(String param)
            {
                if (param == "Logon Name")
                    return samAccountName;
                else if (param == "Department")
                    return Department;
                else if (param == "Distinguished Name")
                    return DistinguishedName;
                else if (param == "Account Status")
                    return AccountStatus;
                else if (param == "Password Expiry Date")
                    return PasswordExpiryDate;
                else if (param == "Account Expiry Date")
                    return AccountExpiryDate;
                else if (param == "Empty Password")
                    return EmptyPassword.ToString();
                else if (param == "User Principal Name")
                    return userPrincipalName;
                else if (param == "Description")
                    return Description;
                else if (param == "Email Address")
                    return EmailAddress;
                else if (param == "Employee ID")
                    return EmployeeID;
                else if (param == "First Name")
                    return FirstName;
                else if (param == "Full Name")
                    return FullName;
                else if (param == "Home Page")
                    return HomePage;
                else
                    return "";
            }

        }

        public Form1()
        {
            ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
            cl.executeDll(28);
            InitializeComponent();
            this.dataGridView1.Rows.Add(18);
            HideProgressBar();
            //Calculating NTHash for empty string
            byte[] temp_hash = NTHash.ComputeHash("");
            for (int j = 0; j < 16; j++)
            {
                EmptyHashNt = EmptyHashNt + temp_hash[j].ToString();
            }

            foreach (object temp in attribute.listBox1.Items)
            {
                Array.Resize<String>(ref attribute.attributes_listbox1, attribute.attributes_listbox1.Length + 1);
                attribute.attributes_listbox1[attribute.attributes_listbox1.Length - 1] = temp.ToString();
            }
            foreach (object temp in attribute.listBox2.Items)
            {
                Array.Resize<String>(ref attribute.attributes_listbox2, attribute.attributes_listbox2.Length + 1);
                attribute.attributes_listbox2[attribute.attributes_listbox2.Length - 1] = temp.ToString();
            }
        }
        public void HideProgressBar()
        {
            this.panel2.Hide();
            panel3.Height = panel3.Height + panel3.Location.Y - panel1.Location.Y - 5 - panel1.Height;
            dataGridView1.Height = dataGridView1.Height + panel3.Location.Y - panel1.Location.Y - 5 - panel1.Height;
            panel3.Location = new Point(panel3.Location.X, panel1.Location.Y + panel1.Height);

        }
        private void ShowProgressBar()
        {
            panel2.Show();
            dataGridView1.Height = dataGridView1.Height - (panel2.Height + 5);
            panel3.Height = panel3.Height - (panel2.Height + 5);
            panel3.Location = new Point(panel3.Location.X, panel2.Location.Y + panel2.Height);

        }
        //Function for creating Logs
        private void AppendLogFile(String log_data)
        {
            using (StreamWriter file = File.AppendText(log_path + "./..//logs//WeakPasswordUsersReport_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
            {

                file.WriteLine(log_data);

            }

        }
        //To get connected domain
        private void GetDomain(bool status)
        {
            Domain domain;
            try
            {
                if (status)
                {
                    domain = Domain.GetCurrentDomain();
                    connectedDomain = domain.Name;
                    this.textBox1.Text = domain.Name;
                }
                else
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, domain_name));
                comboBox1.Items.Clear();
                foreach (DomainController dc in domain.DomainControllers)
                {
                    comboBox1.Items.Add(dc.Name);
                }
            }
            catch (Exception)
            {

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            log_path = new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath;
            //creating log file
            if (File.Exists(log_path + "./..//logs//WeakPasswordUsersReport_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
            {
                using (StreamWriter file = File.AppendText(log_path + "./..//logs//WeakPasswordUsersReport_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
                {
                    file.WriteLine("\r\n" + DateTime.Now.ToString() + "  " + "program loaded successfully.");
                }
            }
            else
            {
                using (StreamWriter file = File.CreateText(log_path + "./..//logs//WeakPasswordUsersReport_" + DateTime.Now.Month + "_" + DateTime.Now.Year + ".txt"))
                {
                    file.WriteLine(DateTime.Now.ToString() + "  " + "program loaded successfully.");
                }
            }
            GetDomain(true);
        }
        //Resets all variables for report generation
        private void reset()
        {
            data_grid_index = 0;
            userAccountsCount = 0;
            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(18);
            label4.Text = "Total number of users with weak password: " + data_grid_index.ToString();
            weakness_found = false;
            hashmap = new Dictionary<String, accountDetails[]>();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (this.weakness_found)
            {
                this.saveFileDialog1.Title = "Choose a location to save";
                this.saveFileDialog1.InitialDirectory = @"C:\";
                this.saveFileDialog1.DefaultExt = ".csv";
                this.saveFileDialog1.FileName = domain_name.ToUpper() + "_WeakPasswordUsersReport";
                this.saveFileDialog1.CheckPathExists = true;
                this.saveFileDialog1.ShowDialog();
            }

            else
            {
                MessageBox.Show("There is no data to export.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        //Generate Button
        private void button1_Click(object sender, EventArgs e)
        {
            reset();
            user_name = textBox2.Text;
            password = textBox3.Text;
            domain_controller = comboBox1.Text;
            domain_name = textBox1.Text;
            if (domain_name == "")
            {

                MessageBox.Show("Please enter a domain name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (domain_controller == "")
            {
                MessageBox.Show("Please select or enter a domain controller.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (user_name == "" || password == "")
            {

                MessageBox.Show("Please enter a valid username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (!backgroundWorker1.IsBusy)
                {
                    ShowProgressBar();
                    button1.Enabled = false;
                    button2.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();

                }

            }
        }

        private void ReplicateUserDetails()
        {

            admin = new NetworkCredential(user_name, password, string.Empty);
            String log_data = DateTime.Now.ToString() + "  Report Generation started with the details : Domain Name:" + domain_name + " Domain Controller:" + domain_controller + " User Name:" + user_name;
            AppendLogFile(log_data);
            try
            {
                DomainDistinguishedName = DSInternals.Common.Data.DistinguishedName.GetDNFromDNSName(domain_name).ToString();
                using (DirectoryReplicationClient client = new DirectoryReplicationClient(domain_controller, RpcProtocol.TCP, admin))
                {
                    using (var context = new PrincipalContext(ContextType.Domain, domain_controller, user_name, password))
                    {
                        using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                        {
                            int num = 0, num1 = 0, num2;
                            PrincipalSearchResult<Principal> res = searcher.FindAll();
                            int totalObjectCount = res.Count();
                            log_data = DateTime.Now.ToString() + "  Estimated User Accounts : " + totalObjectCount;
                            AppendLogFile(log_data);
                            label8.Text = "Fetching Data...";
                            this.backgroundWorker1.ReportProgress(0);
                            foreach (Principal result in res)
                            {
                                num++;
                                num2 = (int)(((double)num / (double)totalObjectCount) * 100);
                                if (num2 > num1)
                                {
                                    this.backgroundWorker1.ReportProgress(num2);
                                    num1 = num2;
                                }
                                DSAccount dsaccount = client.GetAccount(result.Sid); client.GetAccount(result.DistinguishedName);
                                String nthash = "";
                               
                                    Byte[] temp_hash = dsaccount.NTHash;
                                    try
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            nthash = nthash + temp_hash[j].ToString();

                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                
                                accountDetails[] accountdetails = new accountDetails[0];
                                if (hashmap.ContainsKey(nthash))
                                {
                                    accountdetails = hashmap[nthash];
                                    hashmap.Remove(nthash);
                                    Array.Resize<accountDetails>(ref accountdetails, accountdetails.Length + 1);

                                }
                                else
                                {
                                    Array.Resize<accountDetails>(ref accountdetails, accountdetails.Length + 1);

                                }
                                accountdetails[accountdetails.Length - 1].samAccountName = dsaccount.SamAccountName;
                                accountdetails[accountdetails.Length - 1].userPrincipalName = dsaccount.UserPrincipalName;
                                accountdetails[accountdetails.Length - 1].DistinguishedName = dsaccount.DistinguishedName;
                                accountdetails[accountdetails.Length - 1].NtHash = nthash;
                                if (dsaccount.Enabled)
                                    accountdetails[accountdetails.Length - 1].AccountStatus = "Enabled";
                                else
                                    accountdetails[accountdetails.Length - 1].AccountStatus = "Disabled";
                                userAccountsCount++;
                                DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                                accountdetails[accountdetails.Length - 1].AccountExpiryDate = "Never Expires";
                                accountdetails[accountdetails.Length - 1].PasswordExpiryDate = "Never Expires";
                                accountdetails[accountdetails.Length - 1].has_weak_password = false;
                                accountdetails[accountdetails.Length - 1].Department = "";
                                try
                                {
                                    DateTime temp = ((DateTime)de.InvokeGet("AccountExpirationDate"));
                                    if (temp.Year > 1970)
                                    {
                                        accountdetails[accountdetails.Length - 1].AccountExpiryDate = temp.ToString();
                                    }
                                    temp = ((DateTime)de.InvokeGet("PasswordExpirationDate"));
                                    if (temp.Year > 1970)
                                    {
                                        accountdetails[accountdetails.Length - 1].PasswordExpiryDate = temp.ToString();
                                    }
                                    accountdetails[accountdetails.Length - 1].samAccountName = result.SamAccountName;
                                    accountdetails[accountdetails.Length - 1].userPrincipalName = result.UserPrincipalName;
                                    accountdetails[accountdetails.Length - 1].DistinguishedName = result.DistinguishedName;
                                    foreach (String property in de.Properties.PropertyNames)
                                    {
                                        if (property == "department")
                                        {
                                            accountdetails[accountdetails.Length - 1].Department = de.Properties["department"].Value.ToString();
                                  
                                        }
                                        if(property=="description")
                                        {
                                            accountdetails[accountdetails.Length - 1].Description = de.Properties["description"].Value.ToString();
                                        }
                                        if(property=="mail")
                                        {
                                            accountdetails[accountdetails.Length - 1].EmailAddress = de.Properties["mail"].Value.ToString();
                                        }
                                        if(property=="employeeID")
                                        {
                                            accountdetails[accountdetails.Length - 1].EmployeeID = de.Properties["employeeID"].Value.ToString();
                                        }
                                        if (property == "givenName")
                                        {
                                            accountdetails[accountdetails.Length - 1].FirstName = de.Properties["givenName"].Value.ToString();
                                        }
                                        if (property == "displayName")
                                        {
                                            accountdetails[accountdetails.Length - 1].FullName = de.Properties["displayName"].Value.ToString();
                                        }
                                        if (property == "wWWHomePage")
                                        {
                                            accountdetails[accountdetails.Length - 1].HomePage = de.Properties["wWWHomePage"].Value.ToString();
                                        }
                                      
                                    }
                                    hashmap.Add(nthash, accountdetails);
                                }
                                catch (Exception e)
                                {
                                    hashmap.Add(nthash, accountdetails);
                                }
                            }
                        }
                    }
                }
                log_data = DateTime.Now.ToString() + "  Fetching Data from Active Directory Completed";
                AppendLogFile(log_data);
            }
            catch (Exception e)
            {

                String ErrorMessage = e.Message;
                if (e.Message == "The specified domain does not exist or cannot be contacted.")
                {
                    ErrorMessage = "The specified domain name does not exist or cannot be contacted right now.";
                }
                else if (e.Message == "Access is denied")
                {
                    ErrorMessage = "Access denied. Please make sure you have the Replicating Directory Changes permission or belong to the Domain Admins group.";
                }
                else if (e.Message == "The replication synchronization attempt failed because a master replica attempted to sync from a partial replica")
                {
                    ErrorMessage = "Access denied. Please make sure you entered child domain credentials.";
                }
                else if (e.Message == "The source server is currently rejecting replication requests")
                {
                    ErrorMessage = "The source server is currently rejecting replication requests. Please enable outbound replication in Domain Controller.\r\nDo you wish to know more ?";
                }
                else if (e.Message == "The RPC server is unavailable")
                {
                    try
                    {
                        using (var client = new WebClient())
                        using (var stream = client.OpenRead("http://www.google.com"))
                        {
                            ErrorMessage = "The server is not available.";
                        }
                    }
                    catch
                    {
                        ErrorMessage = "Please check your Network Connections.";
                    }
                }
                if (e.Message != "The source server is currently rejecting replication requests")
                    MessageBox.Show(ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    if (MessageBox.Show("The report could not be generated as outbound replication is disabled. \n\rDo you wish to enable outbound replication?", "Outbound replication disabled", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        Process.Start("IExplore.exe", log_path + "./../help/weak-password-users-report.html");
                    }
                }


                log_data = DateTime.Now.ToString() + "   Display Message:" + ErrorMessage + "\r\n  Error Message: " + e.Message + "\r\n  StackTrace" + e.StackTrace.ToString();
                AppendLogFile(log_data);
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(30);
            }

        }

        private void print_to_grid( String i)
        {
            foreach (accountDetails temp in hashmap[i])
            {
                data_grid_index++;
                this.label4.Text = "Total number of users with weak password: " + data_grid_index.ToString();
                if (dataGridView1.Rows.Count < data_grid_index)
                {
                    dataGridView1.Rows.Add();
                }
                for (int x = 0; x < attribute.attributes_listbox2.Length; x++)
                {
                    dataGridView1.Rows[data_grid_index - 1].Cells[x].Value = temp.getData(attribute.attributes_listbox2[x]);
                }
            }
        }
        private void weak_password_scanner()
        {
            label8.Text = "Scanning...";
            this.backgroundWorker1.ReportProgress(0);
            int num = 0;
            if (!File.Exists(password_dictionary_path))
            {
                MessageBox.Show("PasswordPatterns file not found in " + log_path + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                String log_data = DateTime.Now.ToString() + "  PasswordPatterns.txt file is not found in " + log_path;
                AppendLogFile(log_data);
                weakness_found = true;
            }
            else
            {
                using (StreamReader reader = File.OpenText(password_dictionary_path))
                {

                    long length = reader.BaseStream.Length;
                    long num1 = 0L;
                    String log_data = DateTime.Now.ToString() + "  Scan for Weak Password Users started";
                    AppendLogFile(log_data);
                    String s;
                    //Checks for empty password

                    if (hashmap.ContainsKey(EmptyHashNt))
                    {
                        weakness_found = true;
                        for (int i = 0; i < hashmap[EmptyHashNt].Length; i++)
                        {
                            hashmap[EmptyHashNt][i].has_weak_password = true;
                            hashmap[EmptyHashNt][i].EmptyPassword = true;
                            i++;
                        }
                        print_to_grid(EmptyHashNt);
                    }
                    while ((s = reader.ReadLine()) != null)
                    {
                        num1 += s.Length + 2;
                        byte[] temp_hash = NTHash.ComputeHash(s);
                        String hash_dictionary_nt = "";
                        for (int k = 0; k < 16; k++)
                        {
                            hash_dictionary_nt = hash_dictionary_nt + temp_hash[k].ToString();

                        }
                        //checks for weak password
                        if (hash_dictionary_nt != EmptyHashNt && hashmap.ContainsKey(hash_dictionary_nt))
                        {
                            weakness_found = true;
                            for (int i = 0; i < hashmap[hash_dictionary_nt].Length; i++)
                            {
                                hashmap[hash_dictionary_nt][i].has_weak_password = true;
                                i++;
                            }
                            print_to_grid(hash_dictionary_nt);
                        }
                        int num4 = (int)((num1 * 100) / ((double)length));
                        if (num4 > num)
                        {
                            if (num4 <= 100)
                                this.backgroundWorker1.ReportProgress(num4);
                            num = num4;
                        }
                    }
                }
            }
            this.backgroundWorker1.ReportProgress(100);
        }

        private void print_results()
        {

            if (!weakness_found)
            {
                MessageBox.Show("No Users with weak passwords", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                for (int x = 17; x >= data_grid_index; x--)
                {
                    dataGridView1.Rows.RemoveAt(x);
                }
            }
        }
        private void initiateScanner()
        {
            if (userAccountsCount != 0)
            {
                weak_password_scanner();
                HideProgressBar();
                print_results();
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(29);

                String log_data = DateTime.Now.ToString() + "  Report generated successfully, Total Users:" + userAccountsCount + " Total Users with weak password:" + data_grid_index;
                AppendLogFile(log_data);
            }
            else
            {
                HideProgressBar();
            }
            button1.Enabled = true; button2.Enabled = true;
            label8.Text = string.Format("Processing...");

            label7.Text = "";
            progressBar1.Value = 0;

        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ReplicateUserDetails();
            initiateScanner();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label7.Text = string.Format("{0}%", e.ProgressPercentage);
            progressBar1.Update();
        }
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            fileName = saveFileDialog1.FileName;
            label8.Text = "Exporting...";
            ShowProgressBar();
            button1.Enabled = false;
            button2.Enabled = false;
            if (!backgroundWorker4.IsBusy)
            {
                this.backgroundWorker4.RunWorkerAsync();
            }

        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            String log_data = DateTime.Now.ToString() + "  program terminated successfully.";

            AppendLogFile(log_data);
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", log_path + "./../help/weak-password-users-report.html");
        }

        private void button2_Click(object sender, EventArgs e)
        {

            attribute.Show();
            while (attribute.Visible)
            {
                Application.DoEvents();
            }
            dataGridView1.Columns.Clear();

            for (int i = 0; i < attribute.attributes_listbox2.Length; i++)
            {
                DataGridViewColumn temp = new DataGridViewTextBoxColumn();
                temp.HeaderText = attribute.attributes_listbox2[i];
                temp.DividerWidth = 1;
                temp.ReadOnly = true;
                temp.Name = "Column" + (i + 1);
                dataGridView1.Columns.Add(temp);
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                dataGridView1.Columns[i].Width = 200;
            }

            if (!weakness_found)
            {
                dataGridView1.Rows.Add(18);
            }
            else
            {
                if (!backgroundWorker3.IsBusy)
                {
                    backgroundWorker3.RunWorkerAsync();
                }

            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && button1.Enabled)
            {
                button1_Click(sender, e);
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text != connectedDomain)
                comboBox1.Items.Clear();
            else
                GetDomain(true);
        }
        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {

            data_grid_index = 0;
            IEnumerator<accountDetails[]> enu = hashmap.Values.GetEnumerator();
            while (enu.MoveNext())
            {
                if (enu.Current[0].has_weak_password)
                {
                    print_to_grid( enu.Current[0].NtHash);
                }
            }

        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            string csv = string.Empty;
            int num = 0, num1 = 0, total = data_grid_index;
            label7.Text = "0%";
            progressBar1.Value = 0;
            using (StreamWriter write = new StreamWriter(fileName))
            {
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    csv += "\"" + column.HeaderText + "\"" + ",";
                }
               csv= csv.Remove(csv.Length-1);
                write.WriteLine(csv);
                csv = "";
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    num++;
                    int num4 = (int)((num * 100) / ((double)total));
                    if (num4 > num1)
                    {
                        if (num4 <= 100)
                        {
                            progressBar1.Value = num4;
                            label7.Text = string.Format("{0}%", num4);
                        }
                        num1 = num4;
                    }
                    foreach (DataGridViewCell cell in row.Cells)
                    {

                        try
                        {

                            csv += "\"" + cell.Value.ToString() + "\"" + ",";
                        }
                        catch (Exception)
                        {
                            csv += "\"\",";
                        }
                    }
                    csv = csv.Remove(csv.Length-1);
                    write.WriteLine(csv);
                    csv = "";
                }

            }

            MessageBox.Show("File saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            label8.Text = "Processing...";
            label7.Text = "";
            progressBar1.Value = 0;
            HideProgressBar();
            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
