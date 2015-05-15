using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Factories;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime.Implementation.AssemblyDispatch;

namespace InfinniPlatform.Runtime.Factories
{
	/// <summary>
	///  Конструктор фабрик исполнения скриптов
	/// </summary>
	public sealed class ScriptFactoryBuilder : IScriptFactoryBuilder
	{
	    private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        
        private readonly IChangeListener _changeListener;
	    
	    public ScriptFactoryBuilder(IConfigurationObjectBuilder configurationObjectBuilder,IChangeListener changeListener)
	    {
	        _configurationObjectBuilder = configurationObjectBuilder;
	        _changeListener = changeListener;
	    }


	    /// <summary>
	    ///   Создать фабрику прикладных скриптов для указанной версии конфигурации
	    /// </summary>
	    /// <returns>Фабрика скриптов</returns>
	    public IScriptFactory BuildScriptFactory(string metadataConfigurationId)
		{
            return new ScriptFactory(new AssemblyVersionLoader(_configurationObjectBuilder),_changeListener,metadataConfigurationId);
		}
	}
}
