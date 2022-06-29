using System;
using System.Linq;
using System.Text;
using FTMS.DAL;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing.Printing;

namespace FTMS.Common
{
    public class Helpers
    {
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public class Constants
        {
            public static readonly DateTime nullDate = new DateTime(1900, 1, 1);
            public const String unknownLocationCode = "unknown";
            public const String unknownConditionCode = "unknown";
            public const String landFileClassfcode = "land";
            public const String ValuationFileClassfcode = "valuation";
            public const String AdminFileClassfcode = "admin";
            public const String TestFileClassfcode = "test";
            public const String TownPlanningClassfcode = "TP";
            public const String OldLandFileClassfcode = "oldLand";
            public const String notConfigured = "NOT CONFIGURED";

            /// <summary>
            /// Setting strings
            /// </summary>
            //set to merlin when integrating with merlin, set to standalone when operating alone.
            public const String ftmsEnvironmentSetting = "ftmsEnv";
            public const String ftmsVersionSetting = "ftmsVersion";
            public const String ftmsEnvironmentMerlin = "merlin";
            public const String ftmsEnvironmentStandalone = "standalone";
            public const String ftmsEnvironmentState = "ftmsState";

            //These constants are part of setting data that were to be hardcoded
            public const String FileNoTextboxName = "FileNo";
            public const String FileAliasTextboxName = "FileAlias";
            public const String RegisterNoTextboxName = "RegisterNo";
            public const String QRImageBoxName = "QRGraphic";
            public const String RackNoTextboxName = "RackNo";

            //These Constants are for Watermark Addition for Training/Development Environment
            public const String WatermarkTextBox = "CURVED-TEXT";
            public const String WatermarkStateTextbox = "TEXT";
            public const String WatermarkAgencyTextBox = "TEXT_1";
            public const String Training = "training";
            public const String Development = "dev";
            public const String Live = "live";            
            

            //These constants were saved in ftms_setting table
            public const String BatSizeSetting = "BatSize";
            public const String PaperSizeSetting = "PaperSize";
            public const String AppSignatureSetting = "AppSignature";
            public const String AlternativeFileSetting = "AlternativeFile";
            public const String FileNoLabelTextSetting = "FileNoLabelText";
            public const String AltFileNoSetting = "AltFileNo";
            public const String BatPerRackSetting = "BatPerRack";
            public const String RegisterTextSetting = "RegisterText";
            public const String BatchNoOffsetSetting = "BatchNoOffset";
            public const String RackTextSetting = "RackText";
            public const String FileNoOffsetSetting = "FileNoOffset";
            public const String RackNoOffsetSetting = "RackNoOffset";
            public const String BatFirstRackSetting = "BatFirstRack";
            public const String ReportsPathSetting = @"ReportsPath";
            public const String RequestsPathSetting = @"RequestsPath";
            public const String RackNoLabelSetting = "RackNoLabel";
            public const String BatchNoLabelSetting = "BatchNoLabel";
            public const String StickerStatusSetting = "StickerStatus";
            public const String RequestFileTabSetting = "RequestFiles";
            public const String FileCensusTabSetting = "FileCensus";
            public const String oldFileExistsSetting = "oldFileExists";
            public const String printAliasSetting = "printAlias";
            public const String waterMarkerSetting = "dbEnv";

            //File statuses
            public const String FileStatusOpen = "open";

           
            //Agency Setting
            public const String AgencySetting = "agency";
        }
        public static int extractNumberFromFilenumber(String fileNumber)
        {
            var file_num = new String((from c in fileNumber
                                       where Char.IsDigit(c)
                                       select c).ToArray());
            if (fileNumber.Length == 0)
                return 0;
            return Convert.ToInt32(file_num);
        }

