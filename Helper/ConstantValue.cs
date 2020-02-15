using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Helper
{
    public class ConstantValue
    {
        public ConstantValue()
        {

        }
        //Status
        public static string UserStatusActive = "a";
        public static string UserStatusDeactive = "d";
        //User
        public static string UserLevelAdmin = "a";
        public static string UserLevelEmployee = "e";
        //Employee 
        public static string GenderMale = "m";
        public static string GenderFemale = "f";
        public static string GenderOther = "o";
        public static string MaritalStatusSingle = "s";
        public static string MaritalStatusMarried = "m";
        public static string MaritalStatusSeparated = "x";
        //Position
        public static string DutyTypeFullTime = "f";
        public static string DutyTypePartTime = "p";
        public static string DutyTypeContructual = "c";
        public static string DutyTypeOther = "o";
        public static string RateTypeHourly = "h";
        public static string RateTypeSalary = "s";
        public static string PayFreqTypeDaily = "d";
        public static string PayFreqTypeWeekly = "w";
        public static string PayFreqTypeBiWeekly = "b";
        public static string PayFreqTypeMonthly = "m";
        //Leave Application
        public static string LeaveStatusApproved = "a";
        public static string LeaveStatusPending = "p";
        public static string LeaveStatusCanceled = "c";
        //Transaction Type
        public static string TransactionTypeExpense = "e";
        public static string TransactionTypeIncome = "i";
        //Transaction Sheet
        public static string TransactionSheetPayTypeCash = "c";
        public static string TransactionSheetPayTypeBank = "b";
        public static string TransactionSheetTransTypePurchase = "p";
        public static string TransactionSheetTransTypeSell = "s";
        //Payroll
        public static string SalaryGradeAdd = "a";
        public static string SalaryGradeDeduct = "d";
        //Activity Type
        public static string TypeActive = "a";
        public static string TypeDeactive = "d";
        //Attendance
        public static string AttendanceCheckIn = "i";
        public static string AttendanceCheckOut = "o";
        //Vendor
        public static string VendorTypeSupplier = "s";
        public static string VendorTypeBuyer = "b";
        //Stock
        public static string StockForAdd = "a";
        public static string StockForDeduct = "d";
    }
}