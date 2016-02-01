using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.ElasticSearch.IndexTypeVersions;

using Nest;

namespace InfinniPlatform.ElasticSearch.ElasticProviders.SchemaIndexVersion
{
    public static class TypeMappingExtensions
    {
        public static IEnumerable<string> GetMappingsTypeNames(this KeyValuePair<string, IEnumerable<TypeMapping>> elasticMappings)
        {
            var baseTypes = elasticMappings.Value.Select(elasticMapping => elasticMapping.TypeName);

            return baseTypes;
        }

        public static IEnumerable<string> GetMappingsTypeNames(this IEnumerable<TypeMapping> elasticMappings)
        {
            var baseTypes = elasticMappings.Select(elasticMapping => elasticMapping.TypeName);

            return baseTypes;
        }

        public static IEnumerable<string> GetMappingsBaseTypeNames(this KeyValuePair<string, IEnumerable<TypeMapping>> elasticMappings)
        {
            var baseTypes = elasticMappings.Value.Select(elasticMapping => elasticMapping.GetTypeBaseName());

            return baseTypes;
        }

        public static IEnumerable<string> GetMappingsBaseTypeNames(this IEnumerable<TypeMapping> elasticMappings)
        {
            var baseTypes = elasticMappings.Select(elasticMapping => elasticMapping.GetTypeBaseName());

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

        public static int GetTypeActualVersion(this IEnumerable<TypeMapping> typeMappings, string typeName)
        {
            var actualVersion = typeMappings.Select(mapping => mapping.GetTypeVersion())
                                            .Max()
                                            .GetValueOrDefault();

            return actualVersion;
        }

        public static string GetTypeActualName(this IEnumerable<TypeMapping> typeMappings)
        {
            var max = 0;
            string actualTypeName = null;

            foreach (var typeMapping in typeMappings)
            {
                var version = typeMapping.GetTypeVersion().GetValueOrDefault();

                if (version >= max)
                {
                    actualTypeName = typeMapping.TypeName;
                    max = version;
                }
            }

            return actualTypeName;
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