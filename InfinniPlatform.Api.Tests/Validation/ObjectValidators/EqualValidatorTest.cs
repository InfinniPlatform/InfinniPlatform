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
    public sealed class EqualValidatorTest
    {
        private static readonly ValidatorTestCase[] FailureTestCase =
            {
                new ValidatorTestCase {ValidationObject = "", Value = "Abc"},
                new ValidatorTestCase {ValidationObject = "Abc", Value = ""},
                new ValidatorTestCase {ValidationObject = null, Value = 123},
                new ValidatorTestCase {ValidationObject = 123, Value = null},
                new ValidatorTestCase {ValidationObject = null, Value = new DateTime(2014, 1, 1)},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = null},
                new ValidatorTestCase {ValidationObject = null, Value = "2014-01-01"}
            };

        private static readonly ValidatorTestCase[] SuccessTestCase =
            {
                new ValidatorTestCase {ValidationObject = null, Value = null},
                new ValidatorTestCase {ValidationObject = "", Value = ""},
                new ValidatorTestCase {ValidationObject = "Abc", Value = "Abc"},
                new ValidatorTestCase {ValidationObject = 123, Value = 123},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = new DateTime(2014, 1, 1)},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = "2014-01-01"}
            };


        [Test]
        [TestCaseSource("FailureTestCase")]
        public void ShouldValidateWhenFailure(ValidatorTestCase testCase)
        {
            // Given
            var validator = new EqualValidator {Value = testCase.Value, Message = "Error"};

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(testCase.ValidationObject, result);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Items != null && result.Items.Count == 1);
            Assert.AreEqual(validator.Message, result.Items[0].Message);
        }


        [Test]
        [TestCaseSource("SuccessTestCase")]
        public void ShouldValidateWhenSuccess(ValidatorTestCase testCase)
        {
            // Given
            var validator = new EqualValidator {Value = testCase.Value, Message = "Error"};

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(testCase.ValidationObject, result);

            // Then
            Assert.IsTrue(isValid);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Items == null || result.Items.Count == 0);
        }
    }
}