using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataReaders
{
    public sealed class MetadataReaderSolution : IDataReader
    {
        private readonly string _version;

        public MetadataReaderSolution(string version)
        {
            _version = version;
        }

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
