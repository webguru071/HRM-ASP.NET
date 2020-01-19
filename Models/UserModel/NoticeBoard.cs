using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMSApp.Models.UserModel
{
    public class NoticeBoard
    {
        public long ID { get; set; }
        public string NOTICE { get; set; }
        public Nullable<long> ACTION_BY { get; set; }
        public System.DateTime ACTION_DATE { get; set; }
        public Nullable<long> UPDATE_BY { get; set; }
        public Nullable<System.DateTime> UPDATE_DATE { get; set; }
        public string STATUS { get; set; }
        public string ACTIVE_DATE { get; set; }
    }
}