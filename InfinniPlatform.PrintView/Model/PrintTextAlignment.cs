﻿namespace InfinniPlatform.PrintView.Model
{
    /// <summary>
    /// Горизонтальное выравнивание текста элемента.
    /// </summary>
    public enum PrintTextAlignment
    {
        /// <summary>
        /// Выравнивание по левому краю родительского блока.
        /// </summary>
        Left,

        /// <summary>
        /// Выравнивание по центру родительского блока.
        /// </summary>
        Center,

        /// <summary>
        /// Выравнивание по правому краю родительского блока.
        /// </summary>
        Right,

        /// <summary>
        /// Выравнивание по ширине родительского блока.
        /// </summary>
        Justify
    }
}