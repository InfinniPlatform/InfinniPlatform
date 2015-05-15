using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class Module : IItemElement
    {
        public string Include { get; set; }
        public string Link { get; set; }
        public string AutoGen { get; set; }
        public string DesignTime { get; set; }
        public string DependentUpon { get; set; }
        public string CopyToOutputDirectory { get; set; }

        public string XmlElementTagName
        {
            get { return "Compile"; }
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
                    new ReflectionPair("Link", "Link"),
                    new ReflectionPair("AutoGen", "AutoGen"),
                    new ReflectionPair("DesignTime", "DesignTime"),
                    new ReflectionPair("DependentUpon", "DependentUpon"),
                    new ReflectionPair("CopyToOutputDirectory", "CopyToOutputDirectory")
                };
        }
    }
}
