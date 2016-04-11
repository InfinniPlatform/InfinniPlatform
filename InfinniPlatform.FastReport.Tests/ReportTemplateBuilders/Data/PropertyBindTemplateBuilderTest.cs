using FastReport;

using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Data;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class PropertyBindTemplateBuilderTest
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
		[TestCase("[Document.Title]", "Title")]
		[TestCase("[Document.Patient.FirstName]", "Patient.FirstName")]
		[TestCase("[Document.Patient.Passport.IssueDate]", "Patient.Passport.IssueDate")]
		[TestCase("[Document.Patient.Addresses.City]", "Patient.Addresses.$.City")]
		[TestCase("[Document.Authors.Type]", "Authors.$.Type")]
		[TestCase("[Document.Authors.Employee.FirstName]", "Authors.$.Employee.FirstName")]
		[TestCase("[Document.Authors.Contacts.Value]", "Authors.$.Contacts.$.Value")]
		public void ShouldBuildPropertyBind(string expression, string propertyPath)
		{
			// Given

			// When
			var target = new PropertyBindTemplateBuilder();
			var result = target.BuildTemplate(Context, expression);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(TestReportDataSource.Instance.Name, result.DataSource);
			Assert.AreEqual(propertyPath, result.Property);
		}

		[Test]
		public void ShouldThrowExceptionWhenUnknownDataSource()
		{
			// Given
			const string expression = "[UnknownDataSource.UnknownProperty]";

			// When
			TestDelegate test = () =>
									{
										var target = new PropertyBindTemplateBuilder();
										target.BuildTemplate(Context, expression);
									};

			// Then
			Assert.IsNotNull(Assert.Catch(test));
		}

		[Test]
		public void ShouldThrowExceptionWhenUnknownProperty()
		{
			// Given
			const string expression = "[Document.UnknownProperty]";

			// When
			TestDelegate test = () =>
									{
										var target = new PropertyBindTemplateBuilder();
										target.BuildTemplate(Context, expression);
									};

			// Then
			Assert.IsNotNull(Assert.Catch(test));
		}

		[Test]
		[TestCase("[Document]")]
		[TestCase("[Document.Patient]")]
		[TestCase("[Document.Patient.Passport]")]
		[TestCase("[Document.Patient.Addresses]")]
		[TestCase("[Document.Authors]")]
		[TestCase("[Document.Authors.Employee]")]
		[TestCase("[Document.Authors.Contacts]")]
		public void ShouldThrowExceptionWhenReferencedOnComplexType(string expression)
		{
			// Given

			// When
			TestDelegate test = () =>
									{
										var target = new PropertyBindTemplateBuilder();
										target.BuildTemplate(Context, expression);
									};

			// Then
			Assert.IsNotNull(Assert.Catch(test));
		}
	}
}