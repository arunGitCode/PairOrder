using Newtonsoft.Json;

namespace PairOrder.Models.Request
{
    internal class GetSessionIdModel : GetEncryptionModel
    {
        [JsonProperty("userData")]
        public string userData { get; set; }
    }
}
