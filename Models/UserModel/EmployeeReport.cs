using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Models.UserModel
{
    public class EmployeeReport
    {
        public long ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string CONTACT { get; set; }
        public string EMAIL { get; set; }
        public string NID { get; set; }
        public string IMAGE { get; set; }
        public string STATUS { get; set; }
        public string JOINING_DATE { get; set; }
        public string RESIGNING_DATE { get; set; }
        public string DESIGNATION { get; set; }        
        public string CITY { get; set; }
        public string POSTAL_CODE { get; set; }
        public string DOB { get; set; }
        public string GENDER_TEXT { get; set; }
        public string MARITALA_STATUS_TEXT { get; set; }
        public string NATIONALITY { get; set; }
        public string BLOOD_GROUP { get; set; }
        public long DEPT_ID { get; set; }
        public string DEPT_TITLE { get; set; }
        public long DIV_ID { get; set; }
        public string DIV_TITLE { get; set; }
        public long POSITION_ID { get; set; }
        public string POSITION_TITLE { get; set; }
        public decimal BASIC_SALARY { get; set; }
        public string DUTY_TYPE_TEXT { get; set; }
        public string RATE_TYPE_TEXT { get; set; }
        public string PAY_FREQ_TEXT { get; set; }
    }
}