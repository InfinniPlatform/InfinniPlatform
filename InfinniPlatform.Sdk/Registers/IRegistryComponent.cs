using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Registers
{
    /// <summary>
    /// Компонент для работы с регистрами внутри глобального контекста
    /// </summary>
    public interface IRegistryComponent
    {
        /// <summary>
        /// Создаёт и инициализирует новую запись регистра накоплений
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="documentId">Идентификатор метаданных объекта</param>
        /// <param name="sourceDocument">Содержимое проводящегося документа</param>
        /// <param name="documentDate">Дата документа</param>
        /// <returns>Заготовка записи в регистр, которая может быть дополнена при необходимости</returns>
        dynamic CreateAccumulationRegisterEntry(string configId, string registerId, string documentId, dynamic sourceDocument, DateTime? documentDate = null);

        /// <summary>
        /// Создаёт и инициализирует новую запись регистра сведений
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="documentId">Идентификатор метаданных объекта</param>
        /// <param name="sourceDocument">Содержимое проводящегося документа</param>
        /// <param name="documentDate">Дата документа</param>
        /// <returns>Заготовка записи в регистр, которая может быть дополнена при необходимости</returns>
        dynamic CreateInfoRegisterEntry(string configId, string registerId, string documentId, dynamic sourceDocument, DateTime? documentDate = null);

        /// <summary>
        /// Выполняет проводку в регистр
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="registerEntries">Записи регистра</param>
        void PostRegisterEntries(string configId, string registerId, IEnumerable<object> registerEntries);

        /// <summary>
        /// Удаляет проводку из регистра
        /// </summary>
        /// <param name="configId">Идентификатор конфигурации</param>
        /// <param name="registerId">Идентификатор регистра</param>
        /// <param name="registar">Идентификатор документа-регистратора</param>
        void DeleteRegisterEntry(string configId, string registerId, string registar);
    }
}