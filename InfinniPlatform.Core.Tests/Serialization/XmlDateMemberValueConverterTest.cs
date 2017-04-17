using System;
using System.Xml.Serialization;

using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.Core.Abstractions.Types;
using InfinniPlatform.Core.Serialization;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Serialization
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class XmlDateMemberValueConverterTest
    {
        [Test]
        public void ShouldSerialize()
        {
            // Given

            var date1 = DateTime.Today.AddDays(1);
            var date2 = DateTime.Today.AddDays(2);
            var date3 = DateTime.Today.AddDays(3);

            var serializer = new JsonObjectSerializer(false, valueConverters: new[] { new XmlDateMemberValueConverter() });

            var instance = new XmlDateClass
            {
                DateProperty = date1,
                NullableDateProperty = date2,
                ComplexProperty = new XmlDateClass
                {
                    DateProperty = date3
                }
            };

            var expectedJson = @"{" +
                               "\"DateProperty\":" + ((Date)date1).UnixTime + "," +
                               "\"NullableDateProperty\":" + ((Date)date2).UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + ((Date)date3).UnixTime +
                               "}" +
                               "}";

            // When

            var actualJson = serializer.ConvertToString(instance);

            // Then
            Assert.AreEqual(expectedJson, actualJson);
        }

        [Test]
        public void ShouldDeserialize()
        {
            // Given

            var date1 = DateTime.Today.AddDays(1);
            var date2 = DateTime.Today.AddDays(2);
            var date3 = DateTime.Today.AddDays(3);

            var serializer = new JsonObjectSerializer(false, valueConverters: new[] { new XmlDateMemberValueConverter() });

            var instanceJson = @"{" +
                               "\"DateProperty\":" + ((Date)date1).UnixTime + "," +
                               "\"NullableDateProperty\":" + ((Date)date2).UnixTime + "," +
                               "\"ComplexProperty\":{" +
                               "\"DateProperty\":" + ((Date)date3).UnixTime +
                               "}" +
                               "}";

            // When

            var instance = serializer.Deserialize<XmlDateClass>(instanceJson);

            // Then
            Assert.IsNotNull(instance);
            Assert.AreEqual(date1, instance.DateProperty);
            Assert.AreEqual(date2, instance.NullableDateProperty);
            Assert.IsNotNull(instance.ComplexProperty);
            Assert.AreEqual(date3, instance.ComplexProperty.DateProperty);
            Assert.IsNull(instance.ComplexProperty.NullableDateProperty);
        }


        private class XmlDateClass
        {
            [XmlElement(DataType = "date")]
            public DateTime DateProperty { get; set; }

            [XmlElement(DataType = "date")]
            public DateTime? NullableDateProperty { get; set; }

            public XmlDateClass ComplexProperty { get; set; }
        }
    }
}