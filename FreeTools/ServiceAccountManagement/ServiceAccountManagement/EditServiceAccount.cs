using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceAccountManagement
{
    public partial class EditServiceAccount : Form
    {
        public String name;
        public String description;
        public String samaccountname;
        public bool isenabled;
        public bool isedited;
        public String distinguishedname;
        public String guid;
        public String hostcomputers;
        
        public EditServiceAccount()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EditServiceAccount_Load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            textBox1.Text = name;
            textBox2.Text = samaccountname;
            textBox3.Text = description;
                if(isenabled)
                radioButton1.Checked = true;
                else
                radioButton2.Checked = true;
            textBox4.Text = distinguishedname;
            textBox5.Text = guid;
            textBox6.Text = hostcomputers; 
            isedited = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isedited = true;
            name = textBox1.Text;
            samaccountname = textBox2.Text;
            description = textBox3.Text;
            if (radioButton1.Checked)
                isenabled = true;
            else
                isenabled = false;
            this.Close();
        }
    }
}
