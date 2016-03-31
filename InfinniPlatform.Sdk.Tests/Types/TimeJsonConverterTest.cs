using System.Globalization;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Types;

using NUnit.Framework;

namespace InfinniPlatform.Sdk.Tests.Types
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

            var serializer = JsonObjectSerializer.Default;

            var instance = new TimeClassExample
            {
                TimeProperty = time1,
                ComplexProperty = new TimeClassExample
                {
                    TimeProperty = time2
                }
            };

            var expectedJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) +
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

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<TimeClassExample>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(time1, instance.TimeProperty);
            Assert.IsNotNull(instance.ComplexProperty);
            Assert.AreEqual(time2, instance.ComplexProperty.TimeProperty);
        }

        [Test]
        public void ShouldSerializeDynamicWrapper()
        {
            // Given

            var time1 = Time.Now.AddSeconds(1.456);
            var time2 = Time.Now.AddSeconds(2.789);

            var serializer = JsonObjectSerializer.Default;

            var instance = new DynamicWrapper
                           {
                               { "TimeProperty", time1 },
                               {
                                   "ComplexProperty", new DynamicWrapper
                                                      {
                                                          { "TimeProperty", time2 }
                                                      }
                               }
                           };

            var expectedJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) +
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

            var serializer = JsonObjectSerializer.Default;

            var instanceJson = @"{" +
                               "\"TimeProperty\":" + time1.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) + "," +
                               "\"ComplexProperty\":{" +
                               "\"TimeProperty\":" + time2.TotalSeconds.ToString("r", CultureInfo.InvariantCulture) +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<DynamicWrapper>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(time1.TotalSeconds, instance["TimeProperty"]);
            Assert.IsNotNull(instance["ComplexProperty"]);
            Assert.AreEqual(time2.TotalSeconds, ((DynamicWrapper)instance["ComplexProperty"])["TimeProperty"]);
        }


        private class TimeClassExample
        {
            public Time TimeProperty { get; set; }

            public TimeClassExample ComplexProperty { get; set; }
        }
    }
}