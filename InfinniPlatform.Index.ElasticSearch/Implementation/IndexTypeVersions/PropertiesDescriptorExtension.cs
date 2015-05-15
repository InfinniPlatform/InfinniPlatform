using Nest;
using System.Collections.Generic;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.IndexTypeVersions
{
    public static class PropertiesDescriptorExtension
    {
        public static PropertiesDescriptor<dynamic> AddProperties(
            this PropertiesDescriptor<dynamic> descriptor,
            IDictionary<PropertyNameMarker, IElasticType> props)
        {
            foreach (var prop in props)
            {
                descriptor.Properties.Add(prop);
            }
            
            return descriptor;
        }
    }
}