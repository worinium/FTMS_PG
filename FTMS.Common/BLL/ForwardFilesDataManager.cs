using System;
using System.Collections.Generic;
using System.Linq;
using FTMS.Common.DTOs;
using FTMS.DAL;

namespace FTMS.Common
{
    public class ForwardFilesDataManager
    {
        /// <summary>
        /// Function which takes a file number and file location (optional) search criteria and returns it to the Search Tab or Reports tab
        /// Gets a lot of data so it can be split into two, one for each of the search and the report.
        /// </summary>
        /// <param name="file_number"></param>
        /// <param name="file_location"></param>
        /// <returns>File results is a datatype which combines a File and the Last transaction on it.</returns>
        public static List<FileResults> getFiles(String file_number)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var fileResults = context.ftms_file.Where(f => f.file_number.StartsWith(file_number))
                                                   .Select(record => new FileResults
                                                    {
                                                        file_id = record.file_id,
                                                        transaction_id = record.current_transaction.Value,
                                                        file_no = record.file_number,
                                                        owner_name = record.owner_name,
                                                        app_date = record.app_date,
                                                        current_location = record.ftms_transaction.ftms_locations1.description,
                                                        to_location_mrcode = record.ftms_transaction.ftms_locations1.mr_code,
                                                        file_condition_mrcode = record.ftms_transaction.lkp_file_condition.mr_code,
                                                        current_condition = record.ftms_transaction.lkp_file_condition.description,
                                                        num_of_pages = record.ftms_transaction.number_page,
                                                        transaction_date = record.ftms_transaction.transaction_date,
                                                        lga_code = record.lga_code,
                                                        rofo_date = record.rofo_date,
                                                        commencement_date = record.commencement_date,
                                                        remark = record.remark,
                                                        phone_number = record.phone_number,
                                                        file_alias = record.file_alias,
                                                        batchNo = record.register_number,
                                                        rackNo = record.rack_number,
                                                        tracking_remark = record.ftms_transaction.tracking_remark
                                                    }).ToList();

                    return fileResults;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ForwardFilesDataManager.SearchFiles: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Verifies that the CurrentLocation setting in the appconfig is present in database.
        /// </summary>
        /// <param name="mrcode"></param>
        /// <returns></returns>
        public static Location getLocation(String mrcode)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var locs = context.ftms_locations.Where(l => l.mr_code == mrcode).FirstOrDefault();
                    if (locs != null)
                        return new Location() { mr_code = locs.mr_code, description = locs.description, active = locs.active };
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ForwardFilesDataManager.getLocation: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Creates a new transaction for a file
        /// </summary>
        /// <param name="newTransaction"></param>
        public static void addTransactions(List<Transaction> newTransaction)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    foreach (var trans in newTransaction)
                    {
                        ftms_transaction transaction = new ftms_transaction();
                        transaction.transaction_id = trans.TransactionID;
                        transaction.file_id = trans.FileID;
                        transaction.transaction_date = (DateTime)trans.TransactionDate;
                        transaction.number_page = trans.NumOfPages;
                        transaction.logged_user = trans.LoggedUser;
                        transaction.pc_name = trans.PCName;
                        transaction.tracking_remark = trans.TrackingRemark;
                        transaction.from_location = trans.FromLocation;
                        transaction.to_location = trans.ToLocation;
                        transaction.file_condition = trans.FileCondition;
                        context.ftms_transaction.Add(transaction);
                        context.SaveChanges();
                        ftms_file file = context.ftms_file.Where(f => f.file_id == trans.FileID).FirstOrDefault();
                        file.ftms_transaction = transaction;
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ForwardFilesDataManager.SaveTransactions: {0}", ex.Message));
            }

        }

        //public static void SaveFileLocations(List<File> movedFiles)
        //{
        //    try
        //    {
        //        using (FtmsEntities context = new FtmsEntities())
        //        {
        //            foreach (var file in movedFiles)
        //            {
        //                ftms_file existingFile = context.ftms_file.Where(f => f.file_id == file.FileID).FirstOrDefault();                        
        //                existingFile.transaction_date = file.CurrentTransactionDate;
        //                context.SaveChanges();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(string.Format("ForwardFilesDataManager.SaveFileLocations: {0}", ex.Message));
        //    }
        //}
    }
}
