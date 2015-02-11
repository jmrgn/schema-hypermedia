using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schema.Hypermedia.Models
{
    public class Link : IHyperlink
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Title { get; set; }
        public string EncType { get; set; }
        public string Method { get; set; }
        public string Schema { get; set; }
    }
}
