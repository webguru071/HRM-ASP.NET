﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Models.UserModel
{
    public class AttendanceClass
    {
        public long ATNDNC_ID { get; set; }
        public long EMPLOYEE_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string ATT_DATE { get; set; }
        public string CHECK_IN_TIME { get; set; }
        public string CHECK_OUT_TIME { get; set; }
        public string TOTAL_WORKING_HOUR { get; set; }
        public string PERDAY_WORKING_HOUR { get; set; }
        public string TOTAL_BREAK { get; set; }
        public string LESS_WORK { get; set; }
        public string OVER_TIME { get; set; }
        public string LATE_ARRIVED { get; set; }

    }
}