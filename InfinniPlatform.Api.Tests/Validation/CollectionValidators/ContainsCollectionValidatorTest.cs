using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.CollectionValidators;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Validation.CollectionValidators
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ContainsCollectionValidatorTest
	{
		private static readonly ContainsCollectionValidator Validator = new ContainsCollectionValidator { Value = 3, Message = "Error" };


		private static readonly object[] FailureTestCase =
		{
			null,
			new object[] { },
			new object[] { 1, 2 }
		};

		private static readonly object[] SuccessTestCase =
		{
			new object[] { 1, 2, 3 }
		};


		[Test]
		[TestCaseSource("FailureTestCase")]
		public void ShouldValidateWhenFailure(object validationObject)
		{
			// When
			var result = new ValidationResult();
			var isValid = Validator.Validate(validationObject, result);

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
			var isValid = Validator.Validate(validationObject, result);

			// Then
			Assert.IsTrue(isValid);
			Assert.IsTrue(result.IsValid);
			Assert.IsTrue(result.Items == null || result.Items.Count == 0);
		}
	}
}