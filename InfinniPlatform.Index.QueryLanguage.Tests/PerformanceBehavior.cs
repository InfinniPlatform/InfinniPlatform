using System;
using System.Collections.Generic;
using System.Diagnostics;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.QueryLanguage.Implementation;
using Moq;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Index.QueryLanguage.Tests
{
	
	[TestFixture]
	[Category(TestCategories.PerformanceTest)]
	public sealed class PerformanceBehavior
	{
        private IFilterBuilder _filterFactory = FilterBuilderFactory.GetInstance();

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
									#region ObjectsMetadata level = 2	
									ObjectsMetadata = new object[]
										                  {
											                  new
												                  {
													                  MetadataId = "Patient",
																	  ServiceConfiguration = new []
																		                         {
																			                        "Search", 
																									"Post"
																		                         }																			                       																																										                         
																	  
												                  },
															  new
																  {
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
													#region FieldsMetadata level 3
                                                    FieldsMetadata = new[] 
                                                        {
                                                            new {
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
                                                            new {
                                                                    Type = "Update",
                                                                    Url = "http://localhost:456"    
                                                            }
                                                        }
													#endregion
                                                },
                                            new
                                                {
                                                    MetadataId = "REF_CLPHPOINTER",
													#region FieldsMetadata level 3
                                                    FieldsMetadata = new []
                                                        {
															
                                                            new {
                                                                    
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

				                                                        MetadataId = "ID",
				                                                        MetadataName = "Идентификатор группы",
				                                                        DataFieldName = "Id"
                                                                }
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
                                                            new {
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
                        }
				#endregion
			};
			return JObject.FromObject(config);
		}

		private List<JObject> CreateTestData(int countObjects)
		{
			var listQuery = new List<JObject>();
			var config = CreateJConfiguration();

			for (int i = 0; i < countObjects; i++)
			{
				listQuery.Add(config);
			}
			return listQuery;
		}

		[TestCase(100, 0.001)]
		[TestCase(1000, 0.001)]
		[TestCase(10000, 0.001)]
		[TestCase(100000, 0.001)]
		public void ShouldMakeSelectQueryFromObjectsNoLongerThanOneMicrosecond(int countObjects, double resultMilliseconds)
		{
			//Given
			var listQuery = CreateTestData(countObjects);

			var criteria = new WhereObject()
			{
				RawProperty = "Configurations.$.ConfigurationId",
				Property = "Configurations.$.ConfigurationId",
				CriteriaType = CriteriaType.IsEquals,
				Value = "DrugsVidal"
			};

			//warm up test
			listQuery.FilterItems(new[] { criteria });

			//When

			var watch = Stopwatch.StartNew();

			listQuery.FilterItems(new[] { criteria });

			watch.Stop();

			//Then
			Console.WriteLine(string.Format("elapsed milliseconds: {0}", watch.ElapsedMilliseconds));
			Assert.True(watch.ElapsedMilliseconds / countObjects < resultMilliseconds);
		} 

		[TestCase(100, 0.002)]
		[TestCase(1000, 0.002)]
		[TestCase(10000, 0.002)]
		[TestCase(100000, 0.002)]
		public void ShouldMakeFullQueryWithoutIndexingNoLongerThanTwoMicroseconds(int countObjects, double resultMilliseconds)
		{
			//Given
			var jquery = JObject.FromObject(new
			{
				From = new
				{
					Index = "test",
					Type = "test",
					Alias = "config"
				},
				Where = new[] {
							new {
								Property = "Configurations.$.ConfigurationId",
								CriteriaType = CriteriaType.IsEquals,
								Value = "DrugsVidal"
							}
						},
				Select = new[]
				{
					"Configurations.$.ObjectsMetadata.$.MetadataId"
				}
			});

			var listQuery = CreateTestData(countObjects);

			var mockFactory = new Mock<IIndexFactory>();
			var mockIndexQueryExecutor = new Mock<IIndexQueryExecutor>();
            mockFactory.Setup(m => m.BuildIndexQueryExecutor(It.IsAny<string>(), It.IsAny<string>(), AuthorizationStorageExtensions.AnonimousUser)).Returns(mockIndexQueryExecutor.Object);
			mockIndexQueryExecutor.Setup(m => m.QueryOverObject(It.IsAny<SearchModel>(), It.IsAny<Func<dynamic, string, string, object>>()))
				.Returns(new SearchViewModel(0,countObjects,countObjects,listQuery));

			//When
			var indexQueryExecutor = new JsonQueryExecutor(mockFactory.Object, _filterFactory,AuthorizationStorageExtensions.AnonimousUser);

			//warmup
			indexQueryExecutor.ExecuteQuery(jquery);

			//Then
			var watch = Stopwatch.StartNew();
			indexQueryExecutor.ExecuteQuery(jquery);
			watch.Stop();

			Console.WriteLine(string.Format("elapsed milliseconds: {0}", watch.ElapsedMilliseconds));
			Assert.True(watch.ElapsedMilliseconds / countObjects < resultMilliseconds);
		}

	}
}
