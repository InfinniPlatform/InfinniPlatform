using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Hosting
{
    public interface IExtensionPointHandlerConfig
    {
        Dictionary<string, ContextTypeKind> WorkflowExtensionPoints { get; }

        /// <summary>
        ///     Инициализировать обработчик на основе конфигурации
        /// </summary>
        /// <param name="handler">Обработчик</param>
        void BuildHandler(IExtensionPointHandler handler);
    }
}