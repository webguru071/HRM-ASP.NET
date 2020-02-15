using EMSApp.Models.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using EMSApp.Helper;
using EMSApp.Models;

namespace EMSApp.Services.Inventory
{
    public class InventoryService : IInventory
    {
        DBHelper dbHelperObj = new DBHelper();
        public bool InsertInventoryData(INV_INFO collection, STOCK_INFO stock, string status = "")
        {
            var list = new List<KeyValuePair<SqlCommand, string>>();
            list = GetInventoryDataIntoList(collection,list);
            list = GetStockDataIntoList(stock,list);
            bool result = dbHelperObj.ExecuteCommandWithParameterList(list);
            return result;
        }

        public bool UpdateInventoryData(INV_INFO collection, STOCK_INFO stock, long id = 0, string status = "")
        {
            var list = new List<KeyValuePair<SqlCommand, string>>();
            list = GetInventoryDataIntoList(collection, list,"update",id);
            list = GetStockDataIntoList(stock, list, "update", id);
            bool result = dbHelperObj.ExecuteCommandWithParameterList(list);
            return result;
        }
        private List<KeyValuePair<SqlCommand, string>> GetStockDataIntoList(STOCK_INFO stock, List<KeyValuePair<SqlCommand, string>> list,string qType="", long id=0)
        {
            string query = (string.IsNullOrEmpty(qType) ? InsertQueryStock() : UpdateQueryStock(id));
            SqlCommand command = new SqlCommand();           
            command.Parameters.Add("EQP_ID", SqlDbType.BigInt).Value = stock.EQP_ID;
            command.Parameters.Add("UNIT", SqlDbType.Int).Value = stock.UNIT;
            command.Parameters.Add("STOCK_TYPE", SqlDbType.Char, 1).Value = stock.STOCK_TYPE;
            command.Parameters.Add("STOCK_FOR", SqlDbType.Char, 1).Value = stock.STOCK_FOR;
            if (string.IsNullOrEmpty(qType))
            {
                command.Parameters.Add("ACTION_BY", SqlDbType.BigInt).Value = stock.ACTION_BY;
                command.Parameters.Add("ACTION_DATE", SqlDbType.DateTime).Value = stock.ACTION_DATE;
                command.Parameters.Add("INV_ID", SqlDbType.BigInt).Value = stock.INV_ID;
            }
            else
            {
                command.Parameters.Add("INV_ID", SqlDbType.BigInt).Value = id;
                command.Parameters.Add("UPDATE_BY", SqlDbType.BigInt).Value = stock.UPDATE_BY;
                command.Parameters.Add("UPDATE_DATE", SqlDbType.DateTime).Value = stock.UPDATE_DATE;
            }
            list.Add(new KeyValuePair<SqlCommand, string>(command, query));
            return list;
        }
        private List<KeyValuePair<SqlCommand, string>> GetInventoryDataIntoList(INV_INFO collection, List<KeyValuePair<SqlCommand, string>> list, string qType = "", long id = 0)
        {
            string query = (string.IsNullOrEmpty(qType) ? InsertQueryInventory() : UpdateQueryInventory(id));
            SqlCommand command = new SqlCommand();
            command.Parameters.Add("INV_ID", SqlDbType.BigInt).Value = id;
            command.Parameters.Add("EQP_ID", SqlDbType.BigInt).Value = collection.EQP_ID;
            command.Parameters.Add("VENDOR_ID", SqlDbType.BigInt).Value = collection.VENDOR_ID;
            command.Parameters.Add("UNIT", SqlDbType.Int).Value = collection.UNIT;
            command.Parameters.Add("DATE", SqlDbType.NVarChar,50).Value = collection.DATE;
            command.Parameters.Add("STOCK_TYPE", SqlDbType.Char,1).Value = collection.STOCK_TYPE;
            if (string.IsNullOrEmpty(qType))
            {
                command.Parameters.Add("ACTION_BY", SqlDbType.BigInt).Value = collection.ACTION_BY;
                command.Parameters.Add("ACTION_DATE", SqlDbType.DateTime).Value = collection.ACTION_DATE;
            }
            else
            {
                command.Parameters.Add("UPDATE_BY", SqlDbType.BigInt).Value = collection.UPDATE_BY;
                command.Parameters.Add("UPDATE_DATE", SqlDbType.DateTime).Value = collection.UPDATE_DATE;
            }           
            list.Add(new KeyValuePair<SqlCommand, string>(command, query));
            return list;
        }
        string InsertQueryStock()
        {
            string query = @"";
            return query;
        }
        string InsertQueryInventory()
        {
            string query = @"";
            return query;
        }
        string UpdateQueryStock(long id)
        {
            string query = @"UPDATE STOCK_INFO SET EQP_ID=@EQP_ID,UNIT=@UNIT,STOCK_TYPE=@STOCK_TYPE,STOCK_FOR=@STOCK_FOR,UPDATE_BY=@UPDATE_BY,UPDATE_DATE=@UPDATE_DATE WHERE INV_ID=" + id;
            return query;
        }
        string UpdateQueryInventory(long id)
        {
            string query = @"UPDATE INV_INFO SET EQP_ID=@EQP_ID,VENDOR_ID=@VENDOR_ID,UNIT=@UNIT,STOCK_TYPE=@STOCK_TYPE,UPDATE_BY=@UPDATE_BY,UPDATE_DATE=@UPDATE_DATE WHERE INV_ID="+id;
            return query;
        }
    }
}