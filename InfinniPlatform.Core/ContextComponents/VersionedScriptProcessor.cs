using InfinniPlatform.Api.Factories;

namespace InfinniPlatform.ContextComponents
{
    public sealed class VersionedScriptProcessor
    {
        public string Version { get; set; }
        public string ConfigurationId { get; set; }
        public IScriptProcessor ScriptProcessor { get; set; }
    }
}