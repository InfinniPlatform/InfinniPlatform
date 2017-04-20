using System;

using InfinniPlatform.PrintView.Defaults;
using InfinniPlatform.PrintView.Format;
using InfinniPlatform.PrintView.Inline;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintRunFactoryTest
    {
        [Test]
        public void ShouldAppyText()
        {
            // Given
            var template = new PrintRun { Text = "Some Text" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.Text, element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenSimple()
        {
            // Given
            const string dataSource = "Some Text";
            var template = new PrintRun { Source = "$" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource, element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenObject()
        {
            // Given
            var dataSource = new { Property1 = "Some Text" };
            var template = new PrintRun { Source = "$.Property1" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource.Property1, element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenSimpleCollection()
        {
            // Given
            var dataSource = new[] { "Some Text" };
            var template = new PrintRun { Source = "$.0" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0], element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenObjectCollection()
        {
            // Given
            var dataSource = new[] { new { Property1 = "Some Text" } };
            var template = new PrintRun { Source = "$.0.Property1" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0].Property1, element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenCollectionCollection()
        {
            // Given
            var dataSource = new[] { "Text1", "Text2" };
            var template = new PrintRun { Source = "$" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0] + "; " + dataSource[1], element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenSimple()
        {
            // Given
            const string dataSource = "Some Text";
            var template = new PrintRun { Source = "" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, initContext: c => { c.ElementSourceValue = dataSource; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource, element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenObject()
        {
            // Given
            var dataSource = new { Property1 = "Some Text" };
            var template = new PrintRun { Source = "Property1" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, initContext: c => { c.ElementSourceValue = dataSource; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource.Property1, element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenSimpleCollection()
        {
            // Given
            var dataSource = new[] { "Some Text" };
            var template = new PrintRun { Source = "0" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, initContext: c => { c.ElementSourceValue = dataSource; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0], element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenObjectCollection()
        {
            // Given
            var dataSource = new[] { new { Property1 = "Some Text" } };
            var template = new PrintRun { Source = "0.Property1" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, initContext: c => { c.ElementSourceValue = dataSource; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0].Property1, element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenCollectionCollection()
        {
            // Given
            var dataSource = new[] { "Text1", "Text2" };
            var template = new PrintRun { Source = "" };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, initContext: c => { c.ElementSourceValue = dataSource; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0] + "; " + dataSource[1], element.Text);
        }

        [Test]
        public void ShouldBuildWhenVisibilityNever()
        {
            // Given

            const string dataSource = "Some Text";

            var template = new PrintRun
                           {
                               Text = "",
                               Source = "$",
                               Visibility = PrintVisibility.Never
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNull(element);
        }

        [Test]
        public void ShouldBuildWhenVisibilityAlways()
        {
            // Given

            object dataSource = null;

            var template = new PrintRun
                           {
                               Text = "",
                               Source = "$",
                               Visibility = PrintVisibility.Always
                           };

            // When

            // ReSharper disable ExpressionIsAlwaysNull
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);
            // ReSharper restore ExpressionIsAlwaysNull

            // Then
            Assert.IsNotNull(element);
            Assert.True(string.IsNullOrEmpty(element.Text));
        }

        [Test]
        public void ShouldBuildWhenVisibilitySource()
        {
            // Given

            object dataSource = null;

            var template = new PrintRun
                           {
                               Text = "",
                               Source = "$",
                               Visibility = PrintVisibility.Source
                           };

            // When

            // ReSharper disable ExpressionIsAlwaysNull
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);
            // ReSharper restore ExpressionIsAlwaysNull

            // Then
            Assert.IsNull(element);
        }

        [Test]
        public void ShouldApplyFont()
        {
            // Given

            var font = new PrintFont
                       {
                           Family = "Arial",
                           Size = 12,
                           SizeUnit = PrintSizeUnit.Pt,
                           Style = PrintFontStyle.Italic,
                           Stretch = PrintFontStretch.UltraExpanded,
                           Weight = PrintFontWeight.Bold,
                           Variant = PrintFontVariant.Subscript
                       };

            var template = new PrintRun
                           {
                               Font = font,
                               Text = "Some Text"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Font);
            Assert.AreEqual(font.Family, element.Font.Family);
            Assert.AreEqual(font.Size, element.Font.Size);
            Assert.AreEqual(font.SizeUnit, element.Font.SizeUnit);
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
                           SizeUnit = PrintSizeUnit.Pt,
                           Style = PrintFontStyle.Italic,
                           Stretch = PrintFontStretch.UltraExpanded,
                           Weight = PrintFontWeight.Bold,
                           Variant = PrintFontVariant.Subscript
                       };

            var style = new PrintStyle
                        {
                            Name = "Style1",
                            Font = font
                        };

            var template = new PrintRun
                           {
                               Style = style.Name,
                               Text = "Some Text"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Font);
            Assert.AreEqual(font.Family, element.Font.Family);
            Assert.AreEqual(font.Size, element.Font.Size);
            Assert.AreEqual(font.SizeUnit, element.Font.SizeUnit);
            Assert.AreEqual(font.Style, element.Font.Style);
            Assert.AreEqual(font.Stretch, element.Font.Stretch);
            Assert.AreEqual(font.Weight, element.Font.Weight);
            Assert.AreEqual(font.Variant, element.Font.Variant);
        }

        [Test]
        public void ShouldApplyForeground()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "Some Text",
                               Foreground = PrintViewDefaults.Colors.Red
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.Foreground, element.Foreground);
        }

        [Test]
        public void ShouldApplyForegroundFromStyle()
        {
            // Given

            var style = new PrintStyle
                        {
                            Name = "Style1",
                            Foreground = PrintViewDefaults.Colors.Red
                        };

            var template = new PrintRun
                           {
                               Style = style.Name,
                               Text = "Some Text"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(style.Foreground, element.Foreground);
        }

        [Test]
        public void ShouldApplyBackground()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "Some Text",
                               Background = PrintViewDefaults.Colors.Green
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.Background, element.Background);
        }

        [Test]
        public void ShouldApplyBackgroundFromStyle()
        {
            // Given

            var style = new PrintStyle
                        {
                            Name = "style1",
                            Background = "Green"
                        };

            var template = new PrintRun
                           {
                               Style = style.Name,
                               Text = "Some Text"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(style.Background, element.Background);
        }

        [Test]
        public void ShouldApplyTextDecoration()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "Some Text",
                               TextDecoration = PrintTextDecoration.Underline
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(template.TextDecoration, element.TextDecoration);
        }

        [Test]
        public void ShouldApplyTextDecorationFromStyle()
        {
            // Given

            var style = new PrintStyle
                        {
                            Name = "Style1",
                            TextDecoration = PrintTextDecoration.Underline
                        };

            var template = new PrintRun
                           {
                               Style = style.Name,
                               Text = "Some Text"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(style.TextDecoration, element.TextDecoration);
        }

        [Test]
        public void ShouldApplyTestCaseWhenNormal()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "Some Text",
                               TextCase = PrintTextCase.Normal
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenSentenceCase()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "some text",
                               TextCase = PrintTextCase.SentenceCase
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some text", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenLowercase()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "SOME TEXT",
                               TextCase = PrintTextCase.Lowercase
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("some text", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenUppercase()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "some text",
                               TextCase = PrintTextCase.Uppercase
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("SOME TEXT", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenToggleCase()
        {
            // Given
            var template = new PrintRun
                           {
                               Text = "SoMe TeXt",
                               TextCase = PrintTextCase.ToggleCase
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("sOmE tExT", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseFromStyle()
        {
            // Given

            var style = new PrintStyle
                        {
                            Name = "Style1",
                            TextCase = PrintTextCase.SentenceCase
                        };

            var template = new PrintRun
                           {
                               Style = style.Name,
                               Text = "some text"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, styles: new[] { style });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some text", element.Text);
        }

        [Test]
        public void ShouldApplySourceFormatForObject()
        {
            // Given

            var dataSource = DateTime.Now;

            var sourceFormat = new DateTimeFormat { Format = "yyyy'.'MM'.'dd" };

            var template = new PrintRun
                           {
                               Source = "$",
                               SourceFormat = sourceFormat
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource.ToString(sourceFormat.Format), element.Text);
        }

        [Test]
        public void ShouldApplySourceFormatForCollection()
        {
            // Given

            var dataSource = new[] { DateTime.Now.AddDays(0), DateTime.Now.AddDays(2) };

            var sourceFormat = new DateTimeFormat { Format = "yyyy'.'MM'.'dd" };

            var template = new PrintRun
                           {
                               Source = "$",
                               SourceFormat = sourceFormat
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintRun>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(dataSource[0].ToString(sourceFormat.Format) + "; " + dataSource[1].ToString(sourceFormat.Format), element.Text);
        }
    }
}