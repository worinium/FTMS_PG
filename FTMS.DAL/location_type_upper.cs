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
    
    public partial class location_type_upper
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public location_type_upper()
        {
            this.ftms_file = new HashSet<ftms_file>();
        }
    
        public string mr_code { get; set; }
        public bool active { get; set; }
        public string description { get; set; }
        public string state { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ftms_file> ftms_file { get; set; }
        public virtual lkp_state lkp_state { get; set; }
    }
}
