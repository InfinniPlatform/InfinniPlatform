using System;

using InfinniPlatform.Dynamic;
using InfinniPlatform.PrintView.Abstractions.Format;

using NUnit.Framework;

namespace InfinniPlatform.PrintView.Tests.Factories.Format
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ObjectFormatFactoryTest
    {
        [Test]
        public void ShouldReturnNullWhenFormatStringIsNull()
        {
            // Given
            var value = new object();
            var template = new ObjectFormat();

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual(value.ToString(), result);
        }

        [Test]
        public void ShouldReturnFormatStringWhenNoPlaceholders()
        {
            // Given
            var value = new object();
            var template = new ObjectFormat { Format = "Some format string" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual("Some format string", result);
        }

        [Test]
        public void ShouldReturnNullWhenValueIsNull()
        {
            // Given
            var template = new ObjectFormat { Format = "{}" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(null);

            // Then
            Assert.AreEqual("", result);
        }

        [Test]
        public void ShouldFomatString()
        {
            // Given
            const string value = "Value1";
            var template = new ObjectFormat { Format = "Value: {}!" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual($"Value: {value}!", result);
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
            var template = new ObjectFormat { Format = "{:" + numberFormat + "}" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
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
            var template = new ObjectFormat { Format = "{:" + numberFormat + "}" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
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
            var template = new ObjectFormat { Format = "{:" + numberFormat + "}" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
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
            var template = new ObjectFormat { Format = "{:" + dateTimeFormat + "}" };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            var expectedFormat = "{0:" + dateTimeFormat + "}";
            Assert.AreEqual(string.Format(expectedFormat, value), result);
        }

        [Test]
        public void ShouldFormatObject()
        {
            // Given

            var value = new DynamicWrapper
                        {
                            { "FirstName", "Ivan" },
                            { "LastName", "Ivanov" },
                            { "BirthDate", DateTime.Now },
                            { "Weight", 123.456 }
                        };

            var template = new ObjectFormat { Format = "Patient: {FirstName} {LastName}. Birth date: {BirthDate:d}, weight: {Weight:N0} kg." };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual($"Patient: {value["FirstName"]} {value["LastName"]}. Birth date: {value["BirthDate"]:d}, weight: {value["Weight"]:N0} kg.", result);
        }

        [Test]
        public void ShouldFormatObjectWithStringProperties()
        {
            // Given

            var value = new DynamicWrapper
                        {
                            { "FirstName", "Ivan" },
                            { "LastName", "Ivanov" },
                            { "BirthDate", DateTime.Now.ToString("O") },
                            { "Weight", 123.456.ToString("n3") }
                        };

            var template = new ObjectFormat { Format = "Patient: {FirstName} {LastName}. Birth date: {BirthDate:d}, weight: {Weight:N0} kg." };

            // When
            var format = BuildTestHelper.BuildElement<Func<object, string>>(template);
            var result = format(value);

            // Then
            Assert.AreEqual($"Patient: {value["FirstName"]} {value["LastName"]}. Birth date: {DateTime.Parse((string)value["BirthDate"]):d}, weight: {double.Parse((string)value["Weight"]):N0} kg.", result);
        }
    }
}