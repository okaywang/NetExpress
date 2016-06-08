using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetExpress.Http
{
    public class HttpHelper
    {
        public static string Get(string url)
        {
            var webRequest = WebRequest.Create(url);
            var webResp = webRequest.GetResponse();
            using (Stream s = webResp.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.UTF8);
                return reader.ReadToEnd();

            } 
        }
    }
}
