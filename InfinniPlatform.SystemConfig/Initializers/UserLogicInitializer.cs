using System.Linq;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime;

namespace InfinniPlatform.SystemConfig.Initializers
{
	/// <summary>
	///   Инициализация пользовательской логики при старте системы
	/// </summary>
	public sealed class UserLogicInitializer : IStartupInitializer
	{
		private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
		private readonly IGlobalContext _globalContext;
		private readonly IChangeListener _changeListener;
	    private readonly ICrossConfigSearcher _crossConfigSearcher;

	    public UserLogicInitializer(
            IConfigurationObjectBuilder configurationObjectBuilder,
            IMetadataConfigurationProvider metadataConfigurationProvider, 
            IGlobalContext globalContext, 
            IChangeListener changeListener,
            ICrossConfigSearcher crossConfigSearcher)
		{
			_configurationObjectBuilder = configurationObjectBuilder;
			_metadataConfigurationProvider = metadataConfigurationProvider;
			_globalContext = globalContext;            
			_changeListener = changeListener;
		    _crossConfigSearcher = crossConfigSearcher;
		}


		public void OnStart(HostingContextBuilder contextBuilder)
		{		
			var applyContext = new ApplyContext();
			applyContext.Context = _globalContext;
			
			var configList = _metadataConfigurationProvider.Configurations.ToList();
			configList.ForEach(config => config.MoveWorkflow("system", "afterstart",applyContext));

			foreach (var metadataConfiguration in configList)
			{
				IMetadataConfiguration configuration = metadataConfiguration;
				_changeListener.RegisterOnChange("UserLogic", module =>
					                                  {
						                                  if (configuration.ConfigurationId.ToLowerInvariant() ==
						                                      module.ToLowerInvariant())
						                                  {
							                                  configuration.MoveWorkflow("system", "afterupdate", applyContext);
						                                  }


					                                  });
			}
		}
	}
}
