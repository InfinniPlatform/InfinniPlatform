using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;
using InfinniPlatform.PrintView.Tests.Properties;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Inline
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintImageFactoryTest
    {
        [Test]
        public void ShouldApplyImageData()
        {
            // Given

            var expectedImage = Resources.ImageRotate0;

            var template = new PrintImage { Data = TestHelper.BitmapToBytes(expectedImage) };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.IsNull(element.Size);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyImageDataFromSource()
        {
            // Given

            var expectedImage = Resources.ImageRotate0;

            var dataSource = TestHelper.BitmapToBytes(expectedImage);

            var template = new PrintImage { Source = "$" };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template, dataSource);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.IsNull(element.Size);
            TestHelper.AssertImagesAreEqual(expectedImage, element);
        }

        [Test]
        public void ShouldApplyRotation0()
        {
            // Given

            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate0;

            var template = new PrintImage
                           {
                               Data = TestHelper.BitmapToBytes(original),
                               Rotation = PrintImageRotation.Rotate0
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.IsNull(element.Size);
            TestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        public void ShouldApplyRotation90()
        {
            // Given

            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate90;

            var template = new PrintImage
                           {
                               Data = TestHelper.BitmapToBytes(original),
                               Rotation = PrintImageRotation.Rotate90
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.IsNull(element.Size);
            TestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        public void ShouldApplyRotation180()
        {
            // Given

            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate180;

            var template = new PrintImage
                           {
                               Data = TestHelper.BitmapToBytes(original),
                               Rotation = PrintImageRotation.Rotate180
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.IsNull(element.Size);
            TestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        public void ShouldApplyRotation270()
        {
            // Given

            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate270;

            var template = new PrintImage
                           {
                               Data = TestHelper.BitmapToBytes(original),
                               Rotation = PrintImageRotation.Rotate270
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.IsNull(element.Size);
            TestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        public void ShouldApplySize()
        {
            // Given

            var image = Resources.ImageRotate0;

            var template = new PrintImage
                           {
                               Size = new PrintSize
                                      {
                                          Width = 2 * image.Width,
                                          Height = 2 * image.Height,
                                          SizeUnit = PrintSizeUnit.Px
                                      },
                               Data = TestHelper.BitmapToBytes(image)
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.AreEqual(2*image.Width, element.Size.Width, 0.1);
            Assert.AreEqual(2*image.Height, element.Size.Height, 0.1);
        }

        [Test]
        public void ShouldApplyStretch()
        {
            // Given

            var image = Resources.ImageRotate0;

            var template = new PrintImage
                           {
                               Size = new PrintSize
                                      {
                                          Width = 2 * image.Width,
                                          Height = 2 * image.Height,
                                          SizeUnit = PrintSizeUnit.Px
                                      },
                               Stretch = PrintImageStretch.Fill,
                               Data = TestHelper.BitmapToBytes(image)
                           };

            // When
            var element = BuildTestHelper.BuildElement<PrintImage>(template);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Data);
            Assert.AreEqual(2*image.Width, element.Size.Width, 0.1);
            Assert.AreEqual(2*image.Height, element.Size.Height, 0.1);
            Assert.AreEqual(PrintImageStretch.Fill, element.Stretch);
        }
    }
}