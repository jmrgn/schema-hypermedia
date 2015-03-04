using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Schema.Hypermedia.Models
{
    public abstract class HypermediaResource : IHypermediaResource
    {
        public IEnumerable<Link> Links { get; set; }

        public void Validate(JsonSchema schema)
        {
            Validate(schema, new JsonSerializer());
        }

        public virtual void Validate(JsonSchema schema, JsonSerializer serializer)
        {
            var jObj = JObject.FromObject(this, serializer);
            IList<string> reasons = new List<string>();
            if (!jObj.IsValid(schema, out reasons))
            {
                var builder = new StringBuilder("Entity is not valid for the given scehma. Reasons: ");
                string delim = "";
                foreach (var reason in reasons)
                {
                    builder.Append(delim).Append(reason);
                    delim = ", ";
                }
                throw new ArgumentException(builder.ToString());
            }
        }
    }
}
