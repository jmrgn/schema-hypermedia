using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Schema.Hypermedia.Models;
using System.Collections.Generic;

namespace Schema.Hypermedia.Interfaces
{
    public interface IHypermediaGenerator
    {
        JsonSerializer Serializer { get; set; }
        InspectionBehavior InspectionBehavior { get; set; }
        IEnumerable<Link> GetLinks(string schema, IHypermediaResource entity);
        IEnumerable<Link> GetLinks(string schema, JObject entity);
    }
}
