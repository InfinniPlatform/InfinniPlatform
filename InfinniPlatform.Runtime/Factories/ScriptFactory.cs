using InfinniPlatform.Api.Factories;
using InfinniPlatform.Factories;
using InfinniPlatform.Runtime.Implementation.ScriptMetadataProviders;
using InfinniPlatform.Runtime.Implementation.ScriptProcessors;

namespace InfinniPlatform.Runtime.Factories
{
	/// <summary>
	///   Фабрика скриптовых компонентов 
	/// </summary>
	public sealed class ScriptFactory : IScriptFactory
	{
	    private readonly string _metadataConfigurationId;

        private readonly string _version;

	    private readonly IVersionLoader _versionLoader;
	    
        private readonly IChangeListener _changeListener;

	    private IScriptMetadataProvider _scriptMetadataProvider;

	    private volatile ScriptProcessor _scriptProcessor;

        /// <summary>
        ///  Кэш версий конфигурации скриптов
        /// </summary>
	    private volatile MethodInvokationCacheList _versionCacheList;

	    /// <summary>
	    ///   Конструктор принимает идентификатор конфигурации метаданных, для которой создается фабрика
	    ///   Это необходимо для реализации корректной реакции на событие обновления метаданных конфигураций
	    /// </summary>
	    /// <param name="changeListener"></param>
	    /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
	    /// <param name="versionLoader"></param>
	    /// <param name="version"></param>
	    public ScriptFactory(IVersionLoader versionLoader, IChangeListener changeListener, string metadataConfigurationId, string version)
        {
            _versionLoader = versionLoader;
            _changeListener = changeListener;
            _changeListener.RegisterOnChange(metadataConfigurationId, UpdateCache);
            _metadataConfigurationId = metadataConfigurationId;
	        _version = version;
        }

	    /// <summary>
	    ///   Создать процессор запуска прикладных скриптов на основе использования
	    ///   распределенного хранилища
	    /// </summary>
	    /// <returns>Процессор запуска прикладных скриптов</returns>
	    public IScriptProcessor BuildScriptProcessor()
	    {
	        if (_scriptProcessor == null)
	        {
	            UpdateCache(_version, _metadataConfigurationId);
	        }
	        return _scriptProcessor;
	    }

        private readonly object _lockCache = new object();
	    

	    private void UpdateCache(string version, string metadataConfigurationId)
        {
            //если событие обновления соответствует текущей конфигурации метаданных, то выполняем обновление кэша метаданных
            if (metadataConfigurationId.ToLowerInvariant() == _metadataConfigurationId.ToLowerInvariant() && _version == version)
            {
                lock (_lockCache)
                {
                    if (_versionCacheList != null)
                    {
                        _versionLoader.UpdateInvokationCache(version, _metadataConfigurationId, _versionCacheList);
                    }
                    else
                    {
                        _versionCacheList = _versionLoader.ConstructInvokationCache(version, _metadataConfigurationId);
                    }


                    _scriptProcessor = new ScriptProcessor(_versionCacheList, _scriptMetadataProvider);
                    _scriptProcessor.UpdateCache(_versionCacheList);
                }
            }
        }


	    /// <summary>
        ///   Создать провайдер метаданных прикладных скриптов
        /// </summary>
        /// <returns>Провайдер метаданных прикладных скриптов</returns>
        public IScriptMetadataProvider BuildScriptMetadataProvider()
        {
            return _scriptMetadataProvider ?? (_scriptMetadataProvider = new ScriptMetadataProviderMemory());
        }


	}

}