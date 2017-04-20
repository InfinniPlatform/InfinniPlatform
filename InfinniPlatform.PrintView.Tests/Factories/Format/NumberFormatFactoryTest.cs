using System;

using InfinniPlatform.PrintView.Format;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Factories.Format
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class NumberFormatFactoryTest
    {
        [Test]
        public void ShouldReturnNullWhenNotNumberValue()
        {
            // Given
            var template = new NumberFormat();

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
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
            var template = new NumberFormat();

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString("n"), result);
        }

        [Test]
        public void ShouldFormatWithSettings()
        {
            // Given
            const double value = 12345.6789;
            var template = new NumberFormat { Format = "n3" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString("n3"), result);
        }
    }
}