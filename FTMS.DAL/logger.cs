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
    
    public partial class logger
    {
        public int id { get; set; }
        public Nullable<System.DateTime> createdate { get; set; }
        public string machinename { get; set; }
        public string message { get; set; }
        public string category { get; set; }
    }
}