using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
