using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Metadata.Implementation.Modules
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
            _serviceTemplateConfiguration.CreateDefaultServiceConfiguration();
        }
    }
}