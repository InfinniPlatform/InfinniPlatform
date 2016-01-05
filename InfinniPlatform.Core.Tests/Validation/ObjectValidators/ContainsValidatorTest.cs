using InfinniPlatform.Core.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Environment.Validations;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class ContainsValidatorTest
    {
        private static readonly ContainsValidator Validator = new ContainsValidator {Value = "Abc", Message = "Error"};


        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("Xyz")]
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
        [TestCase("AbcXyz")]
        [TestCase("abcXyz")]
        [TestCase("XyAbcz")]
        [TestCase("Xyabcz")]
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