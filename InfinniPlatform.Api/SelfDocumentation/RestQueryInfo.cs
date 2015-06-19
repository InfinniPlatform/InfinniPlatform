namespace InfinniPlatform.Api.SelfDocumentation
{
    /// <summary>
    ///     Справочная информация об одном REST запросе
    /// </summary>
    public sealed class RestQueryInfo
    {
        /// <summary>
        ///     Конфигурация
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        ///     Метаданные конфигурации
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        ///     Выполняемое действие
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        ///     Описание запроса
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        ///     Тип запроса (POST, GET)
        /// </summary>
        public string QueryType { get; set; }

        /// <summary>
        ///     URL запроса
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     Тело запроса
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///     Ответ сервера на запрос
        /// </summary>
        public string ResponceContent { get; set; }

        /// <summary>
        ///     Источник информации о запросе
        /// </summary>
        public string ExampleSource { get; set; }
    }
}