using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inline
{
    [TestFixture]
    [Platform(Exclude = "Mono")]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintBarcodeQrFactoryTest
    {
        private const string BarcodeQrText = "123456789012";

        [Test]
        public void ShouldApplyText()
        {
            // Given

            var expectedImage = Resources.BarcodeQrRotate0;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Text = BarcodeQrText
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyTextFromSource()
        {
            // Given

            var expectedImage = Resources.BarcodeQrRotate0;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Source = "$"
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template, BarcodeQrText);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation0()
        {
            // Given

            var expectedImage = Resources.BarcodeQrRotate0;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Text = BarcodeQrText,
                               Rotation = PrintImageRotation.Rotate0
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation90()
        {
            // Given

            var expectedImage = Resources.BarcodeQrRotate90;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Text = BarcodeQrText,
                               Rotation = PrintImageRotation.Rotate90
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation180()
        {
            // Given

            var expectedImage = Resources.BarcodeQrRotate180;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Text = BarcodeQrText,
                               Rotation = PrintImageRotation.Rotate180
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation270()
        {
            // Given

            var expectedImage = Resources.BarcodeQrRotate270;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Text = BarcodeQrText,
                               Rotation = PrintImageRotation.Rotate270
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyErrorCorrectionLow()
        {
            // Given

            var expectedImage = Resources.BarcodeQrErrorCorrectionLow;

            var template = new PrintBarcodeQr
                           {
                               ShowText = false,
                               Text = BarcodeQrText,
                               ErrorCorrection = PrintBarcodeQrErrorCorrection.Low
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyErrorCorrectionMedium()
        {
            // Given

            var expectedImage = Resources.BarcodeQrErrorCorrectionMedium;

            var template = new PrintBarcodeQr
                           {
                               Text = BarcodeQrText,
                               ShowText = false,
                               ErrorCorrection = PrintBarcodeQrErrorCorrection.Medium
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyErrorCorrectionQuartile()
        {
            // Given

            var expectedImage = Resources.BarcodeQrErrorCorrectionQuartile;

            var template = new PrintBarcodeQr
                           {
                               Text = BarcodeQrText,
                               ShowText = false,
                               ErrorCorrection = PrintBarcodeQrErrorCorrection.Quartile
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyErrorCorrectionHigh()
        {
            // Given

            var expectedImage = Resources.BarcodeQrErrorCorrectionHigh;

            var template = new PrintBarcodeQr
                           {
                               Text = BarcodeQrText,
                               ShowText = false,
                               ErrorCorrection = PrintBarcodeQrErrorCorrection.High
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(expectedImage.Width, element.Size.Width, 0.1);
            Assert.AreEqual(expectedImage.Height, element.Size.Height, 0.1);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }
    }
}