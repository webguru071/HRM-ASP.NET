//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EMSApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NOTICE_BOARD
    {
        public long ID { get; set; }
        public string NOTICE { get; set; }
        public Nullable<long> ACTION_BY { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string STATUS { get; set; }
    }
}
