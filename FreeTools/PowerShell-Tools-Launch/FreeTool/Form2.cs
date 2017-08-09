using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace FreeTool
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/ad-manager/index.html");
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/active-directory-audit/index.html");
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Process.Start("IExplore.exe", "http://www.manageengine.com/products/self-service-password/index.html");
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}