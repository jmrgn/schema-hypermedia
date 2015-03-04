using NUnit.Framework;
using Schema.Hypermedia.Test.Mocks;

namespace Schema.Hypermedia.Test
{
    [TestFixture]
    public class ReflectionTests
    {
        private Person person;

        [SetUp]
        public void SetUp()
        {
            person = new Person()
            {
                FamilyName = "Doe",
                GivenName = "John",
                HonorificPrefix = "Mr.",
                HonorificSuffix = "III",
                Id = "12345",
            };
        }


        [TestCase("id")]
        [TestCase("Id")]
        [TestCase("GivenName")]
        [TestCase("givenName")]
        [TestCase("honorificPrefix")]
        [TestCase("familyname")]
        [TestCase("id")]
        public void ItShouldGetPropertiesByTemplateKey(string key)
        {
            var actual = Reflection.GetPropertyValue(key, person);
            Assert.That(actual, Is.Not.Null.Or.Empty);
        }

        [TestCase("i d")]
        [TestCase("doesNotExist")]
        public void ItShoulHandleANonExistentTemplateKey(string key)
        {
            Assert.That(() => Reflection.GetPropertyValue(key, person), Throws.ArgumentException);
        }

        [TestCase("id")]
        public void ItShoulHandleAnInvalidPropertyValue(string key)
        {
            person.Id = null;
            Assert.That(() => Reflection.GetPropertyValue(key, person), Throws.ArgumentException);
        }
    }
}
