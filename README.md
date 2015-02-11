# schema-hypermedia
A lightweight library for using Json Hyperschemas to generate hypermedia links for enriching REST API Responses.

### Usage

Any resource must have a corresponding [Json Hyper-Schema (v4)] (http://tools.ietf.org/html/draft-luff-json-hyper-schema-00)  with a `links` section denoting valid hyperlink templates.

Entities are expected to implement the interface `IHypermediaResource`, an empty Interface acting as a type constraint. The generator uses `JSON.NET` to validate the resource against the schema string provided, using `JSON.NET's` default `JsonSerializer`. The ability to override this serializer is provided via a parameterized constructor or by use of a public property.

#### Example
```CSharp
public class Person : IHypermediaResource
{
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("familyName")]
        public string FamilyName { get; set; }

        [JsonProperty("honorificPrefix")]
        public string HonorificPrefix{ get; set; }

        [JsonProperty("honorificSuffix")]
        public string HonorificSuffix { get; set; }
}
```

An alternative is to provide a different `JsonSerializer` to suit different needs. In order to resolve the casing issue in the example above, a new serializer could be used to ensure camel casing:

```Csharp
    var serializer = new JsonSerializer
     {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
     };
```

By convention, entities are expected to implement properties matching those defined in the JSON Schema. Casing issues can be resolved via use of the JsonProperty  Hyperlinks are expected to follow the standard curly-bracketed template format to denote route-parameter in the template that should be replaced with a matching property value. `{propertyname}`

#### Example Schema
```json
{
  "$schema": "http://json-schema.org/draft-03/schema#",
  "title": "Person",
  "type": "object",
  "properties": {
    "id": {
      "title": "Person Id",
      "description": "Unique identifier for a person",
      "type": "string",
      "required": true
    },
    "givenName": {
      "title": "Given Name",
      "description": "A person's given name",
      "type": "string",
      "required": true
    },
    "familyName": {
      "title": "Family Name",
      "description": "A person's Family Name",
      "type": "string",
      "required": true
    },
    "honorificPrefix": {
        "title": "Family Name",
        "description": "A  person's honorific prefix (e.g. Jr, M.D.)",
        "type": "string",
        "required": true
    },
    "honorificSuffix": {
        "title": "Honorific Suffix",
        "description": "A person's honorific suffix (e.g. Jr, M.D.)",
        "type": "string",
        "required": true
    }

  },
  "links": [
    {
        "rel": "collection",
        "title": "View all persons",
        "href": "/baseApiUrl/persons",
        "method": "GET"
    },
    {
        "rel": "parent",
        "title": "View all persons",
        "href": "/baseApiUrl/persons",
        "method": "GET"
    },
    {
        "rel": "self",
        "title": "get",
        "href": "/baseApiUrl/persons/{id}",
        "method": "POST"
    },
    {
        "rel": "hypothetical-subresource",
        "title": "get",
        "href": "/baseApiUrl/persons/{id}/honorific/{test}",
        "method": "POST",
        "schema":  ""
    },
    {
      "encType": "application/x-www-form-urlencoded",
      "method": "POST",
      "rel": "create-form",
      "title": "Create a new person",
      "href": "/baseApiUrl/persons"
    }
  ]
}
```

The utility will inspect the schema string, ensure the given entity is valid, and generate a list of links based on a combination of Link templates and entity data. The constructor parameter `additionalData` is an optional `Dictionary<string,string>` that allows for additional key/value data to be provided in case a specific link template contains a parameter whose value is not encapsulated by the entity provided. 

