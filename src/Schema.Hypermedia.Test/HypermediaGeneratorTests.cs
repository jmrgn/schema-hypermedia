using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Schema.Hypermedia.Models;
using Schema.Hypermedia.Test.Mocks;
using Schema.Hypermedia.Test.Utility;
using System.Collections;
using System.Collections.Generic;

namespace Schema.Hypermedia.Test
{
    [TestFixture]
    public class HypermediaGeneratorTests
    {
        private Link self;
        private Link parent;
        private JsonSchema personSchema;
        private string rawSchema;
        private HypermediaGenerator generator;
        private Person person;
        private Dictionary<string, string> additional;

        [SetUp]
        public void SetUp()
        {
            generator = new HypermediaGenerator();
            var helper = new FileHelper();
            rawSchema = helper.GetResourceTextFile("person-schema.json");
            personSchema = JsonSchema.Parse(rawSchema);

            self = new Link()
            {
                Href = "/baseApiUrl/persons/12345",
                Rel = "self",
                Title = "Self",
                Method = "Get"
            };

            parent = new Link()
            {
                Href = "/baseApiUrl/persons",
                Rel = "self",
                Title = "Self",
                Method = "Get"
            };

            person = new Person(personSchema)
            {
                FamilyName = "Doe",
                GivenName = "John",
                HonorificPrefix = "Mr.",
                HonorificSuffix = "III",
                Id = "12345",
            };
            
            additional = new Dictionary<string, string>
            {
                {"{nonsense}", "still_nonsense"},
                {"{test}", "testing"},
                {"nobraces", "no-braces"}
            };
        }

        [Test, TestCaseSource("GetLinkTestData")]
        public void ItShouldGetLinksFromSchema(string schema, int expectedCount)
        {
            var links = generator.GetLinksFromSchema(schema);
            Assert.That(links.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void ItShouldGenerateLinks()
        {
            generator = new HypermediaGenerator(additional);
            var links = generator.GetLinks(rawSchema, person);

            Assert.That(links.Count(), Is.EqualTo(5));
            var getLink = links.First(l => l.Rel == "self");
            var subResourceLink = links.First(l => l.Rel == "hypothetical-subresource");
            Assert.That(getLink.Href, Is.EqualTo("/baseApiUrl/persons/12345"));
            Assert.That(subResourceLink.Href, Is.EqualTo("/baseApiUrl/persons/12345/honorific/III/testing/no-braces"));

        }

        [Test]
        public void ItShouldEnrichLinksWithData()
        {
            var expected = new  [] {"/v1/12345/John", "/v1/12345/John/Doe/Mr./III", "/v1/12345/John/Doe"};
           
            var links = new List<Link>()
            { 
                new Link()
                {
                    Href = "/v1/{id}/{givenName}"
                },
                new Link()
                {
                    Href = "/v1/{id}/{givenName}/{familyName}/{honorificPrefix}/{honorificSuffix}"
                },
                new Link()
                {
                    Href = "/v1/{id}/{givenName}/{familyName}"
                }
            };
            generator.EnrichLinksWithData(person, links);
            Assert.That(links[0].Href, Is.EqualTo(expected[0]));
            Assert.That(links[1].Href, Is.EqualTo(expected[1]));
            Assert.That(links[2].Href, Is.EqualTo(expected[2]));
        }

        [Test]
        public void ItShouldEnrichLinksForJObjects()
        {
            var expected = new[] { "/v1/12345/John", "/v1/12345/John/Doe/Mr./III", "/v1/12345/John/Doe" };
            generator = new HypermediaGenerator();

            var json =
                JObject.Parse(@"{""id"": ""12345"", ""givenName"": ""John"", ""familyName"": ""Doe"", ""honorificPrefix"": ""Mr."", ""honorificSuffix"": ""III""}");

            var links = new List<Link>()
            { 
                new Link()
                {
                    Href = "/v1/{id}/{givenName}"
                },
                new Link()
                {
                    Href = "/v1/{id}/{givenName}/{familyName}/{honorificPrefix}/{honorificSuffix}"
                },
                new Link()
                {
                    Href = "/v1/{id}/{givenName}/{familyName}"
                }
            };

            generator.EnrichLinksWithData(json, links);
            Assert.That(links[0].Href, Is.EqualTo(expected[0]));
            Assert.That(links[1].Href, Is.EqualTo(expected[1]));
            Assert.That(links[2].Href, Is.EqualTo(expected[2]));
        }

        [TestCase("/v1/valid")]
        [TestCase("/v1/valid{?id,familyName}")]
        public void ItShouldALinkRequiringNoSubstitution(string expected)
        {
            var links = new List<Link>()
            { 
                new Link()
                {
                    Href = expected
                }
            };
            generator.EnrichLinksWithData(person, links);
            Assert.That(links[0].Href, Is.EqualTo(expected));
        }

        [TestCase("/v1/valid/{notavalidproperty}")]
        [TestCase("/v1/valid/{familyName}")]
        public void ItShouldNotReturnLinksWithInvalidDataIfBehaviorIsLoose(string expected)
        {
            person.FamilyName = null;
            var links = new List<Link>()
            { 
                new Link()
                {
                    Href = expected
                }
            };
            generator = new HypermediaGenerator(additional)
            {
                InspectionBehavior = InspectionBehavior.Loose
            };
            var result = generator.EnrichLinksWithData(person, links);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void ItShouldEnrichALinkWithAdditionalData()
        {
            generator = new HypermediaGenerator(additional);
            string expected = "/v1/still_nonsense/12345/John/Doe";
            var links = new List<Link>()
            { 
                new Link()
                {
                    Href = "/v1/{nonsense}/{id}/{givenName}/{familyName}"
                }
            };
            generator.EnrichLinksWithData(person, links);
            Assert.That(links[0].Href, Is.EqualTo(expected));
        }

        public static IEnumerable GetLinkTestData()
        {
            var helper = new FileHelper();
            yield return new TestCaseData(helper.GetResourceTextFile("person-schema.json"), 5);
            yield return new TestCaseData(helper.GetResourceTextFile("person-schema-no-links.json"), 0);
        }
    }
}
