using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.DirectoryServices.ActiveDirectory; // for 'List'
using System.Collections;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Net.NetworkInformation;
using System.IO;
using CheckComboBoxTest;
using LocalUserManagement;
namespace LocalUserManagement
{
    public partial class AddMultipleComputers : Form
    {
        int ind = 0;
        public bool defaultUser = true;
        bool is_import = false;
        bool is_load = false;
        string path = null, uripath = null;
        System.Collections.Generic.List<String> AllComputersList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> AllComputersLocationList = new System.Collections.Generic.List<String>();
        public bool rightUser = false;
        public String user, pass, fullName, desc;
        ImportComputersForm importform;
        int distinguishedname_position = -1;
        String DomainName;
        public String UserName;
        public String Password;
        public bool fav_set = false;
        public AddMultipleComputers()
        {
            InitializeComponent();
        }
        
        private void AddMultipleComputers_Load(object sender, EventArgs e)
        {
            this.Left = 250;
            this.Top =  100;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            is_load = true;
            Constants.computerList.Clear();
            Constants.checkedComputerList.Clear();
            this.comboBox1.Items.Clear();
            try
            {
                System.DirectoryServices.ActiveDirectory.Forest currentForest = System.DirectoryServices.ActiveDirectory.Forest.GetCurrentForest();
                System.DirectoryServices.ActiveDirectory.DomainCollection dc = currentForest.Domains;
                foreach (System.DirectoryServices.ActiveDirectory.Domain d in dc)
                comboBox1.Items.Add(d.Name);
                comboBox1.SelectedItem = comboBox1.Items[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred while getting Domain details", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            button6_Click(this, e);
            
        }
        public void recolour()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            
        }

        private void SelectAllComputers(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView1.Items.Count; i++)
                    listView1.Items[i].Checked = false;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
                //MessageBox.Show("User name is a mandatory field", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Please Enter User Name", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (textBox4.Text.Equals(""))
                //MessageBox.Show("Password is a mandatory field", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show("Please Enter Password", "Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                user = textBox1.Text;
                fullName = textBox2.Text;
                desc = textBox3.Text;
                pass = textBox4.Text;
                rightUser = true;
                this.Close();
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3.Focus();
                button3_Click(sender, e);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
         
            /* 
            defaultUser = true;
            Domain domain;
            if (comboBox1.Text.Equals("Domain Name"))
            {
                MessageBox.Show("Please Enter the Domain Name", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if ((textBox1.Text.Equals("User Name") && !textBox2.Text.Equals("Password")) || (!textBox1.Text.Equals("User Name") && textBox2.Text.Equals("Password")))
            {
                MessageBox.Show("Please Enter User Name & Password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (!textBox1.Text.Equals("User Name") && !textBox2.Text.Equals("Password"))
                defaultUser = false;
            try
            {
                if (!defaultUser)
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName, UserName, Password));
                else
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName));
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show(" The network path was not found ");
                return;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter Correct Username & Password");
                return;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Unspecified Error");
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                 MessageBox.Show("Logon Failure. Please Enter Correct Username & Password");
                 return;
                }
                else
                {
                 MessageBox.Show("Unspecified Error");
                 return;                
                }
            }
             */
             panel2.Visible = false;
             importform =new ImportComputersForm();
             importform.FormClosed += AddMultipleUserFormClosed;
             importform.ShowDialog();
            //addMultipleUserForm = new AddMultipleUserForm();
           // addMultipleUserForm.FormClosed += AddMultipleUserFormClosed;
            //addMultipleUserForm.ShowDialog();
         
            // AddMultipleUserForm addMultipleUserForm = new AddMultipleUserForm();
            // addMultipleUserForm.ShowDialog();
        }
         public void AddMultipleUserFormClosed(object sender, EventArgs e)
        {
            if (importform.filePath == null)
            {
            }
            else if (importform.isImport == false)
            {
            }
            else
            {
                try
                {
                    System.IO.StreamReader myFile = new System.IO.StreamReader(importform.filePath);

                    string lin = "";
                    //line.ToCharArray();
                    //  if (isValid(myString) == false)
                    //      MessageBox.Show("This CSV file is not in correct format..\nThe Format of file is : \n \tUser Name,Full Name,Description,Password", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    int i = 0, j = 0, k = 0, comma_count = 0, position = 0, header_position = -1, header_comma_count = 0, quotation = 0;
                    bool is_header = true;
                    is_import = true;
                    CheckBox chkBox = new CheckBox();
                    chkBox.Checked = false;
                    chkBox.Name = "ToSelectAllComputers";
                    chkBox.Size = new System.Drawing.Size(13, 13);
                    chkBox.Location = new System.Drawing.Point(5, 2);
                    chkBox.CheckedChanged += SelectAllComputers;
                    listView1.Controls.Add(chkBox);
                    listView1.Items.Clear();
                    AllComputersList.Clear();
                    AllComputersLocationList.Clear();
                    while ((lin = myFile.ReadLine()) != null)
                    {
                        char c = '\n';
                        lin += c;
                        char[] line = lin.ToCharArray();
                        char[] word = new char[lin.Length + 1];
                        comma_count = 0;
                        position = 0;
                        quotation = 0;

                        for (i = 0, j = 0; i < line.Length; i++)
                        {

                            if (is_header)
                            {
                                word[j++] = char.ToLower(line[i]);
                            }

                            else
                                word[j++] = line[i];
                            
                            if (word[j - 1] == '"') quotation++;
                            if (line[i] == ',' || line[i] == '\n')
                            {
                               
                                if (word[0] != '"')
                                    word[j - 1] = '\0';
                                else if (word[0] == '"' && word[j - 2] == '"')
                                    word[j - 1] = '\0';
                                else
                                {
                                    while (true)
                                    {
                                        if (line[i] == '\n') break;
                                        if (line[i] == '"') break;
                                        if (is_header)
                                            word[j++] = char.ToLower(line[++i]);
                                        else
                                            word[j++] = line[++i];
                                        if (word[j - 1] == '"') quotation++;
                                    }
                                    if (line[i] == '\n')
                                    {
                                        MessageBox.Show("Not a Valid CSV Format");
                                        return;
                                    }
                                    if (line[i] == '"')
                                    {
                                        if (line[i + 1] == ',' || line[i + 1] == '\n')
                                            word[j] = '\0';
                                        else if (line[i + 1] == ';')
                                        {
                                            i++;
                                            while (true)
                                            {
                                                if (is_header)
                                                    word[j] = char.ToLower(line[i]);
                                                else
                                                    word[j] = line[i];
                                                if (word[j] == '"') quotation++;
                                                j++;
                                                i++;
                                                if (line[i] == '\n') break;
                                                if (line[i] == ';' && ((line[i + 1] != '"' && line[i - 1] == '"') || (line[i - 1] != '"' && line[i + 1] == '"')))
                                                {
                                                    MessageBox.Show("Not a Valid CSV Format");
                                                    return;
                                                }
                                                if (line[i] == '"' && line[i + 1] == ',')
                                                {
                                                    if (is_header)
                                                        word[j] = char.ToLower(line[i]);
                                                    else
                                                        word[j] = line[i];
                                                    if (word[j] == '"') quotation++;
                                                    word[j + 1] = '\0';
                                                    j++;
                                                    break;
                                                }
                                            }
                                            if (line[i] == '\n')
                                            {
                                                MessageBox.Show("Not a Valid CSV Format");
                                                return;
                                            }
                                        }
                                        else if (line[i + 1] == '"')
                                        {
                                            i++;
                                            while (true)
                                            {
                                                if (is_header)
                                                    word[j] = char.ToLower(line[i]);
                                                else
                                                    word[j] = line[i];
                                                if (word[j] == '"') quotation++;
                                                j++;
                                                i++;
                                                if (line[i] == '\n') break;
                                                if (line[i] == '"' && line[i + 1] == ',')
                                                {
                                                    if (is_header)
                                                        word[j] = char.ToLower(line[i]);
                                                    else
                                                        word[j] = line[i];
                                                    if (word[j] == '"') quotation++;
                                                    word[j + 1] = '\0';
                                                    j++;
                                                    break;
                                                }
                                            }
                                            if (line[i] == '\n')
                                            {
                                                MessageBox.Show("Not a Valid CSV Format");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Not a Valid CSV Format");
                                            return;
                                        }
                                    }
                                    i++;
                                    j++;
                                }


                                if (!(line[i] == '\n'))
                                {
                                    if (is_header)
                                        header_comma_count++;
                                    else
                                        comma_count++;
                                }
                               
                                if (j > 1)
                                {
                                    if (((word[0] != '"') && (word[j - 2] == '"')) || ((word[0] == '"') && (word[j - 2] != '"')) || (word[0] == '"' && word[1] == '\0'))
                                    {
                                        MessageBox.Show("Not a Valid CSV Format");
                                        return;
                                    }
                                }
                                if (is_header)
                                {
                                    //// if there are name and cn and computername field in given csv , the order of priority will be name>cn>computername ////
                                    string str = new string(word);
                                    string str2 = str.Substring(0, j - 1);

                                    if ((str2.Equals("name")) || (str2.Equals("\"name\"")))
                                    {
                                        k = 3;                                                            //prioritising variable
                                        header_position = position;
                                        position++;
                                    }
                                    else if ((str2.Equals("\"cn\"")) || (str2.Equals("cn")))
                                    {
                                        if (k < 3)
                                        {
                                            header_position = position;
                                            k = 2;
                                        }
                                        position++;
                                    }
                                    else if ((str2.Equals("\"computername\"")) || (str2.Equals("computername")))
                                    {
                                        if (k < 2)
                                        {
                                            header_position = position;
                                            k = 1;
                                        }
                                        position++;
                                    }
                                    else
                                        position++;

                                    if ((str2.Equals("\"distinguished name\"")) || (str2.Equals("distinguished name")) || (str2.Equals("distinguishedname")) || (str2.Equals("\"distinguishedname\"")))
                                        distinguishedname_position = position;

                                    if (j > 1)
                                    {

                                        if (((word[0] != '"') && (word[j - 2] == '"')) || ((word[0] == '"') && (word[j - 2] != '"')) || (word[0] == '"' && word[1] == '\0'))
                                        {
                                            MessageBox.Show("Not a Valid CSV Format");
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    position++;
                                    if ((position - 1) == header_position)
                                    {
                                        for (k = 0; word[k] == '"'; k++) ;
                                        for (j = 0; word[j + k] != '\0'; j++)
                                        {
                                            word[j] = word[j + k];
                                        }
                                        if ((j - k) > 0)
                                            word[j - k] = '\0';
                                        else
                                            word[0] = '\0';
                                        string str = new string(word);
                                        int index = str.IndexOf('\0');
                                        string str2 = str.Substring(0,index);
                                        AllComputersList.Add(str2);
                                    }

                                    if (distinguishedname_position != -1)
                                    {
                                        if (position == distinguishedname_position)
                                        {
                                            string str = new string(word);
                                            AllComputersLocationList.Add(str);
                                        }
                                    }
                                }
                                if (quotation % 2 != 0)
                                {
                                    MessageBox.Show("Not a Valid CSV Format");
                                    return;
                                }
                              
                                word[0] = '\0';
                                j = 0;
                                quotation = 0;
                          
                            }
                        }
                        if (is_header)
                        {
                            if (header_position == -1)
                            {
                                MessageBox.Show("Invalid header in CSV file.\nPlease provide any one of the following attributes as its header:\n name or cn or ComputerName","Error Message",MessageBoxButtons.OK,MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            if (comma_count != header_comma_count)
                            {
                                MessageBox.Show("Not a Valid CSV Format");
                                return;
                            }
                        }
                        is_header = false;

                    }//while
                    
                    for (i = 0; i < AllComputersList.Count; i++)
                    {
                        ListViewItem item = new ListViewItem(AllComputersList[i]);
                        listView1.Items.Add(item);
                        if(distinguishedname_position!=-1 && is_import)
                        item.SubItems.Add(AllComputersLocationList[i]);
                        else
                            item.SubItems.Add("-");
                    }
                    listView1.Sorting = SortOrder.Ascending;
                    recolour();
                    label7.Text = "Total Number of Computers  : " + i;
                    
               }

               catch (Exception)
               {
                 MessageBox.Show("File does not Exist", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 return; 
               }
                textBox5.Visible = true;
                button9.Visible = true;
                pictureBox1.Visible = true;
            }
         }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            

         }

        private void button6_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            button7.Visible = true;
            pictureBox3.Visible = true;
            button1.Visible = false; 
            listView1.Enabled = false;
           
            is_import = false;
            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";

        /*    if (progressBar3.Visible == true)
            {
                MessageBox.Show("Loading is going on... Please, wait for a while.", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
         
            if (comboBox1.Text.Equals("Domain Name"))
            {
                MessageBox.Show("Please Enter the Domain Name", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if ((textBox1.Text.Equals("User Name") && !textBox2.Text.Equals("Password")) || (!textBox1.Text.Equals("User Name") && textBox2.Text.Equals("Password")))
            {
                MessageBox.Show("Please Enter User Name & Password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
               else if (!textBox1.Text.Equals("User Name") && !textBox2.Text.Equals("Password"))
                defaultUser = false;
         */

            AllComputersList.Clear();
            AllComputersLocationList.Clear();
            textBox5.Text = "";
            listView1.Items.Clear();

            DomainName = comboBox1.Text;
            UserName = textBox1.Text;
            Password = textBox2.Text;

         /* progressBar3.Value = 40;
            progressBar3.Visible = true;
            progressBar3.Refresh();
            button13.Visible = true;
            button13.Refresh();
            label20.Visible = true;
            label20.Text = "Loading Computer List...";
            label20.Refresh();
          */
            backgroundWorker1.RunWorkerAsync();
            
            
        }
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                // Getting DC list
                System.Collections.Generic.List<String> dcList = new System.Collections.Generic.List<string>();
                Domain domain;
                if (!defaultUser)
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName, UserName, Password));
                else
                    domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName));
                foreach (System.DirectoryServices.ActiveDirectory.DomainController dc in domain.DomainControllers)
                {
                    if (backgroundWorker1.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    int index = dc.Name.IndexOf('.');
                    String dcName = dc.Name.Substring(0, index);
                    dcList.Add(dcName);
                }

                String adsPath = "LDAP://" + DomainName;
                DirectoryEntry domainEntry;
                backgroundWorker1.ReportProgress(3);
                if (!defaultUser)
                {
                    domainEntry = new DirectoryEntry(adsPath, UserName, Password);
                }
                else
                {
                    domainEntry = new DirectoryEntry(adsPath);
                }

                backgroundWorker1.ReportProgress(3);
                DirectorySearcher mySearcher = new DirectorySearcher(domainEntry);
                mySearcher.Filter = "(ObjectCategory=computer)";
                mySearcher.PageSize = 1000;
                domainEntry.Children.SchemaFilter.Add("computer");
                foreach (SearchResult result in mySearcher.FindAll())
                {
                    if (backgroundWorker1.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    backgroundWorker1.ReportProgress(3);
                    String name = result.GetDirectoryEntry().Name;
                    String addName = name.Substring(name.IndexOf('=') + 1);
                    ListViewItem item = new ListViewItem(addName);

                    bool canDisplay = false;
                    int i;
                    for (i = 0; i < dcList.Count; i++)
                    {
                        if (addName.ToLower().Equals(dcList[i].ToLower()))
                            break;
                    }
                    if (i == dcList.Count)
                        canDisplay = true;

                    if (canDisplay == true)
                    {
                        String path = result.GetDirectoryEntry().Path;
                        String addPath = path.Substring(path.IndexOf(',') + 1);
                        item.SubItems.Add(addPath);
                        AllComputersList.Add(addName);
                        AllComputersLocationList.Add(addPath);
                        backgroundWorker1.ReportProgress(1, item);
                        System.Threading.Thread.Sleep(1);
                    }

                }

                domainEntry.Dispose();
            }   // try
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                backgroundWorker1.ReportProgress(2, "  The network path was not found " );
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                backgroundWorker1.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                backgroundWorker1.ReportProgress(2, "Unspecified Error");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    backgroundWorker1.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
                }
                else
                {
                    backgroundWorker1.ReportProgress(2, "Unspecified Error");
                }
            }

        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
          //  progressBar3.Value += 3;
          //  progressBar3.Refresh();

