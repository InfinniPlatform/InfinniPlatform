namespace InfinniPlatform.Api.Security
{
    /// <summary>
    ///     Ссылка на внешний документ.
    /// </summary>
    public sealed class ForeignKey
    {
        /// <summary>
        ///     Уникальный идентификатор внешнего документа.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     Строковое представление внешнего документа.
        /// </summary>
        public string DisplayName { get; set; }
    }
}