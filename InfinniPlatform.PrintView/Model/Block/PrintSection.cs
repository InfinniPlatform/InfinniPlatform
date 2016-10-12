using System.Collections.Generic;

using InfinniPlatform.PrintView.Properties;

namespace InfinniPlatform.PrintView.Model.Block
{
    /// <summary>
    /// Элемент для создания группы блочных элементов.
    /// </summary>
    public class PrintSection : PrintBlock
    {
        /// <summary>
        /// Имя типа для сериализации.
        /// </summary>
        public const string TypeName = "Section";


        /// <summary>
        /// Конструктор.
        /// </summary>
        public PrintSection()
        {
            Blocks = new List<PrintBlock>();
        }


        /// <summary>
        /// Список элементов.
        /// </summary>
        public List<PrintBlock> Blocks { get; set; }


        /// <summary>
        /// Возвращает отображаемое имя типа элемента.
        /// </summary>
        public override string GetDisplayTypeName()
        {
            return Resources.PrintSection;
        }
    }
}