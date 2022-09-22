using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairOrder.Models.Request
{
    public class PlaceOrder
    {
        public string complexty { get; set; }
        public string discqty { get; set; }
        public string exch { get; set; }
        public string pCode { get; set; }
        public string prctyp { get; set; }
        public string price { get; set; }
        public int qty { get; set; }
        public string ret { get; set; }
        public string symbol_id { get; set; }
        public string trading_symbol { get; set; }
        public string transtype { get; set; }
        public string trigPrice { get; set; }
    }
}
