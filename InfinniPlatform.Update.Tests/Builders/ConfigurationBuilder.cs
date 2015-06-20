using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.BlobStorage;
using InfinniPlatform.Cassandra.Client;
using InfinniPlatform.Compression;
using InfinniPlatform.Hosting;
using InfinniPlatform.Hosting.Implementation.ServiceRegistration;
using InfinniPlatform.Hosting.Implementation.ServiceTemplates;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Json;
using InfinniPlatform.Metadata;
using InfinniPlatform.Metadata.Implementation.Handlers;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.Metadata.Implementation.MetadataConfiguration;
using InfinniPlatform.Modules;
using InfinniPlatform.Runtime.Factories;
using InfinniPlatform.Runtime.Implementation.ChangeListeners;
using InfinniPlatform.Sdk.Application.Dynamic;
using InfinniPlatform.SystemConfig.RoutingFactory;
using InfinniPlatform.Update.Installers;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Update.Tests.Builders
{
    public static class ConfigurationBuilder
    {
        private const string ConfigurationModule = "InfinniConfiguration.Integration.dll";

        private static void RebuildIndex(string indexName)
        {
            var elasticFactory = new ElasticFactory(new RoutingFactoryBase());
            IIndexStateProvider indexProvider = elasticFactory.BuildIndexStateProvider();
            indexProvider.CreateIndexType(indexName, string.Empty, deleteExistingVersion: true);
        }

        public static void RefreshData()
        {
            new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider().Refresh();
        }


        public static ApplyChangesHandler BuildQueryContextUpdate(this ApplyChangesHandler query)
        {
            var documentContentConfigRequestProvider = new LocalDataProvider("update", "package", "install", null, null);
            query.ConfigRequestProvider = documentContentConfigRequestProvider;
            return query;
        }


        private static IMetadataConfigurationProvider InitConfig(Type configurationModule)
        {
            IServiceTemplateConfiguration templateConfig =
                new ServiceTemplateConfiguration().CreateDefaultServiceConfiguration();
            var metadataConfigurationProvider = new MetadataConfigurationProvider(
                new ServiceRegistrationContainerFactory(templateConfig), templateConfig);
            var installer =
                (IModuleInstaller)
                Activator.CreateInstance(configurationModule, metadataConfigurationProvider,
                                         BuildActionConfig(metadataConfigurationProvider));
            var config = (IMetadataConfiguration) installer.InstallModule();
            config.ScriptConfiguration.InitActionUnitStorage(null);
            return metadataConfigurationProvider;
        }

        private static ScriptConfiguration BuildActionConfig(
            IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            var cassandraFactory = new CassandraDatabaseFactory();
            var blobStorageFactory = new CassandraBlobStorageFactory(cassandraFactory);

            return
                new ScriptConfiguration(
                    new ScriptFactoryBuilder(
                        new ConfigurationObjectBuilder(new ElasticFactory(new RoutingFactoryBase()), blobStorageFactory,
                                                       metadataConfigurationProvider), new ChangeListener()));
        }

        public static IMetadataConfigurationProvider CreateConfigurationUpdate()
        {
            return InitConfig(typeof (UpdateInstaller));
        }

        public static IEnumerable<dynamic> CreateEventsFromArchive()
        {
            var result = new List<dynamic>();
            JsonArrayStreamEnumerable jsonEnumerable =
                new GZipDataCompressor().ReadAsJsonEnumerable(Path.Combine(Directory.GetCurrentDirectory(),
                                                                           @"TestData\TestArchive.zip"));

            foreach (object json in jsonEnumerable)
            {
                result.Add(((JObject) json).ToObject<dynamic>());
            }
            return result;
        }

        public static dynamic GetPackages()
        {
            var factory = new ElasticFactory(new RoutingFactoryBase());
            factory.BuildIndexStateProvider().Refresh();


            var model = new SearchModel();
            model.AddSort("TimeStamp", SortOrder.Descending);
            return
                factory.BuildIndexQueryExecutor("update_package", string.Empty,
                                                AuthorizationStorageExtensions.AnonimousUser)
                       .Query(model)
                       .Items.ToList();
        }

        public static void InitScriptStorage()
        {
            IIndexStateProvider indexUpdater = new ElasticFactory(new RoutingFactoryBase()).BuildIndexStateProvider();
            indexUpdater.RecreateIndex("update", "package");

            Guid itemId = Guid.NewGuid();
            Guid contentId = Guid.NewGuid();

            dynamic item = new DynamicWrapper();
            item.Id = itemId;
            item.ModuleId = ConfigurationModule;
            item.Version = "version1";
            item.ContentId = contentId;

            ICrudOperationProvider provider =
                new ElasticFactory(new RoutingFactoryBase()).BuildCrudOperationProvider("update", "package",
                                                                                        AuthorizationStorageExtensions
                                                                                            .AnonimousUser, null);
            provider.Set(item);
            provider.Refresh();


            byte[] assemblyData =
                File.ReadAllBytes(Path.Combine(Directory.GetCurrentDirectory(), @"TestData\TestAssemblies",
                                               ConfigurationModule));
            var cassandraFactory = new CassandraDatabaseFactory();
            var blobStorageFactory = new CassandraBlobStorageFactory(cassandraFactory);
            IBlobStorage blobStorage = blobStorageFactory.CreateBlobStorage();
            blobStorage.SaveBlob(contentId, ConfigurationModule, assemblyData);
        }
    }
}