namespace InfinniPlatform.DocumentStorage.HttpService.QuerySyntax
{
    /// <summary>
    /// Тип токена строки запроса.
    /// </summary>
    public enum QueryTokenKind
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        /// <remarks>
        /// Например, имя метода или свойства документа.
        /// </remarks>
        Identifier,

        /// <summary>
        /// Открывающая скобка.
        /// </summary>
        /// <remarks>
        /// Например, символ <c>(</c>.
        /// </remarks>
        OpenBracket,

        /// <summary>
        /// Нулевой указатель.
        /// </summary>
        /// <remarks>
        /// Например, литерал <c>null</c>.
        /// </remarks>
        Null,

        /// <summary>
        /// Логическое значение.
        /// </summary>
        /// <remarks>
        /// Например, литерал <c>true</c> или <c>false</c>.
        /// </remarks>
        Boolean,

        /// <summary>
        /// Целое значение.
        /// </summary>
        /// <remarks>
        /// Например, литерал <c>12345</c>.
        /// </remarks>
        Integer,

        /// <summary>
        /// Дробное значение.
        /// </summary>
        /// <remarks>
        /// Например, литерал <c>123.45</c>.
        /// </remarks>
        Float,

        /// <summary>
        /// Строковое значение.
        /// </summary>
        /// <remarks>
        /// Например, литерал <c>'Abc'</c>.
        /// </remarks>
        String,

        /// <summary>
        /// Разделитель аргументов.
        /// </summary>
        /// <remarks>
        /// Например, символ <c>,</c>.
        /// </remarks>
        ArgumentSeparator,

        /// <summary>
        /// Закрывающая скобка.
        /// </summary>
        /// <remarks>
        /// Например, символ <c>)</c>.
        /// </remarks>
        CloseBracket
    }
}