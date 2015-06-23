using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Factories;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata;
using InfinniPlatform.Runtime;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.Initializers
{
    /// <summary>
    ///     Инициализация пользовательской логики при старте системы
    /// </summary>
    public sealed class UserLogicInitializer : IStartupInitializer
    {
        private readonly IChangeListener _changeListener;
        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;
        private readonly ICrossConfigSearcher _crossConfigSearcher;
        private readonly IGlobalContext _globalContext;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

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

            List<IMetadataConfiguration> configList = _metadataConfigurationProvider.Configurations.ToList();
            configList.ForEach(config => config.MoveWorkflow("system", "afterstart", applyContext));

            foreach (IMetadataConfiguration metadataConfiguration in configList)
            {
                IMetadataConfiguration configuration = metadataConfiguration;
                _changeListener.RegisterOnChange("UserLogic", (version, module) =>
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