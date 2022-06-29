using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTMS.Common.DTOs
{
    public class Transaction
    {
        //Transaction Table
        public int TransactionID { get; set; }
        public int FileID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int NumOfPages { get; set; }
        public String LoggedUser { get; set; }
        public String PCName { get; set; }
        public String TrackingRemark { get; set; }
        public String FromLocation { get; set; }
        public String ToLocation { get; set; }
        public String FileCondition { get; set; }
    }
}
