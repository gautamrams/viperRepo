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
            this.DomainControllerlist = new System.Windows.Forms.ListView();
            this.LastLogonTimestampradioButton = new System.Windows.Forms.RadioButton();
            this.LastLogonradioButton = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.GenerateReport = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GenerateReport);
            this.groupBox1.Controls.Add(this.DomainControllerlist);
            this.groupBox1.Controls.Add(this.LastLogonTimestampradioButton);
            this.groupBox1.Controls.Add(this.LastLogonradioButton);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 214);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // DomainControllerlist
            // 
            this.DomainControllerlist.Location = new System.Drawing.Point(12, 61);
            this.DomainControllerlist.Name = "DomainControllerlist";
            this.DomainControllerlist.Size = new System.Drawing.Size(463, 110);
            this.DomainControllerlist.TabIndex = 3;
            this.DomainControllerlist.UseCompatibleStateImageBehavior = false;
            // 
            // LastLogonTimestampradioButton
            // 
            this.LastLogonTimestampradioButton.AutoSize = true;
            this.LastLogonTimestampradioButton.Location = new System.Drawing.Point(304, 22);
            this.LastLogonTimestampradioButton.Name = "LastLogonTimestampradioButton";
            this.LastLogonTimestampradioButton.Size = new System.Drawing.Size(174, 17);
            this.LastLogonTimestampradioButton.TabIndex = 2;
            this.LastLogonTimestampradioButton.TabStop = true;
            this.LastLogonTimestampradioButton.Text = "\"lastLogonTimestamp\" Attribute";
            this.LastLogonTimestampradioButton.UseVisualStyleBackColor = true;
            // 
            // LastLogonradioButton
            // 
            this.LastLogonradioButton.AutoSize = true;
            this.LastLogonradioButton.Location = new System.Drawing.Point(174, 22);
            this.LastLogonradioButton.Name = "LastLogonradioButton";
            this.LastLogonradioButton.Size = new System.Drawing.Size(123, 17);
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
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select Your Preferred Attribute:";
            // 
            // GenerateReport
            // 
            this.GenerateReport.Image = global::SendGreetingcheck.Properties.Resources.generate_report;
            this.GenerateReport.Location = new System.Drawing.Point(199, 180);
            this.GenerateReport.Name = "GenerateReport";
            this.GenerateReport.Size = new System.Drawing.Size(118, 24);
            this.GenerateReport.TabIndex = 4;
            this.GenerateReport.UseVisualStyleBackColor = true;
            this.GenerateReport.Click += new System.EventHandler(this.GenerateReport_Click);
            // 
            // ListDC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cornsilk;
            this.ClientSize = new System.Drawing.Size(512, 233);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
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