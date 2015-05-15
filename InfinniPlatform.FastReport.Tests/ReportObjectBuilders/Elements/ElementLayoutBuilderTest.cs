using System;

using FastReport;
using FastReport.Table;
using FastReport.Utils;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Elements;
using InfinniPlatform.FastReport.TemplatesFluent.Elements;
using InfinniPlatform.FastReport.Templates.Elements;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Elements
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class ElementLayoutBuilderTest
	{
		private static readonly ElementLayoutBuilder Target = new ElementLayoutBuilder();


		[Test]
		public void ShouldBuildLayoutForText()
		{
			// Given

			var template = CreateTemplate(l => l
				.Top(10)
				.Left(20)
				.Width(100)
				.Height(200));

			var context = new FrReportObjectBuilderContext();
			var parent = new TextObject();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual(10 * Units.Millimeters, parent.Top);
			Assert.AreEqual(20 * Units.Millimeters, parent.Left);
			Assert.AreEqual(100 * Units.Millimeters, parent.Width);
			Assert.AreEqual(200 * Units.Millimeters, parent.Height);
		}

		[Test]
		public void ShouldBuildLayoutForTable()
		{
			// Given

			var template = CreateTemplate(l => l
				.Top(10)
				.Left(20)
				.Width(100)
				.Height(200));

			var context = new FrReportObjectBuilderContext();
			var parent = new TableObject();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual(10 * Units.Millimeters, parent.Top);
			Assert.AreEqual(20 * Units.Millimeters, parent.Left);
			Assert.AreEqual(100 * Units.Millimeters, parent.Width);
			Assert.AreEqual(200 * Units.Millimeters, parent.Height);
		}

		[Test]
		public void ShouldBuildLayoutForTableWithAutoSize()
		{
			// Given

			var template = CreateTemplate(l => l
				.Top(10)
				.Left(20));

			var context = new FrReportObjectBuilderContext();
			var parent = new TableObject();

			// When
			Target.BuildObject(context, template, parent);

			// Then
			Assert.AreEqual(10 * Units.Millimeters, parent.Top);
			Assert.AreEqual(20 * Units.Millimeters, parent.Left);
			Assert.AreEqual(0, parent.Width);
			Assert.AreEqual(0, parent.Height);
		}

		private static ElementLayout CreateTemplate(Action<ElementLayoutConfig> config)
		{
			var template = new ElementLayout();
			config(new ElementLayoutConfig(template));
			return template;
		}
	}
}