using System;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.FlowDocument.Tests.Properties;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementBarcodeQrFactoryTest
    {
        private const string BarcodeQrText = "123456789012";

        [Test]
        [STAThread]
        public void ShouldApplyText()
        {
            // Given
            var expectedImage = Resources.BarcodeQrRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyTextFromSource()
        {
            // Given
            var expectedImage = Resources.BarcodeQrRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";
            elementMetadata.ShowText = false;

            // When
            var element = BuildTestHelper.BuildBarcodeQr((object) elementMetadata,
                c => { c.PrintViewSource = BarcodeQrText; });

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation0()
        {
            // Given
            var expectedImage = Resources.BarcodeQrRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate0";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation90()
        {
            // Given
            var expectedImage = Resources.BarcodeQrRotate90;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate90";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation180()
        {
            // Given
            var expectedImage = Resources.BarcodeQrRotate180;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate180";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation270()
        {
            // Given
            var expectedImage = Resources.BarcodeQrRotate270;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.Rotation = "Rotate270";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyErrorCorrectionLow()
        {
            // Given
            var expectedImage = Resources.BarcodeQrErrorCorrectionLow;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.ErrorCorrection = "Low";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyErrorCorrectionMedium()
        {
            // Given
            var expectedImage = Resources.BarcodeQrErrorCorrectionMedium;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.ErrorCorrection = "Medium";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyErrorCorrectionQuartile()
        {
            // Given
            var expectedImage = Resources.BarcodeQrErrorCorrectionQuartile;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.ErrorCorrection = "Quartile";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyErrorCorrectionHigh()
        {
            // Given
            var expectedImage = Resources.BarcodeQrErrorCorrectionHigh;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Text = BarcodeQrText;
            elementMetadata.ShowText = false;
            elementMetadata.ErrorCorrection = "High";

            // When
            PrintElementImage element = BuildTestHelper.BuildBarcodeQr(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(expectedImage, element);
        }
    }
}