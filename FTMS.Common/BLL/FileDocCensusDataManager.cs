using System;
using System.Collections.Generic;
using System.Linq;
using FTMS.DAL;
using FTMS.Common.DTOs;

namespace FTMS.Common
{
    public class FileDocCensusDataManager
    {
        public static List<FileRequest> getRequestByFile(string file_number)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var fileRequest = context.request_details.Where(c => c.ftms_file.file_number == file_number).Select(c => new FileRequest
                    {
                        request_id = c.request_id,
                        request_date = c.ftms_file_request.request_date,
                        request_purpose = c.ftms_file_request.request_purpose,
                        requestor_name = c.ftms_file_request.requestor_name
                    }).ToList();
                    return fileRequest;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("FileDocCensusDataManager.getRequestByFile: {0}", ex.Message));
            }
        }

        public static void createCensusDetails(List<string> file_nums, DateTime census_date, string pc_name, string logged_user, string location_code)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    foreach (var fileNo in file_nums)
                    {
                        try
                        {
                            ftms_census census = new ftms_census();
                            census.ftms_file = context.ftms_file.Where(y => y.file_number == fileNo).FirstOrDefault();
                            census.census_date = census_date;
                            census.pc_name = pc_name;
                            census.logged_user = logged_user;
                            census.location_code = location_code;
                            context.ftms_census.Add(census);
                        }
                        catch (Exception) { }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("FileDocCensusDataManager.createCensusDetails: {0}", ex.Message));
            }
        }

        public static List<DTOs.ReportModel> getCensusReport(String location_code, DateTime census_date)
        {
            List<File> fileDataList;
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    DateTime startDate = census_date.Date;
                    DateTime endDate = census_date.AddDays(1).Date;
                    //-- get the Census Data of that Date.
                    var censusFileDataListQ = context.ftms_census.Where(c => c.census_date >= startDate && c.census_date < endDate)
                                                .Select(record => new CensusResult
                                                {
                                                    census_date = record.census_date,
                                                    file_number = record.ftms_file.file_number,
                                                    transaction_date = record.ftms_file.ftms_transaction.transaction_date,
                                                    census_location_description = record.ftms_locations.description,
                                                    current_location_description = record.ftms_file.ftms_transaction.ftms_locations1.description,
                                                    census_location_code = record.location_code,
                                                    current_location_code = record.ftms_file.ftms_transaction.to_location
                                                });

                    if (location_code != "")
                        censusFileDataListQ = censusFileDataListQ.Where(c => c.current_location_code == location_code);

                    var censusFileDataList = censusFileDataListQ.ToList();

                    //-- empty location means get all census data. 
                    var fileData = context.ftms_file.Select(c => c);
                    if (location_code != "")
                        fileData = fileData.Where(c => c.ftms_transaction.to_location == location_code);

                    fileDataList = fileData.Select(c => new File
                    {
                        FileNumber = c.file_number,
                        CurrentLocationDesc = c.ftms_transaction.ftms_locations1.description,
                        CurrentTransactionDate = c.ftms_transaction.transaction_date,
                        CurrentLocationCode = c.ftms_transaction.to_location
                    }).ToList();

                    //-- files which exist in the selected location.
                    var correct_files = censusFileDataList.Where(t => t.current_location_code == t.census_location_code).Select(y => new
                    {
                        y.file_number,
                        y.transaction_date,
                        y.current_location_description
                    }).ToList();

                    //-- files which do not forward to the selected location.
                    var not_tracked_files = censusFileDataList.Where(t => t.current_location_code != t.census_location_code).Select(y => new
                    {
                        y.file_number,
                        y.transaction_date,
                        y.current_location_description
                    }).ToList();

                    //-- files which do not exist in the selected location.
                    var wrong_ftms_files = fileDataList.Where(p => !correct_files.Any(p1 => p1.file_number == p.FileNumber)).Select(y => new
                    {
                        file_no = y.FileNumber,
                        y.CurrentTransactionDate,
                        current_location_description = y.CurrentLocationDesc
                    }).ToList();

                    int maxRowNum = Math.Max(correct_files.Count, Math.Max(fileDataList.Count, wrong_ftms_files.Count));
                    int correctFileCnt = correct_files.Count;
                    int notTrackedCnt = not_tracked_files.Count;
                    int wrongFTMSCnt = wrong_ftms_files.Count;
                    List<DTOs.ReportModel> lsReportModel = new List<DTOs.ReportModel>();
                    for (int i = 0; i < maxRowNum; i++)
                    {
                        DTOs.ReportModel rpModel = new DTOs.ReportModel();
                        if (i < correctFileCnt)
                        {
                            rpModel.column_1 = correct_files[i].file_number;
                            rpModel.column_2 = correct_files[i].transaction_date.ToString();
                            rpModel.column_3 = correct_files[i].current_location_description;
                        }

                        if (i < notTrackedCnt)
                        {
                            rpModel.column_4 = not_tracked_files[i].file_number;
                            rpModel.column_5 = not_tracked_files[i].transaction_date.ToString();
                            rpModel.column_6 = not_tracked_files[i].current_location_description;
                        }

                        if (i < wrongFTMSCnt)
                        {
                            rpModel.column_7 = wrong_ftms_files[i].file_no;
                            rpModel.column_8 = wrong_ftms_files[i].CurrentTransactionDate.ToString();
                            rpModel.column_9 = wrong_ftms_files[i].current_location_description;
                        }
                        lsReportModel.Add(rpModel);
                    }

                    return lsReportModel;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("FileDocCensusDataManager.getCensusReport: {0}", ex.Message));
            }
        }
        public static List<CensusResult> getCensusByFile(string file_number)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var fileCensus = context.ftms_census.Where(c => c.ftms_file.file_number == file_number).Select(c => new CensusResult
                    {
                        census_location_description = c.ftms_locations.description,
                        census_date = c.census_date
                    }).ToList();
                    return fileCensus;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("FileDocCensusDataManager.getRequestByFile: {0}", ex.Message));
            }
        }
    }
}