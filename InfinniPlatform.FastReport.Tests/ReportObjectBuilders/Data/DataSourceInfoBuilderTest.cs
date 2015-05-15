using System;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Data;
using InfinniPlatform.FastReport.TemplatesFluent.Data;
using InfinniPlatform.FastReport.Templates.Data;

using NUnit.Framework;

using FRReport = FastReport.Report;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Data
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class DataSourceInfoBuilderTest
	{
		[Test]
		public void ShouldBuildForSimpleObject()
		{
			// Given

			var template = CreateTemplate("Product", ds => ds
				.Property("Name", SchemaDataType.String)
				.Property("Price", SchemaDataType.Float));

			var context = new FrReportObjectBuilderContext();
			var target = new DataSourceInfoBuilder();
			var parent = new FRReport();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(1, parent.Dictionary.DataSources.Count);

			var productDataSource = parent.Dictionary.DataSources.FindByName("Product");
			Assert.IsNotNull(productDataSource);
			Assert.IsNotNull(productDataSource.Columns.FindByName("Name"));
			Assert.IsNotNull(productDataSource.Columns.FindByName("Price"));
		}

		[Test]
		public void ShouldBuildForObjectWithReference()
		{
			// Given

			var template = CreateTemplate("OrderItem", ds => ds
				.Property("Product", p => p
					.Property("Name", SchemaDataType.String)
					.Property("Price", SchemaDataType.Float))
				.Property("Quantity", SchemaDataType.Float));

			var context = new FrReportObjectBuilderContext();
			var target = new DataSourceInfoBuilder();
			var parent = new FRReport();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(1, parent.Dictionary.DataSources.Count);

			var orderItemDataSource = parent.Dictionary.DataSources.FindByName("OrderItem");
			Assert.IsNotNull(orderItemDataSource);
			Assert.IsNotNull(orderItemDataSource.Columns.FindByName("Quantity"));

			var productDataSource = orderItemDataSource.Columns.FindByName("Product");
			Assert.IsNotNull(productDataSource);
			Assert.IsNotNull(productDataSource.Columns.FindByName("Name"));
			Assert.IsNotNull(productDataSource.Columns.FindByName("Price"));
		}

		[Test]
		public void ShouldBuildForObjectWithCollection()
		{
			// Given

			var template = CreateTemplate("Account", ds => ds
				.Property("UserName", SchemaDataType.String)
				.Property("Password", SchemaDataType.String)
				.Collection("Roles", p => p.Property("Name", SchemaDataType.String)));

			var context = new FrReportObjectBuilderContext();
			var target = new DataSourceInfoBuilder();
			var parent = new FRReport();

			// When
			target.BuildObject(context, template, parent);

			// Then

			Assert.AreEqual(1, parent.Dictionary.DataSources.Count);

			var accountDataSource = parent.Dictionary.DataSources.FindByName("Account");
			Assert.IsNotNull(accountDataSource);
			Assert.IsNotNull(accountDataSource.Columns.FindByName("UserName"));
			Assert.IsNotNull(accountDataSource.Columns.FindByName("Password"));

			var rolesDataSource = accountDataSource.Columns.FindByName("Roles");
			Assert.IsNotNull(rolesDataSource);
			Assert.IsNotNull(rolesDataSource.Columns.FindByName("Name"));
		}


		private static DataSourceInfo CreateTemplate(string name, Action<ObjectDataSchemaConfig> config)
		{
			var template = new DataSourceInfo { Name = name };
			new DataSourceInfoConfig(template).Schema(config);
			return template;
		}
	}
}