using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using QRCoder;
using FTMS.DAL;
using FTMS.Common;
using System.Data.Entity;
using System.Linq;
using MaterialSkin.Controls;

namespace FTMS_PG
{
    public partial class frmReport : Form
    {
        DYMO.Label.Framework.ILabel finalLabel;
        string Full_text = "";
        string File_number = "";
        public frmReport(FTMS.Common.DTOs.File newfile)
        {
            InitializeComponent();
            // Giving a Customized Skin to the Form
            //ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);
            File_number = newfile.FileNumber;
            constructQRLabel(newfile);
        }
        
        private void constructQRLabel(FTMS.Common.DTOs.File newfile)
        {
            QRCodeGenerator.ECCLevel eccLevel = QRCodeGenerator.ECCLevel.H;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            Full_text = "^^" + newfile.FileID + "/" + newfile.FileNumber + "/" + newfile.OwnerName + "/" + newfile.RegisterNumber + "/" + Helpers.getFtmsSetting(Helpers.Constants.AppSignatureSetting) + "^^\n";
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Full_text, eccLevel);
            Bitmap bm = new QRCode(qrCodeData).GetGraphic(4, Color.Black, Color.White, null, 30);

            //TODO this logic will bug out if the file number doesnt have a space in the config
            String register = newfile.FileNumber.Substring(0, newfile.FileNumber.LastIndexOf(' ')) + " " + Helpers.getFtmsSetting(Helpers.Constants.RegisterTextSetting) + newfile.RegisterNumber;
            String rack = Helpers.getFtmsSetting(Helpers.Constants.RackTextSetting) + newfile.RackNumber;
            String fileNo = newfile.FileNumber;
            string version = Helpers.getPublicSetting(Helpers.Constants.waterMarkerSetting);
            string _watermark = string.Empty;
            using (FileStream fs = new FileStream("ReportsAndQRLabel\\" + Properties.Settings.Default.TemplateName, FileMode.Open))
            {
                DYMO.Label.Framework.ILabel dymoLabel = DYMO.Label.Framework.Label.Open(fs);
                dymoLabel.SetObjectText(Helpers.Constants.FileNoTextboxName, fileNo);
                dymoLabel.SetObjectText(Helpers.Constants.RegisterNoTextboxName, register);
                dymoLabel.SetObjectText(Helpers.Constants.RackNoTextboxName, rack);
                if (Convert.ToBoolean(Helpers.getFtmsSetting(Helpers.Constants.printAliasSetting)))
                    dymoLabel.SetObjectText(Helpers.Constants.FileAliasTextboxName, newfile.FileAlias);
                switch (version.ToLower())
                {
                    case Helpers.Constants.Training: 
                        {
                            _watermark = "TRAINING";
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkTextBox, _watermark);
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkStateTextbox, "");
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkAgencyTextBox, "");
                        } 
                        break;
                    case Helpers.Constants.Development:
                        {
                            _watermark = "DEVELOPMENT";
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkTextBox, _watermark);
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkStateTextbox, "");
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkAgencyTextBox, "");
                        }
                        break;
                    case Helpers.Constants.Live:
                        {
                            dymoLabel.SetObjectText(Helpers.Constants.WatermarkTextBox, "");
                        } 
                        break;
                    default: MessageBox.Show("Unknow Literial Exist on dbEnv value"); break;

                }
                finalLabel = dymoLabel;
                using (Stream QRStream = new MemoryStream((byte[])new ImageConverter().ConvertTo(bm, typeof(byte[]))))
                {
                    dymoLabel.SetImagePngData(Helpers.Constants.QRImageBoxName, QRStream);
                    using (Stream stream = new MemoryStream(dymoLabel.RenderAsPng(null, null)))
                        pictureBox1.Image = new Bitmap(stream);
                }
            }
        }

        private void btnPrintLabel_Click(object sender, EventArgs e)
        {
            try
            {
                ManageFilesDataManager.addFtmsSticker(File_number, Full_text, true);
                finalLabel.Print(FTMS_PG.Properties.Settings.Default.PrinterName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
