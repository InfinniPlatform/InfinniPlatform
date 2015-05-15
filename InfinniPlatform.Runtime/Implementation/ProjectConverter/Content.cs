using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class Content : IItemElement
    {
        public string Include { get; set; }
        public string CopyToOutputDirectory { get; set; }
        public string SubType { get; set; }

        public string XmlElementTagName
        {
            get { return "Content"; }
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
                    new ReflectionPair("CopyToOutputDirectory", "CopyToOutputDirectory"),
                    new ReflectionPair("SubType", "SubType")
                };
        }
    }
}
