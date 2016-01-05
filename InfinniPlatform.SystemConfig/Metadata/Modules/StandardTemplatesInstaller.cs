using InfinniPlatform.Core.Modules;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.SystemConfig.Metadata.HostServerConfiguration;

namespace InfinniPlatform.SystemConfig.Metadata.Modules
{
    /// <summary>
    /// Регистратор стандартных шаблонов обработчиков запроса
    /// </summary>
    internal sealed class StandardTemplatesInstaller : ITemplateInstaller
    {
        public StandardTemplatesInstaller(IServiceTemplateConfiguration serviceTemplateConfiguration)
        {
            _serviceTemplateConfiguration = serviceTemplateConfiguration;
        }

        private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

        /// <summary>
        /// Зарегистрировать шаблоны в конфигурации
        /// </summary>
        public void RegisterTemplates()
        {
            InfinniPlatformHostFactory.CreateDefaultServiceConfiguration(_serviceTemplateConfiguration);
        }
    }
}