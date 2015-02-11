using Newtonsoft.Json;

namespace Schema.Hypermedia.Test.Mocks
{
    public class Person : IHypermediaResource
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("honorificPrefix")]
        public string HonorificPrefix{ get; set; }

        [JsonProperty("honorificSuffix")]
        public string HonorificSuffix { get; set; }
    }
}
