using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Utility.Helper
{
    public class JsonHelper
    {
        public static string End(string code, object data = null, string msg = null)
        {
            return Serialize(new { code = code, data = data, msg = msg });
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T DeSerialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
