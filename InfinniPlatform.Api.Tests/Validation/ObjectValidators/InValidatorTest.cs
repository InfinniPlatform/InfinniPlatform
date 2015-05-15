using InfinniPlatform.Api.Validation;
using InfinniPlatform.Api.Validation.ObjectValidators;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.Validation.ObjectValidators
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class InValidatorTest
	{
		private static readonly InValidator Validator = new InValidator { Items = new[] { 1, 2, 3 }, Message = "Error" };


		[Test]
		[TestCase(null)]
		[TestCase(0)]
		[TestCase(4)]
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
		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
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