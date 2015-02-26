using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Schema.Hypermedia.Models;

namespace Schema.Hypermedia.Test.Mocks
{
    public class Person : HypermediaResource
    {
        private JsonSchema schema;

        public Person(JsonSchema schema)
        {
            this.schema = schema;
        }

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

        protected override JsonSchema Schema
        {
            get { return this.schema; }
        }
    }
}
