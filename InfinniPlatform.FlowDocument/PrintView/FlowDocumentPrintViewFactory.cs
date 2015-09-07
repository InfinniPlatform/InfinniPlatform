using InfinniPlatform.FlowDocument.Builders;
using InfinniPlatform.FlowDocument.Builders.Factories.Blocks;
using InfinniPlatform.FlowDocument.Builders.Factories.DisplayFormats;
using InfinniPlatform.FlowDocument.Builders.Factories.Inlines;
using InfinniPlatform.FlowDocument.Builders.Factories.Views;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.PrintView
{
	public sealed class FlowDocumentPrintViewFactory : IFlowDocumentPrintViewFactory
	{
		static FlowDocumentPrintViewFactory()
		{
			ElementBuilder = new PrintElementBuilder();

			// DisplayFormats
			ElementBuilder.Register("BooleanFormat", new BooleanFormatFactory());
			ElementBuilder.Register("DateTimeFormat", new DateTimeFormatFactory());
			ElementBuilder.Register("NumberFormat", new NumberFormatFactory());
			ElementBuilder.Register("ObjectFormat", new ObjectFormatFactory());

			// Blocks
			ElementBuilder.Register("Section", new PrintElementSectionFactory());
			ElementBuilder.Register("Paragraph", new PrintElementParagraphFactory());
			ElementBuilder.Register("List", new PrintElementListFactory());
			ElementBuilder.Register("Table", new PrintElementTableFactory());
			ElementBuilder.Register("Line", new PrintElementLineFactory());
			ElementBuilder.Register("PageBreak", new PrintElementPageBreakFactory());

			// Inlines
			ElementBuilder.Register("Span", new PrintElementSpanFactory());
			ElementBuilder.Register("Bold", new PrintElementBoldFactory());
			ElementBuilder.Register("Italic", new PrintElementItalicFactory());
			ElementBuilder.Register("Underline", new PrintElementUnderlineFactory());
			ElementBuilder.Register("Hyperlink", new PrintElementHyperlinkFactory());
			ElementBuilder.Register("LineBreak", new PrintElementLineBreakFactory());
			ElementBuilder.Register("Run", new PrintElementRunFactory());
			ElementBuilder.Register("Image", new PrintElementImageFactory());

			// Barcodes
			ElementBuilder.Register("BarcodeEan13", new PrintElementBarcodeEan13Factory());
			ElementBuilder.Register("BarcodeQr", new PrintElementBarcodeQrFactory());

			// Views
			ElementBuilder.Register("PrintView", new PrintViewFactory());
		}


		private static readonly PrintElementBuilder ElementBuilder;


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