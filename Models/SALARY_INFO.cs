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
    
    public partial class SALARY_INFO
    {
        public long ID { get; set; }
        public long EMPLOYEE_ID { get; set; }
        public string SALARY_PAID { get; set; }
        public decimal GROSS_SALARY { get; set; }
        public Nullable<decimal> BONUS { get; set; }
        public Nullable<decimal> DEDUCTION { get; set; }
        public Nullable<decimal> TOTAL { get; set; }
        public Nullable<decimal> OTHERS { get; set; }
        public Nullable<decimal> ADDITION { get; set; }
        public string STATUS { get; set; }
        public Nullable<decimal> ADVANCE { get; set; }
        public Nullable<decimal> COMMISSION { get; set; }
        public string REMARKS { get; set; }
        public long ACTION_BY { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    
        public virtual EMPLOYEE_INFO EMPLOYEE_INFO { get; set; }
    }
}
