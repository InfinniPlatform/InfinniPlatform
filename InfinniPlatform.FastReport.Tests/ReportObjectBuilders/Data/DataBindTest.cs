using System.Collections.Generic;

using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Data;
using InfinniPlatform.FastReport.Templates.Data;

using NUnit.Framework;

using FRReport = FastReport.Report;
using SortOrder = InfinniPlatform.FastReport.Templates.Data.SortOrder;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class DataBindTest
	{
		[Test]
		[TestCase(10, "[10]")]
		[TestCase(10.1, "[10.1]")]
		[TestCase("Abc def", "Abc def")]
		public void ShouldBuildConstantBind(object value, string expectedExpression)
		{
			// Given

			var template = new ConstantBind
							   {
								   Value = value
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new ConstantBindBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			Assert.IsNotNull(parent.Text);
			Assert.AreEqual(expectedExpression, parent.Text.Replace(',', '.'));
		}

		[Test]
		public void ShouldBuildParameterBind()
		{
			// Given

			var template = new ParameterBind
							   {
								   Parameter = "Parameter1"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new ParameterBindBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual("[Parameter1]", parent.Text);
		}

		[Test]
		public void ShouldBuildTotalBind()
		{
			// Given

			var template = new TotalBind
							   {
								   Total = "Total1"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new TotalBindBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual("[Total1]", parent.Text);
		}

		[Test]
		public void ShouldBuildPropertyBind()
		{
			// Given

			var template = new PropertyBind
							   {
								   DataSource = "Order",
								   Property = "Date"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new PropertyBindBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual("[Order.Date]", parent.Text);
		}

		[Test]
		public void ShouldBuildCollectionBind()
		{
			// Given

			var template = new CollectionBind
							   {
								   DataSource = "Order",
								   Property = "Items.$",
								   SortFields = new List<SortField>
						                            {
							                            new SortField { Property = "Items.$.Cost", SortOrder = SortOrder.Ascending },
							                            new SortField { Property = "Items.$.Quantity", SortOrder = SortOrder.Descending }
						                            }
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new CollectionBindBuilder();

			var report = new FRReport();

			var reportPage = new ReportPage { Name = "Page1" };
			report.Pages.Add(reportPage);

			var orderDataSource = new JsonObjectDataSource { Name = "Order" };
			var orderItemsDataSource = new JsonObjectDataSource { Name = "Items" };
			orderDataSource.Columns.Add(orderItemsDataSource);
			orderItemsDataSource.Columns.Add(new Column { Name = "Cost", DataType = typeof(double) });
			orderItemsDataSource.Columns.Add(new Column { Name = "Quantity", DataType = typeof(double) });
			report.Dictionary.DataSources.Add(orderDataSource);

			var dataBand = new DataBand();
			reportPage.Bands.Add(dataBand);

			// When
			target.BuildObject(context, template, dataBand);

			// Then
			Assert.AreEqual(orderItemsDataSource, dataBand.DataSource);
			Assert.AreEqual(2, dataBand.Sort.Count);
			Assert.IsFalse(dataBand.Sort[0].Descending);
			Assert.AreEqual("[Order.Items.Cost]", dataBand.Sort[0].Expression);
			Assert.IsTrue(dataBand.Sort[1].Descending);
			Assert.AreEqual("[Order.Items.Quantity]", dataBand.Sort[1].Expression);
		}
	}
}