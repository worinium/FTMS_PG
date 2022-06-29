using System;
using System.Linq;
using FTMS.Common;
using System.Windows.Forms;
using MaterialSkin.Controls;


namespace FTMS_PG
{
    public partial class frmCensusReport : Form
    {
        public frmCensusReport(string location, DateTime census_date)
        {
            InitializeComponent();
            // Giving a Customized Skin to the Form
            //ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);
            censusReport1.Load();
            censusReport1.SetDataSource(FileDocCensusDataManager.getCensusReport(location, census_date));
            var locField = censusReport1.ReportDefinition.Sections["Section4"].ReportObjects.OfType<CrystalDecisions.CrystalReports.Engine.TextObject>().Where(p => p.Name == "lbl_location").FirstOrDefault();
            var censusDateField = censusReport1.ReportDefinition.Sections["Section4"].ReportObjects.OfType<CrystalDecisions.CrystalReports.Engine.TextObject>().Where(p => p.Name == "lbl_census_date").FirstOrDefault();
            locField.Text = location == "" ? "All" : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(ForwardFilesDataManager.getLocation(location).description.ToLower());
            censusDateField.Text = census_date.ToString();

            rprtViewer.ReportSource = censusReport1;
            rprtViewer.Refresh();
        }
    }
}
