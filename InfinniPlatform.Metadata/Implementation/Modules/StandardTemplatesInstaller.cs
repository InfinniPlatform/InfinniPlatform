using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;

namespace InfinniPlatform.Metadata.Implementation.Modules
{
    /// <summary>
    ///     Регистратор стандартных шаблонов обработчиков запроса
    /// </summary>
    public sealed class StandardTemplatesInstaller : ITemplateInstaller
    {
        private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

        public StandardTemplatesInstaller(IServiceTemplateConfiguration serviceTemplateConfiguration)
        {
            _serviceTemplateConfiguration = serviceTemplateConfiguration;
        }

        /// <summary>
        ///     Зарегистрировать шаблоны в конфигурации
        /// </summary>
        public void RegisterTemplates()
        {
            _serviceTemplateConfiguration.CreateDefaultServiceConfiguration();
        }
    }
}