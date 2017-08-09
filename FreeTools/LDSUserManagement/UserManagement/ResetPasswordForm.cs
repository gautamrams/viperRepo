using System;
using System.DirectoryServices;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;

namespace ADLDSManagement
{
    public partial class ResetPasswordForm : Form
    {
        public String user;
        public String userName;
        public String password;
        public bool defaultUser = false;
        public String path;
        public bool is_excep = false;
        public bool is_change = false;

        public ResetPasswordForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            is_change = false;
        }

        private void ResetPasswordForm_Load(object sender, EventArgs e)
        {
            this.Left = 350;
            this.Top = 150;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("") || textBox2.Text.Equals(""))
                MessageBox.Show("Please enter the details correctly", " Missing Details", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (!textBox1.Text.Equals(textBox2.Text))
                MessageBox.Show(" \"Confirm Password\" is not equal to \"New Password\"", " Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                password = textBox2.Text;
                is_change = true;
                this.Close();
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
    }
}
