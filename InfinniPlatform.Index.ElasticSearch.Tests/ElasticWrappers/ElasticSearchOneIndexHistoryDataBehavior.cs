using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.Index.ElasticSearch.Implementation.Extensions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters;
using InfinniPlatform.Index.ElasticSearch.Implementation.Versioning.IndexStrategies;
using InfinniPlatform.SearchOptions;
using NUnit.Framework;

namespace InfinniPlatform.Index.ElasticSearch.Tests.ElasticWrappers
{
	[TestFixture]
	[Category(TestCategories.IntegrationTest)]
	public sealed class ElasticSearchOneIndexHistoryDataBehavior
	{
		private IVersionProvider _elasticSearchVersionedData;
		private IIndexStateProvider _indexStateProvider;
		private ICrudOperationProvider _elasticSearchProvider;

		private void IndexWithTimestamp(string index, dynamic item, DateTime timeStamp)
		{
			var elasticConnection = new ElasticConnection();
			elasticConnection.ConnectIndex();
			var indexObject = IndexObjectExtension.ToIndexObject(item, new InsertItemStrategy());
			indexObject.TimeStamp = timeStamp;
			elasticConnection.Client.Index(indexObject,index,index);
		}


		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
            _indexStateProvider = new ElasticFactory().BuildIndexStateProvider();
            _indexStateProvider.RecreateIndex("testperson","testperson");

			_elasticSearchVersionedData = new ElasticFactory().BuildVersionProviderManager().BuildVersionProvider("testperson",  "testperson", VersionProviderType.HistoryStrategyOneIndexHistory);
            _elasticSearchProvider = new ElasticFactory().BuildCrudOperationProvider("testperson", "testperson");
		}

		[Test]
		public void ShouldFindActualVersion()
		{
			dynamic instance1 = new DynamicInstance();
			instance1.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance1.Status = "Published";
			instance1.TestValue = "v1";
			instance1.IsValid = true;
			IndexWithTimestamp("testperson", instance1, new DateTime(2012, 01, 01));


			dynamic instance2 = new DynamicInstance();
			instance2.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance2.Status = "Published";
			instance2.TestValue = "v2";
			instance2.IsValid = true;
			IndexWithTimestamp("testperson", instance2, new DateTime(2012, 02, 01));

			_elasticSearchProvider.Refresh();

			var actualVersion = _elasticSearchVersionedData.GetDocument("{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant());

			Assert.IsNotNull(actualVersion);
			Assert.AreEqual(actualVersion.TestValue, "v2");
		}

		[Test]
		public void ShouldFindVersionByStatus()
		{
			dynamic instance1 = new DynamicInstance();
			instance1.Id = "{C74887E5-D039-4215-977C-05BC827B6589}".ToLowerInvariant();
			instance1.Status = "Published";
			instance1.IsValid = true;
			instance1.TestValue = "1";

			IndexWithTimestamp("testperson", instance1, new DateTime(2012, 01, 01));

			dynamic instance2 = new DynamicInstance();
			instance2.Id = "{C74887E5-D039-4215-977C-05BC827B6589}".ToLowerInvariant();
			instance2.Status = "Published";
			instance2.IsValid = true;
			instance2.TestValue = "2";

			IndexWithTimestamp("testperson", instance2, new DateTime(2012, 02, 01));

			dynamic instance3 = new DynamicInstance();
			instance3.Id = "{C74887E5-D039-4215-977C-05BC827B6589}".ToLowerInvariant();
			instance3.TestValue = "3";
			instance3.Status = "Published";
			instance3.IsValid = true;

			IndexWithTimestamp("testperson", instance3, new DateTime(2010, 02, 01));


			_elasticSearchProvider.Refresh();

            var actualVersion = _elasticSearchVersionedData.GetDocument("{C74887E5-D039-4215-977C-05BC827B6589}".ToLowerInvariant());

			Assert.IsNotNull(actualVersion);
			Assert.AreEqual(actualVersion.TestValue.ToString(), "2");
		}

		[TestCase(0, 10, 3, "v1,v2,v3")]
		[TestCase(1, 2, 1, "v1,v2,v3")]
		public void ShouldFindAllVersionsByFilter(int pageNumber, int pageSize, int expectedCount,
												  string expectedVersions)
		{
			_indexStateProvider.RecreateIndex("testperson","testperson");

			dynamic instance1 = new DynamicInstance();
			instance1.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance1.Status = "Published";
			instance1.TestValue = "v1";
			instance1.IsValid = true;
			instance1.TestProperty = "Иван1";
			IndexWithTimestamp("testperson", instance1, new DateTime(2012, 01, 01));


			dynamic instance2 = new DynamicInstance();
			instance2.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance2.Status = "Published";
			instance2.TestValue = "v2";
			instance2.IsValid = true;
			instance2.TestProperty = "Иван1";
			IndexWithTimestamp("testperson", instance2, new DateTime(2012, 02, 01));

			dynamic instance3 = new DynamicInstance();
			instance3.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance3.Status = "Published";
			instance3.TestValue = "v3";
			instance3.IsValid = true;
			instance3.TestProperty = "Иван1";
			IndexWithTimestamp("testperson", instance3, new DateTime(2012, 03, 01));

			dynamic instance4 = new DynamicInstance();
			instance4.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance4.Status = "Published";
			instance4.TestValue = "v4";
			instance4.IsValid = true;
			instance4.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance4, new DateTime(2012, 04, 01));

			dynamic instance5 = new DynamicInstance();
			instance5.Id = "{F5B40A6B-0DE8-49E8-B15D-21FE8E5D8787}".ToLowerInvariant();
			instance5.Status = "Published";
			instance5.TestValue = "v5";
			instance5.IsValid = true;
			instance5.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance5, new DateTime(2012, 03, 01));

