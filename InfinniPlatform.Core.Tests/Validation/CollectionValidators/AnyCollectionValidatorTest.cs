using System;
using System.Linq;

using InfinniPlatform.Core.Validation;
using InfinniPlatform.Core.Validation.CollectionValidators;

using NUnit.Framework;

namespace InfinniPlatform.Core.Tests.Validation.CollectionValidators
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public sealed class AnyCollectionValidatorTest
    {
        private static readonly Func<object, bool> Predicate = i => Equals(i, 3);

        private static readonly PredicateValidationOperator PredicateOperator = new PredicateValidationOperator(Predicate) { Message = "Error" };

        private static readonly object[] FailureTestCase =
        {
            new object[] { },
            new object[] { 1, 2 }
        };

        private static readonly object[] SuccessTestCase =
        {
            new object[] { 1, 2, 3 }
        };


        [Test]
        public void ShouldBuildValidationResult()
        {
            // Given
            var validator = new AnyCollectionValidator { Operator = PredicateOperator };

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(new[] { 1, 2 }, result, "Collection1");

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Items != null && result.Items.Count == 2);
            Assert.AreEqual("Collection1.0", result.Items[0].Property);
            Assert.AreEqual("Collection1.1", result.Items[1].Property);
            Assert.AreEqual(PredicateOperator.Message, result.Items[0].Message);
            Assert.AreEqual(PredicateOperator.Message, result.Items[1].Message);
        }

        [Test]
        [TestCaseSource(nameof(FailureTestCase))]
        public void ShouldValidateWhenFailure(object validationObjejct)
        {
            // Given
            var validator = new AnyCollectionValidator { Operator = PredicateOperator };

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(validationObjejct, result);

            // Then
            Assert.IsFalse(isValid);
            Assert.IsFalse(result.IsValid);
            int falseItemCount = (validationObjejct != null)
                                     ? ((object[])validationObjejct).Count(i => !Predicate(i))
                                     : 0;
            Assert.IsTrue((result.Items != null && result.Items.Count == falseItemCount &&
                           result.Items.All(i => i.Message == PredicateOperator.Message)) || falseItemCount == 0);
        }

        [Test]
        [TestCaseSource(nameof(SuccessTestCase))]
        public void ShouldValidateWhenSuccess(object validationObjejct)
        {
            // Given
            var validator = new AnyCollectionValidator { Operator = PredicateOperator };

            // When
            var result = new ValidationResult();
            bool isValid = validator.Validate(validationObjejct, result);

            // Then
            Assert.IsTrue(isValid);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Items == null || result.Items.Count == 0);
        }
    }
}