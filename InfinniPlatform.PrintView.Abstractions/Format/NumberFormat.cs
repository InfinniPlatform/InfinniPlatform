﻿using System;

namespace InfinniPlatform.PrintView.Format
{
    /// <summary>
    /// Формат отображения числового значения.
    /// </summary>
    [Serializable]
    public class NumberFormat : ValueFormat
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "NumberFormat";


        /// <summary>
        /// Строка форматирования значения.
        /// </summary>
        public string Format { get; set; }
    }
}