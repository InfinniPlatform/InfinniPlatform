namespace InfinniPlatform.Core.Http
{
    /// <summary>
    /// Результат обработки запроса.
    /// </summary>
    /// <typeparam name="TResult">Тип результата.</typeparam>
    public class ServiceResult<TResult>
    {
        /// <summary>
        /// Успешность выполнения запроса.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Результат выполнения запроса.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// Сообщение об ошибке выполнения запроса.
        /// </summary>
        public string Error { get; set; }
    }
}