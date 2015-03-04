using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Schema.Hypermedia.Models;

namespace Schema.Hypermedia
{
    public interface IHypermediaResource
    {
        IEnumerable<Link> Links { get; set; }
        void Validate(JsonSchema schema);
        void Validate(JsonSchema schema, JsonSerializer serializer);
    }
}
