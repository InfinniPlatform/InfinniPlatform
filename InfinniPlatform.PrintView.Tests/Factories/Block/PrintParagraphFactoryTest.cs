using System.Linq;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;
using InfinniPlatform.PrintView.Model.Defaults;
using InfinniPlatform.PrintView.Model.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Block
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintParagraphFactoryTest
    {
        [Test]
        public void ShouldBuildInlines()
        {
            // Given

            var inline1 = new PrintRun { Text = "Inline1" };
            var inline2 = new PrintRun { Text = "Inline2" };

            var template = new PrintParagraph();
            template.Inlines.Add(inline1);
            template.Inlines.Add(inline2);

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Inlines);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.IsInstanceOf<PrintRun>(element.Inlines.First());
            Assert.IsInstanceOf<PrintRun>(element.Inlines.Last());
            Assert.AreEqual(inline1.Text, ((PrintRun)element.Inlines.First()).Text);
            Assert.AreEqual(inline2.Text, ((PrintRun)element.Inlines.Last()).Text);
        }

        [Test]
        public void ShouldApplyIndentSize()
        {
            // Given
            var template = new PrintParagraph { IndentSize = 10, IndentSizeUnit = PrintSizeUnit.Px };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.IndentSize, element.IndentSize);
            Assert.AreEqual(template.IndentSizeUnit, element.IndentSizeUnit);
        }

        [Test]
        public void ShouldApplyFont()
        {
            // Given

            var font = new PrintFont
            {
                Family = "Arial",
                Size = 12,
                SizeUnit = PrintSizeUnit.Px,
                Style = PrintFontStyle.Italic,
                Stretch = PrintFontStretch.UltraExpanded,
                Weight = PrintFontWeight.Bold,
                Variant = PrintFontVariant.Subscript
            };

            var template = new PrintParagraph { Font = font };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(font.Family, element.Font.Family);
            Assert.AreEqual(font.Size, element.Font.Size);
            Assert.AreEqual(font.Style, element.Font.Style);
            Assert.AreEqual(font.Stretch, element.Font.Stretch);
            Assert.AreEqual(font.Weight, element.Font.Weight);
            Assert.AreEqual(font.Variant, element.Font.Variant);
        }

        [Test]
        public void ShouldApplyFontFromStyle()
        {
            // Given

            var font = new PrintFont
            {
                Family = "Arial",
                Size = 12,
                SizeUnit = PrintSizeUnit.Px,
                Style = PrintFontStyle.Italic,
                Stretch = PrintFontStretch.UltraExpanded,
                Weight = PrintFontWeight.Bold,
                Variant = PrintFontVariant.Subscript
            };

            var style = new PrintStyle { Name = "Style1", Font = font };

            var template = new PrintParagraph { Style = style.Name };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(font.Family, element.Font.Family);
            Assert.AreEqual(font.Size, element.Font.Size);
            Assert.AreEqual(font.Style, element.Font.Style);
            Assert.AreEqual(font.Stretch, element.Font.Stretch);
            Assert.AreEqual(font.Weight, element.Font.Weight);
            Assert.AreEqual(font.Variant, element.Font.Variant);
        }

        [Test]
        public void ShouldApplyForeground()
        {
            // Given
            var template = new PrintParagraph { Foreground = PrintViewDefaults.Colors.Red };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.Foreground, element.Foreground);
        }

        [Test]
        public void ShouldApplyForegroundFromStyle()
        {
            // Given
            var style = new PrintStyle { Name = "Style1", Foreground = PrintViewDefaults.Colors.Red };
            var template = new PrintParagraph { Style = style.Name };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(style.Foreground, element.Foreground);
        }

        [Test]
        public void ShouldApplyBackground()
        {
            // Given
            var template = new PrintParagraph { Background = PrintViewDefaults.Colors.Green };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.Background, element.Background);
        }

        [Test]
        public void ShouldApplyBackgroundFromStyle()
        {
            // Given
            var style = new PrintStyle { Name = "Style1", Background = PrintViewDefaults.Colors.Green };
            var template = new PrintParagraph { Style = style.Name };

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(style.Background, element.Background);
        }

        [Test]
        public void ShouldApplyTestCase()
        {
            // Given

            var inline1 = new PrintRun { Text = "Inline1" };
            var inline2 = new PrintRun { Text = "Inline2" };

            var template = new PrintParagraph { TextCase = PrintTextCase.Uppercase };
            template.Inlines.Add(inline1);
            template.Inlines.Add(inline2);

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.AreEqual(inline1.Text.ToUpper(), ((PrintRun)element.Inlines.First()).Text);
            Assert.AreEqual(inline2.Text.ToUpper(), ((PrintRun)element.Inlines.Last()).Text);
        }

        [Test]
        public void ShouldApplyTestCaseFromStyle()
        {
            // Given

            var style = new PrintStyle { Name = "Style1", TextCase = PrintTextCase.Uppercase };

            var inline1 = new PrintRun { Text = "Inline1" };
            var inline2 = new PrintRun { Text = "Inline2" };

            var template = new PrintParagraph { Style = style.Name };
            template.Inlines.Add(inline1);
            template.Inlines.Add(inline2);

            // When
            var element = BuildTestHelper.BuildElement<PrintParagraph>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(2, element.Inlines.Count);
            Assert.AreEqual(inline1.Text.ToUpper(), ((PrintRun)element.Inlines.First()).Text);
            Assert.AreEqual(inline2.Text.ToUpper(), ((PrintRun)element.Inlines.Last()).Text);
        }
    }
}