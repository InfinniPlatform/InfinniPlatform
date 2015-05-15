using System.Collections.Generic;
using System.Linq;

using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Bands;
using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Bands
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ReportDataBandBuilderTest
	{
		private static readonly ReportDataBandBuilder Target = new ReportDataBandBuilder();

		public IParent[] Containers
			= new IParent[]
				  {
					  new ReportPage(),
					  new DataBand()
				  };

		[Test]
		[TestCaseSource("Containers")]
		public void ShouldBuildReportDataBand(Base parent)
		{
			// Given
			var template = new ReportDataBand();
			var context = new FrReportObjectBuilderContext();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual(1, parent.ChildObjects.Count);
			Assert.IsInstanceOf<DataBand>(parent.ChildObjects[0]);
		}

		[Test]
		[TestCaseSource("Containers")]
		public void ShouldBuildReportDataBandWithAllProperties(Base parent)
		{
			// Given

			var context = new FrReportObjectBuilderContext();
			var collectionBindBuilder = new Mock<IReportObjectBuilder<CollectionBind>>();
			context.RegisterBuilder(collectionBindBuilder.Object);
			var contentBuilder = new Mock<IReportObjectBuilder<ReportBand>>();
			context.RegisterBuilder(contentBuilder.Object);
			var detailsBuilder = new Mock<IReportObjectBuilder<ReportDataBand>>();
			context.RegisterBuilder(detailsBuilder.Object);
			var groupBuilder = new Mock<IReportObjectBuilder<ReportGroupBand>>();
			context.RegisterBuilder(groupBuilder.Object);

			var template = new ReportDataBand
							   {
								   DataBind = new CollectionBind(),
								   Content = new ReportBand(),
								   Details = new ReportDataBand(),
								   Groups = new List<ReportGroupBand>
						                        {
							                        new ReportGroupBand(),
							                        new ReportGroupBand(),
							                        new ReportGroupBand()
						                        }
							   };

			// When
			Target.BuildObject(context, template, parent);

			// Then
			collectionBindBuilder.Verify(m => m.BuildObject(context, template.DataBind, It.IsAny<object>()));
			contentBuilder.Verify(m => m.BuildObject(context, template.Content, It.IsAny<object>()));
			detailsBuilder.Verify(m => m.BuildObject(context, template.Details, It.IsAny<object>()));
			groupBuilder.Verify(m => m.BuildObject(context, template.Groups.ElementAt(0), It.IsAny<object>()));
			groupBuilder.Verify(m => m.BuildObject(context, template.Groups.ElementAt(1), It.IsAny<object>()));
			groupBuilder.Verify(m => m.BuildObject(context, template.Groups.ElementAt(2), It.IsAny<object>()));
		}
	}
}