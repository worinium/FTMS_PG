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
    
    public partial class appuser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public appuser()
        {
            this.appuser1 = new HashSet<appuser>();
            this.appuser11 = new HashSet<appuser>();
            this.files = new HashSet<file>();
        }
    
        public int appuser_id { get; set; }
        public string username { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public Nullable<int> created_by { get; set; }
        public System.DateTime created_date { get; set; }
        public Nullable<int> modified_by { get; set; }
        public System.DateTime modified_date { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appuser> appuser1 { get; set; }
        public virtual appuser appuser2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<appuser> appuser11 { get; set; }
        public virtual appuser appuser3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<file> files { get; set; }
    }
}