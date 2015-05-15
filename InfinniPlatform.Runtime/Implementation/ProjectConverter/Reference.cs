using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class Reference : IItemElement
    {
        public string Include { get; set; }
        public string SpecificVersion { get; set; }
        public string HintPath { get; set; }
        public string RequiredTargetFramework { get; set; }
        public string Private { get; set; }

        public string XmlElementTagName
        {
            get { return "Reference"; }
        }

        public IEnumerable<ReflectionPair> GetAttributes()
        {
            return new List<ReflectionPair>
                {
                    new ReflectionPair("Include", "Include")
                };
        }

        public IEnumerable<ReflectionPair> GetElements()
        {
            return new List<ReflectionPair>
                {
                    new ReflectionPair("SpecificVersion", "SpecificVersion"),
                    new ReflectionPair("HintPath", "HintPath"),
                    new ReflectionPair("RequiredTargetFramework", "RequiredTargetFramework"),
                    new ReflectionPair("Private", "Private")
                };
        }
    }
}
