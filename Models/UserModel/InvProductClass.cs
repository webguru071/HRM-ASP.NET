using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Models.UserModel
{
    public class InvProductClass
    {
        public long TRNS_S_ID { get; set; }
        public long EQP_ID { get; set; }
        public string EQUIPMENT_TITLE { get; set; }           
        public string DATE { get; set; }
        public string PAY_TYPE { get; set; }
        public string VOUCHER_NO { get; set; }
        public string TYPE { get; set; }
        public long VENDOR_ID { get; set; }
        public long ASSET_ID { get; set; }
        public int UNIT { get; set; }
        public string REMARKS { get; set; }
    }
}