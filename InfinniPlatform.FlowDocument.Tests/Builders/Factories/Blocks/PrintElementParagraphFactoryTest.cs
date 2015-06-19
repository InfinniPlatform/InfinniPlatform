using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using InfinniPlatform.Sdk.Application.Dynamic;
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
            elementMetadata.Inlines = new[] {inline1, inline2};

            // When
            Paragraph element = BuildTestHelper.BuildParagraph(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.IsInstanceOf<Run>(element.Inlines.FirstInline);
            Assert.IsInstanceOf<Run>(element.Inlines.LastInline);
            Assert.AreEqual("Inline1", ((Run) element.Inlines.FirstInline).Text);
            Assert.AreEqual("Inline2", ((Run) element.Inlines.LastInline).Text);
        }

        [Test]
        public void ShouldApplyIndentSize()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.IndentSize = 10;
            elementMetadata.IndentSizeUnit = "Px";

            // When
            Paragraph element = BuildTestHelper.BuildParagraph(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(10, element.TextIndent);
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
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Arial", element.FontFamily.FamilyNames.First().Value);
            Assert.AreEqual(12, element.FontSize);
            Assert.AreEqual(FontStyles.Italic, element.FontStyle);
            Assert.AreEqual(FontStretches.UltraExpanded, element.FontStretch);
            Assert.AreEqual(FontWeights.Bold, element.FontWeight);
            Assert.AreEqual(FontVariants.Subscript, element.Typography.Variants);
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
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Arial", element.FontFamily.FamilyNames.First().Value);
            Assert.AreEqual(12, element.FontSize);
            Assert.AreEqual(FontStyles.Italic, element.FontStyle);
            Assert.AreEqual(FontStretches.UltraExpanded, element.FontStretch);
            Assert.AreEqual(FontWeights.Bold, element.FontWeight);
            Assert.AreEqual(FontVariants.Subscript, element.Typography.Variants);
        }

        [Test]
        public void ShouldApplyForeground()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Foreground = "Red";

            // When
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(Brushes.Red, element.Foreground);
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
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(Brushes.Red, element.Foreground);
        }

        [Test]
        public void ShouldApplyBackground()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Background = "Green";

            // When
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(Brushes.Green, element.Background);
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
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(Brushes.Green, element.Background);
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
            elementMetadata.Inlines = new[] {inline1, inline2};
            elementMetadata.TextCase = "Uppercase";

            // When
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.AreEqual("INLINE1", ((Run) element.Inlines.FirstInline).Text);
            Assert.AreEqual("INLINE2", ((Run) element.Inlines.LastInline).Text);
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
            elementMetadata.Inlines = new[] {inline1, inline2};
            elementMetadata.Style = "style1";

            // When
            var element = BuildTestHelper.BuildParagraph((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.AreEqual("INLINE1", ((Run) element.Inlines.FirstInline).Text);
            Assert.AreEqual("INLINE2", ((Run) element.Inlines.LastInline).Text);
        }
    }
}