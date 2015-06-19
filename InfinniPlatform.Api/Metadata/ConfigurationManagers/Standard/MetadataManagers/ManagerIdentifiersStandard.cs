using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class ManagerIdentifiersStandard : IManagerIdentifiers
    {
        /// <summary>
        ///     Получить идентификатор элемента конфигурации
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="name">Наименование элемента</param>
        /// <returns>Идентификатор элемента</returns>
        public string GetConfigurationUid(string version, string name)
        {
            var configList = GetConfigList(version);

            return
                configList.Where(
                    c =>
                        c.Name.ToLowerInvariant() == name.ToLowerInvariant() &&
                        ((c.Version == null || version == null) ||
                         c.Version.ToLowerInvariant() == version.ToLowerInvariant()))
                    .Select(c => c.Id).FirstOrDefault();
        }

        /// <summary>
        ///     Получить идентификатор документа
        /// </summary>
        public string GetDocumentUid(string version, string configurationId, string documentId)
        {
            var config =
                GetConfigList(version)
                    .FirstOrDefault(
                        c =>
                            c.Name.ToLowerInvariant() == configurationId.ToLowerInvariant() &&
                            ((c.Version == null || version == null) ||
                             c.Version.ToLowerInvariant() == version.ToLowerInvariant()));
            if (config != null)
            {
                IEnumerable<dynamic> documents = config.Documents;
                if (config.Documents != null)
                {
                    foreach (var document in documents)
                    {
                        if (document.Name.ToLowerInvariant() == documentId.ToLowerInvariant())
                        {
                            return document.Id;
                        }
                    }
                }
            }
            return null;
        }

        private static IEnumerable<dynamic> GetConfigList(string version)
        {
            IEnumerable<dynamic> configList = DynamicWrapperExtensions.ToEnumerable(
                RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getregisteredconfiglist", null, null, version)
                    .ToDynamic().ConfigList);
            return configList;
        }
    }
}