using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Data;
using InfinniPlatform.FastReport.Templates.Data;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class DataBindTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context
			= new FrReportObjectTemplateBuilderContext();

		[TestFixtureSetUp]
		public void Setup()
		{
			var report = new Report();
			report.Dictionary.DataSources.Add(TestReportDataSource.Instance);
			report.Dictionary.Parameters.Add(new Parameter("Parameter1"));
			report.Dictionary.Totals.Add(new Total { Name = "Total1" });

			_context.Report = report;
		}

		[Test]
		public void ShouldBuildDataBind()
		{
			// Given

			const string constantExpression = "Some constant";
			const string parameterExpression = "[Parameter1]";
			const string totalExpression = "[Total1]";
			const string propertyExpression = "[Document.Title]";

			var constantBindBuilder = new Mock<IReportObjectTemplateBuilder<ConstantBind>>();
			constantBindBuilder.Setup(m => m.BuildTemplate(_context, constantExpression))
				.Returns(new ConstantBind { Value = "Some constant" });

			var parameterBindBuilder = new Mock<IReportObjectTemplateBuilder<ParameterBind>>();
			parameterBindBuilder.Setup(m => m.BuildTemplate(_context, parameterExpression))
				.Returns(new ParameterBind { Parameter = "Parameter1" });

			var totalBindBuilder = new Mock<IReportObjectTemplateBuilder<TotalBind>>();
			totalBindBuilder.Setup(m => m.BuildTemplate(_context, totalExpression))
				.Returns(new TotalBind { Total = "Total1" });

			var propertyBindBindBuilder = new Mock<IReportObjectTemplateBuilder<PropertyBind>>();
			propertyBindBindBuilder.Setup(m => m.BuildTemplate(_context, propertyExpression))
				.Returns(new PropertyBind { DataSource = "Document", Property = "Title" });

			_context.RegisterBuilder(constantBindBuilder.Object);
			_context.RegisterBuilder(parameterBindBuilder.Object);
			_context.RegisterBuilder(totalBindBuilder.Object);
			_context.RegisterBuilder(propertyBindBindBuilder.Object);

			// When
			var target = new DataBindTemplateBuilder();
			var constantBind = target.BuildTemplate(_context, constantExpression);
			var parameterBind = target.BuildTemplate(_context, parameterExpression);
			var totalBind = target.BuildTemplate(_context, totalExpression);
			var propertyBind = target.BuildTemplate(_context, propertyExpression);

			// Then
			Assert.IsNotNull(constantBind);
			Assert.IsNotNull(parameterBind);
			Assert.IsNotNull(totalBind);
			Assert.IsNotNull(propertyBind);
		}
	}
}