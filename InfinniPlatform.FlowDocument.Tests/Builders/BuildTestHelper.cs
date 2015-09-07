using System;

using InfinniPlatform.FlowDocument.Builders;
using InfinniPlatform.FlowDocument.Builders.Factories.Blocks;
using InfinniPlatform.FlowDocument.Builders.Factories.DisplayFormats;
using InfinniPlatform.FlowDocument.Builders.Factories.Inlines;
using InfinniPlatform.FlowDocument.Builders.Factories.Views;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Inlines;
using FrameworkFlowDocument = InfinniPlatform.FlowDocument.Model.Views.PrintViewDocument;

namespace InfinniPlatform.FlowDocument.Tests.Builders
{
	static class BuildTestHelper
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

			return new PrintElementBuildContext { ElementBuilder = elementBuilder };
		}


		private static T BuildElement<T>(string elementType, object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
			var buildContext = CreateBuildContext();

			if (elementContext != null)
			{
				elementContext(buildContext);
			}

			return (T)buildContext.ElementBuilder.BuildElement(buildContext, elementMetadata, elementType);
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

		public static PrintElementSpan BuildSpan(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementSpan>("Span", elementMetadata, elementContext);
		}

		public static PrintElementBold BuildBold(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementBold>("Bold", elementMetadata, elementContext);
		}

		public static PrintElementItalic BuildItalic(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementItalic>("Italic", elementMetadata, elementContext);
		}

		public static PrintElementUnderline BuildUnderline(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementUnderline>("Underline", elementMetadata, elementContext);
		}

		public static PrintElementHyperlink BuildHyperlink(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementHyperlink>("Hyperlink", elementMetadata, elementContext);
		}

		public static PrintElementLineBreak BuildLineBreak(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementLineBreak>("LineBreak", elementMetadata, elementContext);
		}

		public static PrintElementRun BuildRun(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementRun>("Run", elementMetadata, elementContext);
		}

		public static PrintElementImage BuildImage(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementImage>("Image", elementMetadata, elementContext);
		}

        public static PrintElementImage BuildBarcodeEan13(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementImage>("BarcodeEan13", elementMetadata, elementContext);
		}

        public static PrintElementImage BuildBarcodeQr(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementImage>("BarcodeQr", elementMetadata, elementContext);
		}


		// Blocks

		public static PrintElementSection BuildSection(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementSection>("Section", elementMetadata, elementContext);
		}

		public static PrintElementParagraph BuildParagraph(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementParagraph>("Paragraph", elementMetadata, elementContext);
		}

		public static PrintElementList BuildList(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementList>("List", elementMetadata, elementContext);
		}

		public static PrintElementTable BuildTable(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementTable>("Table", elementMetadata, elementContext);
		}

		public static PrintElementBlock BuildLine(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementBlock>("Line", elementMetadata, elementContext);
		}

        public static PrintElementBlock BuildPageBreak(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
            return BuildElement<PrintElementBlock>("PageBreak", elementMetadata, elementContext);
		}


		// Views

		public static FrameworkFlowDocument BuildPrintView(object elementMetadata, Action<PrintElementBuildContext> elementContext = null)
		{
			return BuildElement<FrameworkFlowDocument>("PrintView", elementMetadata, elementContext);
		}
	}
}