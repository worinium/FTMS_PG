using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FTMS.Common.DTOs
{
    public class FileRequest
    {
        [DisplayName("Request ID")]
        public int request_id { get; set; }
        [DisplayName("Requestor")]
        public string requestor_name { get; set; }
        [DisplayName("Request Date")]
        public DateTime request_date { get; set; }
        [DisplayName("Request Purpose")]
        public string request_purpose { get; set; }
    }
}
