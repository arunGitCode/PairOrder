using Newtonsoft.Json;
namespace PairOrder.Models.Request
{
    internal class GetEncryptionModel
    {
        [JsonProperty("userId")]
        public string userId { get; set; }
    }
}
