using System.Reflection;

namespace InfinniPlatform.MetadataDesigner.Views.Exchange
{
    public sealed class SourceAssemblyInfo
    {
        public string Name { get; set; }
        public Assembly Assembly { get; set; }
        public string AssemblyFileName { get; set; }
    }
}