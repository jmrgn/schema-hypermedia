﻿using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Schema.Hypermedia.Interfaces;
using Schema.Hypermedia.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Schema.Hypermedia
{
    public class HypermediaGenerator : IHypermediaGenerator
    {
        private Dictionary<string, string> valueCache;
        private const string LeftDelim = "{";
        private const string RightDelim = "}";
        private const string RegExp = LeftDelim + @"([^\?].*?)" + RightDelim;
        public JsonSerializer Serializer { get; set; }
        public InspectionBehavior InspectionBehavior { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="behavior"></param>
        public HypermediaGenerator(InspectionBehavior behavior = InspectionBehavior.Strict)
        {
            Serializer = new JsonSerializer();
            valueCache = new Dictionary<string, string>();
            this.InspectionBehavior = behavior;
        }

        /// <summary>
        /// Constructor allowing for adding additional parameterized data
        /// </summary>
        /// <param name="additionalData"></param>
        /// <param name="behavior"></param>
        public HypermediaGenerator(Dictionary<string, string> additionalData, InspectionBehavior behavior = InspectionBehavior.Strict)
        {
            this.valueCache = additionalData;
            this.Serializer = new JsonSerializer();
            this.InspectionBehavior = behavior;
        }

        /// <summary>
        /// Constructor allowing for a custom Json serializer to be used
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="behavior"></param>
        public HypermediaGenerator(JsonSerializer serializer, InspectionBehavior behavior = InspectionBehavior.Strict)
        {
            this.Serializer = serializer;
            valueCache = new Dictionary<string, string>();
            this.InspectionBehavior = behavior;
        }

        /// <summary>
        /// Constructor allowing for a custom serializer and additional parameterized data
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="additionalData"></param>
        /// <param name="behavior"></param>
        public HypermediaGenerator(JsonSerializer serializer, Dictionary<string, string> additionalData, InspectionBehavior behavior = InspectionBehavior.Strict)
        {
            this.Serializer = serializer;
            this.valueCache = additionalData;
            this.InspectionBehavior = behavior;
        }

        /// <summary>
        /// Given an entity and its matching json schema, generate links from templates defined in the schema
        /// using properties of the entity. 
        /// </summary>
        /// <param name="schema">The schema json in string form</param>
        /// <param name="entity">The entity for which the utility generates links for</param>
        /// <returns></returns>
        public IEnumerable<Link> GetLinks(string schema, IHypermediaResource entity)
        {
            var parsedSchema = JsonSchema.Parse(schema);
            entity.Validate(parsedSchema, Serializer);
            var links = GetLinksFromSchema(schema);
            
            EnrichLinksWithData(entity, links);
            return links;
        }

        protected internal IList<Link> EnrichLinksWithData(IHypermediaResource entity, IList<Link> links)
        {
            var enriched = new List<Link>();
            foreach (var link in links)
            {
                var matches = Regex.Matches(link.Href, RegExp)
                    .Cast<Match>()
                    .Select(m => m.Value)
                    .ToList();
                try
                {
                    foreach (var match in matches)
                    {
                        var replacement = string.Empty;

                        // Get and use values from cache if possible
                        if (valueCache.ContainsKey(match))
                        {
                            replacement = valueCache[match];
                        }
                        else if (valueCache.ContainsKey(GetParamName(match)))
                        {
                            replacement = valueCache[GetParamName(match)];
                        }
                        else
                        {
                            replacement = Reflection.GetPropertyValue(GetParamName(match), entity);
                            valueCache.Add(match, replacement);
                        }
                        link.Href = link.Href.Replace(match, replacement);
                        
                    }
                    enriched.Add(link);
                }
                catch (ArgumentException)
                {
                    if (InspectionBehavior == InspectionBehavior.Strict)
                    {
                        throw;
                    }
                }
            }
            return enriched;
        }

        private string GetParamName(string match)
        {
            var propName = match.Replace(LeftDelim, string.Empty).Replace(RightDelim, string.Empty);
            return propName;
        }

        protected internal IList<Link> GetLinksFromSchema(string schema)
        {
            var hyperSchema = JsonConvert.DeserializeObject<HypermediaSchema>(schema);
            return hyperSchema.Links;
        }
    }
}
