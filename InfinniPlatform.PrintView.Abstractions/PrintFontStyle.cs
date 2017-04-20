﻿using System;

namespace InfinniPlatform.PrintView
{
    /// <summary>
    /// Стиль шрифта.
    /// </summary>
    /// <remarks>
    /// Некоторые шрифты могут не поддерживать определенные стили шрифта. Например, курсив - это специальный
    /// шрифт имитирующий рукописный, наклонный же образуется путем программного (искусственного) наклона
    /// обычных знаков вправо. Таким образом, наклонный обычно используется для имитации курсива в случае,
    /// если у используемого шрифта отсутствует курсивное начертание.
    /// </remarks>
    [Serializable]
    public enum PrintFontStyle
    {
        /// <summary>
        /// Обычный.
        /// </summary>
        Normal,

        /// <summary>
        /// Курсив.
        /// </summary>
        Italic,

        /// <summary>
        /// Наклонный.
        /// </summary>
        Oblique
    }
}