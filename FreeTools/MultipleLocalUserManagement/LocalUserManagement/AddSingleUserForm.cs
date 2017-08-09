using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LocalUserManagement
{
    public partial class AddSingleUserForm : Form
    {
        public bool rightUser = false;
        public String user, pass, fullName, desc;

        public AddSingleUserForm()
        {
            InitializeComponent();
        }

        private void AddSingleUserForm_Load(object sender, EventArgs e)
        {
            this.Left = 350;
            this.Top = 150;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
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
    }
}
