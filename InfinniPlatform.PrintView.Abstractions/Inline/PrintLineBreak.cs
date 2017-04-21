using System;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Inline
{
    /// <summary>
    /// Элемент для создания разрыва строки.
    /// </summary>
    [Serializable]
    public class PrintLineBreak : PrintInline
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "LineBreak";


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintLineBreak;
        }
    }
}