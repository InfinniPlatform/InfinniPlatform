using System;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Стиль маркера элементов списка.
    /// </summary>
    [Serializable]
    public enum PrintListMarkerStyle
    {
        /// <summary>
        /// Маркер не отображается.
        /// </summary>
        None,

        /// <summary>
        /// Маркер отображается в виде закрашенного круга.
        /// </summary>
        Disc,

        /// <summary>
        /// Маркер отображается в виде незакрашенного круга.
        /// </summary>
        Circle,

        /// <summary>
        /// Маркер отображается в виде незакрашенного квадрата.
        /// </summary>
        Square,

        /// <summary>
        /// Маркер отображается в виде закрашенного квадрата.
        /// </summary>
        Box,

        /// <summary>
        /// Маркер отображается строчными римскими цифрами (например, i, ii, iii, iv, v).
        /// </summary>
        LowerRoman,

        /// <summary>
        /// Маркер отображается прописными римскими цифрами (например, I, II, III, IV, V).
        /// </summary>
        UpperRoman,

        /// <summary>
        /// Маркер отображается строчными латинскими буквами (например, a, b, c, d, e, f).
        /// </summary>
        LowerLatin,

        /// <summary>
        /// Маркер отображается прописными латинскими буквами (например, A, B, C, D, E, F).
        /// </summary>
        UpperLatin,

        /// <summary>
        /// Маркер отображается арабскими цифрами (например, 1, 2, 3, 4, 5).
        /// </summary>
        Decimal
    }
}