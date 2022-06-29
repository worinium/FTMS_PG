using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FTMS.Common.DTOs;
using FTMS.DAL;
using FTMS.Common;
using System.ComponentModel;
using MaterialSkin.Controls;


namespace FTMS_PG
{
    public partial class frmFileForward : Form
    {
        private String file_number;
        private Transaction last_transaction;
        private BindingList<FileResults> batchFiles;
        public String TrackingRemark = "";

        public frmFileForward(BindingList<FileResults> files)
        {
            InitializeComponent();
            //-- forward single file.
            if (files.Count == 1)
            {
                FileResults fi = files.FirstOrDefault();
                file_number = fi.file_no;
                last_transaction = new Transaction
                {
                    FileCondition = fi.current_condition,
                    NumOfPages = fi.num_of_pages,
                    ToLocation = fi.current_location
                };
            }
            //-- forward multiple files.
            else if (files.Count > 0)
            {
                file_number = "BATCH";
                last_transaction = new Transaction();
                last_transaction.FileCondition = "NA";
                var batchLocations = files.Select(f => f.current_location).Distinct();
                if (batchLocations.Count() == 1)
                    last_transaction.ToLocation = batchLocations.FirstOrDefault();
                else last_transaction.ToLocation = "MULTIPLE";
                last_transaction.NumOfPages = 0;
                cmbxFileCondition.Enabled = false;
                txtNumberPages.Enabled = false;
            }
            batchFiles = files;

            label2.Text = file_number;
            label4.Text = last_transaction.FileCondition;
            label8.Text = last_transaction.NumOfPages.ToString();
            label15.Text = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            label7.Text = last_transaction.ToLocation;

            cmbxToLocation.DataSource = new BindingSource(ManageFilesDataManager.getLocations(), null);
            cmbxToLocation.DisplayMember = "description";
            cmbxToLocation.ValueMember = "mr_code";
            cmbxToLocation.SelectedValue = "";
            cmbxFileCondition.DataSource = new BindingSource(ManageFilesDataManager.getFileConditions(), null);
            cmbxFileCondition.DisplayMember = "description";
            cmbxFileCondition.ValueMember = "mr_code";
            cmbxFileCondition.SelectedValue = "";
        }

         /// <summary>
        /// set 'tolocation' combobox with the current location when checkin list of files.
        /// </summary>
        /// <param name="mrcode"></param>
        public void setToLocationAuto(String mrcode)
        {
            this.cmbxToLocation.SelectedValue = mrcode;
        }

        public void btnForward_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                //-- check if a location is selected.
                if (cmbxToLocation.SelectedValue != null)
                {
                    //-- check if the number of pages has changed.
                    if (txtNumberPages.Text != "" && Convert.ToInt32(txtNumberPages.Text) < Convert.ToInt32(label8.Text))
                    {
                        MessageBox.Show("Error: Number of pages has decreased!");
                        return;
                    }
                    List<Transaction> tran = new List<Transaction>();
                    foreach (var file in batchFiles)
                    {
                        Transaction nT = new Transaction();
                        nT.TransactionID = file.transaction_id;
                        nT.FileCondition = cmbxFileCondition.SelectedValue == null ? file.file_condition_mrcode : cmbxFileCondition.SelectedValue.ToString();
                        nT.FileID = file.file_id;
                        nT.LoggedUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        nT.PCName = Environment.MachineName;
                        nT.ToLocation = cmbxToLocation.SelectedValue.ToString();
                        nT.NumOfPages = txtNumberPages.Text == "" ? file.num_of_pages : Convert.ToInt32(txtNumberPages.Text);
                        nT.FromLocation = file.to_location_mrcode;
                        nT.TransactionDate = DateTime.Now;
                        tran.Add(nT);
                    }
                    //-- show report form to add a remark and generate report.
                    frmReportMaker fm = new frmReportMaker(batchFiles.Count + " file(s) have been sent to location " + cmbxToLocation.Text.ToString());
                    fm.Owner = this;
                    fm.batchFiles = batchFiles;
                    fm.fromTxt = label7.Text;
                    fm.toTxt = ((Location)cmbxToLocation.SelectedItem).description;
                    fm.ShowDialog();
                    //-- set the remark in transaction list.
                    foreach (var tr in tran)
                        tr.TrackingRemark = TrackingRemark;
                    //-- insert transactions.
                    ForwardFilesDataManager.addTransactions(tran);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error: Please select a location to transfer the file(s) to.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to forward a file due to: " + ex.Message);
                Console.Write(ex.Message);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
