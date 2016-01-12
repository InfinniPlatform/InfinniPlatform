using System;

namespace InfinniPlatform.Core.Hosting
{
    /// <summary>
    ///     Регистрация сервиса
    /// </summary>
    public interface IServiceRegistration
    {
        /// <summary>
        ///     Наименование объекта метаданных
        /// </summary>
        string MetadataName { get; }

        /// <summary>
        ///     Обработчик запроса
        /// </summary>
        IQueryHandler QueryHandler { get; }

        /// <summary>
        ///     Обработчик действия запроса
        /// </summary>
        IExtensionPointHandler ExtensionPointHandler { get; }

        /// <summary>
        ///     Наименование зарегистрированного сервиса
        /// </summary>
        string ServiceName { get; }

        /// <summary>
        ///     Зарегистрировать сервис для конфигурации
        /// </summary>
        /// <param name="extensionPointHandler"></param>
        /// <param name="queryHandlerAction">инициализатор обработчика запроса</param>
        /// <returns></returns>
        IServiceRegistration RegisterService(Func<string, IExtensionPointHandler> extensionPointHandler,
            Action<IQueryHandler> queryHandlerAction = null);

        /// <summary>
        ///     Установить обработчик результата запроса
        /// </summary>
        IServiceRegistration SetResultHandler(HttpResultHandlerType httpResultHandlerType);

        IServiceRegistration SetExtensionPointHandler(Func<string, IExtensionPointHandler> extensionPointHandler);
    }
}