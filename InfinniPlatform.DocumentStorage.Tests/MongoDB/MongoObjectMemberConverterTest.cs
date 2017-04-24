using System.Collections;
using System.Linq;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoObjectMemberConverterTest
    {
        [Test]
        public void ShouldDeserializeObjectPropertiesAsDynamicDocument()
        {
            // Given

            object id = 123;

            var dynamicDocument = new DynamicDocument
                                  {
                                      { "_id", id },
                                      { "PropertyInt", 111 },
                                      { "PropertyScalar", 222 },
                                      { "PropertyObject", new DynamicDocument { { "Property1", 333 }, { "Property2", "Hello!" } } },
                                      { "PropertyArray", new[] { 1, 2, 3 } }
                                  };

            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldDeserializeObjectPropertiesAsDynamicDocument));
            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<SomeClass>(nameof(ShouldDeserializeObjectPropertiesAsDynamicDocument));

            // When

            dynamicStorage.InsertOne(dynamicDocument);

            var typedDocument = classStorage.Find(i => i._id == id).FirstOrDefault();

            // Then

            Assert.IsNotNull(typedDocument);

            // Strong type
            Assert.AreEqual(111, typedDocument.PropertyInt);

            // Scalar type
            Assert.AreEqual(222, typedDocument.PropertyScalar);

            // Object type
            Assert.IsInstanceOf<DynamicDocument>(typedDocument.PropertyObject);
            var propertyObject = (DynamicDocument)typedDocument.PropertyObject;
            Assert.AreEqual(333, propertyObject["Property1"]);
            Assert.AreEqual("Hello!", propertyObject["Property2"]);

            // Array type
            Assert.IsInstanceOf<IEnumerable>(typedDocument.PropertyArray);
            var propertyArray = ((IEnumerable)typedDocument.PropertyArray).Cast<object>().ToArray();
            Assert.AreEqual(3, propertyArray.Length);
            Assert.AreEqual(1, propertyArray[0]);
            Assert.AreEqual(2, propertyArray[1]);
            Assert.AreEqual(3, propertyArray[2]);
        }


        // ReSharper disable UnusedAutoPropertyAccessor.Local
        // ReSharper disable InconsistentNaming

        private class SomeClass
        {
            public object _id { get; set; }

            public int PropertyInt { get; set; }

            public object PropertyScalar { get; set; }

            public object PropertyObject { get; set; }

            public object PropertyArray { get; set; }
        }

        // ReSharper restore InconsistentNaming
        // ReSharper restore UnusedAutoPropertyAccessor.Local
    }
}