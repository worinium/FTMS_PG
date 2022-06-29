using System;
using System.Collections.Generic;
using System.Linq;
using FTMS.Common.DTOs;
using FTMS.DAL;
using System.Data;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.Objects;

namespace FTMS.Common
{
    /// <summary>
    /// Function which takes a file number and file location (optional) search criteria and returns it to the Search Tab or Reports tab
    /// Gets a lot of data so it can be split into two, one for each of the search and the report.
    /// </summary>
    /// <param name="file_number"></param>
    /// <param name="file_location"></param>
    /// <returns>File results is a datatype which combines a File and the Last transaction on it.</returns>
    public class SearchRportDataManager
    {
        public static List<FileResults> getFileByCriteria(String fileNo, String altFileNo, String regisNo, String ownerName, String location, DateTime dtFrom, DateTime dtTo, DateTime dtCFrom, DateTime dtCTo)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var flResults = context.ftms_file.Select(record => new FileResults
                    {
                        file_id = record.file_id,
                        transaction_id = record.current_transaction.Value,
                        file_no = record.file_number,
                        owner_name = record.owner_name,
                        app_date = record.app_date,
                        current_location = record.ftms_transaction.ftms_locations1.description,
                        previous_location = record.ftms_transaction.ftms_locations.description,
                        to_location_mrcode = record.ftms_transaction.to_location,
                        file_condition_mrcode = record.ftms_transaction.file_condition,
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
                        tracking_remark = record.ftms_transaction.tracking_remark,
                        pc_name = record.ftms_transaction.pc_name,
                        logged_user = record.ftms_transaction.logged_user,
                        create_date = record.created_date
                    });
                    if (fileNo != "")
                        flResults = flResults.Where(c => c.file_no.Contains(fileNo));
                    if (altFileNo != "")
                        flResults = flResults.Where(c => c.file_alias.ToLower().Contains(altFileNo.ToLower()));
                    if (regisNo != "")
                        flResults = flResults.Where(c => c.batchNo == regisNo);
                    if (ownerName != "")
                        flResults = flResults.Where(c => c.owner_name.ToLower().Contains(ownerName.ToLower()));
                    if (location != "")
                        flResults = flResults.Where(c => c.to_location_mrcode == location);
                    if (dtFrom > new DateTime(1900, 1, 1))
                        flResults = flResults.Where(c => c.transaction_date >= dtFrom);
                    if (dtTo < new DateTime(3000, 1, 1))
                        flResults = flResults.Where(c => c.transaction_date <= dtTo);
                    if (dtCFrom > new DateTime(1900, 1, 1))
                        flResults = flResults.Where(c => c.create_date.HasValue && c.create_date >= dtCFrom);
                    if (dtCTo < new DateTime(3000, 1, 1))
                        flResults = flResults.Where(c => c.create_date.HasValue && c.create_date <= dtCTo);

                    return flResults.OrderBy(c => c.file_no).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SearchRportDataManager.SearchFiles: {0}", ex.Message));
            }
        }

        public static List<FileResults> getTransactionsByCriteria(String fileNo, String altFileNo, String regisNo, String ownerName, String location, DateTime dtFrom, DateTime dtTo, DateTime dtCFrom, DateTime dtCTo)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var flResults = context.ftms_transaction.Select(record => new FileResults
                    {
                        file_id = record.file_id,
                        transaction_id = record.transaction_id,
                        file_no = record.ftms_file1.file_number,
                        owner_name = record.ftms_file1.owner_name,
                        app_date = record.ftms_file1.app_date,
                        current_location = record.ftms_locations1.description,
                        previous_location = record.ftms_locations.description,
                        to_location_mrcode = record.to_location,
                        file_condition_mrcode = record.file_condition,
                        current_condition = record.lkp_file_condition.description,
                        num_of_pages = record.number_page,
                        transaction_date = record.transaction_date,
                        lga_code = record.ftms_file1.lga_code,
                        rofo_date = record.ftms_file1.rofo_date,
                        commencement_date = record.ftms_file1.commencement_date,
                        remark = record.ftms_file1.remark,
                        phone_number = record.ftms_file1.phone_number,
                        file_alias = record.ftms_file1.file_alias,
                        batchNo = record.ftms_file1.register_number,
                        rackNo = record.ftms_file1.rack_number,
                        tracking_remark = record.tracking_remark,
                        pc_name = record.pc_name,
                        logged_user = record.logged_user,
                        create_date = record.ftms_file1.created_date
                    });
                    if (fileNo != "")
                        flResults = flResults.Where(c => c.file_no.Contains(fileNo));
                    if (altFileNo != "")
                        flResults = flResults.Where(c => c.file_alias.ToLower().Contains(altFileNo.ToLower()));
                    if (regisNo != "")
                        flResults = flResults.Where(c => c.batchNo == regisNo);
                    if (ownerName != "")
                        flResults = flResults.Where(c => c.owner_name.ToLower().Contains(ownerName.ToLower()));
                    if (location != "")
                        flResults = flResults.Where(c => c.to_location_mrcode == location);
                    if (dtFrom > new DateTime(1900, 1, 1))
                        flResults = flResults.Where(c => c.transaction_date >= dtFrom);
                    if (dtTo < new DateTime(3000, 1, 1))
                        flResults = flResults.Where(c => c.transaction_date <= dtTo);
                    if (dtCFrom > new DateTime(1900, 1, 1))
                        flResults = flResults.Where(c => c.create_date >= dtCFrom);
                    if (dtCTo < new DateTime(3000, 1, 1))
                        flResults = flResults.Where(c => c.create_date <= dtCTo);

                    return flResults.OrderBy(c => c.file_no).ThenBy(c => c.transaction_date).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SearchRportDataManager.getTransactionsByCriteria: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Fetches all transactions made on a specific file in a CSV String
        /// Only called when search results yield one file and the return type is set to transactions
        /// </summary>
        /// <param name="fileID"></param>
        /// <returns></returns>
        public static String getCSVFileTransactions(int fileID)
        {
            IQueryable<ftms_transaction> allTransactions;
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    allTransactions = context.ftms_transaction.Include("ftms_locations").Include("ftms_locations1").Where(t => t.file_id == fileID).OrderBy(t => t.transaction_date);
                    //allTransactions = context.ftms_transaction.Include("ftms_locations").OrderByDescending(t => t.transaction_date);

                    //7 Columns are created in the datatable representing a transaction
                    String[] st = new String[8] { "Previous Location", "New Location", "Transaction Date", "File Condition", "Number of Pages", "User", "PC Name", "Tracking Remark" };
                    String CSVString = String.Empty;
                    for (int i = 0; i < 8; i++)
                    {
                        CSVString += "\"" + st[i] + "\",";
                    }
                    CSVString = CSVString.Substring(0, CSVString.Length - 1);
                    CSVString += "\n";
                    foreach (ftms_transaction transaction in allTransactions)
                    {
                        CSVString += "\"" + transaction.ftms_locations.description + "\",";
                        CSVString += "\"" + transaction.ftms_locations1.description + "\",";
                        CSVString += "\"" + transaction.transaction_date.ToString() + "\",";
                        CSVString += "\"" + transaction.file_condition + "\",";
                        CSVString += "\"" + transaction.number_page + "\",";
                        CSVString += "\"" + transaction.logged_user + "\",";
                        CSVString += "\"" + transaction.pc_name + "\",";
                        CSVString += "\"" + transaction.tracking_remark + "\"";
                        CSVString += "\n";
                    }

                    return CSVString;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("SearchRportDataManager.getCSVFileTransactions: {0}", ex.Message));
            }
        }
    }
}