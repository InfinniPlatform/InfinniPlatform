using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class EmbeddedResource : IItemElement
    {
        public string Include { get; set; }
        public string Generator { get; set; }
        public string LastGenOutput { get; set; }
        public string SubType { get; set; }
        public string CopyToOutputDirectory { get; set; }

        public string XmlElementTagName
        {
            get { return "EmbeddedResource"; }
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
                    new ReflectionPair("Generator", "Generator"),
                    new ReflectionPair("LastGenOutput", "LastGenOutput"),
                    new ReflectionPair("SubType", "SubType"),
                    new ReflectionPair("CopyToOutputDirectory", "CopyToOutputDirectory")
                };
        }
    }
}
