namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.ScrollPanel
{
    /// <summary>
    ///     Видимость полосы прокрутки.
    /// </summary>
    public enum ScrollVisibility
    {
        /// <summary>
        ///     Полоса прокрутки видима, если контейнер не может отобразить все содержимое.
        /// </summary>
        Auto,

        /// <summary>
        ///     Полоса прокрутки видима всегда, даже если контейнер может отобразить все содержимое.
        /// </summary>
        Visible,

        /// <summary>
        ///     Полоса прокрутки не видима, даже если контейнер не может отобразить все содержимое.
        /// </summary>
        Hidden
    }
}