using FastReport;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportGroupBandTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildReportGroupBand()
		{
			// Given

			var dataBindBuilder = new Mock<IReportObjectTemplateBuilder<IDataBind>>();
			var reportBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportBand>>();
			var reportGroupBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportGroupBand>>();

			_context.RegisterBuilder(dataBindBuilder.Object);
			_context.RegisterBuilder(reportBandBuilder.Object);
			_context.RegisterBuilder(reportGroupBandBuilder.Object);

			var groupHeaderBand = new GroupHeaderBand
									  {
										  Condition = "[DataSource1.GroupProperty1]",
										  GroupFooter = new GroupFooterBand()
									  };

			// When
			var target = new ReportGroupBandTemplateBuilder();
			var result = target.BuildTemplate(_context, groupHeaderBand);

			// Then

			Assert.IsNotNull(result);

			dataBindBuilder.Verify(m => m.BuildTemplate(_context, groupHeaderBand.Condition));
			reportBandBuilder.Verify(m => m.BuildTemplate(_context, groupHeaderBand));
			reportBandBuilder.Verify(m => m.BuildTemplate(_context, groupHeaderBand.GroupFooter));
		}
	}
}