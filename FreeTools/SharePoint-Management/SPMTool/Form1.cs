using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web.UI.WebControls.WebParts;
using System.Management.Automation; // Windows PowerShell namespace
using System.Management.Automation.Runspaces; // Windows PowerShell namespace
using System.Security;
using System.Collections.ObjectModel; // For the secure password
using Microsoft.PowerShell;


namespace SPMTool
{
    public partial class Form2 : Form
    {
        bool exception = false;
        Runspace remoteRunSpace = null;
        System.Windows.Forms.ListView lvs = null;
        System.Windows.Forms.ListView lvw = null;
        System.Windows.Forms.ListView lvf = null;
        System.Windows.Forms.ListView lvp = null;
        ListView lv = null;
        int i = 0;
        TreeNode farmnode = null;
        TreeNode serverNode = null;
        public Form2()
        {
            InitializeComponent();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                //MessageBox.Show("" + e.Node.Tag.ToString());
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            
            //Check SP is installed or not
            //Windows SharePoint Services Administration service started or not
            //SQLServer (SQLEXPRESS) is installed or not

            textBox2.Visible = false;
            label3.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
           // listBox1.Visible = false;
           // tableLayoutPanel1.Visible = false;
            farmnode = treeView1.Nodes.Add("Local Farm");
            try
            {
                Runspace runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                // As User name and password is needed to connect to remote server this always throws unnecessary exception in the start

                // create a pipeline and feed it the script text

             //   Pipeline pipeline = runspace.CreatePipeline();
            //    pipeline.Commands.AddScript("Add-PsSnapin Microsoft.SharePoint.PowerShell");
            //    pipeline.Commands.AddScript("$server=Get-SpServer");
           //     pipeline.Commands.AddScript("echo $server.Name");
            //    pipeline.Commands.Add("Out-String");
           //     Collection<PSObject> results = pipeline.Invoke();
          //      PSObject re = results[0];
          //      MessageBox.Show(re.ToString());
         //       server.Text = re.ToString();

            }
            catch (System.IO.FileNotFoundException fe)
            {

            }
            catch (System.Management.Automation.CommandNotFoundException ee)
            {
               // MessageBox.Show("Please Enter The Server Name And User Credentials", "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MessageBox.Show(ee.Message);
            }
            
            

        }
        public void initRunSpace()
        {
            //remoteRunSpace = null;
            try
            {
                openRunspace("http://" + server.Text + ":5985/wsman",
                   "http://schemas.microsoft.com/powershell/Microsoft.PowerShell",
                   username.Text,
                   password.Text,
                   ref remoteRunSpace);
            }
            catch(Exception e) {
                MessageBox.Show("" + e.Message, "Free AD Tool error Information", MessageBoxButtons.OK);
                return;
            }
        }
        public void openRunspace(string uri, string schema, string username, string livePass, ref Runspace remoteRunspace)
        {
            System.Security.SecureString password = new System.Security.SecureString();
            foreach (char c in livePass.ToCharArray())
            {
                password.AppendChar(c);
            }
            try
            {
                PSCredential psc = new PSCredential(username, password);
                WSManConnectionInfo rri = new WSManConnectionInfo(new Uri(uri), schema, psc);
                rri.AuthenticationMechanism = AuthenticationMechanism.Default;
                rri.ProxyAuthentication = AuthenticationMechanism.Negotiate;
                remoteRunspace = RunspaceFactory.CreateRunspace(rri);
                remoteRunspace.Open();
            }
            catch (InvalidRunspaceStateException ee)
            {
                exception = true;

            }
        }

        public void addSnappin()
        {
            StringBuilder stringBuilder = new StringBuilder();
            PowerShell powershell = PowerShell.Create();
            powershell.Runspace = remoteRunSpace;
            powershell.AddScript("Add-PSSnapin Microsoft.SharePoint.PowerShell");
            powershell.Invoke();
        }
        public Collection<PSObject> executeCommand(String command)
        {
            PowerShell powershell = PowerShell.Create();
            powershell.Runspace = remoteRunSpace;
            powershell.AddScript(command);
            Collection<PSObject> results = powershell.Invoke();
            return results;
        }

