using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.ContextComponents
{
    public sealed class VersionedScriptProcessor
    {
        public string Version { get; set; }
        public string ConfigurationId { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
    }
}