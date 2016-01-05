using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.SystemConfig.StartupInitializers
{
    /// <summary>
    /// Обработчик событий приложения системной конфигурации.
    /// </summary>
    internal sealed class SystemConfigApplicationEventHandler : IApplicationEventHandler
    {
        public SystemConfigApplicationEventHandler(IEnumerable<IStartupInitializer> startupInitializers)
        {
            _startupInitializers = startupInitializers.OrderBy(i => i.Order);
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