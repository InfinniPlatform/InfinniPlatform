using InfinniPlatform.Dynamic;
using InfinniPlatform.Types;

using NUnit.Framework;

namespace InfinniPlatform.DocumentStorage.MongoDB
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MongoTimeBsonSerializerTest
    {
        [Test]
        public void ShouldSaveClassWithTime()
        {
            // Given

            var time1 = Time.Now.AddDays(1);
            var time2 = Time.Now.AddDays(2);
            var time3 = Time.Now.AddDays(3);

            var instance = new TimeClassExample
                           {
                               _id = 1,
                               TimeProperty = time1,
                               NullableTimeProperty = time2,
                               ComplexProperty = new TimeClassExample
                                                 {
                                                     TimeProperty = time3
                                                 }
                           };

            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<TimeClassExample>(nameof(ShouldSaveClassWithTime));
            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldSaveClassWithTime));

            // When

            classStorage.InsertOne(instance);

            var classInstance = classStorage.Find().FirstOrDefault();
            var dynamicInstance = dynamicStorage.Find().FirstOrDefault();

            // Then

            Assert.IsNotNull(classInstance);
            Assert.AreEqual(time1, classInstance.TimeProperty);
            Assert.AreEqual(time2, classInstance.NullableTimeProperty);
            Assert.IsNotNull(classInstance.ComplexProperty);
            Assert.AreEqual(time3, classInstance.ComplexProperty.TimeProperty);
            Assert.IsNull(classInstance.ComplexProperty.NullableTimeProperty);

            Assert.IsNotNull(dynamicInstance);
            Assert.AreEqual(time1.TotalSeconds, dynamicInstance["TimeProperty"]);
            Assert.AreEqual(time2.TotalSeconds, dynamicInstance["NullableTimeProperty"]);
            Assert.IsNotNull(dynamicInstance["ComplexProperty"]);
            Assert.AreEqual(time3.TotalSeconds, ((DynamicWrapper)dynamicInstance["ComplexProperty"])["TimeProperty"]);
            Assert.IsNull(((DynamicWrapper)dynamicInstance["ComplexProperty"])["NullableTimeProperty"]);
        }

        [Test]
        public void ShouldSaveDynamicWrapperWithTime()
        {
            // Given

            var time1 = Time.Now.AddDays(1);
            var time2 = Time.Now.AddDays(2);
            var time3 = Time.Now.AddDays(3);

            var instance = new DynamicWrapper
                           {
                               { "_id", 1 },
                               { "TimeProperty", time1 },
                               { "NullableTimeProperty", time2 },
                               {
                                   "ComplexProperty", new DynamicWrapper
                                                      {
                                                          { "TimeProperty", time3 }
                                                      }
                               }
                           };

            var classStorage = MongoTestHelpers.GetEmptyStorageProvider<TimeClassExample>(nameof(ShouldSaveDynamicWrapperWithTime));
            var dynamicStorage = MongoTestHelpers.GetStorageProvider(nameof(ShouldSaveDynamicWrapperWithTime));

            // When

            dynamicStorage.InsertOne(instance);

            var classInstance = classStorage.Find().FirstOrDefault();
            var dynamicInstance = dynamicStorage.Find().FirstOrDefault();

            // Then

            Assert.IsNotNull(classInstance);
            Assert.AreEqual(time1, classInstance.TimeProperty);
            Assert.AreEqual(time2, classInstance.NullableTimeProperty);
            Assert.IsNotNull(classInstance.ComplexProperty);
            Assert.AreEqual(time3, classInstance.ComplexProperty.TimeProperty);
            Assert.IsNull(classInstance.ComplexProperty.NullableTimeProperty);

            Assert.IsNotNull(dynamicInstance);
            Assert.AreEqual(time1.TotalSeconds, dynamicInstance["TimeProperty"]);
            Assert.AreEqual(time2.TotalSeconds, dynamicInstance["NullableTimeProperty"]);
            Assert.IsNotNull(dynamicInstance["ComplexProperty"]);
            Assert.AreEqual(time3.TotalSeconds, ((DynamicWrapper)dynamicInstance["ComplexProperty"])["TimeProperty"]);
            Assert.IsNull(((DynamicWrapper)dynamicInstance["ComplexProperty"])["NullableTimeProperty"]);
        }


        private class TimeClassExample
        {
            public object _id { get; set; }

            public Time TimeProperty { get; set; }

            public Time? NullableTimeProperty { get; set; }

            public TimeClassExample ComplexProperty { get; set; }
        }
    }
}