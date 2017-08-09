using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices.ActiveDirectory;

namespace ServiceAccountManagement
{
    public partial class Credentials : Form
    {
        public string username = "User Name";
        public string password = "Password";
        public bool defaultuser = true;
        public Credentials()
        {
            InitializeComponent();
        }

        private void Credentials_Load(object sender, EventArgs e)
        {
            this.Left = 350;
            this.Top = 150;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            this.BackColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            defaultuser = true;
            if ((textBox1.Text.Equals("User Name") && !textBox2.Text.Equals("Password")) || (!textBox1.Text.Equals("User Name") && textBox2.Text.Equals("Password")))
            {
                MessageBox.Show("Please Enter User Name & Password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            else if (!textBox2.Text.Equals("User Name") && !textBox2.Text.Equals("Password"))
            {
                username = textBox1.Text;
                password = textBox2.Text;
                defaultuser = false;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            username = "User Name";
            password = "Password";
            defaultuser = true;
            this.Close();
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

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                textBox1.ForeColor = Color.Gray;
                textBox1.Text = "User Name";
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
    }
}
