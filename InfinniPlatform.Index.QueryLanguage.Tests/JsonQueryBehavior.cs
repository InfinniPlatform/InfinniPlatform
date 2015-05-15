using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.NestFilters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using InfinniPlatform.SystemConfig.RoutingFactory;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class JsonQueryBehavior
	{
		private JObject _selectingJson;
		private JObject _jsonQuery;

		private ICrudOperationProvider _elasticSearchProvider;
		private IIndexStateProvider _indexProvider;
		private ICrudOperationProvider _elasticSearchProviderMain;
		private IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			var elasticFactory = new ElasticFactory(new RoutingFactoryBase());

			_indexProvider = elasticFactory.BuildIndexStateProvider();
            _indexProvider.RecreateIndex("TestIndex", "TestIndex");
			_indexProvider.RecreateIndex("Document","Document");

            _elasticSearchProvider = elasticFactory.BuildCrudOperationProvider("TestIndex", "TestIndex", AuthorizationStorageExtensions.AnonimousUser);
			_elasticSearchProviderMain = elasticFactory.BuildCrudOperationProvider("Document", "Document", AuthorizationStorageExtensions.AnonimousUser);

		}

		private void CreateReferencedEntity()
		{
			dynamic referencedEntity = new ExpandoObject();
			referencedEntity.Id = "testid";
			referencedEntity.Property1 = new ExpandoObject();
			referencedEntity.Property1.Value = "TestValue1";
			referencedEntity.SimpleProperty = 1;

			_elasticSearchProvider.Set(referencedEntity, IndexItemStrategy.Insert);
			_elasticSearchProvider.Refresh();
		}

		private void CreateReferenceIndex()
		{
			_indexProvider.RecreateIndex("TestIndex", "TestIndex");
		}

		private void CreateJsonQuery()
		{
			_jsonQuery = JObject.FromObject(new
			{
				From = new
				{
					Index = "Document",
					Type = "Document",
					Alias = "Document"
				},
				Join = new[] {
						   new 
						   {
							   Index = "Patient",
							   Alias = "Patient",
							   Type = "Patient",
							   Path = "PatientId"
						   },
						   new
							   {
								   Index = "Organization",
								   Alias = "Organization",
								   Type = "Organization",
							       Path = "PatientId.OrganizationId"
							   },

						},
				Where = new[] {
							new {
								Property = "Event.DateFrom",
								CriteriaType = CriteriaType.IsMoreThan,
								Value = new DateTime(2011,01,01)
							},
							new {
								Property = "Event.DateFrom",
								CriteriaType = CriteriaType.IsLessThan,
								Value = new DateTime(2012,01,01)
							}
						},
				Select = new[]
							 {
								 "Name",
								 "Patient.FirstName",
								 "Patient.LastName",
								 "Organization.Name",
								 "Organization.Phones",
								 "Author"
							 }
			});

		}

		private void CreateSelectingJsonObject()
		{
			_selectingJson = JObject.FromObject(new
			{
				ConfigurationName = "DrugsVidal",
				ObjectMetadata = new[]
		                {
		                    new
		                        {
		                            MetadataId = "REF_VIDAL",
		                            SomeObject = new
		                                {
		                                    SomeProperty = "SomeValue"
		                                }
		                        },
		                    new
		                        {
		                            MetadataId = "REF_CLPHPOINTER",
		                            SomeObject = new
		                                {
		                                    SomeProperty = "AnotherValue"
		                                }
		                        }
		                },
				FirstLevelObject = new
				{

				},
				ReferenceId = "testid",
				NestedReferenceField = new
				{
					NestedReferenceId = "testid"
				},
				CollectionAliasField = new [] {
					new
						{
							ReferenceId = "testid"
						}				                                    
				}

			});
		}

		//[Test]
		//public void ShouldGetProjections()
		//{
		//	CreateSelectingJsonObject();

		//	//получение проекции свойства вложенного контейнера элемента коллекции
		//	var selectPart = new ProjectionObject()
		//						 {
		//							 ProjectionPath = "ObjectMetadata.$.SomeObject.SomeProperty"
		//						 };


		//	var projectionBuilder = new ProjectionBuilder();
		//	var projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());

		//	Assert.AreEqual(projection.ToString(),"{\r\n  \"Result\": {\r\n    \"ObjectMetadata\": [\r\n      {\r\n        \"SomeObject\": {\r\n          \"SomeProperty\": \"SomeValue\"\r\n        }\r\n      },\r\n      {\r\n        \"SomeObject\": {\r\n          \"SomeProperty\": \"AnotherValue\"\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}");
			
		//	//получение проекции элементов коллекции
		//	selectPart = new ProjectionObject()
		//	{
		//		ProjectionPath = "ObjectMetadata.$"
		//	};


		//	projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());
		//	Assert.AreEqual(projection.ToString(),"{\r\n  \"Result\": {\r\n    \"ObjectMetadata\": [\r\n      {\r\n        \"MetadataId\": \"REF_VIDAL\",\r\n        \"SomeObject\": {\r\n          \"SomeProperty\": \"SomeValue\"\r\n        }\r\n      },\r\n      {\r\n        \"MetadataId\": \"REF_CLPHPOINTER\",\r\n        \"SomeObject\": {\r\n          \"SomeProperty\": \"AnotherValue\"\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}");

		//	//получение проекции данных элемента по индексу
		//	selectPart = new ProjectionObject()
		//	{
		//		ProjectionPath = "ObjectMetadata.1"
		//	};

		//	projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());

		//	Assert.AreEqual(projection.ToString(),"{\r\n  \"Result\": {\r\n    \"ObjectMetadata\": [\r\n      {\r\n        \"MetadataId\": \"REF_CLPHPOINTER\",\r\n        \"SomeObject\": {\r\n          \"SomeProperty\": \"AnotherValue\"\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}");

			
		//	//получение проекции данных последнего элемента коллекции
		//	selectPart = new ProjectionObject()
		//	{
		//		ProjectionPath = "ObjectMetadata.@"
		//	};

		//	projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());

		//	Assert.AreEqual(projection.ToString(), "{\r\n  \"Result\": {\r\n    \"ObjectMetadata\": [\r\n      {\r\n        \"MetadataId\": \"REF_CLPHPOINTER\",\r\n        \"SomeObject\": {\r\n          \"SomeProperty\": \"AnotherValue\"\r\n        }\r\n      }\r\n    ]\r\n  }\r\n}");

		//	selectPart = new ProjectionObject()
		//	{
		//		ProjectionPath = "ConfigurationName"
		//	};

		//	//получение проекции данных контейнера
		//	projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());
		//	Assert.AreEqual(projection.ToString(),"{\r\n  \"Result\": {\r\n    \"ConfigurationName\": \"DrugsVidal\"\r\n  }\r\n}");

		//	selectPart = new ProjectionObject()
		//	{
		//		ProjectionPath = "FirstLevelObject"
		//	};

		//	//удаление пустых результатов из проекции
		//	projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());
		//	Assert.AreEqual(projection.ToString(), "{\r\n  \"Result\": {}\r\n}");

		//	selectPart = new ProjectionObject()
		//	{
		//		ProjectionPath = "NotFoundToken"
		//	};

		//	projection = projectionBuilder.GetProjection(_selectingJson, new[] { selectPart }, new List<Criteria>());
		//	Assert.AreEqual(projection.ToString(), "{\r\n  \"Result\": {}\r\n}");



		//	var selectParts = new List<ProjectionObject>();
		//	selectParts.Add(new ProjectionObject()
		//	{
		//		ProjectionPath = "ConfigurationName"
		//	});
		//	selectParts.Add(new ProjectionObject()
		//						{
		//							ProjectionPath = "ObjectMetadata.$"
		//						});

		//	projection = projectionBuilder.GetProjection(_selectingJson, selectParts, new List<Criteria>());
		//	Assert.AreEqual(JsonConvert.SerializeObject(projection),
		//					"{\"Result\":{\"ConfigurationName\":\"DrugsVidal\",\"ObjectMetadata\":[{\"MetadataId\":\"REF_VIDAL\",\"SomeObject\":{\"SomeProperty\":\"SomeValue\"}},{\"MetadataId\":\"REF_CLPHPOINTER\",\"SomeObject\":{\"SomeProperty\":\"AnotherValue\"}}]}}");

		//}


		[Test]
		public void ShouldLoadReferencedFields()
		{
			CreateSelectingJsonObject();
			CreateReferenceIndex();
			CreateReferencedEntity();


			var references = new List<ReferencedObject>()
				                 {
					                 new ReferencedObject()
						                 {
							                 Alias = "Reference",
							                 Index = "TestIndex",
											 Type = "TestIndex",
							                 Path = "ReferenceId"
						                 }
				                 };

			var referenceBuilder = new ReferenceBuilder(new ElasticFactory(new RoutingFactoryBase()),AuthorizationStorageExtensions.AnonimousUser);
			referenceBuilder.FillReference(_selectingJson, references.ToArray(),new List<WhereObject>(),  new NestFilterBuilder());

            Assert.AreEqual("{\r\n  \"ConfigurationName\": \"DrugsVidal\",\r\n  \"ObjectMetadata\": [\r\n    {\r\n      \"MetadataId\": \"REF_VIDAL\",\r\n      \"SomeObject\": {\r\n        \"SomeProperty\": \"SomeValue\"\r\n      }\r\n    },\r\n    {\r\n      \"MetadataId\": \"REF_CLPHPOINTER\",\r\n      \"SomeObject\": {\r\n        \"SomeProperty\": \"AnotherValue\"\r\n      }\r\n    }\r\n  ],\r\n  \"FirstLevelObject\": {},\r\n  \"ReferenceId\": \"testid\",\r\n  \"NestedReferenceField\": {\r\n    \"NestedReferenceId\": \"testid\"\r\n  },\r\n  \"CollectionAliasField\": [\r\n    {\r\n      \"ReferenceId\": \"testid\"\r\n    }\r\n  ],\r\n  \"Reference\": {\r\n    \"Id\": \"testid\",\r\n    \"Property1\": {\r\n      \"Value\": \"TestValue1\"\r\n    },\r\n    \"SimpleProperty\": 1,\r\n    \"__ConfigId\": \"testindex\",\r\n    \"__DocumentId\": \"testindex\"\r\n  }\r\n}", _selectingJson.ToString());
		}

		[Test]
		public void ShouldLoadRefFieldsByNestedAliases()
		{
			CreateSelectingJsonObject();
			CreateReferenceIndex();
			CreateReferencedEntity();

			var references = new List<ReferencedObject>()
				                 {
					                 new ReferencedObject()
						                 {
							                 Alias = "Reference",
							                 Index = "TestIndex",
											 Type = "TestIndex",
							                 Path = "NestedReferenceField.NestedReferenceId"
						                 }
				                 };

			var referenceBuilder = new ReferenceBuilder(new ElasticFactory(new RoutingFactoryBase()),AuthorizationStorageExtensions.AnonimousUser);
			referenceBuilder.FillReference(_selectingJson, references.ToArray(), new List<WhereObject>(), new NestFilterBuilder());

            Assert.AreEqual("{\r\n  \"ConfigurationName\": \"DrugsVidal\",\r\n  \"ObjectMetadata\": [\r\n    {\r\n      \"MetadataId\": \"REF_VIDAL\",\r\n      \"SomeObject\": {\r\n        \"SomeProperty\": \"SomeValue\"\r\n      }\r\n    },\r\n    {\r\n      \"MetadataId\": \"REF_CLPHPOINTER\",\r\n      \"SomeObject\": {\r\n        \"SomeProperty\": \"AnotherValue\"\r\n      }\r\n    }\r\n  ],\r\n  \"FirstLevelObject\": {},\r\n  \"ReferenceId\": \"testid\",\r\n  \"NestedReferenceField\": {\r\n    \"NestedReferenceId\": \"testid\",\r\n    \"Reference\": {\r\n      \"Id\": \"testid\",\r\n      \"Property1\": {\r\n        \"Value\": \"TestValue1\"\r\n      },\r\n      \"SimpleProperty\": 1,\r\n      \"__ConfigId\": \"testindex\",\r\n      \"__DocumentId\": \"testindex\"\r\n    }\r\n  },\r\n  \"CollectionAliasField\": [\r\n    {\r\n      \"ReferenceId\": \"testid\"\r\n    }\r\n  ]\r\n}", _selectingJson.ToString());
		}

		[Test]
		public void ShouldLoadRefFieldsByCollectionField()
		{
			CreateSelectingJsonObject();
			CreateReferenceIndex();
			CreateReferencedEntity();

			var references = new List<ReferencedObject>()
				                 {
					                 new ReferencedObject()
						                 {
							                 Alias = "Reference",
							                 Index = "TestIndex",
											 Type = "TestIndex",
							                 Path = "CollectionAliasField.$.ReferenceId"
						                 }
				                 };

			var referenceBuilder = new ReferenceBuilder(new ElasticFactory(new RoutingFactoryBase()),AuthorizationStorageExtensions.AnonimousUser);
			referenceBuilder.FillReference(_selectingJson, references.ToArray(), new List<WhereObject>(), new NestFilterBuilder());

            Assert.AreEqual("{\r\n  \"ConfigurationName\": \"DrugsVidal\",\r\n  \"ObjectMetadata\": [\r\n    {\r\n      \"MetadataId\": \"REF_VIDAL\",\r\n      \"SomeObject\": {\r\n        \"SomeProperty\": \"SomeValue\"\r\n      }\r\n    },\r\n    {\r\n      \"MetadataId\": \"REF_CLPHPOINTER\",\r\n      \"SomeObject\": {\r\n        \"SomeProperty\": \"AnotherValue\"\r\n      }\r\n    }\r\n  ],\r\n  \"FirstLevelObject\": {},\r\n  \"ReferenceId\": \"testid\",\r\n  \"NestedReferenceField\": {\r\n    \"NestedReferenceId\": \"testid\"\r\n  },\r\n  \"CollectionAliasField\": [\r\n    {\r\n      \"ReferenceId\": \"testid\",\r\n      \"Reference\": {\r\n        \"Id\": \"testid\",\r\n        \"Property1\": {\r\n          \"Value\": \"TestValue1\"\r\n        },\r\n        \"SimpleProperty\": 1,\r\n        \"__ConfigId\": \"testindex\",\r\n        \"__DocumentId\": \"testindex\"\r\n      }\r\n    }\r\n  ]\r\n}", _selectingJson.ToString());
		}

		[Test]
		public void ShouldGetProjectionItemsFromSyntaxTree()
		{
			CreateJsonQuery();

			var syntaxTree = new QuerySyntaxTree(_jsonQuery);
			var syntaxItems = syntaxTree.GetProjectionObjects().ToArray();

			//Title = "Name",
			//FN = "Patient.FirstName",
			//LN = "Patient.LastName",
			//OrgName = "Organization.Name",
			//Phones = "Organization.Phones",
			//MW = "Author"

			Assert.AreEqual(syntaxItems.Count(), 6);

			Assert.AreEqual(syntaxItems[0].ProjectionPath, "Name");
			Assert.AreEqual(syntaxItems[1].ProjectionPath, "Patient.FirstName");
			Assert.AreEqual(syntaxItems[2].ProjectionPath, "Patient.LastName");
			Assert.AreEqual(syntaxItems[3].ProjectionPath, "PatientId.Organization.Name");
			Assert.AreEqual(syntaxItems[4].ProjectionPath, "PatientId.Organization.Phones");
			Assert.AreEqual(syntaxItems[5].ProjectionPath, "Author");
		}

		[Test]
		public void ShouldGetReferenceItemsFromSyntaxTree()
		{
			CreateJsonQuery();

			var syntaxTree = new QuerySyntaxTree(_jsonQuery);
			var syntaxItems = syntaxTree.GetReferenceObjects().ToArray();

			//new 
			//{
			//	Index = "Patient",
			//	Alias = "Patient",
			//	Path = "PatientId"
			//},
			//new
			//	{
			//		Index = "Organization",
			//		Alias = "Organization",
			//		Path = "PatientId.OrganizationId"
			//	},

			Assert.AreEqual(syntaxItems.Count(), 2);

			Assert.AreEqual(syntaxItems[0].Index, "Patient");
			Assert.AreEqual(syntaxItems[0].Alias, "Patient");
			Assert.AreEqual(syntaxItems[0].Path, "PatientId");

			Assert.AreEqual(syntaxItems[1].Index, "Organization");
			Assert.AreEqual(syntaxItems[1].Alias, "Organization");
			Assert.AreEqual(syntaxItems[1].Path, "PatientId.OrganizationId");
		}

		[Test]
		public void ShouldGetConditionItemsFromSyntaxTree()
		{
			CreateJsonQuery();

			var syntaxTree = new QuerySyntaxTree(_jsonQuery);
			var syntaxItems = syntaxTree.GetConditionCriteria().ToArray();

			//new[] {
			//		   new {
			//			   Property = "Event.DateFrom",
			//			   CriteriaType = CriteriaType.MoreThan,
			//			   Value = new DateTime(2011,01,01)
			//		   },
			//		   new {
			//			   Property = "Event.DateFrom",
			//			   CriteriaType = CriteriaType.LessThan,
			//			   Value = new DateTime(2012,01,01)
			//		   }
			//	   },

			Assert.AreEqual(syntaxItems.Count(), 2);

			Assert.AreEqual(syntaxItems[0].CriteriaType, CriteriaType.IsMoreThan);
			Assert.AreEqual(syntaxItems[0].Property, "Event.DateFrom");
			Assert.AreEqual(Convert.ToDateTime(syntaxItems[0].Value), new DateTime(2011, 01, 01));

			Assert.AreEqual(syntaxItems[1].CriteriaType, CriteriaType.IsLessThan);
			Assert.AreEqual(syntaxItems[1].Property, "Event.DateFrom");
			Assert.AreEqual(Convert.ToDateTime(syntaxItems[1].Value), new DateTime(2012, 01, 01));
		}

		[Test]
		public void ShouldGetFromItemFromSyntaxTree()
		{
			CreateJsonQuery();

			var syntaxTree = new QuerySyntaxTree(_jsonQuery);
			var fromItem = syntaxTree.GetFrom();

			//From = new
			//{
			//	Index = "Document",
			//	Alias = "Document"
			//},

			Assert.IsNotNull(fromItem);

			Assert.AreEqual(fromItem.Index, "Document");
			Assert.AreEqual(fromItem.Type, "Document");
			Assert.AreEqual(fromItem.Alias, "Document");
			Assert.AreEqual(fromItem.Path, null);
		}


		private void CreateObjectForFilter()
		{
			dynamic filterableObject = new ExpandoObject();

			filterableObject.Id = Guid.NewGuid().ToString();
			filterableObject.DateTimeFieldFrom = new DateTime(2011, 01, 01);
			filterableObject.DateTimeFieldTo = new DateTime(2012, 01, 01);
			filterableObject.NumericField = 1.25;
			filterableObject.StringField = "test";
			filterableObject.NestedField = new ExpandoObject();
			filterableObject.NestedField.NestedFieldValue = "test2";
			filterableObject.ReferenceId = "testid";

			_indexProvider.RecreateIndex("Document", "Document");
			_indexProvider.Refresh();
			_elasticSearchProviderMain.Set(filterableObject, IndexItemStrategy.Insert);
			_elasticSearchProviderMain.Refresh();
		}


		[Test]
		public void ShouldGetQueryResult()
		{
			CreateObjectForFilter();
			CreateReferenceIndex();
			CreateReferencedEntity();

			var jsonQuery = JObject.FromObject(new
			{
				From = new
				{
					Index = "Document",
					Type = "Document",
					Alias = "Document"
				},
				Join = new[] {
						   new 
						   {
							   Index = "TestIndex",
							   Type = "TestIndex",
							   Alias = "Reference",
							   Path = "ReferenceId"
						   }
						},
				Where = new[] {
							new {
								Property = "StringField",
								CriteriaType = CriteriaType.IsEquals,
								Value = "test"
							}
						},
				Select = new[]
				{
					"StringField",
					"Reference",
					"NestedField.NestedFieldValue"
					
				}
			});

			var result = new JsonQueryExecutor(new ElasticFactory(new RoutingFactoryBase()), _filterFactory, AuthorizationStorageExtensions.AnonimousUser).ExecuteQuery(jsonQuery);
			var compareResult = new[]
		        {
					new
						{
							Result = new
								{
									StringField = "test",
									Reference = new
										{
											Id = "testid",
											Property1 = new
												{
													Value = "TestValue1"
												},
											SimpleProperty = 1,
                                            __ConfigId = "testindex",
                                            __DocumentId = "testindex"
										},
									NestedField = new
													  {
														  NestedFieldValue = "test2"
													  },
								}
						}
		        };

			Assert.AreEqual(JArray.FromObject(compareResult).ToString(), result.ToString());
		}


		[Test]
		public void ShouldGetQueryResultWithoutWhereCondition()
		{
			CreateObjectForFilter();
			CreateReferenceIndex();
			CreateReferencedEntity();

			var jsonQuery = JObject.FromObject(new
			{
				From = new
				{
					Index = "Document",
					Type = "Document",
					Alias = "Document"
				},
				Join = new[] {
						   new 
						   {
							   Index = "TestIndex",
							   Type = "TestIndex",
							   Alias = "Reference",
							   Path = "ReferenceId"
						   }
						},
				Select = new[]
                {
                    "StringField",
                    "Reference.Property1.Value",
					"NestedField.NestedFieldValue"
                    
                }
			});

			var result = new JsonQueryExecutor(new ElasticFactory(new RoutingFactoryBase()), _filterFactory,AuthorizationStorageExtensions.AnonimousUser).ExecuteQuery(jsonQuery);

			var compareResult = new[]
		        {
					new {
						Result = new
									{
										StringField = "test",
										Reference = new
											{
												Property1 = new
													{
														Value = "TestValue1"
													}
											},
										NestedField = new
																  {
																	  NestedFieldValue = "test2"
																  },
									}
								}
		        };

			Assert.AreEqual(JArray.FromObject(compareResult).ToString(), result.ToString());
		}

		[Test]
		public void ShouldMakeSelectWithoutJoin()
		{
			CreateObjectForFilter();
			CreateReferenceIndex();
			CreateReferencedEntity();

			var jsonQuery = JObject.FromObject(new
			{
				From = new
				{
					Index = "Document",
					Alias = "Document",
					Type = "Document"
				},
				Where = new[] {
							new {
								Property = "StringField",
								CriteriaType = CriteriaType.IsEquals,
								Value = "test"
							}
						},
				Select = new[]
				{
					"StringField",
					"NestedField.NestedFieldValue",
				}
			});

			var result = new JsonQueryExecutor(new ElasticFactory(new RoutingFactoryBase()), _filterFactory,AuthorizationStorageExtensions.AnonimousUser).ExecuteQuery(jsonQuery);

			var compareResult = new[]
		        {
		            new {
						Result = new
		                {
		                    StringField = "test",
                            NestedField = new
	                                          {
		                                        NestedFieldValue = "test2"
	                                          },
		                }
					}
		        };

			Assert.AreEqual(JArray.FromObject(compareResult).ToString(), result.ToString());
		}



		[Test]
		public void ShouldRemoveUnsatisfiedItem()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			var criteria = new WhereObject()
				{
					RawProperty = "SomeObject.SomeProperty",
					CriteriaType = CriteriaType.IsEquals,
					Property = "SomeObject.SomeProperty",
					Value = "Property1"
				};

			jtoken.FilterItems(new[] { criteria });

			var compareToken2 = JObject.FromObject(new
				{
					Title = "Name2"
				});

			Assert.AreEqual(compareToken2.ToString(), jtoken.ToString());
		}

		[Test]
		public void ShouldReturnsEmptyResult()
		{
			var jtoken1 = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeProperty = "Property1",
					SomeValue = 1
				}
			});

			var criteria1 = new WhereObject()
			{
				CriteriaType = CriteriaType.IsEquals,
				RawProperty = "Title",
				Property = "Title",
				Value = "Name2"
			};

			var criteria2 = new WhereObject()
			{
				CriteriaType = CriteriaType.IsEquals,
				RawProperty = "SomeObject.SomeProperty",
				Property = "SomeObject.SomeProperty",
				Value = "NotExistingValue"
			};

			jtoken1.FilterItems(new[] { criteria1, criteria2 });

			var compareResult = JObject.FromObject(new
			{

			});
			Assert.AreEqual(compareResult.ToString(), jtoken1.ToString());
		}


		[Test]
		public void ShouldReturnsFullResult()
		{
			var jtoken1 = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeProperty = "Property1",
					SomeValue = 1
				}
			});

			var criteria1 = new WhereObject()
			{
				RawProperty = "Title",
				CriteriaType = CriteriaType.IsEquals,
				Property = "Title",
				Value = "Name1"
			};

			var criteria2 = new WhereObject()
			{
				RawProperty ="SomeObject.SomeProperty",
				CriteriaType = CriteriaType.IsEquals,
				Property = "SomeObject.SomeProperty",
				Value = "Property1"
			};

			jtoken1.FilterItems(new[] { criteria1, criteria2 });

			var compareResult = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeProperty = "Property1",
					SomeValue = 1
				}
			});
			Assert.AreEqual(compareResult.ToString(), jtoken1.ToString());
		}

		[Test]
		public void ShouldReturnsObjectWithCollectionFiltered()
		{
			var jtoken1 = JObject.FromObject(new
			{
				Title = "Name1",
				SomeCollection = new[] {
                    new
                    {
                        SomeProperty = "Property1",
                        SomeValue = 1
                    },
                    new
                    {
                        SomeProperty = "Property2",
                        SomeValue = 2
                    },
                    new
                    {
                        SomeProperty = "Property1",
                        SomeValue = 2
                    }
                }
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsEquals,
				Property = "SomeCollection.$.SomeProperty",
				RawProperty = "SomeCollection.$.SomeProperty",
				Value = "Property1"
			};


			jtoken1.FilterItems(new[] { criteria });

			var compareResult = JObject.FromObject(new
			{
				Title = "Name1",
				SomeCollection = new[] {
                    new
                    {
                        SomeProperty = "Property1",
                        SomeValue = 1
                    },
                    new
                    {
                        SomeProperty = "Property1",
                        SomeValue = 2
                    }
                }
			});
			Assert.AreEqual(compareResult.ToString(), jtoken1.ToString());
		}

		[Test]
		public void ShouldApplyAndOperatorForCriteriaResults()
		{
			var jtoken1 = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeCollection = new[] {
						new
						{
							SomeProperty = "Property1",
							SomeValue = 1
						},
						new
						{
							SomeProperty = "Property2",
							SomeValue = 2
						},
						new
						{
							SomeProperty = "Property3",
							SomeValue = 2
						}
					}
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsEquals,
				Property = "SomeObject.SomeCollection.$.SomeValue",
				RawProperty = "SomeObject.SomeCollection.$.SomeValue",
				Value = 2
			};
			var criteria1 = new WhereObject()
								{
									CriteriaType = CriteriaType.IsEquals,
									Property = "SomeObject.SomeCollection.$.SomeProperty",
									RawProperty = "SomeObject.SomeCollection.$.SomeProperty",
									Value = "Property1"
								};

			jtoken1.FilterItems(new[] { criteria, criteria1 });

			var compareResult = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeCollection = new object[] {

					}
				}
			});
			Assert.AreEqual(compareResult.ToString(), jtoken1.ToString());


		}


		[Test]
		public void ShouldApplyNumericIndexForCollectionFilter()
		{
			var jtoken1 = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeCollection = new[] {
						new
						{
							SomeProperty = "Property1",
							SomeValue = 1
						},
						new
						{
							SomeProperty = "Property2",
							SomeValue = 2
						},
						new
						{
							SomeProperty = "Property3",
							SomeValue = 2
						}
					}
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsEquals,
				RawProperty = "SomeObject.SomeCollection.$.SomeValue",
				Property = "SomeObject.SomeCollection.$.SomeValue",
				Value = 2
			};
			var criteria1 = new WhereObject()
			{
				CriteriaType = CriteriaType.IsEquals,
				RawProperty = "SomeObject.SomeCollection.0.SomeProperty",
				Property = "SomeObject.SomeCollection.0.SomeProperty",
				Value = "Property1"
			};

			jtoken1.FilterItems(new[] { criteria, criteria1 });

			var compareResult = JObject.FromObject(new
			{
				Title = "Name1",
				SomeObject = new
				{
					SomeCollection = new object[] {
						new
						{
							SomeProperty = "Property3",
							SomeValue = 2
						}
					}
				}
			});
			Assert.AreEqual(compareResult.ToString(), jtoken1.ToString());
		}


		[Test]
		public void ShouldMoreThanOperatorSatisfy()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsMoreThan,
				RawProperty = "SomeObject.SomeValue",
				Property = "SomeObject.SomeValue",
				Value = 1
			};

			jtoken.FilterItems(new[] { criteria });

			var compareToken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			Assert.AreEqual(compareToken.ToString(), jtoken.ToString());
		}

		[Test]
		public void ShouldMoreThanOperatorNotSatisfy()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsMoreThan,
				RawProperty = "SomeObject.SomeValue",
				Property = "SomeObject.SomeValue",
				Value = 3
			};

			jtoken.FilterItems(new[] { criteria });

			var compareToken = JObject.FromObject(new
			{
				Title = "Name2"
			});

			Assert.AreEqual(compareToken.ToString(), jtoken.ToString());
		}


		[Test]
		public void ShouldLessThanOperatorSatisfy()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsLessThan,
				RawProperty = "SomeObject.SomeValue",
				Property = "SomeObject.SomeValue",
				Value = 3
			};

			jtoken.FilterItems(new[] { criteria });

			var compareToken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			Assert.AreEqual(compareToken.ToString(), jtoken.ToString());
		}

		[Test]
		public void ShouldLessThanOperatorNotSatisfy()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = 2
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsLessThan,
				Property = "SomeObject.SomeValue",
				RawProperty = "SomeObject.SomeValue",
				Value = 1
			};

			jtoken.FilterItems(new[] { criteria });

			var compareToken = JObject.FromObject(new
			{
				Title = "Name2"
			});

			Assert.AreEqual(compareToken.ToString(), jtoken.ToString());
		}

		[Test]
		public void ShouldIsContainsOperatorSatisfy()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = "TestValue"
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsContains,
				RawProperty = "SomeObject.SomeValue",
				Property = "SomeObject.SomeValue",
				Value = "Test"
			};

			jtoken.FilterItems(new[] { criteria });

			var compareToken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = "TestValue"
				}
			});

			Assert.AreEqual(compareToken.ToString(), jtoken.ToString());
		}

		[Test]
		public void ShouldIsNotContainsOperatorSatisfy()
		{
			var jtoken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = "TestValue"
				}
			});

			var criteria = new WhereObject()
			{
				CriteriaType = CriteriaType.IsNotContains,
				RawProperty = "SomeObject.SomeValue",
				Property = "SomeObject.SomeValue",
				Value = "NotContainsValue"
			};

			jtoken.FilterItems(new[] { criteria });

			var compareToken = JObject.FromObject(new
			{
				Title = "Name2",
				SomeObject = new
				{
					SomeProperty = "Property2",
					SomeValue = "TestValue"
				}
			});

			Assert.AreEqual(compareToken.ToString(), jtoken.ToString());
		}

	}
}