    /*    private void GetWebPartInventory(SPSite site)
        {
            lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700);
            lv.Columns.Add("WebPart Name", -2, System.Windows.Forms.HorizontalAlignment.Left); lv.Columns.Add("WebPart Title", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lv.GridLines = true;            
            lv.Scrollable = true;
            ListViewItem lvitem = null;
            SPListItemCollection collListItems = null;
            lv.Items.Clear();
            panel3.Controls.Clear();
            //MessageBox.Show("" + site);
            collListItems = obj.GetWebPartGallery(site);
            if (collListItems != null)
            {
                foreach (SPListItem listitem in collListItems)
                {
                    lvitem = new ListViewItem("" + listitem.Name);
                    lvitem.SubItems.Add("" + listitem.Title);
                    lv.Items.Add(lvitem);
                }
            }
            else
            {
                MessageBox.Show("WebPart is not available");
            }
            panel3.Controls.Add(lv);
        }
        private ListView GetWebPartPerPage_ForSite(SPWeb site)
        {
            lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700);
            lv.Columns.Add("Page URL", -2, System.Windows.Forms.HorizontalAlignment.Left); lv.Columns.Add("WebPart Title", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lv.Scrollable = true;  
            ListViewItem lvitem = null;
            SPFile file = null;

            //It will include webparts used in the site page.(always site  page ends with sitename + /default.aspx)
            file = site.GetFile(site.Url + "/default.aspx");
           // MessageBox.Show("file :" + site.Url);
            if (file != null && file.Exists)
            {
                //MessageBox.Show("file :" + file);
                SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
                SPLimitedWebPartCollection wc = wm.WebParts;
                //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);
                lvitem = new ListViewItem("" + file.ServerRelativeUrl);
                foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
                {
                    lvitem.SubItems.Add("" + webpart.EffectiveTitle);
                    lv.Items.Add(lvitem);
                    lvitem = new ListViewItem("");
                }
            }

            // MessageBox.Show("It will display all webparts in single site(Within Document Library) with page:"+site);
            foreach (SPList list in site.Lists)
            {
                foreach (SPListItem item in list.Items)
                {
                    if (item.ContentType != null && item.ContentType.Name.Equals("Document"))
                    {
                        file = item.File;
                        if (file != null && file.Exists && file.Url.Contains("aspx"))
                        {
                            SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
                            SPLimitedWebPartCollection wc = wm.WebParts;
                            //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);
                            lvitem = new ListViewItem("" + file.Url);
                            foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
                            {
                                lvitem.SubItems.Add("" + webpart.EffectiveTitle);
                                lv.Items.Add(lvitem);
                                lvitem = new ListViewItem("");
                            }
                        }
                    }
                }
            } //splist closed         
            return lv;
        }
        private void GetWebPartPerPage_ForSC(SPSite sitecollection)
        {
            lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700);
            lv.Columns.Add("Page URL", -2, System.Windows.Forms.HorizontalAlignment.Left); lv.Columns.Add("WebPart Title", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lv.Scrollable = true;  
            ListViewItem lvitem = null;
            SPFile file = null;
            foreach (SPWeb site in sitecollection.AllWebs)
            {
                //It will include webparts used in the site page.(always site  page ends with sitename + /default.aspx)
                file = site.GetFile(site.Url + "/default.aspx");
                // MessageBox.Show("file :" + site.Url);
                if (file != null && file.Exists)
                {
                    //MessageBox.Show("file :" + file);
                    SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
                    SPLimitedWebPartCollection wc = wm.WebParts;
                    //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);
                    lvitem = new ListViewItem("" + file.ServerRelativeUrl);
                    foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
                    {
                        lvitem.SubItems.Add("" + webpart.EffectiveTitle);
                        lv.Items.Add(lvitem);
                        lvitem = new ListViewItem("");
                    }
                }

                // MessageBox.Show("It will display all webparts in single site(Within Document Library) with page:"+site);
                foreach (SPList list in site.Lists)
                {
                    foreach (SPListItem item in list.Items)
                    {
                        if (item.ContentType != null && item.ContentType.Name.Equals("Document"))
                        {
                            file = item.File;
                            if (file != null && file.Exists && file.Url.Contains("aspx"))
                            {
                                SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
                                SPLimitedWebPartCollection wc = wm.WebParts;
                                //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);
                                lvitem = new ListViewItem("" + file.Url);
                                foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
                                {
                                    lvitem.SubItems.Add("" + webpart.EffectiveTitle);
                                    lv.Items.Add(lvitem);
                                    lvitem = new ListViewItem("");
                                }
                            }
                        }
                    }
                } //splist closed         
            }
            panel3.Controls.Add(lv);
        }*/
   /*     private ArrayList CheckAllWebPartPages_ForWebpart(SPWeb site, String webpartname)
        {
            ArrayList allpages = new ArrayList();
            SPFile file = null;
            foreach (SPList list in site.Lists)
            {
                foreach (SPListItem item in list.Items)
                {
                    if (item.ContentType != null && item.ContentType.Name.Equals("Document"))
                    {
                        file = item.File;
                        if (file != null && file.Exists && file.Url.Contains("aspx"))
                        {
                            SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
                            SPLimitedWebPartCollection wc = wm.WebParts;
                            //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);                                                                               
                            foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
                            {
                                if (webpart != null && webpart.Title.Equals(webpartname))
                                {
                                    allpages.Add("" + file.ServerRelativeUrl);
                                }
                            }
                        }
                    }
                }
            } //splist closed
            //For site pages 
            file = site.GetFile(site.Url + "/default.aspx");
            // MessageBox.Show("file :" + site.Url);
            if (file != null && file.Exists)
            {
                //MessageBox.Show("file :" + file);
                SPLimitedWebPartManager wm = file.GetLimitedWebPartManager(PersonalizationScope.Shared);
                SPLimitedWebPartCollection wc = wm.WebParts;
                //u can also use web.GetLimitedWebPartManager(file.url,personalizationScopee.shared);

                foreach (Microsoft.SharePoint.WebPartPages.WebPart webpart in wc)
                {
                    if (webpart != null && webpart.Title.Equals(webpartname))
                    {
                        allpages.Add("" + file.ServerRelativeUrl);
                    }
                }
            }
            return allpages;
        }*/
      /*  private void GetPagePerWebPart(SPSite site1)
        {
            String webpartname = null; int j = 0, flag, webcollncnt = 0;
            String siteurl = site1.Url;
            SPListItemCollection collListItems = obj.GetWebPartGallery(site1);
            if (collListItems != null)
            {
                lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700);
                lv.Scrollable = true;  
                lv.Columns.Add("WebPart Name", -2, System.Windows.Forms.HorizontalAlignment.Left); lv.Columns.Add("Page URL", -2, System.Windows.Forms.HorizontalAlignment.Left);
                ListViewItem lvitem = null;
                foreach (SPListItem listitem in collListItems)      //for all webparts we need to check all pages which is using this webpart
                {
                    webpartname = listitem.Title;
                    SPWebCollection webcollection = null;

                    SPSecurity.RunWithElevatedPrivileges(delegate()
                     {
                         SPSite site = new SPSite(siteurl);
                         webcollection = site.AllWebs;
                         site.Dispose();
                     });
                    flag = 1; webcollncnt = 0;
                    foreach (SPWeb web in webcollection)
                    {
                        ArrayList result = CheckAllWebPartPages_ForWebpart(web, webpartname);
                        webcollncnt++;
                        if (result.Count <= 0 && flag != 0 && webcollncnt == webcollection.Count) //no pages are used this webpartname and only one time is allowed inside for each webpart
                        {
                            flag = 0;
                            lvitem = new ListViewItem("" + webpartname);
                            lvitem.SubItems.Add("No webparts used");
                            lv.Items.Add(lvitem);
                        }
                        else
                        {
                            if (flag == 1)
                                lvitem = new ListViewItem("" + webpartname);
                            else
                                lvitem = new ListViewItem("");


                            for (j = 0; j < result.Count; j++)
                            {
                                flag = 0;
                                lvitem.SubItems.Add("" + result[j]);
                                lv.Items.Add(lvitem);
                                lvitem = new ListViewItem("");
                            }
                        }

                    }
                } //SPListItem closed
                panel3.Controls.Add(lv);
            }  //if close ie., if there is no web part gallery in selected sitecollection

        }
        private void GetListContent(SPList list)
        {
            //MessageBox.Show("List: " + e.Node.Tag);
            panel3.Controls.Clear();
            lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700); lv.Scrollable = true;  
            lv.Columns.Add("List Name", -2, System.Windows.Forms.HorizontalAlignment.Left); //lv.Columns.Add("List Title", -2, System.Windows.Forms.HorizontalAlignment.Left);
            ListViewItem lvitem = null;

            foreach (SPListItem listitem in list.Items)
            {
                lvitem = new ListViewItem("" + listitem.Name);
                lv.Items.Add(lvitem);
            }
            panel3.Controls.Add(lv);
        }
        private void GetAllFiles_ForSC(SPSite site1, int size,string mborkb, string criteria)
        {
            //MessageBox.Show("site: " + site1 + " Size:" + size + "  MB or kB :" + mborkb + " criteria:" + criteria);
            SPFile file = null;String  columnname = "File Size in";
            SPWebCollection webcollection = null;
            String siteurl = site1.Url;
            long filesize = 0, displayfilesize=0;
            if (mborkb.Equals("MB"))
            {
                columnname = columnname + " MB";
                filesize = (size * 1048576);
            }
            else
            {
                columnname = columnname + " KB";
                filesize = (size * 1024);
            }
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700);
                lv.Columns.Add(""+columnname, -2, System.Windows.Forms.HorizontalAlignment.Left); lv.Columns.Add("File URL", -2, System.Windows.Forms.HorizontalAlignment.Left);
                lv.GridLines = true;
                lv.Scrollable = true;                            
                ListViewItem lvitem = null;
                SPSite site = new SPSite(siteurl);
                webcollection = site.AllWebs;
                foreach (SPWeb web in webcollection)
                {
                    foreach (SPList list in web.Lists)
                    {
                        foreach (SPListItem item in list.Items)
                        {
                            file = item.File;                            
                            if (file != null && file.Exists)
                            {
                                if (criteria.Equals("Greater than") && (file.TotalLength > filesize) )
                                {
                                    if (mborkb.Equals("MB"))
                                    {
                                        displayfilesize = (file.TotalLength / 1048576);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    else
                                    {
                                        displayfilesize = (file.TotalLength / 1024);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    lvitem.SubItems.Add("" + file.Url);
                                    lv.Items.Add(lvitem);                                    
                                }
                                else if (criteria.Equals("Lesser than") && (file.TotalLength < filesize ))
                                {
                                    if (mborkb.Equals("MB"))
                                    {
                                        displayfilesize = (file.TotalLength / 1048576);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    else
                                    {
                                        displayfilesize = (file.TotalLength / 1024);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    lvitem.SubItems.Add("" + file.Url);
                                    lv.Items.Add(lvitem);
                                }
                            }
                        }
                    }
                    //foreach (SPFile file in filecollection)
                    //{
                    //    listBox1.Items.Add("" + file.TotalLength+ ": "+file.Url);
                    //}
                    web.Dispose();
                }
                site.Dispose();
                panel3.Controls.Add(lv);
            });

        }
        private void GetAllFiles_ForSite(SPWeb web, int size, string mborkb, string criteria)
        {
            //MessageBox.Show("site: " + web + " Size:" + size + "  MB or kB :" + mborkb + " criteria:" + criteria);
            SPFile file = null; String columnname = "File Size in";                     
            long filesize = 0, displayfilesize = 0;
            lv = new ListView(); lv.View = View.Details; lv.Bounds = new Rectangle(0, 0, 500, 700);
            lv.Scrollable = true;  
            if (mborkb.Equals("MB"))
            {
                columnname = columnname + " MB";
                filesize = (size * 1048576);
            }
            else
            {
                columnname = columnname + " KB";
                filesize = (size * 1024);
            }            
            lv.Columns.Add("" + columnname, -2, System.Windows.Forms.HorizontalAlignment.Left); lv.Columns.Add("File URL", -2, System.Windows.Forms.HorizontalAlignment.Left);
            ListViewItem lvitem = null;                      
            foreach (SPList list in web.Lists)
                    {
                        foreach (SPListItem item in list.Items)
                        {
                            file = item.File;                            
                            if (file != null && file.Exists)
                            {
                                if (criteria.Equals("Greater than") && (file.TotalLength > filesize) )
                                {
                                    if (mborkb.Equals("MB"))
                                    {
                                        displayfilesize = (file.TotalLength / 1048576);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    else
                                    {
                                        displayfilesize = (file.TotalLength / 1024);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    lvitem.SubItems.Add("" + file.Url);
                                    lv.Items.Add(lvitem);                                    
                                }
                                else if (criteria.Equals("Lesser than") && (file.TotalLength < filesize ))
                                {
                                    if (mborkb.Equals("MB"))
                                    {
                                        displayfilesize = (file.TotalLength / 1048576);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    else
                                    {
                                        displayfilesize = (file.TotalLength / 1024);
                                        lvitem = new ListViewItem("" + displayfilesize);
                                    }
                                    lvitem.SubItems.Add("" + file.Url);
                                    lv.Items.Add(lvitem);
                                }
                            }
                        }
                    }
                    //foreach (SPFile file in filecollection)
                    //{
                    //    listBox1.Items.Add("" + file.TotalLength+ ": "+file.Url);
                    //}
                    web.Dispose();
                    panel3.Controls.Add(lv);                                                           
        }
     */   private String GetErrorMsg()
        {
            if (comboBox1.Text != null && comboBox1.Text.Equals("List WebPart for each page in Site") || comboBox1.Text.Equals("File Size Report"))
            {

                return "Select Level 4 or Level 5";
            }
            else
            {
                return "Select Level 4";
            }
        }
       
        
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("comboBox1_SelectedIndexChanged");
            
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
           // MessageBox.Show("Select appropriate one.Don't type any thing.");
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            label4.Visible = true;
            textBox1.Text = e.Node.Text;
            textBox1.Tag = e.Node.Tag;
            panel3.Controls.Clear();
          /*  if (e.Node.Tag is SPList)
            {
                SPList list = (SPList)e.Node.Tag;
                pictureBox1.Visible = false;
                GetListContent(list);
                pictureBox1.Visible = true;
            }
            else
            {
               label4.Text=GetErrorMsg();
            }*/
            
        }

