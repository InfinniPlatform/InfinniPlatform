using InfinniPlatform.Api.Schema.Prefill;

using NUnit.Framework;

namespace InfinniPlatform.Api.Tests.TestDataGenerator
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public class TestDataGeneratorBehavior
	{
		[Test]
		public void ShoudlFillSimpleProperties()
		{
			var expressionBuilder = new TestDataBuilder("TestCreateDatabase");

			expressionBuilder.FillProperty("Name", DefaultPropertyNames.LastNameMan);

			dynamic instance = expressionBuilder.Build();

			Assert.IsNotNull(instance["Name"]);
		}

		[Test]
		public void ShouldFillComplexProperties()
		{
			var expressionBuilder = new TestDataBuilder("TestCreateDatabase");

			dynamic result = expressionBuilder
				.FillProperty("Gender", DefaultPropertyNames.Gender)
				.FillExpressionProperty("LastName",
							  instance =>
							  instance.Gender.Name == "Man"
								  ? DefaultPropertyNames.LastNameMan
								  : DefaultPropertyNames.LastNameWoman)
				.FillCalculatedProperty("PostIndex", (instance) => "454000").Build();

			Assert.IsNotNull(result.Gender);
			Assert.IsNotNull(result.LastName);
			Assert.AreEqual(result.PostIndex, "454000");
		}
	}
}