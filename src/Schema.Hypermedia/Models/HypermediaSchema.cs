using Schema.Hypermedia.Interfaces;
using System.Collections.Generic;

namespace Schema.Hypermedia.Models
{
    public class HypermediaSchema : IHypermediaSchema
    {
        public IList<Link> Links { get; set; }
        public HypermediaSchema()
        {
            Links = new List<Link>();
        }
    }
}
