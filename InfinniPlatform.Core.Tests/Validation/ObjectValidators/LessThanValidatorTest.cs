using System;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.ObjectValidators;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.ObjectValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class LessThanValidatorTest
    {
        private static readonly ValidatorTestCase[] FailureTestCase =
            {
                new ValidatorTestCase {ValidationObject = null, Value = 123},
                new ValidatorTestCase {ValidationObject = 123, Value = 123},
                new ValidatorTestCase {ValidationObject = 124, Value = 123},
                new ValidatorTestCase {ValidationObject = null, Value = new DateTime(2014, 1, 1)},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = new DateTime(2014, 1, 1)},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 2), Value = new DateTime(2014, 1, 1)},
                new ValidatorTestCase {ValidationObject = null, Value = "2014-01-01"},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = "2014-01-01"},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 2), Value = "2014-01-01"}
            };

        private static readonly ValidatorTestCase[] SuccessTestCase =
            {
                new ValidatorTestCase {ValidationObject = 123, Value = 1234},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = new DateTime(2014, 1, 2)},
                new ValidatorTestCase {ValidationObject = new DateTime(2014, 1, 1), Value = "2014-01-02"}
            };


        [Test]
        [TestCaseSource("FailureTestCase")]
        public void ShouldValidateWhenFailure(ValidatorTestCase testCase)
        {
            // Given
            var validator = new LessThanValidator {Value = testCase.Value, Message = "Error"};

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
            var validator = new LessThanValidator {Value = testCase.Value, Message = "Error"};

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