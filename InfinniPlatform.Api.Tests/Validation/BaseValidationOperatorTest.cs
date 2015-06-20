using InfinniPlatform.Api.Validation;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Validation
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class BaseValidationOperatorTest
    {
        [Test]
        [TestCase(null, null, "")]
        [TestCase(null, "", "")]
        [TestCase("", null, "")]
        [TestCase("", "", "")]
        [TestCase("Parent", null, "Parent")]
        [TestCase("Parent", "", "Parent")]
        [TestCase(null, "Property", "Property")]
        [TestCase("", "Property", "Property")]
        [TestCase("Parent", "Property", "Parent.Property")]
        public void ShouldBuildValidationResultWhenFail(string parentProperty, string validationProperty,
                                                        string expectedPropertyPath)
        {
            // Given
            var validator = new FaseValidationOperator {Property = validationProperty, Message = "Error"};

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(null, result, parentProperty);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Items != null && result.Items.Count == 1);
            Assert.AreEqual(expectedPropertyPath, result.Items[0].Property);
            Assert.AreEqual(validator.Message, result.Items[0].Message);
        }

        [Test]
        public void ShouldBuildValidationResultWhenSuccess()
        {
            // Given
            var validator = new TrueValidationOperator();

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(null, result);

            // Then
            Assert.IsTrue(isValid);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Items == null || result.Items.Count == 0);
        }
    }
}