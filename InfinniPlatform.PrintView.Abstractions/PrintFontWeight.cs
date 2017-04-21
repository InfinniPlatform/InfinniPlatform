using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Насыщенность шрифта.
    /// </summary>
    /// <remarks>
    /// Многие шрифты поддерживают не все уровни насыщенности.
    /// </remarks>
    [Serializable]
    public enum PrintFontWeight
    {
        /// <summary>
        /// Нормальный (400).
        /// </summary>
        Normal,

        /// <summary>
        /// Ультра-тонкий (100).
        /// </summary>
        UltraLight,

        /// <summary>
        /// Экстра-тонкий (200).
        /// </summary>
        ExtraLight,

        /// <summary>
        /// Тонкий (300).
        /// </summary>
        Light,

        /// <summary>
        /// Средний (500).
        /// </summary>
        Medium,

        /// <summary>
        /// Полужирный (600).
        /// </summary>
        SemiBold,

        /// <summary>
        /// Жирный (700).
        /// </summary>
        Bold,

        /// <summary>
        /// Экстра-жирный (800).
        /// </summary>
        ExtraBold,

        /// <summary>
        /// Ультра-жирный (900).
        /// </summary>
        UltraBold
    }
}