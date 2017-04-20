using System;

using InfinniPlatform.PrintView.Format;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Factories.Format
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class BooleanFormatFactoryTest
    {
        [Test]
        public void ShouldReturnNullWhenNotBooleanValue()
        {
            // Given
            var template = new BooleanFormat();

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
            var template = new BooleanFormat();

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
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
            var template = new BooleanFormat { TrueText = "Yes", FalseText = "No" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var trueString = format(true);
            var falseString = format(false);

            // Then
            Assert.AreEqual(template.TrueText, trueString);
            Assert.AreEqual(template.FalseText, falseString);
        }
    }
}