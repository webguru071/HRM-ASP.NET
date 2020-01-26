using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using EMSApp.Helper;

namespace EMSApp.Services
{
    public class CombineServices : ICombine
    {
        DBHelper dbHelper = new DBHelper();
        public string GetTeamLeaderById(long id)
        {
            string query = @"SELECT * FROM TEAM_INFO TI 
                            INNER JOIN EMPLOYEE_INFO EI ON EI.ID=TI.TEAM_LEADER
                            WHERE EI.IS_DELETED='a' AND TI.ID=" + id;
            DataTable data = dbHelper.GetDataTable(query);
            string leaderName = data.Rows[0]["EMPLOYEE_NAME"].ToString();
            return leaderName;
        }

        public bool SalarySetupStatusChange(long id, string statusV)
        {
            string query = @"UPDATE SALARY_SETUP  SET CANGE_TYPE='"+ statusV + "' WHERE SALARY_SET_ID="+id;
            bool result = dbHelper.ExecuteDML(query);
            return result;
        }
    }
}