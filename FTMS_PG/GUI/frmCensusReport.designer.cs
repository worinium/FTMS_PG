namespace FTMS_PG
{
    partial class frmCensusReport
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
            this.rprtViewer = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.censusReport1 = new FTMS_PG.ReportsAndQRLabel.CensusReport();
            this.SuspendLayout();
            // 
            // rprtViewer
            // 
            this.rprtViewer.ActiveViewIndex = -1;
            this.rprtViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rprtViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rprtViewer.Cursor = System.Windows.Forms.Cursors.Default;
            this.rprtViewer.Location = new System.Drawing.Point(0, 65);
            this.rprtViewer.Margin = new System.Windows.Forms.Padding(2);
            this.rprtViewer.Name = "rprtViewer";
            this.rprtViewer.Size = new System.Drawing.Size(490, 349);
            this.rprtViewer.TabIndex = 0;
            this.rprtViewer.ToolPanelWidth = 150;
            // 
            // frmCensusReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 414);
            this.Controls.Add(this.rprtViewer);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmCensusReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Census Report ";
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer rprtViewer = null;
        private ReportsAndQRLabel.CensusReport censusReport1;
    }
}