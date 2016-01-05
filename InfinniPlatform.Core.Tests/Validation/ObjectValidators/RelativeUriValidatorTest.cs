using InfinniPlatform.Core.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Environment.Validations;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class RelativeUriValidatorTest
    {
        private static readonly RelativeUriValidator Validator = new RelativeUriValidator {Message = "Error"};


        [Test]
        public void ShouldValidateAbsoluteUri()
        {
            // Given
            const string validationObject = "http://wiki.infinnity.lan:8081/display/MC/IsAbsoluteUri";

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
        public void ShouldValidateRelativeUri()
        {
            // Given
            const string validationObject = "/display/MC/IsRelativeUri";

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