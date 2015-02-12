
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;

namespace Schema.Hypermedia
{
    public interface IHypermediaResource
    {
        void Validate(JsonSchema schema);
        void Validate(JsonSchema schema, JsonSerializer serializer);
    }
}
