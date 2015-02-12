using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Schema.Hypermedia.Models;
using Schema.Hypermedia.Test.Mocks;
using Schema.Hypermedia.Test.Utility;

namespace Schema.Hypermedia.Test.Models
{
    [TestFixture]
    public class HypermediaResourceTests
    {
        private IHypermediaResource resource;
        private JsonSchema personSchema;

        [SetUp]
        public void SetUp()
        {
            resource = new Person
            {
                FamilyName = "Doe",
                GivenName = "John",
                HonorificPrefix = "Mr.",
                HonorificSuffix = "III",
                Id = "12345",
            };

            var helper = new FileHelper();
            var rawSchema = helper.GetResourceTextFile("person-schema.json");
            personSchema = JsonSchema.Parse(rawSchema);
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
            Assert.That(() => personNoAttr.Validate(personSchema), Throws.ArgumentException);
        }

        [Test]
        public void ItShouldBeValidWithPropertyAttributes()
        {
            Assert.DoesNotThrow(() => resource.Validate(personSchema));
        }

        [Test]
        public void ItShouldBeValidWithCustomSerializer()
        {
            var serializer = new JsonSerializer
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            Assert.DoesNotThrow(() => resource.Validate(personSchema, serializer));
        }
    }
}
