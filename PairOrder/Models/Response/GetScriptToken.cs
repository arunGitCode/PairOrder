using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PairOrder.Models.Response
{
    public class GetScriptToken
    {
        public int id { get; set; }
        public string exch { get; set; }
        public object exchange { get; set; }
        public object userId { get; set; }
        public string exchange_segment { get; set; }
        public object group_name { get; set; }
        public string symbol { get; set; }
        public string token { get; set; }
        public object description { get; set; }
        public object isin { get; set; }
        public string instrument_type { get; set; }
        public string option_type { get; set; }
        public string strike_price { get; set; }
        public string instrument_name { get; set; }
        public string expiry_date { get; set; }
        public string lot_size { get; set; }
        public string tick_size { get; set; }
        public object decimal_precision { get; set; }
        public object multiplier { get; set; }
        public object price_range_from { get; set; }
        public object price_range_to { get; set; }
        public int sort_order { get; set; }
        public int sort_order_1 { get; set; }
        public int sort_order_2 { get; set; }
        public int sort_order_3 { get; set; }
        public object created_on { get; set; }
        public object created_by { get; set; }
        public object updated_on { get; set; }
        public object updated_by { get; set; }
        public object active_status { get; set; }
        public object userSessionID { get; set; }
        public object tradingSymbol { get; set; }
        public object formattedInsName { get; set; }
    }

}
