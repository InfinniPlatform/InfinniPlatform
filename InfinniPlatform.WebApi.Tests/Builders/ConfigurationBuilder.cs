using System;
using System.IO;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Binary;

namespace InfinniPlatform.WebApi.Tests.Builders
{
	public sealed class ConfigurationBuilder
	{
		public static void InitScriptStorage()
		{
			const string assemblyName = "InfinniPlatform.Metadata.Tests.dll";

			var indexUpdater = new ElasticFactory().BuildIndexStateProvider();
			indexUpdater.RecreateIndex("update_package", "update_package");
			indexUpdater.RecreateIndex("update_configuration", "update_configuration");

			dynamic itemConfig = new DynamicWrapper();
			itemConfig.Id = Guid.NewGuid();
			itemConfig.ConfigurationName = "QueryExecutorTest";
			itemConfig.Version = "version_metadatatests";

			var itemId = Guid.NewGuid();
			var contentId = Guid.NewGuid().ToString("N");

			dynamic item = new DynamicWrapper();
			item.Id = itemId;
			item.ConfigurationName = "QueryExecutorTest";
			item.ModuleId = assemblyName;
			item.Version = "version_federal";
			item.ContentId = contentId;

			var providerConfig = new ElasticFactory().BuildCrudOperationProvider("update_configuration", "update_configuration", AuthorizationStorageExtensions.AnonimousUser);
			providerConfig.Set(itemConfig);
			providerConfig.Refresh();

			var provider = new ElasticFactory().BuildCrudOperationProvider("update_package", "update_package", AuthorizationStorageExtensions.AnonimousUser);
			provider.Set(item);
			provider.Refresh();

			var assemblyData = File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), assemblyName));
			var blobStorage = CreateBlobStorage();
			blobStorage.SaveBlob(contentId, assemblyName, assemblyData);
		}

		private static IBlobStorage CreateBlobStorage()
		{
			var blobStorageFactory = new FileSystemBlobStorageFactory();
			return blobStorageFactory.CreateBlobStorage();
		}
	}
}