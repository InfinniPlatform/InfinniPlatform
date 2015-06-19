using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using InfinniPlatform.FlowDocument.Tests.Properties;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.Inlines
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class PrintElementImageFactoryTest
    {
        [Test]
        [STAThread]
        public void ShouldApplyImageData()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(image.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(image.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(image, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyImageDataFromSource()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Source = "$";

            // When
            var element = BuildTestHelper.BuildImage((object) elementMetadata,
                c => { c.PrintViewSource = ImageTestHelper.BitmapToBase64(image); });

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(image.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(image.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(image, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation0()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);
            elementMetadata.Rotation = "Rotate0";

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(Resources.ImageRotate0.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(Resources.ImageRotate0.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(Resources.ImageRotate0, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation90()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);
            elementMetadata.Rotation = "Rotate90";

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(Resources.ImageRotate90.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(Resources.ImageRotate90.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(Resources.ImageRotate90, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation180()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);
            elementMetadata.Rotation = "Rotate180";

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(Resources.ImageRotate180.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(Resources.ImageRotate180.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(Resources.ImageRotate180, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation270()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);
            elementMetadata.Rotation = "Rotate270";

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(Resources.ImageRotate270.Width, ((Image) element.Child).Width, 0.1);
            Assert.AreEqual(Resources.ImageRotate270.Height, ((Image) element.Child).Height, 0.1);
            ImageTestHelper.AssertImagesAreEqual(Resources.ImageRotate270, (Image) element.Child);
        }

        [Test]
        [STAThread]
        public void ShouldApplySize()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Size = new DynamicWrapper();
            elementMetadata.Size.Width = 2*image.Width;
            elementMetadata.Size.Height = 2*image.Height;
            elementMetadata.Size.SizeUnit = "Px";
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(2*image.Width, ((Image) element.Child).Width);
            Assert.AreEqual(2*image.Height, ((Image) element.Child).Height);
        }

        [Test]
        [STAThread]
        public void ShouldApplyStretch()
        {
            // Given
            var image = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Size = new DynamicWrapper();
            elementMetadata.Size.Width = 2*image.Width;
            elementMetadata.Size.Height = 2*image.Height;
            elementMetadata.Size.SizeUnit = "Px";
            elementMetadata.Stretch = "Fill";
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(image);

            // When
            InlineUIContainer element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNotNull(element.Child);
            Assert.IsInstanceOf<Image>(element.Child);
            Assert.AreEqual(2*image.Width, ((Image) element.Child).Width);
            Assert.AreEqual(2*image.Height, ((Image) element.Child).Height);
            Assert.AreEqual(Stretch.Fill, ((Image) element.Child).Stretch);
        }
    }
}