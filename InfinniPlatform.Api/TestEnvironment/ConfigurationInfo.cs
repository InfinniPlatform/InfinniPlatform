using System.Collections.Generic;

namespace InfinniPlatform.Api.TestEnvironment
{
    public sealed class ConfigurationInfo
    {
        public string ConfigurationFilePath { get; set; }
        public IEnumerable<string> AppliedAssemblyList { get; set; }
    }
}