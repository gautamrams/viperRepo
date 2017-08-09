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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UserSearchlist = new System.Windows.Forms.ListView();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DomainNamedcomboBox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Search = new System.Windows.Forms.Button();
            this.SelectAll = new System.Windows.Forms.Button();
            this.GetLastLogonDetails = new System.Windows.Forms.Button();
            this.DeSelectAll = new System.Windows.Forms.Button();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label1.Location = new System.Drawing.Point(37, 77);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
            this.label2.Location = new System.Drawing.Point(57, 110);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(460, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "If the desired domain name is not available in the text box given below, you can " +
                "even type it in.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label3.Location = new System.Drawing.Point(57, 144);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(596, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "User name can be any of these: a user’s First Name, Last Name,Display Name, sAMAc" +
                "count Name, User Principle Name    ";
            // 
            // UserSearchlist
            // 
            this.UserSearchlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.UserSearchlist.GridLines = true;
            this.UserSearchlist.Location = new System.Drawing.Point(39, 175);
            this.UserSearchlist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.UserSearchlist.Name = "UserSearchlist";
            this.UserSearchlist.OwnerDraw = true;
            this.UserSearchlist.Size = new System.Drawing.Size(918, 368);
            this.UserSearchlist.TabIndex = 6;
            this.UserSearchlist.UseCompatibleStateImageBehavior = false;
            this.UserSearchlist.View = System.Windows.Forms.View.Details;
            this.UserSearchlist.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.UserSearchlist_DrawColumnHeader);
            this.UserSearchlist.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.UserSearchlist_DrawItem);
            this.UserSearchlist.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.UserSearchlist_DrawSubItem);
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(282, 61);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(199, 26);
            this.txtUserName.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label5.Location = new System.Drawing.Point(286, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
            this.label4.Location = new System.Drawing.Point(43, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "Domain Name";
            // 
            // DomainNamedcomboBox
            // 
            this.DomainNamedcomboBox.FormattingEnabled = true;
            this.DomainNamedcomboBox.Location = new System.Drawing.Point(43, 59);
            this.DomainNamedcomboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DomainNamedcomboBox.Name = "DomainNamedcomboBox";
            this.DomainNamedcomboBox.Size = new System.Drawing.Size(211, 28);
            this.DomainNamedcomboBox.TabIndex = 2;
            this.DomainNamedcomboBox.SelectedIndexChanged += new System.EventHandler(this.DomainNamedcomboBox_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(28, 9);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(241, 32);
            this.label6.TabIndex = 9;
            this.label6.Text = "Last Logon Finder";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label7.Location = new System.Drawing.Point(57, 179);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(530, 14);
            this.label7.TabIndex = 12;
            this.label7.Text = "You can run this Cmdlet in Powershell using the command: \'add-PSSnapIn GetLastLog" +
                "on\' and Get-LastLogon";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.UserSearchlist);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtUserName);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.Search);
            this.panel1.Controls.Add(this.DomainNamedcomboBox);
            this.panel1.Controls.Add(this.SelectAll);
            this.panel1.Controls.Add(this.GetLastLogonDetails);
            this.panel1.Controls.Add(this.DeSelectAll);
            this.panel1.Location = new System.Drawing.Point(15, 234);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(993, 572);
            this.panel1.TabIndex = 13;
            // 
            // Search
            // 
            this.Search.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(183)))));
            this.Search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Search.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Search.ForeColor = System.Drawing.Color.White;
            this.Search.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Search.Location = new System.Drawing.Point(494, 59);
            this.Search.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(75, 37);
            this.Search.TabIndex = 1;
            this.Search.Text = "Search";
            this.Search.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.Search.UseVisualStyleBackColor = false;
            this.Search.Click += new System.EventHandler(this.Search_Click);
            // 
            // SelectAll
            // 
            this.SelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.SelectAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.SelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectAll.Font = new System.Drawing.Font("Arial", 7F);
            this.SelectAll.Location = new System.Drawing.Point(40, 133);
            this.SelectAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SelectAll.Name = "SelectAll";
            this.SelectAll.Size = new System.Drawing.Size(85, 32);
            this.SelectAll.TabIndex = 7;
            this.SelectAll.Text = "Select All";
            this.SelectAll.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SelectAll.UseVisualStyleBackColor = false;
            this.SelectAll.Click += new System.EventHandler(this.SelectAll_Click);
            // 
            // GetLastLogonDetails
            // 
            this.GetLastLogonDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(183)))));
            this.GetLastLogonDetails.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(183)))));
            this.GetLastLogonDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetLastLogonDetails.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GetLastLogonDetails.ForeColor = System.Drawing.Color.White;
            this.GetLastLogonDetails.Image = global::SendGreetingcheck.Properties.Resources.last_logon1;
            this.GetLastLogonDetails.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GetLastLogonDetails.Location = new System.Drawing.Point(750, 124);
            this.GetLastLogonDetails.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GetLastLogonDetails.Name = "GetLastLogonDetails";
            this.GetLastLogonDetails.Size = new System.Drawing.Size(206, 37);
            this.GetLastLogonDetails.TabIndex = 9;
            this.GetLastLogonDetails.Text = "Get Last Logon Details";
            this.GetLastLogonDetails.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GetLastLogonDetails.UseVisualStyleBackColor = false;
            this.GetLastLogonDetails.Click += new System.EventHandler(this.GetLastLogonDetails_Click);
            // 
            // DeSelectAll
            // 
            this.DeSelectAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.DeSelectAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.DeSelectAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DeSelectAll.Font = new System.Drawing.Font("Arial", 7F);
            this.DeSelectAll.Location = new System.Drawing.Point(132, 133);
            this.DeSelectAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DeSelectAll.Name = "DeSelectAll";
            this.DeSelectAll.Size = new System.Drawing.Size(100, 32);
            this.DeSelectAll.TabIndex = 8;
            this.DeSelectAll.Text = "Deselect All";
            this.DeSelectAll.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.DeSelectAll.UseVisualStyleBackColor = false;
            this.DeSelectAll.Click += new System.EventHandler(this.DeSelectAll_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.ErrorImage = null;
            this.pictureBox4.Image = global::SendGreetingcheck.Properties.Resources.star1;
            this.pictureBox4.Location = new System.Drawing.Point(38, 176);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(18, 18);
            this.pictureBox4.TabIndex = 11;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.ErrorImage = null;
            this.pictureBox3.Image = global::SendGreetingcheck.Properties.Resources.star1;
            this.pictureBox3.Location = new System.Drawing.Point(38, 142);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(18, 18);
            this.pictureBox3.TabIndex = 8;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.ErrorImage = null;
            this.pictureBox2.Image = global::SendGreetingcheck.Properties.Resources.star1;
            this.pictureBox2.Location = new System.Drawing.Point(38, 109);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(18, 18);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // LastLogonTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(1042, 818);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LastLogonTool";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LastLogonTool";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
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
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
    }
}