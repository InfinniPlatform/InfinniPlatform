using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class ProjectReference : IItemElement
    {
        public string Include { get; set; }
        public string ProjectGuid { get; set; }
        public string Name { get; set; }

        public string XmlElementTagName
        {
            get { return "ProjectReference"; }
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
                    new ReflectionPair("ProjectGuid", "Project"),
                    new ReflectionPair( "Name", "Name")
                };
        }
    }
}
