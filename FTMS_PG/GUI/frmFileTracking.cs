using System;
using System.Collections.Generic;
using System.Linq;
using FTMS.Common.DTOs;
using FTMS.Common;
using FTMS.DAL;
using FTMS.Common.BLL;
using System.Security.Principal;
using System.ComponentModel;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MaterialSkin.Controls;


namespace FTMS_PG.GUI
{
    public partial class frmFileTracking : MaterialForm
    {
        //-- binding list facilite deleting from dataGridView.
        BindingList<FileResults> lsBatchResult = new BindingList<FileResults>();
        BindingList<FileResults> lsRequests = new BindingList<FileResults>();
        Boolean oldFileMappingExists = false;
        Boolean allowNewNumber = false;
        private readonly string _watermarkSetting = Helpers.getPublicSetting(Helpers.Constants.waterMarkerSetting);
        private readonly string _ftmsEnvironmentSetting = Helpers.getFtmsSetting(Helpers.Constants.ftmsEnvironmentSetting);
        private readonly string _AltFileNoSetting = Helpers.getFtmsSetting(Helpers.Constants.AltFileNoSetting);
        private readonly string _batchNoLabelSetting = Helpers.getFtmsSetting(Helpers.Constants.BatchNoLabelSetting);
        private readonly string _batSizeSetting = Helpers.getFtmsSetting(Helpers.Constants.BatSizeSetting);
        private readonly string _FileNoOffsetSetting = Helpers.getFtmsSetting(Helpers.Constants.FileNoOffsetSetting);
        #region Form Functions

