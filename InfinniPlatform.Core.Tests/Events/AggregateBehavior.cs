using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using InfinniPlatform.Factories;
using InfinniPlatform.Json;
using InfinniPlatform.Sdk.Events;

using Newtonsoft.Json.Linq;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Events
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class AggregateBehavior
    {
        [Test]
        public void ShouldConstructEmptyAggregate()
        {
            var aggregateProvider = new AggregateProvider();
            dynamic aggregate = aggregateProvider.CreateAggregate();
            Assert.IsNotNull(aggregate.Id);
        }

        [Test]
        public void ShouldApplyChangesToAggregate()
        {
            //given
            var aggregateProvider = new AggregateProvider();
            var metadataHttpQuery = new ObjectMetadataHandler();
            dynamic aggregate = aggregateProvider.CreateAggregate();
            var events = new List<EventDefinition>
                         {
                             metadataHttpQuery.CreateContainer("TestObject"),
                             metadataHttpQuery.CreateProperty("TestObject.TestProperty", "test")
                         };

            //when
            aggregateProvider.ApplyChanges(ref aggregate, events);

            //then
            var jobject = JObject.FromObject(aggregate);
            var path1 = new JsonParser().FindJsonToken(jobject, "TestObject");
            var path2 = new JsonParser().FindJsonToken(jobject, "TestObject.TestProperty");

            Assert.IsNotNull(path1);
            Assert.IsNotNull(path2);
        }

        [Test]
        public void ShouldAddCollectionItemInAggregate()
        {
            var aggregateProvider = new AggregateProvider();

            object itemToApply = new
                                 {
                                     Test = new[]
                                            {
                                                new
                                                {
                                                    TestObject = new
                                                                 {
                                                                     TestProperty = "Test1"
                                                                 }
                                                }
                                            }
                                 };

            var obj = new
                      {
                          TestObject = new
                                       {
                                           TestProperty = "Test2"
                                       }
                      };

            var addCollectionItem = new AddCollectionItem("Test", obj);
            var events = addCollectionItem.GetEvents().ToArray();

            aggregateProvider.ApplyChanges(ref itemToApply, events);

            Assert.AreEqual(itemToApply.ToString(),
                "{\r\n  \"Test\": [\r\n    {\r\n      \"TestObject\": {\r\n        \"TestProperty\": \"Test1\"\r\n      }\r\n    },\r\n    {\r\n      \"TestObject\": {\r\n        \"TestProperty\": \"Test2\"\r\n      }\r\n    }\r\n  ]\r\n}".Replace("\r\n", Environment.NewLine));
        }

        [Test]
        [Ignore("Need to fix")]
        public void ShouldAddCollectionItemInInnerAggregateCollection()
        {
            var aggregateProvider = new AggregateProvider();

            object itemToApply = new
                                 {
                                     Test = new[]
                                            {
                                                new
                                                {
                                                    TestProperty = "Test1",
                                                    TestInnerCollection = new[]
                                                                          {
                                                                              new
                                                                              {
                                                                                  TestObject = new
                                                                                               {
                                                                                                   TestInnerProperty = "Test1"
                                                                                               }
                                                                              }
                                                                          }
                                                },
                                                new
                                                {
                                                    TestProperty = "Test2",
                                                    TestInnerCollection = new[]
                                                                          {
                                                                              new
                                                                              {
                                                                                  TestObject = new
                                                                                               {
                                                                                                   TestInnerProperty = "Test2"
                                                                                               }
                                                                              }
                                                                          }
                                                }
                                            }
                                 };

            var obj = new
                      {
                          TestObject = new
                                       {
                                           TestInnerProperty = "Test3"
                                       }
                      };

            var addCollectionItem = new AddCollectionItem("Test.$.TestProperty:Test2.TestInnerCollection", obj);
            var events = addCollectionItem.GetEvents().ToArray();

            aggregateProvider.ApplyChanges(ref itemToApply, events);

            Assert.AreEqual(itemToApply.ToString(),
                "{\r\n  \"Test\": [\r\n    {\r\n      \"TestProperty\": \"Test1\",\r\n      \"TestInnerCollection\": [\r\n        {\r\n          \"TestObject\": {\r\n            \"TestInnerProperty\": \"Test1\"\r\n          }\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"TestProperty\": \"Test2\",\r\n      \"TestInnerCollection\": [\r\n        {\r\n          \"TestObject\": {\r\n            \"TestInnerProperty\": \"Test2\"\r\n          }\r\n        },\r\n        {\r\n          \"TestObject\": {\r\n            \"TestInnerProperty\": \"Test3\"\r\n          }\r\n        }\r\n      ]\r\n    }\r\n  ]\r\n}".Replace("\r\n", Environment.NewLine));
        }

        [Test]
        [Ignore("Еще раз рассмотреть данный юзкейс в случае возникновения прецедента")]
        public void ShouldRemoveCollectionItemFromAggregate()
        {
            var aggregateProvider = new AggregateProvider();

            object itemToApply = new
                                 {
                                     Test = new[]
                                            {
                                                new
                                                {
                                                    TestObject = new
                                                                 {
                                                                     TestProperty = "Test1"
                                                                 }
                                                },
                                                new
                                                {
                                                    TestObject = new
                                                                 {
                                                                     TestProperty = "Test2"
                                                                 }
                                                },
                                                new
                                                {
                                                    TestObject = new
                                                                 {
                                                                     TestProperty = "Test3"
                                                                 }
                                                }
                                            }
                                 };

            var addCollectionItem = new RemoveCollectionItem("Test.$.TestObject.TestProperty:Test2");
            var events = addCollectionItem.GetEvents().ToArray();

            aggregateProvider.ApplyChanges(ref itemToApply, events);

            Assert.AreEqual(itemToApply.ToString(),
                "{\r\n  \"Test\": [\r\n    {\r\n      \"TestObject\": {\r\n        \"TestProperty\": \"Test1\"\r\n      }\r\n    },\r\n    {\r\n      \"TestObject\": {\r\n        \"TestProperty\": \"Test3\"\r\n      }\r\n    }\r\n  ]\r\n}".Replace("\r\n", Environment.NewLine));
        }
    }
}