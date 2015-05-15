using System;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestQuery.EventObjects;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Tests.Events
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class EventListBuilderBehavior
    {
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
            var eventStrings = jobject.ToEventListAsObject("").GetSerializedEvents();
            File.WriteAllLines("1.txt", eventStrings);

            var expectedResult = "{\"Property\":\"TestProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject\",\"Action\":1,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeProperty\",\"Value\":\"stringProp\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeIntProperty\",\"Value\":123,\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeDateTimeProp\",\"Value\":\"2000-10-10T00:00:00\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeBoolProp\",\"Value\":true,\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Action\":8,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"123\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"456\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"789\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":8,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.0.SomeProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp\",\"Action\":1,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp.TestProperty1\",\"Value\":456,\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.1.SomeProperty\",\"Value\":\"456\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp\",\"Action\":1,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp.TestProperty1\",\"Value\":789,\"Action\":2,\"Index\":-1}";


            Assert.AreEqual(expectedResult, File.ReadAllText("1.txt").Replace("\r\n", "").Replace(" ", ""));

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

            var eventStrings = new EventList(obj.ToDynamic(), "").GetSerializedEvents();
            
            File.WriteAllLines("1.txt", eventStrings);

            var expectedResult = "{\"Property\":\"TestProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject\",\"Action\":1,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeProperty\",\"Value\":\"stringProp\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeIntProperty\",\"Value\":123,\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeDateTimeProp\",\"Value\":\"2000-10-10T00:00:00\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeBoolProp\",\"Value\":true,\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Action\":8,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"123\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"456\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeArrayProp\",\"Value\":\"789\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":8,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.0.SomeProperty\",\"Value\":\"123\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp\",\"Action\":1,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.0.SomeNestedProp.TestProperty1\",\"Value\":456,\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp\",\"Action\":16,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.1.SomeProperty\",\"Value\":\"456\",\"Action\":2,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp\",\"Action\":1,\"Index\":-1}" +
                                 "{\"Property\":\"TestObject.SomeObjectProp.1.SomeNestedProp.TestProperty1\",\"Value\":789,\"Action\":2,\"Index\":-1}";


            Assert.AreEqual(File.ReadAllText("1.txt").Replace("\r\n", "").Replace(" ", ""), expectedResult);

        }

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
			Assert.AreEqual(events.Count(),3);
            Assert.AreEqual(events[0], "{\r\n  \"Property\": \"Menu\",\r\n  \"Action\": 16,\r\n  \"Index\": -1\r\n}");
            Assert.AreEqual(events[1], "{\r\n  \"Property\": \"Menu.@.Item1\",\r\n  \"Action\": 1,\r\n  \"Index\": -1\r\n}");
            Assert.AreEqual(events[2], "{\r\n  \"Property\": \"Menu.@.Item1.Property1\",\r\n  \"Value\": \"test\",\r\n  \"Action\": 2,\r\n  \"Index\": -1\r\n}");
		}

    }
}
