using System;

using InfinniPlatform.PrintView.Abstractions.Properties;

namespace InfinniPlatform.PrintView.Abstractions.Inline
{
    /// <summary>
    /// Элемент для отображения содержимого с эффектом подчеркивания.
    /// </summary>
    [Serializable]
    public class PrintUnderline : PrintSpan
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public new const string TypeName = "Underline";


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintUnderline;
        }
    }
}