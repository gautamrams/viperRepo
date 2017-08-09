using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Diagnostics;
using GetDomains;
using GetDuplicates;
//using fin_design;
using ActiveDirectoryReplicationManager;
using TerminalSessionManager;
using SPMTool;
using LocalUserManagement;
using SendGreetingcheck;
using ADLDSManagement;
//using 1DnsReporter;

using System.Runtime.InteropServices;



namespace FreeTool
{

    public partial class Form1 : Form
    {
        
        bool shrink1 = false, shrink2 = false, shrink3 = false, shrink4 = false, shrink5 = false,shrink6 = false;
        Form f = null,frontpage=null,notification=null;
        String uripath=null,path = null, path1=null;
        Process p = null;
        public Form1()
        {
           
            InitializeComponent();
            
        }       
        bool isFrameWorkInstalled()
        {
            String windirpath = System.Environment.GetEnvironmentVariable("windir");

            windirpath = windirpath + "\\Microsoft.NET\\Framework\\v2.0.50727";
           
            //MessageBox.Show("windirpath " + windirpath);
            if (System.IO.Directory.Exists(windirpath))
            {
                return true;
            }
            else
            {
                windirpath = System.Environment.GetEnvironmentVariable("windir"); 
                windirpath = windirpath + "\\Microsoft.NET\\Framework64\\v2.0.50727"; 
                //MessageBox.Show("windirpath " + windirpath);	               
                return System.IO.Directory.Exists(windirpath);                
            }
             
            
            
            
        }
        bool isPowerShellInstalled()
        {
            String windirpath = System.Environment.GetEnvironmentVariable("windir");
            //MessageBox.Show("windirpath " + windirpath);
            String checkpath = windirpath + "\\syswow64\\WindowsPowerShell\\v1.0\\powershell.exe";
            //MessageBox.Show("" + System.IO.File.Exists(checkpath));
            if (System.IO.File.Exists(checkpath))
            {
                return true;
            }
            else
            {
                checkpath = null;
                checkpath = windirpath + "\\system32\\windowspowershell\\v1.0\\powershell.exe";
               // MessageBox.Show("" + System.IO.File.Exists(checkpath));
                return System.IO.File.Exists(checkpath);
            }
        }
        void p_Exited(object sender, EventArgs e)
        {
            //MessageBox.Show("this: " + this + "and state:" + this.WindowState);
            //MessageBox.Show("" + p);
            p.Close();
            try
            {
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception exception)
            {
                //MessageBox.Show("" + exception.Message, "Free ADTolls Error Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        void NotificationMsg()
        {
            notification = new Form3();
            notification.MdiParent = this;
            notification.Dock = DockStyle.Fill;
            notification.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(notification);
            notification.Show();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            uripath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            path = new Uri(uripath).LocalPath;
            //NotificationMsg();
            //MessageBox.Show("path : " + path );
            if (!isFrameWorkInstalled())
            {
                MessageBox.Show("You need to install .NET Framework 2.0 or above and PowerShell", "AD Tools Information", MessageBoxButtons.OK);
                Application.Exit();
            }
            if (isPowerShellInstalled())
            {
                 if (f != null)
                    {
                        f.Close();
                    }
                    f = new Form2();
                    f.MdiParent = this;
                    f.Dock = DockStyle.Fill;
                    f.FormBorderStyle = FormBorderStyle.None;
                    panel2.Controls.Add(f);
                    f.Show();
                    
                    //MessageBox.Show("Registering Cmdlet:" + path + "\\GetDomains.bat");
                    //p = new Process();
                    
                    //p.StartInfo.FileName = path + "\\GetDomains.bat";
                    //p.StartInfo.UseShellExecute = false;
                    //p.StartInfo.CreateNoWindow = true;                   
                    //p.Start();
                    //p.WaitForExit();
                    //p.Close(); 
                   //notification.Close();                    
               
            }
            if (f != null)
            {
                f.Close();
            }
            f = new Form2();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(f);
            f.Show();
            label3_Click(sender, e);
            label1_Click(sender, e);
            label4_Click(sender, e);
            label9_Click(sender, e);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        //private void button1_Click(object sender, EventArgs e)
        //{

        //}                     

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/free-windows-active-directory-tools/free-active-directory-tools-index.html");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore.exe", "http://forums.manageengine.com?ftid=49000002687179");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/free-windows-active-directory-tools/support.html");
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/ad-manager/index.html");
        }

        // Visual C++ free tools
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel7.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel7.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\CSVGenerator.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {
                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel7.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                //MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
            
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel5.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel10.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\EmptyPasswordChecker.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {

                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();
                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel5.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                //MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
            

        }

        private void linkLabel15_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel15.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel15.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel10.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\Password_Policy_Manager.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {

                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();
                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel15.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                //MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }

        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel13.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel13.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel10.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\ADQuery.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {                
                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel13.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                //MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
            
        }

        private void linkLabel19_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel19.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel19.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel10.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\1DnsReporter.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {

                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel19.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                //MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
            
        }

        /*private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (isPowerShellInstalled())
            {
                if (f != null)
                {
                    f.Close();
                }
                f = new SendGreetingcheck.LastLogonTool();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();
                //f = new GetDomains.Form1();
                //f.MdiParent = this;
                //f.Dock = DockStyle.Fill;
               // f.FormBorderStyle = FormBorderStyle.None;
                //panel2.Controls.Add(f);
                //f.Show();

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx");
            }
            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\LastLogonToolMFC.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            try
            {
                p = new Process();
                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                //MessageBox.Show("Process before close" + p.ToString());
                p.Close();
                //MessageBox.Show("Process after close" + p.ToString());
            }
            catch (Win32Exception w)
            {
                p.Close();
                // MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
        }*/

        private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel14.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel14.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel10.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            
            if (isPowerShellInstalled())
            {
                if (f != null)
                {
                    f.Close();
                }
                f = new SendGreetingcheck.LastLogonTool();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();
                //f = new GetDomains.Form1();
                //f.MdiParent = this;
                //f.Dock = DockStyle.Fill;
               // f.FormBorderStyle = FormBorderStyle.None;
                //panel2.Controls.Add(f);
                //f.Show();

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
           
        }

       /*private void linkLabel14_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\LastLogonToolMFC.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            try
            {
                p = new Process();
                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                //MessageBox.Show("Process before close" + p.ToString());
                p.Close();
                //MessageBox.Show("Process after close" + p.ToString());
            }
            catch (Win32Exception w)
            {
                p.Close();
                // MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
        }*/

        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel12.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel12.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel10.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\DCMonitoringTool.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            try
            {
                p = new Process();
                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                //MessageBox.Show("Process before close" + p.ToString());
                p.Close();
                //MessageBox.Show("Process after close" + p.ToString());

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel12.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                // MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }        

        //PowerShell Tools

        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel11.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel10.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            
            if (isPowerShellInstalled())
            {
                if (f != null)
                {
                    f.Close();
                }
                f = new GetDomains.Form1();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel10.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel10.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;
            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            if (isPowerShellInstalled())
            {
                if (f != null)
                {
                    f.Close();
                }
                //f = new fin_design.Form1();
                f = new LocalUserManagement.Form1();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                //System.Collections.IEnumerator ie= f.Controls.GetEnumerator();
                f.Show();

                Control.ControlCollection coll = f.Controls;

                foreach (Control c in coll)
                {
                    if (c.Name.Equals("pictureBox2"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(590, 9);
                    }
                    else if (c.Name.Equals("pictureBox4"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(590, 9);
                    }
                    else if (c.Name.Equals("pictureBox3"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(627, 9);
                    }
                    else if (c.Name.Equals("pictureBox6"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(627, 9);
                    }
                    else if (c.Name.Equals("groupBox1"))
                    {
                        Control.ControlCollection coll1 = c.Controls;
                        foreach (Control c1 in coll1)
                        {
                            if (c1.Name.Equals("comboBox1"))
                            {
                                ComboBox te = (ComboBox)c1;
                                te.ForeColor = Color.Black;
                                c1.Size = new Size(121, 23);
                                c1.Location = new Point(30, 44);
                            }
                            else if (c1.Name.Equals("textBox1"))
                            {
                                c1.Size = new Size(121, 21);
                                c1.Location = new Point(206, 44);
                            }
                            else if (c1.Name.Equals("textBox2"))
                            {
                                c1.Size = new Size(121, 21);
                                c1.Location = new Point(367, 44);
                            }
                            else if (c1.Name.Equals("button1"))
                            {
                                c1.Size = new Size(121, 23);
                                c1.Location = new Point(206, 76);
                            }
                            else if (c1.Name.Equals("button2"))
                            {
                                c1.Size = new Size(123, 23);
                                c1.Location = new Point(365, 76);
                            }
                            else if (c1.Name.Equals("label12"))
                            {
                                c1.Size = new Size(79, 15);
                                c1.Location = new Point(203, 21);
                            }
                            else if (c1.Name.Equals("label13"))
                            {
                                c1.Size = new Size(70, 15);
                                c1.Location = new Point(362, 22);
                            }

                        } // foreach
                    } // elseif
                    else if (c.Name.Equals("groupBox3"))
                    {
                        c.Size = new Size(518, 300);
                        c.Refresh();
                        Control.ControlCollection coll1 = c.Controls;
                        foreach (Control c1 in coll1)
                        {
                            if (c1.Name.Equals("textBox3"))
                            {
                                c1.Size = new Size(96, 21);
                                c1.Location = new Point(351, 25);
                            }
                            else if (c1.Name.Equals("pictureBox1"))
                            {
                                c1.Size = new Size(41, 19);
                                c1.Location = new Point(447, 26);
                            }
                            else if (c1.Name.Equals("listView1"))
                            {
                                //c1.Size = new Size(458, 245);
                                c1.Size = new Size(458, 190);
                                c1.Location = new Point(30, 48);
                            }
                            else if (c1.Name.Equals("progressBar1"))
                            {
                                c1.Size = new Size(314, 23);
                                //c1.Location = new Point(32, 318);
                                c1.Location = new Point(32, 263);
                            }
                            else if (c1.Name.Equals("progressBar3"))
                            {
                                c1.Size = new Size(226, 23);
                                //c1.Location = new Point(32, 318);
                                c1.Location = new Point(100, 263);
                            }
                            else if (c1.Name.Equals("label14"))
                            {
                                c1.Size = new Size(58, 15);
                                //c1.Location = new Point(27, 295);
                                c1.Location = new Point(27, 240);
                            }
                            else if (c1.Name.Equals("label20"))
                            {
                                c1.Size = new Size(58, 15);
                                //c1.Location = new Point(27, 295);
                                c1.Location = new Point(27, 240);
                            }
                            else if (c1.Name.Equals("button3"))
                            {
                                c1.Size = new Size(121, 23);
                                //c1.Location = new Point(199, 318);
                                c1.Location = new Point(199, 263);
                            }
                            else if (c1.Name.Equals("button12"))
                            {
                                c1.Size = new Size(119, 23);
                                //c1.Location = new Point(362, 318);
                                c1.Location = new Point(362, 263);
                            }
                            else if (c1.Name.Equals("button13"))
                            {
                                c1.Size = new Size(119, 23);
                                //c1.Location = new Point(362, 318);
                                c1.Location = new Point(362, 263);
                            }
                        } // foreach
                        
                    } // elseif
                    else if (c.Name.Equals("groupBox4"))
                    {
                        Control.ControlCollection coll1 = c.Controls;
                        foreach (Control c1 in coll1)
                        {
                            if (c1.Name.Equals("label18"))
                            {
                                c1.Location = new Point(143, 25);
                            }
                            else if (c1.Name.Equals("progressBar2"))
                            {
                                c1.Size = new Size(227, 23);
                                c1.Location = new Point(171, 52);
                            }
                            else if (c1.Name.Equals("button11"))
                            {
                                c1.Size = new Size(110, 23);
                                c1.Location = new Point(420, 52);
                            }
                            else if (c1.Name.Equals("listView2"))
                            {
                                //c1.Size = new Size(669, 354);
                                c1.Size = new Size(669, 334);
                                c1.Location = new Point(5, 92);
                            }
                            else if (c1.Name.Equals("groupBox7"))
                            {
                                Control.ControlCollection coll2 = c1.Controls;
                                foreach (Control c2 in coll2)
                                {
                                    if (c2.Name.Equals("label16"))
                                    {
                                        c2.Size = new Size(70, 15);
                                        c2.Location = new Point(87, 30);
                                    }
                                    else if (c2.Name.Equals("comboBox2"))
                                    {
                                        c2.Size = new Size(121, 23);
                                        c2.Location = new Point(166, 27);
                                    }
                                    else if (c2.Name.Equals("label15"))
                                    {
                                        c2.Size = new Size(53, 15);
                                        c2.Location = new Point(376, 30);
                                    }
                                    else if (c2.Name.Equals("comboBox3"))
                                    {
                                        c2.Visible = true;
                                        c2.Size = new Size(121, 23);
                                        c2.Location = new Point(440, 27);
                                    }
                                } // foreach
                            } //elseif
                            else if (c1.Name.Equals("groupBox6"))
                            {
                                c1.Location = new Point(5, 432);
                                //c1.Size = new Size(245, 90);
                                c1.Size = new Size(245, 70);
                                Control.ControlCollection coll2 = c1.Controls;
                                foreach (Control c2 in coll2)
                                {
                                    if (c2.Name.Equals("button10"))
                                    {
                                        c2.Size = new Size(80, 23);
                                        //c2.Location = new Point(17, 43);
                                        c2.Location = new Point(15, 32);
                                    }
                                    else if (c2.Name.Equals("button9"))
                                    {
                                        c2.Size = new Size(108, 23);
                                        //c2.Location = new Point(118, 43);
                                        c2.Location = new Point(118, 32);
                                    }
                                } // foreach
                            } // elseif
                            else if (c1.Name.Equals("groupBox5"))
                            {
                                c1.Visible = true;
                                c1.Refresh();
                                //c1.Location = new Point(301, 452);
                                c1.Location = new Point(301, 432);
                                //c1.Size = new Size(370, 90);
                                c1.Size = new Size(370, 70);
                                Control.ControlCollection coll2 = c1.Controls;
                                foreach (Control c2 in coll2)
                                {
                                    if (c2.Name.Equals("button8"))
                                    {
                                        c2.Size = new Size(75, 23);
                                        //c2.Location = new Point(19, 42);
                                        c2.Location = new Point(19, 32);
                                    }
                                    else if (c2.Name.Equals("button4"))
                                    {
                                        c2.Size = new Size(75, 23);
                                        //c2.Location = new Point(110, 42);
                                        c2.Location = new Point(110, 32);
                                    }
                                    else if (c2.Name.Equals("button6"))
                                    {
                                        c2.Size = new Size(66, 23);
                                        //c2.Location = new Point(199, 42);
                                        c2.Location = new Point(199, 32);
                                    }
                                    else if (c2.Name.Equals("button5"))
                                    {
                                        c2.Size = new Size(75, 23);
                                        //c2.Location = new Point(277, 42);
                                        c2.Location = new Point(277, 32);
                                    }
                                } // foreach
                            } // elseif

                        } // foreach
                    } // elseif
                } // outer "foreach"

                panel2.Refresh();
                
                
            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel6.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel6.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel10.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            if (isPowerShellInstalled())
            {

                if (f != null)
                {
                    f.Close();
                }
                f = new GetDuplicates.Form1();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();
                
            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel8.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel8.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel10.Image = null;
            linkLabel9.Image = null;
            
            try
            {

                if (isPowerShellInstalled())
                {
                    if (f != null)
                    {
                        f.Close();
                    }
                    try
                    {

                        f = new SPMTool.Form2();
                        f.MdiParent = this;
                        f.Dock = DockStyle.Fill;
                        f.FormBorderStyle = FormBorderStyle.None;
                        panel2.Controls.Add(f);
                        f.Show();

                    }
                    catch (System.IO.FileNotFoundException fe)
                    {
                        /*if (f != null)
                        {
                            f.Close();
                        }
                        f = new SPMTool.Form2();
                        f.MdiParent = this;
                        f.Dock = DockStyle.Fill;
                        f.FormBorderStyle = FormBorderStyle.None;
                        panel2.Controls.Add(f);
                        f.Show();*/
                        //MessageBox.Show("It will give report only when Microsoft Office SharePoint Server 2007 is installed in your machine", "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {

                        if (f != null)
                        {
                            f.Close();
                        }
                        f = new Form2();
                        f.MdiParent = this;
                        f.Dock = DockStyle.Fill;
                        f.FormBorderStyle = FormBorderStyle.None;
                        panel2.Controls.Add(f);
                        f.Show();
                        MessageBox.Show("" + ex.Message, "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (System.IO.FileNotFoundException fe)
            {
                MessageBox.Show("It will give report only when Microsoft Office SharePoint Server 2007 is installed in your machine", "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception excep)
            {
                MessageBox.Show(""+excep.Message,"AD Tools Information",MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel9.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel9.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel10.Image = null;

            try
            {
            if (isPowerShellInstalled())
            {
                if (f != null)
                {
                    f.Close();
                }
                try
                {
                f = new SPMTool.Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);               
                
                

                    f.Show();
                }
                catch (System.IO.FileNotFoundException fe)
                {
                   /* if (f != null)
                    {
                        f.Close();
                    }
                    f = new SPMTool.Form2();
                    f.MdiParent = this;
                    f.Dock = DockStyle.Fill;
                    f.FormBorderStyle = FormBorderStyle.None;
                    panel2.Controls.Add(f);
                    f.Show();*/
                   // MessageBox.Show("It will give report only when Microsoft Office SharePoint Server 2007 is installed in your machine", "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                  //  MessageBox.Show("" + ex.ToString());
                    if (f != null)
                    {
                        f.Close();
                    }
                    f = new Form2();
                    f.MdiParent = this;
                    f.Dock = DockStyle.Fill;
                    f.FormBorderStyle = FormBorderStyle.None;
                    panel2.Controls.Add(f);
                    f.Show();
                    MessageBox.Show("" + ex.Message,"AD Tools Information",MessageBoxButtons.OK, MessageBoxIcon.Error);
                } 
            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            }
            catch (System.IO.FileNotFoundException fe)
            {
                MessageBox.Show("It will give report only when Microsoft Office SharePoint Server 2007 has installed in your machine", "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception excep)
            {
                MessageBox.Show(""+excep.Message,"AD Tools Information",MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        //Image increase and decrease

        private void label1_Click(object sender, EventArgs e)
        {

            linkLabel20.Image = null;
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));


            linkLabel17.Image = null;
            linkLabel10.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            
            
            //MessageBox.Show(panel6.Height + "Panel 6");

            if (shrink2)
            {
                Bitmap image = new Bitmap(path + "./..//images//menu2_active2.png");
                label1.Image = image;
             //   label1.ImageAlign = ContentAlignment.MiddleRight;
                panel6.Height = panel6.Height + 98;
                shrink2 = false;
            }
            else
            {
                Bitmap image = new Bitmap(path + "./..//images//menu2_deactive2.png");
                label1.Image = image;
             //   label1.ImageAlign = ContentAlignment.MiddleRight;
                panel6.Height = panel6.Height - 98;
                shrink2 = true;

            }
            if (f != null)
            {
                f.Close();
            }
            f = new Form2();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(f);
            f.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {

            linkLabel20.Image = null;
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));


            linkLabel17.Image = null;
            linkLabel10.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            
            
            //MessageBox.Show(panel6.Height + "Panel 6");

            if (shrink5)
            {
                Bitmap image = new Bitmap(path + "./..//images//menu4_active.png");
                label9.Image = image;
          //      label9.ImageAlign = ContentAlignment.MiddleRight;
                panel11.Height = panel11.Height + 33;
                shrink5 = false;
            }
            else
            {
                Bitmap image = new Bitmap(path + "./..//images//menu4_deactive.png");
                label9.Image = image;
          //      label9.ImageAlign = ContentAlignment.MiddleRight;
                panel11.Height = panel11.Height - 33;
                shrink5 = true;

            }
            if (f != null)
            {
                f.Close();
            }
            f = new Form2();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(f);
            f.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(panel4.Height + "Panel 4");
 
            
            if (shrink1)
            {
               
                Bitmap image = new Bitmap(path + "./..//images//menu_active.gif");
                label2.Image = image;
             //   label2.ImageAlign = ContentAlignment.MiddleRight;
                panel4.Height = panel4.Height + 90;
                shrink1 = false;
            }
            else
            {
                
                Bitmap image = new Bitmap(path + "./..//images//menu_deactive.gif");
                label2.Image = image;
                label2.ImageAlign = ContentAlignment.MiddleRight;
                panel4.Height = panel4.Height - 90;
                shrink1 = true;
            }
            if (f != null)
            {
                f.Close();
            }
           
            f = new Form2();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(f);
            f.Show();
           
        }

        private void label3_Click(object sender, EventArgs e)
        {
            linkLabel20.Image = null;
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));


            linkLabel17.Image = null;
            linkLabel10.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            //MessageBox.Show(panel4.Height + "Panel 4");
            if (shrink3)
            {
                Bitmap image = new Bitmap(path + "./..//images//menu1_active.png");
                label3.Image = image;
              //  label3.ImageAlign = ContentAlignment.MiddleRight;
                panel5.Height = panel5.Height + 65;
                shrink3 = false;
            }
            else
            {
                Bitmap image = new Bitmap(path + "./..//images//menu1_deactive.png");
                label3.Image = image;
             //   label3.ImageAlign = ContentAlignment.MiddleRight;
                panel5.Height = panel5.Height - 65;
                shrink3 = true;
            }
            if (f != null)
            {
                f.Close();
            }
            f = new Form2();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(f);
            f.Show();

        }

        private void label4_Click(object sender, EventArgs e)
        {

            linkLabel20.Image = null;
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));


            linkLabel17.Image = null;
            linkLabel10.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;


            if (shrink4)
            {
                Bitmap image = new Bitmap(path + "./..//images//menu3_active.png");
                label4.Image = image;
             //   label4.ImageAlign = ContentAlignment.MiddleRight;
                panel7.Height = panel7.Height + 115;
               
                shrink4 = false;
            }
            else
            {
                Bitmap image = new Bitmap(path + "./..//images//menu3_deactive.png");
                label4.Image = image;
             //   label4.ImageAlign = ContentAlignment.MiddleRight;
                panel7.Height = panel7.Height - 115;
               
                shrink4 = true;
            }
            if (f != null)
            {
                f.Close();
            }
            f = new Form2();
            f.MdiParent = this;
            f.Dock = DockStyle.Fill;
            f.FormBorderStyle = FormBorderStyle.None;
            panel2.Controls.Add(f);
            f.Show();


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/ad-manager/index.html");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/self-service-password/index.html");      
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/active-directory-audit/index.html");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("IExplore.exe", path + "./../help/index.html");
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel16_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel16.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel16.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel10.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            //MessageBox.Show("" + System.Environment.CurrentDirectory);
            String path1 = path + "\\DMZAnalyzer.exe";
            //MessageBox.Show(path1);
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {

                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel16.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                //MessageBox.Show("Error code" + w.ErrorCode);
                // MessageBox.Show("Error base Exception " + w.GetBaseException());
                MessageBox.Show("You have to install  Microsoft Visual C++ libraries from \n http://www.microsoft.com/downloads/details.aspx?displaylang=en&FamilyID=32bc1bee-a3f9-4c13-9c99-220b62a191ee", "AD Tools Information", MessageBoxButtons.OK);
            }
              
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel17_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel17.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel17.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel10.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;
            
            if (isPowerShellInstalled())
            {

                if (f != null)
                {
                    f.Close();
                }
                f = new TerminalSessionManager.Form1();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                Control.ControlCollection coll = f.Controls;

                foreach (Control c in coll)
                {
                    if (c.Name.Equals("pictureBox2"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(590, 9);
                    }
                    else if (c.Name.Equals("pictureBox4"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(590, 9);
                    }
                    else if (c.Name.Equals("pictureBox3"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(627, 9);
                    }
                    else if (c.Name.Equals("pictureBox6"))
                    {
                        c.Size = new Size(35, 34);
                        c.Location = new Point(627, 9);
                    }
                    else if (c.Name.Equals("groupBox1"))
                    {
                        Control.ControlCollection coll1 = c.Controls;
                        foreach (Control c1 in coll1)
                        {
                            if (c1.Name.Equals("comboBox1"))
                            {
                                c1.Size = new Size(121, 23);
                                c1.Location = new Point(30, 44);
                            }
                            else if (c1.Name.Equals("textBox1"))
                            {
                                c1.Size = new Size(121, 21);
                                c1.Location = new Point(206, 44);
                            }
                            else if (c1.Name.Equals("textBox2"))
                            {
                                c1.Size = new Size(121, 21);
                                c1.Location = new Point(367, 44);
                            }
                            else if (c1.Name.Equals("button1"))
                            {
                                c1.Size = new Size(121, 23);
                                c1.Location = new Point(206, 76);
                            }
                            else if (c1.Name.Equals("button2"))
                            {
                                c1.Size = new Size(123, 23);
                                c1.Location = new Point(365, 76);
                            }
                            else if (c1.Name.Equals("label12"))
                            {
                                c1.Size = new Size(79, 15);
                                c1.Location = new Point(203, 21);
                            }
                            else if (c1.Name.Equals("label13"))
                            {
                                c1.Size = new Size(70, 15);
                                c1.Location = new Point(362, 22);
                            }

                        } // foreach
                    } // elseif
                    else if (c.Name.Equals("groupBox3"))
                    {
                        c.Size = new Size(518, 300);
                        c.Refresh();
                        Control.ControlCollection coll1 = c.Controls;
                        foreach (Control c1 in coll1)
                        {
                            if (c1.Name.Equals("textBox3"))
                            {
                                c1.Size = new Size(96, 21);
                                c1.Location = new Point(351, 25);
                            }
                            else if (c1.Name.Equals("pictureBox1"))
                            {
                                c1.Size = new Size(41, 19);
                                c1.Location = new Point(447, 26);
                            }
                            else if (c1.Name.Equals("label17"))
                            {
                                //c1.Size = new Size(458, 245);
                                //c1.Size = new Size(458, 190);
                                c1.Location = new Point(30, 30);
                            }
                            else if (c1.Name.Equals("listView1"))
                            {
                                //c1.Size = new Size(458, 245);
                                c1.Size = new Size(458, 190);
                                c1.Location = new Point(30, 48);
                            }
                            else if (c1.Name.Equals("progressBar1"))
                            {
                                c1.Size = new Size(314, 23);
                                //c1.Location = new Point(32, 318);
                                c1.Location = new Point(32, 263);
                            }
                            else if (c1.Name.Equals("progressBar3"))
                            {
                                c1.Size = new Size(235, 23);
                                //c1.Location = new Point(32, 318);
                                c1.Location = new Point(117, 263);
                            }
                            else if (c1.Name.Equals("label14"))
                            {
                                c1.Size = new Size(58, 15);
                                //c1.Location = new Point(27, 295);
                                c1.Location = new Point(27, 240);
                            }
                            else if (c1.Name.Equals("label15"))
                            {
                                c1.Size = new Size(58, 15);
                                //c1.Location = new Point(27, 295);
                                c1.Location = new Point(27, 240);
                            }
                            else if (c1.Name.Equals("button3"))
                            {
                                c1.Size = new Size(121, 23);
                                //c1.Location = new Point(199, 318);
                                c1.Location = new Point(199, 263);
                            }
                            else if (c1.Name.Equals("button12"))
                            {
                                c1.Size = new Size(119, 23);
                                //c1.Location = new Point(362, 318);
                                c1.Location = new Point(362, 263);
                            }
                            else if (c1.Name.Equals("button5"))
                            {
                                c1.Size = new Size(119, 23);
                                //c1.Location = new Point(362, 318);
                                c1.Location = new Point(362, 263);
                            }
                        } // foreach
                    } // elseif
                    else if (c.Name.Equals("groupBox4"))
                    {
                        Control.ControlCollection coll1 = c.Controls;
                        foreach (Control c1 in coll1)
                        {
                            if (c1.Name.Equals("label18"))
                            {
                                c1.Location = new Point(143, 25);
                            }
                            else if (c1.Name.Equals("progressBar2"))
                            {
                                c1.Size = new Size(227, 23);
                                c1.Location = new Point(171, 52);
                            }
                            else if (c1.Name.Equals("button11"))
                            {
                                c1.Size = new Size(110, 23);
                                c1.Location = new Point(420, 52);
                            }
                            else if (c1.Name.Equals("button7"))
                            {
                                c1.Size = new Size(110, 23);
                                c1.Location = new Point(420, 52);
                            }
                            else if (c1.Name.Equals("button4"))
                            {
                                c1.Size = new Size(110, 23);
                                c1.Location = new Point(420, 52);
                            }
                            else if (c1.Name.Equals("listView2"))
                            {
                                //c1.Size = new Size(669, 354);
                                c1.Size = new Size(632, 340);
                                c1.Location = new Point(27, 106);
                            }
                            else if (c1.Name.Equals("groupBox6"))
                            {
                                c1.Location = new Point(27, 20);
                                c1.Size = new Size(273, 80);

                                Control.ControlCollection coll2 = c1.Controls;
                                foreach (Control c2 in coll2)
                                {
                                    if (c2.Name.Equals("button10"))
                                    {
                                        c2.Size = new Size(86, 23);
                                        c2.Location = new Point(30, 35);
                                    }
                                    else if (c2.Name.Equals("button9"))
                                    {
                                        c2.Size = new Size(94, 23);
                                        c2.Location = new Point(153, 35);
                                    }
                                } //foreach
                            } //elseif
                            else if (c1.Name.Equals("groupBox7"))
                            {
                                Control.ControlCollection coll2 = c1.Controls;
                                foreach (Control c2 in coll2)
                                {
                                    if (c2.Name.Equals("label16"))
                                    {
                                        //c1.Size = new Size(669, 354);
                                        c2.Size = new Size(50, 15);
                                        c2.Location = new Point(36, 35);
                                    }
                                    else if (c2.Name.Equals("comboBox2"))
                                    {
                                        //c1.Size = new Size(669, 354);
                                        c2.Size = new Size(121, 23);
                                        c2.Location = new Point(95, 33);
                                    }
                                } //foreach
                            } //elseif
                        } //foreach
                    } //elseif

                } //foreach

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel18_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel18.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel18.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel10.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            if (isPowerShellInstalled())
            {
                if (f != null)
                {
                    f.Close();
                }
                f = new ActiveDirectoryReplicationManager.ReplicationManager();
                //f = new ActiveDirectoryReplicationManager.ReplicationManager();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel19_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel20_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            linkLabel20.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel20.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;

            linkLabel17.Image = null;
            linkLabel10.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;


            String path1 = path + "\\ServiceAccountManagement.exe";
           
            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {

                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel20.Image = null;
            }
            catch (Win32Exception w)
            {
                p.Close();
                
                MessageBox.Show("You have to install Dot Net Framework 4.0", "AD Tools Information", MessageBoxButtons.OK);
            }
            
        }

        private void label15_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, label15.DisplayRectangle, Color.FromArgb(221,221,221), ButtonBorderStyle.Solid);
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel1.DisplayRectangle, Color.FromArgb(221, 221, 221), ButtonBorderStyle.Solid);
        }

        private void panel17_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/free-windows-active-directory-tools/free-active-directory-tools-index.html");
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://forums.manageengine.com?ftid=49000002687179");
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/free-windows-active-directory-tools/support.html");
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", path + "./../help/index.html");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/exchange-reports/index.html");
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel2.Image = null;
            linkLabel20.Image = null;
            linkLabel17.Image = null;
            linkLabel10.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel6.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;


            String path1 = path + "\\WeakPasswordUsers.exe";

            FreeTool.Form1.ActiveForm.WindowState = FormWindowState.Minimized;
            p = new Process();
            try
            {

                p.StartInfo.FileName = path1;
                p.EnableRaisingEvents = true;
                p.Exited += new EventHandler(p_Exited);
                p.Start();
                p.WaitForExit();
                p.Close();

                if (f != null)
                {
                    f.Close();
                }
                f = new Form2();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

                linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
                linkLabel1.Image = null;
            }

            catch (Win32Exception w)
            {
                p.Close();

                MessageBox.Show("You have to install Dot Net Framework 4.5.1", "AD Tools Information", MessageBoxButtons.OK);
            }

        }

        private void pictureBox16_Click(object sender, EventArgs e)
        {

        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));


            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel10.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            if (isPowerShellInstalled())
            {

                if (f != null)
                {
                    f.Close();
                }
                f = new ADLDSManagement.Form1();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void linkLabel2_LinkClicked_2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel2.Image = global::FreeTool.Properties.Resources.greentab;
            linkLabel2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            linkLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(114)))), ((int)(((byte)(121)))));

            linkLabel17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel1.Image = null;
            linkLabel17.Image = null;
            linkLabel20.Image = null;
            linkLabel13.Image = null;
            linkLabel14.Image = null;
            linkLabel10.Image = null;

            linkLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel7.Image = null;
            linkLabel5.Image = null;
            linkLabel19.Image = null;
            linkLabel11.Image = null;
            linkLabel12.Image = null;

            linkLabel18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));
            linkLabel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(97)))), ((int)(((byte)(103)))));

            linkLabel18.Image = null;
            linkLabel15.Image = null;
            linkLabel16.Image = null;
            linkLabel8.Image = null;
            linkLabel9.Image = null;

            if (isPowerShellInstalled())
            {

                if (f != null)
                {
                    f.Close();
                }
                f = new ADLDSManagement.Form1();
                f.MdiParent = this;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                panel2.Controls.Add(f);
                f.Show();

            }
            else
            {
                MessageBox.Show("You need to install PowerShell. You can download it from \n \n http://www.microsoft.com/technet/scriptcenter/topics/msh/download.mspx", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        }

        
    }
