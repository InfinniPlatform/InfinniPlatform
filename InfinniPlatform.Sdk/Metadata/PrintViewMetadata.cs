using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class PrintViewMetadata
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string ViewType { get; set; }

        public IEnumerable<dynamic> Blocks { get; set; }

        public string Source { get; set; }
    }
}
