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
        public int MEDI_LEAVE { get; set; }
        public int CASUAL_LEAVE { get; set; }
        public int HALF_DAY_LEAVE { get; set; }
        public int FULL_DAY_LEAVE { get; set; }
    }
}