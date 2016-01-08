using System.Collections.Generic;

using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Core.ContextTypes.ContextImpl
{
    /// <summary>
    /// Контекст фильтрации событий
    /// </summary>
    public class ProcessEventContext : IProcessEventContext
    {
        /// <summary>
        /// Идентификатор обрабатываемого объекта
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Список событий
        /// </summary>
        public List<EventDefinition> Events { get; set; }

        /// <summary>
        /// Свойства, значения которых должны устанавливаться по умолчанию
        /// </summary>
        public List<EventDefinition> DefaultProperties { get; set; }

        /// <summary>
        /// Результат обработки событий
        /// </summary>
        public dynamic Item { get; set; }

        /// <summary>
        /// Результат фильтрации событий
        /// </summary>
        public dynamic ValidationMessage { get; set; }

        /// <summary>
        /// Признак успешности обработки события фильтрации событий
        /// </summary>
        public bool IsValid { get; set; }

        public bool IsInternalServerError { get; set; }

        /// <summary>
        /// Конфигурация текущего запроса
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Метаданные текущего запроса
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Действие текущего запроса
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Авторизованный пользователь системы
        /// </summary>
        public string UserName { get; set; }
    }
}