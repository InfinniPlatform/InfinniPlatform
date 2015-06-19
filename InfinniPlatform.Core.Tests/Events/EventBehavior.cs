using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Json;
using InfinniPlatform.Json.EventBuilders;
using InfinniPlatform.Sdk.Application.Events;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Events
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public class EventBehavior
	{
		[Test]
		public void ShouldParsePropertyDefinition()
		{
			var jsonObject = JObject.FromObject(new
				                 {
					                 ObjectMetadata = new[]
						                                  {
							                                  new
								                                  {
									                                  FieldMetadata = new
										                                                  {
											                                                  MetadataDataType = new
												                                                                     {
													                                                                     MetadataIdentifier = "REF_TEST"
												                                                                     }

										                                                  }
								                                  },
															 new
																 {
																	 FieldMetadata = new
																		                 {
																			                 MetadataDataType = new
																				                                    {
																					                                    MetadataIdentifier = "REF_SECOND"
																				                                    }
																		                 }
																 }
						                                  }
				                 });
			var testProperty = "ObjectMetadata.0.FieldMetadata.MetadataDataType.MetadataIdentifier";
			var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, testProperty).FirstOrDefault();
			Assert.IsNotNull(resultJson);
			Assert.AreEqual(((JProperty)resultJson).Value.ToString(),"REF_TEST");

			testProperty = "ObjectMetadata.1.FieldMetadata.MetadataDataType.MetadataIdentifier";
			eventParser = new JsonParser();
            resultJson = eventParser.FindJsonToken(jsonObject, testProperty).FirstOrDefault();
			Assert.IsNotNull(resultJson);
			Assert.AreEqual(((JProperty)resultJson).Value.ToString(), "REF_SECOND");

		}

		public void ShouldReturnRootObjectIfNoPath()
		{
			
		}


		[Test]
		public void ShouldBuildObjectCollection()
		{
			var testProperty = "ObjectMetadata";
			var jsonObject = new JObject();

			var eventCreateCollection = new ObjectMetadataHandler().CreateContainerCollection(testProperty);
			new ContainerCollectionBuilder().BuildJObject(jsonObject, eventCreateCollection);

			Assert.AreEqual("{\"ObjectMetadata\":[]}",JsonConvert.SerializeObject(jsonObject));
		}

		[Test]
		public void ShouldBuildObject()
		{
			var testProperty = "SomeObject";
			var jsonObject = new JObject();

			var eventCreateContainer = new ObjectMetadataHandler().CreateContainer(testProperty);
			new ContainerBuilder().BuildJObject(jsonObject, eventCreateContainer);

			Assert.AreEqual("{\"SomeObject\":{}}", JsonConvert.SerializeObject(jsonObject));
			
		}

		[Test]
		public void ShouldBuildProperty()
		{
			var testProperty = "SomeProperty";
			var testValue = "SomeValue";
			var jsonObject = new JObject();

			var eventCreateProperty = new ObjectMetadataHandler().CreateProperty(testProperty,testValue);
			new PropertyBuilder().BuildJObject(jsonObject,eventCreateProperty);

			Assert.AreEqual("{\"SomeProperty\":\"SomeValue\"}", JsonConvert.SerializeObject(jsonObject));
		}


		[Test]
		public void ShouldThrowIfPathNotFound()
		{
			var testProperty = "SomeObject.SomeNestedObject";
			JObject jsonObject = null;

			var eventCreateContainer = new ObjectMetadataHandler().CreateContainer(testProperty);
			Assert.Throws<ArgumentException>(() => new ContainerBuilder().BuildJObject(jsonObject, eventCreateContainer));
		}

		[Test]
		public void ShouldFindAndCreateNestedProperty()
		{
			var jsonObject = JObject.FromObject(new
			{
				ObjectMetadata = new[]
					                {
							            new
								            {
									            FieldMetadata = new
										                            {
											                            MetadataDataType = new
												                                                {
													                                                //MetadataIdentifier = "REF_TEST"  <--- this we add on the test
												                                                }

										                            }
								            }
						            }
			});
			var propertyPath = "ObjectMetadata.0.FieldMetadata.MetadataDataType";
			var propertyToAdd = "MetadataIdentifier";
			var propertyValue = "REF_TEST";

			var eventParser = new JsonParser();
			var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath).FirstOrDefault();
		
			var eventCreateProperty = new ObjectMetadataHandler().CreateProperty(propertyToAdd, propertyValue);
			new PropertyBuilder().BuildJObject(resultJson, eventCreateProperty);

			Assert.AreEqual("{\"ObjectMetadata\":[{\"FieldMetadata\":{\"MetadataDataType\":{\"MetadataIdentifier\":\"REF_TEST\"}}}]}", JsonConvert.SerializeObject(jsonObject));
		}

		[Test]
		public void ShouldFindAndReturnAllCollectionItems()
		{
			var jsonObject = JObject.FromObject(new
			{
				ObjectMetadata = new[]
					                {
							            new
								            {
												TestProperty = "Test1"
								            },
										new
											{
												TestProperty = "Test2"
											}
						            }
			});
			var propertyPath = "ObjectMetadata.$.TestProperty";
			
			var eventParser = new JsonParser();
			var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath);
		
            Assert.AreEqual(resultJson.Count(),2);
            		

			Assert.AreEqual(resultJson.First().ToString(), "\"TestProperty\": \"Test1\"");
            Assert.AreEqual(resultJson.Skip(1).First().ToString(), "\"TestProperty\": \"Test2\"");	
		}


        [Test]
        public void ShouldFindAndReturnAllNestedContainerCollectionItems()
        {
            var jsonObject = JObject.FromObject(new
            {
                ObjectMetadata = new[]
					                {
							            new
								            {
												TestContainer = new
												    {
												        TestProperty = "Test1",
												    }
								            },
										new
											{
												TestContainer = new
												    {
												        TestProperty = "Test2"
												    }
											}
						            }
            });
            var propertyPath = "ObjectMetadata.$.TestContainer.TestProperty";

            var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath);

            Assert.AreEqual(resultJson.Count(), 2);


            Assert.AreEqual(resultJson.First().ToString(), "\"TestProperty\": \"Test1\"");
            Assert.AreEqual(resultJson.Skip(1).First().ToString(), "\"TestProperty\": \"Test2\"");
        }

        [Test]
        public void ShouldFindAndReturnAllContainerNestedCollectionItems()
        {
            var jsonObject = JObject.FromObject(new
            {
                ObjectMetadata = new[]
					                {
							            new
								            {
												TestContainer = new
												    {
												        TestCollection = new[]
												            {
												                new
												                    {
												                        TestProperty = "Test1"
												                    },
                                                                new
                                                                    {
                                                                        TestProperty = "Test2"
                                                                    }
												            }
												    }
								            },
										new
											{
												TestContainer = new
												    {
												        TestCollection = new[]
												            {
												                new
												                    {
												                        TestProperty = "Test3"
												                    },
                                                                new
                                                                    {
                                                                        TestProperty = "Test4"
                                                                    }
												            }
												    }
											}
						            }
            });
            var propertyPath = "ObjectMetadata.$.TestContainer.TestCollection.$.TestProperty";

            var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath);

            Assert.AreEqual(resultJson.Count(), 4);


            Assert.AreEqual(resultJson.First().ToString(), "\"TestProperty\": \"Test1\"");
            Assert.AreEqual(resultJson.Skip(1).First().ToString(), "\"TestProperty\": \"Test2\"");
            Assert.AreEqual(resultJson.Skip(2).First().ToString(), "\"TestProperty\": \"Test3\"");
            Assert.AreEqual(resultJson.Skip(3).First().ToString(), "\"TestProperty\": \"Test4\"");
        }

        [Test]
        public void ShouldFindAndReturnAllContainers()
        {
            var jsonObject = JObject.FromObject(new
            {
                ObjectMetadata = new[]
					                {
							            new
								            {
												TestContainer = new
												    {
												        TestCollection = new[]
												            {
												                new
												                    {
												                        TestProperty = "Test1"
												                    },
                                                                new
                                                                    {
                                                                        TestProperty = "Test2"
                                                                    }
												            }
												    }
								            },
										new
											{
												TestContainer = new
												    {
												        TestCollection = new[]
												            {
												                new
												                    {
												                        TestProperty = "Test3"
												                    },
                                                                new
                                                                    {
                                                                        TestProperty = "Test4"
                                                                    }
												            }
												    }
											}
						            }
            });
            var propertyPath = "ObjectMetadata.$.TestContainer.TestCollection.$";

            var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath);

            Assert.AreEqual(resultJson.Count(), 4);


            Assert.AreEqual(JsonConvert.SerializeObject(resultJson.First()), "{\"TestProperty\":\"Test1\"}");
            Assert.AreEqual(JsonConvert.SerializeObject(resultJson.Skip(1).First()), "{\"TestProperty\":\"Test2\"}");
            Assert.AreEqual(JsonConvert.SerializeObject(resultJson.Skip(2).First()), "{\"TestProperty\":\"Test3\"}");
            Assert.AreEqual(JsonConvert.SerializeObject(resultJson.Skip(3).First()), "{\"TestProperty\":\"Test4\"}");
        }


		[Test]
		public void ShouldFindAndCreateNestedContainer()
		{
			var jsonObject = JObject.FromObject(new
			{
				ObjectMetadata = new[]
					                {
							            new
								            {
									            FieldMetadata = new
										                            {
																		//MetadataDataType = new    <-- this should be added on
																		//						{
																		//						}

										                            }
								            }
						            }
			});
			var propertyPath = "ObjectMetadata.0.FieldMetadata";
			var propertyToAdd = "MetadataDataType";

			var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath).FirstOrDefault();

			var eventCreateProperty = new ObjectMetadataHandler().CreateContainer(propertyToAdd);
			new ContainerBuilder().BuildJObject(resultJson, eventCreateProperty);

			Assert.AreEqual("{\"ObjectMetadata\":[{\"FieldMetadata\":{\"MetadataDataType\":{}}}]}", JsonConvert.SerializeObject(jsonObject));
		}


		[Test]
		public void ShouldFindAndCreateCollectionItemProperty()
		{
			var jsonObject = JObject.FromObject(new
			{
				ObjectMetadata = new[]
					                {
							            new
								            {

								            }
						            }
			});
			var propertyPath = "ObjectMetadata.0";
			var propertyToAdd = "FieldMetadata";

			var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath).FirstOrDefault();

			var eventCreateProperty = new ObjectMetadataHandler().CreateContainer(propertyToAdd);
			new ContainerBuilder().BuildJObject(resultJson, eventCreateProperty);

			Assert.AreEqual("{\"ObjectMetadata\":[{\"FieldMetadata\":{}}]}", JsonConvert.SerializeObject(jsonObject));
		}

		[Test]
		public void ShouldFindCollectionItemByProperty()
		{
			var jsonObject = JObject.FromObject(new
			{
				ObjectMetadata = new[]
					                {
							            new
								            {
												Property1 = "123"
								            },

										new
											{
												Property1 = "345"
											},

										new
											{
												Property1 = "456"
											}
						            }
			});
			var propertyPath = "ObjectMetadata.$.Property1:345";

			var eventParser = new JsonParser();
			var resultJson = eventParser.FindJsonToken(jsonObject, propertyPath).FirstOrDefault();

			Assert.AreEqual("{\r\n  \"Property1\": \"345\"\r\n}", resultJson.ToString());
		}

		[Test]
		public void ShouldFindAndCreateCollectionItem()
		{
			var jsonObject = JObject.FromObject(new
			{
				ObjectMetadata = new object[]
					                {

						            }
			});
			var propertyPath = "ObjectMetadata";

			var eventCreateProperty = new ObjectMetadataHandler().AddItemToCollection(propertyPath);

			
			new CollectionItemBuilder().BuildJObject(jsonObject, eventCreateProperty);

			Assert.AreEqual("{\"ObjectMetadata\":[{}]}", JsonConvert.SerializeObject(jsonObject));
		}

		[Test]
		public void ShouldFindAndCreateRoot()
		{
			var jsonObject = JObject.FromObject(new
			{

			});
			var propertyPath = "ObjectMetadata";

			var eventParser = new JsonParser();
            var resultJson = eventParser.FindJsonToken(jsonObject, "").FirstOrDefault();

			var eventCreateContainerCollection = new ObjectMetadataHandler().CreateContainerCollection(propertyPath);
			new ContainerCollectionBuilder().BuildJObject(resultJson, eventCreateContainerCollection);

			Assert.AreEqual("{\"ObjectMetadata\":[]}", JsonConvert.SerializeObject(jsonObject));
		}


        [Test]
        public void ShouldConstructAccordingTypeProperty()
        {
            /* 
            SomeMetadata = new
            {
                SomeObjectProperty = new
                {
                    SomeDoubleProperty = 1.234,                    
                    SomeStringProperty = "SomeString",
                    SomeDateTimeProperty = new DateTime(2000, 1, 1),
                },
                SomeBoolProperty = true,
                SomeIntProperty = 12
            });*/

            var formMetadata = new ObjectMetadataHandler();
            IList<EventDefinition> events = new List<EventDefinition>();
            events.Add(formMetadata.CreateContainer("SomeMetadata"));
            events.Add(formMetadata.CreateContainer("SomeMetadata.SomeObjectProperty"));
            events.Add(formMetadata.CreateProperty("SomeMetadata.SomeIntProperty", 12));
            events.Add(formMetadata.CreateProperty("SomeMetadata.SomeBoolProperty", true));
            events.Add(formMetadata.CreateProperty("SomeMetadata.SomeObjectProperty.SomeDoubleProperty", 1.234));
            events.Add(formMetadata.CreateProperty("SomeMetadata.SomeObjectProperty.SomeStringProperty", "SomeString"));
            events.Add(formMetadata.CreateProperty("SomeMetadata.SomeObjectProperty.SomeDateTimeProperty", new DateTime(2000,1,1)));

            var formRenderer = new ObjectRenderingHandler();
			var result = JsonConvert.SerializeObject(formRenderer.RenderEvents(new object(), events));

            Assert.AreEqual("{\"SomeMetadata\":{\"SomeObjectProperty\":{\"SomeDoubleProperty\":1.234,\"SomeStringProperty\":\"SomeString\",\"SomeDateTimeProperty\":\"2000-01-01T00:00:00\"},\"SomeIntProperty\":12,\"SomeBoolProperty\":true}}", result);
        }







		[Test]
		public void ShouldCreatePrimitiveContainers()
		{
			var target = new
				             {
					             TestData = new[]
						                        {
							                        "Тестовая строка1",
							                        "Тестовая строка2",
							                        "Тестовая строка3"
						                        }
				             };
			
			var formMetadata = new ObjectMetadataHandler();
            IList<EventDefinition> events = new List<EventDefinition>();
			events.Add(formMetadata.CreateContainer("SomeMetadata"));
			events.Add(formMetadata.CreateContainerCollection("SomeMetadata.TestData"));
			events.Add(formMetadata.AddItemToCollection("SomeMetadata.TestData", "Тестовая строка1"));
			events.Add(formMetadata.AddItemToCollection("SomeMetadata.TestData", "Тестовая строка2"));
			events.Add(formMetadata.AddItemToCollection("SomeMetadata.TestData", "Тестовая строка3"));
			var formRenderer = new ObjectRenderingHandler();
			var result = JsonConvert.SerializeObject(formRenderer.RenderEvents(new object(), events));
			Assert.AreEqual(result,"{\"SomeMetadata\":{\"TestData\":[\"Тестовая строка1\",\"Тестовая строка2\",\"Тестовая строка3\"]}}");
		}


	    [Test]
	    public void ShouldFoldedContainersWithSameNamesAppliesCorrect()
	    {
            var target = new
                             {
                                 TestData = new
                                 {
                                     TestData = new
                                                    {
                                                        TestData = new
                                                                       {
                                                                           TestField = true
                                                                       }                                                                    
                                                    },
                                 }
                             };
	        var events = target
	            .ToEventListAsObject()
	            .GetEvents();
	        var formRenderer = new ObjectRenderingHandler();
	        var result = JsonConvert.SerializeObject(formRenderer.RenderEvents(new object(), events));

            Assert.AreEqual(result, "{\"FormMetadata\":{\"TestData\":{\"TestData\":{\"TestData\":{\"TestField\":true}}}}}");
	    }



	}
}
