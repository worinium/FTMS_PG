namespace FTMS_PG
{
    partial class frmQRCapture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQRCapture));
            this.grdQRFiles = new System.Windows.Forms.DataGridView();
            this.file_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.file_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.register_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Owner_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.currentLocation = new System.Windows.Forms.Label();
            this.btnForwardManual = new System.Windows.Forms.Button();
            this.cmbxCensusLocations = new System.Windows.Forms.ComboBox();
            this.panelQRForwardChekin = new System.Windows.Forms.Panel();
            this.btnQRForward = new MaterialSkin.Controls.MaterialRaisedButton();
            this.panelCensus = new System.Windows.Forms.Panel();
            this.btnQRCensus = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lblResult = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grdQRFiles)).BeginInit();
            this.panelQRForwardChekin.SuspendLayout();
            this.panelCensus.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdQRFiles
            // 
            this.grdQRFiles.AllowUserToAddRows = false;
            this.grdQRFiles.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdQRFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdQRFiles.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.file_id,
            this.file_no,
            this.register_no,
            this.Owner_Name});
            this.grdQRFiles.Location = new System.Drawing.Point(-1, 26);
            this.grdQRFiles.Margin = new System.Windows.Forms.Padding(0);
            this.grdQRFiles.Name = "grdQRFiles";
            this.grdQRFiles.ReadOnly = true;
            this.grdQRFiles.Size = new System.Drawing.Size(672, 333);
            this.grdQRFiles.TabIndex = 0;
            // 
            // file_id
            // 
            this.file_id.FillWeight = 30F;
            this.file_id.HeaderText = "File ID";
            this.file_id.Name = "file_id";
            this.file_id.ReadOnly = true;
            // 
            // file_no
            // 
            this.file_no.FillWeight = 45F;
            this.file_no.HeaderText = "File Number";
            this.file_no.Name = "file_no";
            this.file_no.ReadOnly = true;
            // 
            // register_no
            // 
            this.register_no.FillWeight = 35F;
            this.register_no.HeaderText = "Register No";
            this.register_no.Name = "register_no";
            this.register_no.ReadOnly = true;
            // 
            // Owner_Name
            // 
            this.Owner_Name.HeaderText = "Owner Name";
            this.Owner_Name.Name = "Owner_Name";
            this.Owner_Name.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(-1, -1);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(669, 27);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please use the barcode reader on the forwarded files ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentLocation
            // 
            this.currentLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.currentLocation.AutoSize = true;
            this.currentLocation.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLocation.Location = new System.Drawing.Point(263, 7);
            this.currentLocation.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.currentLocation.Name = "currentLocation";
            this.currentLocation.Size = new System.Drawing.Size(125, 22);
            this.currentLocation.TabIndex = 3;
            this.currentLocation.Text = "current_location";
            this.currentLocation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.currentLocation.Visible = false;
            // 
            // btnForwardManual
            // 
            this.btnForwardManual.Location = new System.Drawing.Point(4, 4);
            this.btnForwardManual.Margin = new System.Windows.Forms.Padding(4);
            this.btnForwardManual.Name = "btnForwardManual";
            this.btnForwardManual.Size = new System.Drawing.Size(143, 26);
            this.btnForwardManual.TabIndex = 5;
            this.btnForwardManual.Text = "Add Files Manually";
            this.btnForwardManual.UseVisualStyleBackColor = true;
            this.btnForwardManual.Visible = false;
            this.btnForwardManual.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnForwardManual_Click);
            // 
            // cmbxCensusLocations
            // 
            this.cmbxCensusLocations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxCensusLocations.FormattingEnabled = true;
            this.cmbxCensusLocations.Location = new System.Drawing.Point(7, 4);
            this.cmbxCensusLocations.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbxCensusLocations.Name = "cmbxCensusLocations";
            this.cmbxCensusLocations.Size = new System.Drawing.Size(327, 24);
            this.cmbxCensusLocations.TabIndex = 6;
            // 
            // panelQRForwardChekin
            // 
            this.panelQRForwardChekin.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelQRForwardChekin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelQRForwardChekin.Controls.Add(this.btnQRForward);
            this.panelQRForwardChekin.Controls.Add(this.currentLocation);
            this.panelQRForwardChekin.Controls.Add(this.btnForwardManual);
            this.panelQRForwardChekin.Location = new System.Drawing.Point(9, 364);
            this.panelQRForwardChekin.Margin = new System.Windows.Forms.Padding(4);
            this.panelQRForwardChekin.Name = "panelQRForwardChekin";
            this.panelQRForwardChekin.Size = new System.Drawing.Size(669, 37);
            this.panelQRForwardChekin.TabIndex = 9;
            // 
            // btnQRForward
            // 
            this.btnQRForward.Depth = 0;
            this.btnQRForward.Location = new System.Drawing.Point(520, 1);
            this.btnQRForward.Margin = new System.Windows.Forms.Padding(4);
            this.btnQRForward.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnQRForward.Name = "btnQRForward";
            this.btnQRForward.Primary = true;
            this.btnQRForward.Size = new System.Drawing.Size(137, 26);
            this.btnQRForward.TabIndex = 12;
            this.btnQRForward.Text = "FORWARD";
            this.btnQRForward.UseVisualStyleBackColor = true;
            this.btnQRForward.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnQRForward_Click);
            // 
            // panelCensus
            // 
            this.panelCensus.BackColor = System.Drawing.Color.Gainsboro;
            this.panelCensus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelCensus.Controls.Add(this.btnQRCensus);
            this.panelCensus.Controls.Add(this.cmbxCensusLocations);
            this.panelCensus.Location = new System.Drawing.Point(9, 364);
            this.panelCensus.Margin = new System.Windows.Forms.Padding(4);
            this.panelCensus.Name = "panelCensus";
            this.panelCensus.Size = new System.Drawing.Size(669, 37);
            this.panelCensus.TabIndex = 10;
            // 
            // btnQRCensus
            // 
            this.btnQRCensus.Depth = 0;
            this.btnQRCensus.Location = new System.Drawing.Point(514, 4);
            this.btnQRCensus.Margin = new System.Windows.Forms.Padding(4);
            this.btnQRCensus.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnQRCensus.Name = "btnQRCensus";
            this.btnQRCensus.Primary = true;
            this.btnQRCensus.Size = new System.Drawing.Size(141, 26);
            this.btnQRCensus.TabIndex = 11;
            this.btnQRCensus.TabStop = false;
            this.btnQRCensus.Text = "Save Census";
            this.btnQRCensus.UseVisualStyleBackColor = true;
            this.btnQRCensus.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnQRCensus_Click);
            // 
            // lblResult
            // 
            this.lblResult.BackColor = System.Drawing.Color.LightGray;
            this.lblResult.Location = new System.Drawing.Point(-1, 393);
            this.lblResult.Margin = new System.Windows.Forms.Padding(0);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(669, 26);
            this.lblResult.TabIndex = 8;
            this.lblResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.grdQRFiles);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lblResult);
            this.panel1.Location = new System.Drawing.Point(9, 4);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 420);
            this.panel1.TabIndex = 11;
            // 
            // frmQRCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(687, 430);
            this.Controls.Add(this.panelCensus);
            this.Controls.Add(this.panelQRForwardChekin);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmQRCapture";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "QR Batch Forward";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmQRCapture_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.grdQRFiles)).EndInit();
            this.panelQRForwardChekin.ResumeLayout(false);
            this.panelQRForwardChekin.PerformLayout();
            this.panelCensus.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdQRFiles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label currentLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn file_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn file_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn register_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn Owner_Name;
        private System.Windows.Forms.Button btnForwardManual;
        private System.Windows.Forms.ComboBox cmbxCensusLocations;
        private System.Windows.Forms.Panel panelQRForwardChekin;
        private System.Windows.Forms.Panel panelCensus;
        private System.Windows.Forms.Label lblResult;
        private MaterialSkin.Controls.MaterialRaisedButton btnQRCensus;
        private MaterialSkin.Controls.MaterialRaisedButton btnQRForward;
        private System.Windows.Forms.Panel panel1;
    }
}