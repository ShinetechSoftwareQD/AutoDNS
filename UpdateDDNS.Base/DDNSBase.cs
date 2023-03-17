using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace UpdateDDNS.Base
{
    public class DDNSBase
    {
        public string GetLocalIP(string url)
        {
            string ip = "";


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())

            using (StreamReader reader = new StreamReader(stream))
            {
                ip = reader.ReadToEnd();
            }

            return ip;
        }
    }
}
