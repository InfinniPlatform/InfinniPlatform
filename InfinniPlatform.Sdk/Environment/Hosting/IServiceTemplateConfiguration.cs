using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Environment.Hosting
{
    public interface IServiceTemplateConfiguration
    {
        /// <summary>
        ///     Зарегистрировать шаблон сервиса
        /// </summary>
        /// <typeparam name="T">Тип класса, предоставляющего сервис</typeparam>
        /// <param name="containerName">Наименование, под который сервис будет зарегистрирован</param>
        /// <param name="methodName">Имя исполянемого метода</param>
        /// <param name="extensionPointHandlerConfig">Конфигуратор обработчика запроса</param>
        /// <returns></returns>
        IServiceTemplateConfiguration RegisterServiceTemplate<T>(string containerName, string methodName,
            IExtensionPointHandlerConfig extensionPointHandlerConfig = null) where T : class;

        /// <summary>
        ///     Получить зарегистрированный обработчик запроса для указанного имени контейнера
        /// </summary>
        /// <param name="containerName">Имя зарегистрированного контейера</param>
        /// <returns>Обработчик запроса</returns>
        IServiceTemplate GetServiceHandler(string containerName);

        /// <summary>
        ///     Получить список типов классов, реализующих сервисы
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetRegisteredTypes();

        /// <summary>
        ///     Получить список зарегистрированных шаблонов сервисов
        /// </summary>
        /// <returns>Список информации о шаблонах</returns>
        IEnumerable<dynamic> GetRegisteredTemplatesInfo();
    }
}