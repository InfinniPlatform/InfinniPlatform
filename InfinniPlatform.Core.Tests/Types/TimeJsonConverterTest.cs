using System.Globalization;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.Core.Abstractions.Types;
using InfinniPlatform.Core.Serialization;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Types
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class TimeJsonConverterTest
    {
        [Test]
        public void ShouldSerializeClass()
        {
            // Given

            var time1 = Time.Now.AddSeconds(1.456);
            var time2 = Time.Now.AddSeconds(2.789);
            var time3 = Time.Now.AddSeconds(3.012);

            var serializer = JsonObjectSerializer.Default;

            var instance = new TimeClassExample
            {
                TimeProperty = time1,
                NullableTimeProperty = time2,
                ComplexProperty = new TimeClassExample
                {
                    TimeProperty = time3
                }
            };

            var expectedJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"NullableTimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time3.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) +
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

            var time1 = Time.Now.AddSeconds(1.456);
            var time2 = Time.Now.AddSeconds(2.789);
            var time3 = Time.Now.AddSeconds(3.012);

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"NullableTimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time3.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"NullableTimeProperty\":null" +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<TimeClassExample>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(time1, instance.TimeProperty);
            Assert.AreEqual(time2, instance.NullableTimeProperty);
            Assert.IsNotNull(instance.ComplexProperty);
            Assert.AreEqual(time3, instance.ComplexProperty.TimeProperty);
            Assert.IsNull(instance.ComplexProperty.NullableTimeProperty);
        }

        [Test]
        public void ShouldSerializeDynamicWrapper()
        {
            // Given

            var time1 = Time.Now.AddSeconds(1.456);
            var time2 = Time.Now.AddSeconds(2.789);
            var time3 = Time.Now.AddSeconds(3.012);

            var serializer = JsonObjectSerializer.Default;

            var instance = new DynamicWrapper
                           {
                               { "TimeProperty", time1 },
                               { "NullableTimeProperty", time2 },
                               {
                                   "ComplexProperty", new DynamicWrapper
                                                      {
                                                          { "TimeProperty", time3 }
                                                      }
                               }
                           };

            var expectedJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"NullableTimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time3.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) +
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

            var time1 = Time.Now.AddSeconds(1.456);
            var time2 = Time.Now.AddSeconds(2.789);
            var time3 = Time.Now.AddSeconds(3.012);

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"NullableTimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time3.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"NullableTimeProperty\":null" +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<DynamicWrapper>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(time1.TotalSeconds, instance["TimeProperty"]);
            Assert.AreEqual(time2.TotalSeconds, instance["NullableTimeProperty"]);
            Assert.IsNotNull(instance["ComplexProperty"]);
            Assert.AreEqual(time3.TotalSeconds, ((DynamicWrapper)instance["ComplexProperty"])["TimeProperty"]);
            Assert.IsNull(((DynamicWrapper)instance["ComplexProperty"])["NullableTimeProperty"]);
        }


        private class TimeClassExample
        {
            public Time TimeProperty { get; set; }

            public Time? NullableTimeProperty { get; set; }

            public TimeClassExample ComplexProperty { get; set; }
        }
    }
}