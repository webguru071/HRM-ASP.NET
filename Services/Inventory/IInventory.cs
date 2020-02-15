using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EMSApp.Models;
using EMSApp.Models.UserModel;

namespace EMSApp.Services.Inventory
{
    public interface IInventory
    {
        bool InsertInventoryData(INV_INFO collection, STOCK_INFO stock, string status = "");
        bool UpdateInventoryData(INV_INFO collection, STOCK_INFO stock, long id = 0, string status = "");
    }
}