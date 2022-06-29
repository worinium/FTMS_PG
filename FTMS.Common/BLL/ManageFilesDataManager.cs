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
using System.Drawing;

namespace FTMS.Common
{
    public class ManageFilesDataManager
    {
        /// <summary>
        /// Gets Locations for use in the application
        /// </summary>
        /// <param name="getActive">sets whether to get the active only or the full list of locations</param>
        /// <returns></returns>
        public static List<Location> getLocations(Boolean getActive = true)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var locations = getActive ? context.ftms_locations.Where(l => l.active).OrderBy(l => l.positions) : context.ftms_locations.OrderBy(l => l.positions);
                    return locations.Select(c => new Location() { mr_code = c.mr_code, description = c.description, active = c.active }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.getLocations: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Fetches the list of file conditions
        /// </summary>
        /// <returns></returns>
        public static List<FileCondition> getFileConditions()
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var flConditions = context.lkp_file_condition.Where(c => c.active).OrderBy(c => c.positions);
                    //string sqlStatement = (conditions as ObjectQuery).ToTraceString();
                    return flConditions.Select(c => new FileCondition() { mr_code = c.mr_code, description = c.description, active = c.active }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.getFileConditions: {0}", ex.Message));
            }
        }

        /// <summary>
        /// Inserts a new file into the file table
        /// add new transaction if the file is new. 
        /// </summary>
        /// <param name="file"></param>
        public static int CreateEditFile(File file)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    Boolean createNew = false;
                    ftms_file currentFile = context.ftms_file.Include("location_type_upper").Include("ftms_transaction").Where(f => f.file_id == file.FileID).FirstOrDefault();
                    ftms_transaction currentTransaction = null;
                    if (currentFile == null)
                    {
                        createNew = true;
                        currentFile = new ftms_file();
                        currentFile.created_by = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        currentFile.created_date = DateTime.Now;
                        currentFile.modified_by = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        currentFile.modified_date = DateTime.Now;
                    }
                    else
                    {
                        if (currentFile.created_date == null)
                        {
                            currentFile.created_by = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                            currentFile.created_date = DateTime.Now;
                        }
                        currentTransaction = currentFile.ftms_transaction;
                        currentFile.modified_by = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        currentFile.modified_date = DateTime.Now;
                    }
                    currentFile.file_number = file.FileNumber;
                    currentFile.owner_name = file.OwnerName;
                    currentFile.cofo_exist = file.CofOExists;
                    currentFile.rofo_exist = file.RofOExists;
                    currentFile.phone_number = file.PhoneNumber;
                    currentFile.app_date = file.ApplicationDate;
                    currentFile.remark = file.Remark;
                    currentFile.register_number = file.RegisterNumber;
                    currentFile.rofo_date = file.RofODate;
                    currentFile.commencement_date = file.CommencementDate;
                    currentFile.file_alias = file.FileAlias;
                    currentFile.location_type_upper = context.location_type_upper.Where(t => t.mr_code == file.LGACode).FirstOrDefault();
                    currentFile.rack_number = file.RackNumber;
                    if (createNew)
                    {
                        context.ftms_file.Add(currentFile);
                    }
                    context.SaveChanges();

