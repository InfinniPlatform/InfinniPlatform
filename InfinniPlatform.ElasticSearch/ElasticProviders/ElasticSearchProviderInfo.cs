using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Core.Index;

namespace InfinniPlatform.ElasticSearch.ElasticProviders
{
    public sealed class ElasticSearchProviderInfo
    {
        public ElasticSearchProviderInfo(string indexName, string typeName, ICrudOperationProvider provider)
        {
            Index = indexName;
            Type = typeName;
            Provider = provider;
        }

        public string Index { get; set; }

        public string Type { get; set; }

        public ICrudOperationProvider Provider { get; set; }
    }

    public static class ElasticSearchProviderInfoExtensions
    {
        public static ElasticSearchProviderInfo FindInfo(this IEnumerable<ElasticSearchProviderInfo> providers,
            string indexName,
            string type)
        {
            return
                providers.FirstOrDefault(
                    p =>
                        p.Index.ToLowerInvariant() == indexName.ToLowerInvariant() &&
                        p.Type.ToLowerInvariant() == type.ToLowerInvariant());
        }
    }
}
