using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Sdk.Contracts;
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

            //TODO: Везде где размер страницы более 100 элементов, необходимо ввести загрузку данных по пачкам
            IEnumerable<dynamic> configVersions = _indexFactory.BuildVersionProvider(AuthorizationStorageExtensions.AuthorizationConfigId, 
                AuthorizationStorageExtensions.VersionStore, "system", null).GetDocument(null, 0, 1000000);

            dynamic userConfigVersion = configVersions.FirstOrDefault(v => v.ConfigId.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant() &&
                                              v.UserName == userName);

            if (userConfigVersion == null)
            {
                return
                    configurationVersions.Where(
                        r => r.Item1.ToLowerInvariant() == metadataConfigurationId.ToLowerInvariant())
                                         .Select(r => r.Item2)
                                         .OrderByDescending(r =>
                                             {
                                                 double result;
                                                 if (double.TryParse(r, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
                                                 {
                                                     return result;
                                                 }
                                                 return -1;
                                             }).FirstOrDefault();
            }
            return userConfigVersion.Version;
        }

    }
}
