using System.Linq;

using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Data;

using NUnit.Framework;

using SortOrder = InfinniPlatform.FastReport.Templates.Data.SortOrder;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class CollectionBindTemplateBuilderTest
	{
		private static readonly FrReportObjectTemplateBuilderContext Context
			= new FrReportObjectTemplateBuilderContext();

		[OneTimeSetUp]
		public void Setup()
		{
			var report = new Report();
			report.Dictionary.DataSources.Add(TestReportDataSource.Instance);

			Context.Report = report;
		}

		[Test]
		public void ShouldBuildCollectionBindForRootDataSource()
		{
			// Given
			var dataSource = TestReportDataSource.Instance;
			var dataBand = new DataBand { DataSource = dataSource };

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("", result.Property);
			Assert.IsTrue(result.SortFields == null || result.SortFields.Count <= 0);
		}

		[Test]
		public void ShouldBuildCollectionBindForCollectionOfNestedObject()
		{
			// Given
			var dataSource = (DataSourceBase)TestReportDataSource.Instance.FindObject("Addresses");
			var dataBand = new DataBand { DataSource = dataSource };

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("Patient.Addresses.$", result.Property);
			Assert.IsTrue(result.SortFields == null || result.SortFields.Count <= 0);
		}

		[Test]
		public void ShouldBuildCollectionBindForNestedCollection()
		{
			// Given
			var dataSource = (DataSourceBase)TestReportDataSource.Instance.FindObject("Authors");
			var dataBand = new DataBand { DataSource = dataSource };

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("Authors.$", result.Property);
			Assert.IsTrue(result.SortFields == null || result.SortFields.Count <= 0);
		}

		[Test]
		public void ShouldBuildCollectionBindForCollectionOfNestedCollection()
		{
			// Given
			var dataSource = (DataSourceBase)TestReportDataSource.Instance.FindObject("Contacts");
			var dataBand = new DataBand { DataSource = dataSource };

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("Authors.$.Contacts.$", result.Property);
			Assert.IsTrue(result.SortFields == null || result.SortFields.Count <= 0);
		}


		[Test]
		public void ShouldBuildSortFieldsForRootDataSource()
		{
			// Given
			var dataSource = TestReportDataSource.Instance;
			var dataBand = new DataBand { DataSource = dataSource };
			dataBand.Sort.Add(new Sort { Expression = "[Document.Id]", Descending = false });
			dataBand.Sort.Add(new Sort { Expression = "[Document.Title]", Descending = true });

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("", result.Property);
			Assert.IsNotNull(result.SortFields);
			Assert.AreEqual(2, result.SortFields.Count);
			Assert.AreEqual("Id", result.SortFields.ElementAt(0).Property);
			Assert.AreEqual(SortOrder.Ascending, result.SortFields.ElementAt(0).SortOrder);
			Assert.AreEqual("Title", result.SortFields.ElementAt(1).Property);
			Assert.AreEqual(SortOrder.Descending, result.SortFields.ElementAt(1).SortOrder);
		}

		[Test]
		public void ShouldBuildSortFieldsForCollectionOfNestedObject()
		{
			// Given
			var dataSource = (DataSourceBase)TestReportDataSource.Instance.FindObject("Addresses");
			var dataBand = new DataBand { DataSource = dataSource };
			dataBand.Sort.Add(new Sort { Expression = "[Document.Patient.Addresses.City]", Descending = false });
			dataBand.Sort.Add(new Sort { Expression = "[Document.Patient.Addresses.Street]", Descending = true });

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("Patient.Addresses.$", result.Property);
			Assert.IsNotNull(result.SortFields);
			Assert.AreEqual(2, result.SortFields.Count);
			Assert.AreEqual("Patient.Addresses.$.City", result.SortFields.ElementAt(0).Property);
			Assert.AreEqual(SortOrder.Ascending, result.SortFields.ElementAt(0).SortOrder);
			Assert.AreEqual("Patient.Addresses.$.Street", result.SortFields.ElementAt(1).Property);
			Assert.AreEqual(SortOrder.Descending, result.SortFields.ElementAt(1).SortOrder);
		}

		[Test]
		public void ShouldBuildSortFieldsForNestedCollection()
		{
			// Given
			var dataSource = (DataSourceBase)TestReportDataSource.Instance.FindObject("Authors");
			var dataBand = new DataBand { DataSource = dataSource };
			dataBand.Sort.Add(new Sort { Expression = "[Document.Authors.Type]", Descending = false });
			dataBand.Sort.Add(new Sort { Expression = "[Document.Authors.Employee.FirstName]", Descending = true });

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("Authors.$", result.Property);
			Assert.IsNotNull(result.SortFields);
			Assert.AreEqual(2, result.SortFields.Count);
			Assert.AreEqual("Authors.$.Type", result.SortFields.ElementAt(0).Property);
			Assert.AreEqual(SortOrder.Ascending, result.SortFields.ElementAt(0).SortOrder);
			Assert.AreEqual("Authors.$.Employee.FirstName", result.SortFields.ElementAt(1).Property);
			Assert.AreEqual(SortOrder.Descending, result.SortFields.ElementAt(1).SortOrder);
		}

		[Test]
		public void ShouldBuildSortFieldsForCollectionOfNestedCollection()
		{
			// Given
			var dataSource = (DataSourceBase)TestReportDataSource.Instance.FindObject("Contacts");
			var dataBand = new DataBand { DataSource = dataSource };
			dataBand.Sort.Add(new Sort { Expression = "[Document.Authors.Contacts.Caption]", Descending = false });
			dataBand.Sort.Add(new Sort { Expression = "[Document.Authors.Contacts.Value]", Descending = true });

			// When
			var target = new CollectionBindTemplateBuilder();
			var result = target.BuildTemplate(Context, dataBand);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Document", result.DataSource);
			Assert.AreEqual("Authors.$.Contacts.$", result.Property);
			Assert.IsNotNull(result.SortFields);
			Assert.AreEqual(2, result.SortFields.Count);
			Assert.AreEqual("Authors.$.Contacts.$.Caption", result.SortFields.ElementAt(0).Property);
			Assert.AreEqual(SortOrder.Ascending, result.SortFields.ElementAt(0).SortOrder);
			Assert.AreEqual("Authors.$.Contacts.$.Value", result.SortFields.ElementAt(1).Property);
			Assert.AreEqual(SortOrder.Descending, result.SortFields.ElementAt(1).SortOrder);
		}
	}
}