using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using InfinniConfiguration.Update.Tests.Builders;

using InfinniPlatform;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi;
using InfinniPlatform.Api.RestApi.TestServerApi;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Metadata;
using InfinniPlatform.SearchOptions;

using NUnit.Framework;

namespace InfinniConfiguration.Update.Tests.Integrated
{
	[TestFixture]
	[Category(TestCategories.BusinessLogicTest)]
	public sealed class ServicesBehavior
	{
		private const string VersionIdentifier = "version1";
		private const string AnotherVersionIdentifier = "anotherversion";

		private const string AnotherConfigurationModuleSample = "AnotherSampleConfiguration.dll";

		[TestFixtureSetUp]
		public void SetupFixture()
		{
            TestApi.TestServer.Start(
                p => p.SetPort(9999)
                    .SetServerName("localhost"));

			IndexApi.RebuildIndex("update","package");

		}

		[Test]
		public void ShouldInstallConfigurationNewVersion()
		{
			//given
			var query = ConfigurationBuilder.BuildApplyChangesQuery(_config).BuildQueryContextUpdate();

			ConfigurationBuilder.InitScriptStorage();


			//when

			var result = query.ApplyEventsWithMetadata(null, new[] { ConfigurationBuilder.BuildPackageHeader(ConfigurationBuilder.ConfigurationModule), 
				ConfigurationBuilder.BuildVersionHeader(AnotherVersionIdentifier), 
				ConfigurationBuilder.BuildPackageContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(),@"TestData\TestAssemblies\InfinniConfiguration.Integration.dll"  ))) });

			//then
			//создали новую версию модуля для новой конфигурации
			Assert.IsNotNull(result);
			Assert.AreEqual(result.ModuleId, ConfigurationBuilder.ConfigurationModule);
			Assert.AreEqual(result.Status, "Published");
			Assert.AreEqual(result.Version, AnotherVersionIdentifier);
			Assert.IsNotNull(result.ContentId);
			//добавили новую конфигурацию и новый модуль
			dynamic packages = ConfigurationBuilder.GetPackages();
			Assert.AreEqual(1, packages.Count);

			Assert.AreEqual(AnotherVersionIdentifier, packages[0].Version);


		}

		[Test]
		public void ShouldInstallConfigurationAddModuleInVersion()
		{
			//given
			var query = ConfigurationBuilder.BuildApplyChangesQuery(_config).BuildQueryContextUpdate();

			ConfigurationBuilder.InitScriptStorage();


			//when

			var result = query.ApplyEventsWithMetadata(null, new[] { ConfigurationBuilder.BuildPackageHeader(AnotherConfigurationModuleSample), 
				ConfigurationBuilder.BuildVersionHeader(VersionIdentifier), 
				ConfigurationBuilder.BuildPackageContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(),@"TestData\TestAssemblies\InfinniConfiguration.Integration.dll"  ))) });

			//then
			//создали новую версию модуля для новой конфигурации
			Assert.IsNotNull(result);
			Assert.AreEqual(result.ModuleId, AnotherConfigurationModuleSample);
			Assert.AreEqual(result.Status, "Published");
			Assert.AreEqual(result.Version, VersionIdentifier);
			Assert.IsNotNull(result.ContentId);
			//добавили новую конфигурацию и новый модуль
			dynamic packages = ConfigurationBuilder.GetPackages();
			Assert.AreEqual(1, packages.Count);

			Assert.AreEqual(VersionIdentifier, packages[0].Version);


		}

		[Test]
		public void ShouldInstallConfigurationUpdateVersionOfPackage()
		{
			//given
			var query = ConfigurationBuilder.BuildApplyChangesQuery(_config).BuildQueryContextUpdate();

			ConfigurationBuilder.InitScriptStorage();


			//when

			var result = query.ApplyEventsWithMetadata(null, new[] { ConfigurationBuilder.BuildPackageHeader(ConfigurationBuilder.ConfigurationModule), 
				ConfigurationBuilder.BuildVersionHeader(VersionIdentifier), 
				ConfigurationBuilder.BuildPackageContent(File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(),@"TestData\TestAssemblies\InfinniConfiguration.Integration.dll"  ))) });

			//then
			//создали новую версию модуля для новой конфигурации
			Assert.IsNotNull(result);
			Assert.AreEqual(result.ModuleId, ConfigurationBuilder.ConfigurationModule);
			Assert.AreEqual(result.Status, "Published");
			Assert.AreEqual(result.Version, VersionIdentifier);
			Assert.IsNotNull(result.ContentId);
			//добавили новую конфигурацию и новый модуль
			dynamic packages = ConfigurationBuilder.GetPackages();
			Assert.AreEqual(1, packages.Count);

			Assert.AreEqual(VersionIdentifier, packages[0].Version);

		}


		[Test]
		public void ShouldSearchPackage()
		{
			//given
			var query = ConfigurationBuilder.BuildSearchQuery(_config).BuildQueryContextSearch();

			var itemId = Guid.NewGuid();
			var contentId = Guid.NewGuid();

			dynamic item = new DynamicInstance();
			item.Id = itemId;
			item.ModuleId = "TestConfig";
			item.Version = VersionIdentifier;
			item.Status = "Published";
			item.ContentId = contentId;

            var provider = new ElasticFactory().BuildCrudOperationProvider("update_package");
			provider.Set(item);
			provider.Refresh();

			var cassandraFactory = new CassandraDatabaseFactory();
			var blobStorageFactory = new CassandraBlobStorageFactory(cassandraFactory);
			var blobStorage = blobStorageFactory.CreateBlobStorage();
			blobStorage.AddData(contentId, "InfinniConfiguration.Update", Encoding.UTF8.GetBytes("somedata"));

			//when
			dynamic filter = new DynamicInstance();
			filter.Property = "Id";
			filter.Value = itemId;
			filter.CriteriaType = CriteriaType.IsEquals;

			var filterObject = new List<dynamic>
				                   {
					                   filter
				                   };

			dynamic searchResult = query.GetSearchResult(filterObject, 0, 1);
			//then
			Assert.AreEqual(1, searchResult.Count);
			Assert.AreEqual(Encoding.UTF8.GetString(searchResult[0].Assembly.ToArray()), "somedata");
		}
	}
}