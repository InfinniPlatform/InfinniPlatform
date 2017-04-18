using System;

namespace InfinniPlatform.PrintView.Abstractions
{
    /// <summary>
    /// Единицы измерения размера.
    /// </summary>
    [Serializable]
    public enum PrintSizeUnit
    {
        /// <summary>
        /// Пункты.
        /// </summary>
        Pt,

        /// <summary>
        /// Пиксели.
        /// </summary>
        Px,

        /// <summary>
        /// Дюймы.
        /// </summary>
        In,

        /// <summary>
        /// Сантиметры.
        /// </summary>
        Cm,

        /// <summary>
        /// Миллиметры.
        /// </summary>
        Mm
    }
}