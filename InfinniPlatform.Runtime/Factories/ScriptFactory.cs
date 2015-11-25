using System;

using InfinniPlatform.Factories;
using InfinniPlatform.Runtime.Implementation.ScriptMetadataProviders;
using InfinniPlatform.Runtime.Implementation.ScriptProcessors;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Scripts;

namespace InfinniPlatform.Runtime.Factories
{
    public sealed class ScriptFactory : IScriptFactory
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="versionLoader">Загрузчик версий прикладных скриптов.</param>
        /// <param name="performanceLog">Сервис протоколирования длительности вызова методов компонентов.</param>
        public ScriptFactory(IVersionLoader versionLoader, IPerformanceLog performanceLog)
        {
            _versionLoader = versionLoader;
            _performanceLog = performanceLog;

            _scriptInvokationCache = new Lazy<IMethodInvokationCacheList>(CreateScriptInvokationCache);
            _scriptMetadataProvider = new Lazy<IScriptMetadataProvider>(CreateScriptMetadataProvider);
            _scriptProcessor = new Lazy<IScriptProcessor>(CreateScriptProcessor);
        }


        private readonly IVersionLoader _versionLoader;
        private readonly IPerformanceLog _performanceLog;


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
            return _versionLoader.ConstructInvokationCache();
        }

        private static IScriptMetadataProvider CreateScriptMetadataProvider()
        {
            return new ScriptMetadataProviderMemory();
        }

        private IScriptProcessor CreateScriptProcessor()
        {
            return new ScriptProcessor(_scriptInvokationCache.Value, _scriptMetadataProvider.Value, _performanceLog);
        }
    }
}