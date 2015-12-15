using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions;

using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders.SchemaIndexVersion
{
    public static class TypeMappingExtensions
    {
        public static IEnumerable<string> GetMappingsTypeNames(this KeyValuePair<string, IList<TypeMapping>> elasticMappings)
        {
            var baseTypes = elasticMappings.Value.Select(elasticMapping => elasticMapping.TypeName);

            return baseTypes;
        }

        public static IEnumerable<string> GetMappingsBaseTypeNames(this KeyValuePair<string, IList<TypeMapping>> elasticMappings)
        {
            var baseTypes = elasticMappings.Value.Select(elasticMapping => elasticMapping.GetTypeBaseName());

            return baseTypes;
        }

        public static string GetTypeBaseName(this TypeMapping typeMapping)
        {
            return typeMapping.TypeName.Replace($"{IndexTypeMapper.MappingTypeVersionPattern}{typeMapping.GetTypeVersion()}", string.Empty);
        }

        public static int? GetTypeVersion(this TypeMapping typeMapping)
        {
            int version;

            return int.TryParse(typeMapping.TypeName.Split('_').Last(), out version)
                       ? (int?)version
                       : null;
        }

        public static string GetTypeBaseName(this string typeName)
        {
            return typeName.Replace($"{IndexTypeMapper.MappingTypeVersionPattern}{typeName.GetTypeVersion()}", string.Empty);
        }

        public static int? GetTypeVersion(this string typeName)
        {
            int version;

            return int.TryParse(typeName.Split('_').Last(), out version)
                       ? (int?)version
                       : null;
        }
    }
}