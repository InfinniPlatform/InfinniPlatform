using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class SolutionMetadata
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<ConfigurationMetadata> ReferencedConfigurations { get; set; }

        public string Version { get; set; }
    }
}
