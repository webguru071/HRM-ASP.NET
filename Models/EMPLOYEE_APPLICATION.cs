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
    
    public partial class EMPLOYEE_APPLICATION
    {
        public long APP_ID { get; set; }
        public string APP_SUB { get; set; }
        public string APP_BODY { get; set; }
        public long EMP_ID { get; set; }
    
        public virtual EMPLOYEE_INFO EMPLOYEE_INFO { get; set; }
    }
}
