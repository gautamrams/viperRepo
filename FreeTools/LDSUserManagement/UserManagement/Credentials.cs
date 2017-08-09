using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace LocalUserManagement
{
    public partial class SaveForm : Form
    {
        public string username = "User Name";
        public string password = "Password";
        public string path = "LDAP://";
        public string port = ".";
        public string partition = ".";
        public string dc = ".";

        public bool defaultuser = true;
        AddMultipleComputers addmultiplecomputers = new AddMultipleComputers();
        public SaveForm()
        {
            InitializeComponent();
        }
        private void SaveForm_Load(object sender, EventArgs e)
        {
            this.Left = 350;
            this.Top = 150;
            this.BackColor = Color.White;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            String  DomainName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            //Domain domain;
            defaultuser = true;
            if ((textBox2.Text.Equals("User Name") && !textBox3.Text.Equals("Password")) || (!textBox2.Text.Equals("User Name") && textBox3.Text.Equals("Password")))
            {
                MessageBox.Show("Please Enter User Name & Password correctly", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                return;
            }
            else if (!textBox2.Text.Equals("User Name") && !textBox3.Text.Equals("Password"))
            {
                username = textBox2.Text;
                password = textBox3.Text;
                port = textBox4.Text;
                partition = textBox5.Text;
                dc = textBox1.Text;
                path = "LDAP://"+dc+":"+port+"/"+partition;
                MessageBox.Show(path);
                defaultuser = false;
                addmultiplecomputers.defaultUser = defaultuser;
                addmultiplecomputers.UserName = username;
                addmultiplecomputers.Password = password;
            }
            try
            {
                if (!defaultuser)
                {
                    DirectoryEntry de = new DirectoryEntry(path, username, password, AuthenticationTypes.Secure);
                    if (de != null)
                    {
                        MessageBox.Show("CONNECTED");
                    }
                }
                //domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName, username, password));
                else
                {
                    DirectoryEntry de = new DirectoryEntry(path);
                }
                    //domain = Domain.GetDomain(new DirectoryContext(DirectoryContextType.Domain, DomainName));
                
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryObjectNotFoundException ex)
            {
                MessageBox.Show(" The network path was not found ","Failure",MessageBoxButtons.OK, MessageBoxIcon.Error);
                defaultuser = true;
                return;
            }
            catch (System.Security.Authentication.AuthenticationException ex)
            {
                MessageBox.Show("Logon Failure. Please Enter a valid Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                defaultuser = true;
                return;
            }
            catch (System.DirectoryServices.ActiveDirectory.ActiveDirectoryOperationException ex)
            {
                MessageBox.Show("Unspecified Error", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                defaultuser = true;
                return;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Logon failure: "))
                {
                    MessageBox.Show("Logon Failure. Please Enter Correct Username & Password", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    defaultuser = true;
                    return;
                }
                else
                {
                    MessageBox.Show("Unspecified Error", "Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    defaultuser = true;
                    return;
                }
            }
            this.Close();

        }   
        private void textBox2_Enter(object sender, EventArgs e)
        {

            if (textBox2.ForeColor == Color.Gray)
            {
                textBox2.Text = "";
                textBox2.ForeColor = Color.Black;
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.ForeColor == Color.Gray)
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Black;
                textBox3.PasswordChar = '*';
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text.Equals(""))
            {
                textBox2.ForeColor = Color.Gray;
                textBox2.Text = "User Name";
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {

            if (textBox3.Text.Equals(""))
            {
                textBox3.ForeColor = Color.Gray;
                textBox3.Text = "Password";
                textBox3.PasswordChar = '\0';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            username = "User Name";
            password = "Password";
            defaultuser = true;
            this.Close();
        }

    }
}
