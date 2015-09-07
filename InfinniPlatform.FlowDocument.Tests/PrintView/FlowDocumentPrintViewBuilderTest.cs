using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.PrintView;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.PrintView;
using InfinniPlatform.FlowDocument.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.PrintView
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class FlowDocumentPrintViewBuilderTest
	{
		[Test]
        [Ignore("Manual")]
        [RequiresMTA]
		[TestCase(PrintViewFileFormat.Pdf)]
        [TestCase(PrintViewFileFormat.Html)]
		public void ShouldBuildFile(PrintViewFileFormat printViewFileFormat)
		{
			// Given
			var target = new FlowDocumentPrintViewBuilder();
			var printView = CreateTestPrintView();

			// When
			byte[] result = target.BuildFile(printView, null, printViewFileFormat);

			// Then
			Assert.IsNotNull(result);
			Assert.Greater(result.Length, 0);
		}

		[Test]
		[RequiresMTA]
		[Ignore("Manual")]
		[TestCase(PrintViewFileFormat.Pdf)]
		[TestCase(PrintViewFileFormat.Html)]
		public void ShouldBuildFileAndThenOpenIt(PrintViewFileFormat printViewFileFormat)
		{
			// Given
			var target = new FlowDocumentPrintViewBuilder();
			var printView = CreateTestPrintView();

			// When
			byte[] result = target.BuildFile(printView, null, printViewFileFormat);

			// Then
			Assert.IsNotNull(result);
			Assert.Greater(result.Length, 0);

			OpenResultFile(result, printViewFileFormat);
		}

		private static void OpenResultFile(byte[] file, PrintViewFileFormat printViewFileFormat)
		{
			var fileName = "PrintView." + printViewFileFormat;

			using (var writer = File.Create(fileName))
			{
				writer.Write(file, 0, file.Length);
				writer.Flush();
				writer.Close();
			}

			Process.Start(fileName);
		}


		private static dynamic CreateTestPrintView()
		{
			// Проверяются основные аспекты форматирования

			dynamic printView = new DynamicWrapper();
			printView.Blocks = new List<object>();

			printView.Blocks.Add(CreateParagraph(CreateRun("Normal text. Normal text. Normal text. Normal text. Normal text. Normal text. Normal text. Normal text. Normal text. Normal text. Normal text. Normal text.")));
			printView.Blocks.Add(CreateParagraph(CreateBold(CreateRun("Bold text. Bold text. Bold text. Bold text. Bold text. Bold text. Bold text. Bold text. Bold text. Bold text. Bold text. Bold text."))));
			printView.Blocks.Add(CreateParagraph(CreateItalic(CreateRun("Italic text. Italic text. Italic text. Italic text. Italic text. Italic text. Italic text. Italic text. Italic text. Italic text. Italic text. Italic text."))));
			printView.Blocks.Add(CreateParagraph(CreateUnderline(CreateRun("Underline text. Underline text. Underline text. Underline text. Underline text. Underline text. Underline text. Underline text. Underline text. Underline text. Underline text. Underline text."))));
			printView.Blocks.Add(CreateParagraph(CreateHyperlink(CreateRun("Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text. Hyperlink text."))));
			printView.Blocks.Add(CreateParagraph(CreateBold(CreateItalic(CreateUnderline(CreateHyperlink(CreateRun("Bold & Italic & Underline & Hyperlink text. Bold & Italic & Underline & Hyperlink text. Bold & Italic & Underline & Hyperlink text. Bold & Italic & Underline & Hyperlink text.")))))));
			printView.Blocks.Add(CreateParagraph(CreateRun("Before line break. Before line break. Before line break. Before line break. Before line break. Before line break. Before line break. Before line break."), CreateLineBreak(), CreateRun("After line break.")));
			printView.Blocks.Add(CreateParagraph(CreateSpan(CreateRun("Span text. "), CreateBold(CreateRun("Span text. ")), CreateItalic(CreateRun("Span text. ")), CreateUnderline(CreateRun("Span text.")), CreateRun(" "), CreateHyperlink(CreateRun("Span text. ")))));
			printView.Blocks.Add(CreateParagraph(CreateImage("Rotate0"), CreateImage("Rotate90"), CreateImage("Rotate180"), CreateImage("Rotate270")));
			printView.Blocks.Add(CreateParagraph(CreateBarcodeEan13("Rotate0"), CreateBarcodeEan13("Rotate90"), CreateBarcodeEan13("Rotate180"), CreateBarcodeEan13("Rotate270")));
			printView.Blocks.Add(CreateParagraph(CreateBarcodeQr("Rotate0"), CreateBarcodeQr("Rotate90"), CreateBarcodeQr("Rotate180"), CreateBarcodeQr("Rotate270")));
			printView.Blocks.Add(CreateSection(CreateParagraph(CreateRun("Before page break.")), CreatePageBreak(), CreateParagraph(CreateRun("After page break."))));
			printView.Blocks.Add(CreateList("List with 'None' marker style:", "None"));
			printView.Blocks.Add(CreateList("List with 'Disc' marker style:", "Disc"));
			printView.Blocks.Add(CreateList("List with 'Circle' marker style:", "Circle"));
			printView.Blocks.Add(CreateList("List with 'Square' marker style:", "Square"));
			printView.Blocks.Add(CreateList("List with 'Box' marker style:", "Box"));
			printView.Blocks.Add(CreateList("List with 'LowerRoman' marker style:", "LowerRoman"));
			printView.Blocks.Add(CreateList("List with 'UpperRoman' marker style:", "UpperRoman"));
			printView.Blocks.Add(CreateList("List with 'LowerLatin' marker style:", "LowerLatin"));
			printView.Blocks.Add(CreateList("List with 'UpperLatin' marker style:", "UpperLatin"));
			printView.Blocks.Add(CreateList("List with 'Decimal' marker style:", "Decimal"));
			printView.Blocks.Add(CreateTable("Table:"));
			printView.Blocks.Add(CreateSection(CreateParagraph(CreateRun("Before line.")), CreateLine(), CreateParagraph(CreateRun("After line."))));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Arial 10pt. "), CreateRunWithFont("Arial 15pt. ", "Arial", 15), CreateRunWithFont("Arial 20pt. ", "Arial", 20)));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Tahoma 10pt. ", "Tahoma"), CreateRunWithFont("Tahoma 15pt. ", "Tahoma", 15), CreateRunWithFont("Tahoma 20pt. ", "Tahoma", 20)));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Courier New 10pt. ", "Courier New"), CreateRunWithFont("Courier New 15pt. ", "Courier New", 15), CreateRunWithFont("Courier New 20pt. ", "Courier New", 20)));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Normal. ", "Arial", 10, style: "Normal"), CreateRunWithFont("Italic. ", "Arial", 10, style: "Italic"), CreateRunWithFont("Oblique. ", "Arial", 10, style: "Oblique")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Normal. ", "Arial", 10, stretch: "Normal"), CreateRunWithFont("UltraCondensed. ", "Arial", 10, stretch: "UltraCondensed"), CreateRunWithFont("ExtraCondensed. ", "Arial", 10, stretch: "ExtraCondensed"), CreateRunWithFont("Condensed. ", "Arial", 10, stretch: "Condensed"), CreateRunWithFont("SemiCondensed. ", "Arial", 10, stretch: "SemiCondensed"), CreateRunWithFont("SemiExpanded. ", "Arial", 10, stretch: "SemiExpanded"), CreateRunWithFont("Expanded. ", "Arial", 10, stretch: "Expanded"), CreateRunWithFont("ExtraExpanded. ", "Arial", 10, stretch: "ExtraExpanded"), CreateRunWithFont("UltraExpanded. ", "Arial", 10, stretch: "UltraExpanded")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Normal. ", "Arial", 10, weight: "Normal"), CreateRunWithFont("UltraLight. ", "Arial", 10, weight: "UltraLight"), CreateRunWithFont("ExtraLight. ", "Arial", 10, weight: "ExtraLight"), CreateRunWithFont("Light. ", "Arial", 10, weight: "Light"), CreateRunWithFont("Medium. ", "Arial", 10, weight: "Medium"), CreateRunWithFont("SemiBold. ", "Arial", 10, weight: "SemiBold"), CreateRunWithFont("Bold. ", "Arial", 10, weight: "Bold"), CreateRunWithFont("ExtraBold. ", "Arial", 10, weight: "ExtraBold"), CreateRunWithFont("UltraBold. ", "Arial", 10, weight: "UltraBold")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("Normal. ", "Arial", 10, variant: "Normal"), CreateRunWithFont("Subscript. ", "Arial", 10, variant: "Subscript"), CreateRunWithFont("Superscript. ", "Arial", 10, variant: "Superscript")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("H"), CreateRunWithFont("2", "Palatino Linotype", variant: "Subscript"), CreateRunWithFont("O")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("C"), CreateRunWithFont("2", "Palatino Linotype", variant: "Subscript"), CreateRunWithFont("H"), CreateRunWithFont("5", "Palatino Linotype", variant: "Subscript"), CreateRunWithFont("OH")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithFont("a"), CreateRunWithFont("2", "Palatino Linotype", variant: "Superscript"), CreateRunWithFont(" + b"), CreateRunWithFont("2", "Palatino Linotype", variant: "Superscript"), CreateRunWithFont(" = c"), CreateRunWithFont("2", "Palatino Linotype", variant: "Superscript")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithTextDecoration("Normal. ", "Normal"), CreateRunWithTextDecoration("Overline. ", "Overline"), CreateRunWithTextDecoration("Strikethrough. ", "Strikethrough"), CreateRunWithTextDecoration("Underline. ", "Underline")));
			printView.Blocks.Add(CreateParagraph(CreateRunWithColor("Green foreground. ", "Green", null), CreateRunWithColor("Red background. ", null, "Red"), CreateRunWithColor("Green foreground & Red background. ", "Green", "Red")));
			printView.Blocks.Add(CreatePageBreak());
			printView.Blocks.Add(CreateParagraphWithTextAlignment("Left", CreateRun("Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text. Left text.")));
			printView.Blocks.Add(CreateParagraphWithTextAlignment("Right", CreateRun("Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text. Right text.")));
			printView.Blocks.Add(CreateParagraphWithTextAlignment("Center", CreateRun("Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text. Center text.")));
			printView.Blocks.Add(CreateParagraphWithTextAlignment("Justify", CreateRun("Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text. Justify text.")));
			printView.Blocks.Add(CreateParagraphWithTextCase("SentenceCase", CreateRun("sentencecase"), CreateRun(" sentencecase.")));
			printView.Blocks.Add(CreateParagraphWithTextCase("Lowercase", CreateRun("LOWERCASE "), CreateRun(" LOWERCASE.")));
			printView.Blocks.Add(CreateParagraphWithTextCase("Uppercase", CreateRun("uppercase "), CreateRun(" uppercase.")));
			printView.Blocks.Add(CreateParagraphWithTextCase("ToggleCase", CreateRun("Togglecase "), CreateRun(" tOGGLECASE.")));
            printView.Blocks.Add(CreateSectionWithBorder(new PrintElementThickness(15), new PrintElementThickness(20), new PrintElementThickness(4), "Red", "Green", CreateSectionWithBorder(new PrintElementThickness(5), new PrintElementThickness(10), new PrintElementThickness(2), "Blue", "Yellow", CreateParagraph(CreateRun("Section & Margin & Padding & Border & Background")))));

			return printView;
		}

		private static dynamic CreateList(string listHeaderText, string markerStyle)
		{
			dynamic listSection = CreateSection();

			dynamic listHeader = CreateParagraph(CreateBold(CreateRun(listHeaderText)));
			listSection.Section.Blocks.Add(listHeader);

			dynamic listItem = new DynamicWrapper();
			listItem.List = new DynamicWrapper();
			listItem.List.StartIndex = 1;
			listItem.List.MarkerStyle = markerStyle;
			listItem.List.MarkerOffsetSize = 5;
			listItem.List.MarkerOffsetSizeUnit = "Px";
			listItem.List.Padding = new DynamicWrapper();
			listItem.List.Padding.Left = 50;
			listItem.List.Padding.SizeUnit = "Px";
			listItem.List.Items = new List<object>();
			listSection.Section.Blocks.Add(listItem);

			var item1 = CreateParagraph(CreateRun("Item1"));
			listItem.List.Items.Add(item1);

			var item2 = CreateParagraph(CreateRun("Item2"));
			listItem.List.Items.Add(item2);

			var item3 = CreateParagraph(CreateRun("Item3"));
			listItem.List.Items.Add(item3);

			return listSection;
		}

		private static dynamic CreateTable(string tableHeaderText)
		{
			dynamic tableSection = CreateSection();

			dynamic listHeader = CreateParagraph(CreateBold(CreateRun(tableHeaderText)));
			tableSection.Section.Blocks.Add(listHeader);

			dynamic tableItem = new DynamicWrapper();
			tableItem.Table = new DynamicWrapper();
			tableItem.Table.ShowHeader = true;
			tableItem.Table.Columns = new List<object>();
			tableItem.Table.Rows = new List<object>();
			tableSection.Section.Blocks.Add(tableItem);

			dynamic tableColumn1 = new DynamicWrapper();
			tableColumn1.Size = 100;
			tableColumn1.SizeUnit = "Px";
			tableColumn1.Header = CreateTableCell("Header1");
			tableItem.Table.Columns.Add(tableColumn1);

			dynamic tableColumn2 = new DynamicWrapper();
			tableColumn2.Size = 200;
			tableColumn2.SizeUnit = "Px";
			tableColumn2.Header = CreateTableCell("Header2");
			tableItem.Table.Columns.Add(tableColumn2);

			dynamic tableColumn3 = new DynamicWrapper();
			tableColumn3.Size = null;
			tableColumn3.SizeUnit = null;
			tableColumn3.Header = CreateTableCell("Header3");
			tableItem.Table.Columns.Add(tableColumn3);

			dynamic tableRow1 = new DynamicWrapper();
			tableRow1.Cells = new List<object>();
			tableRow1.Cells.Add(CreateTableCell("11"));
			tableRow1.Cells.Add(CreateTableCell("12"));
			tableRow1.Cells.Add(CreateTableCell("13"));
			tableItem.Table.Rows.Add(tableRow1);

			dynamic tableRow2 = new DynamicWrapper();
			tableRow2.Cells = new List<object>();
			tableRow2.Cells.Add(CreateTableCell("21"));
			tableRow2.Cells.Add(CreateTableCell("22"));
			tableRow2.Cells.Add(CreateTableCell("23"));
			tableItem.Table.Rows.Add(tableRow2);

			dynamic tableRow3 = new DynamicWrapper();
			tableRow3.Cells = new List<object>();
			tableRow3.Cells.Add(CreateTableCell("31"));
			tableRow3.Cells.Add(CreateTableCell("32"));
			tableRow3.Cells.Add(CreateTableCell("33"));
			tableItem.Table.Rows.Add(tableRow3);

			return tableSection;
		}

		private static dynamic CreateTableCell(string text)
		{
			dynamic cell = new DynamicWrapper();
			cell.Block = CreateParagraph(CreateRun(text));

			return cell;
		}

        private static dynamic CreateSectionWithBorder(PrintElementThickness margin, PrintElementThickness padding, PrintElementThickness border, string borderBrush, string background, params dynamic[] blocks)
		{
			dynamic sectionItem = CreateSection(blocks);

			sectionItem.Section.Margin = new DynamicWrapper();
			sectionItem.Section.Margin.Left = margin.Left;
			sectionItem.Section.Margin.Top = margin.Top;
			sectionItem.Section.Margin.Right = margin.Right;
			sectionItem.Section.Margin.Bottom = margin.Bottom;
			sectionItem.Section.Margin.SizeUnit = "Px";

			sectionItem.Section.Padding = new DynamicWrapper();
			sectionItem.Section.Padding.Left = padding.Left;
			sectionItem.Section.Padding.Top = padding.Top;
			sectionItem.Section.Padding.Right = padding.Right;
			sectionItem.Section.Padding.Bottom = padding.Bottom;
			sectionItem.Section.Padding.SizeUnit = "Px";

			sectionItem.Section.Border = new DynamicWrapper();
			sectionItem.Section.Border.Thickness = new DynamicWrapper();
			sectionItem.Section.Border.Thickness.Left = border.Left;
			sectionItem.Section.Border.Thickness.Top = border.Top;
			sectionItem.Section.Border.Thickness.Right = border.Right;
			sectionItem.Section.Border.Thickness.Bottom = border.Bottom;
			sectionItem.Section.Border.Thickness.SizeUnit = "Px";
			sectionItem.Section.Border.Color = borderBrush;

			sectionItem.Section.Background = background;

			return sectionItem;
		}

		private static dynamic CreateSection(params dynamic[] blocks)
		{
			dynamic sectionItem = new DynamicWrapper();
			sectionItem.Section = new DynamicWrapper();
			sectionItem.Section.Blocks = new List<object>();

			if (blocks != null)
			{
				foreach (var block in blocks)
				{
					sectionItem.Section.Blocks.Add(block);
				}
			}

			return sectionItem;
		}

		private static dynamic CreateParagraphWithTextAlignment(string textAlignment, params dynamic[] inlines)
		{
			dynamic paragraphItem = CreateParagraph(inlines);
			paragraphItem.Paragraph.TextAlignment = textAlignment;

			return paragraphItem;
		}

		private static dynamic CreateParagraphWithTextCase(string textCase, params dynamic[] inlines)
		{
			dynamic paragraphItem = CreateParagraph(inlines);
			paragraphItem.Paragraph.TextCase = textCase;

			return paragraphItem;
		}

		private static dynamic CreateParagraph(params dynamic[] inlines)
		{
			dynamic paragraphItem = new DynamicWrapper();
			paragraphItem.Paragraph = new DynamicWrapper();
			paragraphItem.Paragraph.IndentSize = 10;
			paragraphItem.Paragraph.IndentSizeUnit = "Px";
			paragraphItem.Paragraph.Margin = new DynamicWrapper();
			paragraphItem.Paragraph.Margin.Top = 10;
			paragraphItem.Paragraph.Margin.Bottom = 10;
			paragraphItem.Paragraph.Inlines = new List<object>();

			if (inlines != null)
			{
				foreach (var inline in inlines)
				{
					paragraphItem.Paragraph.Inlines.Add(inline);
				}
			}

			return paragraphItem;
		}

		private static dynamic CreateLine()
		{
			dynamic lineItem = new DynamicWrapper();
			lineItem.Line = new DynamicWrapper();

			return lineItem;
		}

		private static dynamic CreatePageBreak()
		{
			dynamic pageBreakItem = new DynamicWrapper();
			pageBreakItem.PageBreak = new DynamicWrapper();

			return pageBreakItem;
		}

		private static dynamic CreateSpan(params dynamic[] inlines)
		{
			dynamic spanItem = new DynamicWrapper();
			spanItem.Span = new DynamicWrapper();
			spanItem.Span.Inlines = new List<object>();

			if (inlines != null)
			{
				foreach (var inline in inlines)
				{
					spanItem.Span.Inlines.Add(inline);
				}
			}

			return spanItem;
		}

		private static dynamic CreateBold(params dynamic[] inlines)
		{
			dynamic boldItem = new DynamicWrapper();
			boldItem.Bold = new DynamicWrapper();
			boldItem.Bold.Inlines = new List<object>();

			if (inlines != null)
			{
				foreach (var inline in inlines)
				{
					boldItem.Bold.Inlines.Add(inline);
				}
			}

			return boldItem;
		}

		private static dynamic CreateItalic(params dynamic[] inlines)
		{
			dynamic italicItem = new DynamicWrapper();
			italicItem.Italic = new DynamicWrapper();
			italicItem.Italic.Inlines = new List<object>();

			if (inlines != null)
			{
				foreach (var inline in inlines)
				{
					italicItem.Italic.Inlines.Add(inline);
				}
			}

			return italicItem;
		}

		private static dynamic CreateUnderline(params dynamic[] inlines)
		{
			dynamic underlineItem = new DynamicWrapper();
			underlineItem.Underline = new DynamicWrapper();
			underlineItem.Underline.Inlines = new List<object>();

			if (inlines != null)
			{
				foreach (var inline in inlines)
				{
					underlineItem.Underline.Inlines.Add(inline);
				}
			}

			return underlineItem;
		}

		private static dynamic CreateHyperlink(params dynamic[] inlines)
		{
			dynamic hyperlinkItem = new DynamicWrapper();
			hyperlinkItem.Hyperlink = new DynamicWrapper();
			hyperlinkItem.Hyperlink.Reference = "http://google.com";
			hyperlinkItem.Hyperlink.Inlines = new List<object>();

			if (inlines != null)
			{
				foreach (var inline in inlines)
				{
					hyperlinkItem.Hyperlink.Inlines.Add(inline);
				}
			}

			return hyperlinkItem;
		}

		private static dynamic CreateLineBreak()
		{
			dynamic lineBreakItem = new DynamicWrapper();
			lineBreakItem.LineBreak = new DynamicWrapper();

			return lineBreakItem;
		}

		private static dynamic CreateRun(string text)
		{
			dynamic runItem = new DynamicWrapper();
			runItem.Run = new DynamicWrapper();
			runItem.Run.Text = text;

			return runItem;
		}

		private static dynamic CreateRunWithFont(string text, string family = "Arial", double size = 10, string sizeUnit = "pt", string style = null, string stretch = null, string weight = null, string variant = null)
		{
			dynamic font = new DynamicWrapper();
			font.Family = family;
			font.Size = size;
			font.SizeUnit = sizeUnit;
			font.Style = style;
			font.Stretch = stretch;
			font.Weight = weight;
			font.Variant = variant;

			var runItem = CreateRun(text);
			runItem.Run.Font = font;

			return runItem;
		}

		private static dynamic CreateRunWithTextDecoration(string text, string textDecoration)
		{
			var runItem = CreateRun(text);
			runItem.Run.TextDecoration = textDecoration;

			return runItem;
		}

		private static dynamic CreateRunWithColor(string text, string foreground, string background)
		{
			var runItem = CreateRun(text);
			runItem.Run.Foreground = foreground;
			runItem.Run.Background = background;

			return runItem;
		}

		private static dynamic CreateImage(string rotation)
		{
			dynamic imageItem = new DynamicWrapper();
			imageItem.Image = new DynamicWrapper();
			imageItem.Image.Size = new DynamicWrapper();
			imageItem.Image.Size.Width = Resources.Image.Width / 3;
			imageItem.Image.Size.Height = Resources.Image.Height / 3;
			imageItem.Image.Size.SizeUnit = "Px";
			imageItem.Image.Data = ImageTestHelper.BitmapToBase64(Resources.Image);
			imageItem.Image.Rotation = rotation;

			return imageItem;
		}

		private static dynamic CreateBarcodeEan13(string rotation)
		{
			dynamic barcodeEan13Item = new DynamicWrapper();
			barcodeEan13Item.BarcodeEan13 = new DynamicWrapper();
			barcodeEan13Item.BarcodeEan13.Text = "123456789012";
			barcodeEan13Item.BarcodeEan13.Rotation = rotation;

			return barcodeEan13Item;
		}

		private static dynamic CreateBarcodeQr(string rotation)
		{
			dynamic barcodeQrItem = new DynamicWrapper();
			barcodeQrItem.BarcodeQr = new DynamicWrapper();
			barcodeQrItem.BarcodeQr.ShowText = false;
			barcodeQrItem.BarcodeQr.Text = "http://google.com";
			barcodeQrItem.BarcodeQr.Rotation = rotation;

			return barcodeQrItem;
		}
	}
}