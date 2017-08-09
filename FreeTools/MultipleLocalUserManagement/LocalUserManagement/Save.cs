using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LocalUserManagement
{
    public partial class SaveForm : Form
    {
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
            if (panel1.Visible == true)
                panel1.Visible = false;
            else
                panel1.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddMultipleUserForm addMultipleUserForm = new AddMultipleUserForm();
            addMultipleUserForm.ShowDialog();
        }

    }
}
