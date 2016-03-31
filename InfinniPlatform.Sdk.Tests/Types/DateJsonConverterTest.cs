using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Types;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests.Types
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class DateJsonConverterTest
    {
        [Test]
        public void ShouldSerializeClass()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);

            var serializer = JsonObjectSerializer.Default;

            var instance = new DateClassExample
            {
                DateProperty = date1,
                ComplexProperty = new DateClassExample
                {
                    DateProperty = date2
                }
            };

            var expectedJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date2.UnixTime +
                               "}" +
                               "}";

            // When

            var actualJson = serializer.ConvertToString(instance);

            // Then
            Assert.AreEqual(expectedJson, actualJson);
        }

        [Test]
        public void ShouldDeserializeClass()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date2.UnixTime +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<DateClassExample>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(date1, instance.DateProperty);
            Assert.IsNotNull(instance.ComplexProperty);
            Assert.AreEqual(date2, instance.ComplexProperty.DateProperty);
        }

        [Test]
        public void ShouldSerializeDynamicWrapper()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);

            var serializer = JsonObjectSerializer.Default;

            var instance = new DynamicWrapper
                           {
                               { "DateProperty", date1 },
                               {
                                   "ComplexProperty", new DynamicWrapper
                                                      {
                                                          { "DateProperty", date2 }
                                                      }
                               }
                           };

            var expectedJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date2.UnixTime +
                               "}" +
                               "}";

            // When

            var actualJson = serializer.ConvertToString(instance);

            // Then
            Assert.AreEqual(expectedJson, actualJson);
        }

        [Test]
        public void ShouldDeserializeDynamicWrapper()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date2.UnixTime +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<DynamicWrapper>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(date1.UnixTime, instance["DateProperty"]);
            Assert.IsNotNull(instance["ComplexProperty"]);
            Assert.AreEqual(date2.UnixTime, ((DynamicWrapper)instance["ComplexProperty"])["DateProperty"]);
        }


        private class DateClassExample
        {
            public Date DateProperty { get; set; }

            public DateClassExample ComplexProperty { get; set; }
        }
    }
}