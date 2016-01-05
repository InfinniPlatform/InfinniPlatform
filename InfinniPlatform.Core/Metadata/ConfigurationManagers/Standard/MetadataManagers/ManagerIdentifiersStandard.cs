using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.RestApi.DataApi;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataManagers
{
    public sealed class ManagerIdentifiersStandard : IManagerIdentifiers
    {
        public string GetSolutionUid(string name)
        {
            var solutionList = GetSolutionList();

            return solutionList
                .Where(c => c.Name.ToLowerInvariant() == name.ToLowerInvariant())
                .Select(c => c.Id)
                .FirstOrDefault();
        }

        /// <summary>
        /// Получить идентификатор элемента конфигурации
        /// </summary>
        /// <param name="name">Наименование элемента</param>
        /// <returns>Идентификатор элемента</returns>
        public string GetConfigurationUid(string name)
        {
            var configList = (IEnumerable<dynamic>)PackageMetadataLoader.GetConfigurations();

            return
                configList.Where(c => c.Name.ToLowerInvariant() == name.ToLowerInvariant())
                          .Select(c => c.Id)
                          .FirstOrDefault();
        }

        /// <summary>
        /// Получить идентификатор документа
        /// </summary>
        public string GetDocumentUid(string configurationId, string documentId)
        {
            var config = ((IEnumerable<dynamic>)PackageMetadataLoader.GetConfigurations())
                .FirstOrDefault(c => c.Name.ToLowerInvariant() == configurationId.ToLowerInvariant());

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

        private IEnumerable<dynamic> GetSolutionList()
        {
            return null;
        }
    }
}