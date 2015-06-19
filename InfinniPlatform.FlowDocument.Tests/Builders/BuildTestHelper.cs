using System;
using System.Windows.Documents;
using InfinniPlatform.FlowDocument.Builders;
using InfinniPlatform.FlowDocument.Builders.Factories.Blocks;
using InfinniPlatform.FlowDocument.Builders.Factories.DisplayFormats;
using InfinniPlatform.FlowDocument.Builders.Factories.Inlines;
using InfinniPlatform.FlowDocument.Builders.Factories.Views;
using FrameworkFlowDocument = System.Windows.Documents.FlowDocument;

namespace InfinniPlatform.FlowDocument.Tests.Builders
{
    internal static class BuildTestHelper
    {
        private static PrintElementBuildContext CreateBuildContext()
        {
            var elementBuilder = new PrintElementBuilder();

            // DisplayFormats
            elementBuilder.Register("BooleanFormat", new BooleanFormatFactory());
            elementBuilder.Register("DateTimeFormat", new DateTimeFormatFactory());
            elementBuilder.Register("NumberFormat", new NumberFormatFactory());
            elementBuilder.Register("ObjectFormat", new ObjectFormatFactory());

            // Blocks
            elementBuilder.Register("Section", new PrintElementSectionFactory());
            elementBuilder.Register("Paragraph", new PrintElementParagraphFactory());
            elementBuilder.Register("List", new PrintElementListFactory());
            elementBuilder.Register("Table", new PrintElementTableFactory());
            elementBuilder.Register("Line", new PrintElementLineFactory());
            elementBuilder.Register("PageBreak", new PrintElementPageBreakFactory());

            // Inlines
            elementBuilder.Register("Span", new PrintElementSpanFactory());
            elementBuilder.Register("Bold", new PrintElementBoldFactory());
            elementBuilder.Register("Italic", new PrintElementItalicFactory());
            elementBuilder.Register("Underline", new PrintElementUnderlineFactory());
            elementBuilder.Register("Hyperlink", new PrintElementHyperlinkFactory());
            elementBuilder.Register("LineBreak", new PrintElementLineBreakFactory());
            elementBuilder.Register("Run", new PrintElementRunFactory());
            elementBuilder.Register("Image", new PrintElementImageFactory());

            // Barcodes
            elementBuilder.Register("BarcodeEan13", new PrintElementBarcodeEan13Factory());
            elementBuilder.Register("BarcodeQr", new PrintElementBarcodeQrFactory());

            // Views
            elementBuilder.Register("PrintView", new PrintViewFactory());

            return new PrintElementBuildContext {ElementBuilder = elementBuilder};
        }

        private static T BuildElement<T>(string elementType, object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            var buildContext = CreateBuildContext();

            if (elementContext != null)
            {
                elementContext(buildContext);
            }

            return (T) buildContext.ElementBuilder.BuildElement(buildContext, elementMetadata, elementType);
        }

        // DisplayFormats

        public static Func<object, string> BuildBooleanFormat(object elementMetadata)
        {
            return BuildElement<Func<object, string>>("BooleanFormat", elementMetadata);
        }

        public static Func<object, string> BuildDateTimeFormat(object elementMetadata)
        {
            return BuildElement<Func<object, string>>("DateTimeFormat", elementMetadata);
        }

        public static Func<object, string> BuildNumberFormat(object elementMetadata)
        {
            return BuildElement<Func<object, string>>("NumberFormat", elementMetadata);
        }

        public static Func<object, string> BuildObjectFormat(object elementMetadata)
        {
            return BuildElement<Func<object, string>>("ObjectFormat", elementMetadata);
        }

        // Inlines

        public static Span BuildSpan(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Span>("Span", elementMetadata, elementContext);
        }

        public static Bold BuildBold(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Bold>("Bold", elementMetadata, elementContext);
        }

        public static Italic BuildItalic(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Italic>("Italic", elementMetadata, elementContext);
        }

        public static Underline BuildUnderline(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Underline>("Underline", elementMetadata, elementContext);
        }

        public static Hyperlink BuildHyperlink(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Hyperlink>("Hyperlink", elementMetadata, elementContext);
        }

        public static LineBreak BuildLineBreak(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<LineBreak>("LineBreak", elementMetadata, elementContext);
        }

        public static Run BuildRun(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Run>("Run", elementMetadata, elementContext);
        }

        public static InlineUIContainer BuildImage(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<InlineUIContainer>("Image", elementMetadata, elementContext);
        }

        public static InlineUIContainer BuildBarcodeEan13(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<InlineUIContainer>("BarcodeEan13", elementMetadata, elementContext);
        }

        public static InlineUIContainer BuildBarcodeQr(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<InlineUIContainer>("BarcodeQr", elementMetadata, elementContext);
        }

        // Blocks

        public static Section BuildSection(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Section>("Section", elementMetadata, elementContext);
        }

        public static Paragraph BuildParagraph(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Paragraph>("Paragraph", elementMetadata, elementContext);
        }

        public static List BuildList(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<List>("List", elementMetadata, elementContext);
        }

        public static Table BuildTable(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Table>("Table", elementMetadata, elementContext);
        }

        public static Block BuildLine(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Block>("Line", elementMetadata, elementContext);
        }

        public static Block BuildPageBreak(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<Block>("PageBreak", elementMetadata, elementContext);
        }

        // Views

        public static FrameworkFlowDocument BuildPrintView(object elementMetadata,
            Action<PrintElementBuildContext> elementContext = null)
        {
            return BuildElement<FrameworkFlowDocument>("PrintView", elementMetadata, elementContext);
        }
    }
}