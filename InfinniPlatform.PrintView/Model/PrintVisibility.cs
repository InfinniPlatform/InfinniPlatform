namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Видимость элемента.
    /// </summary>
    public enum PrintVisibility
    {
        /// <summary>
        /// Не отображать элемент.
        /// </summary>
        Never,

        /// <summary>
        /// Всегда отображать элемент.
        /// </summary>
        Always,

        /// <summary>
        /// Отображать элемент только при наличии данных.
        /// </summary>
        Source
    }
}