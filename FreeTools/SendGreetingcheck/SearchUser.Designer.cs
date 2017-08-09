namespace SendGreetingcheck
{
    partial class LastLogonTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LastLogonTool));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GetLastLogonDetails = new System.Windows.Forms.Button();
            this.DeSelectAll = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.Button();
            this.UserSearchlist = new System.Windows.Forms.ListView();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DomainNamedcomboBox = new System.Windows.Forms.ComboBox();
            this.Search = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label1.Location = new System.Drawing.Point(25, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(546, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "NOTE:  Provide this tool a user’s domain name and username, it will give his/her " +
                "last logon details.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label2.Location = new System.Drawing.Point(38, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(461, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "If the desired domain name is not available in the text box given below, you can " +
                "even type it in.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label3.Location = new System.Drawing.Point(38, 123);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(596, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "User name can be any of these: a user’s First Name, Last Name,Display Name, sAMAc" +
                "count Name, User Principle Name    ";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GetLastLogonDetails);
            this.groupBox1.Controls.Add(this.DeSelectAll);
            this.groupBox1.Controls.Add(this.SelectAll);
            this.groupBox1.Controls.Add(this.UserSearchlist);
            this.groupBox1.Controls.Add(this.txtUserName);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.DomainNamedcomboBox);
            this.groupBox1.Controls.Add(this.Search);
            this.groupBox1.Location = new System.Drawing.Point(23, 165);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 332);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // GetLastLogonDetails
            // 
            this.GetLastLogonDetails.Image = global::SendGreetingcheck.Properties.Resources.get_last_logon;
            this.GetLastLogonDetails.Location = new System.Drawing.Point(492, 78);
            this.GetLastLogonDetails.Name = "GetLastLogonDetails";
            this.GetLastLogonDetails.Size = new System.Drawing.Size(135, 23);
            this.GetLastLogonDetails.TabIndex = 9;
            this.GetLastLogonDetails.UseVisualStyleBackColor = true;
            this.GetLastLogonDetails.Click += new System.EventHandler(this.GetLastLogonDetails_Click);
            // 
            // DeSelectAll
            // 
            this.DeSelectAll.Image = global::SendGreetingcheck.Properties.Resources.deselect_all;
            this.DeSelectAll.Location = new System.Drawing.Point(78, 85);
            this.DeSelectAll.Name = "DeSelectAll";
            this.DeSelectAll.Size = new System.Drawing.Size(69, 20);
            this.DeSelectAll.TabIndex = 8;
            this.DeSelectAll.UseVisualStyleBackColor = true;
            this.DeSelectAll.Click += new System.EventHandler(this.DeSelectAll_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.Image = global::SendGreetingcheck.Properties.Resources.select_all;
            this.SelectAll.Location = new System.Drawing.Point(19, 86);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(55, 19);
            this.SelectAll.TabIndex = 7;
            this.SelectAll.UseVisualStyleBackColor = true;
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // UserSearchlist
            // 
            this.UserSearchlist.Location = new System.Drawing.Point(18, 107);
            this.UserSearchlist.Name = "UserSearchlist";
            this.UserSearchlist.Size = new System.Drawing.Size(613, 207);
            this.UserSearchlist.TabIndex = 6;
            this.UserSearchlist.UseCompatibleStateImageBehavior = false;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(177, 45);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(134, 20);
            this.txtUserName.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label5.Location = new System.Drawing.Point(176, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "User Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label4.Location = new System.Drawing.Point(19, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Domain Name";
            // 
            // DomainNamedcomboBox
            // 
            this.DomainNamedcomboBox.FormattingEnabled = true;
            this.DomainNamedcomboBox.Location = new System.Drawing.Point(21, 45);
            this.DomainNamedcomboBox.Name = "DomainNamedcomboBox";
            this.DomainNamedcomboBox.Size = new System.Drawing.Size(142, 21);
            this.DomainNamedcomboBox.TabIndex = 2;
            // 
            // Search
            // 
            this.Search.Image = global::SendGreetingcheck.Properties.Resources.search;
            this.Search.Location = new System.Drawing.Point(317, 44);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(49, 20);
            this.Search.TabIndex = 1;
            this.Search.UseVisualStyleBackColor = true;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(49, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(149, 19);
            this.label6.TabIndex = 9;
            this.label6.Text = "Last Logon Finder";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SendGreetingcheck.Properties.Resources.icon;
            this.pictureBox1.Location = new System.Drawing.Point(24, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 21);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.ErrorImage = null;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(25, 124);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(12, 12);
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(25, 101);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(12, 12);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.ErrorImage = null;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(25, 148);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(12, 12);
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label7.Location = new System.Drawing.Point(38, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(557, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "You can run this Cmdlet in Powershell using the command: \'add-PSSnapIn FindString" +
                "PSSnapIn\' and Get-LastLogon";
            // 
            // LastLogonTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.ClientSize = new System.Drawing.Size(695, 509);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LastLogonTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LastLogonTool";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox DomainNamedcomboBox;
        private System.Windows.Forms.ListView UserSearchlist;
        private System.Windows.Forms.Button GetLastLogonDetails;
        private System.Windows.Forms.Button DeSelectAll;
        private System.Windows.Forms.Button SelectAll;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label7;
    }
}