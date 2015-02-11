using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Schema.Hypermedia.Models;
using Newtonsoft.Json.Schema;
using Schema.Hypermedia.Test.Mocks;
using Schema.Hypermedia.Test.Utility;

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

            person = new Person
            {
                FamilyName = "Doe",
                GivenName = "John",
                HonorificPrefix = "Mr.",
                HonorificSuffix = "III",
                Id = "12345",
            };
            

            var helper = new FileHelper();
            rawSchema = helper.GetResourceTextFile("person-schema.json");
            var jObj = JsonConvert.DeserializeObject(rawSchema); 
            personSchema = JsonSchema.Parse(rawSchema);
            
            additional = new Dictionary<string, string>
            {
                {"{nonsense}", "still_nonsense"}
            };
        }

        [Test]
        public void ItShouldBeValidWithPropertyAttributes()
        {

            Assert.DoesNotThrow( () => generator.ValidateEntity(personSchema, person)); 
        }

        [Test]
        public void ItShouldBeValidWithCustomSerializer()
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            generator = new HypermediaGenerator(serializer);

            Assert.DoesNotThrow(() => generator.ValidateEntity(personSchema, person));
        }

        [Test]
        public void ItShouldNotBeValidWithoutPropertyAttributes()
        {
            var personNoAttr = new PersonNoAttr
            {
                FamilyName = "Doe",
                GivenName = "John",
                HonorificPrefix = "Mr.",
                HonorificSuffix = "III",
                Id = "12345",

            };
            Assert.That(() => generator.ValidateEntity(personSchema, personNoAttr), Throws.ArgumentException);
        }
        
        [Test, TestCaseSource("GetLinkTestData")]
        public void ItShouldGetLinksFromSchema(string schema, int expectedCount)
        {
            var links = generator.GetLinksFromSchema(schema);
            Assert.That(links.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void ItShouldEnrichALinkWithData()
        {
            string expected = "/v1/12345/John/Doe";
            var links = new List<Link>()
            { 
                new Link()
                {
                    Href = "/v1/{id}/{givenName}/{familyName}"
                }
            };
            generator.EnrichLinksWithData(person, links);
            Assert.That(links[0].Href, Is.EqualTo(expected));
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

        [TestCase("{id}")]
        [TestCase("{Id}")]
        [TestCase("{GivenName}")]
        [TestCase("{givenName}")]
        [TestCase("{honorificPrefix}")]
        [TestCase("{familyname}")]
        [TestCase("id")]
        public void ItShouldGetPropertiesByTemplateKey(string key)
        {
            var actual = generator.GetPropertyValue(key, person);
            Assert.That(actual, Is.Not.Null.Or.Empty);
        }

        [TestCase("{i d}")]
        [TestCase("{id}")]
        public void ItShoulHandleANonExistentTemplateKey(string key)
        {
            person.Id = null;
            Assert.That( () => generator.GetPropertyValue(key, person), Throws.ArgumentException);
        }

        [TestCase("{id}")]
        public void ItShoulHandleAnInvalidPropertyValue(string key)
        {
            person.Id = null;
            Assert.That(() => generator.GetPropertyValue(key, person), Throws.ArgumentException);
        }

        public static IEnumerable GetLinkTestData()
        {
            var helper = new FileHelper();
            yield return new TestCaseData(helper.GetResourceTextFile("person-schema.json"), 5);
            yield return new TestCaseData(helper.GetResourceTextFile("person-schema-no-links.json"), 0);
        }
    }
}
