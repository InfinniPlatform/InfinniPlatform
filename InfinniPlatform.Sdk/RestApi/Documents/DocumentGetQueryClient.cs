namespace InfinniPlatform.Sdk.RestApi.Documents
{
    /// <summary>
    /// Запрос на получение документов.
    /// </summary>
    public sealed class DocumentGetQueryClient
    {
        /// <summary>
        /// Строка полнотекстового поиска.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Правило фильтрации документов.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Правило отображения документов.
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// Правило сортировки документов.
        /// </summary>
        public string Order { get; set; }

        /// <summary>
        /// Необходимость подсчета количества.
        /// </summary>
        public bool? Count { get; set; }

        /// <summary>
        /// Количество документов, которое нужно пропустить.
        /// </summary>
        public int? Skip { get; set; }

        /// <summary>
        /// Максимальное количество документов, которое нужно выбрать.
        /// </summary>
        public int? Take { get; set; }
    }
}