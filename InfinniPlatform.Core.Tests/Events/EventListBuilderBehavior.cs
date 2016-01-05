using System;
using System.Linq;

using InfinniPlatform.Core.RestQuery.EventObjects;
using InfinniPlatform.Sdk.Dynamic;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Events
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class EventListBuilderBehavior
    {
        [Test]
        public void ShouldCreateEventsForLastCollectionItem()
        {
            var obj = new
                      {
                          Item1 = new
                                  {
                                      Property1 = "test"
                                  }
                      };

            var events = obj.ToEventListCollectionItem("Menu").GetSerializedEvents().ToArray();
            Assert.AreEqual(3, events.Count());
            Assert.AreEqual(string.Format("{{{0}  \"Property\": \"Menu\",{0}  \"Action\": 16,{0}  \"Index\": -1{0}}}", Environment.NewLine), events[0]);
            Assert.AreEqual(string.Format("{{{0}  \"Property\": \"Menu.@.Item1\",{0}  \"Action\": 1,{0}  \"Index\": -1{0}}}", Environment.NewLine), events[1]);
            Assert.AreEqual(string.Format("{{{0}  \"Property\": \"Menu.@.Item1.Property1\",{0}  \"Value\": \"test\",{0}  \"Action\": 2,{0}  \"Index\": -1{0}}}", Environment.NewLine), events[2]);
        }

        [Test]
        public void ShouldCreateEventsFromDynamicWrapper()
        {
            var obj = new
                      {
                          TestProperty = "123",
                          TestObject = new
                                       {
                                           SomeProperty = "stringProp",
                                           SomeIntProperty = 123,
                                           SomeDateTimeProp = new DateTime(2000, 10, 10),
                                           SomeBoolProp = true,
                                           SomeArrayProp = new[]
                                                           {
                                                               "123",
                                                               "456",
                                                               "789"
                                                           },
                                           SomeObjectProp = new[]
                                                            {
                                                                new
                                                                {
                                                                    SomeProperty = "123",
                                                                    SomeNestedProp = new
                                                                                     {
                                                                                         TestProperty1 = 456
                                                                                     }
                                                                },
                                                                new
                                                                {
                                                                    SomeProperty = "456",
                                                                    SomeNestedProp = new
                                                                                     {
                                                                                         TestProperty1 = 789
                                                                                     }
                                                                }
                                                            }
                                       }
                      };

            var actualResult = new EventList(obj.ToDynamic(), "").GetSerializedEvents();

            var expectedResult = new[]
                                 {
                                     "{\"Property\":\"TestProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject\",\"Action\":1,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeProperty\",\"Value\":\"stringProp\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeIntProperty\",\"Value\":123,\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeDateTimeProp\",\"Value\":\"2000-10-10T00:00:00\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeBoolProp\",\"Value\":true,\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Action\":8,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"123\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"456\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"789\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":8,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.0.SomeProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp\",\"Action\":1,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp.TestProperty1\",\"Value\":456,\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.1.SomeProperty\",\"Value\":\"456\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp\",\"Action\":1,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp.TestProperty1\",\"Value\":789,\"Action\":2,\"Index\":-1}"
                                 };

            CollectionAssert.AreEqual(expectedResult, actualResult.Select(i => i.Replace(Environment.NewLine, "").Replace(" ", "")));
        }

        [Test]
        public void ShouldCreateEventsFromJObject()
        {
            var obj = new
                      {
                          TestProperty = "123",
                          TestObject = new
                                       {
                                           SomeProperty = "stringProp",
                                           SomeIntProperty = 123,
                                           SomeDateTimeProp = new DateTime(2000, 10, 10),
                                           SomeBoolProp = true,
                                           SomeArrayProp = new[]
                                                           {
                                                               "123",
                                                               "456",
                                                               "789"
                                                           },
                                           SomeObjectProp = new[]
                                                            {
                                                                new
                                                                {
                                                                    SomeProperty = "123",
                                                                    SomeNestedProp = new
                                                                                     {
                                                                                         TestProperty1 = 456
                                                                                     }
                                                                },
                                                                new
                                                                {
                                                                    SomeProperty = "456",
                                                                    SomeNestedProp = new
                                                                                     {
                                                                                         TestProperty1 = 789
                                                                                     }
                                                                }
                                                            }
                                       }
                      };

            var jobject = JObject.FromObject(obj);

            var actualResult = jobject.ToEventListAsObject("").GetSerializedEvents();

            var expectedResult = new[]
                                 {
                                     "{\"Property\":\"TestProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject\",\"Action\":1,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeProperty\",\"Value\":\"stringProp\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeIntProperty\",\"Value\":123,\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeDateTimeProp\",\"Value\":\"2000-10-10T00:00:00\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeBoolProp\",\"Value\":true,\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Action\":8,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"123\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"456\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"789\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":8,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.0.SomeProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp\",\"Action\":1,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp.TestProperty1\",\"Value\":456,\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.1.SomeProperty\",\"Value\":\"456\",\"Action\":2,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp\",\"Action\":1,\"Index\":-1}",
                                     "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp.TestProperty1\",\"Value\":789,\"Action\":2,\"Index\":-1}"
                                 };

            CollectionAssert.AreEqual(expectedResult, actualResult.Select(i => i.Replace(Environment.NewLine, "").Replace(" ", "")));
        }
    }
}