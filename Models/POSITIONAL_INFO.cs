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
    
    public partial class POSITIONAL_INFO
    {
        public long POSITION_ID { get; set; }
        public string POSITION_TITLE { get; set; }
        public string DUTY_TYPE { get; set; }
        public string RATE_TYPE { get; set; }
        public string PAY_FREQ { get; set; }
        public long DIV_ID { get; set; }
        public long ACTION_BY { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public long EMPLOYEE_ID { get; set; }
    
        public virtual DIVISION_INFO DIVISION_INFO { get; set; }
        public virtual EMPLOYEE_INFO EMPLOYEE_INFO { get; set; }
    }
}