using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace InfinniPlatform.Index.QueryLanguage.Tests
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public class ConfigurationQueryBehavior
	{

		private IIndexStateProvider _indexProvider;
		private ICrudOperationProvider _elasticSearchProviderMain;
		private ICrudOperationProvider _elasticSearchProviderDoc;
		private string _indexName = "Configuration";
		private string _indexJoin = "Document";
		private ElasticFactory _elasticFactory;
		private IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

		[TestFixtureSetUp]
		public void SetupFixture()
		{
			_elasticFactory = new ElasticFactory();


		}

		private void FillIndexConfiguration()
		{
			var expando = CreateExpandoConfiguration();

			_indexProvider = _elasticFactory.BuildIndexStateProvider();
			_indexProvider.RecreateIndex(_indexName,_indexName);
			_indexProvider.RecreateIndex(_indexJoin, _indexJoin);

			_elasticSearchProviderMain = _elasticFactory.BuildCrudOperationProvider(_indexName,_indexName, AuthorizationStorageExtensions.AnonimousUser);
			_elasticSearchProviderMain.Set(expando,IndexItemStrategy.Insert);
			_elasticSearchProviderMain.Refresh();


			_elasticSearchProviderDoc = _elasticFactory.BuildCrudOperationProvider(_indexJoin,_indexJoin, AuthorizationStorageExtensions.AnonimousUser);
			_elasticSearchProviderDoc.Set(CreateDoc1(), IndexItemStrategy.Insert);
			_elasticSearchProviderDoc.Set(CreateDoc2(), IndexItemStrategy.Insert);
			_elasticSearchProviderDoc.Refresh();

		}

		private DynamicWrapper CreateDoc1()
		{
			dynamic instanceDoc = new DynamicWrapper();
			instanceDoc.Id = 10;
			instanceDoc.SomeField = 1;
			return instanceDoc;
		}

		private DynamicWrapper CreateDoc2()
		{
			dynamic instanceDoc = new DynamicWrapper();
			instanceDoc.Id = 1000;
			instanceDoc.SomeField = 1000000;
			return instanceDoc;
		}

		private ExpandoObject CreateExpandoConfiguration()
		{
			var jconfig = CreateJConfiguration();
			var serializedObj = JsonConvert.SerializeObject(jconfig);
			return JsonConvert.DeserializeObject<ExpandoObject>(serializedObj);
		}

		private JObject CreateJConfiguration()
		{
			var config = new
							 {
								 Id = "1",

								 #region Configurations level = 0
								 Configurations = new object[]
                                                      {
                                                          #region Configuration level = 1	
                                                          new
                                                              {
                                                                  ConfigurationId = "Integration",
																  TestEmptyObject = new
																	                    {
																		                    
																	                    },
                                                                  #region ObjectsMetadata level = 2	
                                                                  ObjectsMetadata = new object[]
                                                                                        {
                                                                                            new
                                                                                                {
                                                                                                    MetadataId = "Patient",
																									IdExternal = 20,
                                                                                                    ServiceConfiguration = new[]
                                                                                                                               {
                                                                                                                                   "Search",
                                                                                                                                   "Post"
                                                                                                                               }

                                                                                                },
                                                                                            new
                                                                                                {
																									IdExternal = 20,
                                                                                                    MetadataId = "Document"
                                                                                                }
                                                                                        }
                                                                  #endregion
                                                              },
                                                          new
                                                              {
                                                                  ConfigurationId = "DrugsVidal",
                                                                  #region ObjectsMetadata level = 2
                                                                  ObjectsMetadata = new object[]
                                                                                        {
                                                                                            new
                                                                                                {
                                                                                                    MetadataId = "REF_VIDAL",
																									IdExternal = 10,
                                                                                                    #region FieldsMetadata level 3
                                                                                                    FieldsMetadata = new[]
                                                                                                                         {
                                                                                                                             new
                                                                                                                                 {
                                                                                                                                     Id = 500,
                                                                                                                                     IsEditable = false,
                                                                                                                                     MetadataDataType = new
                                                                                                                                                            {
                                                                                                                                                                MetadataIdentifier = "string",
                                                                                                                                                                MetadataTypeKind = "SimpleType"
                                                                                                                                                            },

                                                                                                                                     MetadataId = "DOCUMENT_ID",
                                                                                                                                     MetadataName = "Идентификатор документа",
                                                                                                                                     DataFieldName = "DocumentId"
                                                                                                                                 }
                                                                                                                         },

                                                                                                    #endregion
                                                                                                    #region FormsMetadata level 3
                                                                                                    FormsMetadata = new object[]
                                                                                                                        {
                                                                                                                            new
                                                                                                                                {
                                                                                                                                    FormMetadataId = "REF_VIDAL_JOURNAL"
                                                                                                                                },
                                                                                                                            new
                                                                                                                                {
                                                                                                                                    FormMetadataId = "REF_VIDAL_EDIT"
                                                                                                                                },
                                                                                                                            new
                                                                                                                                {
                                                                                                                                    FormMetadataId = "REF_VIDAL_SELECT"
                                                                                                                                }
                                                                                                                        },

                                                                                                    #endregion
                                                                                                    #region Actions level 3
                                                                                                    Actions = new[]
                                                                                                                  {
                                                                                                                      new
                                                                                                                          {
                                                                                                                              Type = "Select",
                                                                                                                              Url = "http://localhost:123"
                                                                                                                          },
                                                                                                                      new
                                                                                                                          {
                                                                                                                              Type = "Update",
                                                                                                                              Url = "http://localhost:456"
                                                                                                                          }
                                                                                                                  }
                                                                                                    #endregion
                                                                                                },
                                                                                            new
                                                                                                {
                                                                                                    MetadataId = "REF_CLPHPOINTER",
																									IdExternal = 10,
                                                                                                    #region FieldsMetadata level 3
                                                                                                    FieldsMetadata = new[]
                                                                                                                         {

                                                                                                                             new
                                                                                                                                 {

                                                                                                                                     Id = 500,
                                                                                                                                     IsIdentifier = true,
                                                                                                                                     IsEditable = false,
                                                                                                                                     MetadataDataType = new
                                                                                                                                                            {
                                                                                                                                                                MetadataIdentifier = "string",
                                                                                                                                                                MetadataTypeKind = "SimpleType"
                                                                                                                                                            },

                                                                                                                                     MetadataId = "ID",
                                                                                                                                     MetadataName = "Идентификатор пункта КФУ",
                                                                                                                                     DataFieldName = "Id"

                                                                                                                                 },

                                                                                                                             new
                                                                                                                                 {
                                                                                                                                     Id = 501,
                                                                                                                                     IsIdentifier = false,
                                                                                                                                     IsEditable = false,
                                                                                                                                     MetadataDataType = new
                                                                                                                                                            {
                                                                                                                                                                MetadataIdentifier = "string",
                                                                                                                                                                MetadataTypeKind = "SimpleType"
                                                                                                                                                            },
                                                                                                                                     MetadataId = "Name",
                                                                                                                                     MetadataName = "Наименование пункта КФУ",
                                                                                                                                     DataFieldName = "Name"
                                                                                                                                 }
                                                                                                                         },

                                                                                                    #endregion
                                                                                                    #region FormsMEtadata level 3
                                                                                                    FormsMetadata = new object[]
                                                                                                                        {

                                                                                                                            new
                                                                                                                                {
                                                                                                                                    FormMetadataId = "REF_CLPHPOINTER_SELECT"
                                                                                                                                }

                                                                                                                        }
                                                                                                    #endregion
                                                                                                },
                                                                                            new
                                                                                                {
                                                                                                    MetadataId = "REF_PHTHGROUP",
																									IdExternal = 10,
                                                                                                    #region FieldsMetadata level 3
                                                                                                    FieldsMetadata = new[]
                                                                                                                         {
                                                                                                                             new
                                                                                                                                 {
                                                                                                                                     Id = 510,
                                                                                                                                     IsIdentifier = true,
                                                                                                                                     IsEditable = false,
                                                                                                                                     MetadataDataType = new
                                                                                                                                                            {
                                                                                                                                                                MetadataIdentifier = "string",
                                                                                                                                                                MetadataTypeKind = "SimpleType"
                                                                                                                                                            },

                                                                                                                                     MetadataId = "GROUP_ID",
                                                                                                                                     MetadataName = "Идентификатор группы",
                                                                                                                                     DataFieldName = "Id"
                                                                                                                                 },
                                                                                                                                 new
                                                                                                                                 {
                                                                                                                                     Id = 520,
                                                                                                                                     IsIdentifier = true,
                                                                                                                                     IsEditable = false,
                                                                                                                                     MetadataDataType = new
                                                                                                                                                            {
                                                                                                                                                                MetadataIdentifier = "string",
                                                                                                                                                                MetadataTypeKind = "SimpleType"
                                                                                                                                                            },

                                                                                                                                     MetadataId = "FOO_ID",
                                                                                                                                     MetadataName = "",
                                                                                                                                     DataFieldName = "FooId"
                                                                                                                                 },
                                                                                                                         },

                                                                                                    #endregion
                                                                                                    #region FormsMetadata level 3
                                                                                                    FormsMetadata = new[]
                                                                                                                        {
                                                                                                                            new
                                                                                                                                {
                                                                                                                                    FormMetadataId = "REF_PHTHGROUP_SELECT"
                                                                                                                                }
                                                                                                                        },

                                                                                                    #endregion
                                                                                                    #region Actions level 3
                                                                                                    Actions = new[]
                                                                                                                  {
                                                                                                                      new
                                                                                                                          {
                                                                                                                              Type = "Select",
                                                                                                                              Url = "http://localhost:123"
                                                                                                                          },
                                                                                                                      new
                                                                                                                          {
                                                                                                                              Type = "Update",
                                                                                                                              Url = "http://localhost:456"
                                                                                                                          }
                                                                                                                  }
                                                                                                    #endregion
                                                                                                }
                                                                                        }
                                                                  #endregion
                                                              }
                                                          #endregion
                                                      },
								 #endregion
                                 __ConfigId = _indexName.ToLowerInvariant(),
                                 __DocumentId = _indexName.ToLowerInvariant(),
							 };
			return JObject.FromObject(config);
		}

		[Test]
		public void ShouldFindAllConfigurations()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName,
																   Alias = "config"
															   },
													Select = new[]
																 {
																	 "Configurations.$.ConfigurationId",
																	 "Configurations.$.TestEmptyObject"
																 }
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);

			//[
			//  {
			//	"Result": {
			//	  "Configurations": [
			//		{
			//		  "ConfigurationId": "Integration"
			//		},
			//		{
			//		  "ConfigurationId": "DrugsVidal"
			//		}
			//	  ]
			//	}
			//  }
			//]

			var compareResult = JArray.FromObject(new[] {														 
                                                        new
                                                              {
                                                                  Result =  new 
																  {
																	Configurations = new object[] {
																							   new
																								   {
																									   ConfigurationId = "Integration",
																									   TestEmptyObject = new
																										                     {
																											                     
																										                     }
																								   },
																							   new
																								   {
																									   ConfigurationId = "DrugsVidal"
																								   }
                                                                                           }
																	}
                                                              }
                                                      });

			Assert.AreEqual(compareResult.ToString(), result.ToString());
		}

		[Test]
		public void ShouldFindAllConfigurationObjectMetadata()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName,
																   Alias = "config"
															   },
													Join = new[] 
													{
													  new        {
														           Index = "document",
																   Type = "document",
  																   Alias = "doc",
																   Path = "Configurations.$.ObjectsMetadata.$.IdExternal"
													           }
													},
													Where = new object[]
                                                                {
                                                                    new
                                                                        {
                                                                            Property = "Configurations.$.ConfigurationId",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = "DrugsVidal"
                                                                        },
                                                                    new
                                                                        {
                                                                            Property = "doc.SomeField",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = 1
                                                                        }
                                                                },
													Select = new[] {
                                                                     "Configurations.$.ObjectsMetadata.$.MetadataId"
                                                                 },
												    Limit = new
													            {
														            PageSize = 1000,
																	StartPage = 0,
																	Skip = 0
                                                                 }
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);


			//[
			//	{
			//	"Result": {
			//		"Configurations": [
			//		{
			//			"ObjectsMetadata": [
			//			{
			//				"MetadataId": "REF_VIDAL"
			//			},
			//			{
			//				"MetadataId": "REF_CLPHPOINTER"
			//			},
			//			{
			//				"MetadataId": "REF_PHTHGROUP"
			//			}
			//			]
			//		}
			//		]
			//	}
			//	}
			//]
			var compareResult = JArray.FromObject(new[]
                                                      {
														  new
                                                              {
                                                                  Result = new
	                                                                           {
		                                                                           Configurations = new[]
			                                                                                            {	
			                                                                                            	new
			                                                                                            		{
			                                                                                            			ObjectsMetadata = new[]
				                                                                                            			                  {
																																			  new
																																				  {
																																					 MetadataId = "REF_VIDAL"	  
																																				  },
				                                                                                            								  new {
					                                                                                            								  MetadataId = "REF_CLPHPOINTER"
				                                                                                            								  },
				                                                                                            								  new {
					                                                                                            								  MetadataId = "REF_PHTHGROUP"
				                                                                                            								  },

				                                                                                            			                  }
			                                                                                            		}
			                                                                                            }
	                                                                           }
                                                              }
                                                      });



			Assert.AreEqual(compareResult.ToString(), result.ToString());
		}



		[Test]
		public void ShouldFindAllConfigurationObjectMetadataForms()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName,
																   Alias = "config"
															   },
													Where = new[]
                                                                {
                                                                    new
                                                                        {
                                                                            Property = "Configurations.$.ConfigurationId",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = "DrugsVidal"
                                                                        },
                                                                    new
                                                                        {
                                                                            Property = "Configurations.$.ObjectsMetadata.$.MetadataId",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = "REF_PHTHGROUP"
                                                                        }
                                                                },
													Select = new[]
																 {
																	 "Configurations.$.ObjectsMetadata.$.FormsMetadata.$.FormMetadataId"
																 }
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory,AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);


			var compareResult = JArray.FromObject(new[]
                                                      {
                                                          new {
															  Result = new
                                                              {
                                                                  Configurations = new[]
	                                                                                   {
																						   new {
		                                                                                   ObjectsMetadata = new []
			                                                                                                     {
				                                                                                                     new
					                                                                                                     {
						                                                                                                     FormsMetadata = new []
							                                                                                                                     {
								                                                                                                                     new
									                                                                                                                     {
										                                                                                                                     FormMetadataId = "REF_PHTHGROUP_SELECT"
									                                                                                                                     }
							                                                                                                                     }
					                                                                                                     }
			                                                                                                     }
																						   }
	                                                                                   } 
                                                              }
															}
                                                      });



			Assert.AreEqual(compareResult.ToString(), result.ToString());
		}


		[Test]
		public void ShouldFindNoResultsOnMutualExceptedColumns()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName,
																   Alias = "config"
															   },
													Where = new[]
                                                                {
                                                                    new
                                                                        {
                                                                            Property = "Configurations.$.ConfigurationId",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = "Integration"
                                                                        },
                                                                    new
                                                                        {
                                                                            Property = "Configurations.$.ObjectsMetadata.$.MetadataId",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = "REF_PHTHGROUP"
                                                                        }
                                                                },
													Select = new[]
																 {
																	 "Configurations.$.ObjectsMetadata.$.FormsMetadata.$.FormMetadataId"
																 }
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);

			var compareResult = JArray.FromObject(new[]
                                                      {
                                                          new
                                                              {
                                                                  Result = new {}
                                                              }
                                                      });

			Assert.AreEqual(compareResult.ToString(), result.ToString());
		}

		[Test]
		public void ShouldReturnsEmptyResultIfCollectionItemNotSatisfiedClientCondition()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName,
																   Alias = "config"
															   },
													Where = new[]
                                                                {
                                                                    new
                                                                        {
                                                                            Property = "Configurations.1.ObjectsMetadata.2.FormsMetadata.0.FormMetadataId",
                                                                            CriteriaType = CriteriaType.IsEquals,
                                                                            Value = "REF_PHTHGROUP_NONEXISTING"
                                                                        }
                                                                },
													Select = new[]
																 {
																	 "Configurations.1.ObjectsMetadata.2.FormsMetadata.0.FormMetadataId"
																 }
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);

			var compareResult = JArray.FromObject(new[]
                                                      {
                                                          new
                                                              {
                                                                  Result = new {}
                                                              }
                                                      });

			Assert.AreEqual(compareResult.ToString(), result.ToString());
		}

		[Test]
		public void ShouldSelectFullConfigurationObject()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName
															   },
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);

			var compareResult = JArray.FromObject(new[]
                                                      {
                                                         new
	                                                         {
																Result = CreateJConfiguration()   
	                                                         }
                                                      });

			Assert.AreEqual(result.ToString(), compareResult.ToString());
		}

		[Test]
		public void ShouldSelectFullConfigurationObjectIfSelectListEmpty()
		{
			FillIndexConfiguration();

			var jquery = JObject.FromObject(new
												{
													From = new
															   {
																   Index = _indexName,
																   Type = _indexName
																 }
												});

			var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
			var result = indexQueryExecutor.ExecuteQuery(jquery);

			var compareResult = JArray.FromObject(new[]
                                                      {
                                                          new
	                                                          {
																Result = CreateJConfiguration()   
	                                                          }
                                                      });

			Assert.AreEqual(result.ToString(), compareResult.ToString());
		}

		[Test]
		public void ShouldApplyAllCriteriaTypes()
		{
			FillIndexConfiguration();

			var from = new
						   {
							   Index = _indexName,
							   Type = _indexName,
							   Alias = "config"
						   };
			var select = new[]
							 {
								  "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId"
							 };

			var clauses = new List<Tuple<string, List<Criteria>, JArray>>
                              {
                                  #region Equals
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "Equals",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsEquals,
                                                      Value = "ID"
                                                  }
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })
                                      ),

                                  #endregion

                                  #region NotEquals
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "NotEquals",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsNotEquals,
                                                      Value = "ID"
                                                  }
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    },
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    },
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "GROUP_ID"
													                                                                                                                                     },
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "FOO_ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),


                                  #endregion

                                  #region Contains
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "Contains",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsContains,
                                                      Value = "ID"
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    },
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    },
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "GROUP_ID"
													                                                                                                                                     },
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "FOO_ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion

                                  #region NotContains
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "NotContains",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsNotContains,
                                                      Value = "ID"
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })
                                      ),

                                  #endregion

