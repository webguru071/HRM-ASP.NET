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
    
    public partial class SALARY_SETUP
    {
        public long SALARY_SET_ID { get; set; }
        public long EMP_ID { get; set; }
        public string PAY_TYPE { get; set; }
        public decimal GROSS_SALARY { get; set; }
        public long ACTION_BY { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
    
        public virtual EMPLOYEE_INFO EMPLOYEE_INFO { get; set; }
    }
}
