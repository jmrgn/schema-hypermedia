using Schema.Hypermedia.Models;
using System.Collections.Generic;

namespace Schema.Hypermedia.Interfaces
{
    public interface IHypermediaSchema
    {
        IList<Link> Links { get; set; }
    }
}
