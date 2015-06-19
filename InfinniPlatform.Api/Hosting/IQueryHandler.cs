using System;
using System.Collections.Generic;
using InfinniPlatform.Api.RestQuery;

namespace InfinniPlatform.Api.Hosting
{
    public interface IQueryHandler
    {
        /// <summary>
        ///     Список обработчиков запросов
        /// </summary>
        IList<IExtensionPointHandler> ActionHandlers { get; }

        /// <summary>
        ///     Тип класса, содержащего метод, обрабатывающий запрос
        /// </summary>
        Type QueryHandlerType { get; }

        /// <summary>
        ///     Обработчик результата выполнения запроса
        /// </summary>
        HttpResultHandlerType HttpResultHandlerType { get; }

        /// <summary>
        ///     Установить обработчик действия на запрос
        /// </summary>
        /// <param name="extensionPointHandler"></param>
        /// <returns></returns>
        IQueryHandler ForAction(IExtensionPointHandler extensionPointHandler);

        /// <summary>
        ///     Установить обработчик результата запроса
        /// </summary>
        /// <returns></returns>
        IQueryHandler SetResultHandler(HttpResultHandlerType resultHandlerType);
    }
}