using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Events;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using NUnit.Framework;
using Newtonsoft.Json;

namespace InfinniPlatform.Api.Tests.Events
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ObjectToEventSerializerBehavior
	{
		[Test]
		public void ShouldSerializeObjectStandard()
		{
			var obj = new
				          {
					          TestObject = new
						                       {
							                       TestProperty = "Test"
						                       }
				          };
			var serializer = new ObjectToEventSerializerStandard(obj);
			var events = serializer.GetEvents().Select(e => (JsonConvert.SerializeObject(e))).ToArray();
			Assert.AreEqual(events.Count(),2);
			Assert.AreEqual(events[0], "{\"Property\":\"TestObject\",\"Value\":null,\"Action\":1,\"Index\":-1}");
			Assert.AreEqual(events[1], "{\"Property\":\"TestObject.TestProperty\",\"Value\":\"Test\",\"Action\":2,\"Index\":-1}");

		}

		[Test]
		public void ShouldSerializeAddCollectionItem()
		{
			var obj = new
			{
				TestObject = new
				{
					TestProperty = "Test"
				}
			};

			var addCollectionItem = new AddCollectionItem("Test", obj,null);

			var events = addCollectionItem.GetEvents().Select(e => (JsonConvert.SerializeObject(e))).ToArray();
			Assert.AreEqual(events.Count(), 3);
			Assert.AreEqual(events[0], "{\"Property\":\"Test\",\"Value\":null,\"Action\":16,\"Index\":-1}");
			Assert.AreEqual(events[1], "{\"Property\":\"Test.@.TestObject\",\"Value\":null,\"Action\":1,\"Index\":-1}");
			Assert.AreEqual(events[2], "{\"Property\":\"Test.@.TestObject.TestProperty\",\"Value\":\"Test\",\"Action\":2,\"Index\":-1}");
		}

        [Test]
        public void ShouldSerializeRemoveCollectionItem()
        {
            var removeCollectionItem = new RemoveCollectionItem("Test:123",null);

            var events = removeCollectionItem.GetEvents().Select(e => (JsonConvert.SerializeObject(e))).ToArray();
            Assert.AreEqual(events.Count(), 1);
            Assert.AreEqual(events.First(), "{\"Property\":\"Test:123\",\"Value\":null,\"Action\":32,\"Index\":-1}");
        }

        [Test]
        public void ShouldSerializeUpdateCollectionItem()
        {
            var objectToUpdate = new
                {
                    TestObject = new
                        {
                            TestProperty = "Test"
                        }
                };

            var updateCollectionItem = new UpdateCollectionItem("Test", 1, objectToUpdate,null);
            
            var events = updateCollectionItem.GetEvents().Select(e => (JsonConvert.SerializeObject(e))).ToArray();
			Assert.AreEqual(events.Count(), 2);
			Assert.AreEqual(events[0], "{\"Property\":\"Test.1.TestObject\",\"Value\":null,\"Action\":1,\"Index\":-1}");
			Assert.AreEqual(events[1], "{\"Property\":\"Test.1.TestObject.TestProperty\",\"Value\":\"Test\",\"Action\":2,\"Index\":-1}");

        }
	}
}
