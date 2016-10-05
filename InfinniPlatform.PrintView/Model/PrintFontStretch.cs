namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Степень растягивания шрифта по горизонтали.
    /// </summary>
    /// <remarks>
    /// Многие шрифты поддерживают не все уровни растягивания, а некоторые вообще не 
    /// поддерживают растягивания (например, моноширные шрифты типа "Courier New").
    /// </remarks>
    public enum PrintFontStretch
    {
        /// <summary>
        /// Нормальный (100%).
        /// </summary>
        Normal,

        /// <summary>
        /// Ультра-уплотненный (50% от нормального).
        /// </summary>
        UltraCondensed,

        /// <summary>
        /// Экстра-уплотненный (62.5% от нормального).
        /// </summary>
        ExtraCondensed,

        /// <summary>
        /// Уплотненный (75.0% от нормального).
        /// </summary>
        Condensed,

        /// <summary>
        /// Полууплотненный (87.5% от нормального).
        /// </summary>
        SemiCondensed,

        /// <summary>
        /// Полурастянутый (112.5% от нормального).
        /// </summary>
        SemiExpanded,

        /// <summary>
        /// Растянутый (125.0% от нормального).
        /// </summary>
        Expanded,

        /// <summary>
        /// Экстра-растянутый (150.0% от нормального).
        /// </summary>
        ExtraExpanded,

        /// <summary>
        /// Ультра-растянутый (200.0% от нормального).
        /// </summary>
        UltraExpanded
    }
}