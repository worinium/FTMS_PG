using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using FTMS.Common;
using FTMS.Common.DTOs;
using System.Drawing;
using FTMS_PG.GUI;
using MaterialSkin.Controls;

namespace FTMS_PG
{
    public partial class frmQRCapture : Form
    {
        private Boolean checkIntoArchive;
        String inputU = "";
        private Timer timer = new Timer();
        private Boolean saveSticker = false;

        public frmQRCapture(Boolean archiveCheckin = false, Boolean census = false)
        {
            InitializeComponent();
            // Giving a Customized Skin to the Form
            //ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);

            saveSticker = Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.StickerStatusSetting));
            checkIntoArchive = archiveCheckin;
            register_no.HeaderText = Helpers.getFtmsSetting(Helpers.Constants.BatchNoLabelSetting);
            label1.ForeColor = Color.Black;
            if (archiveCheckin)
            {
                this.Text = "QR Batch checkin";
                var location = ForwardFilesDataManager.getLocation(FTMS_PG.Properties.Settings.Default.CurrentRoomCode);
                currentLocation.Text = location != null ? ForwardFilesDataManager.getLocation(FTMS_PG.Properties.Settings.Default.CurrentRoomCode).description : Helpers.Constants.notConfigured;
                btnQRForward.Text = "Check In";
                currentLocation.Visible = true;
                //btnForwardManual.Visible = false;

                //Setting Visibility of Panel Containing Forward Button true
                panelQRForwardChekin.Visible = true;
                //Setting Visibility of Panel Containing Save Census Button false
                panelCensus.Visible = false;
            }
            else if (census)
            {
                this.Text = "QR Census";
                currentLocation.Visible = false;// DataManager.checkLocCode(LIS_File_Tracking.Properties.Settings.Default.CurrentRoomCode);
                btnQRForward.Text = "Check In";
                btnQRForward.Visible = false;
                btnForwardManual.Visible = false;
                btnQRCensus.Visible = true;
                label1.Text = "Please use the barcode reader on the Census files";
                cmbxCensusLocations.DataSource = new BindingSource(ManageFilesDataManager.getLocations(), null);
                cmbxCensusLocations.DisplayMember = "description";
                cmbxCensusLocations.ValueMember = "mr_code";
                cmbxCensusLocations.SelectedValue = "";

                panelQRForwardChekin.Visible = false;
                panelCensus.Visible = true;
            }
            else
            {
                //btnForwardManual.Visible = true;

                //Setting Visibility of Panel Containing Forward Button true
                panelQRForwardChekin.Visible = true;
                //Setting Visibility of Panel Containing Save Census Button false
                panelCensus.Visible = false;
            }
        }

        private void TimerEventProcessor(object sender, EventArgs myEventArgs)
        {
            String[] res = inputU.Substring(0, inputU.Length).Split('/');
            if (res.Length == 5 && inputU.StartsWith("^^"))
            {
                grdQRFiles.Rows.Add(res[0].Substring(2, res[0].Length - 2), res[1], res[3], res[2]);
            }
            else if (res.Length == 3 && Regex.IsMatch(res[0].Trim(), "^[A-Za-z]* [0-9]{5}$") && Regex.IsMatch(res[2].Trim(), "^[0-9]*$"))
            {
                grdQRFiles.Rows.Add(res[2], res[0].Trim(), "0", res[1]);
            }

            //HOT fix for the idiots who are putting slash in the owner name
            if (res.Length == 6 && inputU.StartsWith("^^"))
            {
                grdQRFiles.Rows.Add(res[0].Substring(2, res[0].Length - 2), res[1], res[4], res[2]);
            }
            else if (res.Length == 3 && Regex.IsMatch(res[0].Trim(), "^[A-Za-z]* [0-9]{5}$") && Regex.IsMatch(res[2].Trim(), "^[0-9]*$"))
            {
                grdQRFiles.Rows.Add(res[2], res[0].Trim(), "0", res[1]);
            }
            inputU = "";
            timer.Tick -= TimerEventProcessor;
            timer.Stop();

            lblResult.Text = "Total Captured Files -> " + grdQRFiles.Rows.Count;
            lblResult.ForeColor = Color.Black;
        }

        private void frmQRCapture_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (inputU == "")
                {
                    timer.Tick += new EventHandler(TimerEventProcessor);
                    timer.Interval = 700;
                    timer.Start();
                }
                inputU += e.KeyChar;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to press the form due to: " + ex.Message);
            }
        }

        private void finishQRBatch(Boolean autoforward)
        {
            try
            {
                if (grdQRFiles.Rows.Count > 0)
                {
                    List<File> lsFile = new List<File>();
                    foreach (DataGridViewRow row in grdQRFiles.Rows)
                    {
                        File fl = new File()
                        {
                            FileID = Convert.ToInt32(row.Cells["file_id"].Value.ToString()),
                            FileNumber = row.Cells["file_no"].Value.ToString().ToUpper(),
                            RegisterNumber = row.Cells["register_no"].Value.ToString().ToUpper(),
                            OwnerName = row.Cells["Owner_Name"].Value.ToString().ToUpper()
                        };
                        lsFile.Add(fl);
                    }

                    if (saveSticker && lsFile.Count > 0)
                        ManageFilesDataManager.saveFloatingFtmsSticker(lsFile);

                    ((frmFileTracking)this.Owner).QRBatch(lsFile.Select(c => c.FileNumber).Distinct().ToList(), checkIntoArchive, autoforward);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("frmQRCapture.finishQRBatch: {0}", ex.Message);
            }
            
        }
        private void btnQRCensus_Click(object sender, MouseEventArgs e)
        {
            try
            {
                if (cmbxCensusLocations.SelectedValue != null)
                {
                    string location = cmbxCensusLocations.SelectedValue.ToString();
                    if (grdQRFiles.Rows.Count > 0)
                    {
                        List<File> lsFile = new List<File>();
                        foreach (DataGridViewRow row in grdQRFiles.Rows)
                        {
                            File fl = new File()
                            {
                                FileID = Convert.ToInt32(row.Cells["file_id"].Value.ToString()),
                                FileNumber = row.Cells["file_no"].Value.ToString().ToUpper(),
                                RegisterNumber = row.Cells["register_no"].Value.ToString().ToUpper(),
                                OwnerName = row.Cells["Owner_Name"].Value.ToString().ToUpper()
                            };
                            lsFile.Add(fl);
                        }
                        if (saveSticker && lsFile.Count > 0)
                            ManageFilesDataManager.saveFloatingFtmsSticker(lsFile);

                        ((frmFileTracking)this.Owner).QRCensus(lsFile.Select(c => c.FileNumber).Distinct().ToList(), location);
                        this.Close();
                    }
                }
                else
                    MessageBox.Show("Please select a location to transfer the file(s) to!");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("frmQRCapture.btnQRCensus_Click: {0}", ex.Message);
            }
            
        }

        private void btnQRForward_Click(object sender, MouseEventArgs e)
        {
            finishQRBatch(true);
        }

        private void btnForwardManual_Click(object sender, MouseEventArgs e)
        {
            finishQRBatch(false);
        }
    }
}