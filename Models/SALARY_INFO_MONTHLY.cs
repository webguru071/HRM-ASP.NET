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
    
    public partial class SALARY_INFO_MONTHLY
    {
        public long SALARY_INFO_MONTHLY_ID { get; set; }
        public long EMPLOYEE_ID { get; set; }
        public decimal BASIC_SALARY { get; set; }
        public decimal GROSS_SALARY { get; set; }
        public Nullable<decimal> ADDITIONAL { get; set; }
        public Nullable<decimal> BONUS { get; set; }
        public Nullable<decimal> COMMISSION { get; set; }
        public Nullable<decimal> ADVANCE { get; set; }
        public Nullable<decimal> DEDUCTION { get; set; }
        public Nullable<double> LEAVE_COUNT { get; set; }
        public decimal TOTAL_SALARY { get; set; }
        public string NOTE { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public long ACTION_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
    
        public virtual EMPLOYEE_INFO EMPLOYEE_INFO { get; set; }
    }
}
