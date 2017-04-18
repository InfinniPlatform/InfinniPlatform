using System;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Вертикальное выравнивание шрифта.
    /// </summary>
    /// <remarks>
    /// Некоторые шрифты могут не поддерживать вертикальное выравнивание.
    /// </remarks>
    [Serializable]
    public enum PrintFontVariant
    {
        /// <summary>
        /// Без выравнивания.
        /// </summary>
        Normal,

        /// <summary>
        /// Подстрочное выравнивание (подстрочный индекс).
        /// </summary>
        Subscript,

        /// <summary>
        /// Надстрочное выравнивание (надстрочный индекс).
        /// </summary>
        Superscript
    }
}