using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class Project
    {
        public ProjectProperty ProjectProperty { get; set; }
        public IEnumerable<Module> Modules { get; set; }
        public IEnumerable<Reference> References { get; set; }
        public IEnumerable<ProjectReference> ProjectReferences { get; set; }
        public IEnumerable<EmbeddedResource> EmbeddedResources { get; set; }
        public IEnumerable<Content> Contents { get; set; }
        public IEnumerable<Import> Imports { get; set; }
    }
}
