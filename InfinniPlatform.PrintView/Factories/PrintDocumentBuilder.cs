using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Factories.Block;
using InfinniPlatform.PrintView.Factories.Format;
using InfinniPlatform.PrintView.Factories.Inline;
using InfinniPlatform.PrintView.Model;

namespace InfinniPlatform.PrintView.Factories
{
    /// <summary>
    /// Реализует <see cref="IPrintDocumentBuilder"/>.
    /// </summary>
    /// <remarks>
    /// Предполагается, что в явном виде этот класс будет создаваться только в редакторе
    /// печатных представлений. В иных случаях будет использоваться IoC контейнер.
    /// </remarks>
    public class PrintDocumentBuilder : IPrintDocumentBuilder
    {
        private static readonly PrintElementBuilder Factory;


        static PrintDocumentBuilder()
        {
            Factory = new PrintElementBuilder();

            Factory.Register(new PrintDocumentFactory());

            // Format
            Factory.Register(new BooleanFormatFactory());
            Factory.Register(new DateTimeFormatFactory());
            Factory.Register(new NumberFormatFactory());
            Factory.Register(new ObjectFormatFactory());

            // Block
            Factory.Register(new PrintLineFactory());
            Factory.Register(new PrintListFactory());
            Factory.Register(new PrintPageBreakFactory());
            Factory.Register(new PrintParagraphFactory());
            Factory.Register(new PrintSectionFactory());
            Factory.Register(new PrintTableFactory());

            // Inline
            Factory.Register(new PrintBarcodeEan13Factory());
            Factory.Register(new PrintBarcodeQrFactory());
            Factory.Register(new PrintBoldFactory());
            Factory.Register(new PrintHyperlinkFactory());
            Factory.Register(new PrintImageFactory());
            Factory.Register(new PrintItalicFactory());
            Factory.Register(new PrintLineBreakFactory());
            Factory.Register(new PrintRunFactory());
            Factory.Register(new PrintSpanFactory());
            Factory.Register(new PrintUnderlineFactory());
        }


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="isDesignMode">Работа в режиме редактора печатных представлений.</param>
        public PrintDocumentBuilder(bool isDesignMode = false)
        {
            _isDesignMode = isDesignMode;
        }


        private readonly bool _isDesignMode;


        /// <summary>
        /// Создает документ печатного представления.
        /// </summary>
        /// <param name="template">Шаблон печатного представления.</param>
        /// <param name="dataSource">Данные печатного представления.</param>
        /// <param name="documentMap">Соответствие между элементами документа и их шаблонами.</param>
        /// <returns>Документ печатного представления.</returns>
        public PrintDocument Build(PrintDocument template, object dataSource, PrintDocumentMap documentMap = null)
        {
            var context = new PrintElementFactoryContext
                          {
                              IsDesignMode = _isDesignMode,
                              Source = dataSource,
                              Factory = Factory,
                              DocumentMap = documentMap
                          };

            var document = (PrintDocument)Factory.BuildElement(context, template);

            return document;
        }
    }
}