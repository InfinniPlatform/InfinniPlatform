using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Исполнитель скриптов из глобального контекста
    /// </summary>
    public sealed class ScriptRunnerComponent : IScriptRunnerComponent
    {
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
        private readonly List<VersionedScriptProcessor> _scriptProcessors = new List<VersionedScriptProcessor>();

        public ScriptRunnerComponent(IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }

        /// <summary>
        ///     Получить исполнителя скриптов для указанного идентификатора метаданных конфигурации
        /// </summary>
        /// <param name="version"></param>
        /// <param name="configurationId">Идентификатор метаданных конфигурации</param>
        /// <returns>Исполнитель скриптов</returns>
        public IScriptProcessor GetScriptRunner(string version, string configurationId)
        {
            
            var scriptProcessorVersioned =
                _scriptProcessors.FirstOrDefault(sc => sc.ConfigurationId == configurationId && sc.Version == version);

            if (scriptProcessorVersioned == null)
            {
                var scriptProcessor =
                    _metadataConfigurationProvider.GetMetadataConfiguration(configurationId)
                        .ScriptConfiguration.GetScriptProcessor();

                scriptProcessorVersioned = new VersionedScriptProcessor
                {
                    ConfigurationId = configurationId,
                    ScriptProcessor = scriptProcessor,
                    Version = version
                };

                _scriptProcessors.Add(scriptProcessorVersioned);
            }

            return scriptProcessorVersioned.ScriptProcessor;
        }
    }
}