        public static string getPublicSetting(string setting_code)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    var settingObj = context.settings.Where(s => s.mr_code == setting_code && s.active).FirstOrDefault();
                    if (settingObj != null)
                        return settingObj.value;
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error While getting Setting due to: {0}", ex.Message));
            }
        }
        public static string getFtmsSetting(string setting_code)
        {
            try
            {
                using (ftmsdbEntities context = new ftmsdbEntities())
                {
                    ftms_setting settingObj = context.ftms_setting.Where(s => s.mr_code == setting_code && s.active).FirstOrDefault();
                    if (settingObj != null)
                        return settingObj.value;
                    else
                        return "";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error While getting FtmsSetting due to: {0}", ex.Message));
            }
        }
        
        public static String validateFileNumber(String file_number, String fRow)
        {
            try
            {
                Int64 fNum = 0;
                if (!Int64.TryParse(file_number, out fNum))
                    return "";
                String fullNo = String.Empty;
                var fileTypes = ManageFilesDataManager.getFileTypes().Where(c => c.fileTypeCode == fRow && c.minNumber <= fNum && c.maxNumber >= fNum).FirstOrDefault();
                if (fileTypes != null)
                    fullNo = fileTypes.fileTypeCode + fNum.ToString().PadLeft(fileTypes.minSize, '0');
                return fullNo;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error While validating the File/Alternative number due to: {0}", ex.Message));
            }
        }

        //TODO unify the this logic with the logic in the function validate file number
        public static String formatFileNumber(String file_number, String fileType)
        {
            try
            {
                Int64 fNum = 0;
                if (!Int64.TryParse(file_number, out fNum))
                    return "";
                String fullNo = String.Empty;
                var fileTypes = ManageFilesDataManager.getFileTypes().Where(c => c.fileTypeCode == fileType && c.minNumber <= fNum && c.maxNumber >= fNum).FirstOrDefault();
                if (fileTypes != null)
                    fullNo = fNum.ToString().PadLeft(fileTypes.minSize, '0');
                return fullNo;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error While validating the File/Alternative number due to: {0}", ex.Message));
            }
        }

        private static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string decryptedConnString()
        {
            var encrConnString = ConfigurationManager.ConnectionStrings["FtmsEntities"].ConnectionString;
            var decrConnString = Decrypt(encrConnString, "ftms@123456").Replace("&quot;", "\"").ToString();
            return decrConnString;
        }

        public static void logMessage(String Message, String Category)
        {
            using (ftmsdbEntities context = new ftmsdbEntities())
            {
                logger logEntry = new logger();
                logEntry.category = Category;
                logEntry.createdate = DateTime.Now;
                logEntry.machinename = System.Environment.MachineName;
                logEntry.message = Message;

                context.loggers.Add(logEntry);
                context.SaveChanges();
            }
        }

        public static string customConvertOldFile(string oldNo, string letter)
        {
            string newNo = oldNo;

            string stateName = Helpers.getFtmsSetting(Helpers.Constants.ftmsEnvironmentState);
            switch (stateName)
            {
                case "Oyo":
                    {
                        switch (letter.ToUpper())
                        {
                            case "LUD":
                                newNo = "0" + newNo;
                                break;
                            case "LUDA":
                                newNo = "1" + newNo;
                                break;
                            case "LUDB":
                                newNo = "2" + newNo;
                                break;
                            case "LUDC":
                                newNo = "3" + newNo;
                                break;
                            case "LUDD":
                                newNo = "4" + newNo;
                                break;
                            default:
                                newNo = "";
                                break;
                        }
                        break;
                    }
                case "Edo":
                    {
                        switch (letter.ToUpper())
                        {
                            case "LUD":
                                newNo = "0" + newNo;
                                break;
                            case "LUA":
                                newNo = "1" + newNo;
                                break;
                            case "LUB":
                                newNo = "2" + newNo;
                                break;
                            case "LUC":
                                newNo = "3" + newNo;
                                break;
                            default:
                                newNo = "";
                                break;
                        }
                        break;
                    }
                default:
                    return "";
            }
            return newNo;
        }

        public static bool printerExists(string printer_name)
        {
            try
            {
                PrinterSettings printer = new PrinterSettings();
                printer.PrinterName = printer_name;
                return printer.IsValid;                   
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error While checking the printer due to: {0}", ex.Message));
            }
        }

       
    }
}