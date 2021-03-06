﻿using System;

namespace InfinniPlatform.PrintView.Inline
{
    /// <summary>
    /// Поворот изображения.
    /// </summary>
    [Serializable]
    public enum PrintImageRotation
    {
        /// <summary>
        /// Без поворота.
        /// </summary>
        Rotate0,

        /// <summary>
        /// Поворот на 90° по часовой стрелке.
        /// </summary>
        Rotate90,

        /// <summary>
        /// Поворот на 180° по часовой стрелке.
        /// </summary>
        Rotate180,

        /// <summary>
        /// Поворот на 270° по часовой стрелке.
        /// </summary>
        Rotate270
    }
}