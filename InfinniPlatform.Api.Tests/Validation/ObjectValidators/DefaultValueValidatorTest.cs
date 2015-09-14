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
    public sealed class DefaultValueValidatorTest
    {
        private static readonly DefaultValueValidator Validator = new DefaultValueValidator {Message = "Error"};


        private static readonly object[] FailureTestCase =
            {
                123,
                DateTime.Today,
                Guid.NewGuid()
            };

        private static readonly object[] SuccessTestCase =
            {
                default(int),
                default(DateTime),
                default(Guid)
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