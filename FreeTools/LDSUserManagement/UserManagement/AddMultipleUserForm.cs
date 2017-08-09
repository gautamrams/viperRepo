using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ADLDSManagement
{
    public partial class AddMultipleUserForm : Form
    {
        public bool isImport = false;
        public String filePath="";

        public AddMultipleUserForm()
        {
            InitializeComponent();
        }

        private void AddMultipleUserForm_Load(object sender, EventArgs e)
        {
            this.Left = 350;
            this.Top = 150;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            filePath = textBox1.Text;
            if (filePath.Equals(""))
                MessageBox.Show("Please Select the Correct file", " Missing Details ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                isImport = true;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Open a CSV File ";
            openFileDialog1.Filter = "CSV Files|*.csv";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog1.FileName;
                textBox1.Text = filePath;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
