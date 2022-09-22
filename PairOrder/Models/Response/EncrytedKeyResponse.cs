using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairOrder.Models.Response
{
     public class EncrytedKeyResponse
    {
        public string userId { get; set; }
        public string userData { get; set; }
        public string encKey { get; set; }
        public string apikey { get; set; }
        public string stat { get; set; }
        public object emsg { get; set; }
        public object loginType { get; set; }
        public object version { get; set; }
        public bool login { get; set; }
    }
}
