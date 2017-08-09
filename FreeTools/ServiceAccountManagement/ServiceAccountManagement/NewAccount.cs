using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Tools;

namespace ServiceAccountManagement
{
    public partial class NewAccount : Form
    {
        public static bool isadded = false;
        public NewAccount()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
              try
            {
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    if (Form1.defaultUser)
                    {
                        DirectoryEntry dirEntry = new DirectoryEntry(textBox2.Text);
                        DirectoryEntry newMSA = dirEntry.Children.Add("CN=" + textBox4.Text, "msDS-ManagedServiceAccount");
                        newMSA.InvokeSet("sAMAccountName", textBox1.Text);
                        if (!string.IsNullOrWhiteSpace(textBox3.Text))
                        newMSA.InvokeSet("description", textBox3.Text);
                        newMSA.Properties["userAccountControl"].Value = 0x1000;
                        newMSA.CommitChanges();
                        try
                        {
                            PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Form1.SelectedDomain);
                            ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, textBox4.Text);
                            computer.SetPassword(textBox4.Text + '$');
                            if (computer != null)
                            {
                                if (radioButton1.Checked == true)
                                    computer.Enabled = true;
                                else
                                    computer.Enabled = false;
                                computer.Save();
                                MessageBox.Show("Managed service account \"" + textBox4.Text + "\" created successfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                isadded = true;
                                this.Close();
                            }
                        }
                        catch (Exception et)
                        {
                            MessageBox.Show(et.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        try
                        {
                           
                            using (new Impersonator(Form1.UserName, Form1.DomainName, Form1.Password))
                            {
                                DirectoryEntry dirEntry = new DirectoryEntry(textBox2.Text);
                                DirectoryEntry newMSA = dirEntry.Children.Add("CN=" + textBox4.Text, "msDS-ManagedServiceAccount");
                                newMSA.InvokeSet("sAMAccountName", textBox1.Text);
                                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                                newMSA.InvokeSet("description", textBox3.Text);
                                newMSA.Properties["userAccountControl"].Value = 0x1000;
                                newMSA.CommitChanges();
                                try
                                {
                                    PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
                                    ComputerPrincipal computer = ComputerPrincipal.FindByIdentity(ctx, textBox4.Text);
                                    computer.SetPassword(textBox4.Text + '$');
                                    if (computer != null)
                                    {
                                        if (radioButton1.Checked == true)
                                            computer.Enabled = true;
                                        else
                                            computer.Enabled = false;
                                        computer.Save();
                                        MessageBox.Show("Managed service account \"" + textBox4.Text + "\" created successfully", " Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                        isadded = true;
                                        this.Close();
                                    }
                                }
                                catch (Exception et)
                                {
                                    MessageBox.Show(et.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                        }
                        catch (Exception ew)
                        {
                            MessageBox.Show(ew.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        
                    }
                }
                else
                    MessageBox.Show("Please Enter a Valid Name", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
              }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.Text = textBox4.Text;
            textBox1.Text = textBox4.Text + "$";
        }

        private void NewAccount_load(object sender, EventArgs e)
        {
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            textBox2.Text = "LDAP://CN=Managed Service Accounts," + Form1.resultdc;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton2.Checked = false;
            radioButton1.Checked = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
