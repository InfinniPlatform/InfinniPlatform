using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model.Inline
{
    /// <summary>
    /// Элемент для выделения содержимого курсивным шрифтом.
    /// </summary>
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