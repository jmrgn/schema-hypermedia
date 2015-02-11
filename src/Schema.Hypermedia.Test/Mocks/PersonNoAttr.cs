
namespace Schema.Hypermedia.Test.Mocks
{
    public class PersonNoAttr : IHypermediaResource
    {
        public string Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string HonorificPrefix{ get; set; }
        public string HonorificSuffix { get; set; }
    }
}
