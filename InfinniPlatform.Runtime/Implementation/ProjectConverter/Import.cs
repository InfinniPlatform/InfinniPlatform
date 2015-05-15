using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class Import : IItemElement
    {
        public string Project { get; set; }
        public string Condition { get; set; }

        public string XmlElementTagName
        {
            get { return "Import"; }
        }

        public IEnumerable<ReflectionPair> GetAttributes()
        {
            return new List<ReflectionPair>
                {
                    new ReflectionPair("Project", "Project"),
                    new ReflectionPair("Condition", "Condition")
                };
        }

        public IEnumerable<ReflectionPair> GetElements()
        {
            return new List<ReflectionPair>();
        }
    }
}
