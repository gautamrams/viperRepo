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
    public partial class ContainerTree : Form
    {
        public String userName;
        public String password;
        public String path;
        public String port;
        public String partition;
        public String dc;
        public bool defaultUser = true;
        public ListView.CheckedListViewItemCollection useritem;
        public string defaultNamingContext;
        public List<string> nodesToExpand = new List<string>();
        public List<TreeNode> checkednode = new List<TreeNode>();
        public String dnofcn;
        public String userdn;
        public int flag = 0;
        public int moveuserflag = 0;
        public int propertiesflag = 0;
        public int createdflag = 0;
        public int ifblockflag = 0;

        public ContainerTree()
        {
            InitializeComponent();
            Shown+=new EventHandler(ContainerTree_Shown);
        }
        private void ContainerTree_Shown(object sender, EventArgs e)
        {
            ContainerTree.ActiveForm.Text = "Loading...";
            BuildTree();
            
        }
        private void ContainerTree_Load(object sender, EventArgs e)
        {
            flag = 0;
            treeView1.Nodes.Clear();
            nodesToExpand.AddRange(new string[] { "organizationalUnit", "container" });
            label1.Text = "Loading containers ...";
            label1.Refresh();
            //BuildTree();
            if (moveuserflag == 1 && propertiesflag == 0)
            {
                //MessageBox.Show("1");
                button4.Visible = true;
                button1.Visible = false;
                button3.Visible = false;
            }
            
            else if (propertiesflag == 1 && moveuserflag == 0)
            {
                //MessageBox.Show("3");
                button4.Visible = false;
                button1.Visible = true;
                button3.Visible = false;
            }
            else
            {
                //MessageBox.Show("4");
                button4.Visible = false;
                button1.Visible = true;
                button3.Visible = true;
            }
            label1.Text = "";
            checkednode.Clear();
        }
        private void BuildTree()
        {
            string adsPath = path;
            if (!defaultUser)
            {
                using (DirectoryEntry domain = new DirectoryEntry(adsPath, userName, password, AuthenticationTypes.Secure))
                {

                    MyTreeNode treeRoot = new MyTreeNode(partition, domain.Properties["distinguishedName"].Value.ToString());
                    this.treeView1.Nodes.Add(treeRoot);
                    AddNodes(domain, treeRoot);
                    treeRoot.Expand();
                    ContainerTree.ActiveForm.Text = "Containers";
                }
            }
            else
            {
                using (DirectoryEntry domain = new DirectoryEntry(adsPath))
                {

                    MyTreeNode treeRoot = new MyTreeNode(partition, domain.Properties["distinguishedName"].Value.ToString());
                    this.treeView1.Nodes.Add(treeRoot);
                    AddNodes(domain, treeRoot);
                    treeRoot.Expand();
                    ContainerTree.ActiveForm.Text = "Containers";
                }
            }
        }
        private void AddNodes(DirectoryEntry entry, MyTreeNode node)
        {
            foreach (DirectoryEntry child in entry.Children)
            {                
                if (nodesToExpand.Contains(child.SchemaClassName))
                {
                    MyTreeNode childNode = new MyTreeNode(child.Name, child.Properties["distinguishedName"].Value.ToString());
                    node.Nodes.Add(childNode);
                    this.AddNodes(child, childNode);
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            checkednode.Clear();
            dnofcn = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            findchecked(treeView1.Nodes);
            if (checkednode.Count != 1)
            {
               
                MessageBox.Show("Please Select a container");
                checkednode.Clear();
                flag = 1;
                
                
            }
            else
            {
                foreach (MyTreeNode node in checkednode)
                {
                    //MessageBox.Show(" else  "+checkednode.Count.ToString());
                    //MessageBox.Show(node.dn);
                    dnofcn = node.dn;
                    if (node.IsSelected)
                    {
                        
                        //MessageBox.Show("still selected");
                        //MessageBox.Show("flag : "+flag.ToString());
                    }
                    
                    
                        flag = 0;
                    

                }
                checkednode.Clear();
                
            }
            if(flag==0)
            {
                
                this.Close();
            }
            
        }
        void findchecked(TreeNodeCollection nodes)
        {
            foreach (MyTreeNode node in nodes)
            {
                if (node.IsSelected == true)
                {
                    checkednode.Add(node);
                    
                }
                else
                {
                    findchecked(node.Nodes);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            checkednode.Clear();
            CreateContainer cc = new CreateContainer();
            cc.userName = userName;
            cc.password = password;
            cc.port = port;
            cc.partition = partition;
            cc.path = path;
            cc.dc = dc;
            cc.defaultUser = defaultUser;
            flag=1;
            //button1_Click(sender,e);
            findchecked(treeView1.Nodes);
            if (checkednode.Count != 1)
            {
                
                MessageBox.Show("Please Select a container");
                checkednode.Clear();             
            }
            else
            {
                foreach (MyTreeNode node in checkednode)
                {
                    //MessageBox.Show(node.dn);
                    dnofcn = node.dn;
                                       
                }
                checkednode.Clear();

            }


            cc.dnofcn = dnofcn;
            cc.ShowDialog();
            createdflag = cc.createdflag;
            flag = cc.flag;
            if( createdflag == 1)
            {
                flag = 1;
                ContainerTree_Load(sender, e);
                ContainerTree_Shown(sender, e);
                createdflag = 0;
            }
            checkednode.Clear();
            treeView1.Focus();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            findchecked(treeView1.Nodes);
            if (checkednode.Count != 1)
            {
                //MessageBox.Show(checkednode.Count.ToString());
                MessageBox.Show("Please Select a container");
                checkednode.Clear();
            }
            else
            {
                foreach (MyTreeNode node in checkednode)
                {
                    //MessageBox.Show(node.dn);
                    dnofcn = node.dn;

                }
            }
            try
            {
                String containerpath = "LDAP://" + dc + ":" + port + "/" + dnofcn;
                DirectoryEntry theNewParent;
                if (!defaultUser)
                {
                    theNewParent = new DirectoryEntry(containerpath, userName, password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                }
                else
                {
                    theNewParent = new DirectoryEntry(containerpath);
                }
                foreach (ListViewItem useritem1 in useritem)
                {

                    String userpath = "LDAP://" + dc + ":" + port + "/" + useritem1.SubItems[3].Text;
                    //MessageBox.Show(userpath);
                    DirectoryEntry theObjectToMove;
                    if (!defaultUser)
                    {
                        theObjectToMove = new DirectoryEntry(userpath, userName, password, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                    }
                    else
                    {
                        theObjectToMove = new DirectoryEntry(userpath);
                    }
                    theObjectToMove.MoveTo(theNewParent);
                }
                MessageBox.Show("User(s) Moved successfully ");
                this.Close();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("(0x80005000)"))
                {
                    //
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
