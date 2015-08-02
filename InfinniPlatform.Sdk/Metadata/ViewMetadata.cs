using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class ViewMetadata
    {
        public ViewMetadata()
        {
            DataSources = new List<dynamic>();
            ChildViews = new List<dynamic>();
            Scripts = new List<dynamic>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string MetadataType { get; set; }

        public string Text { get; set; }

        public dynamic LayoutPanel { get; set; }

        public IEnumerable<dynamic> DataSources { get; set; }

        public IEnumerable<dynamic> ChildViews { get; set; }

        public IEnumerable<dynamic> Scripts { get; set; } 
    }
}
