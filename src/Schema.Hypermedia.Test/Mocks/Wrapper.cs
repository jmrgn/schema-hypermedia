using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Schema.Hypermedia.Test.Mocks
{
    public class Wrapper<T> : IHypermediaResource 
    {
        public Wrapper()
        {
                
        }
        
        T Person { get; set; }

        public IList<IHyperlink> Links { get; set; }
    }
}
