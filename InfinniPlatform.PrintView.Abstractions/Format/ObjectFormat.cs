using System;

namespace InfinniPlatform.PrintView.Abstractions.Format
{
    /// <summary>
    /// Формат отображения объекта.
    /// </summary>
    [Serializable]
    public class ObjectFormat : ValueFormat
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "ObjectFormat";


        /// <summary>
        /// Строка форматирования значения.
        /// </summary>
        public string Format { get; set; }
    }
}