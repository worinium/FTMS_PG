using System;
using System.Collections.Generic;
using System.Linq;
using FTMS.DAL;
using FTMS.Common.DTOs;

namespace FTMS.Common
{
    public class RequestFilesDataManager
    {
        public static File getFileByNumber(string file_number)
        {
            try
            {
                File file;
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    file = context.ftms_file.Where(f => f.file_number == file_number)
                                            .Select(record => new File
                                             {
                                                 FileID = record.file_id,
                                                 FileNumber = record.file_number,
                                                 CurrentLocationDesc = record.ftms_transaction.ftms_locations1.description,
                                                 CurrentTransactionDate = record.ftms_transaction.transaction_date,
                                                 RegisterNumber = record.register_number,
                                                 RackNumber = record.rack_number
                                             }).FirstOrDefault();
                    return file;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("RequestFilesDataManager.getFileByNumber: {0}", ex.Message));
            }
        }

        public static int createRequest(String requestor, String requestPurpose, DateTime requestDate)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    ftms_file_request fileRequest = new ftms_file_request();
                    fileRequest.requestor_name = requestor;
                    fileRequest.request_purpose = requestPurpose;
                    fileRequest.request_date = requestDate;
                    fileRequest.requestor_pcname = System.Environment.MachineName;
                    fileRequest.requestor_logged_user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    context.ftms_file_request.Add(fileRequest);
                    context.SaveChanges();
                    int requestID = fileRequest.request_id;
                    return requestID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("RequestFilesDataManager.createRequest: {0}", ex.Message));
            }
        }

        public static void createRequestDetails(int requestID, List<int> fileIDs)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    foreach (var fileID in fileIDs)
                    {
                        request_details requestDetails = new request_details();
                        requestDetails.request_id = requestID;
                        requestDetails.file_id = fileID;
                        context.request_details.Add(requestDetails);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("RequestFilesDataManager.createRequestDetails: {0}", ex.Message));
            }
        }
    }
}
