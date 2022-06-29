using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FTMS.Common.DTOs
{
    public class CensusResult
    {
        public string file_number { get; set; }
        public string census_location_code { get; set; }
        public string census_location_description { get; set; }
        public DateTime census_date { get; set; }
        public string current_location_code { get; set; }
        public string current_location_description { get; set; }
        public DateTime transaction_date { get; set; }
    }
}
