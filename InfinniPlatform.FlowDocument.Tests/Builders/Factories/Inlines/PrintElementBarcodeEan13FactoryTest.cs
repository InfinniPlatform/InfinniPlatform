using System;
using System.Windows.Controls;
using System.Windows.Documents;
using InfinniPlatform.FlowDocument.Tests.Properties;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementBarcodeEan13FactoryTest
    {
        private const string BarcodeEan13Text = "123456789012";

        [Test]
        [STAThread]
        public void ShouldApplyText()
        {
            // Given
            var expectedImage = Resources.BarcodeEan13Rotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeEan13Text;
            elementMetadata.ShowText = false;

            // When
            InlineUIContainer element = BuildTestHelper.BuildBarcodeEan13(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(expectedImage.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(expectedImage.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyTextFromSource()
        {
            // Given
            var expectedImage = Resources.BarcodeEan13Rotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";
            elementMetadata.ShowText = false;

            // When
            var element = BuildTestHelper.BuildBarcodeEan13((object) elementMetadata,
                c => { c.PrintViewSource = BarcodeEan13Text; });

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(expectedImage.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(expectedImage.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation0()
        {
            // Given
            var expectedImage = Resources.BarcodeEan13Rotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeEan13Text;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate0";

            // When
            InlineUIContainer element = BuildTestHelper.BuildBarcodeEan13(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(expectedImage.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(expectedImage.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation90()
        {
            // Given
            var expectedImage = Resources.BarcodeEan13Rotate90;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeEan13Text;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate90";

            // When
            InlineUIContainer element = BuildTestHelper.BuildBarcodeEan13(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(expectedImage.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(expectedImage.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation180()
        {
            // Given
            var expectedImage = Resources.BarcodeEan13Rotate180;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeEan13Text;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate180";

            // When
            InlineUIContainer element = BuildTestHelper.BuildBarcodeEan13(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(expectedImage.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(expectedImage.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation270()
        {
            // Given
            var expectedImage = Resources.BarcodeEan13Rotate270;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeEan13Text;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate270";

            // When
            InlineUIContainer element = BuildTestHelper.BuildBarcodeEan13(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(expectedImage.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(expectedImage.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, (Image) element.Child);
        }
    }
}