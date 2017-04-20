using System;
using System.Collections.Generic;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Inline
{
    /// <summary>
    /// Элемент для создания группы строковых элементов.
    /// </summary>
    [Serializable]
    public class PrintSpan : PrintInline
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Span";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintSpan()
        {
            Inlines = new List<PrintInline>();
        }


        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintInline> Inlines { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintSpan;
        }
    }
}