			dynamic instance6 = new DynamicInstance();
			instance6.Id = "{F5B40A6B-0DE8-49E8-B15D-21FE8E5D8787}".ToLowerInvariant();
			instance6.Status = "Published";
			instance6.TestValue = "v6";
			instance6.IsValid = true;
			instance6.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance6, new DateTime(2012, 04, 01));

			dynamic instance7 = new DynamicInstance();
			instance7.Id = "{E8B8FB27-9219-4802-933D-5288F69E0CD9}".ToLowerInvariant();
			instance7.Status = "Published";
			instance7.TestValue = "v7";
			instance7.IsValid = true;
			instance7.TestProperty = "Петр";
			IndexWithTimestamp("testperson", instance7, new DateTime(2012, 04, 01));

			_elasticSearchProvider.Refresh();

		    var criteria = new Criteria
		                       {
		                           Property = "TestProperty",
		                           Value = "Иван1"
		                       };

			var filterObject = new List<Criteria>()
				                   {
					                   criteria
				                   };

			var versionByFilter = _elasticSearchVersionedData.GetDocumentHistory(filterObject, pageNumber, pageSize).ToList();

			Assert.IsNotNull(versionByFilter);
			Assert.AreEqual(versionByFilter.Count, expectedCount);
			for (int i = 0; i < versionByFilter.Count; i++)
			{
				Assert.True(expectedVersions.Contains(versionByFilter[i].TestValue));
			}
		}


		[Test]
		public void ShouldFindActualVersionByFilter()
		{
			
			dynamic instance1 = new DynamicInstance();
			instance1.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance1.Status = "Published";
			instance1.TestValue = "v1";
			instance1.IsValid = true;
			instance1.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance1, new DateTime(2012, 01, 01));

			dynamic instance2 = new DynamicInstance();
			instance2.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance2.Status = "Published";
			instance2.TestValue = "v2";
			instance2.IsValid = true;
			instance2.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance2, new DateTime(2012, 02, 01));

			dynamic instance3 = new DynamicInstance();
			instance3.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance3.Status = "Published";
			instance3.TestValue = "v3";
			instance3.IsValid = true;
			instance3.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance3, new DateTime(2012, 03, 01));

			dynamic instance31 = new DynamicInstance();
			instance31.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance31.Status = "Saved";
			instance31.TestValue = "v31";
			instance31.IsValid = true;
			instance31.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance31, new DateTime(2012, 04, 01));

			dynamic instance4 = new DynamicInstance();
			instance4.Id = "{C74887E5-D039-4215-977C-05BC827B6585}".ToLowerInvariant();
			instance4.Status = "Invalid";
			instance4.TestValue = "v4";
			instance4.IsValid = true;
			instance4.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance4, new DateTime(2012, 05, 01));




			dynamic instance5 = new DynamicInstance();
			instance5.Id = "{F5B40A6B-0DE8-49E8-B15D-21FE8E5D8787}".ToLowerInvariant();
			instance5.Status = "Published";
			instance5.TestValue = "v5";
			instance5.IsValid = true;
			instance5.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance5, new DateTime(2012, 03, 01));

			dynamic instance51 = new DynamicInstance();
			instance51.Id = "{F5B40A6B-0DE8-49E8-B15D-21FE8E5D8787}".ToLowerInvariant();
			instance51.Status = "Invalid";
			instance51.TestValue = "v51";
			instance51.IsValid = true;
			instance51.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance51, new DateTime(2012, 04, 01));

			dynamic instance6 = new DynamicInstance();
			instance6.Id = "{F5B40A6B-0DE8-49E8-B15D-21FE8E5D8787}".ToLowerInvariant();
			instance6.Status = "Saved";
			instance6.TestValue = "v6";
			instance6.IsValid = true;
			instance6.TestProperty = "Иван";
			IndexWithTimestamp("testperson", instance6, new DateTime(2012, 04, 01));



			dynamic instance7 = new DynamicInstance();
			instance7.Id = "{E8B8FB27-9219-4802-933D-5288F69E0CD9}".ToLowerInvariant();
			instance7.Status = "Published";
			instance7.TestValue = "v7";
			instance7.IsValid = true;
			instance7.TestProperty = "Петр";
			IndexWithTimestamp("testperson", instance7, new DateTime(2012, 04, 01));

			dynamic instance8 = new DynamicInstance();
			instance8.Id = "{E8B8FB27-9219-4802-933D-5288F69E0CD9}".ToLowerInvariant();
			instance8.Status = "Saved";
			instance8.TestValue = "v8";
			instance8.IsValid = true;
			instance8.TestProperty = "Петр";
			IndexWithTimestamp("testperson", instance8, new DateTime(2012, 09, 01));

			_elasticSearchProvider.Refresh();

		    var criteria = new Criteria
		                       {
		                           Property = "TestProperty",
		                           Value = "Иван"
		                       };

			var filterObject = new List<Criteria>
				                   {
					                   criteria
				                   };

			IEnumerable<dynamic> actualVersion = DynamicInstanceExtensions.ToEnumerable(_elasticSearchVersionedData.GetDocument(filterObject, 0, 2));

			Assert.IsNotNull(actualVersion);
			Assert.AreEqual(actualVersion.Count(), 2);
			var versions = string.Join(";",actualVersion.Select(f => f.TestValue).ToArray());
			Assert.True(versions.Contains("v31"));
			Assert.True(versions.Contains("v6"));
		}

	}
}
