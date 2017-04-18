using System;

using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Inline
{
    /// <summary>
    /// Элемент для выделения содержимого курсивным шрифтом.
    /// </summary>
    [Serializable]
    public class PrintItalic : PrintSpan
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public new const string TypeName = "Italic";


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintItalic;
        }
    }
}