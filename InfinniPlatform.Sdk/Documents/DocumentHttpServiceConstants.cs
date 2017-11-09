namespace InfinniPlatform.Sdk.Documents
{
    public static class DocumentHttpServiceConstants
    {
        /// <summary>
        /// Базовый путь по умолчанию к методам сервиса документов.
        /// </summary>
        public const string DefaultServicePath = "/documents";

        /// <summary>
        /// Имя ключа с документом в запросе на сохранение документа.
        /// </summary>
        public const string DocumentFormKey = "document";

        /// <summary>
        /// Имя сегмента с идентификатором документа в запросе на удаление документа.
        /// </summary>
        public const string DocumentIdKey = "id";
    }
}