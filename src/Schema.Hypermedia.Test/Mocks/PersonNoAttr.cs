
using Newtonsoft.Json.Schema;
using Schema.Hypermedia.Models;

namespace Schema.Hypermedia.Test.Mocks
{
    public class PersonNoAttr : HypermediaResource
    {
        private JsonSchema schema;

        public PersonNoAttr(JsonSchema schema)
        {
            this.schema = schema;
        }
        public string Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string HonorificPrefix{ get; set; }
        public string HonorificSuffix { get; set; }

        protected override JsonSchema Schema
        {
            get { return schema; }
        }
    }
}
