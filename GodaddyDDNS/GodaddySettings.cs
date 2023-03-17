using System;
using System.Collections.Generic;
using System.Text;

namespace GodaddyDDNS
{
    public class GodaddySettings
    {
        public string Key { set; get; }
        public string Secret { set; get; }

        public string Name { get; set; }

        public string Domain { get; set; }
        public string MyIP { get; set; }

    }
}
