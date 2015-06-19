using System;
using InfinniPlatform.Sdk.Application.Dynamic;
using NUnit.Framework;

namespace InfinniPlatform.FlowDocument.Tests.Builders.Factories.DisplayFormats
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ObjectFormatFactoryTest
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
        public void ShouldReturnNullWhenFormatStringIsNull()
        {
            // Given
            var value = new object();
            dynamic elementMetadata = new DynamicWrapper();

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString(), result);
        }

        [Test]
        public void ShouldReturnFormatStringWhenNoPlaceholders()
        {
            // Given
            var value = new object();
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "Some format string";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual("Some format string", result);
        }

        [Test]
        public void ShouldReturnNullWhenValueIsNull()
        {
            // Given
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "{}";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(null);

            // Then
            Assert.AreEqual("", result);
        }

        [Test]
        public void ShouldFomatString()
        {
            // Given
            const string value = "Value1";
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "Value: {}!";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual(string.Format("Value: {0}!", value), result);
        }

        [Test]
        [TestCase("N")]
        [TestCase("N0")]
        [TestCase("N2")]
        [TestCase("N2")]
        [TestCase("N3")]
        [TestCase("N4")]
        [TestCase("N5")]
        public void ShouldFomatNumber(string numberFormat)
        {
            // Given
            const double value = 1234.567;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "{:" + numberFormat + "}";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            var expectedFormat = "{0:" + numberFormat + "}";
            Assert.AreEqual(string.Format(expectedFormat, value), result);
        }

        [Test]
        [TestCase("P")]
        [TestCase("P0")]
        [TestCase("P2")]
        [TestCase("P2")]
        [TestCase("P3")]
        [TestCase("P4")]
        [TestCase("P5")]
        public void ShouldFomatPercent(string numberFormat)
        {
            // Given
            const double value = 1234.567;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "{:" + numberFormat + "}";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            var expectedFormat = "{0:" + numberFormat + "}";
            Assert.AreEqual(string.Format(expectedFormat, value), result);
        }

        [Test]
        [TestCase("C")]
        [TestCase("C0")]
        [TestCase("C2")]
        [TestCase("C2")]
        [TestCase("C3")]
        [TestCase("C4")]
        [TestCase("C5")]
        public void ShouldFomatCurrency(string numberFormat)
        {
            // Given
            const double value = 1234.567;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "{:" + numberFormat + "}";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            var expectedFormat = "{0:" + numberFormat + "}";
            Assert.AreEqual(string.Format(expectedFormat, value), result);
        }

        [Test]
        [TestCase("D")]
        [TestCase("d")]
        [TestCase("T")]
        [TestCase("t")]
        public void ShouldFomatDateTime(string dateTimeFormat)
        {
            // Given
            var value = DateTime.Now;
            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format = "{:" + dateTimeFormat + "}";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            var expectedFormat = "{0:" + dateTimeFormat + "}";
            Assert.AreEqual(string.Format(expectedFormat, value), result);
        }

        [Test]
        public void ShouldFormatObject()
        {
            // Given

            dynamic value = new DynamicWrapper();
            value.FirstName = "Ivan";
            value.LastName = "Ivanov";
            value.BirthDate = DateTime.Now;
            value.Weight = 123.456;

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format =
                "Patient: {FirstName} {LastName}. Birth date: {BirthDate:d}, weight: {Weight:N0} kg.";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual(
                string.Format("Patient: {0} {1}. Birth date: {2:d}, weight: {3:N0} kg.", value.FirstName, value.LastName,
                    value.BirthDate, value.Weight), result);
        }

        [Test]
        public void ShouldFormatObjectWithStringProperties()
        {
            // Given

            dynamic value = new DynamicWrapper();
            value.FirstName = "Ivan";
            value.LastName = "Ivanov";
            value.BirthDate = DateTime.Now.ToString("O");
            value.Weight = 123.456.ToString("n3");

            dynamic elementMetadata = new DynamicWrapper();
            elementMetadata.Format =
                "Patient: {FirstName} {LastName}. Birth date: {BirthDate:d}, weight: {Weight:N0} kg.";

            // When
            Func<object, string> format = BuildTestHelper.BuildObjectFormat(elementMetadata);
            var result = format(value);

            // Then
            Assert.AreEqual(
                string.Format("Patient: {0} {1}. Birth date: {2:d}, weight: {3:N0} kg.", value.FirstName, value.LastName,
                    DateTime.Parse(value.BirthDate), double.Parse(value.Weight)), result);
        }
    }
}