namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Оформление текста элемента.
    /// </summary>
    public enum PrintTextDecoration
    {
        /// <summary>
        /// Без оформления.
        /// </summary>
        Normal,

        /// <summary>
        /// Линия над текстом.
        /// </summary>
        OverLine,

        /// <summary>
        /// Линия поверх текста (зачеркнутый текст).
        /// </summary>
        Strikethrough,

        /// <summary>
        /// Линия под текста (подчеркнутый текст).
        /// </summary>
        Underline
    }
}