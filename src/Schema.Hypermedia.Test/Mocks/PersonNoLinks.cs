using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Schema.Hypermedia.Test.Mocks
{
    public class PersonNoLinks
    {
        [JsonProperty("id")]
        public string Id { get; set; }
 
        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("honorificPrefix")]
        public string HonorificPrefix { get; set; }

        [JsonProperty("honorificSuffix")]
        public string HonorificSuffix { get; set; }
    }
}
