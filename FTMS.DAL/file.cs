//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FTMS.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class file
    {
        public int file_id { get; set; }
        public string file_number { get; set; }
        public bool auto_generated { get; set; }
        public string file_alias { get; set; }
        public int file_type { get; set; }
        public Nullable<int> owner_id { get; set; }
        public string file_status_code { get; set; }
        public string property_no { get; set; }
        public Nullable<decimal> property_size { get; set; }
        public string location_code { get; set; }
        public string landuse_code { get; set; }
        public string landpurpose_code { get; set; }
        public System.DateTime recordation { get; set; }
        public Nullable<bool> active { get; set; }
        public Nullable<int> leaseyear_code { get; set; }
        public Nullable<bool> rofo_exists { get; set; }
        public Nullable<bool> cofo_exists { get; set; }
        public int modified_by { get; set; }
        public string allocation_status { get; set; }
        public string transaction_status { get; set; }
        public Nullable<int> other_claimants_code { get; set; }
        public Nullable<int> file_number_only { get; set; }
    
        public virtual allocation_status allocation_status1 { get; set; }
        public virtual allocation_status allocation_status2 { get; set; }
        public virtual appuser appuser { get; set; }
        public virtual file_status file_status { get; set; }
        public virtual file_type file_type1 { get; set; }
        public virtual party party { get; set; }
    }
}