//ВНИМАНИЕ! НЕОБХОДИМО АКТУАЛИЗИРОВАТЬ ТЕСТЫ ДЛЯ ВЫБОРКИ ПО ОСТАВШИМСЯ КРИТЕРИЯМ!
                                /*  #region EndsWith
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "EndsWith",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsEndsWith,
                                                      Value = "ID"
                                                  }
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     },
																																													 new {
														                                                                                                                                     MetadataId = "ID"
													                                                                                                                                     },
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "GROUP_ID"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "FOO_ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion

                                  #region NotEndsWith
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "NotEndsWith",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsNotEndsWith,
                                                      Value = "ID"
                                                  }
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
																																													 new {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),
                                  #endregion

                                  #region StartsWith
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "StartsWith",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsStartsWith,
                                                      Value = "DOCUMENT"
                                                  }
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),
                                  #endregion

                                  #region NotStartsWith
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "NotStartsWith",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataId",
                                                      CriteriaType = CriteriaType.IsNotStartsWith,
                                                      Value = "DOCUMENT"
                                                  }
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "ID"
													                                                                                                                                     },
																																													 new {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     },
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "GROUP_ID"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "FOO_ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),
                                  #endregion

                                  #region LessThan
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "LessThan",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.Id",
                                                      CriteriaType = CriteriaType.IsLessThan,
                                                      Value = 501
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion

                                  #region LessThanOrEquals
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "LessThanOrEquals",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.Id",
                                                      CriteriaType = CriteriaType.IsLessThanOrEquals,
                                                      Value = 501
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "ID"
																																														 },
																																													 new {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion

                                  #region GreaterThan
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "GreaterThan",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.Id",
                                                      CriteriaType = CriteriaType.IsMoreThan,
                                                      Value = 501
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "GROUP_ID"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "FOO_ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),
                                  #endregion

                                  #region GreaterThanOrEquals
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "GreaterThanOrEquals",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.Id",
                                                      CriteriaType = CriteriaType.IsMoreThanOrEquals,
                                                      Value = 501
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
																																													 new {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     },
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "GROUP_ID"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "FOO_ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion
                                      
                                  #region IsEmpty
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "IsEmpty",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataName",
                                                      CriteriaType = CriteriaType.IsEmpty,
                                                      Value = null
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
																																													 new
																																														 {
																																															 MetadataId = "FOO_ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion

                                  #region IsNotEmpty
                                  new Tuple<string, List<Criteria>, JArray>
                                      (
                                      "IsNotEmpty",
                                      new List<Criteria>
                                          {
                                              new Criteria
                                                  {
                                                      Property = "Configurations.$.ObjectsMetadata.$.FieldsMetadata.$.MetadataName",
                                                      CriteriaType = CriteriaType.IsNotEmpty,
                                                      Value = null
                                                  },
                                          },
                                      JArray.FromObject(new[]
                                                            {
                                                                new
                                                                    {
                                                                        Result = new
	                                                                                 {
		                                                                                 Configurations = new []
			                                                                                                  {
				                                                                                                  new
					                                                                                                  {
						                                                                                                  ObjectsMetadata = new []
							                                                                                                                    {
								                                                                                                                    new
									                                                                                                                    {
										                                                                                                                    FieldsMetadata = new []
											                                                                                                                                     {
												                                                                                                                                     new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "DOCUMENT_ID"
													                                                                                                                                     },
																																													 new {
														                                                                                                                                     MetadataId = "ID"
													                                                                                                                                     },
																																													 new
													                                                                                                                                     {
														                                                                                                                                     MetadataId = "Name"
													                                                                                                                                     },
																																													 new
																																														 {
																																															 MetadataId = "GROUP_ID"
																																														 }
											                                                                                                                                     }
									                                                                                                                    }
							                                                                                                                    }
					                                                                                                  }
			                                                                                                  }
	                                                                                 } 
                                                                    }
                                                            })),

                                  #endregion */
                              };

			var errors = new List<string>();

			foreach (var clause in clauses)
			{
				var query = JObject.FromObject(new
				{
					From = from,
					Where = clause.Item2,
					Select = select
				});

				var indexQueryExecutor = new JsonQueryExecutor(new ElasticFactory(), _filterFactory, AuthorizationStorageExtensions.AnonimousUser);
				var result = indexQueryExecutor.ExecuteQuery(query);

				if (result.ToString() != clause.Item3.ToString())
				{
					errors.Add(clause.Item1);
				}
			}

			if (errors.Any())
			{
				Assert.Fail(string.Join("; ", errors));
			}
		}
	}
}
