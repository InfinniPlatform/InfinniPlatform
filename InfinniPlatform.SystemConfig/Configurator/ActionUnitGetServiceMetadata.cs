using InfinniPlatform.Sdk.Application.Contracts;
using InfinniPlatform.WebApi.Factories;

namespace InfinniPlatform.SystemConfig.Configurator
{
    /// <summary>
    ///     Получить метаданные типов существующих сервисов
    /// </summary>
    public sealed class ActionUnitGetServiceMetadata
    {
        public void Action(ISearchContext context)
        {
            InfinniPlatformHostServer serviceHost = InfinniPlatformHostServer.Instance;
            context.SearchResult = serviceHost.ServiceTemplateConfiguration.GetRegisteredTemplatesInfo();
        }
    }
}