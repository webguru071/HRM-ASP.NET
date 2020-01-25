using EMSApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;

namespace EMSApp.Helper
{
    public class JsonHelper
    {
        EMSEntities db = new EMSEntities();
        public static string GetJsonString(List<object> list)
        {
            string JsonStr = null;
           

            MemoryStream str = new MemoryStream();

            //this line very important which make ready program to make JSON  
            //GetType giving idea about you are going to create json for "System.Collections.Generic.List`1[tblMyFriend]"  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(list.GetType());

            //Writing the JSON  
            ser.WriteObject(str, list);

            str.Position = 0;
            StreamReader sr = new StreamReader(str);
            JsonStr = sr.ReadToEnd();
            object empObj = JsonConvert.DeserializeObject<object>(JsonStr);
            return JsonStr;
        }
    }
}