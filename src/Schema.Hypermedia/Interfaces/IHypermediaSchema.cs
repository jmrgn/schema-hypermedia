using Schema.Hypermedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Hypermedia.Interfaces
{
    public interface IHypermediaSchema
    {
        IList<Link> Links { get; set; }
    }
}
