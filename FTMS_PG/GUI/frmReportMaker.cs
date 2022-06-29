using System;
using FTMS.Common.BLL;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using FTMS.Common.DTOs;
using FTMS.Common;
using iTextSharp.text.pdf;
using iTextSharp.text;
using MaterialSkin.Controls;


namespace FTMS_PG
{
    public partial class frmReportMaker : Form
    {
        public String fromTxt;
        public String toTxt;
        public BindingList<FileResults> batchFiles;
        private readonly bool _generatePDFReport = FTMS_PG.Properties.Settings.Default.GeneratePDFReports;
        private readonly bool _printReports = FTMS_PG.Properties.Settings.Default.PrintReports;
        private readonly string _reportPath = Helpers.getFtmsSetting(Helpers.Constants.ReportsPathSetting);
        public frmReportMaker(String Status)
        {
            InitializeComponent();
            rchStatus.Text = Status;
            if (!_generatePDFReport || !_printReports)
                btnPrint.Enabled = false;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rchRemark.Text))
            {
                ((frmFileForward)this.Owner).TrackingRemark = rchRemark.Text;
                //TODO right now, if both print and gen pdf are enabled, the pdf comes out upon clicking yes print pdf. All other cases arent handled.
                //TODO find out why forwarding the first time works, creashes the second (Heiko's machine)
                if (_generatePDFReport && _printReports)
                    try
                    {
                        if (!Directory.Exists(Helpers.getFtmsSetting(Helpers.Constants.ReportsPathSetting)))
                            Directory.CreateDirectory(Helpers.getFtmsSetting("ReportsPath"));

                        String fileName = _reportPath + "FTMS_WorkOrder_" + DateTime.Now.ToString("yyMMddHHmmss");
                        using (Document document = new Document())
                        {
                            using (PdfSmartCopy copy = new PdfSmartCopy(document, new FileStream(fileName + ".pdf", FileMode.Create)))
                            {
                                int pages = (batchFiles.Count - 1) / 120 + 1;
                                document.Open();
                                var filess = batchFiles.Select(sl => sl.file_no).ToList();
                                int counter = 0;
                                for (int i = 0; i < pages; i++)
                                {
                                    // replace this with your PDF form template          
                                    PdfReader pdfReader = new PdfReader("ReportsAndQRLabel\\FTMS_Forward_Tracking_Report.pdf");
                                    using (var ms = new MemoryStream())
                                    {
                                        using (PdfStamper stamper = new PdfStamper(pdfReader, ms))
                                        {
                                            AcroFields fields = stamper.AcroFields;
                                            fields.SetField("txtFromLocation", fromTxt);
                                            fields.SetField("txtToLocation", toTxt);
                                            fields.SetField("txtOperatedBy", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
                                            fields.SetField("txtTimeNow", DateTime.Now.ToString("dd-MMM-yyyy HH:mm"));
                                            fields.SetField("txtRemark", rchRemark.Text);
                                            for (int j = 1; j <= 120 && counter < filess.Count; j++)
                                            {
                                                fields.SetField("txtFileNo_" + j, filess[counter]);
                                                counter++;
                                            }

                                            stamper.FormFlattening = true;
                                        }
                                        pdfReader = new PdfReader(ms.ToArray());
                                        copy.AddPage(copy.GetImportedPage(pdfReader, 1));
                                    }
                                    pdfReader.Close();
                                }
                            }
                        }

                        if (_printReports)
                        {
                            try
                            {
                                string watermarktext = WatermarkManager.GetWaterMark();
                                if (watermarktext != "")
                                {
                                    WatermarkManager.AddWaterMark(fileName + ".pdf", fileName + "_t.pdf", watermarktext);
                                    System.IO.File.Delete(fileName + ".pdf");
                                    fileName = fileName + "_t";
                                }
                                System.Diagnostics.Process.Start(fileName + ".pdf");
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Error: Process Could Not Open Generated Report");
                            }
                        }

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error generating the work order");
                    }
                this.Close();
            }
            else MessageBox.Show("Please Enter a Remark.");
        }

        private void btnNoPrint_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rchRemark.Text))
            {
                ((frmFileForward)this.Owner).TrackingRemark = rchRemark.Text;
                this.Close();
            }
            else MessageBox.Show("Please Enter a Remark.");
        }
    }
}