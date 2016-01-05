using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Environment.Index;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Core.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    public static class QueryMetadata
    {
        /// <summary>
        /// Выполнить IQL запрос к метаданным конфигурации
        /// </summary>
        /// <param name="iqlQuery">IQL запрос</param>
        /// <param name="doNotCheckVersion">Не проверять версию метаданных</param>
        /// <returns>Выборка из конфигурации</returns>
        public static IEnumerable<dynamic> QueryConfiguration(string iqlQuery, bool doNotCheckVersion = false)
        {
            return Enumerable.Empty<object>();
        }

        public static string GetConfigurationMetadataShortListIql(string configurationId, string metadataContainer)
        {
            return JObject.FromObject(new
                                      {
                                          Version = "",
                                          ConfigId = configurationId,
                                          From = new
                                                 {
                                                     Index = "systemconfig",
                                                     Type = "metadata"
                                                 },
                                          Select = new[]
                                                   {
                                                       "Id",
                                                       "Name",
                                                       "Caption",
                                                       "Description",
                                                       "Version",
                                                       string.Format("{0}.$.Id", metadataContainer),
                                                       string.Format("{0}.$.Name", metadataContainer),
                                                       string.Format("{0}.$.Caption", metadataContainer)
                                                   }
                                      }).ToString();
        }

        public static string GetDocumentMetadataShortListIql(string configurationId, string documentId, string metadataContainer)
        {
            return JObject.FromObject(new
                                      {
                                          Version = "",
                                          ConfigId = configurationId,
                                          DocumentId = documentId,
                                          From = new
                                                 {
                                                     Index = "systemconfig",
                                                     Type = "documentmetadata"
                                                 },
                                          Select = new[]
                                                   {
                                                       "Id",
                                                       "Name",
                                                       "Caption",
                                                       "Description",
                                                       "Version",
                                                       string.Format("{0}.$.Id", metadataContainer),
                                                       string.Format("{0}.$.Name", metadataContainer),
                                                       string.Format("{0}.$.Caption", metadataContainer)
                                                   }
                                      }).ToString();
        }

        public static string GetDocumentMetadataByNameIql(string configurationId, string documentId, string metadataName, string metadataContainer, string metadataType)
        {
            return JObject.FromObject(new
                                      {
                                          Version = "",
                                          ConfigId = configurationId,
                                          DocumentId = documentId,
                                          From = new
                                                 {
                                                     Index = "systemconfig",
                                                     Type = "documentmetadata"
                                                 },
                                          Select = new[]
                                                   {
                                                       string.Format("{0}Full", metadataType)
                                                   },
                                          Join = new[]
                                                 {
                                                     new
                                                     {
                                                         Index = "systemconfig",
                                                         Alias = string.Format("{0}Full", metadataType),
                                                         Path = string.Format("{0}.$.Id", metadataContainer),
                                                         Type = string.Format("{0}metadata", metadataType)
                                                     }
                                                 },
                                          Where = new[]
                                                  {
                                                      new
                                                      {
                                                          Property = string.Format("{0}Full.Name", metadataType),
                                                          CriteriaType = CriteriaType.IsEquals,
                                                          Value = metadataName
                                                      }
                                                  }
                                      }).ToString();
        }

        public static string GetConfigurationMetadataByNameIql(string configurationId, string metadataName, string metadataContainer, string metadataType)
        {
            return JObject.FromObject(new
                                      {
                                          ConfigId = configurationId,
                                          Version = "",
                                          From = new
                                                 {
                                                     Index = "systemconfig",
                                                     Type = "metadata"
                                                 },
                                          Select = new[]
                                                   {
                                                       string.Format("{0}Full", metadataType)
                                                   },
                                          Join = new[]
                                                 {
                                                     new
                                                     {
                                                         Index = "systemconfig",
                                                         Alias = string.Format("{0}Full", metadataType),
                                                         Path = string.Format("{0}.$.Id", metadataContainer),
                                                         Type = string.Format("{0}metadata", metadataType)
                                                     }
                                                 },
                                          Where = new[]
                                                  {
                                                      new
                                                      {
                                                          Property = string.Format("{0}Full.Name", metadataType),
                                                          CriteriaType = CriteriaType.IsEquals,
                                                          Value = metadataName
                                                      }
                                                  }
                                      }).ToString();
        }
    }
}