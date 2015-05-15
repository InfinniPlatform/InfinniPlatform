using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Data;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ConstantBindTemplateBuilderTest
	{
		private static readonly FrReportObjectTemplateBuilderContext Context
			= new FrReportObjectTemplateBuilderContext();

		[Test]
		public void ShouldBuildConstantBind()
		{
			// Given
			var constantValue = new object();

			// When
			var target = new ConstantBindTemplateBuilder();
			var result = target.BuildTemplate(Context, constantValue);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(constantValue, result.Value);
		}
	}
}