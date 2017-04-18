using System;

using InfinniPlatform.PrintView.Abstractions.Format;
using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Inline
{
    /// <summary>
    /// Элемент для вывода неформатированного текста.
    /// </summary>
    [Serializable]
    public class PrintRun : PrintInline
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Run";


        /// <summary>
        /// Текст.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Формат отображения значения источника данных.
        /// </summary>
        public ValueFormat SourceFormat { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintRun;
        }
    }
}