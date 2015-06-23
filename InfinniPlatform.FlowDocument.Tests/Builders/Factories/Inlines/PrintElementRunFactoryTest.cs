using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementRunFactoryTest
    {
        [Test]
        public void ShouldAppyText()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "Some Text";

            // When
            Run element = BuildTestHelper.BuildRun(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenSimple()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata, c => { c.PrintViewSource = "Some Text"; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenObject()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$.Property1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewSource = new {Property1 = "Some Text"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenSimpleCollection()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$.0";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewSource = new[] {"Some Text"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenObjectCollection()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$.0.Property1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewSource = new[] {new {Property1 = "Some Text"}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppySourceWhenCollectionCollection()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewSource = new[] {"Text1", "Text2"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Text1; Text2", element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenSimple()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.ElementSourceValue = "Some Text"; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenObject()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "Property1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.ElementSourceValue = new {Property1 = "Some Text"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenSimpleCollection()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "0";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.ElementSourceValue = new[] {"Some Text"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenObjectCollection()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "0.Property1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.ElementSourceValue = new[] {new {Property1 = "Some Text"}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldAppyRelativeSourceWhenCollectionCollection()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.ElementSourceValue = new[] {"Text1", "Text2"}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Text1; Text2", element.Text);
        }

        [Test]
        public void ShouldBuildWhenVisibilityNever()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "";
            elementMetadata.Source = "$";
            elementMetadata.Visibility = "Never";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata, c => { c.PrintViewSource = "Some Text"; });

            // Then
            Assert.IsNull(element);
        }

        [Test]
        public void ShouldBuildWhenVisibilityAlways()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "";
            elementMetadata.Source = "$";
            elementMetadata.Visibility = "Always";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata, c => { c.PrintViewSource = null; });

            // Then
            Assert.IsNotNull(element);
            Assert.IsNullOrEmpty(element.Text);
        }

        [Test]
        public void ShouldBuildWhenVisibilitySource()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "";
            elementMetadata.Source = "$";
            elementMetadata.Visibility = "Source";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata, c => { c.PrintViewSource = null; });

            // Then
            Assert.IsNull(element);
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
            elementMetadata.Text = "Some Text";
            elementMetadata.Font = font;

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

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
            elementMetadata.Text = "Some Text";
            elementMetadata.Style = "style1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
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
            elementMetadata.Text = "Some Text";
            elementMetadata.Foreground = "Red";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

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
            elementMetadata.Text = "Some Text";
            elementMetadata.Style = "style1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
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
            elementMetadata.Text = "Some Text";
            elementMetadata.Background = "Green";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

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
            elementMetadata.Text = "Some Text";
            elementMetadata.Style = "style1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(Brushes.Green, element.Background);
        }

        [Test]
        public void ShouldApplyTextDecoration()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "Some Text";
            elementMetadata.TextDecoration = "Underline";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(TextDecorations.Underline, element.TextDecorations);
        }

        [Test]
        public void ShouldApplyTextDecorationFromStyle()
        {
            // Given

            dynamic style = new DynamicWrapper();
            style.Name = "style1";
            style.TextDecoration = "Underline";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "Some Text";
            elementMetadata.Style = "style1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(TextDecorations.Underline, element.TextDecorations);
        }

        [Test]
        public void ShouldApplyTestCaseWhenNormal()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "Some Text";
            elementMetadata.TextCase = "Normal";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some Text", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenSentenceCase()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "some text";
            elementMetadata.TextCase = "SentenceCase";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some text", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenLowercase()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "SOME TEXT";
            elementMetadata.TextCase = "Lowercase";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("some text", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenUppercase()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "some text";
            elementMetadata.TextCase = "Uppercase";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("SOME TEXT", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseWhenToggleCase()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "SoMe TeXt";
            elementMetadata.TextCase = "ToggleCase";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("sOmE tExT", element.Text);
        }

        [Test]
        public void ShouldApplyTestCaseFromStyle()
        {
            // Given

            dynamic style = new DynamicWrapper();
            style.Name = "style1";
            style.TextCase = "SentenceCase";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = "some text";
            elementMetadata.Style = "style1";

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewStyles = new Dictionary<string, object> {{"style1", style}}; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("Some text", element.Text);
        }

        [Test]
        public void ShouldApplySourceFormatForObject()
        {
            // Given

            dynamic sourceFormat = new DynamicWrapper();
            sourceFormat.DateTimeFormat = new DynamicWrapper();
            sourceFormat.DateTimeFormat.Format = "yyyy'.'MM'.'dd";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";
            elementMetadata.SourceFormat = sourceFormat;

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c => { c.PrintViewSource = new DateTime(2014, 10, 15); });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("2014.10.15", element.Text);
        }

        [Test]
        public void ShouldApplySourceFormatForCollection()
        {
            // Given

            dynamic sourceFormat = new DynamicWrapper();
            sourceFormat.DateTimeFormat = new DynamicWrapper();
            sourceFormat.DateTimeFormat.Format = "yyyy'.'MM'.'dd";

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";
            elementMetadata.SourceFormat = sourceFormat;

            // When
            var element = BuildTestHelper.BuildRun((object) elementMetadata,
                c =>
                {
                    c.PrintViewSource = new[]
                    {new DateTime(2014, 10, 15), new DateTime(2014, 10, 16), new DateTime(2014, 10, 17)};
                });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual("2014.10.15; 2014.10.16; 2014.10.17", element.Text);
        }
    }
}