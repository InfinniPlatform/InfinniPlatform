using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintView.Factories.Block;
using InfinniPlatform.PrintView.Factories.Format;
using InfinniPlatform.PrintView.Factories.Inline;

namespace InfinniPlatform.PrintView.Factories
{
    internal static class BuildTestHelper
    {
        private static PrintElementFactoryContext CreateBuildContext(object dataSource = null, IEnumerable<PrintStyle> styles = null, PrintDocumentMap documentMap = null)
        {
            var factory = new PrintElementBuilder();

            factory.Register(new PrintDocumentFactory());

            // Format
            factory.Register(new BooleanFormatFactory());
            factory.Register(new DateTimeFormatFactory());
            factory.Register(new NumberFormatFactory());
            factory.Register(new ObjectFormatFactory());

            // Block
            factory.Register(new PrintLineFactory());
            factory.Register(new PrintListFactory());
            factory.Register(new PrintPageBreakFactory());
            factory.Register(new PrintParagraphFactory());
            factory.Register(new PrintSectionFactory());
            factory.Register(new PrintTableFactory());

            // Inline
            factory.Register(new PrintBarcodeEan13Factory());
            factory.Register(new PrintBarcodeQrFactory());
            factory.Register(new PrintBoldFactory());
            factory.Register(new PrintHyperlinkFactory());
            factory.Register(new PrintImageFactory());
            factory.Register(new PrintItalicFactory());
            factory.Register(new PrintLineBreakFactory());
            factory.Register(new PrintRunFactory());
            factory.Register(new PrintSpanFactory());
            factory.Register(new PrintUnderlineFactory());

            var context = new PrintElementFactoryContext
                          {
                              IsDesignMode = false,
                              Source = dataSource,
                              Styles = styles?.ToDictionary(i => i.Name),
                              Factory = factory,
                              DocumentMap = documentMap
                          };

            return context;
        }


        public static T BuildElement<T>(object template, object dataSource = null, IEnumerable<PrintStyle> styles = null, Action<PrintElementFactoryContext> initContext = null)
        {
            var context = CreateBuildContext(dataSource, styles);

            initContext?.Invoke(context);

            return (T)context.Factory.BuildElement(context, template);
        }
    }
}