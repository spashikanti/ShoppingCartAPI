using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartApi.Models
{
    public class ProductModel : Metadata
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "specification")]
        public List<Specification> Specification { get; set; }
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }
    }
    public class Specification
    {
        [JsonProperty(PropertyName = "size")]
        public string Size { get; set; }
        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }
    }
    public class Images
    {
        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }
    }
}
