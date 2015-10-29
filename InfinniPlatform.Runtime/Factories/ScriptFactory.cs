using System;
using System.Collections.Generic;

using InfinniPlatform.Factories;
using InfinniPlatform.Runtime.Implementation.ScriptMetadataProviders;
using InfinniPlatform.Runtime.Implementation.ScriptProcessors;
using InfinniPlatform.Runtime.Properties;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Runtime.Factories
{
    public sealed class ScriptFactory : IScriptFactory
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="versionLoader">Загрузчик версий прикладных скриптов.</param>
        /// <param name="changeListener">Слушатель изменений загруженных конфигураций.</param>
        /// <param name="metadataConfigurationId">Идентификатор загружаемой конфигурации.</param>
        /// <param name="version">Идентификатор загружаемой версии конфигурации.</param>
        public ScriptFactory(IVersionLoader versionLoader, IChangeListener changeListener, string metadataConfigurationId, string version)
        {
            _versionLoader = versionLoader;
            _metadataConfigurationId = metadataConfigurationId;
            _version = version;

            _scriptInvokationCache = new Lazy<IMethodInvokationCacheList>(CreateScriptInvokationCache);
            _scriptMetadataProvider = new Lazy<IScriptMetadataProvider>(CreateScriptMetadataProvider);
            _scriptProcessor = new Lazy<IScriptProcessor>(CreateScriptProcessor);

            changeListener.RegisterOnChange(metadataConfigurationId, UpdateCache, Order.NoMatter);
        }


        private readonly IVersionLoader _versionLoader;
        private readonly string _metadataConfigurationId;
        private readonly string _version;


        private readonly Lazy<IScriptMetadataProvider> _scriptMetadataProvider;
        private readonly Lazy<IMethodInvokationCacheList> _scriptInvokationCache;
        private readonly Lazy<IScriptProcessor> _scriptProcessor;


        public IScriptProcessor BuildScriptProcessor()
        {
            return _scriptProcessor.Value;
        }

        public IScriptMetadataProvider BuildScriptMetadataProvider()
        {
            return _scriptMetadataProvider.Value;
        }


        private IMethodInvokationCacheList CreateScriptInvokationCache()
        {
            return _versionLoader.ConstructInvokationCache(_version, _metadataConfigurationId);
        }

        private static IScriptMetadataProvider CreateScriptMetadataProvider()
        {
            return new ScriptMetadataProviderMemory();
        }

        private IScriptProcessor CreateScriptProcessor()
        {
            return new ScriptProcessor(_scriptInvokationCache.Value, _scriptMetadataProvider.Value);
        }


        private void UpdateCache(string version, string metadataConfigurationId)
        {
            // Если событие обновления соответствует текущей конфигурации метаданных, то выполняем обновление кэша метаданных

            if (string.Equals(_version, version, StringComparison.OrdinalIgnoreCase)
                && string.Equals(_metadataConfigurationId, metadataConfigurationId, StringComparison.OrdinalIgnoreCase))
            {
                if (_scriptInvokationCache.IsValueCreated)
                {
                    Logging.Logger.Log.Info(Resources.UpdateCacheRequest, new Dictionary<string, object>
                                                                          {
                                                                              { "version", _version },
                                                                              { "configurationId", _metadataConfigurationId },
                                                                          });

                    _versionLoader.UpdateInvokationCache(_version, _metadataConfigurationId, _scriptInvokationCache.Value);
                }
            }
        }
    }
}