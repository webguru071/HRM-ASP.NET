using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Models.UserModel
{
    public class EmployeeLeaveClass
    {
        public long EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public double MEDI_LEAVE { get; set; }
        public double CASUAL_LEAVE { get; set; }
        public double HALF_DAY_LEAVE { get; set; }
        public double FULL_DAY_LEAVE { get; set; }
        public double TOTAL_LEAVE { get; set; }
        public double TOTAL_LEAVE_TAKEN { get; set; }
        public double REMAIN_LEAVE { get; set; }
        public double EXCEED_LEAVE { get; set; }
    }
}