using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartApi.Models
{
    public class UserDetailsModel:Metadata
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
        [JsonProperty(PropertyName = "emailID")]
        public string emailID { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public string contact { get; set; }
        [JsonProperty(PropertyName = "firstName")]
        public string firstName { get; set; }
        [JsonProperty(PropertyName = "lastName")]
        public string lastName { get; set; }
        //[JsonProperty(PropertyName = "deliveryAddress")]
        //public string deliveryAddress { get; set; }
        [JsonProperty(PropertyName = "deliveryAddress")]
        public Deliveryaddress deliveryAddress { get; set; }
        //public Deliveryaddress deliveryAddress { get; set; }
        //public List<DeliveryAddress> deliveryAddress { get; set; }
    }

    public class Deliveryaddress
    {
        [JsonProperty(PropertyName = "houseNO")]
        public string houseNO { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string state { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string city { get; set; }
        [JsonProperty(PropertyName = "landmark")]
        public string landmark { get; set; }
        [JsonProperty(PropertyName = "street")]
        public string street { get; set; }
        [JsonProperty(PropertyName = "pincode")]
        public string pincode { get; set; }
    }
}
