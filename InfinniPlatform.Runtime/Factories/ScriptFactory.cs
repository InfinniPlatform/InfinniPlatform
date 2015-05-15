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

	    private readonly IVersionLoader _versionLoader;
	    
        private readonly IChangeListener _changeListener;

	    private IScriptMetadataProvider _scriptMetadataProvider;

	    private ScriptProcessor _scriptProcessor;

        /// <summary>
        ///  Кэш версий конфигурации скриптов
        /// </summary>
	    private MethodInvokationCacheList _versionCacheList;

	    /// <summary>
	    ///   Конструктор принимает идентификатор конфигурации метаданных, для которой создается фабрика
	    ///   Это необходимо для реализации корректной реакции на событие обновления метаданных конфигураций
	    /// </summary>
	    /// <param name="changeListener"></param>
	    /// <param name="metadataConfigurationId">Идентификатор конфигурации метаданных</param>
	    /// <param name="versionLoader"></param>
	    public ScriptFactory(IVersionLoader versionLoader, IChangeListener changeListener, string metadataConfigurationId)
        {
            _versionLoader = versionLoader;
            _changeListener = changeListener;
            _changeListener.RegisterOnChange(metadataConfigurationId, UpdateCache);
            _metadataConfigurationId = metadataConfigurationId;
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
	            UpdateCache(_metadataConfigurationId);
	        }
	        return _scriptProcessor;
	    }

        private void UpdateCache(string metadataConfigurationId)
        {
            //если событие обновления соответствует текущей конфигурации метаданных, то выполняем обновление кэша метаданных
            if (metadataConfigurationId.ToLowerInvariant() == _metadataConfigurationId.ToLowerInvariant())
            {

                _versionCacheList = _versionLoader.ConstructInvokationCache(_metadataConfigurationId);
                if (_scriptProcessor == null)
                {
                    _scriptProcessor = new ScriptProcessor(_versionCacheList, _scriptMetadataProvider);
                }
                _scriptProcessor.UpdateCache(_versionCacheList);
            }
        }

        /// <summary>
        ///   Получить актуальную версию скриптовой конфигурации
        /// </summary>
        /// <returns>Актуальная версия скриптовой конфигурации</returns>
        public string GetActualVersion()
        {
            
            if (_versionCacheList == null)
            {
                UpdateCache(_metadataConfigurationId);
            }

            //кэш может отсутствовать, в случае если ни одной версии конфигурации еще не установлено
            var actualCache = _versionCacheList.GetActualCache();
            if (actualCache != null)
            {
                return actualCache.Version;
            }

            return null;
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