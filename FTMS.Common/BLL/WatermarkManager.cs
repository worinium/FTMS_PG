using FTMS.DAL;
using System.Linq;
using System;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;

namespace FTMS.Common.BLL
{
    public class WatermarkManager
    {
        public static string GetWaterMark()
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    //-- check the database environment
                    var dbEnv = context.settings.Where(c => c.mr_code == "dbEnv").FirstOrDefault();
                    switch (dbEnv.value)
                    {
                        case "training":
                            return "TRAINING";
                        case "dev":
                            return "DEVELOPMENT";
                        case "live":
                            break;
                    }
                    return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DocumentManager.GetWaterMark:: {0}", ex.Message) + ex.InnerException == null ? "" : ex.InnerException.Message);
            }
        }
        public static void AddWaterMark(string inputPath, string outputPath, string watermark)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    bool portrait = true;
                    PdfReader PDFReader = new PdfReader(inputPath);

                    FileStream Stream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);

                    PdfStamper PDFStamper = new PdfStamper(PDFReader, Stream);
                    //-- check the paper type.
                    if (Math.Ceiling(PDFReader.GetPageSize(1).Width) == PageSize.A4.Height)
                        portrait = false;

                    iTextSharp.text.Rectangle pageRectangle = PDFReader.GetPageSizeWithRotation(1);

                    var textSize = Convert.ToInt64(context.settings.Where(c => c.mr_code == "waterMarkSize").FirstOrDefault() != null ? context.settings.Where(c => c.mr_code == "waterMarkSize").FirstOrDefault().value : "150");


                    var X = Convert.ToInt64(Math.Ceiling(Math.Cos(35 * (Math.PI / 180)) * ((textSize * 35) / 100)));
                    var Y = Convert.ToInt64(Math.Ceiling(Math.Sin(35 * (Math.PI / 180)) * ((textSize * 35) / 100)));

                    PdfGState graphicsStateWM = new PdfGState();
                    graphicsStateWM.FillOpacity = 0.5F;

                    PdfGState graphicsStateFooter = new PdfGState();
                    graphicsStateFooter.FillOpacity = 5F;

                    for (int iCount = 0; iCount < PDFStamper.Reader.NumberOfPages; iCount++)
                    {
                        PdfContentByte PDFData = PDFStamper.GetOverContent(iCount + 1);
                        //-- set the opacity of the watermark text.
                        PDFData.SetGState(graphicsStateWM);
                        //-- create font style.
                        BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.WINANSI, BaseFont.EMBEDDED);
                        PDFData.BeginText();
                        //-- set the color of watermark.
                        PDFData.SetColorFill(CMYKColor.LIGHT_GRAY);
                        // set the font and size of watermark.
                        PDFData.SetFontAndSize(baseFont, textSize);
                        PDFData.ShowTextAligned(PdfContentByte.ALIGN_CENTER, watermark, pageRectangle.Width / 2 + (portrait ? Y : X), pageRectangle.Height / 2 - (portrait ? Y : X), portrait ? 55 : 35);
                        //-- add footer text. 
                        PDFData.SetGState(graphicsStateFooter);
                        PDFData.SetColorFill(CMYKColor.RED);
                        PDFData.SetFontAndSize(baseFont, 10);
                        PDFData.EndText();
                    }
                    PDFStamper.Close();
                    PDFReader.Close();
                    Stream.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DocumentManager.AddWaterMark:: {0}", ex.Message) + ex.InnerException == null ? "" : ex.InnerException.Message);
            }

        }
    }
}
