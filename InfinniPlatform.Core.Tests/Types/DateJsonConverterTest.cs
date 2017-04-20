using InfinniPlatform.Dynamic;
using InfinniPlatform.Serialization;

using NUnit.Framework;

namespace InfinniPlatform.Types
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
            var date3 = Date.Now.AddDays(3);

            var serializer = JsonObjectSerializer.Default;

            var instance = new DateClassExample
            {
                DateProperty = date1,
                NullableDateProperty = date2,
                ComplexProperty = new DateClassExample
                {
                    DateProperty = date3
                }
            };

            var expectedJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"NullableDateProperty\":" + date2.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date3.UnixTime +
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
            var date3 = Date.Now.AddDays(3);

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"NullableDateProperty\":" + date2.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date3.UnixTime + "," +
                               "\"NullableDateProperty\":null" +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<DateClassExample>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(date1, instance.DateProperty);
            Assert.AreEqual(date2, instance.NullableDateProperty);
            Assert.IsNotNull(instance.ComplexProperty);
            Assert.AreEqual(date3, instance.ComplexProperty.DateProperty);
            Assert.IsNull(instance.ComplexProperty.NullableDateProperty);
        }

        [Test]
        public void ShouldSerializeDynamicWrapper()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);
            var date3 = Date.Now.AddDays(3);

            var serializer = JsonObjectSerializer.Default;

            var instance = new DynamicWrapper
                           {
                               { "DateProperty", date1 },
                               { "NullableDateProperty", date2 },
                               {
                                   "ComplexProperty", new DynamicWrapper
                                                      {
                                                          { "DateProperty", date3 }
                                                      }
                               }
                           };

            var expectedJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"NullableDateProperty\":" + date2.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date3.UnixTime +
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
            var date3 = Date.Now.AddDays(3);

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"DateProperty\":" + date1.UnixTime + "," +
                               "\"NullableDateProperty\":" + date2.UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + date3.UnixTime + "," +
                               "\"NullableDateProperty\":null" +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<DynamicWrapper>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(date1.UnixTime, instance["DateProperty"]);
            Assert.AreEqual(date2.UnixTime, instance["NullableDateProperty"]);
            Assert.IsNotNull(instance["ComplexProperty"]);
            Assert.AreEqual(date3.UnixTime, ((DynamicWrapper)instance["ComplexProperty"])["DateProperty"]);
            Assert.IsNull(((DynamicWrapper)instance["ComplexProperty"])["NullableDateProperty"]);
        }


        private class DateClassExample
        {
            public Date DateProperty { get; set; }

            public Date? NullableDateProperty { get; set; }

            public DateClassExample ComplexProperty { get; set; }
        }
    }
}