        public frmFileTracking()
        {
            try
            {
                InitializeComponent();
                // Giving a Customized Skin to the Form
                ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);

                if (!Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.RequestFileTabSetting)))
                    tabControl1.TabPages.Remove(tabPage4);
                if (!Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.FileCensusTabSetting)))
                    tabControl1.TabPages.Remove(tabPage5);

                oldFileMappingExists = Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.oldFileExistsSetting));
                if (!oldFileMappingExists)
                {
                    grpBoxConvert.Hide();
                    grpBoxConvert.Visible = false;
                    cmbxFileType3.Visible = false;
                    txtFileOld.Visible = false;
                    txtFileLetter.Visible = false;
                }
                else
                {
                    cmbxFileType2.Visible = false;
                    txtFileAlias.Location = new System.Drawing.Point(txtPhoneNumber.Location.X, txtFileAlias.Location.Y);
                }

                if (_ftmsEnvironmentSetting == Helpers.Constants.ftmsEnvironmentMerlin)
                {
                    ckbxNewNo.Visible = true;
                    allowNewNumber = true;
                }

                refreshDatabind();
                if (!IsUserAdministrator())
                    btnOpenConfig.Hide();

                //btnOpenConfig.Show();
                var location = ForwardFilesDataManager.getLocation(FTMS_PG.Properties.Settings.Default.CurrentRoomCode);
                String CurrentRoom = location != null ? location.description : Helpers.Constants.notConfigured;
                currentRoom.Text = CurrentRoom;

                if (!Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.AlternativeFileSetting)))
                {
                    cmbxFileType2.Visible = false;
                    txtFileAlias.Visible = false;
                    label3.Visible = false;
                    label20.Visible = false;
                    txtAltFileCriterion.Visible = false;
                }
                if (Helpers.Constants.notConfigured == CurrentRoom)
                {
                    btnMassCheckin.Enabled = false;
                    btnBatchCheckin.Enabled = false;
                }
                label8.Text = Helpers.getFtmsSetting(Helpers.Constants.FileNoLabelTextSetting) + "*";
                label3.Text = _AltFileNoSetting;
                //label3.Location = new System.Drawing.Point(35, 199);
                label20.Text = _AltFileNoSetting;
                btnSavePrint.Visible = Properties.Settings.Default.PrintLabel;
                btnInfo.Visible = Properties.Settings.Default.PrintLabel;
                toolTip1.SetToolTip(dtFrom, "Activating this field will cause the search to go through all transactions, not just the latest");
                toolTip1.SetToolTip(dtTo, "Activating this field will cause the search to go through all transactions, not just the latest");
                label19.Text = Helpers.getFtmsSetting(Helpers.Constants.RackNoLabelSetting); // For NAGIS
                label18.Text = _batchNoLabelSetting; // For NAGIS
                label17.Text = _batchNoLabelSetting;
                btnInfo.Enabled = false;
            }
            catch (Exception ex)
            {
                String Message = string.Format("Error in: frmFileTracking(). {0}. ", ex.Message);
                if (ex.InnerException != null)
                    Message += "Inner Exception: " + ex.InnerException.Message;
                MessageBox.Show(Message);
            }
        }

        private void btnOpenConfig_Click(object sender, EventArgs e)
        {
            using (frmConfig cF = new frmConfig())
            {
                cF.Owner = this;
                cF.ShowDialog();
            }

        }
        #region Forwards Files
        private void btnQRForward_Click(object sender, EventArgs e)
        {
            var location = grpBoxKeyBoard.PointToScreen(System.Drawing.Point.Empty);
            using (frmQRCapture qf = new frmQRCapture())
            {
                qf.StartPosition = FormStartPosition.Manual;
                qf.Location = location;
                qf.Owner = this;
                qf.ShowDialog();
            }

        }
        private void btnMassCheckin_Click(object sender, EventArgs e)
        {
            var location = grpBoxKeyBoard.PointToScreen(System.Drawing.Point.Empty);
            using (frmQRCapture qf = new frmQRCapture(true))
            {
                qf.StartPosition = FormStartPosition.Manual;
                qf.Location = location;
                qf.Owner = this;
                qf.ShowDialog();
            }

        }
        #endregion

        #region Manage Files

        private void chbxROFOexists_CheckedChanged(object sender, EventArgs e)
        {
            rofo_date.Enabled = chbxROFOexists.Checked;
            if (!chbxROFOexists.Checked)
                rofo_date_label.Text = rofo_date_label.Text.Substring(0, rofo_date_label.Text.Length - 1);
            else
                rofo_date_label.Text += "*";
        }

        private void chbxCOFOexists_CheckedChanged(object sender, EventArgs e)
        {
            commencement_date.Enabled = chbxCOFOexists.Checked;
            if (!chbxCOFOexists.Checked)
                commencement_date_label.Text = commencement_date_label.Text.Substring(0, commencement_date_label.Text.Length - 1);
            else
                commencement_date_label.Text += "*";
        }

        private void chbxAppExists_CheckedChanged(object sender, EventArgs e)
        {
            application_date.Enabled = chbxAppExists.Checked;
            if (!chbxAppExists.Checked)
                app_date_label.Text = app_date_label.Text.Substring(0, app_date_label.Text.Length - 1);
            else
                app_date_label.Text += "*";
        }
        private void btnFetchStickerRecord(object sender, EventArgs e)
        {
            try
            {
                if (txtFileID.Text != "")
                {
                    var file_id = Convert.ToInt32(txtFileID.Text);
                    if (ManageFilesDataManager.fileStickerExist(file_id))
                    {
                        frmManageFiles m = new frmManageFiles(file_id);
                        m.ShowDialog();
                    }
                    else
                        MessageBox.Show("Ftms Sticker Not Yet Captured", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Please fetch a file");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while getting data of the file sticker due to " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion


        #region Generic Functions

        public void refreshDataBindFileType()
        {
            //-- land file type codes.
            var landFileCodes = ManageFilesDataManager.getFileTypes().Where(c => (c.fileClassificationCode == Helpers.Constants.landFileClassfcode || c.fileClassificationCode == Helpers.Constants.AdminFileClassfcode || c.fileClassificationCode == Helpers.Constants.TownPlanningClassfcode || c.fileClassificationCode == Helpers.Constants.ValuationFileClassfcode) && c.active).Select(c => c.fileTypeCode).Distinct().ToList();
            cmbxFileTypes.DataSource = new BindingList<String>(landFileCodes);
            cmbxFileTypes.SelectedIndex = -1;

            //-- other file type codes.
            var otherFileCodes = new List<String>();
            otherFileCodes.Add("");
            otherFileCodes.AddRange(ManageFilesDataManager.getFileTypes().Where(c => c.fileClassificationCode != Helpers.Constants.landFileClassfcode && c.fileClassificationCode != Helpers.Constants.AdminFileClassfcode && c.fileClassificationCode != Helpers.Constants.TownPlanningClassfcode && c.fileClassificationCode != Helpers.Constants.ValuationFileClassfcode && c.fileClassificationCode != Helpers.Constants.TestFileClassfcode && c.active).Select(c => c.fileTypeCode).Distinct().ToList());

            cmbxFileType2.DataSource = new BindingList<String>(otherFileCodes);
            cmbxFileType2.SelectedIndex = 0;

            //the old land file codes are usually left disabled in the database so no one creates new instances of them
            var otherlandFileCodes = new List<String>();
            otherlandFileCodes.Add("");
            otherlandFileCodes.AddRange(ManageFilesDataManager.getFileTypes().Where(c => c.fileClassificationCode == Helpers.Constants.OldLandFileClassfcode).Select(c => c.fileTypeCode).Distinct().ToList());

            cmbxFileType3.DataSource = new BindingList<String>(otherlandFileCodes);
            cmbxFileType3.SelectedIndex = 0;

        }

        /// <summary>
        /// Called on init and when the configuration tables change
        /// </summary>
        public void refreshDatabind()
        {
            try
            {
                //-- Manage Files Tab --//
                cmbxCondition.DataSource = new BindingList<FileCondition>(ManageFilesDataManager.getFileConditions());
                cmbxCondition.DisplayMember = "description";
                cmbxCondition.ValueMember = "mr_code";
                cmbxCondition.SelectedValue = "";

                //-- land file type codes.
                var landFileCodes = ManageFilesDataManager.getFileTypes().Where(c => (c.fileClassificationCode == Helpers.Constants.landFileClassfcode || c.fileClassificationCode == Helpers.Constants.AdminFileClassfcode || c.fileClassificationCode == Helpers.Constants.TownPlanningClassfcode || c.fileClassificationCode == Helpers.Constants.ValuationFileClassfcode) && c.active).Select(c => c.fileTypeCode).Distinct().ToList();
                cmbxFileTypes.DataSource = new BindingList<String>(landFileCodes);
                cmbxFileTypes.SelectedIndex = -1;

                //-- other file type codes.
                var otherFileCodes = new List<String>();
                otherFileCodes.Add("");
                otherFileCodes.AddRange(ManageFilesDataManager.getFileTypes().Where(c => c.fileClassificationCode != Helpers.Constants.landFileClassfcode && c.fileClassificationCode != Helpers.Constants.AdminFileClassfcode && c.fileClassificationCode != Helpers.Constants.TownPlanningClassfcode && c.fileClassificationCode != Helpers.Constants.ValuationFileClassfcode && c.fileClassificationCode != Helpers.Constants.TestFileClassfcode && c.active).Select(c => c.fileTypeCode).Distinct().ToList());

                cmbxFileType2.DataSource = new BindingList<String>(otherFileCodes);
                cmbxFileType2.SelectedIndex = 0;

                //the old land file codes are usually left disabled in the database so no one creates new instances of them
                var otherlandFileCodes = new List<String>();
                otherlandFileCodes.Add("");
                otherlandFileCodes.AddRange(ManageFilesDataManager.getFileTypes().Where(c => c.fileClassificationCode == Helpers.Constants.OldLandFileClassfcode).Select(c => c.fileTypeCode).Distinct().ToList());

                cmbxFileType3.DataSource = new BindingList<String>(otherlandFileCodes);
                cmbxFileType3.SelectedIndex = 0;

                var lgas = new List<location_type_upper>();
                lgas.Add(new location_type_upper { mr_code = "", description = " NOT SET " });
                lgas.AddRange(ManageFilesDataManager.getLGAs());

                cmbxLGA.DataSource = new BindingSource(lgas, null);
                cmbxLGA.DisplayMember = "description";
                cmbxLGA.ValueMember = "mr_code";
                cmbxLGA.SelectedValue = "";

                //-- Search and Report Tab --//
                var locations = new List<Location>();
                locations.Add(new Location { mr_code = "", description = "" });
                locations.AddRange(ManageFilesDataManager.getLocations(false));

                cmbxLocationCriterion.DataSource = new BindingList<Location>(locations);
                cmbxLocationCriterion.DisplayMember = "description";
                cmbxLocationCriterion.ValueMember = "mr_code";
                cmbxLocationCriterion.SelectedValue = "";
                //-- Request Files Tab --//
                cmbxFileTypes1.DataSource = new BindingList<String>(landFileCodes);
                cmbxFileTypes1.SelectedIndex = -1;

                //-- File and Doc Census Tab --//
                cmbxLocationCensus.DataSource = new BindingSource(locations, null);
                cmbxLocationCensus.DisplayMember = "description";
                cmbxLocationCensus.ValueMember = "mr_code";
                cmbxLocationCensus.SelectedValue = "";

                cmbxReqFileType.DataSource = new BindingList<String>(landFileCodes);
                cmbxReqFileType.SelectedIndex = -1;
                cmbxReqFileType1.DataSource = new BindingList<String>(landFileCodes);
                cmbxReqFileType1.SelectedIndex = -1;

                //resetFileTab();                
            }
            catch (Exception ex)
            {
                //TODO fix bad throw. This code will throw a runtime error. Nothing is catching it in the manage files tab. You need a catch which displays a message and perform a graceful crash
                throw new Exception(string.Format("refreshDatabind: {0}", ex.Message));
            }
        }

        public bool IsUserAdministrator()
        {
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                //get the currently logged in user
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch (Exception)
            {
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
            return isAdmin;
        }

        /// <summary>
        /// Converts a datagrid to a datatable
        /// </summary>
        /// <param name="dgv"></param>
        /// <returns></returns>
        private void ExportCSVFromDGV(string filePath, DataGridView dgv)
        {
            try
            {
                int colCnt = 0;
                String CSVString = String.Empty;

                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    //Skip invisible columns
                    if (column.Visible)
                    {
                        CSVString += "\"" + column.HeaderText + "\",";
                        colCnt++;
                    }
                }
                //remove the last comma
                CSVString = CSVString.Substring(0, CSVString.Length - 1);
                CSVString += "\n";

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        if (dgv.Columns[row.Cells[i].ColumnIndex].Visible)
                            if (row.Cells[i].Value != null)
                                CSVString += "\"" + row.Cells[i].Value.ToString() + "\","; //.Replace("\"", "\"\"")
                            else
                                CSVString += "\"\",";
                    }
                    CSVString = CSVString.Substring(0, CSVString.Length - 1);
                    CSVString += "\n";
                }
                System.IO.File.WriteAllText(filePath, CSVString);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ExportCSVFromDGV: {0}", ex.Message));
            }
        }

        private void forwardFromBatch(Boolean checkIn = false)
        {
            try
            {
                this.Enabled = false;

                //-- show warning if the files are existing in different locations.
                if (lsBatchResult.Select(c => c.to_location_mrcode).Distinct().Count() > 1)
                {
                    DialogResult result = MessageBox.Show("Warning: Some files are currently present in different locations. Do you wish to continue?", "Warning", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.Cancel)
                        return;
                }

                if (lsBatchResult.Count > 0)
                {
                    var location = grpBoxKeyBoard.PointToScreen(System.Drawing.Point.Empty);
                    frmFileForward f = new frmFileForward(lsBatchResult);
                    f.StartPosition = FormStartPosition.Manual;
                    f.Location = location;
                    //-- clicked Forward button.
                    if (!checkIn)
                        f.ShowDialog();

                    //-- clicked checkIn button.
                    else
                    {
                        //-- set the To_location.
                        f.setToLocationAuto(FTMS_PG.Properties.Settings.Default.CurrentRoomCode);
                        //-- dynamic call for the forward button in frmFileForward.
                        f.btnForward_Click(null, null);
                    }
                    f.Close();
                    grdSearchResults.DataSource = null;
                    grdBatchResults.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to forward from batch due to: " + ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Console.Write(ex.Message);
            }
            finally
            {
                this.Enabled = true;
            }
        }

        /// <summary>
        /// fetches some data from DB based on file number then batch forwards them
        /// </summary>
        /// <param name="filenums"></param>
        public void QRBatch(List<String> filenums, Boolean checkIn, Boolean autoforward)
        {
            try
            {
                grdSearchResults.DataSource = null;
                grdSearchResults.Rows.Clear();
                grdBatchResults.DataSource = null;
                grdBatchResults.Rows.Clear();
                BindingList<FileResults> fr = new BindingList<FileResults>(ManageFilesDataManager.getFiles(filenums));
                if (fr.Count != 0)
                {
                    if (autoforward)
                    {
                        frmFileForward f = new frmFileForward(fr);
                        //-- forward files.
                        if (!checkIn)
                        {
                            f.ShowDialog();
                        }
                        //-- check in files.
                        else
                        {
                            f.setToLocationAuto(FTMS_PG.Properties.Settings.Default.CurrentRoomCode);
                            f.btnForward_Click(null, null);
                        }
                        f.Close();
                    }
                    //-- manual forward.
                    else
                    {
                        lsBatchResult = fr;
                        grdBatchResults.DataSource = null;
                        grdBatchResults.DataSource = lsBatchResult;
                        gridviewColumnsVisbility(grdBatchResults);
                    }
                }
                //-- check if some files are not exist in the database.
                var lsFilesNo = fr.Select(c => c.file_no).ToList();
                var existingFiles = filenums.Where(c => !lsFilesNo.Contains(c)).ToList();
                if (existingFiles.Count > 0)
                    MessageBox.Show("The files: " + string.Join(",", existingFiles.ToArray()) + " don't exist in the database", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to QR forward batch due to: " + ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// fetches some file data from db for the sake of making a census
        /// </summary>
        /// <param name="filenums"></param>
        public void QRCensus(List<string> fileNumbers, string location)
        {
            try
            {
                FileDocCensusDataManager.createCensusDetails(fileNumbers, DateTime.Now, System.Environment.MachineName, System.Security.Principal.WindowsIdentity.GetCurrent().Name, location);
                MessageBox.Show(fileNumbers.Count.ToString() + " file(s) have been added to the file census of your location.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to QR forward batch due to: " + ex.Message, "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void resetManageFilesTab()
        {
            btnInfo.Enabled = false;
            txtFileNumber.Text = "";
            txtFileID.Text = "0";
            owner.Text = "";
            chbxCOFOexists.Checked = false;
            chbxROFOexists.Checked = false;
            chbxAppExists.Checked = false;
            txtFileNumber.Enabled = true;
            txtBatch.Enabled = false;
            txtRackNo.Enabled = false;
            cmbxFileTypes.Enabled = true;
            txtPhoneNumber.Text = "";
            rchRemark.Text = "";
            cmbxCondition.SelectedValue = "";
            txtPageNumber.Text = "";
            cmbxFileTypes.SelectedIndex = -1;
            cmbxFileType2.SelectedIndex = 0;
            cmbxLGA.SelectedValue = "";
            txtFileAlias.Text = "";
            txtBatch.Text = "";
            txtRackNo.Text = "";
            txtFLocation.Text = "";
            rofo_date.Value = Helpers.Constants.nullDate;
            commencement_date.Value = Helpers.Constants.nullDate;
            application_date.Value = Helpers.Constants.nullDate;
            cmbxFileTypes1.SelectedIndex = -1;
            txtReqPurpose.Text = "";
            txtRequestorName.Text = "";
            txtRequestFNo.Text = "";
            if (allowNewNumber)
                ckbxNewNo.Visible = true;
            ckbxNewNo.Checked = false;

            grdRequestGrid.DataSource = null;
            grdRequestGrid.Rows.Clear();
        }
        private void createEditFile(Boolean printQR)
        {
            try
            {
                Boolean newNumber = ckbxNewNo.Checked;
                //-- file number, owner name and file type are mandatory.
                if ((txtFileNumber.Text != "" || newNumber) && owner.Text != "" && cmbxFileTypes.SelectedIndex != -1)
                {
                    //-- dates should not be null and greater than date of today.
                    if ((!chbxAppExists.Checked || (application_date.Value.Date <= DateTime.Now.Date && application_date.Value.Date > Helpers.Constants.nullDate)) && (!chbxROFOexists.Checked || (rofo_date.Value.Date <= DateTime.Now.Date && rofo_date.Value.Date > Helpers.Constants.nullDate)) && (!chbxCOFOexists.Checked || (commencement_date.Value.Date <= DateTime.Now.Date && commencement_date.Value.Date > Helpers.Constants.nullDate)))
                    {
                        int fileTypeID = 0;
                        if (newNumber)
                        {
                            fileTypeID = ManageFilesDataManager.checkFileType((String)cmbxFileTypes.SelectedItem);
                            if (fileTypeID == 0)
                            {
                                MessageBox.Show("The Selected file type does not have an autonumber sequence. Please select a file type which supports autonumber.");
                                return;
                            }
                        }
                        this.Enabled = false;
                        FTMS.Common.DTOs.File f = new FTMS.Common.DTOs.File();

                        //-- set the file alias to empty if nothing selected.
                        //-- checking the alias now happens first sincewe need to put the reformatted string in case we are generating a new number.
                        if (!Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.AlternativeFileSetting)) || (cmbxFileType2.SelectedIndex <= 0 && !oldFileMappingExists))
                        {
                            f.FileAlias = "";
                        }
                        else
                        {
                            //-- validating the alias file number.
                            if (!oldFileMappingExists)
                            {
                                f.FileAlias = Helpers.validateFileNumber(txtFileAlias.Text, (String)cmbxFileType2.SelectedItem);
                                if (string.IsNullOrEmpty(f.FileAlias))
                                {
                                    MessageBox.Show("Invalid Alternate File Number. Enter a valid number!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    return;
                                }
                            }
                            else
                                f.FileAlias = txtFileAlias.Text;
                        }

                        if (newNumber)
                        {
                            txtFileNumber.Text = ManageFilesDataManager.generateMerlinFile(fileTypeID, f.FileAlias);
                            f.FileNumber = (String)cmbxFileTypes.SelectedItem + txtFileNumber.Text;
                        }
                        else
                            f.FileNumber = Helpers.validateFileNumber(txtFileNumber.Text, (String)cmbxFileTypes.SelectedItem);

                        //-- check if the file is already exist.
                        var fileExist = ManageFilesDataManager.CheckIfFileExists(f.FileNumber);
                        if (fileExist && Convert.ToInt32(txtFileID.Text) == 0)
                        {
                            MessageBox.Show("File Already Exists! Use fetch instead");
                            return;
                        }
                        //-- validating the file number.
                        if (string.IsNullOrEmpty(f.FileNumber))
                        {
                            MessageBox.Show("Invalid File Number. Enter a valid number!");
                            return;
                        }

                        f.FileID = Convert.ToInt32(txtFileID.Text);
                        //-- Remove front and back slashes from the owner names. Otherwise it will cause problems in the QR code
                        f.OwnerName = owner.Text.Replace("\\", ".");
                        f.OwnerName = f.OwnerName.Replace("/", ".");
                        //-- Remove double spaces.
                        f.OwnerName = (new Regex("[ ]{2,}", RegexOptions.None)).Replace(f.OwnerName, " ");

                        f.CofOExists = chbxCOFOexists.Checked;
                        f.CommencementDate = f.CofOExists ? commencement_date.Value : (DateTime?)null;

                        f.RofOExists = chbxROFOexists.Checked;
                        f.RofODate = f.RofOExists ? rofo_date.Value : (DateTime?)null;

                        f.ApplicationDate = chbxAppExists.Checked ? application_date.Value : (DateTime?)null;
                        f.LGACode = cmbxLGA.SelectedValue != null && cmbxLGA.SelectedValue.ToString() != "" ? cmbxLGA.SelectedValue.ToString() : "";
                        f.PhoneNumber = txtPhoneNumber.Text;
                        f.Remark = rchRemark.Text;

                        int fnum = Helpers.extractNumberFromFilenumber(txtFileNumber.Text);

                        //-- reserve a batch number.
                        if (f.FileID == 0)
                        {
                            f.RegisterNumber = (fnum < int.Parse(_FileNoOffsetSetting) ? 0 : (fnum - int.Parse(_FileNoOffsetSetting)) / int.Parse(_batSizeSetting) + int.Parse(Helpers.getFtmsSetting(Helpers.Constants.BatchNoOffsetSetting))).ToString();
                            //-- turn off the automation if the file number is less than a specified offset.
                        }
                        else
                        {
                            int num = 0;
                            bool isShelfNO = int.TryParse(txtBatch.Text, out num);
                            if (!isShelfNO)
                            {
                                MessageBox.Show("Invalid data in Batch NO");
                                return;
                            }
                            f.RegisterNumber = num.ToString();
                        }

                        //-- reserve a rack number using the same logic as above
                        if (f.FileID == 0)
                        {
                            f.RackNumber = (fnum < int.Parse(_FileNoOffsetSetting) ? 0 : ((fnum - int.Parse(_FileNoOffsetSetting)) / int.Parse(_batSizeSetting) + int.Parse(Helpers.getFtmsSetting(Helpers.Constants.BatFirstRackSetting))) / int.Parse(Helpers.getFtmsSetting(Helpers.Constants.BatPerRackSetting)) + int.Parse(Helpers.getFtmsSetting(Helpers.Constants.RackNoOffsetSetting))).ToString();
                        }
                        else
                        {
                            int num = 0;
                            bool isShelfNO = int.TryParse(txtRackNo.Text, out num);
                            if (!isShelfNO)
                            {
                                MessageBox.Show("Invalid data in Rack NO");
                                return;
                            }
                            f.RackNumber = num.ToString();
                        }
                        //-- get user's location.
                        var location = ForwardFilesDataManager.getLocation(FTMS_PG.Properties.Settings.Default.CurrentRoomCode);
                        //-- if location is not configured, use 'unknown' location.
                        f.CurrentLocationCode = location != null ? location.mr_code : Helpers.Constants.unknownLocationCode;
                        //-- use 'unknown' file condition if nothing is selected. 
                        f.CurrentFileConditionCode = cmbxCondition.SelectedValue == null ? Helpers.Constants.unknownConditionCode : cmbxCondition.SelectedValue.ToString();
                        f.CurrentNumOfPages = txtPageNumber.Text == "" ? 0 : Convert.ToInt32(txtPageNumber.Text);

                        //Checking File Alias on Create/Edit
                        string data = ManageFilesDataManager.getFileNumberOfAlias(f.FileAlias, f.FileNumber);
                        if (!string.IsNullOrEmpty(data))
                        {
                            DialogResult dialogResult = MessageBox.Show(label3.Text + " " + f.FileAlias + " Exist for " + data + "." + "\nDo you want to Proceed", "Information Message", MessageBoxButtons.YesNo);
                            if (dialogResult == DialogResult.Yes)
                                f.FileID = ManageFilesDataManager.CreateEditFile(f);
                            else if (dialogResult == DialogResult.No) return;
                        }
                        else
                            f.FileID = ManageFilesDataManager.CreateEditFile(f);

                        //-- reset the file tab.
                        resetManageFilesTab();
                        refreshDataBindFileType();
                        //-- show a dialog contains the barcode of the file.
                        if (printQR)
                        {
                            frmReport rf = new frmReport(f);
                            rf.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Success. File number is " + f.FileNumber);
                        }
                    }
                    else
                        MessageBox.Show("Invalid date");
                }
                else
                    MessageBox.Show("Missing mandatory fields");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("duplicate values in the index, primary key, or relationship"))
                    MessageBox.Show("Error: Duplicate entries in some fields arent allowed! Please check file number", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show("Failed to create a file due to: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
            }

        }
        private void reportColumnsVisibility()
        {
            foreach (DataGridViewColumn column in grdReportSearchResults.Columns)
            {
                column.Visible = true;
            }
            grdReportSearchResults.Columns["file_id"].Visible = false;
            grdReportSearchResults.Columns["transaction_id"].Visible = false;
            grdReportSearchResults.Columns["to_location_mrcode"].Visible = false;
            grdReportSearchResults.Columns["file_condition_mrcode"].Visible = false;
            if (rdbtnFiles.Checked)
            {
                grdReportSearchResults.Columns["logged_user"].Visible = false;
                grdReportSearchResults.Columns["transaction_date"].Visible = false;
                grdReportSearchResults.Columns["PC_name"].Visible = false;
                grdReportSearchResults.Columns["tracking_remark"].Visible = false;
                grdReportSearchResults.Columns["previous_location"].Visible = false;
                //-- change columns ordering.
                grdReportSearchResults.Columns["BatchNo"].DisplayIndex = 3;
                grdReportSearchResults.Columns["RackNo"].DisplayIndex = 4;
                grdReportSearchResults.Columns["remark"].DisplayIndex = 5;
                grdReportSearchResults.Columns["current_location"].DisplayIndex = 6;
                grdReportSearchResults.Columns["num_of_pages"].DisplayIndex = 7;
                grdReportSearchResults.Columns["current_condition"].DisplayIndex = 8;
            }
            else
            {
                grdReportSearchResults.Columns["owner_name"].Visible = false;
                grdReportSearchResults.Columns["app_date"].Visible = false;
                grdReportSearchResults.Columns["rofo_date"].Visible = false;
                grdReportSearchResults.Columns["commencement_date"].Visible = false;
                grdReportSearchResults.Columns["phone_number"].Visible = false;
                grdReportSearchResults.Columns["file_alias"].Visible = false;
                grdReportSearchResults.Columns["remark"].Visible = false;
                grdReportSearchResults.Columns["lga_code"].Visible = false;
                //-- change columns ordering.
                grdReportSearchResults.Columns["current_location"].DisplayIndex = 2;
                grdReportSearchResults.Columns["previous_location"].DisplayIndex = 3;
                grdReportSearchResults.Columns["BatchNo"].DisplayIndex = 4;
                grdReportSearchResults.Columns["RackNo"].DisplayIndex = 5;
                grdReportSearchResults.Columns["transaction_date"].DisplayIndex = 6;
                grdReportSearchResults.Columns["num_of_pages"].DisplayIndex = 7;
                grdReportSearchResults.Columns["current_condition"].DisplayIndex = 8;
            }
        }

        private void gridviewColumnsVisbility(DataGridView grid)
        {
            foreach (DataGridViewColumn column in grid.Columns)
            {
                column.Visible = false;
            }
            grid.Columns["file_no"].Visible = true;
            grid.Columns["owner_name"].Visible = true;
            grid.Columns["transaction_date"].Visible = true;
            grid.Columns["current_location"].Visible = true;
            grid.Columns["batchNo"].Visible = true;
            grid.Columns["rackNo"].Visible = true;
            //-- change columns width.
            grid.Columns["file_no"].FillWeight = 23;
            grid.Columns["owner_name"].FillWeight = 27;
            grid.Columns["transaction_date"].FillWeight = 25;
            grid.Columns["batchNo"].FillWeight = 12;
            grid.Columns["rackNo"].FillWeight = 12;
            grid.Columns["current_location"].FillWeight = 48;
            //-- change columns ordering.
            grid.Columns["file_no"].DisplayIndex = 0;
            grid.Columns["owner_name"].DisplayIndex = 1;
            grid.Columns["transaction_date"].DisplayIndex = 2;
            grid.Columns["batchNo"].DisplayIndex = 3;
            grid.Columns["rackNo"].DisplayIndex = 4;
            grid.Columns["current_location"].DisplayIndex = 5;
        }

        private void generateRequestPDF(DataGridView grd, String requestID, DateTime requestDate)
        {
            try
            {
                string _requestPathSetting = Helpers.getFtmsSetting(Helpers.Constants.RequestsPathSetting);
                if (!Directory.Exists(_requestPathSetting))
                    Directory.CreateDirectory(_requestPathSetting);

                String fileName = _requestPathSetting + "\\" + "FTMS_Request_" + requestID;
                using (Document document = new Document())
                {
                    using (PdfSmartCopy copy = new PdfSmartCopy(document, new FileStream(fileName + ".pdf", FileMode.Create)))
                    {
                        document.Open();
                        // replace this with your PDF form template
                        PdfReader pdfReader = new PdfReader("ReportsAndQRLabel\\FTMS_File_Request.pdf");
                        using (var ms = new MemoryStream())
                        {
                            using (PdfStamper stamper = new PdfStamper(pdfReader, ms))
                            {
                                AcroFields fields = stamper.AcroFields;
                                fields.SetField("txtRequestID", requestID);
                                fields.SetField("txtRequestOwner", txtRequestorName.Text);
                                fields.SetField("txtRequestDate", requestDate.ToString("dd-MMM-yyyy HH:mm"));
                                fields.SetField("txtRequestPurpose", txtReqPurpose.Text);

                                int rowcounter = 1;
                                foreach (DataGridViewRow row in grdRequestGrid.Rows)
                                {
                                    if (!row.IsNewRow)
                                    {
                                        fields.SetField("txtFileNo_" + rowcounter, row.Cells[0].Value.ToString());
                                        fields.SetField("txtRackNo_" + rowcounter, row.Cells[1].Value.ToString());
                                        fields.SetField("txtBatchNo_" + rowcounter, row.Cells[2].Value.ToString());
                                        fields.SetField("txtLocation_" + rowcounter, row.Cells[3].Value.ToString());
                                        rowcounter++;
                                    }
                                }
                                stamper.FormFlattening = true;
                            }
                            pdfReader = new PdfReader(ms.ToArray());
                            copy.AddPage(copy.GetImportedPage(pdfReader, 1));

                        }
                        pdfReader.Close();
                    }
                }
                try
                {
                    string watermark = WatermarkManager.GetWaterMark();
                    if (watermark != "")
                    {
                        WatermarkManager.AddWaterMark(fileName + ".pdf", fileName + "_t.pdf", watermark);
                        System.IO.File.Delete(fileName + ".pdf");
                        fileName = fileName + "_t";
                    }
                    System.Diagnostics.Process.Start(fileName + ".pdf");
                }
                catch (Exception)
                {
                    MessageBox.Show("Error: Process Could Not Open Generated Report");
                }
                resetManageFilesTab();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating the request due to: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void frmFileTracking_Load(object sender, EventArgs e)
        {
            try
            {
                
                string _version = Helpers.getFtmsSetting(Helpers.Constants.ftmsVersionSetting);
                if (_ftmsEnvironmentSetting == Helpers.Constants.ftmsEnvironmentMerlin)
                {
                    btnOpenConfig.Visible = false;
                    //Display Wildcard Search feature 
                    loadPlaceholder();
                    //Log that the user is not using the latest FTMS version
                    if ("Version " + _version != lblVersion.Text)
                    {
                        Helpers.logMessage(string.Format("An older version of FTMS found on machine for user {0}. {1}.", System.Security.Principal.WindowsIdentity.GetCurrent().Name, lblVersion.Text), "Information");
                        MessageBox.Show("Wrong Version Installed. Please contact your administrator.");
                        Application.Exit();
                    }
                    if (_watermarkSetting == Helpers.Constants.Training)
                        this.Text = "File Tracking Management System - Version " + _version + " (Training)";
                    else if (_watermarkSetting == Helpers.Constants.Development)
                        this.Text = "File Tracking Management System - Version " + _version + " (Development)";
                    else
                        this.Text = "File Tracking Management System - Version " + _version;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("frmFileTracking.frmFileTracking_Load {0}", ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }
        private void btnMassCheckins_Click(object sender, EventArgs e)
        {
            var location = grpBoxKeyBoard.PointToScreen(System.Drawing.Point.Empty);
            using (frmQRCapture qf = new frmQRCapture(true))
            {
                qf.StartPosition = FormStartPosition.Manual;
                qf.Location = location;
                qf.Owner = this;
                qf.ShowDialog();
            }
        }

        private void btnQRForwards_Click(object sender, EventArgs e)
        {
            var location = grpBoxKeyBoard.PointToScreen(System.Drawing.Point.Empty);
            using (frmQRCapture qf = new frmQRCapture(false, false))
            {
                qf.StartPosition = FormStartPosition.Manual;
                qf.Location = location;
                qf.Owner = this;
                qf.ShowDialog();
            }
        }

        private void btnSearchClick(object sender, EventArgs e)
        {
            try
            {
                if (txtSearchCriterion.Text == "KDL 123*")
                   txtSearchCriterion.Text = "";
                this.Cursor = Cursors.WaitCursor;
                this.UseWaitCursor = true;
                grdSearchResults.DataSource = new BindingList<FileResults>(ForwardFilesDataManager.getFiles(txtSearchCriterion.Text));
                gridviewColumnsVisbility(grdSearchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to search due to: " + ex.Message);
                Console.Write(ex.Message);
            }
            finally
            {

                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
            }
        }

        private void btnAddBatchButton_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow r in grdSearchResults.SelectedRows)
                {
                    var fResult = (FileResults)r.DataBoundItem;
                    if (lsBatchResult.Where(c => c.file_id == fResult.file_id).FirstOrDefault() == null)
                        lsBatchResult.Add(fResult);
                }
                grdBatchResults.DataSource = null;
                grdBatchResults.DataSource = lsBatchResult;
                gridviewColumnsVisbility(grdBatchResults);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add batch due to: " + ex.Message, "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Console.Write(ex.Message);
            }
        }
        private void btnBatchCheckin_Click(object sender, EventArgs e)
        {
            forwardFromBatch(true);
            lsBatchResult = new BindingList<FileResults>();
            grdBatchResults.DataSource = null;
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            forwardFromBatch();
            lsBatchResult = new BindingList<FileResults>();
            grdBatchResults.DataSource = null;
        }
        #endregion

        #region Search and Report Module

        private void rdbtnFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (grdReportSearchResults.DataSource != null)
                reportColumnsVisibility();
            else
            {
                grdReportSearchResults.DataSource = null;
                grdReportSearchResults.Rows.Clear();
            }
        }

        private void rdbtnTransactions_CheckedChanged(object sender, EventArgs e)
        {
            if (grdReportSearchResults.DataSource != null)
                reportColumnsVisibility();
            else
            {
                grdReportSearchResults.DataSource = null;
                grdReportSearchResults.Rows.Clear();
            }
        }
        private void btnReportSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.UseWaitCursor = true;
                grdReportSearchResults.DataSource = null;
                grdReportSearchResults.Rows.Clear();

                DateTime fdate = dtFrom.Checked ? dtFrom.Value : new DateTime(1900, 1, 1);
                DateTime tdate = dtTo.Checked ? dtTo.Value : new DateTime(3000, 1, 1);
                DateTime ffdate = dtCFrom.Checked ? dtCFrom.Value.Date : new DateTime(1900, 1, 1);
                DateTime tfdate = dtCTo.Checked ? dtCTo.Value.Date.AddHours(23) : new DateTime(3000, 1, 1);
                List<FileResults> results = null;

                if (rdbtnFiles.Checked || (rdbtnTransactions.Checked && !dtFrom.Checked && !dtTo.Checked))
                    results = SearchRportDataManager.getFileByCriteria(txtFileNumberCriterion.Text, txtAltFileCriterion.Text, txtBatchNo.Text, txtOwnerSearch.Text, cmbxLocationCriterion.SelectedValue.ToString(), fdate, tdate, ffdate, tfdate);
                else
                    results = SearchRportDataManager.getTransactionsByCriteria(txtFileNumberCriterion.Text, txtAltFileCriterion.Text, txtBatchNo.Text, txtOwnerSearch.Text, cmbxLocationCriterion.SelectedValue.ToString(), fdate, tdate, ffdate, tfdate);

                List<FileResults> ds = results;
                grdReportSearchResults.DataSource = new BindingList<FileResults>(ds);
                reportColumnsVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
            }
        }
        private void btnReportReset_Click(object sender, EventArgs e)
        {
            txtFileNumberCriterion.Text = "";
            txtBatchNo.Text = "";
            cmbxLocationCriterion.SelectedValue = "";
            txtAltFileCriterion.Text = "";
            txtOwnerSearch.Text = "";
            dtTo.Checked = false;
            dtFrom.Checked = false;
            dtCTo.Checked = false;
            dtCFrom.Checked = false;
            grdReportSearchResults.Rows.Clear();
        }
        private void btnExportExcelClick(object sender, EventArgs e)
        {
            try
            {
                if (grdReportSearchResults.Rows.Count > 0)
                {
                    this.Enabled = false;
                    // set a default file name
                    saveFileDialog1.FileName = "temp.csv";
                    // set filters - this can be done in properties as well
                    saveFileDialog1.Filter = "CSV (*.csv)|*.csv";

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                       ExportCSVFromDGV(saveFileDialog1.FileName, grdReportSearchResults);
                    System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                        
                }
                else
                    MessageBox.Show("Please Search By Files or Transaction", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
                this.Focus();
            }
        }
        private void btnExportHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdReportSearchResults.Rows.Count > 0)
                {
                    this.Enabled = false;
                    // set a default file name
                    saveFileDialog1.FileName = "temp.csv";
                    // set filters - this can be done in properties as well
                    saveFileDialog1.Filter = "CSV (*.csv)|*.csv";
                    if (rdbtnTransactions.Checked)
                    {
                        if (grdReportSearchResults.Rows.Count == 1)
                        {
                            grdReportSearchResults.Rows[0].Selected = true;
                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                int fileID = Convert.ToInt32(grdReportSearchResults.SelectedRows[0].Cells["file_id"].Value);
                                System.IO.File.WriteAllText(saveFileDialog1.FileName, SearchRportDataManager.getCSVFileTransactions(fileID));
                                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                            }
                        }
                        else if (grdReportSearchResults.Rows.Count > 1 && grdReportSearchResults.SelectedRows.Count == 1)
                        {
                            int index = grdReportSearchResults.CurrentCell.RowIndex;
                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                int fileID = Convert.ToInt32(grdReportSearchResults.SelectedRows[0].Cells["file_id"].Value);
                                System.IO.File.WriteAllText(saveFileDialog1.FileName, SearchRportDataManager.getCSVFileTransactions(fileID));
                                System.Diagnostics.Process.Start(saveFileDialog1.FileName);
                            }
                        }
                        else
                            MessageBox.Show("Please Ensure to Select only one Row");
                    }
                    else
                        MessageBox.Show("Please Select Transaction RadioButton");

                }
                else
                    MessageBox.Show("Please Search By Transactions", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Enabled = true;
                this.Focus();
            }
        }
        #endregion

        #region Manage Files Module
        private void btnFetchFile_Click(object sender, EventArgs e)
        {
            try
            {
                refreshDataBindFileType();
                this.Enabled = false;
                //-- check if the file is exist.
                var fileExist = ManageFilesDataManager.CheckIfFileExists(txtFetchCriterion.Text);
                if (fileExist)
                {
                    var allFileTypes = ManageFilesDataManager.getFileTypes();
                    FileResults flResult = ManageFilesDataManager.getFile(txtFetchCriterion.Text);

                    var fileAbr = Regex.Match(flResult.file_no, @"\b[A-Za-z]+[ ]*").Value;
                    var fileNo = Regex.Match(flResult.file_no, @"\d+").Value;
                    //-- check if the file type code is active ot not.
                    var fl = allFileTypes.Where(c => c.fileTypeCode == fileAbr && !c.active).FirstOrDefault();
                    //-- add the inactive file type to the combobox.
                    if (fl != null)
                    {
                        var tmpFileCode = (BindingList<String>)cmbxFileTypes.DataSource;
                        cmbxFileTypes.DataSource = null;
                        tmpFileCode.Add(fileAbr);
                        cmbxFileTypes.DataSource = tmpFileCode;
                    }
                    cmbxFileTypes.SelectedItem = fileAbr;
                    txtFileNumber.Text = fileNo;

                    //-- check if the file alias type code is active or not.
                    if (!string.IsNullOrEmpty(flResult.file_alias))
                    {
                        var fileAliasAbr = Regex.Match(flResult.file_alias, @"\b[A-Za-z]+[ ]*").Value;
                        var fileAliasNo = Regex.Match(flResult.file_alias, @"\d+").Value;
                        var flAlias = allFileTypes.Where(c => c.fileTypeCode == fileAliasAbr && !c.active).FirstOrDefault();
                        //-- add the inactive file alias type to the combobox.
                        if (flAlias != null)
                        {
                            var tmpFileAliasCode = (BindingList<String>)cmbxFileType2.DataSource;
                            cmbxFileType2.DataSource = null;
                            tmpFileAliasCode.Add(fileAliasAbr);
                            cmbxFileType2.DataSource = tmpFileAliasCode;
                        }
                        cmbxFileType2.SelectedItem = fileAliasAbr;
                        if (!oldFileMappingExists)
                            txtFileAlias.Text = fileAliasNo;
                        else
                            txtFileAlias.Text = flResult.file_alias;
                    }
                    else
                    {
                        cmbxFileType2.SelectedIndex = 0;
                        txtFileAlias.Text = "";
                    }

                    txtFileID.Text = flResult.file_id.ToString();
                    owner.Text = flResult.owner_name;
                    chbxAppExists.Checked = flResult.app_date.HasValue && flResult.app_date.Value > Helpers.Constants.nullDate;
                    chbxROFOexists.Checked = flResult.rofo_date.HasValue && flResult.rofo_date.Value > Helpers.Constants.nullDate;
                    chbxCOFOexists.Checked = flResult.commencement_date.HasValue && flResult.commencement_date.Value > Helpers.Constants.nullDate;
                    cmbxLGA.SelectedValue = flResult.lga_code != null ? flResult.lga_code : "";

                    application_date.Value = flResult.app_date == null ? Helpers.Constants.nullDate : Convert.ToDateTime(flResult.app_date);
                    rofo_date.Value = flResult.rofo_date == null ? Helpers.Constants.nullDate : Convert.ToDateTime(flResult.rofo_date);
                    commencement_date.Value = flResult.commencement_date == null ? Helpers.Constants.nullDate : Convert.ToDateTime(flResult.commencement_date);
                    txtPhoneNumber.Text = flResult.phone_number;
                    rchRemark.Text = flResult.remark;
                    cmbxCondition.SelectedValue = flResult.current_condition != null ? flResult.current_condition : "";
                    txtPageNumber.Text = flResult.num_of_pages.ToString();
                    txtBatch.Text = flResult.batchNo;
                    txtRackNo.Text = flResult.rackNo;
                    txtFileNumber.Enabled = false;
                    cmbxFileTypes.Enabled = false;
                    txtFLocation.Text = flResult.current_location;
                    btnInfo.Enabled = true;
                    ckbxNewNo.Visible = false;
                    ckbxNewNo.Checked = false;

                    if (Properties.Settings.Default.EditBatchNo)
                    {
                        txtBatch.Enabled = true;
                        txtRackNo.Enabled = true;
                    }
                }
                else
                {
                    MessageBox.Show("No matching results");
                    resetManageFilesTab();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to fetch a file due to: " + ex.Message, "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Enabled = true;
            }
        }
        private void ckbxNewNo_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbxNewNo.Checked)
            {
                if (txtFileID.Text != "0")
                {
                    //Unreacheable code
                    ckbxNewNo.Checked = false;
                    MessageBox.Show("Generating a new number is not allowed. Please save your changes or clear the existing data before generating a new number.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtFileNumber.Text = "";
                txtFileNumber.Enabled = false;
            }
            else
                txtFileNumber.Enabled = true;
        }

        private void btnGetNewNumber_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO make this regular expression read from the database configuration
                //TODO fix the letters to bigger ranges if other projects need it
                //TODO handle the case whereby there are more than 1 land file prefix
                if (Regex.Match(txtFileOld.Text, @"^[0-9]{1,4}$").Success && (txtFileLetter.Text == "" || Regex.Match(txtFileLetter.Text, @"^[a-d,A-D]{1}$").Success) && (String)cmbxFileType3.SelectedItem != "")
                {
                    if (txtFileID.Text == "0" || MessageBox.Show("Warning: Any unsaved data will be lost. Continue?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        resetManageFilesTab();
                        cmbxFileTypes.SelectedIndex = cmbxFileTypes.FindStringExact(ManageFilesDataManager.getFileTypes().Where(c => (c.fileClassificationCode == Helpers.Constants.landFileClassfcode || c.fileClassificationCode == Helpers.Constants.AdminFileClassfcode || c.fileClassificationCode == Helpers.Constants.TownPlanningClassfcode || c.fileClassificationCode == Helpers.Constants.ValuationFileClassfcode) && c.active).Select(c => c.fileTypeCode).FirstOrDefault());
                        string reformattedNo = Helpers.formatFileNumber(txtFileOld.Text, (String)cmbxFileType3.SelectedItem);
                        txtFileNumber.Text = Helpers.customConvertOldFile(reformattedNo, ((String)cmbxFileType3.SelectedItem).Trim() + txtFileLetter.Text);
                        txtFileAlias.Text = (String)cmbxFileType3.SelectedItem + reformattedNo + (string.IsNullOrEmpty(txtFileLetter.Text) ? "" : (" " + txtFileLetter.Text.ToUpper()));
                        txtFileLetter.Text = "";
                        txtFileOld.Text = "";
                        cmbxFileType3.SelectedIndex = 0;
                    }
                }
                else
                    MessageBox.Show("Invalid File Number");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating the file number due to: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            resetManageFilesTab();
            refreshDataBindFileType();
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            createEditFile(false);
        }

        private void btnSavePrint_Click(object sender, EventArgs e)
        {
            createEditFile(true);
        }
        #endregion

        #region File Request Module
        private void btnRequestFile_Click(object sender, EventArgs e)
        {
            try
            {
                //-- check if the file number is empty.
                if (txtRequestFNo.Text != "" && cmbxFileTypes1.SelectedIndex != -1)
                {
                    if (grdRequestGrid.Rows.Count < 31)
                    {
                        var Fno = Helpers.validateFileNumber(txtRequestFNo.Text, (String)cmbxFileTypes1.SelectedItem);
                        foreach (DataGridViewRow row in grdRequestGrid.Rows)
                        {
                            if (!row.IsNewRow)
                                //-- check if a file number is already existed in the gird.
                                if (row.Cells[0].Value.ToString().Equals(Fno))
                                {
                                    MessageBox.Show("File already part of this request.");
                                    return;
                                }
                        }
                        FTMS.Common.DTOs.File fetchedFile = RequestFilesDataManager.getFileByNumber(Fno);
                        //-- check if the file is exist.
                        if (fetchedFile != null)
                            grdRequestGrid.Rows.Add(fetchedFile.FileNumber, fetchedFile.RackNumber, fetchedFile.RegisterNumber, fetchedFile.CurrentLocationDesc, fetchedFile.FileID);
                        else
                            MessageBox.Show("File Not Found");
                        txtRequestFNo.Text = "";
                    }
                    else
                        MessageBox.Show("Maximum files reached. Please generate a new request.");
                }
                else
                    MessageBox.Show("Invalid File Number!");
                txtRequestFNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenRequest_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.UseWaitCursor = true;
                btnGenRequest.Enabled = false;
                DateTime requestDate = DateTime.Now;
                if (txtRequestorName.Text != "" && txtReqPurpose.Text != "" && grdRequestGrid.RowCount > 1)
                {
                    DataGridViewColumn col = grdRequestGrid.Columns[0];
                    grdRequestGrid.Sort(col, ListSortDirection.Ascending);

                    int Request_ID = RequestFilesDataManager.createRequest(txtRequestorName.Text, txtReqPurpose.Text, requestDate);
                    List<int> fileIDs = new List<int>();
                    foreach (DataGridViewRow row in grdRequestGrid.Rows)
                        if (!row.IsNewRow)
                            fileIDs.Add((int)row.Cells[4].Value);
                    RequestFilesDataManager.createRequestDetails(Request_ID, fileIDs);
                    //-- generate pdf Request.
                    generateRequestPDF(grdRequestGrid, Request_ID.ToString(), requestDate);
                }
                else
                    MessageBox.Show("Please fill in all information");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving request in database due to " + ex.Message);
            }
            finally
            {
                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
                btnGenRequest.Enabled = true;
            }
        }

        private void btnSearchReq_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReqFile.Text != "" && cmbxReqFileType.SelectedIndex != -1)
                {
                    String Fno = Helpers.validateFileNumber(txtReqFile.Text, (String)cmbxReqFileType.SelectedItem);
                    List<FileRequest> reqs = FileDocCensusDataManager.getRequestByFile(Fno);
                    grdRequestResults.DataSource = null;
                    grdRequestResults.Rows.Clear();
                    grdRequestResults.DataSource = reqs;
                }
                else
                    MessageBox.Show("Invalid File Number!");
                txtRequestFNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region File and Doc Census

        private void btnSearchCen_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtCenFile.Text != "" && cmbxReqFileType1.SelectedIndex != -1)
                {
                    String Fno = Helpers.validateFileNumber(txtCenFile.Text, (String)cmbxReqFileType1.SelectedItem);
                    List<CensusResult> reqs = FileDocCensusDataManager.getCensusByFile(Fno);
                    grdCensusResults.DataSource = null;
                    grdCensusResults.Rows.Clear();
                    grdCensusResults.DataSource = reqs.Select(t => new { Location = t.census_location_description, Date = t.census_date }).ToList();
                }
                else
                    MessageBox.Show("Invalid File Number!");
                txtRequestFNo.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnQRCensus_Click(object sender, EventArgs e)
        {
            var location = grpBoxKeyBoard.PointToScreen(System.Drawing.Point.Empty);
            using (frmQRCapture qf = new frmQRCapture(false, true))
            {
                qf.StartPosition = FormStartPosition.Manual;
                qf.Location = location;
                qf.Owner = this;
                qf.ShowDialog();
            }
        }

        private void btnGetReportCensus_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.UseWaitCursor = true;
                frmCensusReport cr = new frmCensusReport(cmbxLocationCensus.SelectedValue.ToString(), dtCensus.Value);
                cr.Show();
            }
            catch (Exception ex)
            {
                this.Enabled = true;
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                this.UseWaitCursor = false;
                this.Cursor = Cursors.Default;
            }
        }
        #endregion
      

        private void picBoxSwitchTheme_Click(object sender, EventArgs e)
        {
            int count = Properties.Settings.Default.Theme;
            if (count > 0 && count <= 9)
                Properties.Settings.Default.Theme = ++count;
            else
                Properties.Settings.Default.Theme = 1;
            Properties.Settings.Default.Save();
            ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);
        }

        public void loadPlaceholder()
        {
            txtSearchCriterion.Text = "KDL 123*";
            txtSearchCriterion.ForeColor = System.Drawing.Color.Gray;
            txtSearchCriterion.TextAlign = HorizontalAlignment.Center;
        }

        private void txtSearchCriterion_Enter(object sender, EventArgs e)
        {
            if (txtSearchCriterion.Text == "KDL 123*")
            {
                txtSearchCriterion.Text = "";
                txtSearchCriterion.ForeColor = System.Drawing.Color.Black;
                txtSearchCriterion.TextAlign = HorizontalAlignment.Left;
            }

        }

        private void txtSearchCriterion_Leave(object sender, EventArgs e)
        {
            if (txtSearchCriterion.Text == "")
                loadPlaceholder();
        }
        
        private void txtSearchCriterion_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtSearchCriterion.Text.Length >= 8)
            {
                Match match = Regex.Match(txtSearchCriterion.Text, @"^[A-Z]{2,3}\s\d{4,6}$", RegexOptions.IgnoreCase);
                if (match.Success)
                    this.BeginInvoke(new Action(() => btnSearchClick(null, null)));
                else
                    MessageBox.Show("Ensure your file format is valid", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
