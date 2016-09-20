﻿using System;
using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Inlines;
using InfinniPlatform.PrintView.Tests.Properties;
using InfinniPlatform.Sdk.Dynamic;
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
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsInstanceOf<PrintElementImage>(element);
            Assert.IsNull(element.Size);
            ImageTestHelper.AssertImagesAreEqual(image, element);
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
            Assert.IsNull(element.Size);
            ImageTestHelper.AssertImagesAreEqual(image, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation0()
        {
            // Given
            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate0;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(original);
            elementMetadata.Rotation = "Rotate0";

            // When
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNull(element.Size);
            ImageTestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation90()
        {
            // Given
            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate90;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(original);
            elementMetadata.Rotation = "Rotate90";

            // When
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNull(element.Size);
            ImageTestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation180()
        {
            // Given
            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate180;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(original);
            elementMetadata.Rotation = "Rotate180";

            // When
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNull(element.Size);
            ImageTestHelper.AssertImagesAreEqual(expected, element);
        }

        [Test]
        [STAThread]
        public void ShouldApplyRotation270()
        {
            // Given
            var original = Resources.ImageRotate0;
            var expected = Resources.ImageRotate270;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Data = ImageTestHelper.BitmapToBase64(original);
            elementMetadata.Rotation = "Rotate270";

            // When
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.IsNull(element.Size);
            ImageTestHelper.AssertImagesAreEqual(expected, element);
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
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(2*image.Width, element.Size.Width, 0.1);
            Assert.AreEqual(2*image.Height, element.Size.Height, 0.1);
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
            PrintElementImage element = BuildTestHelper.BuildImage(elementMetadata);

            // Then
            Assert.IsNotNull(element);
            Assert.AreEqual(2*image.Width, element.Size.Width, 0.1);
            Assert.AreEqual(2*image.Height, element.Size.Height, 0.1);
            Assert.AreEqual(PrintElementStretch.Fill, element.Stretch);
        }
    }
}