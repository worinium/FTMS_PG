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
    
    public partial class lkp_file_condition
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public lkp_file_condition()
        {
            this.ftms_transaction = new HashSet<ftms_transaction>();
        }
    
        public string mr_code { get; set; }
        public string description { get; set; }
        public bool active { get; set; }
        public int positions { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ftms_transaction> ftms_transaction { get; set; }
    }
}