using System.Collections.Generic;
using InfinniPlatform.Api.ContextTypes;

namespace InfinniPlatform.Api.Hosting
{
    public interface IExtensionPointHandler
    {
        /// <summary>
        ///     Наименование обработчика
        /// </summary>
        string ActionName { get; }

        /// <summary>
        ///     Тип обработчика
        /// </summary>
        VerbType VerbType { get; }

        /// <summary>
        ///     Получить экземпляр обработчика по наименованию
        /// </summary>
        /// <param name="actionHandlerInstanceName">Наименование обработчика</param>
        /// <returns>Экземпляр обработчика</returns>
        IExtensionPointHandlerInstance GetHandlerInstance(string actionHandlerInstanceName);

        /// <summary>
        ///     Получить список наименований экземпляров обработчика
        /// </summary>
        /// <returns>Список наименований экземпляров обработчика</returns>
        IEnumerable<string> GetInstanceNames();

        /// <summary>
        ///     Зарегистрировать обработчик для указанного типа REST-верба
        /// </summary>
        /// <param name="verbType">Тип верба REST</param>
        /// <returns>Обработчик</returns>
        IExtensionPointHandler AsVerb(VerbType verbType);

        /// <summary>
        ///     Добавить точку расширения
        /// </summary>
        /// <param name="extensionPointName">Наименование точки расширения</param>
        /// <param name="extensionPointContextTypeKind">Тип контекста точки расширения</param>
        /// <returns>Обработчик точки расширения</returns>
        IExtensionPointHandler AddExtensionPoint(string extensionPointName,
            ContextTypeKind extensionPointContextTypeKind);

        /// <summary>
        ///     Добавить обработчик точки расширения
        /// </summary>
        /// <param name="extensionPointHandlerInstance"></param>
        void AddHandlerInstance(IExtensionPointHandlerInstance extensionPointHandlerInstance);
    }
}