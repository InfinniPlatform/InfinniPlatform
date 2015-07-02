using System.IO;
using System.Reflection;
using InfinniPlatform.Api.Index;
using InfinniPlatform.Api.Packages;
using InfinniPlatform.Api.RestApi.AuthApi;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;
using InfinniPlatform.Index.ElasticSearch.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.Metadata;
using InfinniPlatform.SystemConfig.Multitenancy;

namespace InfinniPlatform.SystemConfig.Initializers
{
	/// <summary>
	///   Установка системных конфигураций
	/// </summary>
	public sealed class SystemConfigurationsInitializer : IStartupInitializer
	{
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

        private readonly IIndexFactory _indexFactory;

        public SystemConfigurationsInitializer(IMetadataConfigurationProvider metadataConfigurationProvider)
		{
			_metadataConfigurationProvider = metadataConfigurationProvider;

            _indexFactory = new ElasticFactory(new MultitenancyProvider());
		}

		public void OnStart(HostingContextBuilder contextBuilder)
		{

            Logger.Log.Info("System configurations installed");

			var packageBuilder = new PackageBuilder();

			//При создании системных конфигураций не указываем Сборки. Сборки системных пакетов устанавливаются из текущей папки запуска
			var package1 = packageBuilder.BuildSystemPackage("Update", "version_update",
										"InfinniPlatform.Update.dll");
			var package2 = packageBuilder.BuildSystemPackage("RestfulApi", "version_restfulapi",
										"InfinniPlatform.RestfulApi.exe");
			var package3 = packageBuilder.BuildSystemPackage("SystemConfig", "version_systemconfig",
										"InfinniPlatform.SystemConfig.dll");

            CreateSystemStore("Update");
            CreateSystemStore("RestfulApi");
            CreateSystemStore("SystemConfig");


			UpdateApi.InstallPackages(new[] {package1, package2, package3});

			CreateStorage(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserStore);
			CreateStorage(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.AclStore);
			CreateStorage(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.RoleStore);
			CreateStorage(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.UserRoleStore);
			CreateStorage(AuthorizationStorageExtensions.AuthorizationConfigId, AuthorizationStorageExtensions.ClaimStore);
			

			Logger.Log.Info("System configurations installed");
		}

		private void CreateStorage(string configId, string metadata)
		{
			if (!IndexApi.IndexExists(configId, metadata))
			{
				IndexApi.RebuildIndex(configId,metadata);
			}

		}

		private void CreateSystemStore(string configName)
	    {
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(configName);

            if (metadataConfiguration != null)
            {
                var containers = metadataConfiguration.Containers;
                foreach (var containerId in containers)
                {
                    var versionBuilder = _indexFactory.BuildVersionBuilder(
                        metadataConfiguration.ConfigurationId,
                        metadataConfiguration.GetMetadataIndexType(containerId),
                        metadataConfiguration.GetSearchAbilityType(containerId));
                    
                    // Для системных конфигураций использована упрощенная схема
                    // создания хранилищ - без учета схем данных документов.
                    // На момент написания кода ни для одного документа системной
                    // конфигурации мапинг задан не был. Если в дальнейшем
                    // по каким-то причинам будет необходимо учитывать схему
                    // данных системных документов, необходимо будет использовать механизм
                    // UpdateStoreMigration

					if (!versionBuilder.VersionExists())
					{
						versionBuilder.CreateVersion();
					}
                }
            }
	    }
	}
}
