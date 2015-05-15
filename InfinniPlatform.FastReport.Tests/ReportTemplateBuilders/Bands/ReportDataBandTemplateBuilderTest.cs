using FastReport;
using FastReport.Data;

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
	public sealed class ReportDataBandTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildReportDataBand()
		{
			// Given

			var collectionBindBuilder = new Mock<IReportObjectTemplateBuilder<CollectionBind>>();
			var reportBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportBand>>();
			var reportDataBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportDataBand>>();
			var reportGroupBandBuilder = new Mock<IReportObjectTemplateBuilder<ReportGroupBand>>();
			reportGroupBandBuilder.Setup(m => m.BuildTemplate(_context, It.IsAny<object>())).Returns(new ReportGroupBand());

			_context.RegisterBuilder(collectionBindBuilder.Object);
			_context.RegisterBuilder(reportBandBuilder.Object);
			_context.RegisterBuilder(reportDataBandBuilder.Object);
			_context.RegisterBuilder(reportGroupBandBuilder.Object);

			var dataBand = new DataBand
							   {
								   DataSource = new TableDataSource(),
							   };

			var detailsDataBand = new DataBand();
			dataBand.AddChild(detailsDataBand);

			var subgroupHeaderBand = new GroupHeaderBand();
			dataBand.Parent = subgroupHeaderBand;

			var groupHeaderBand = new GroupHeaderBand();
			subgroupHeaderBand.Parent = groupHeaderBand;

			// When
			var target = new ReportDataBandTemplateBuilder();
			var result = target.BuildTemplate(_context, dataBand);

			// Then

			Assert.IsNotNull(result);

			collectionBindBuilder.Verify(m => m.BuildTemplate(_context, dataBand));
			reportBandBuilder.Verify(m => m.BuildTemplate(_context, dataBand));
			reportDataBandBuilder.Verify(m => m.BuildTemplate(_context, detailsDataBand));
			reportGroupBandBuilder.Verify(m => m.BuildTemplate(_context, groupHeaderBand));
			reportGroupBandBuilder.Verify(m => m.BuildTemplate(_context, subgroupHeaderBand));
		}
	}
}