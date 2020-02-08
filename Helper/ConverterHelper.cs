using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Helper
{
    public class ConverterHelper
    {
        public long GetLoggedUserID()
        {
            long id = Convert.ToInt64(HttpContext.Current.Session["USER_ID"]);
            return id;
        }
        public long GetLoggedEmployeeID()
        {
            long id = Convert.ToInt64(HttpContext.Current.Session["EMP_ID"]);
            return id;
        }
        public string GetLoggedUserLevel()
        {
            string level = Convert.ToString(HttpContext.Current.Session["USER_LEVEL"]);            
            return level;
        }
        public string GetLoggedEmployeeName()
        {
            string name = Convert.ToString(HttpContext.Current.Session["USER_NAME"]);
            return name;
        }
        public string GetFormatted12HTime(string time)
        {
            string strTime = "";
            if (!string.IsNullOrEmpty(time))
            {
                string startPart = "";
                bool isNoon = false;
                string[] timeList = time.Split(':');
                int firstPart = Convert.ToInt32(timeList[0]);
                if (firstPart > 12)
                {
                    startPart = (firstPart - 12).ToString();
                    isNoon = true;
                }
                if (isNoon)
                {
                    strTime = startPart + " : " + timeList[1] + " : " + timeList[2] + " pm";
                }
                else
                {
                    strTime = timeList[0] + " : " + timeList[1] + " : " + timeList[2] + " am";
                }
            }
            return strTime;
        }
    }
}