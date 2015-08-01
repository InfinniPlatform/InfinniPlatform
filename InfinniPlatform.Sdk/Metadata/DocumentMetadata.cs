using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class DocumentMetadata
    {
        public DocumentMetadata()
        {
            Scenarios = new List<dynamic>();
            Processes = new List<dynamic>();
            Views = new List<dynamic>();
            PrintViews = new List<dynamic>();
            Services = new List<dynamic>();
            ValidationErrors = new List<dynamic>();
            ValidationWarnings = new List<dynamic>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<dynamic> Scenarios { get; set; }
        
        public IEnumerable<dynamic> Processes { get; set; }

        public IEnumerable<dynamic> Views { get; set; }

        public IEnumerable<dynamic> PrintViews { get; set; }

        public IEnumerable<dynamic> Services { get; set; }

        public IEnumerable<dynamic> ValidationErrors { get; set; }

        public IEnumerable<dynamic> ValidationWarnings { get; set; } 

        public dynamic Schema { get; set; }

        public string MetadataIndex
        {
            get { return Name; }
        }

        public int Versioning
        {
            get { return 2; }
        }
    }
}
