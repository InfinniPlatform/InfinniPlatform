namespace InfinniPlatform.UserInterface.ViewBuilders.LinkViews
{
    /// <summary>
    ///     Способ открытия представления.
    /// </summary>
    public enum OpenMode
    {
        /// <summary>
        ///     Не открывать.
        /// </summary>
        None,

        /// <summary>
        ///     Открыть в модальном диалоговом окне.
        /// </summary>
        Dialog,

        /// <summary>
        ///     Открыть на новой вкладке текущего представления.
        /// </summary>
        TabPage,

        /// <summary>
        ///     Открыть на новой вкладке приложения.
        /// </summary>
        AppTabPage
    }
}