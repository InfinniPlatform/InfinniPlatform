using System;
using InfinniPlatform.Sdk.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.DisplayFormats
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class NumberFormatFactoryTest
    {
        [Test]
        public void ShouldReturnNullWhenNotNumberValue()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();

            // When
            Func<object, string> format = BuildTestHelper.BuildNumberFormat(elementMetadata);
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
            const double value = 12345.6789;
            dynamic elementMetadata = new DynamicWrapper();

            // When
            Func<object, string> format = BuildTestHelper.BuildNumberFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString("n"), result);
        }

        [Test]
        public void ShouldFormatWithSettings()
        {
            // Given
            const double value = 12345.6789;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "n3";

            // When
            Func<object, string> format = BuildTestHelper.BuildNumberFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString("n3"), result);
        }
    }
}