using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.Sdk.Metadata
{
    public sealed class ProcessMetadata
    {
        public ProcessMetadata()
        {
            Transitions = new List<dynamic>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<dynamic> Transitions { get; set; } 

        /// <summary>
        ///   WithState = 1,
        ///   WithoutState = 2
        /// </summary>
        public int Type  { get; set; }

        /// <summary>
        /// "Default" | "Custom"
        /// </summary>
        public string SettingsType { get; set; }
    }
}
