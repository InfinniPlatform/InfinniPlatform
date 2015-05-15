using System.Collections.Generic;

namespace InfinniPlatform.Runtime.Implementation.ProjectConverter
{
    public sealed class ProjectProperty : IProjectComponent
    {
        /// <summary>
        /// идентификатор проекта 
        /// </summary>
        public string ProjectGuid { get; set; }

        /// <summary>
        /// идентификаторы типа проекта
        /// </summary>
        public string ProjectTypeGuids { get; set; }

        /// <summary>
        /// тип проекта(Exe, Library, WinExe)
        /// </summary>
        public string OutputType { get; set; }

        public string RootNamespace { get; set; }
        public string AssemblyName { get; set; }

        public IEnumerable<ReflectionPair> GetAttributes()
        {
            return new List<ReflectionPair>();
        }

        public IEnumerable<ReflectionPair> GetElements()
        {
            return new List<ReflectionPair>
                {
                    new ReflectionPair("ProjectGuid", "ProjectGuid"),
                    new ReflectionPair("ProjectTypeGuids", "ProjectTypeGuids"),
                    new ReflectionPair("OutputType", "OutputType"),
                    new ReflectionPair("RootNamespace", "RootNamespace"),
                    new ReflectionPair("AssemblyName", "AssemblyName")
                };
        }
    }
}
