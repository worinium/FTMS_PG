using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FTMS.Common.DTOs
{
    public class FileResults
    {
        //File Table
        public int file_id { get; set; }
        [DisplayName("File Number")]
        public String file_no { get; set; }
        [DisplayName("Owner Name")]
        public String owner_name { get; set; }
        [DisplayName("Application Date")]
        public DateTime? app_date { get; set; }
        [DisplayName("RofO Date")]
        public DateTime? rofo_date { get; set; }
        [DisplayName("CofO Date")]
        public DateTime? commencement_date { get; set; }
        [DisplayName("Phone Number")]
        public String phone_number { get; set; }
        [DisplayName("Remark")]
        public String remark { get; set; }
        [DisplayName("File Alias")]
        public String file_alias { get; set; }
        [DisplayName("LGA")]
        public String lga_code { get; set; }
        public string batchNo { get; set; }
        public string rackNo { get; set; }
        [DisplayName("Create Date")]
        public DateTime? create_date { get; set; }


        //Transaction Table
        public int transaction_id { get; set; }
        [DisplayName("Current Location")]
        public String current_location { get; set; }
        [DisplayName("File Condition")]
        public String current_condition { get; set; }
        [DisplayName("Number of Pages")]
        public int num_of_pages { get; set; }
        [DisplayName("Previous Location")]
        public String previous_location { get; set; }
        [DisplayName("Transaction Date")]
        public DateTime transaction_date { get; set; }
        [DisplayName("User")]
        public String logged_user { get; set; }
        [DisplayName("PC Name")]
        public String pc_name { get; set; }
        public String to_location_mrcode { get; set; }
        public String file_condition_mrcode { get; set; }
        [DisplayName("Tracking Remark")]
        public String tracking_remark { get; set; }
    }
}
