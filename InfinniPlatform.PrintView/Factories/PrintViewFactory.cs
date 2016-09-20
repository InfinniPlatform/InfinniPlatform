using InfinniPlatform.PrintView.Factories.Blocks;
using InfinniPlatform.PrintView.Factories.DisplayFormats;
using InfinniPlatform.PrintView.Factories.Inlines;
using InfinniPlatform.PrintView.Factories.Views;
using InfinniPlatform.PrintView.Model.Views;

namespace InfinniPlatform.PrintView.Factories
{
    internal class PrintViewFactory : IPrintViewFactory
    {
        private static readonly PrintElementBuilder ElementBuilder;

        static PrintViewFactory()
        {
            ElementBuilder = new PrintElementBuilder();

            // DisplayFormat
            ElementBuilder.Register("BooleanFormat", new BooleanFormatFactory());
            ElementBuilder.Register("DateTimeFormat", new DateTimeFormatFactory());
            ElementBuilder.Register("NumberFormat", new NumberFormatFactory());
            ElementBuilder.Register("ObjectFormat", new ObjectFormatFactory());

            // Block
            ElementBuilder.Register("Section", new PrintElementSectionFactory());
            ElementBuilder.Register("Paragraph", new PrintElementParagraphFactory());
            ElementBuilder.Register("List", new PrintElementListFactory());
            ElementBuilder.Register("Table", new PrintElementTableFactory());
            ElementBuilder.Register("Line", new PrintElementLineFactory());
            ElementBuilder.Register("PageBreak", new PrintElementPageBreakFactory());

            // Inline
            ElementBuilder.Register("Span", new PrintElementSpanFactory());
            ElementBuilder.Register("Bold", new PrintElementBoldFactory());
            ElementBuilder.Register("Italic", new PrintElementItalicFactory());
            ElementBuilder.Register("Underline", new PrintElementUnderlineFactory());
            ElementBuilder.Register("Hyperlink", new PrintElementHyperlinkFactory());
            ElementBuilder.Register("LineBreak", new PrintElementLineBreakFactory());
            ElementBuilder.Register("Run", new PrintElementRunFactory());
            ElementBuilder.Register("Image", new PrintElementImageFactory());

            // Barcode
            ElementBuilder.Register("BarcodeEan13", new PrintElementBarcodeEan13Factory());
            ElementBuilder.Register("BarcodeQr", new PrintElementBarcodeQrFactory());

            // Views
            ElementBuilder.Register("PrintView", new PrintViewElementFactory());
        }

        public PrintViewDocument Create(object printView, object printViewSource, PrintElementMetadataMap elementMetadataMap = null)
        {
            var buildContext = new PrintElementBuildContext
                               {
                                   IsDesignMode = false,
                                   PrintViewSource = printViewSource,
                                   ElementBuilder = ElementBuilder,
                                   ElementMetadataMap = elementMetadataMap
                               };

            var document = ElementBuilder.BuildElement(buildContext, printView, "PrintView") as PrintViewDocument;

            return document;
        }
    }
}