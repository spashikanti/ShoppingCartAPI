using Newtonsoft.Json;

namespace ShoppingCartApi.Models
{
    public class Metadata
    {
        [JsonProperty(PropertyName = "createdDate")]
        public string createdDate { get; set; }
        [JsonProperty(PropertyName = "createdBy")]
        public string createdBy { get; set; }
        [JsonProperty(PropertyName = "modifiedDate")]
        public string modifiedDate { get; set; }
        [JsonProperty(PropertyName = "modifiedBy")]
        public string modifiedBy { get; set; }
    }
}
