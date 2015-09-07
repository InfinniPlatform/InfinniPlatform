using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;
using InfinniPlatform.FlowDocument.Model.Font;
using InfinniPlatform.FlowDocument.Model.Inlines;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Blocks
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PrintElementParagraphFactoryTest
	{
		[Test]
		public void ShouldBuildInlines()
		{
			// Given

			dynamic inline1 = new DynamicWrapper();
			inline1.Run = new DynamicWrapper();
			inline1.Run.Text = "Inline1";

			dynamic inline2 = new DynamicWrapper();
			inline2.Run = new DynamicWrapper();
			inline2.Run.Text = "Inline2";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Inlines = new[] { inline1, inline2 };

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph(elementMetadata);

			// Then
			Assert.IsNotNull(element);
			Assert.IsNotNull(element.Inlines);
			Assert.AreEqual(2, element.Inlines.Count);
			Assert.IsInstanceOf<PrintElementRun>(element.Inlines.First());
            Assert.IsInstanceOf<PrintElementRun>(element.Inlines.Last());
            Assert.AreEqual("Inline1", ((PrintElementRun)element.Inlines.First()).Text);
            Assert.AreEqual("Inline2", ((PrintElementRun)element.Inlines.Last()).Text);
		}

		[Test]
		public void ShouldApplyIndentSize()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.IndentSize = 10;
			elementMetadata.IndentSizeUnit = "Px";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph(elementMetadata);

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(10, element.IndentSize);
		}


		[Test]
		public void ShouldApplyFont()
		{
			// Given

			dynamic font = new DynamicWrapper();
			font.Family = "Arial";
			font.Size = 12;
			font.SizeUnit = "Px";
			font.Style = "Italic";
			font.Stretch = "UltraExpanded";
			font.Weight = "Bold";
			font.Variant = "Subscript";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Font = font;

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata);

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual("Arial", element.Font.Family);
			Assert.AreEqual(12, element.Font.Size);
			Assert.AreEqual(PrintElementFontStyle.Italic, element.Font.Style);
			Assert.AreEqual(PrintElementFontStretch.UltraExpanded, element.Font.Stretch);
			Assert.AreEqual(PrintElementFontWeight.Bold, element.Font.Weight);
			Assert.AreEqual(PrintElementFontVariant.Subscript, element.Font.Variant);
		}

		[Test]
		public void ShouldApplyFontFromStyle()
		{
			// Given

			dynamic style = new DynamicWrapper();
			style.Name = "style1";
			style.Font = new DynamicWrapper();
			style.Font.Family = "Arial";
			style.Font.Size = 12;
			style.Font.SizeUnit = "Px";
			style.Font.Style = "Italic";
			style.Font.Stretch = "UltraExpanded";
			style.Font.Weight = "Bold";
			style.Font.Variant = "Subscript";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Style = "style1";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata, c => { c.PrintViewStyles = new Dictionary<string, object> { { "style1", style } }; });

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual("Arial", element.Font.Family);
			Assert.AreEqual(12, element.Font.Size);
			Assert.AreEqual(PrintElementFontStyle.Italic, element.Font.Style);
			Assert.AreEqual(PrintElementFontStretch.UltraExpanded, element.Font.Stretch);
			Assert.AreEqual(PrintElementFontWeight.Bold, element.Font.Weight);
			Assert.AreEqual(PrintElementFontVariant.Subscript, element.Font.Variant);
		}


		[Test]
		public void ShouldApplyForeground()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Foreground = "Red";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata);

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(PrintElementColors.Red, element.Foreground);
		}

		[Test]
		public void ShouldApplyForegroundFromStyle()
		{
			// Given

			dynamic style = new DynamicWrapper();
			style.Name = "style1";
			style.Foreground = "Red";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Style = "style1";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata, c => { c.PrintViewStyles = new Dictionary<string, object> { { "style1", style } }; });

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(PrintElementColors.Red, element.Foreground);
		}


		[Test]
		public void ShouldApplyBackground()
		{
			// Given
			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Background = "Green";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata);

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(PrintElementColors.Green, element.Background);
		}

		[Test]
		public void ShouldApplyBackgroundFromStyle()
		{
			// Given

			dynamic style = new DynamicWrapper();
			style.Name = "style1";
			style.Background = "Green";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Style = "style1";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata, c => { c.PrintViewStyles = new Dictionary<string, object> { { "style1", style } }; });

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(PrintElementColors.Green, element.Background);
		}


		[Test]
		public void ShouldApplyTestCase()
		{
			// Given

			dynamic inline1 = new DynamicWrapper();
			inline1.Run = new DynamicWrapper();
			inline1.Run.Text = "Inline1";

			dynamic inline2 = new DynamicWrapper();
			inline2.Run = new DynamicWrapper();
			inline2.Run.Text = "Inline2";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Inlines = new[] { inline1, inline2 };
			elementMetadata.TextCase = "Uppercase";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata);

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(2, element.Inlines.Count);
			Assert.AreEqual("INLINE1", ((PrintElementRun)element.Inlines.First()).Text);
            Assert.AreEqual("INLINE2", ((PrintElementRun)element.Inlines.Last()).Text);
		}

		[Test]
		public void ShouldApplyTestCaseFromStyle()
		{
			// Given

			dynamic style = new DynamicWrapper();
			style.Name = "style1";
			style.TextCase = "Uppercase";

			dynamic inline1 = new DynamicWrapper();
			inline1.Run = new DynamicWrapper();
			inline1.Run.Text = "Inline1";

			dynamic inline2 = new DynamicWrapper();
			inline2.Run = new DynamicWrapper();
			inline2.Run.Text = "Inline2";

			dynamic elementMetadata = new DynamicWrapper();
			elementMetadata.Inlines = new[] { inline1, inline2 };
			elementMetadata.Style = "style1";

			// When
			PrintElementParagraph element = BuildTestHelper.BuildParagraph((object)elementMetadata, c => { c.PrintViewStyles = new Dictionary<string, object> { { "style1", style } }; });

			// Then
			Assert.IsNotNull(element);
			Assert.AreEqual(2, element.Inlines.Count);
            Assert.AreEqual("INLINE1", ((PrintElementRun)element.Inlines.First()).Text);
            Assert.AreEqual("INLINE2", ((PrintElementRun)element.Inlines.Last()).Text);
		}
	}
}