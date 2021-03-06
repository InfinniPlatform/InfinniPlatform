﻿using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;
using InfinniPlatform.Types;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoDateBsonSerializerTest
    {
        [Test]
        public void ShouldSaveClassWithDate()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);
            var date3 = Date.Now.AddDays(3);

            var instance = new DateClassExample
                           {
                               _id = 1,
                               DateProperty = date1,
                               NullableDateProperty = date2,
                               ComplexProperty = new DateClassExample
                                                 {
                                                     DateProperty = date3
                                                 }
                           };

            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<DateClassExample>(nameof(ShouldSaveClassWithDate));
            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldSaveClassWithDate));

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
            Assert.AreEqual(date1.UnixTime, dynamicInstance["DateProperty"]);
            Assert.AreEqual(date2.UnixTime, dynamicInstance["NullableDateProperty"]);
            Assert.IsNotNull(dynamicInstance["ComplexProperty"]);
            Assert.AreEqual(date3.UnixTime, ((DynamicDocument)dynamicInstance["ComplexProperty"])["DateProperty"]);
            Assert.IsNull(((DynamicDocument)dynamicInstance["ComplexProperty"])["NullableDateProperty"]);
        }

        [Test]
        public void ShouldSaveDynamicDocumentWithDate()
        {
            // Given

            var date1 = Date.Now.AddDays(1);
            var date2 = Date.Now.AddDays(2);
            var date3 = Date.Now.AddDays(3);

            var instance = new DynamicDocument
                           {
                               { "_id", 1 },
                               { "DateProperty", date1 },
                               { "NullableDateProperty", date2 },
                               {
                                   "ComplexProperty", new DynamicDocument
                                                      {
                                                          { "DateProperty", date3 }
                                                      }
                               }
                           };

            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<DateClassExample>(nameof(ShouldSaveDynamicDocumentWithDate));
            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldSaveDynamicDocumentWithDate));

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
            Assert.AreEqual(date1.UnixTime, dynamicInstance["DateProperty"]);
            Assert.AreEqual(date2.UnixTime, dynamicInstance["NullableDateProperty"]);
            Assert.IsNotNull(dynamicInstance["ComplexProperty"]);
            Assert.AreEqual(date3.UnixTime, ((DynamicDocument)dynamicInstance["ComplexProperty"])["DateProperty"]);
            Assert.IsNull(((DynamicDocument)dynamicInstance["ComplexProperty"])["NullableDateProperty"]);
        }


        private class DateClassExample
        {
            public object _id { get; set; }

            public Date DateProperty { get; set; }

            public Date? NullableDateProperty { get; set; }

            public DateClassExample ComplexProperty { get; set; }
        }
    }
}