                    try
                    {
                        if (createNew)
                        {
                            currentTransaction = new ftms_transaction();
                            currentTransaction.ftms_locations = context.ftms_locations.Where(o => o.mr_code == Helpers.Constants.unknownLocationCode).FirstOrDefault();
                            currentTransaction.ftms_locations1 = context.ftms_locations.Where(o => o.mr_code == file.CurrentLocationCode).FirstOrDefault();
                            currentTransaction.transaction_date = DateTime.Now;
                            currentTransaction.logged_user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                            currentTransaction.pc_name = System.Environment.MachineName;
                            currentTransaction.ftms_file1 = currentFile;
                            currentTransaction.tracking_remark = "";
                            context.ftms_transaction.Add(currentTransaction);
                        }
                        //Current Transaction attributes
                        currentTransaction.file_condition = file.CurrentFileConditionCode;
                        currentTransaction.number_page = file.CurrentNumOfPages;
                        //Link the file to the last transaction
                        currentFile.ftms_transaction = currentTransaction;
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //Special Exception in case the file has been created yet the transaction failed to be craeted.
                        if (createNew)
                        {
                            context.ftms_file.Remove(currentFile);
                            if (currentTransaction != null)
                                context.ftms_transaction.Remove(currentTransaction);
                            context.SaveChanges();
                            throw new Exception(string.Format("ManageFilesDataManager.CreateEditFile: {0}", ex.Message));
                        }
                    }
                    return currentFile.file_id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.CreateEditFile:CreateTransaction {0}", ex.Message));
            }
        }

        /// <summary>
        /// Gets all file types
        /// </summary>
        /// <returns></returns>
        public static List<FileType> getFileTypes()
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var fileTypes = context.file_type.Select(ft => new FileType
                    {
                        fileTypeId = ft.file_type_id,
                        fileTypeCode = ft.file_type_code,
                        description = ft.description,
                        minNumber = (int)ft.min_number,
                        maxNumber = (int)ft.max_number,
                        minSize = ft.min_size,
                        fileClassificationCode = ft.file_classification_code,
                        active = ft.active
                    }).ToList();
                    return fileTypes;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.getFileTypes: {0}", ex.Message));
            }
        }

        /// <summary>
        /// get file with last transaction.
        /// </summary>
        /// <param name="fileNumber"></param>
        /// <returns></returns>
        public static FileResults getFile(string fileNumber)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var fl = context.ftms_file.Where(c => c.file_number == fileNumber).Select(c => new FileResults
                    {
                        file_id = c.file_id,
                        transaction_id = c.current_transaction.Value,
                        file_no = c.file_number,
                        owner_name = c.owner_name,
                        app_date = c.app_date,
                        phone_number = c.phone_number,
                        rofo_date = c.rofo_date,
                        commencement_date = c.commencement_date,
                        lga_code = c.lga_code,
                        file_alias = c.file_alias,
                        to_location_mrcode = c.ftms_transaction.to_location,
                        current_condition = c.ftms_transaction.file_condition,
                        num_of_pages = c.ftms_transaction.number_page,
                        remark = c.remark,
                        batchNo = c.register_number,
                        rackNo = c.rack_number,
                        tracking_remark = c.ftms_transaction.tracking_remark,
                        current_location = c.ftms_transaction.ftms_locations1.description
                    }).FirstOrDefault();
                    return fl;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.getFile: {0}", ex.Message));
            }
        }

        public static List<FileResults> getFiles(List<String> filenums)
        {
            try
            {
                var flResult = new List<FileResults>();
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    foreach (var item in filenums)
                    {
                        var fl = context.ftms_file.Where(f => f.file_number == item).Select(record => new FileResults
                        {
                            file_id = record.file_id,
                            transaction_id = record.current_transaction.Value,
                            file_no = record.file_number,
                            owner_name = record.owner_name,
                            app_date = record.app_date,
                            current_location = record.ftms_transaction.ftms_locations1.description,
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
                            tracking_remark = record.ftms_transaction.tracking_remark
                        }).FirstOrDefault();
                        if (fl != null)
                            flResult.Add(fl);
                    }
                    return flResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.getFiles: {0}", ex.Message));
            }
        }

        public static List<location_type_upper> getLGAs()
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var state = context.ftms_setting.Where(c => c.mr_code == Helpers.Constants.ftmsEnvironmentState).FirstOrDefault();
                    var lgas = context.location_type_upper.Where(c => c.state == state.value).ToList();
                    return lgas.OrderBy(t => t.description).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.getFileConditions: {0}", ex.Message));
            }
        }

        public static bool CheckIfFileExists(string file_number)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var file = context.ftms_file.Where(f => f.file_number == file_number).FirstOrDefault();
                    return file != null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.CheckIfFileExists: {0}", ex.Message));
            }
        }
        public static string getFileNumberOfAlias(string alias, string file_number)
        {
            List<string> data = new List<string>();
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    if (alias != "")
                    {
                        data = context.ftms_file.Where(x => x.file_alias == alias && x.file_number != file_number).Select(t => t.file_number).ToList();
                        data.AddRange(context.files.Where(x => x.file_alias == alias && x.file_number != file_number).Select(x => x.file_number).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ManageFilesDataManager.fileStickerExist: {0} " + ex.Message);
            }
            return string.Join(",", data.Distinct().ToArray<string>());
        }

        public static void addFtmsSticker(string file_number, string qr_text, bool active)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    // deactivate the old record if exist.
                    var record = context.ftms_sticker.Where(x => x.ftms_file.file_number == file_number && x.active == true).ToList();
                    foreach (var item in record)
                        item.active = false;

                    // add new ftms sticker.
                    ftms_sticker sticker = new ftms_sticker();
                    sticker.ftms_file = context.ftms_file.Where(s => s.file_number == file_number).FirstOrDefault();
                    sticker.qr_full_text = qr_text;
                    sticker.created_date = DateTime.Now;
                    sticker.logged_user = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    sticker.active = active;
                    context.ftms_sticker.Add(sticker);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ManageFilesDataManager.addFtmsSticker: {0}", ex.Message));
            }
        }

        //save floating printed ftms barcode stickers to ftms_sticker table if the sticker is not exist.
        public static void saveFloatingFtmsSticker(List<File> files)
        {
            string qr_text = "";
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    foreach (var item in files)
                    {
                        qr_text = "^^" + item.FileID.ToString() + "/" + item.FileNumber.ToUpper() + "/" + item.RegisterNumber.ToUpper() + "/" + item.OwnerName.ToString() + "/" + Helpers.getFtmsSetting(Helpers.Constants.AppSignatureSetting) + "^^\n";
                        //check to see if floating sticker has been Captured in the ftms_sticker table
                        var qrExist = context.ftms_sticker.Any(c => c.qr_full_text == qr_text);
                        if (!qrExist)
                            addFtmsSticker(item.FileNumber, qr_text, true);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("ManageFilesDataManager.saveFloatingFtmsSticker: {0} " + e.Message);
            }
        }

        //populating  ftms_sticker data into frmManageFiles form gridview 
        public static List<FileSticker> getFileStickers(int file_id)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var lsStickers = context.ftms_sticker.Where(x => x.file_id == file_id).Select(c => new FileSticker()
                    {
                        sticker_id = c.sticker_id,
                        file_id = c.file_id,
                        file_number = c.ftms_file.file_number,
                        owner_name = c.ftms_file.owner_name,
                        created_date = c.created_date,
                        active = c.active,
                        logged_user = c.logged_user,
                        qr_label_txt = c.qr_full_text
                    }).OrderBy(c => c.created_date).ToList();
                    return lsStickers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ManageFilesDataManager.getFileStickers: {0} " + ex.Message);
            }
        }

        public static bool fileStickerExist(int file_id)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    if (context.ftms_sticker.Any(x => x.file_id == file_id))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ManageFilesDataManager.fileStickerExist: {0} " + ex.Message);
            }
        }

        public static int checkFileType(string fileType)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var ftype = context.file_type.Where(t => t.file_type_code == fileType && t.auto_generated && t.active).FirstOrDefault();
                    return ftype == null ? 0 : ftype.file_type_id;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ManageFilesDataManager.checkFileType: {0} " + ex.Message);
            }
        }

        public static string generateMerlinFile(int filetypeID, string fileAlias)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    file current_file = new file();
                    var ftype = context.file_type.Where(f => f.file_type_id == filetypeID).FirstOrDefault();
                    current_file.file_number_only = ftype.current_number.Value;
                    string formattedNo = ftype.current_number.Value.ToString().PadLeft(ftype.min_size, '0');
                    current_file.file_number = ftype.file_type_code + formattedNo;
                    ftype.current_number++;
                    current_file.auto_generated = ftype.auto_generated;
                    current_file.file_type1 = context.file_type.Where(ft => ft.file_type_id == filetypeID).FirstOrDefault();
                    current_file.recordation = DateTime.Now;
                    current_file.file_alias = fileAlias;
                    current_file.appuser = context.appusers.Where(t => t.username == "FTMS").FirstOrDefault();
                    if (current_file.appuser == null)
                        throw new Exception("Error in configuration. Pre-defined user 'FTMS' is missing. Contact your administrator");
                    current_file.file_status = context.file_status.Where(f => Helpers.Constants.FileStatusOpen == f.mr_code).FirstOrDefault();
                    context.SaveChanges();
                    return formattedNo;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ManageFilesDataManager.generateMerlinFile: {0} " + ex.Message);
            }
        }
    }
}