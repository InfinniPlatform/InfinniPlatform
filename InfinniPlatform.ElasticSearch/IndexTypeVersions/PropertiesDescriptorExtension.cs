using System.Collections.Generic;

using Nest;

namespace InfinniPlatform.ElasticSearch.IndexTypeVersions
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