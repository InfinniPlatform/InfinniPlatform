using System;

using InfinniPlatform.PrintView.Model.Format;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Format
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class DateTimeFormatFactoryTest
    {
        [Test]
        public void ShouldReturnNullWhenNotDateTimeValue()
        {
            // Given
            var template = new DateTimeFormat();

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
            var value = DateTime.Now;
            var template = new DateTimeFormat();

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString("G"), result);
        }

        [Test]
        public void ShouldFormatWithSettings()
        {
            // Given
            var value = new DateTime(2014, 10, 15);
            var template = new DateTimeFormat { Format = "yyyy'.'MM'.'dd" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual("2014.10.15", result);
        }
    }
}