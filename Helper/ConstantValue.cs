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
        //User
        public static string UserLevelAdmin="a";
        public static string UserLevelEmployee="e";
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
    }
}