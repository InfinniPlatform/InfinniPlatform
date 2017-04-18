using System;

using InfinniPlatform.PrintView.Abstractions.Format;
using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Inline
{
    /// <summary>
    /// Элемент для выделения содержимого в виде гиперссылки.
    /// </summary>
    [Serializable]
    public class PrintHyperlink : PrintSpan
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public new const string TypeName = "Hyperlink";


        /// <summary>
        /// URI ресурса.
        /// </summary>
        public string Reference { get; set; }

        /// <summary>
        /// Формат отображения значения источника данных.
        /// </summary>
        public ValueFormat SourceFormat { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintHyperlink;
        }
    }
}