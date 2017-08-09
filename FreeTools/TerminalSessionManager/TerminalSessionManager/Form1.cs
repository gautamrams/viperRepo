using System;
using System.DirectoryServices;
using System.Drawing;
using System.Collections.Generic;  // for 'List'
using System.Collections; // for 'ArrayList'
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices; // for "DllImport"

namespace TerminalSessionManager
{
    public partial class Form1 : Form
    {
        int ind = 0;
        String DomainName;
        String UserName;
        String Password;
        bool defaultUser = true;
        System.Collections.Generic.List<String> computerList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> AllComputersList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> AllComputersLocationList = new System.Collections.Generic.List<String>();
        System.Collections.Generic.List<String> checkedComputerList = new System.Collections.Generic.List<String>();
        System.Collections.ArrayList SessionList = new System.Collections.ArrayList();
        System.Collections.ArrayList DisconnectedCheckedArrayList = new System.Collections.ArrayList();
        System.Collections.ArrayList LogoffCheckedArrayList = new System.Collections.ArrayList();

        public Form1()
        {
            InitializeComponent();
        }


        ////////////////////////////////////////////////////////////////

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSLogoffSession(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 SessionId,
            bool bWait
            );

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSDisconnectSession(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 SessionId,
            bool bWait
            );



        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);


        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSQuerySessionInformation(
  IntPtr hServer,
  [MarshalAs(UnmanagedType.U4)] Int32 SessionId,
    WTS_INFO_CLASS WTSInfoClass,
   ref String output,
  [MarshalAs(UnmanagedType.U4)] ref Int32 pBytesReturned
);
        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSQuerySessionInformation(
  IntPtr hServer,
  [MarshalAs(UnmanagedType.U4)] Int32 SessionId,
    WTS_INFO_CLASS WTSInfoClass,
   ref WTS_CONNECTSTATE_CLASS output,
  [MarshalAs(UnmanagedType.U4)] ref Int32 pBytesReturned
);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);


        //        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        private enum WTS_INFO_CLASS
        {
            WTSInitialProgram = 0,
            WTSApplicationName = 1,
            WTSWorkingDirectory = 2,
            WTSOEMId = 3,
            WTSSessionId = 4,
            WTSUserName = 5,
            WTSWinStationName = 6,
            WTSDomainName = 7,
            WTSConnectState = 8,
            WTSClientBuildNumber = 9,
            WTSClientName = 10,
            WTSClientDirectory = 11,
            WTSClientProductId = 12,
            WTSClientHardwareId = 13,
            WTSClientAddress = 14,
            WTSClientDisplay = 15,
            WTSClientProtocolType = 16,
            WTSIdleTime = 17,
            WTSLogonTime = 18,
            WTSIncomingBytes = 19,
            WTSOutgoingBytes = 20,
            WTSIncomingFrames = 21,
            WTSOutgoingFrames = 22,
            WTSClientInfo = 23,
            WTSSessionInfo = 24,
            WTSSessionInfoEx = 25,
            WTSConfigInfo = 26,
            WTSValidationInfo = 27,
            WTSSessionAddressV4 = 28,
            WTSIsRemoteSession = 29
        }

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }
        public static IntPtr OpenServer(String Name)
        {

            try
            {
                IntPtr server = WTSOpenServer(Name);
                return server;

            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
            return WTSOpenServer(Name);
        }
        public static void CloseServer(IntPtr ServerHandle)
        {
            WTSCloseServer(ServerHandle);
        }
        ///////////////////////////////////////////////////////////////

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Left = 150;
            this.Top = 40;

            progressBar3.Visible = false;
            label15.Visible = false;
            pictureBox1.Visible = false;
            label17.Visible = true;
            textBox3.Visible = false;
            listView1.Visible = true;
            label14.Visible = false;
            button3.Visible = true;
            button5.Visible = false;
            button12.Visible = false;
            progressBar1.Visible = false;

            //groupBox1.Size = new Size(518, 121);
            //groupBox1.Location = new Point(140, 75);
            panel1.Visible = true;
            panel2.Visible = true;
            textBox2.Visible = true;
            label13.Visible = true;
            comboBox1.Visible = true;
            textBox1.Visible = true;
            button2.Visible = true;
            button1.Visible = true;
            label8.Visible = true;
            label12.Visible = true;

            //groupBox3.Visible = true;
            //groupBox3.Size = new Size(518, 355);
            //groupBox3.Location = new Point(140, 205);

            

            label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
            label17.Refresh();
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
                MessageBox.Show("Error occurred while getting Domain details.", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void recolour()
        {
            for (int item = 0; item < listView1.Items.Count; ++item)
            {
                var items = listView1.Items[item];
                items.BackColor = (item % 2 != 0) ? Color.FromArgb(243, 243, 243) : Color.White;
            }
           
        }


        // Get-Computers Button
        private void button1_Click(object sender, EventArgs e)
        {
            defaultUser = true;
            ColumnHeader header = listView1.Columns[1];
            header.Text = "Container Name ";

            if (progressBar3.Visible == true)
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

            AllComputersList.Clear();
            AllComputersLocationList.Clear();
            textBox3.Text = "";
            listView1.Items.Clear();

            DomainName = comboBox1.Text;
            UserName = textBox1.Text;
            Password = textBox2.Text;

            progressBar3.Value = 40;
            progressBar3.Visible = true;
            progressBar3.Refresh();

            button5.Visible = true;
            button5.Refresh();

            label14.Visible = true;
            label14.Text = "Loading Computer List...";
            label14.Refresh();

            backgroundWorker5.RunWorkerAsync();

            /////////////////////////////////////////

            //   } //else
        }

        // To Select All Items in List View Items
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

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text.Equals(""))
            {
                textBox3.ForeColor = Color.Gray;
                textBox3.Text = "Computer Name";
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.ForeColor == Color.Gray)
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (comboBox1.Text.Equals(""))
            {
                comboBox1.ForeColor = Color.Black;
                comboBox1.Text = "Domain Name";
            }
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                textBox1.ForeColor = Color.Gray;
                textBox1.Text = "User Name";
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (textBox1.ForeColor == Color.Gray)
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Black;
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.ForeColor == Color.Gray)
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
                textBox2.PasswordChar = '*';
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                textBox2.ForeColor = Color.Gray;
                textBox2.Text = "Password";
                textBox2.PasswordChar = '\0';
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            if (comboBox1.ForeColor == Color.Gray)
            {
                comboBox1.Text = "";
                comboBox1.ForeColor = Color.Black;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (progressBar1.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            listView1.Items.Clear();
            ColumnHeader header = listView1.Columns[1];
            header.Text = "Container Name ";


            if (AllComputersList.Count == 0)
            {
                label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label17.Refresh();
                MessageBox.Show("Please Enter Domain Details", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox3.Text.Equals("") || textBox3.Text.Equals("Computer Name"))
            {
                for (int i = 0; i < AllComputersList.Count; i++)
                {
                    ListViewItem item = new ListViewItem(AllComputersList[i]);
                    item.SubItems.Add(AllComputersLocationList[i]);
                    listView1.Items.Add(item);
                }
            }
            else
            {
                for (int i = 0; i < AllComputersList.Count; i++)
                {
                    //if (AllComputersList[i].StartsWith(textBox3.Text) || AllComputersList[i].StartsWith(textBox3.Text.ToUpper()))
                    if (AllComputersList[i].Contains(textBox3.Text) || AllComputersList[i].ToUpper().Contains(textBox3.Text.ToUpper()))
                    {
                        ListViewItem item = new ListViewItem(AllComputersList[i]);
                        item.SubItems.Add(AllComputersLocationList[i]);
                        listView1.Items.Add(item);
                    }
                }
            }
            label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
            label17.Refresh();

            CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
            cb.Checked = true;
            cb.Checked = false;
            recolour();
            listView1.Refresh();
        }

        // Get-Sessions Button
        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
                MessageBox.Show("Please Select a Computer", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (listView1.CheckedItems.Count < 1)
                MessageBox.Show("Please Select Any one of Computer", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                button3.Visible = false;
                button3.Refresh();
                label14.Visible = true;
                label14.Text = "Process initialized...";
                label14.Refresh();
                progressBar1.Visible = true;
                progressBar1.Refresh();
                button12.Visible = true;
                button12.Refresh();

                computerList.Clear();
                checkedComputerList.Clear();

                listView1.Refresh();

                for (int i = 0; i < listView1.CheckedItems.Count; i++)
                    checkedComputerList.Add(listView1.CheckedItems[i].Text);

                ColumnHeader header = listView1.Columns[1];
                header.Text = "Connectivity ";

                for (int i = 0; i < listView1.Items.Count; i++)
                    listView1.Items[i].SubItems[1].Text = "-----";

                listView1.Refresh();

                backgroundWorker1.RunWorkerAsync(checkedComputerList);
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(17);
            } // else
        }// Get-Sessions Button

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;

            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;

            for (ind = 0; ind < coll.Count; ind++)
            {

                tempBGWorker.ReportProgress(ind, "Checking");

                if (backgroundWorker1.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }


                /////////////////////////////////////////// PingChecking COmputers

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
                            //    System.Threading.Thread.Sleep(100);
                        }
                        catch (Exception ex)
                        {
                            tempBGWorker.ReportProgress(ind, "Not Accessible");
                            //      System.Threading.Thread.Sleep(100);
                        } // catch
                        ///////////////////////////////////////// DirectoryEntry Checking
                    } // else
                } //try
                catch (Exception ex)
                {
                    tempBGWorker.ReportProgress(ind, "Not Accessible");
                    //System.Threading.Thread.Sleep(100);
                }

                /*
                 try
                 {
                    
                     String retval = "";
                     String MachineName = coll[ind] ;
                     System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("nslookup", MachineName);
                     psi.UseShellExecute = false;
                     psi.RedirectStandardOutput = true;
                     psi.RedirectStandardInput = true;
                     psi.RedirectStandardError = true;
                     psi.CreateNoWindow = true;
                     System.Diagnostics.Process p = new System.Diagnostics.Process();
                     p.StartInfo = psi;
                     p.Start();
                     System.IO.StreamReader se = p.StandardError;
                     System.IO.StreamReader so = p.StandardOutput;
                     retval += so.ReadToEnd();
                     retval += se.ReadToEnd();
                     while (!p.HasExited)
                     {
                         retval += so.ReadToEnd();
                         retval += se.ReadToEnd();
                     }
                     retval += so.ReadToEnd();
                     retval += se.ReadToEnd();
                     p.Close();
                     p.Dispose();

                     if (retval.Contains("DNS request timed out"))
                         tempBGWorker.ReportProgress(ind, "Not Accessible");
                     else
                     {
                         /*
                         try
                         {
                             String ads = "WinNT://" + coll[ind] + ",computer";
                             DirectoryEntry computerEntries = new DirectoryEntry(ads);
                             DirectoryEntries userEntries = computerEntries.Children;

                             foreach (DirectoryEntry user in userEntries)
                             {
                             }
                             tempBGWorker.ReportProgress(ind, "Accessible");
                         }
                         catch (Exception ex)
                         {
                             tempBGWorker.ReportProgress(ind, "Not Accessible");
                         }
                          * /
                         tempBGWorker.ReportProgress(ind, "Accessible");
                     }
                 } //try
                 catch (Exception ex)
                 {
                     tempBGWorker.ReportProgress(ind, "Not Accessible");
                     //System.Threading.Thread.Sleep(100);
                 }
     */
                /////////////////////////////////////////// PingChecking COmputers
            } // for loop
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value++;
            progressBar1.PerformStep();
            progressBar1.Refresh();

            if (progressBar1.Value > 150)
                progressBar1.Value = 5;

            if (e.UserState.ToString().Equals("Checking"))
                label14.Text = "Checking accessibility of \"" + checkedComputerList[e.ProgressPercentage] + "\" ";
            else
            {
                listView1.CheckedItems[e.ProgressPercentage].SubItems.RemoveAt(1);
                listView1.CheckedItems[e.ProgressPercentage].SubItems.Add("" + e.UserState);
                listView1.Refresh();
            }

            if (e.UserState.ToString().Equals("Accessible"))
            {
                computerList.Add(listView1.CheckedItems[e.ProgressPercentage].Text);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                progressBar1.Visible = false;
                progressBar1.Refresh();
                label14.Visible = false;
                label14.Refresh();
                button3.Visible = true;

                MessageBox.Show("Error occurred while stopping the process\n" + e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (computerList.Count < 1)
            {
                progressBar1.Visible = false;
                progressBar1.Refresh();
                label14.Visible = false;
                label14.Refresh();
                button3.Visible = true;
                button12.Visible = false;
                MessageBox.Show("Selected Computers are not accessible now", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                progressBar1.Visible = false;
                progressBar1.Refresh();
                label14.Visible = false;
                label14.Refresh();

                //groupBox1.Visible = false;
                //groupBox3.Visible = false;
                panel1.Visible = false;
                panel2.Visible = false;
                progressBar3.Visible = false;
                label15.Visible = false;
                pictureBox1.Visible = false;
                label17.Visible = false;
                textBox3.Visible = false;
                listView1.Visible = false;
                label14.Visible = false;
                button3.Visible = false;
                button5.Visible = false;
                button12.Visible = false;
                progressBar1.Visible = false;



                textBox2.Visible = false;
                label13.Visible = false;
                comboBox1.Visible = false;
                textBox1.Visible = false;
                button2.Visible = false;
                button1.Visible = false;
                label8.Visible = false;
                label12.Visible = false;

                groupBox4.Size = new System.Drawing.Size(683, 464);
                groupBox4.Location = new System.Drawing.Point(52, 86);
                groupBox4.Visible = true;

                TerminalSessionsDetails();
            }
        }

        public void TerminalSessionsDetails()
        {
            pictureBox5.Visible = true;
            pictureBox7.Visible = false;
            pictureBox8.Visible = false;
            pictureBox9.Visible = true;

            listView2.Items.Clear();
            CheckBox chkBox = new CheckBox();
            chkBox.Checked = false;
            chkBox.Name = "ToSelectAll";
            chkBox.Size = new Size(13, 13);
            chkBox.Location = new Point(5, 2);
            chkBox.CheckedChanged += SelectAllSessions;
            listView2.Controls.Add(chkBox);

            refreshAll();

        }  // TerminalSessionsDetails function

        public void refreshAll()
        {
            //UserList.Clear();
            //GroupList.Clear();
            listView2.Items.Clear();
            SessionList.Clear();

            progressBar2.Visible = true;
            progressBar2.Location = new Point(165, 55);
            progressBar2.Refresh();
            label18.Location = new Point(144, 24);
            label18.Visible = true;
            label18.Refresh();

            groupBox6.Visible = false;
            groupBox7.Visible = false;

            button11.Location = new Point(420, 52);
            button11.Visible = true;
            button11.Refresh();

            backgroundWorker2.RunWorkerAsync(computerList);

        } // refresAll function
        // To Select All Sessions
        private void SelectAllSessions(System.Object sender, System.EventArgs e)
        {
            CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll"];

            if (cb.Checked == true)
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = true;
            else
                for (int i = 0; i < this.listView2.Items.Count; i++)
                    listView2.Items[i].Checked = false;

        }
        // Stop Button in Computer Details
        private void button12_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            label14.Text = "Process is about to stop...";
            label14.Refresh();
        }

        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            String ServerName = "";
            Int32 count1;
            IntPtr server = IntPtr.Zero;
            IntPtr ppSessionInfo = IntPtr.Zero;
            Int32 count;
            System.Collections.Generic.List<String> LocalMachineDetailsList = new System.Collections.Generic.List<String>();

            System.Collections.Generic.List<String> coll = (System.Collections.Generic.List<String>)e.Argument;
            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;

            ////////////////////////////////////////////////////////

            String LocalMachine = System.Environment.MachineName;
            server = IntPtr.Zero;
            server = OpenServer(LocalMachine);
            //ssageBox.Show(Marhal.PtrToStringAnsi(server));
            /*
            if (Marshal.ReadInt16(server) == 0)
                MessageBox.Show("Error");
            else
                MessageBox.Show("Ok");
            */

            count1 = 0;
            ppSessionInfo = IntPtr.Zero;

            count = 0;
            Int32 retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
            Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));

            Int32 current = (int)ppSessionInfo;

            LocalMachineDetailsList.Clear();
            if (retval != 0)
            {
                for (int i = 0; i < count; i++)
                {
                    //bool canDisplay = false;
                    WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                    current += dataSize;
                    if (si.pWinStationName.Contains("RDP-Tcp#"))
                    {
                        LocalMachineDetailsList.Add(si.pWinStationName);
                    }
                }
            }
            ////////////////////////////////////////////////////

            for (ind = 0; ind < coll.Count; ind++)
            {
                if (backgroundWorker2.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                tempBGWorker.ReportProgress(ind, "CheckingUsers");

                ServerName = coll[ind];
                server = IntPtr.Zero;
                server = OpenServer(ServerName);

                try
                {
                    int a = Marshal.ReadInt16(server);

                }
                catch (Exception ex)
                {
                    continue;
                }

                count1 = 0;

                try
                {
                    ppSessionInfo = IntPtr.Zero;

                    count = 0;
                    retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
                    dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));

                    current = (int)ppSessionInfo;

                    if (retval != 0)
                    {
                        ArrayList tempArrayList = new ArrayList();
                        for (int i = 0; i < count; i++)
                        {
                            //bool canDisplay = false;
                            WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));

                            current += dataSize;
                            ////////////////////////////////
                            String userName = "";
                            WTSQuerySessionInformation(server, si.SessionID, WTS_INFO_CLASS.WTSUserName, ref userName, ref count1);
                            String clientName = "";
                            WTSQuerySessionInformation(server, si.SessionID, WTS_INFO_CLASS.WTSClientName, ref clientName, ref count1);
                            /////////////////////////////////
                            String state = "" + si.State;

                            bool canDisplay = false;

                            if (!userName.Equals(""))
                            {
                                if (!si.pWinStationName.Equals("Console"))
                                {
                                    if (state.Equals("WTSActive") || state.Equals("WTSDisconnected") || state.Equals("WTSIdle"))
                                    {
                                        if (!ServerName.Equals(LocalMachine))
                                        {
                                            int j = 0;
                                            for (j = 0; j < LocalMachineDetailsList.Count; j++)
                                            {
                                                //    MessageBox.Show("j : " + j + "\nsi.pWinStationName : " + si.pWinStationName + "\nLocalMachineDetailsList[j] : " + LocalMachineDetailsList[j]);
                                                if (si.pWinStationName.Equals(LocalMachineDetailsList[j]))
                                                    break;
                                            } // for loop

                                            if (j == LocalMachineDetailsList.Count)
                                                canDisplay = true;
                                        }
                                        else
                                            canDisplay = true;

                                    }
                                }
                            }
                            if (canDisplay == true)
                            {
                                ListViewItem item = new ListViewItem(userName);
                                item.SubItems.Add("" + si.SessionID);
                                if (state.Equals("WTSActive"))
                                    item.SubItems.Add("Active");
                                else if (state.Equals("WTSDisconnected"))
                                    item.SubItems.Add("Disconnected");
                                else
                                    item.SubItems.Add("Idle");

                                item.SubItems.Add(clientName);
                                item.SubItems.Add(si.pWinStationName);
                                item.SubItems.Add(ServerName);

                                tempBGWorker.ReportProgress(ind, item);
                                //System.Threading.Thread.Sleep(200);
                            }
                        } // for
                        //tempBGWorker.ReportProgress(ind, tempArrayList);
                        WTSFreeMemory(ppSessionInfo);
                    } // if
                } // try
                catch (System.UnauthorizedAccessException ex)
                {
                    tempBGWorker.ReportProgress(ind, "" + ex);
                    System.Threading.Thread.Sleep(400);
                }
                catch (Exception ex)
                {
                    tempBGWorker.ReportProgress(ind, "" + ex);
                    System.Threading.Thread.Sleep(400);
                }
                finally
                {
                    CloseServer(server);
                }
            } // "for" loop

        }

        private void backgroundWorker2_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar2.Value++;
            progressBar2.PerformStep();
            progressBar2.Refresh();

            if (progressBar2.Value > 150)
                progressBar2.Value = 5;
            if (e.UserState.GetType().ToString().Equals("System.String"))
            {
                //  MessageBox.Show("Equal to String -- ");
                if (e.UserState.ToString().Equals("CheckingUsers"))
                {
                    //  MessageBox.Show("Checking user");
                    label18.Text = "Loading Session Details from \"" + computerList[e.ProgressPercentage] + "\" ";
                }
                else //if (e.UserState.ToString().Equals("AccessDenied"))
                {
                    //  MessageBox.Show("Access Denied user");
                    //computerList.RemoveAt(e.ProgressPercentage);
                    //ind--;
                    MessageBox.Show("Problem in " + computerList[e.ProgressPercentage] + ".... \n Error :... " + e.UserState);
                }
            }
            else
            {
                ListViewItem item = (ListViewItem)e.UserState;
                SessionList.Add(item);
                listView2.Items.Add(item);
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();

                // MessageBox.Show("Error occurred while stopping the process\n" + e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (ind != computerList.Count)
                {
                    for (int i = ind; i < computerList.Count; )
                        computerList.RemoveAt(i);
                }

                if (listView2.Items.Count == 0)
                {
                    backButtonClicked();
                    pictureBox7.Visible = true;
                    pictureBox9.Visible = true;

                    MessageBox.Show("There is no Terminal Sessions in Selected Computers", " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                else
                {
                    comboBox2.SelectedIndex = 0;

                    progressBar2.Visible = false;
                    progressBar2.Refresh();
                    label18.Visible = false;
                    label18.Refresh();

                    groupBox6.Location = new Point(27, 20);
                    groupBox6.Size = new Size(273, 80);
                    groupBox6.Visible = true;

                    groupBox7.Location = new Point(415, 20);
                    groupBox7.Size = new Size(244, 80);
                    groupBox7.Visible = true;

                    button11.Visible = false;
                    button11.Refresh();
                } //else

            }
        }

        // Stop Button
        private void button11_Click(object sender, EventArgs e)
        {
            backgroundWorker2.CancelAsync();
            label18.Text = "Process is about to stop...";
            label18.Refresh();
        }  // Stop Button

        // Back Button
        public void backButtonClicked()
        {
            pictureBox5.Visible = false;
            pictureBox7.Visible = true;
            pictureBox8.Visible = true;
            pictureBox9.Visible = false;

            button3.Visible = true;
            button12.Visible = false;

            groupBox4.Visible = false;
            progressBar3.Visible = false;
            label15.Visible = false;
            pictureBox1.Visible = false;
            label17.Visible = true;
            textBox3.Visible = false;
            listView1.Visible = true;
            label14.Visible = false;
            button3.Visible = true;
            button5.Visible = false;
            button12.Visible = false;
            progressBar1.Visible = false;

            textBox2.Visible = true;
            label13.Visible = true;
            comboBox1.Visible = true;
            textBox1.Visible = true;
            button2.Visible = true;
            button1.Visible = true;
            label8.Visible = true;
            label12.Visible = true;
            panel1.Visible = true;
            panel2.Visible = true;
         //   groupBox1.Visible = true;
            //comboBox1.Items.Clear();
            //comboBox1.Items.Add(System.Environment.UserDomainName);

        //    groupBox1.Size = new Size(518, 121);
        //    groupBox1.Location = new Point(62, 75);

            //groupBox3.Visible = true;
            //groupBox3.Size = new Size(518, 355);
           // groupBox3.Location = new Point(102, 205);
        } // Back Button

        // Back Button
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            backButtonClicked();
        } // Back Button

        // Front Button
        private void pictureBox8_Click(object sender, EventArgs e)
        {
            if (progressBar1.Visible == true)
            {
                MessageBox.Show("Loading is being done. So, Please wait for a while", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            pictureBox8.Visible = false;
            pictureBox9.Visible = true;
            pictureBox5.Visible = true;
            pictureBox7.Visible = false;

            progressBar1.Visible = false;
            progressBar1.Refresh();
            label14.Visible = false;
            label14.Refresh();

            //groupBox1.Visible = false;
            //groupBox3.Visible = false;
            panel1.Visible = false;
            panel2.Visible = false;

            progressBar3.Visible = false;
            label15.Visible = false;
            pictureBox1.Visible = false;
            label17.Visible = false;
            textBox3.Visible = false;
            listView1.Visible = false;
            label14.Visible = false;
            button3.Visible = false;
            button5.Visible = false;
            button12.Visible = false;
            progressBar1.Visible = false;

            textBox2.Visible = false;
            label13.Visible = false;
            comboBox1.Visible = false;
            textBox1.Visible = false;
            button2.Visible = false;
            button1.Visible = false;
            label8.Visible = false;
            label12.Visible = false;

            groupBox4.Size = new System.Drawing.Size(683, 464);
            groupBox4.Location = new System.Drawing.Point(52, 86);
            groupBox4.Visible = true;

            CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll"];
            cb.Checked = true;
            cb.Checked = false;
        }  // Front Button

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            if (comboBox2.Text.Equals("All"))
            {
                for (int i = 0; i < SessionList.Count; i++)
                {
                    ListViewItem item = (ListViewItem)SessionList[i];
                    listView2.Items.Add(item);
                }
            }
            else if (comboBox2.Text.Equals("Active"))
            {
                for (int i = 0; i < SessionList.Count; i++)
                {
                    ListViewItem item = (ListViewItem)SessionList[i];
                    if (item.SubItems[2].Text.Equals("Active"))
                        listView2.Items.Add(item);
                }
            }
            else if (comboBox2.Text.Equals("Disconnected"))
            {
                for (int i = 0; i < SessionList.Count; i++)
                {
                    ListViewItem item = (ListViewItem)SessionList[i];
                    if (item.SubItems[2].Text.Equals("Disconnected"))
                        listView2.Items.Add(item);
                }
            }
            else if (comboBox2.Text.Equals("Idle"))
            {
                for (int i = 0; i < SessionList.Count; i++)
                {
                    ListViewItem item = (ListViewItem)SessionList[i];
                    if (item.SubItems[2].Text.Equals("Idle"))
                        listView2.Items.Add(item);
                }
            }
            CheckBox cb = (CheckBox)listView2.Controls["ToSelectAll"];
            cb.Checked = true;
            cb.Checked = false;
        }

        // Disconnect Button
        private void button10_Click(object sender, EventArgs e)
        {
            if (listView2.CheckedItems.Count == 0)
                MessageBox.Show("Please select atleast one Session", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DialogResult result;

                result = MessageBox.Show("Are you sure you want to Disconnect the selected Session(s) ?", "Confirm Session Disconnect", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;

                progressBar2.Visible = true;
                progressBar2.Location = new Point(165, 55);
                progressBar2.Refresh();
                label18.Location = new Point(144, 24);
                label18.Visible = true;
                label18.Refresh();

                groupBox6.Visible = false;
                groupBox7.Visible = false;

                button7.Location = new Point(420, 52);
                button7.Visible = true;
                button7.Refresh();

                ListView.CheckedListViewItemCollection tempCollection = listView2.CheckedItems;
                DisconnectedCheckedArrayList.Clear();

                for (int i = 0; i < tempCollection.Count; i++)
                    DisconnectedCheckedArrayList.Add(tempCollection[i]);

                backgroundWorker3.RunWorkerAsync(DisconnectedCheckedArrayList);

            }
        }  // Disconnect Button


        private void backgroundWorker3_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ArrayList collection = (ArrayList)e.Argument;
            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;

            int result;

            for (ind = 0; ind < collection.Count; ind++)
            {
                ListViewItem item = (ListViewItem)collection[ind];

                if (backgroundWorker3.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                tempBGWorker.ReportProgress(ind, "Checking");

                result = WTSDisconnectSession(OpenServer(item.SubItems[5].Text), Int32.Parse(item.SubItems[1].Text), true);
                if (result == 0)
                    tempBGWorker.ReportProgress(ind, "Error");
            }
        }

        private void backgroundWorker3_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar2.Value++;
            progressBar2.PerformStep();
            progressBar2.Refresh();

            if (progressBar2.Value > 150)
                progressBar2.Value = 5;

            if (e.UserState.ToString().Equals("Checking"))
            {
                ListViewItem item = (ListViewItem)DisconnectedCheckedArrayList[e.ProgressPercentage];
                label18.Text = "\"" + item.Text + "\" Session in \"" + item.SubItems[5].Text + "\" is being Disconnected...";
                //MessageBox.Show("checking" + item.SubItems[5].Text);
            }
            else if (e.UserState.ToString().Equals("Error"))
            {
                ListViewItem item = (ListViewItem)DisconnectedCheckedArrayList[e.ProgressPercentage];
                label18.Text = "Unspecified Error occurred while Disconnecting the Session \"" + item.Text + "\" in \"" + item.SubItems[5].Text + "\" isd being Disconnected";
                //MessageBox.Show("Error in " + item.SubItems[5].Text);
            }
        }

        private void backgroundWorker3_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();

                // MessageBox.Show("Error occurred while stopping the process\n" + e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                comboBox2.SelectedIndex = 0;

                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();

                groupBox6.Location = new Point(27, 20);
                groupBox6.Size = new Size(273, 80);
                groupBox6.Visible = true;

                groupBox7.Location = new Point(415, 20);
                groupBox7.Size = new Size(244, 80);
                groupBox7.Visible = true;

                button7.Visible = false;
                button7.Refresh();

                refreshAll();
            }
        }


        // Logoff Button
        private void button9_Click(object sender, EventArgs e)
        {
            if (listView2.CheckedItems.Count == 0)
                MessageBox.Show("Please select atleast one Session", " Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                DialogResult result;

                result = MessageBox.Show("Are you sure you want to Logoff the selected Session(s) ?", "Confirm Session Logoff", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                    return;

                progressBar2.Visible = true;
                progressBar2.Location = new Point(165, 55);
                progressBar2.Refresh();
                label18.Location = new Point(144, 24);
                label18.Visible = true;
                label18.Refresh();

                groupBox6.Visible = false;
                groupBox7.Visible = false;

                button4.Location = new Point(420, 52);
                button4.Visible = true;
                button4.Refresh();

                ListView.CheckedListViewItemCollection tempCollection = listView2.CheckedItems;

                LogoffCheckedArrayList.Clear();
                for (int i = 0; i < tempCollection.Count; i++)
                    LogoffCheckedArrayList.Add(tempCollection[i]);

                backgroundWorker4.RunWorkerAsync(LogoffCheckedArrayList);

            }
        }  // Logoff Button

        // Stop Button
        private void button7_Click(object sender, EventArgs e)
        {
            backgroundWorker3.CancelAsync();
            label18.Text = "Process is about to stop...";
            label18.Refresh();
        }

        private void backgroundWorker4_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            ArrayList collection = (ArrayList)e.Argument;
            System.ComponentModel.BackgroundWorker tempBGWorker = sender as System.ComponentModel.BackgroundWorker;

            int result;

            for (ind = 0; ind < collection.Count; ind++)
            {
                ListViewItem item1 = (ListViewItem)collection[ind];
                //MessageBox.Show(item1.SubItems[5].Text + "....." + item1.SubItems[1].Text);
                if (backgroundWorker4.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }

                tempBGWorker.ReportProgress(ind, "Checking");
                result = WTSLogoffSession(OpenServer(item1.SubItems[5].Text), Int32.Parse(item1.SubItems[1].Text), true);

                if (result == 0)
                    tempBGWorker.ReportProgress(ind, "Error");
            }
        }

        private void backgroundWorker4_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

            progressBar2.Value++;
            progressBar2.PerformStep();
            progressBar2.Refresh();

            if (progressBar2.Value > 150)
                progressBar2.Value = 5;

            if (e.UserState.ToString().Equals("Checking"))
            {
                ListViewItem item = (ListViewItem)LogoffCheckedArrayList[e.ProgressPercentage];
                label18.Text = "\"" + item.Text + "\" Session in \"" + item.SubItems[5].Text + "\" is logging off...";
                //MessageBox.Show("checking" + item.SubItems[5].Text);
            }
            else if (e.UserState.ToString().Equals("Error"))
            {
                ListViewItem item = (ListViewItem)LogoffCheckedArrayList[e.ProgressPercentage];
                label18.Text = "Unspecified Error occurred while Disconnecting the Session \"" + item.Text + "\" in \"" + item.SubItems[5].Text + "\" is being Disconnected...";
                //MessageBox.Show("Error in " + item.SubItems[5].Text);
            }
        }

        private void backgroundWorker4_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();

                // MessageBox.Show("Error occurred while stopping the process\n" + e.Error.Message, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                comboBox2.SelectedIndex = 0;

                progressBar2.Visible = false;
                progressBar2.Refresh();
                label18.Visible = false;
                label18.Refresh();

                groupBox6.Location = new Point(27, 20);
                groupBox6.Size = new Size(273, 80);
                groupBox6.Visible = true;

                groupBox7.Location = new Point(415, 20);
                groupBox7.Size = new Size(244, 80);
                groupBox7.Visible = true;

                button4.Visible = false;
                button4.Refresh();

                refreshAll();
            }
        }

        // Stop Button
        private void button4_Click(object sender, EventArgs e)
        {
            backgroundWorker4.CancelAsync();
            label14.Text = "Process is about to stop...";
            label14.Refresh();
        }  // Stop Button

        private void comboBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                button1_Click(sender, e);
            }
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                button1_Click(sender, e);
            }
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                button1_Click(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = comboBox1.Items[0];
            textBox1.Text = "";
            textBox2.Text = "";
            comboBox1_Leave(sender, e);
            textBox1_Leave(sender, e);
            textBox2_Leave(sender, e);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }

        private void backgroundWorker5_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                String adsPath = "LDAP://" + DomainName;
                DirectoryEntry domainEntry;
                backgroundWorker5.ReportProgress(3);
                if (!defaultUser)
                {
                    domainEntry = new DirectoryEntry(adsPath, UserName, Password);
                }
                else
                {
                    domainEntry = new DirectoryEntry(adsPath);
                }

                backgroundWorker5.ReportProgress(3);
                DirectorySearcher mySearcher = new DirectorySearcher(domainEntry);
                mySearcher.Filter = "(ObjectCategory=computer)";
                mySearcher.PageSize = 1000;
                //domainEntry.Children.SchemaFilter.Add("computer");
                foreach (SearchResult result in mySearcher.FindAll())
                {
                    if (backgroundWorker5.CancellationPending == true)
                    {
                        e.Cancel = true;
                        break;
                    }

                    backgroundWorker5.ReportProgress(3);
                    String name = result.GetDirectoryEntry().Name;
                    String addName = name.Substring(name.IndexOf('=') + 1);
                    ListViewItem item = new ListViewItem(addName);

                    String path = result.GetDirectoryEntry().Path;
                    String addPath = path.Substring(path.IndexOf(',') + 1);
                    item.SubItems.Add(addPath);
                    AllComputersList.Add(addName);
                    AllComputersLocationList.Add(addPath);
                    backgroundWorker5.ReportProgress(1, item);
                    System.Threading.Thread.Sleep(1);

                }

                domainEntry.Dispose();

            }   // try
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                backgroundWorker5.ReportProgress(2, "  The network path was not found ");
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                backgroundWorker5.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password");
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                backgroundWorker5.ReportProgress(2, "Unspecified Error" + ex.Message);
            }
            catch (Exception ex)
            {
                backgroundWorker5.ReportProgress(2, "Logon Failure. Please Enter Correct Username & Password"); 
            }

        }

        private void backgroundWorker5_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar3.Value += 3;
            progressBar3.Refresh();

            if (progressBar3.Value > 180)
                progressBar3.Value = 5;

            if (e.ProgressPercentage == 1)
            {
                ListViewItem item = (ListViewItem)e.UserState;
                listView1.Items.Add(item);
                label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label17.Refresh();
            }
            else if (e.ProgressPercentage == 2)
            {
                MessageBox.Show((String)e.UserState, " Error Message ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void backgroundWorker5_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            progressBar3.Visible = false;
            progressBar3.Refresh();
            button5.Visible = false;
            button5.Refresh();
            label14.Visible = false;
            label14.Refresh();

            if (listView1.Items.Count > 0)
            {
                // For Select All Purpose
                textBox3.Visible = true;
                pictureBox1.Visible = true;
                textBox3_Leave(sender, e);

                CheckBox chkBox = new CheckBox();
                chkBox.Checked = false;
                chkBox.Name = "ToSelectAllComputers";
                chkBox.Size = new System.Drawing.Size(13, 13);
                chkBox.Location = new System.Drawing.Point(5, 2);
                chkBox.CheckedChanged += SelectAllComputers;
                listView1.Controls.Add(chkBox);


                label17.Text = "Total Number of Computers  : " + listView1.Items.Count;
                label17.Refresh();

                CheckBox cb = (CheckBox)listView1.Controls["ToSelectAllComputers"];
                cb.Checked = true;
                cb.Checked = false;

                listView1.Sorting = SortOrder.Ascending;
                listView1.Sort();
                listView1.Refresh();
            }
            recolour();

        }

        private void textBox3_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.Focus();
                pictureBox1_Click(sender, e);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox1_Leave(sender, e);

            textBox2.Text = "";
            textBox2_Leave(sender, e);

            textBox3.Text = "";
            textBox3_Leave(sender, e);

            listView1.Items.Clear();
            AllComputersList.Clear();
            AllComputersLocationList.Clear();
            computerList.Clear();

            label17.Text = "Total Number of Computers  : " + 0;
            ColumnHeader header = listView1.Columns[1];
            header.Text = "Container Name ";

            pictureBox9.Visible = true;
            pictureBox7.Visible = true;
            pictureBox5.Visible = false;
            pictureBox8.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            backgroundWorker5.CancelAsync();
            label14.Text = "Process is about to stop...";
            label14.Refresh();
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

        private void listView2_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView2_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void listView2_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;

                using (var headerFont = new Font("Microsoft Sans Serif", 9 , FontStyle.Regular))
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(141, 148, 154)), e.Bounds);
                    e.Graphics.DrawString(e.Header.Text, headerFont,
                        Brushes.White, e.Bounds, sf);
                }
            }
        }
    }
}
