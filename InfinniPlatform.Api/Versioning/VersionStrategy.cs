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
                    return (_configVersions ?? (_configVersions = LoadUserVersions())).ToList();
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

            if (userConfigVersion != null && userConfigVersion.Version != null)
            {
                var userVersion = new Version(userConfigVersion.Version);
                var actualBuildVersion =
                    actualConfigVersions
                       .Where(v => v.Major == userVersion.Major && v.Minor == userVersion.Minor ).OrderByDescending(v => v).FirstOrDefault() ??
                    actualConfigVersions
                       .Where(v => v.Major == userVersion.Major).OrderByDescending(v => v).FirstOrDefault();


                if (actualBuildVersion != null)
                {
                    //если сохраненная минорная версия не является актуальной, обновляем минорную версию
                    if(userVersion.CompareTo(actualBuildVersion) < 0)
                    {
                        userConfigVersion.Version = actualBuildVersion.ToString();
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

        /// <summary>
        ///   Получить список неактуальных версий
        /// </summary>
        /// <returns>Список соответствия актуальных и неактуальных версий конфигураций</returns>
        public IEnumerable<dynamic> GetIrrelevantVersionList(IEnumerable<Tuple<string, string>> configurationVersions, string userName)
        {
            //получаем список версий зарегистрировавшегося пользователя
            dynamic userConfigVersionList = ConfigVersions.Cast<dynamic>().Where(v => v.UserName == userName);

            //получили список всех поднятых на сервере конфигураций
            var actualConfigVersions = configurationVersions.Select(r =>
                                                                        {
                                                                            var versionObject = r.Item2 != null ? new Version(r.Item2) : new Version("0.0.0.0");
                                                                            var majorVersion = versionObject.Major > 0
                                                                                                   ? versionObject.Major
                                                                                                   : 0;
                                                                            var minor = versionObject.Minor > 0
                                                                                                   ? versionObject.Minor
                                                                                                   : 0;
                                                                            return new
                                                                            {
                                                                                ConfigId = r.Item1,
                                                                                Version = r.Item2 != null ?
                                                                                    new Version(majorVersion, minor, 0, 0) : new Version("0.0.0.0")
                                                                            };
                                                                         })
                                                           .OrderByDescending(r => r.Version)
                                                           .ToList();

            var result = new List<dynamic>();
            foreach (var userConfig in userConfigVersionList)
            {
                var userVersion = new Version(userConfig.Version);
                var actualVersion = actualConfigVersions.Where(a => a.ConfigId == userConfig.ConfigurationId).Select(a => a.Version).FirstOrDefault();

                if (actualVersion != null && (actualVersion.Major > userVersion.Major || actualVersion.Minor > userVersion.Minor) )
                {
                    result.Add(new
                        {
                            userConfig.ConfigurationId,
                            userConfig.Version,
                            ActualVersion = actualVersion.ToString()
                        });
                }                
            }
            return result;

        }

        public void SetRelevantVersion(string version, string configurationId, string userName)
        {
            if (configurationId == null)
            {
                return;
            }

            dynamic userConfigVersion = ConfigVersions.Cast<dynamic>().FirstOrDefault(v => v.ConfigurationId.ToLowerInvariant() == configurationId.ToLowerInvariant() &&
                                  v.UserName == userName);

            var versionProvider = _indexFactory.BuildVersionProvider(AuthorizationStorageExtensions.AuthorizationConfigId,
                                       AuthorizationStorageExtensions.VersionStore, "system", null);

            if (userConfigVersion == null)
            {
                userConfigVersion = new
                    {
                        ConfigurationId = configurationId,
                        UserName = userName,                        
                    }.ToDynamic();

                _configVersions.Add(userConfigVersion);
            }

            userConfigVersion.Version = version;

            versionProvider.SetDocument(userConfigVersion);
        }
    }
}
