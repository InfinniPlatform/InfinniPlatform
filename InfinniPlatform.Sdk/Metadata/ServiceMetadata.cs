using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class ServiceMetadata
    {
        public ServiceMetadata()
        {
            ExtensionPoints = new List<dynamic>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<dynamic> ExtensionPoints { get; set; } 
    }
}
