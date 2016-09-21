using System;

using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.DisplayFormats
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class BooleanFormatFactoryTest
    {
        [Test]
        public void ShouldReturnNullWhenNotBooleanValue()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            Func<object, string> format = BuildTestHelper.BuildBooleanFormat(elementMetadata);
            var result1 = format(null);
            var result2 = format(new object());

            // Then
            Assert.IsNull(result1);
            Assert.IsNull(result2);
        }

        [Test]
        public void ShouldFormatWithDefaultSettings()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            Func<object, string> format = BuildTestHelper.BuildBooleanFormat(elementMetadata);
            var trueString = format(true);
            var falseString = format(false);

            // Then
            Assert.AreEqual(bool.TrueString, trueString);
            Assert.AreEqual(bool.FalseString, falseString);
        }

        [Test]
        public void ShouldFormatWithSettings()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.TrueText = "Yes";
            elementMetadata.FalseText = "No";

            // When
            Func<object, string> format = BuildTestHelper.BuildBooleanFormat(elementMetadata);
            var trueString = format(true);
            var falseString = format(false);

            // Then
            Assert.AreEqual("Yes", trueString);
            Assert.AreEqual("No", falseString);
        }
    }
}