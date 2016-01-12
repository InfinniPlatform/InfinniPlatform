using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.ObjectValidators;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class EndsWithValidatorTest
    {
        private static readonly EndsWithValidator Validator = new EndsWithValidator {Value = "Abc", Message = "Error"};


        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("Xyz")]
        [TestCase("AbcXyz")]
        [TestCase("abcXyz")]
        [TestCase("AbcXyz")]
        [TestCase("abcXyz")]
        public void ShouldValidateWhenFailure(string validationObject)
        {
            // When
            var result = new ValidationResult();
            bool isValid = Validator.Validate(validationObject, result);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Items != null && result.Items.Count == 1);
            Assert.AreEqual(Validator.Message, result.Items[0].Message);
        }


        [Test]
        [TestCase("Abc")]
        [TestCase("abc")]
        [TestCase("XyzAbc")]
        [TestCase("Xyzabc")]
        public void ShouldValidateWhenSuccess(string validationObject)
        {
            // When
            var result = new ValidationResult();
            bool isValid = Validator.Validate(validationObject, result);

            // Then
            Assert.IsTrue(isValid);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Items == null || result.Items.Count == 0);
        }
    }
}