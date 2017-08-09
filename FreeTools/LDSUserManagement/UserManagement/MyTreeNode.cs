using System;
using System.Collections.Generic;
using System.Text;


namespace ADLDSManagement
{
    class MyTreeNode : System.Windows.Forms.TreeNode
    {
        public String dn;
        public String name;

        public MyTreeNode(String aa, String bb)
        {
            this.name = aa;
            this.dn = bb;
            this.Text = aa;
        }
    }
}
