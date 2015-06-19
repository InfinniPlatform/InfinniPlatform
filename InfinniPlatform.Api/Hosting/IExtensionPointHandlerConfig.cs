using System.Collections.Generic;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Api.Hosting
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