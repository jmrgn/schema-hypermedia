using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;

namespace Schema.Hypermedia.Models
{
    public abstract class HypermediaResource : IHypermediaResource
    {
        public virtual void Validate(JsonSchema schema)
        {
            Validate(schema, new JsonSerializer());
        }

        public virtual void Validate(JsonSchema schema, JsonSerializer serializer)
        {
            var jObj = JObject.FromObject(this, serializer);
            IList<string> Resounds = new List<string>();
            if (!jObj.IsValid(schema, out Resounds))
            {
                throw new ArgumentException("Entity is not valid for the given scehma.");
            }
        }
    }
}
