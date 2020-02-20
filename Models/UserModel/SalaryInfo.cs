using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Models.UserModel
{
    public class SalaryInfo
    {
        public long SALARY_SET_ID { get; set; }
        public long EMP_ID { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string SALARY_MONTH { get; set; }
        public string SALARY_PAID { get; set; }
        public string SALARY_YEAR { get; set; }
        public decimal BASIC_SALARY { get; set; }
        public Nullable< decimal> BONUS { get; set; }
        public Nullable<decimal> OTHERS { get; set; }
        public decimal GROSS_SALARY { get; set; }
        public string SALARY_GRADE_SETUP { get; set; }
        public string REMARKS { get; set; }
    }
}