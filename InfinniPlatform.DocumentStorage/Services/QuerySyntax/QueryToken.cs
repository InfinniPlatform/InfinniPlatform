namespace InfinniPlatform.DocumentStorage.Services.QuerySyntax
{
    /// <summary>
    /// Токен строки запроса.
    /// </summary>
    public sealed class QueryToken
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="kind">Тип токена.</param>
        /// <param name="value">Значение токена.</param>
        public QueryToken(QueryTokenKind kind, object value)
        {
            Kind = kind;
            Value = value;
        }

        /// <summary>
        /// Тип токена.
        /// </summary>
        public readonly QueryTokenKind Kind;

        /// <summary>
        /// Значение токена.
        /// </summary>
        public readonly object Value;
    }
}