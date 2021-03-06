﻿using System;
using System.Xml.Serialization;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Serialization;
using InfinniPlatform.Tests;
using InfinniPlatform.Types;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoXmlDateMemberValueConverterTest
    {
        [Test]
        public void ShouldSaveClassWithXmlDate()
        {
            // Given

            var date1 = DateTime.Today.AddDays(1);
            var date2 = DateTime.Today.AddDays(2);
            var date3 = DateTime.Today.AddDays(3);

            var instance = new XmlDateClass
                           {
                               _id = 1,
                               DateProperty = date1,
                               NullableDateProperty = date2,
                               ComplexProperty = new XmlDateClass
                                                 {
                                                     DateProperty = date3
                                                 }
                           };

            var converters = new IMemberValueConverter[] { new XmlDateMemberValueConverter() };
            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<XmlDateClass>(nameof(ShouldSaveClassWithXmlDate), converters);
            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldSaveClassWithXmlDate), converters);

            // When

            classStorage.InsertOne(instance);

            var classInstance = classStorage.Find().FirstOrDefault();
            var dynamicInstance = dynamicStorage.Find().FirstOrDefault();

            // Then

            Assert.IsNotNull(classInstance);
            Assert.AreEqual(date1, classInstance.DateProperty);
            Assert.AreEqual(date2, classInstance.NullableDateProperty);
            Assert.IsNotNull(classInstance.ComplexProperty);
            Assert.AreEqual(date3, classInstance.ComplexProperty.DateProperty);
            Assert.IsNull(classInstance.ComplexProperty.NullableDateProperty);

            Assert.IsNotNull(dynamicInstance);
            Assert.AreEqual(((Date)date1).UnixTime, dynamicInstance["DateProperty"]);
            Assert.AreEqual(((Date)date2).UnixTime, dynamicInstance["NullableDateProperty"]);
            Assert.IsNotNull(dynamicInstance["ComplexProperty"]);
            Assert.AreEqual(((Date)date3).UnixTime, ((DynamicDocument)dynamicInstance["ComplexProperty"])["DateProperty"]);
            Assert.IsNull(((DynamicDocument)dynamicInstance["ComplexProperty"])["NullableDateProperty"]);
        }

        [Test]
        public void ShouldSaveDynamicDocumentWithXmlDate()
        {
            // Given

            var date1 = DateTime.Today.AddDays(1);
            var date2 = DateTime.Today.AddDays(2);
            var date3 = DateTime.Today.AddDays(3);

            var instance = new DynamicDocument
                           {
                               { "_id", 1 },
                               { "DateProperty", (Date)date1 },
                               { "NullableDateProperty", (Date)date2 },
                               {
                                   "ComplexProperty", new DynamicDocument
                                                      {
                                                          { "DateProperty", (Date)date3 }
                                                      }
                               }
                           };

            var converters = new IMemberValueConverter[] { new XmlDateMemberValueConverter() };
            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<XmlDateClass>(nameof(ShouldSaveDynamicDocumentWithXmlDate), converters);
            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldSaveDynamicDocumentWithXmlDate), converters);

            // When

            dynamicStorage.InsertOne(instance);

            var classInstance = classStorage.Find().FirstOrDefault();
            var dynamicInstance = dynamicStorage.Find().FirstOrDefault();

            // Then

            Assert.IsNotNull(classInstance);
            Assert.AreEqual(date1, classInstance.DateProperty);
            Assert.AreEqual(date2, classInstance.NullableDateProperty);
            Assert.IsNotNull(classInstance.ComplexProperty);
            Assert.AreEqual(date3, classInstance.ComplexProperty.DateProperty);
            Assert.IsNull(classInstance.ComplexProperty.NullableDateProperty);

            Assert.IsNotNull(dynamicInstance);
            Assert.AreEqual(((Date)date1).UnixTime, dynamicInstance["DateProperty"]);
            Assert.AreEqual(((Date)date2).UnixTime, dynamicInstance["NullableDateProperty"]);
            Assert.IsNotNull(dynamicInstance["ComplexProperty"]);
            Assert.AreEqual(((Date)date3).UnixTime, ((DynamicDocument)dynamicInstance["ComplexProperty"])["DateProperty"]);
            Assert.IsNull(((DynamicDocument)dynamicInstance["ComplexProperty"])["NullableDateProperty"]);
        }


        private class XmlDateClass
        {
            public object _id { get; set; }

            [XmlElement(DataType = "date")]
            public DateTime DateProperty { get; set; }

            [XmlElement(DataType = "date")]
            public DateTime? NullableDateProperty { get; set; }

            public XmlDateClass ComplexProperty { get; set; }
        }
    }
}