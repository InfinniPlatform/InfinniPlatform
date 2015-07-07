using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Api.Versioning
{
    public sealed class VersionStrategy : IVersionStrategy
    {
        private readonly IIndexFactory _indexFactory;

        public VersionStrategy(IIndexFactory indexFactory)
        {
            _indexFactory = indexFactory;
        }

        private readonly object lockObject = new object();

        private List<object> _configVersions;

        public List<object> ConfigVersions
        {
            get
            {
                lock (lockObject)
                {
                    return _configVersions ?? (_configVersions = LoadUserVersions());
                }
            }
        }

        private List<object> LoadUserVersions()
        {
            var versionProvider =
                _indexFactory.BuildVersionProvider(AuthorizationStorageExtensions.AuthorizationConfigId,
                                                   AuthorizationStorageExtensions.VersionStore, "system", null);

            //TODO: Везде где размер страницы более 100 элементов, необходимо ввести загрузку данных по пачкам
            return versionProvider.GetDocument(null, 0, 1000000);
        }

        /// <summary>
        /// Получить актуальную минорную версию конфигурации для указанного идентификатора версии
        /// </summary>
        /// <param name="metadataConfigurationId">Идентификатор конфигурации</param>
        /// <param name="configurationVersions">Список версий конфигураций</param>
        /// <param name="userName">Логин пользователя</param>
        /// <returns>Актуальная минорная версия конфигурации</returns>
        public string GetActualVersion(string metadataConfigurationId, IEnumerable<Tuple<string, string>> configurationVersions, string userName)
        {
            
            if (metadataConfigurationId == null)
            {
                return null;
            }

            var versionProvider =
                _indexFactory.BuildVersionProvider(AuthorizationStorageExtensions.AuthorizationConfigId,
                                                   AuthorizationStorageExtensions.VersionStore, "system", null);

            //получили сопоставленную пользователю версию конфигурации
            dynamic userConfigVersion = ConfigVersions.Cast<dynamic>().FirstOrDefault(v => v.ConfigurationId.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant() &&
                                              v.UserName == userName);


            //получили список всех поднятых на сервере конфигураций
            var actualConfigVersions = configurationVersions.Where(
                r => r.Item1.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant())
                                                           .Select(r => r.Item2 != null ? new Version(r.Item2) : new Version("0.0.0.0"))
                                                           .OrderByDescending(r => r)
                                                           .ToList();

            if (userConfigVersion != null)
            {
                var userVersion = new Version(userConfigVersion.Version);
                var actualMinorVersion =
                    actualConfigVersions.Where(
                        v => v.Major == userVersion.Major && v.MajorRevision == userVersion.MajorRevision)
                                       .OrderByDescending(v => v).FirstOrDefault();
                
                if (actualMinorVersion != null)
                {
                    //если сохраненная минорная версия не является актуальной, обновляем минорную версию
                    if (userVersion.Minor < actualMinorVersion.Minor ||
                        userVersion.MinorRevision < actualMinorVersion.MinorRevision)
                    {
                        userConfigVersion.Version = actualMinorVersion.ToString();
                        versionProvider.SetDocument(userConfigVersion);
                    }
                    return userConfigVersion.Version;
                }
                return null;
            }
            if (actualConfigVersions.Count > 0)
            {
                var actualVersion = actualConfigVersions.Select(a => a.ToString()).FirstOrDefault();
                

                //для анонимных и неизвестных пользователей не сохраняем версии конфигурации на сервере
                if (userName != AuthorizationStorageExtensions.UnknownUser &&
                    userName != AuthorizationStorageExtensions.AnonimousUser)
                {
                    dynamic storedConfigVersion = new DynamicWrapper();
                    storedConfigVersion.UserName = userName;
                    storedConfigVersion.ConfigurationId = metadataConfigurationId;
                    storedConfigVersion.Version = actualVersion;

                    versionProvider.SetDocument(storedConfigVersion);
                    ConfigVersions.Add(storedConfigVersion);

                    return storedConfigVersion.Version;
                }
                return actualVersion;
            }

            return null;
        }

    }
}
