using System;

namespace UpdateDDNS.Aliyun
{
    public class AliyunSettings
    {
        public string Key { set; get; }
        public string Secret { set; get; }

        public string Name { get; set; }

        public string Domain { get; set; }

        public string MyIP { get; set; }
    }
}