        private void go_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedItem != null && !comboBox1.SelectedItem.ToString().Equals("Select Report"))
                {
                    if (!textBox1.Text.Equals(""))
                    {
                        
                        if (comboBox1.SelectedItem.ToString().Equals("WebPart Inventory"))  //for webpart inventory report.,only for site collection
                        {
                            pictureBox1.Visible = true;
                            panel3.Controls.Add(pictureBox1);
                            WebpartInventory(textBox1.Text);
                            
                            panel3.Controls.Remove(pictureBox1);
                            pictureBox1.Visible = false;
                            
                        }
                        else if (comboBox1.SelectedItem.ToString().Equals("File Size Report"))
                        {
                            panel3.Controls.Clear();
                            //MessageBox.Show("File Size Report");
                            if (textBox2.Text != null && !textBox2.Text.Equals("") && comboBox2.Text != null && !comboBox2.Text.Equals("select") && comboBox3.Text != null && !comboBox3.Text.Equals("select") && comboBox4.Text != null && !comboBox4.Text.Equals("select"))
                            {
                                //MessageBox.Show("Criteria : "+comboBox3.Text +" size :"+ comboBox2.Text);
                                //MessageBox.Show("Text  " + comboBox3.SelectedText+" Item"+comboBox3.SelectedItem.ToString());
                                if (textBox2.Text.Trim().Contains("."))
                                {
                                    MessageBox.Show("Please Give integer value in size textbox");
                                }
                                else
                                {
                                    pictureBox1.Visible = true;
                                    panel3.Controls.Add(pictureBox1);
                                    FileSizeReport(textBox1.Text);
                                    panel3.Controls.Remove(pictureBox1);
                                    pictureBox1.Visible = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please give criteria to search");
                            }
                        }
                        else if (comboBox1.SelectedItem.ToString().Equals("List Sites for each  Webpart"))
                        {
                            panel3.Controls.Clear();
                            //MessageBox.Show("For SC Listing WebPart in Site for each page");
                            pictureBox1.Visible = true;
                            panel3.Controls.Add(pictureBox1);
                            SitePerWebpart(textBox1.Text);
                            panel3.Controls.Remove(pictureBox1);
                            pictureBox1.Visible = false;
                        }
                        else if (comboBox1.SelectedItem.ToString().Equals("List WebPart for each page in Site"))
                        {
                            panel3.Controls.Clear();
                            //MessageBox.Show("For SC Listing WebPart in Site for each page");
                            pictureBox1.Visible = true;
                            panel3.Controls.Add(pictureBox1);
                            listWebPartPage(textBox1.Text);
                            panel3.Controls.Remove(pictureBox1);
                            pictureBox1.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("" + GetErrorMsg());
                        }
                    }
                   
                    else
                    {
                        MessageBox.Show("" + GetErrorMsg());
                    }
                } //if no report selected first if closed
                else
                {
                    MessageBox.Show("Please select report ");
                }

            }
            catch (Exception excep)
            {
                MessageBox.Show("" + excep.Message, "Free AD Tool error IInformation", MessageBoxButtons.OK);
            }

        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            // MessageBox.Show("comboBox1_SelectedValueChanged");
            // MessageBox.Show("Value :" + comboBox1.SelectedText);
            if (comboBox1.Text.Equals("File Size Report"))
            {
                textBox2.Visible = true;
                label3.Visible = true;
                comboBox2.Visible = true;
                comboBox3.Visible = true;
                comboBox4.Visible = true;
                panel2.Height = 67;
               // MessageBox.Show(""+panel2.Height);                
            }
            else
            {
                panel2.Height = 42;
                textBox2.Visible = false;
                label3.Visible = false;
                comboBox2.Visible = false;
                comboBox3.Visible = false;
                comboBox4.Visible = false;
            }            
            label4.Text =GetErrorMsg();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

       public void WebpartInventory(String site)
        {
            lvw = new ListView(); lvw.View = View.Details; lvw.Bounds = new Rectangle(0, 0, 500, 435);
            lvw.Columns.Add("WebPart Name", -2, System.Windows.Forms.HorizontalAlignment.Left); lvw.Columns.Add("WebPart Title", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lvw.GridLines = true;
            lvw.Scrollable = true;
            ListViewItem lvitem = null;
            lvw.Items.Clear();
            panel3.Controls.Clear();
            List<String> name = null;
            List<String> title = null;
            PowerShell powershell = PowerShell.Create();
            powershell.Runspace = remoteRunSpace;
            powershell.AddScript("Add-PSSnapin Microsoft.SharePoint.PowerShell");
            powershell.AddScript("$site=Get-SPWeb "+site);
            powershell.AddScript("$list=$site.GetCatalog(113)");
            powershell.AddScript("$spQuery = New-Object Microsoft.SharePoint.SPQuery");
            powershell.AddScript("$query= \'<Where-Object><Eq><FieldRef Name=\"Title\" /><Value Type=\"ContentQuery.webpart\">True</Value></Eq></Where>\'");
            powershell.AddScript("$spQuery.Query=$query");
            powershell.AddScript("$listItem=$list.GetItems($spQuery)");
            powershell.AddScript("$listItem | ForEach-Object{ echo $_.Name}");
            Collection<PSObject> results = powershell.Invoke();
            if (results != null)
            {
                powershell.AddScript("$listItem | ForEach-Object{ echo $_.Title}");
                Collection<PSObject> titles = powershell.Invoke();
                PSObject[] arrWeb=new PSObject[titles.Count];
                titles.CopyTo(arrWeb, 0);
                int count = titles.Count;
                int i=0;
                foreach (PSObject webpart in results)
                {
                    lvitem = new ListViewItem("" + webpart.ToString());
                    if(i<count)
                    lvitem.SubItems.Add("" + arrWeb.GetValue(i).ToString());
                    lvw.Items.Add(lvitem);
                    i++;
                }
                       
              
            }

            else
            {
                MessageBox.Show("WebPart is not available");
            }
            
            panel3.Controls.Add(lvw);
            if (lvw.Items.Count > 0)
            {
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(22);
            }
            else
                MessageBox.Show("WebPart is not available", "AD Tools Information", MessageBoxButtons.OK);
           
        }

        public void FileSizeReport(String site)
        {
            lvf = new ListView(); lvf.View = View.Details; lvf.Bounds = new Rectangle(0, 0, 500, 435);
            lvf.Columns.Add("File Size", -2, System.Windows.Forms.HorizontalAlignment.Left); lvf.Columns.Add("File URL", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lvf.GridLines = true;
            lvf.Scrollable = true;
            ListViewItem lvitem = null;
            lvf.Items.Clear();
            panel3.Controls.Clear();
            String gorL = null;
            if (comboBox3.Text.Equals("Greater than"))
            {
                gorL = " -gt ";
            }
            else if (comboBox3.Text.Equals("Lesser than"))
            {
                gorL = " -lt ";
            }
            List<String> name = null;
            List<String> title = null;
            PowerShell powershell = PowerShell.Create();
            powershell.Runspace = remoteRunSpace;
            powershell.AddScript("Add-PSSnapin Microsoft.SharePoint.PowerShell");
            powershell.AddScript("$web = Get-SPWeb "+site);
            powershell.AddScript("$Libraries = $web.Lists | where {$_.BaseType -eq \"DocumentLibrary\"}");
            /* powershell.AddScript("foreach ($library in $Libraries) {");
             powershell.AddScript("$Files = $library.Items | where {$_.FileSystemObjectType -eq \"File\"}");
             powershell.AddScript("foreach ($file in $Files) {");
             powershell.AddScript("Echo \"$($file.File.Length/1MB)\" | where { $file.File.Length/1KB -gt 1}}}");*/
            powershell.AddScript("foreach ($library in $Libraries) {$Files = $library.Items | where {$_.FileSystemObjectType -eq \"File\"};foreach ($file in $Files) {Echo $($file.File.Length/1"+comboBox2.Text+") | where { $file.File.Length/1"+comboBox2.Text+gorL+textBox2.Text+"}};}");
            Collection<PSObject> results = powershell.Invoke();
            if (results != null)
            {
                powershell.AddScript("foreach ($library in $Libraries) {$Files = $library.Items | where {$_.FileSystemObjectType -eq \"File\"};foreach ($file in $Files) {Echo $($file.Url) | where { $file.File.Length/1"+comboBox2.Text+gorL+textBox2.Text+"}};}");
                Collection<PSObject> titles = powershell.Invoke();
                PSObject[] arrWeb = new PSObject[titles.Count];
                titles.CopyTo(arrWeb, 0);
                int count = titles.Count;
                int i = 0;
                foreach (PSObject webpart in results)
                {
                    lvitem = new ListViewItem("" + webpart.ToString());
                    if (i < count)
                        lvitem.SubItems.Add("" + arrWeb.GetValue(i).ToString());
                    lvf.Items.Add(lvitem);
                    i++;
                }


            }

            else
            {
                MessageBox.Show("Files not available");
            }

            panel3.Controls.Add(lvf);
            if (lvf.Items.Count > 0)
            {
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(21);
            }
            else
                MessageBox.Show("Files not available", "AD Tools Information", MessageBoxButtons.OK);
        }


        

        public void listWebPartPage(String site)
        {
            lvp = new ListView(); lvp.View = View.Details; lvp.Bounds = new Rectangle(0, 0, 500, 435);
            lvp.Columns.Add("Page Url", -2, System.Windows.Forms.HorizontalAlignment.Left); lvp.Columns.Add("WebPart Title", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lvp.GridLines = true;
            lvp.Scrollable = true;
            ListViewItem lvitem = null;
            lvp.Items.Clear();
            panel3.Controls.Clear();
            List<String> name = null;
            List<String> title = null;
            PowerShell powershell = PowerShell.Create();
            powershell.Runspace = remoteRunSpace;
            powershell.AddScript("Add-PSSnapin Microsoft.SharePoint.PowerShell");
            powershell.AddScript("$web = Get-SPWeb "+site);
            powershell.AddScript("$Libraries = $web.Lists | where {$_.BaseType -eq \"DocumentLibrary\"}");
            /* powershell.AddScript("foreach ($library in $Libraries) {");
             powershell.AddScript("$Files = $library.Items | where {$_.FileSystemObjectType -eq \"File\"}");
             powershell.AddScript("foreach ($file in $Files) {");
             powershell.AddScript("Echo \"$($file.File.Length/1MB)\" | where { $file.File.Length/1KB -gt 1}}}");*/
            powershell.AddScript("foreach ($library in $Libraries) {$Files = $library.Items | where {$_.FileSystemObjectType -eq \"File\"};foreach ($file in $Files) {Echo \"$($file.Url)\" | where { $file.Url -Like \"*.aspx\"}};}");
            Collection<PSObject> results = powershell.Invoke();
            if (results != null)
            {
                foreach (PSObject obj in results)
                {
                    if (obj.ToString().Length > 0)
                    {

                        //stringBuilder.AppendLine(obj.ToString());
                        powershell.AddScript("$web=Get-SpWeb "+site);
                        powershell.AddScript("[Microsoft.SharePoint.Publishing.PublishingWeb]$pubWeb = [Microsoft.SharePoint.Publishing.PublishingWeb]::GetPublishingWeb($web);");
                        powershell.AddScript("$allowunsafeupdates = $web.AllowUnsafeUpdates");
                        powershell.AddScript("$web.AllowUnsafeUpdates = $true");
                        powershell.AddScript("$spQuery.Query=$query");
                        powershell.AddScript("$page=$web.GetFile(\"" + obj.ToString() + "\")");
                        powershell.AddScript("$page.CheckOut()");
                        powershell.AddScript("$webpartmanager = $web.GetLimitedWebPartManager($page.URL, [System.Web.UI.WebControls.WebParts.PersonalizationScope]::Shared)");
                        powershell.AddScript("$WebPartCollection = $webpartmanager.WebParts");
                        powershell.AddScript("$WebPartCollection | ForEach-Object{ echo $_.EffectiveTitle}");
                        Collection<PSObject> results1 = powershell.Invoke();
                        if (results1.Count > 0)
                        {
                            lvitem = new ListViewItem("" + obj.ToString());
                            
                            foreach (PSObject ob in results1)
                            {
                                lvitem.SubItems.Add(ob.ToString());
                                lvp.Items.Add(lvitem);
                                lvitem = new ListViewItem("");
                                                            //Console.WriteLine("\t\t\t" + ob.ToString())
                            }

                            
                        }

                    }
                }

            }
               
            else
            {
                MessageBox.Show("WebPart is not available");
            }
            /*powershell.AddScript("$web = Get-SPWeb "+site);
            powershell.AddScript("$file=$web.GetFile(\""+site+"/defalt.aspx"+"\")");
            powershell.AddScript("echo $file.Url");
            Collection<PSObject> resultsN = powershell.Invoke();
            if (results != null)
            {
                foreach (PSObject obj in resultsN)
                {
                    if (obj.ToString().Length > 0)
                    {
            */
                        //stringBuilder.AppendLine(obj.ToString());
                        powershell.AddScript("$web=Get-SpWeb " + site);
                        powershell.AddScript("[Microsoft.SharePoint.Publishing.PublishingWeb]$pubWeb = [Microsoft.SharePoint.Publishing.PublishingWeb]::GetPublishingWeb($web);");
                        powershell.AddScript("$allowunsafeupdates = $web.AllowUnsafeUpdates");
                        powershell.AddScript("$web.AllowUnsafeUpdates = $true");
                        powershell.AddScript("$spQuery.Query=$query");
                        powershell.AddScript("$page=$web.GetFile(\"" +site+ "/default.aspx" + "\")");
                        powershell.AddScript("$page.CheckOut()");
                        powershell.AddScript("$webpartmanager = $web.GetLimitedWebPartManager($page.URL, [System.Web.UI.WebControls.WebParts.PersonalizationScope]::Shared)");
                        powershell.AddScript("$WebPartCollection = $webpartmanager.WebParts");
                        powershell.AddScript("$WebPartCollection | ForEach-Object{ echo $_.EffectiveTitle}");
                        Collection<PSObject> results11 = powershell.Invoke();
                        if (results11.Count > 0)
                        {
                            lvitem = new ListViewItem("/default.aspx");
                            foreach (PSObject ob in results11)
                            {
                                lvitem.SubItems.Add(ob.ToString());
                                lvp.Items.Add(lvitem);
                                lvitem = new ListViewItem("");
                              
                            }
                            
                        }

                  /*  }
                }

            }

            else
            {
                MessageBox.Show("WebPart is not available");
            }
*/
            panel3.Controls.Add(lvp);
            if (lvp.Items.Count > 0)
            {
                ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                cl.executeDll(23);
            }
            else
                MessageBox.Show("WebPart is not available","AD Tools Information", MessageBoxButtons.OK);           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            
            
            initRunSpace();
            addSnappin();
            farmnode.Nodes.Clear();
            listView1.Items.Clear();
            listView1.Visible = false;
            serverNode = farmnode.Nodes.Add(server.Text);
            //if (SPFarm.Local== null)
            //{
            //    MessageBox.Show("No Server Form is Found in this machine", "Free AD Tools Error Information"+, MessageBoxButtons.OK);
            //}
            //else
            //{
            try
            {
                Collection<PSObject> webApps = executeCommand("Get-SpWebApplication | ForEach-Object{ echo $_.Url}");
                Collection<PSObject> webAppName = executeCommand("Get-SpWebApplication|ForEach-Object{ echo $_.Name}");
                int i = 0;
                foreach (PSObject webApp in webApps)
                {
                    TreeNode webAppNode = serverNode.Nodes.Add(webAppName[i++].ToString());
                    Collection<PSObject> siteCollections = executeCommand("Get-SPWebApplication " + webApp.ToString() + " | Get-SPSite | ForEach-Object{ echo $_.Url}");
                    foreach (PSObject siteCollection in siteCollections)
                    {
                        TreeNode scNode = webAppNode.Nodes.Add(siteCollection.ToString());//.Substring(11));
                        String scNodeTemp = siteCollection.ToString();//.Substring(11);
                        scNodeTemp += "/*";
                        // MessageBox.Show(scNodeTemp);
                        Collection<PSObject> sites = executeCommand("Get-SPWeb -Identity " + scNodeTemp + " | ForEach-Object{ echo $_.Url}");
                        if (sites.Count < 1)
                            sites.Add(siteCollection);
                        foreach (PSObject site in sites)
                        {
                            TreeNode siteNode = scNode.Nodes.Add(site.ToString());
                            Collection<PSObject> lists = executeCommand("Get-SPWeb " + site.ToString() + " |  Select -ExpandProperty Lists | ForEach-Object{ echo $_.Title}");
                            foreach (PSObject list in lists)
                            {
                                TreeNode listNode = siteNode.Nodes.Add(list.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("No server Farm is found this machine", "AD Tools Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            // } //else closing
            tableLayoutPanel1.Visible = true;
        }

        public void SitePerWebpart(String site)
        {
            lvs = new ListView(); lvs.View = View.Details; lvs.Bounds = new Rectangle(0, 0, 500, 435);
            lvs.Columns.Add("WebPart Name", -2, System.Windows.Forms.HorizontalAlignment.Left); lvs.Columns.Add("Page URl", -2, System.Windows.Forms.HorizontalAlignment.Left);
            lvs.GridLines = true;
            lvs.Scrollable = true;
            ListViewItem lvitem = null;
            lvs.Items.Clear();
            panel3.Controls.Clear();
            List<String> name = null;
            List<String> title = null;
            PowerShell powershell = PowerShell.Create();
            powershell.Runspace = remoteRunSpace;
            powershell.AddScript("Add-PSSnapin Microsoft.SharePoint.PowerShell");
            powershell.AddScript("$site=Get-SPWeb "+site);
            powershell.AddScript("$list=$site.GetCatalog(113)");
            powershell.AddScript("$spQuery = New-Object Microsoft.SharePoint.SPQuery");
            powershell.AddScript("$query= \'<Where-Object><Eq><FieldRef Name=\"ContentType\" /><Value Type=\"Document\">True</Value></Eq></Where>\'");
            powershell.AddScript("$spQuery.Query=$query");
            powershell.AddScript("$listItem=$list.GetItems($spQuery)");
             powershell.AddScript("foreach($list in $listitem){$web = Get-SPWeb \""+site+"\";$Libraries = $web.Lists | where {$_.BaseType -eq \"DocumentLibrary\"};foreach ($library in $Libraries) {$Files = $library.Items | where {$_.FileSystemObjectType -eq \"File\"};foreach ($file in $Files) {if( $file.Name -Like \"*.aspx\"){$wm = $site.GetLimitedWebPartManager($file.URL, [System.Web.UI.WebControls.WebParts.PersonalizationScope]::Shared);$wc = $wm.WebParts;foreach($webpart in $wc){if($webpart.Title.Equals($list.Title)){echo $list.Title;echo $file.Url}}}};}}");
            Collection<PSObject> results = powershell.Invoke();
            if (results != null)
            {
                
                PSObject[] obj = new PSObject[results.Count];
                results.CopyTo(obj, 0);
                for (int i = 0; i < results.Count; i += 2)
                {
                    //MessageBox.Show("Entered");
                    lvitem = new ListViewItem(obj[i].ToString());
                    lvitem.SubItems.Add(obj[i + 1].ToString());
                    lvs.Items.Add(lvitem);

                }

            }

            else
            {
                MessageBox.Show("WebPart is not available");
            }

            panel3.Controls.Add(lvs);
            if (lvs.Items.Count > 0)
            {
                //  ClassLibrary1.Class1 cl = new ClassLibrary1.Class1();
                //  cl.executeDll(24);
            }
            else
                MessageBox.Show("WebPart is not available", "AD Tools Information", MessageBoxButtons.OK);

        }
    } //form1 closed
} //namespace closed