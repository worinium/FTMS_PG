using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTMS.Common.DTOs
{
    public class File
    {
        //File Table
        public int FileID { get; set; }
        public String FileNumber { get; set; }
        public String OwnerName { get; set; }
        public Boolean RofOExists { get; set; }
        public Boolean CofOExists { get; set; }
        public String PhoneNumber { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public String Remark { get; set; }
        public string RegisterNumber { get; set; }
        public DateTime? RofODate { get; set; }
        public DateTime? CommencementDate { get; set; }
        public String FileAlias { get; set; }
        public String LGACode { get; set; }
        public String RackNumber { get; set; }
        public string CurrentLocationDesc { get; set; }
        public string CurrentTransactionDateString { get; set; }
        public DateTime CurrentTransactionDate { get; set; }
        public String CurrentLocationCode { get; set; }
        public int CurrentTransactionID { get; set; }
        public String CurrentFileConditionCode { get; set; }
        public int CurrentNumOfPages { get; set; }
    }
}
