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
    
    public partial class TRANSACTION_SHEET
    {
        public long TRNS_S_ID { get; set; }
        public string DATE { get; set; }
        public long TRNS_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public string PAY_TYPE { get; set; }
        public string VOUCHER_NO { get; set; }
        public string TYPE { get; set; }
        public string REMARKS { get; set; }
        public long ACTION_BY { get; set; }
        public Nullable<System.DateTime> ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    
        public virtual TRANSACTION_ITEM TRANSACTION_ITEM { get; set; }
    }
}
