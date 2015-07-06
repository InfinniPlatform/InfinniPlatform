using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Sdk.ContextComponents
{
    public interface ISystemComponent
    {
        /// <summary>
        ///     Менеджер идентификаторов конфигураций
        /// </summary>
        IManagerIdentifiers ManagerIdentifiers { get; set; }
    }
}