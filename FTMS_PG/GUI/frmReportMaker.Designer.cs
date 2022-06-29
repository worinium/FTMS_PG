namespace FTMS_PG
{
    partial class frmReportMaker
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
            this.rchRemark = new System.Windows.Forms.RichTextBox();
            this.btnNoPrint = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.rchStatus = new System.Windows.Forms.RichTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 87);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Remark";
            // 
            // rchRemark
            // 
            this.rchRemark.Location = new System.Drawing.Point(4, 107);
            this.rchRemark.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rchRemark.Name = "rchRemark";
            this.rchRemark.Size = new System.Drawing.Size(272, 155);
            this.rchRemark.TabIndex = 1;
            this.rchRemark.Text = "";
            // 
            // btnNoPrint
            // 
            this.btnNoPrint.ForeColor = System.Drawing.Color.Black;
            this.btnNoPrint.Location = new System.Drawing.Point(9, 12);
            this.btnNoPrint.Margin = new System.Windows.Forms.Padding(1);
            this.btnNoPrint.Name = "btnNoPrint";
            this.btnNoPrint.Size = new System.Drawing.Size(248, 28);
            this.btnNoPrint.TabIndex = 2;
            this.btnNoPrint.Text = "Save Remark and Dont Print Report";
            this.btnNoPrint.UseVisualStyleBackColor = true;
            this.btnNoPrint.Click += new System.EventHandler(this.btnNoPrint_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.ForeColor = System.Drawing.Color.Black;
            this.btnPrint.Location = new System.Drawing.Point(9, 43);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(1);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(248, 28);
            this.btnPrint.TabIndex = 3;
            this.btnPrint.Text = "Save Remark and Print Report";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // rchStatus
            // 
            this.rchStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rchStatus.BackColor = System.Drawing.Color.Gainsboro;
            this.rchStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rchStatus.Enabled = false;
            this.rchStatus.Location = new System.Drawing.Point(4, 4);
            this.rchStatus.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rchStatus.Name = "rchStatus";
            this.rchStatus.Size = new System.Drawing.Size(272, 79);
            this.rchStatus.TabIndex = 5;
            this.rchStatus.Text = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.rchStatus);
            this.panel2.Controls.Add(this.rchRemark);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(0, 2);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(281, 353);
            this.panel2.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnNoPrint);
            this.panel1.Controls.Add(this.btnPrint);
            this.panel1.Location = new System.Drawing.Point(4, 267);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(273, 78);
            this.panel1.TabIndex = 6;
            // 
            // frmReportMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 355);
            this.Controls.Add(this.panel2);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "frmReportMaker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Report Maker";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox rchRemark;
        private System.Windows.Forms.Button btnNoPrint;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.RichTextBox rchStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
    }
}