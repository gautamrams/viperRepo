namespace SendGreetingcheck
{
    partial class ListDC
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.GenerateReport = new System.Windows.Forms.Button();
            this.DomainControllerlist = new System.Windows.Forms.ListView();
            this.LastLogonTimestampradioButton = new System.Windows.Forms.RadioButton();
            this.LastLogonradioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.groupBox1.Controls.Add(this.GenerateReport);
            this.groupBox1.Controls.Add(this.DomainControllerlist);
            this.groupBox1.Controls.Add(this.LastLogonTimestampradioButton);
            this.groupBox1.Controls.Add(this.LastLogonradioButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(18, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(732, 329);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // GenerateReport
            // 
            this.GenerateReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(183)))));
            this.GenerateReport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(133)))), ((int)(((byte)(183)))));
            this.GenerateReport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GenerateReport.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateReport.ForeColor = System.Drawing.Color.White;
            this.GenerateReport.Image = global::SendGreetingcheck.Properties.Resources.last_logon1;
            this.GenerateReport.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GenerateReport.Location = new System.Drawing.Point(278, 277);
            this.GenerateReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GenerateReport.Name = "GenerateReport";
            this.GenerateReport.Size = new System.Drawing.Size(185, 37);
            this.GenerateReport.TabIndex = 4;
            this.GenerateReport.Text = "Generate Report";
            this.GenerateReport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GenerateReport.UseVisualStyleBackColor = false;
            this.GenerateReport.Click += new System.EventHandler(this.GenerateReport_Click);
            // 
            // DomainControllerlist
            // 
            this.DomainControllerlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.DomainControllerlist.Location = new System.Drawing.Point(18, 94);
            this.DomainControllerlist.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DomainControllerlist.Name = "DomainControllerlist";
            this.DomainControllerlist.OwnerDraw = true;
            this.DomainControllerlist.Size = new System.Drawing.Size(692, 167);
            this.DomainControllerlist.TabIndex = 3;
            this.DomainControllerlist.UseCompatibleStateImageBehavior = false;
            this.DomainControllerlist.DrawColumnHeader += new System.Windows.Forms.DrawListViewColumnHeaderEventHandler(this.DomainControllerlist_DrawColumnHeader);
            this.DomainControllerlist.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.DomainControllerlist_DrawItem);
            this.DomainControllerlist.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.DomainControllerlist_DrawSubItem);
            // 
            // LastLogonTimestampradioButton
            // 
            this.LastLogonTimestampradioButton.AutoSize = true;
            this.LastLogonTimestampradioButton.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastLogonTimestampradioButton.Location = new System.Drawing.Point(456, 34);
            this.LastLogonTimestampradioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LastLogonTimestampradioButton.Name = "LastLogonTimestampradioButton";
            this.LastLogonTimestampradioButton.Size = new System.Drawing.Size(251, 22);
            this.LastLogonTimestampradioButton.TabIndex = 2;
            this.LastLogonTimestampradioButton.TabStop = true;
            this.LastLogonTimestampradioButton.Text = "\"lastLogonTimestamp\" Attribute";
            this.LastLogonTimestampradioButton.UseVisualStyleBackColor = true;
            // 
            // LastLogonradioButton
            // 
            this.LastLogonradioButton.AutoSize = true;
            this.LastLogonradioButton.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LastLogonradioButton.Location = new System.Drawing.Point(261, 34);
            this.LastLogonradioButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LastLogonradioButton.Name = "LastLogonradioButton";
            this.LastLogonradioButton.Size = new System.Drawing.Size(174, 22);
            this.LastLogonradioButton.TabIndex = 1;
            this.LastLogonradioButton.TabStop = true;
            this.LastLogonradioButton.Text = "\"lastLogon\" Attribute";
            this.LastLogonradioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label1.Location = new System.Drawing.Point(18, 35);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Your Preferred Attribute:";
            // 
            // ListDC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(768, 358);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ListDC";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "LastLogonTool";
            this.Load += new System.EventHandler(this.ListDC_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView DomainControllerlist;
        private System.Windows.Forms.RadioButton LastLogonTimestampradioButton;
        private System.Windows.Forms.RadioButton LastLogonradioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button GenerateReport;
    }
}