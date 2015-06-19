using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Api.ContextComponents
{
    public interface ISystemComponent
    {
        /// <summary>
        ///     Менеджер идентификаторов конфигураций
        /// </summary>
        IManagerIdentifiers ManagerIdentifiers { get; set; }
    }
}