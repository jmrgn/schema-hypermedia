using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Schema.Hypermedia.Interfaces;

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
