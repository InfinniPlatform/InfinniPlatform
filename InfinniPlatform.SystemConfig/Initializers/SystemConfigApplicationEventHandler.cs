using System.Collections.Generic;

using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.SystemConfig.Initializers
{
    /// <summary>
    /// Обработчик событий приложения системной конфигурации.
    /// </summary>
    internal sealed class SystemConfigApplicationEventHandler : IApplicationEventHandler
    {
        public SystemConfigApplicationEventHandler(IEnumerable<IStartupInitializer> startupInitializers)
        {
            _startupInitializers = startupInitializers;
        }


        private readonly IEnumerable<IStartupInitializer> _startupInitializers;


        public void OnStart()
        {
            if (_startupInitializers != null)
            {
                foreach (var initializer in _startupInitializers)
                {
                    initializer.OnStart();
                }
            }
        }

        public void OnStop()
        {
        }
    }
}