using Newtonsoft.Json;


namespace PairOrder.Models.Request
{
    public class GetScripModel
    {
        [JsonProperty("symobl")]
        public string symbol { get; set; }

        [JsonProperty("exchange")]
        public List<string> exchange { get; set; }

    }
}
