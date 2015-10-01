namespace InfinniPlatform.Api.Metadata
{
    /// <summary>
    ///     Тип данных.
    /// </summary>
    public enum DataType
    {
        /// <summary>
        ///     Логическое значение.
        /// </summary>
        Bool = 1,

        /// <summary>
        ///     Целое число.
        /// </summary>
        Integer = 2,

        /// <summary>
        ///     Дробное число.
        /// </summary>
        Float = 4,

        /// <summary>
        ///     Строка.
        /// </summary>
        String = 8,

        /// <summary>
        ///     Дата / время.
        /// </summary>
        DateTime = 16,

        /// <summary>
        ///     Уникальный идентификатор.
        /// </summary>
        Uuid = 32,

        /// <summary>
        ///     Двоичные данные.
        /// </summary>
        Binary = 64,

        /// <summary>
        ///     Объект.
        /// </summary>
        Object = 128,

        /// <summary>
        ///     Массив.
        /// </summary>
        Array = 256
    }
}