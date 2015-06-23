using System.Collections.Generic;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Events;

namespace InfinniPlatform.Api.ContextTypes.ContextImpl
{
    /// <summary>
    ///     Контекст применения событий к объекту (бизнес-логика)
    /// </summary>
    public sealed class ApplyContext : IApplyContext
    {
        public ApplyContext()
        {
            IsValid = true;
        }

        /// <summary>
        ///     Глобальный контекст обработки
        /// </summary>
        public IGlobalContext Context { get; set; }

        /// <summary>
        ///     Свойства, значения которых должны устанавливаться по умолчанию
        /// </summary>
        public List<EventDefinition> DefaultProperties { get; set; }

        /// <summary>
        ///     Список событий
        /// </summary>
        public List<EventDefinition> Events { get; set; }

        /// <summary>
        ///     Идентификатор обрабатываемого объекта
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Наименование индекса
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///     Объект, к которому приме
        /// </summary>
        public dynamic Item { get; set; }

        /// <summary>
        ///     Статус обработки документа
        /// </summary>
        public object Status { get; set; }

        /// <summary>
        ///     Результат обработки документа
        /// </summary>
        public dynamic Result { get; set; }

        /// <summary>
        ///     Маркер транзакции используемой при обработке запроса
        /// </summary>
        public string TransactionMarker { get; set; }

        /// <summary>
        ///     Признак успешности обработки события фильтрации событий
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        ///     Признак системной ошибки сервера
        /// </summary>
        public bool IsInternalServerError { get; set; }

        /// <summary>
        ///     Результат фильтрации событий
        /// </summary>
        public dynamic ValidationMessage { get; set; }

        /// <summary>
        ///     Версия исполняемой точки расширения
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        ///     Конфигурация текущего запроса
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        ///     Метаданные текущего запроса
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        ///     Авторизованный пользователь системы
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Действие, выполняемое клиентом
        /// </summary>
        public string Action { get; set; }

        public void CopyPropertiesFrom(IApplyContext context)
        {
            Configuration = context.Configuration;
            Context = context.Context;
            Metadata = context.Metadata;
            Action = context.Action;
            Result = context.Result;
            UserName = context.UserName;
            //Version = context.Version;
            Id = context.Id;
            Type = context.Type;
            Item = context.Item;
            TransactionMarker = context.TransactionMarker;
            Version = context.Version;
        }
    }
}