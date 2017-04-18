using System;

namespace InfinniPlatform.PrintView.Abstractions.Format
{
    /// <summary>
    /// Формат отображения значения даты и времени.
    /// </summary>
    [Serializable]
    public class DateTimeFormat : ValueFormat
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "DateTimeFormat";


        /// <summary>
        /// Строка форматирования значения.
        /// </summary>
        public string Format { get; set; }
    }
}