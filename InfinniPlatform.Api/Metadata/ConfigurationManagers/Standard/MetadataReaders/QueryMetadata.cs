using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions;
using InfinniPlatform.Sdk.Dynamic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    public static class QueryMetadata
    {
        /// <summary>
        ///     Выполнить IQL запрос к метаданным конфигурации
        /// </summary>
        /// <param name="version">Версия конфигурации</param>
        /// <param name="iqlQuery">IQL запрос</param>
        /// <param name="doNotCheckVersion">Не проверять версию метаданных</param>
        /// <returns>Выборка из конфигурации</returns>
        public static IEnumerable<dynamic> QueryConfiguration(string version, string iqlQuery,
            bool doNotCheckVersion = false)
        {
            dynamic jsonQuery;
            try
            {
                jsonQuery = iqlQuery.ToDynamic();
            }
            catch
            {
                throw new ArgumentException("Fail to parse IQL query. There is a syntax error.");
            }

            jsonQuery.DoNotCheckVersion = doNotCheckVersion;

            RestQueryResponse response = RestQueryApi.QueryPostJsonRaw("systemconfig", "metadata", "getmetadata", null,
                jsonQuery, version);

            return response.GetResults().ToList();
        }

        private static IEnumerable<dynamic> GetResults(this RestQueryResponse response)
        {
            //индекс 0 в нижеследующем выражении используется, так как в данном случае идет запрос к конкретной конфигурации
            //соответственно, будет возвращена выборка, содержащая данные из одной конфигурации, но в общем случае выборка может быть и из нескольких

            if (response.ToDynamic().QueryResult.Count > 0)
            {
                IEnumerable<dynamic> results = DynamicWrapperExtensions.ToEnumerable(response.ToDynamic().QueryResult);
                return results.Select(r => r.Result).ToList();
            }
            return new List<dynamic>();
        }

        /// <summary>
        ///     Получить список заголовков конфигураций
        /// </summary>
        /// <returns></returns>
        public static string GetConfigurationShortListIql()
        {
            return JObject.FromObject(new
            {
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
                    "Documents.$"
                }
            }).ToString();
        }

        public static string GetConfigurationMetadataShortListIql(string configurationId, string metadataContainer)
        {
            return JObject.FromObject(new
            {
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

        public static string GetDocumentMetadataShortListIql(string configurationId, string documentId,
            string metadataContainer)
        {
            return JObject.FromObject(new
            {
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

        public static string GetDocumentMetadataByNameIql(string configurationId, string documentId, string metadataName,
            string metadataContainer, string metadataType)
        {
            return JObject.FromObject(new
            {
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

        public static string GetConfigurationMetadataByNameIql(string configurationId, string metadataName,
            string metadataContainer, string metadataType)
        {
            return JObject.FromObject(new
            {
                ConfigId = configurationId,
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
                        Index = string.Format("systemconfig"),
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