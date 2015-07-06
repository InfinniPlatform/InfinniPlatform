using System;
using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.ObjectValidators;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Validations;
using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class GuidValidatorTest
    {
        private static readonly GuidValidator Validator = new GuidValidator {Message = "Error"};


        private static readonly object[] FailureTestCase =
            {
                null,
                "",
                "NotGuid"
            };

        private static readonly object[] SuccessTestCase =
            {
                "436CAC70-4BD9-4476-B513-A13D7A6F197F",
                Guid.NewGuid()
            };


        [Test]
        [TestCaseSource("FailureTestCase")]
        public void ShouldValidateWhenFailure(object validationObject)
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
        [TestCaseSource("SuccessTestCase")]
        public void ShouldValidateWhenSuccess(object validationObject)
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