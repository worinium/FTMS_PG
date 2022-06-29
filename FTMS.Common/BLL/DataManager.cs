using System;
using System.Collections.Generic;
using System.Linq;
using FTMS.Common.BusinessObjects;
using FTMS_PG;
using FTMS.DAL;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.Objects;

namespace FTMS.Common
{
    public class DataManager
    {                       
        public static List<Model.ReportModel> getCensusReport(String location_code, DateTime census_date)
        {
            return null;
            /*
            //Get the Census Data of that Date.
            //empty location means get all census data.
            CensusTableAdapter ca = new CensusTableAdapter();
            var census_file_data = ca.GetCensusByDate(census_date.Date, census_date.AddDays(1)).AsEnumerable().Where(p => location_code != "" ? p.Field<string>("CensusLocationCode") == location_code : true).Select(m => new CensusResult
                        {
                            census_date = m.Field<DateTime>("census_date").ToString("dd-MMM-yyyy"),
                            //census_location_code = m.Field<int>("location_code"),
                            file_no = m.Field<string>("file_no"),
                            transaction_date = m.Field<DateTime>("transa_date").ToString("dd-MMM-yyyy"),
                            current_location_code = m.Field<string>("FileLocationCode"),
                            census_location_description = m.Field<string>("CensusLocation"),
                            census_location_code = m.Field<string>("CensusLocationCode"),
                            current_location_description = m.Field<string>("FileLoc")
                        }).ToList();

            //Get the file current locations according to FTMS
            FileTableAdapter fa = new FileTableAdapter();
            List<File> ftms_file_data = new List<File>();
            if (location_code != "")
            {
                ftms_file_data = fa.GetFilesByLocation(location_code).AsEnumerable().Select(m => new File
                        {
                            file_number = m.Field<string>("file_no"),
                            current_location = m.Field<string>("CurrentLocation"),
                            transaction_date = m.Field<DateTime>("transa_date").ToString("dd-MMM-yyyy"),
                            current_location_code = m.Field<string>("curr_location")
                        }).ToList(); ;
            }
            else
            {
                ftms_file_data = fa.GetFileLocations().AsEnumerable().Select(m => new File
                {
                    file_number = m.Field<string>("file_no"),
                    current_location = m.Field<string>("CurrentLocation"),
                    transaction_date = m.Field<DateTime>("transa_date").ToString("dd-MMM-yyyy"),
                    current_location_code = m.Field<string>("curr_location")
                }).ToList();
            }

            var correct_files = census_file_data.Where(t => t.current_location_code == t.census_location_code).Select(y => new { y.file_no, y.transaction_date, y.current_location_description }).ToList();
            //var correct_file_numbers = correct_files.Select(y => y.file_no).ToList();
            var not_tracked_files = census_file_data.Where(t => t.current_location_code != t.census_location_code).Select(y => new { y.file_no, y.transaction_date, y.current_location_description }).ToList();
            var wrong_ftms_files = ftms_file_data.Where(p => !correct_files.Any(p1 => p1.file_no == p.file_number)).Select(y => new { file_no = y.file_number, y.transaction_date, current_location_description = y.current_location }).ToList();

            int maxRowNum = Math.Max(correct_files.Count, Math.Max(ftms_file_data.Count, wrong_ftms_files.Count));
            int corrFileCnt = correct_files.Count;
            int notTrackCnt = not_tracked_files.Count;
            int wrongFTMSCnt = wrong_ftms_files.Count;
            List<Model.ReportModel> lsReportModel = new List<Model.ReportModel>();
            for (int i = 0; i < maxRowNum; i++)
            {
                Model.ReportModel rpModel = new Model.ReportModel();
                if (i < corrFileCnt)
                {
                    rpModel.column_1 = correct_files[i].file_no;
                    rpModel.column_2 = correct_files[i].transaction_date.ToString();
                    rpModel.column_3 = correct_files[i].current_location_description;
                }

                if (i < notTrackCnt)
                {
                    rpModel.column_4 = not_tracked_files[i].file_no;
                    rpModel.column_5 = not_tracked_files[i].transaction_date.ToString();
                    rpModel.column_6 = not_tracked_files[i].current_location_description;
                }

                if (i < wrongFTMSCnt)
                {
                    rpModel.column_7 = wrong_ftms_files[i].file_no;
                    rpModel.column_8 = wrong_ftms_files[i].transaction_date.ToString();
                    rpModel.column_9 = wrong_ftms_files[i].current_location_description;
                }
                lsReportModel.Add(rpModel);
            }

            return lsReportModel;
             */
        }                                            
        
        public static void createCensusDetails(List<int> fileIDs, DateTime census_date, string pc_name, string logged_user, string location_code)
        {
            /*
            try
            {
                CensusTableAdapter cadapter = new CensusTableAdapter();
                foreach (var fileID in fileIDs)
                    cadapter.Insert(census_date, location_code, fileID, logged_user, pc_name);

            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DataManager.createCensusDetails: {0}", ex.Message));
            }
             * */
        }

        public static List<FileRequest> getRequestByFile(string file_number)
        {
            return null;
            /*
            try
            {
                file_request_detailsTableAdapter fdadapter = new file_request_detailsTableAdapter();
                var f = fdadapter.GetRequestByFileNo(file_number).AsEnumerable().ToList();
                List<FileRequest> requests = fdadapter.GetRequestByFileNo(file_number).AsEnumerable().Select(m => new FileRequest
                {
                    request_id = m.Field<int>("request_id").ToString(),
                    request_purpose = m.Field<string>("request_purpose"),
                    request_date = m.Field<DateTime>("request_date").ToString("dd-MMM-yyyy"),
                    requestor_name = m.Field<string>("requestor_name")
                }).ToList();

                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("DataManager.getRequestByFile: {0}", ex.Message));
            }
             */
        }
    }
}
