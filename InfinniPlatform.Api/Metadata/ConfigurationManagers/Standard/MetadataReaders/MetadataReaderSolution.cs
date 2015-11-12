using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    public sealed class MetadataReaderSolution : IDataReader
    {
        public IEnumerable<dynamic> GetItems()
        {
            return
                DynamicWrapperExtensions.ToEnumerable(
                    RestQueryApi.QueryPostJsonRaw("SystemConfig", "metadata", "getsolutionlist", null, null).ToDynamic().SolutionList);
        }

        public dynamic GetItem(string metadataName)
        {
            return GetItems().FirstOrDefault(g => g.Name.ToLowerInvariant() == metadataName.ToLowerInvariant());
        }
    }
}