         //   if (progressBar3.Value > 180)
         //       progressBar3.Value = 5;

            if (e.ProgressPercentage == 1)
            {
                ListViewItem item = (ListViewItem)e.UserState;
                listView1.Items.Add(item);
                label7.Text = "Total Number of Computers  : " + listView1.Items.Count;
                if (is_load && listView1.Items.Count > 99) {
                    is_load = false;
                    backgroundWorker1.CancelAsync();
                }
               // label17.Refresh();
            }
            else if (e.ProgressPercentage == 2)
            {
                MessageBox.Show((String)e.UserState, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //  progressBar3.Visible = false;
            //  progressBar3.Refresh();
            //   button13.Visible = false;
            //   button13.Refresh();
            //   label20.Visible = false;
            //   label20.Refresh();

            if (listView1.Items.Count > 0)
            {
                // For Select All Purpose
                textBox5.Visible = true;
                pictureBox1.Visible = true;
                textBox5_Leave(sender, e);

                CheckBox chkBox = new CheckBox();
                chkBox.Checked = false;
                chkBox.Name = "ToSelectAllComputers";
                chkBox.Size = new System.Drawing.Size(13, 13);
                chkBox.Location = new System.Drawing.Point(5, 2);
                chkBox.CheckedChanged += SelectAllComputers;
                listView1.Controls.Add(chkBox);

                CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
                cb.Checked = true;
                cb.Checked = false;

                listView1.Sorting = SortOrder.Ascending;
                recolour();
                listView1.Refresh();
              
                // label17.Refresh();
            }
            button7.Visible = false;
            listView1.Enabled = true;
            button1.Visible = true;
            textBox5.Visible = true;
            button9.Visible = true;
            pictureBox1.Visible = true;
            label7.Text = "Total Number of Computers  : " + listView1.Items.Count;
            pictureBox3.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           /* if (progressBar1.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while...", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            */

            listView1.Items.Clear();
            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";
            recolour();
            listView1.Refresh();

            if (AllComputersList.Count == 0)
            {
                label7.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label7.Refresh();
                MessageBox.Show("Please Enter Computer Details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox5.Text.Equals("") || textBox5.Text.Equals("Computer Name"))
            {
                
                for (int i = 0; i < AllComputersList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(AllComputersList[i]);
                    if (distinguishedname_position == -1 && is_import)
                        item.SubItems.Add("-");
                    else
                        item.SubItems.Add(AllComputersLocationList[i]);
                    listView1.Items.Add(item);
                }
            }
            else
            {
                pictureBox4.Visible = true;
                pictureBox1.Visible = false;
                for (int i = 0; i < AllComputersList.Count; i++)
                {
                    if (AllComputersList[i].Contains(textBox5.Text) || AllComputersList[i].ToUpper().Contains(textBox5.Text.ToUpper()))
                    {
                        ListViewItem item = new ListViewItem(AllComputersList[i]);
                        if (distinguishedname_position == -1 && is_import)
                            item.SubItems.Add("-");
                        else
                            item.SubItems.Add(AllComputersLocationList[i]);
                        listView1.Items.Add(item);
                    }
                }
            }
            label7.Text = "Total Number of Computers  : " + listView1.Items.Count;
            label7.Refresh();

            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
            cb.Checked = true;
            cb.Checked = false;
            recolour();
            listView1.Refresh();
            
        }
        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text.Equals(""))
            {
                textBox5.ForeColor = Color.Gray;
                textBox5.Text = "Computer Name";
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.ForeColor == Color.Gray)
            {
                textBox5.Text = "";
                textBox5.ForeColor = Color.Black;
            }
        }
        private void textBox5_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button2.Focus();
                pictureBox1_Click(sender, e);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox5.Text = "";
            textBox5_Leave(sender, e);

            listView1.Items.Clear();
            AllComputersList.Clear();
            AllComputersLocationList.Clear();
            Constants.computerList.Clear();

            label7.Text = "Total Number of Computers  : " + 0;

            ColumnHeader header = listView1.Columns[1];
            header.Text = "    Container Name ";

            //pictureBox6.Visible = true;
            //pictureBox4.Visible = true;
            // pictureBox2.Visible = false;
            // pictureBox3.Visible = false;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.CheckedItems.Count < 1)
            {
                MessageBox.Show("Please select at least one computer", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                button1.Visible = true;
                return;
            }
            recolour();
            ColumnHeader header = listView1.Columns[1];
            label7.Text = " Checking Connectivity  Please Wait ... ";
            header.Text = "    Connectivity ";
            button1.Visible = false;
            button8.Visible = true;
            button9.Visible = false;
            for (int i = 0; i < listView1.Items.Count; i++)
                listView1.Items[i].SubItems[1].Text = "-----";

            for (int i = 0; i < listView1.CheckedItems.Count; i++)
                Constants.checkedComputerList.Add(listView1.CheckedItems[i].Text);
           
            listView1.Refresh();
            // MessageBox.Show(checkedComputerList.Count.ToString());
            backgroundWorker2.RunWorkerAsync(Constants.checkedComputerList);
        }
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            /*
            try
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    checkedComputerList.Add(listView1.CheckedItems[i].Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
             */

            //System.Collections.Generic.List<String> coll =new System.Collections.Generic.List<String>();
            //for (int i = 0; i < checkedComputerList.Count; i++)
            //    coll.Add(checkedComputerList[i]);
           
            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;

            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;

            

            List<String> ComboList = new List<String>();

            Constants.computerList.Clear();

            for (ind = 0; ind < coll.Count; ind++)
            {


                tempBGWorker.ReportProgress(ind, "Checking");

                if (backgroundWorker2.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                /////////////////////////////////////////// PingChecking Computers
                try
                {

                    Ping pingSender = new Ping();
                    PingOptions options = new PingOptions();
                    options.DontFragment = true;
                    String data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                    byte[] buffer = System.Text.Encoding.ASCII.GetBytes(data);
                    int timeout = 60;

                    PingReply reply = pingSender.Send(coll[ind], timeout, buffer, options);

                    if (reply.Status != IPStatus.Success)
                    {
                        tempBGWorker.ReportProgress(ind, "Not Accessible");
                       // MessageBox.Show(coll[ind] + " is Not Accessible", "Warning");
                        //System.Threading.Thread.Sleep(100);
                    }
                    else
                    {
                        ///////////////////////////////////////// DirectoryEntry Checking
                        try
                        {
                            String ads = "WinNT://" + coll[ind] + ",computer";
                            DirectoryEntry computerEntries;
                            if (!defaultUser)
                                computerEntries = new DirectoryEntry(ads, UserName, Password);
                            else
                                computerEntries = new DirectoryEntry(ads);

                            DirectoryEntries userEntries = computerEntries.Children;

                            foreach (DirectoryEntry user in userEntries)
                            {
                            }
                            
                            tempBGWorker.ReportProgress(ind, "Accessible");
                            //Constants.computerList.Add(coll[ind]);
                            //    System.Threading.Thread.Sleep(100);
                        }
                        catch (Exception ex)
                        {
                            tempBGWorker.ReportProgress(ind, "Not Accessible");
                           // MessageBox.Show(coll[ind] + " is Not Accessible", "Warning");
                            //      System.Threading.Thread.Sleep(100);
                        } // catch
                        ///////////////////////////////////////// DirectoryEntry Checking
                    } // else
                } //try
                catch (Exception ex)
                {
                    tempBGWorker.ReportProgress(ind, "Not Accessible");
                    // MessageBox.Show(coll[ind] + " is Not Accessible", "Warning");
                    //System.Threading.Thread.Sleep(100);
                }
                /////////////////////////////////////////// PingChecking COmputers
            } // for loop
        }

        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

            

            
            //     if (progressBar1.Value > 150)
            //           progressBar1.Value = 5;
                      
            if (e.UserState.ToString().Equals("Checking"))
                label7.Text = "Checking accessibility of \"" + Constants.checkedComputerList[e.ProgressPercentage] + "\" ";
            else
            {
                listView1.CheckedItems[e.ProgressPercentage].SubItems.RemoveAt(1);
                listView1.CheckedItems[e.ProgressPercentage].UseItemStyleForSubItems = false;
                listView1.CheckedItems[e.ProgressPercentage].SubItems.Add("" + e.UserState);
               /* if(e.ProgressPercentage % 2 == 0)
                listView1.CheckedItems[e.ProgressPercentage].SubItems[1].BackColor = Color.LightGray;
                else
                listView1.CheckedItems[e.ProgressPercentage].SubItems[1].BackColor = Color.White;*/
                if (e.UserState.Equals("Accessible"))
                listView1.CheckedItems[e.ProgressPercentage].SubItems[1].ForeColor = Color.Green;
                else
                listView1.CheckedItems[e.ProgressPercentage].SubItems[1].ForeColor = Color.Red;
                recolour();
                
                // listView1.Refresh();
            }

             
            if (e.UserState.ToString().Equals("Accessible"))
            {
                //  MessageBox.Show("i : " + e.ProgressPercentage + "state : " + e.UserState + "listView1.CheckedItems[e.ProgressPercentage].Text : " + listView1.CheckedItems[e.ProgressPercentage].Text);
                  Constants.computerList.Add(listView1.CheckedItems[e.ProgressPercentage].Text);
                //  MessageBox.Show("computerList.Count : " + computerList.Count);
            }

        }

    

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            //  MessageBox.Show("Completed..... computerlist.count : " + computerList.Count);
            if (e.Error != null)
            {
               MessageBox.Show("Error occurred while stopping the process\n"+e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (Constants.computerList.Count < 1)
            {
             MessageBox.Show("Selected Computers are not accessible now", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
             label7.Text = "";
            }
            button8.Visible = false;
            button1.Visible = true;
            button9.Visible = true;
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            button7.Visible = false;
            listView1.Enabled = true;
            recolour();
            pictureBox3.Visible = false;
            textBox5.Visible = true;
            button9.Visible = true;
            pictureBox1.Visible = true;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            pictureBox1_Click(sender,e);
            pictureBox4.Visible = false;
            pictureBox1.Visible = true;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = true;
        }

        private void AddMultipleComputers_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button5_Enter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void listView1_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button7_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void textBox5_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            panel2.Visible = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            backgroundWorker2.CancelAsync();
            label7.Text = "Process about to Stop .. Please Wait ..";
            button7.Visible = false;
            listView1.Enabled = true;
            recolour();
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

        private void button9_Click(object sender, EventArgs e)
        {
            uripath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = new Uri(uripath).LocalPath;
            System.IO.StreamWriter file = new System.IO.StreamWriter(path + "/conf/save.txt");
            System.Collections.Generic.List<String> tempList = new System.Collections.Generic.List<String>();

            if (listView1.CheckedItems.Count > 0)
            {
                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    tempList.Add((listView1.CheckedItems[i].Text));
                string s = string.Join(",", tempList.ToArray());
                s = s + ',';
                file.WriteLine(s);
                file.Close();
                MessageBox.Show("The selected computers appears in your favourite list", "Success");
                fav_set = true;
                this.Close();
            }
            else
                MessageBox.Show("Please select at least one computer", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }



        }          
    }

