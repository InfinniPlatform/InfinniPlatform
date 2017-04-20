namespace InfinniPlatform.DocumentStorage
{
    /// <summary>
    /// Результат выполнения проверки корректности свойства документа.
    /// </summary>
    public class DocumentValidationResultItem
    {
        /// <summary>
        /// Путь к свойству с ошибкой.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Message { get; set; }
    }
}