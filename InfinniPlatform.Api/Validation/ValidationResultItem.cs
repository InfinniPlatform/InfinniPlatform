namespace InfinniPlatform.Api.Validation
{
    /// <summary>
    ///     Результат проверки свойства объекта.
    /// </summary>
    public sealed class ValidationResultItem
    {
        /// <summary>
        ///     Возвращает или устанавливает путь свойству.
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        ///     Возвращает или устанавливает сообщение об ошибке.
        /// </summary>
        public object Message { get; set; }
    }
}