using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTMS.DAL;
using FTMS.Common;
using MaterialSkin.Controls;

namespace FTMS_PG
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
            // Giving a Customized Skin to the Form
            //ApplicationLookAndFeel.UseMaterialSkin(this, Properties.Settings.Default.Theme);
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            /*
            // TODO: This line of code loads data into the 'fTS_DataBaseDataSet.LGAs' table. You can move, or remove it, as needed.
            this.lGAsTableAdapter.Fill(this.fTS_DataBaseDataSet.LGAs);
            // TODO: This line of code loads data into the 'fTS_DataBaseDataSet.file_type' table. You can move, or remove it, as needed.
            this.file_typeTableAdapter.Fill(this.fTS_DataBaseDataSet.file_type);
            // TODO: This line of code loads data into the 'fTS_DataBaseDataSet.file_cond_lkp' table. You can move, or remove it, as needed.
            this.file_cond_lkpTableAdapter.Fill(this.fTS_DataBaseDataSet.file_cond_lkp);
            // TODO: This line of code loads data into the 'fTS_DataBaseDataSet.Locations' table. You can move, or remove it, as needed.
            this.locationsTableAdapter.Fill(this.fTS_DataBaseDataSet.Locations);
             */
            using (ftmsdbEntities context = new ftmsdbEntities())
            {
                this.ftmslocationsBindingSource.DataSource = context.ftms_locations.ToList();
                this.lkpfileconditionBindingSource.DataSource = context.lkp_file_condition.ToList();
            }
        }

        private void Save_Config_Loc(object sender, EventArgs e)
        {
            try
            {
                /*
                this.locationsBindingSource.EndEdit();
                this.locationsTableAdapter.Update(this.fTS_DataBaseDataSet);
                MessageBox.Show("Saved");
                ((frmFileTracking)this.Owner).refreshDatabind();
                this.Close();
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save locations due to: " + ex.Message);
            }
        }

        private void Save_Config_Types(object sender, EventArgs e)
        {
            try
            {
                /*
                this.filetypeBindingSource.EndEdit();
                this.file_typeTableAdapter.Update(this.fTS_DataBaseDataSet);
                MessageBox.Show("Saved");
                ((frmFileTracking)this.Owner).refreshDatabind();
                this.Close();
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save types due to: " + ex.Message);
            }
        }

        private void Save_Config_Conds(object sender, EventArgs e)
        {
            try
            {
                /*
                this.filecondlkpBindingSource.EndEdit();
                this.file_cond_lkpTableAdapter.Update(this.fTS_DataBaseDataSet);
                MessageBox.Show("Saved");
                ((frmFileTracking)this.Owner).refreshDatabind();
                this.Close();
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save conditions due to: " + ex.Message);
            }
        }

        private void saveLGA_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                this.lGAsBindingSource.EndEdit();
                this.lGAsTableAdapter.Update(this.fTS_DataBaseDataSet);
                MessageBox.Show("Saved");
                ((frmFileTracking)this.Owner).refreshDatabind();
                this.Close();
                 */
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save LGA due to: " + ex.Message);
            }
        }
    }
}
