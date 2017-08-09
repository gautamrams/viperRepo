using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WeakPasswordUsers
{
    public partial class Attributes : Form
    {
        public String[] attributes_listbox1 = new String[0];
        public String[] attributes_listbox2 = new String[0];
        public bool shouldReplace;
        public Attributes()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            object[] temp = new object[0];
            foreach(object items in listBox1.SelectedItems)
            {
                listBox2.Items.Add(items);
                Array.Resize<object>(ref temp, temp.Length + 1);
                temp[temp.Length - 1] = items;
            }
            for(int i=0;i<temp.Length;i++)
            {
                listBox1.Items.Remove(temp[i]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            object[] temp = new object[0];
            foreach (object items in listBox2.SelectedItems)
            {
                listBox1.Items.Add(items);
                Array.Resize<object>(ref temp, temp.Length + 1);
                temp[temp.Length - 1] = items;
            }
            for (int i = 0; i < temp.Length; i++)
            {
                listBox2.Items.Remove(temp[i]);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            for (int i = 0; i < attributes_listbox1.Length;i++ )
            {
                listBox1.Items.Add((object)(attributes_listbox1[i]));
            }
            for (int i = 0; i < attributes_listbox2.Length; i++)
            {
                listBox2.Items.Add((object)(attributes_listbox2[i]));
            }
                this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count == 0)
            {
                MessageBox.Show("Select atleast one column to be displayed in the report", "Attributes", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                attributes_listbox1 = new String[0];
                attributes_listbox2 = new String[0];
                foreach (object items in listBox1.Items)
                {
                    Array.Resize<String>(ref attributes_listbox1, attributes_listbox1.Length + 1);
                    attributes_listbox1[attributes_listbox1.Length - 1] = items.ToString();
                }
                foreach (object items in listBox2.Items)
                {
                    Array.Resize<String>(ref attributes_listbox2, attributes_listbox2.Length + 1);
                    attributes_listbox2[attributes_listbox2.Length - 1] = items.ToString();
                }
                this.Hide();
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            object[] temp=new object[0];
            foreach (object items in listBox2.SelectedItems)
            {
                Array.Resize<object>(ref temp, temp.Length + 1);
                temp[temp.Length - 1] = items;
            }
            for (int i = 0; i < temp.Length; i++)
            {
                object items = temp[i];
                if (listBox2.Items.IndexOf(items) > 0)
                {

                    listBox2.Items.Insert(listBox2.Items.IndexOf(items) + 1, listBox2.Items[listBox2.Items.IndexOf(items) - 1]);
                    listBox2.Items.RemoveAt(listBox2.Items.IndexOf(items) - 1);
                }
            }
            if (temp.Length == 0)
            {
                MessageBox.Show("Select atleast one column from selected columns", "Attributes", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            object[] temp = new object[0];
            foreach (object items in listBox2.SelectedItems)
            {
                Array.Resize<object>(ref temp, temp.Length + 1);
                temp[temp.Length - 1] = items;
            }
            for (int i = 0; i < temp.Length; i++)
            {
                object items = temp[i];
                if (listBox2.Items.IndexOf(items) < listBox2.Items.Count-1)
                {

                    listBox2.Items.Insert(listBox2.Items.IndexOf(items) , listBox2.Items[listBox2.Items.IndexOf(items) + 1]);
                    listBox2.Items.RemoveAt(listBox2.Items.IndexOf(items) +1);
                }
            }
            if(temp.Length==0)
            {
                MessageBox.Show("Select atleast one column from selected columns", "Attributes", MessageBoxButtons.OK, MessageBoxIcon.Information);
         
            }
        }
    }
}
