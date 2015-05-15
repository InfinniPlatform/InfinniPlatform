using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.BooleanValidators;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Validation.BooleanValidators
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class NotValidatorTest
	{
		private static readonly IValidationOperator FasleOperator = new FaseValidationOperator();
		private static readonly IValidationOperator TrueOperator = new TrueValidationOperator();


		[Test]
		public void ShouldValidateWhenFailure()
		{
			// Given
			var validator = new NotValidator { Operator = TrueOperator, Message = "Error" };

			// When
			var result = new ValidationResult();
			var isValid = validator.Validate(new object(), result);

			// Then
			Assert.IsFalse(isValid);
			Assert.IsFalse(result.IsValid);
			Assert.IsTrue(result.Items != null && result.Items.Count == 1);
			Assert.AreEqual(validator.Message, result.Items[0].Message);
		}


		[Test]
		public void ShouldValidateWhenSuccess()
		{
			// Given
			var validator = new NotValidator { Operator = FasleOperator, Message = "Error" };

			// When
			var result = new ValidationResult();
			var isValid = validator.Validate(new object(), result);

			// Then
			Assert.IsTrue(isValid);
			Assert.IsTrue(result.IsValid);
			Assert.IsTrue(result.Items == null || result.Items.Count == 0);
		}
	}
}