using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FTMS.Common;
using FTMS.DAL;
using FTMS.Common.DTOs;
using System.Data.SqlClient;
using MaterialSkin.Controls;

namespace FTMS_PG.GUI
{
    public partial class frmManageFiles : Form
    {
        public frmManageFiles(int file_id)
        {
            InitializeComponent();
            // Giving a Customized Skin to the Form
            //ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);
           
            List<FileSticker> res = ManageFilesDataManager.getFileStickers(file_id);
            
            grdStickersResults.Columns[3].HeaderCell.Value = Helpers.getFtmsSetting(Helpers.Constants.BatchNoLabelSetting);

            int rowNo = 0;
            foreach (var item in res)
            {
                string[] qr_fulltext_split = item.qr_label_txt.Substring(2, (item.qr_label_txt.Length - 5)).Split('/');

                grdStickersResults.Rows.Add();
                grdStickersResults.Rows[rowNo].Cells[0].Value = qr_fulltext_split[0];
                grdStickersResults.Rows[rowNo].Cells[1].Value = qr_fulltext_split[1];
                grdStickersResults.Rows[rowNo].Cells[2].Value = qr_fulltext_split[2];
                grdStickersResults.Rows[rowNo].Cells[3].Value = qr_fulltext_split[3];
                grdStickersResults.Rows[rowNo].Cells[4].Value = item.created_date;
                grdStickersResults.Rows[rowNo].Cells[5].Value = item.logged_user;
                grdStickersResults.Rows[rowNo].Cells[6].Value = item.active;
                grdStickersResults.Rows[rowNo].Cells[7].Value = item.owner_name;
                rowNo++;
            }
            grdStickersResults.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            grdStickersResults.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdStickersResults.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            grdStickersResults.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            grdStickersResults.BackgroundColor = SystemColors.ControlDark;

            lblResult.Text = "Total = " + grdStickersResults.Rows.Count;
            lblResult.ForeColor = Color.WhiteSmoke;
        }
    }   
}
