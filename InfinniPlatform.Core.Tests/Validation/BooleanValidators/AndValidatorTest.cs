using System.Linq;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.BooleanValidators;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.BooleanValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class AndValidatorTest
    {
        private static readonly FaseValidationOperator FasleOperator = new FaseValidationOperator
            {
                Message = "ErrorFalse"
            };

        private static readonly IValidationOperator TrueOperator = new TrueValidationOperator {Message = "ErrorTrue"};


        private static readonly object[] FailureTestCase =
            {
                new[] {FasleOperator},
                new[] {FasleOperator, FasleOperator},
                new[] {FasleOperator, TrueOperator},
                new[] {TrueOperator, FasleOperator},
                new[] {FasleOperator, FasleOperator, FasleOperator},
                new[] {FasleOperator, FasleOperator, TrueOperator},
                new[] {FasleOperator, TrueOperator, FasleOperator},
                new[] {FasleOperator, TrueOperator, TrueOperator},
                new[] {TrueOperator, FasleOperator, FasleOperator},
                new[] {TrueOperator, FasleOperator, TrueOperator},
                new[] {TrueOperator, TrueOperator, FasleOperator}
            };

        private static readonly object[] SuccessTestCase =
            {
                new IValidationOperator[] {},
                new[] {TrueOperator},
                new[] {TrueOperator, TrueOperator},
                new[] {TrueOperator, TrueOperator, TrueOperator}
            };

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
        public void ShouldBuildValidationResult(string parentProperty, string validationProperty,
                                                string expectedPropertyPath)
        {
            // Given
            var validator = new AndValidator {Property = validationProperty, Operators = new[] {FasleOperator}};

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(null, result, parentProperty);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Items != null && result.Items.Count == 1);
            Assert.AreEqual(expectedPropertyPath, result.Items[0].Property);
            Assert.AreEqual(FasleOperator.Message, result.Items[0].Message);
        }


        [Test]
        [TestCaseSource("FailureTestCase")]
        public void ShouldValidateWhenFailure(IValidationOperator[] operators)
        {
            // Given
            var validator = new AndValidator {Operators = operators};

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(new object(), result);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            int falseOperatorCount = operators.Count(o => o.Equals(FasleOperator));
            Assert.IsTrue((result.Items != null && result.Items.Count == falseOperatorCount &&
                           result.Items.All(i => i.Message == FasleOperator.Message)) || falseOperatorCount == 0);
        }


        [Test]
        [TestCaseSource("SuccessTestCase")]
        public void ShouldValidateWhenSuccess(IValidationOperator[] operators)
        {
            // Given
            var validator = new AndValidator {Operators = operators};

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(new object(), result);

            // Then
            Assert.IsTrue(isValid);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Items == null || result.Items.Count == 0);
        }
    }
}