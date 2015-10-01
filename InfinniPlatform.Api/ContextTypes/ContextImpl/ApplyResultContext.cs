using InfinniPlatform.Api.Context;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.Api.ContextTypes.ContextImpl
{
    public sealed class ApplyResultContext : IApplyResultContext
    {
        public ApplyResultContext()
        {
            IsValid = true;
        }

        /// <summary>
        ///     Глобальный контекст обработки
        /// </summary>
        public IGlobalContext Context { get; set; }

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
        ///     Признак системной ошибки сервера
        /// </summary>
        public bool IsInternalServerError { get; set; }

        /// <summary>
        ///     Конфигурация текущего запроса
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        ///     Метаданные текущего запроса
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        ///     Действие текущего запроса
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        ///     Авторизованный пользователь системы
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///     Результат фильтрации событий
        /// </summary>
        public dynamic ValidationMessage { get; set; }

        /// <summary>
        ///     Признак успешности обработки события фильтрации событий
        /// </summary>
        public bool IsValid { get; set; }
    }
}