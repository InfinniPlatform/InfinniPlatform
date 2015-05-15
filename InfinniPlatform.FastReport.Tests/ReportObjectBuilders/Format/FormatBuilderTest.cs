using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Format;
using InfinniPlatform.FastReport.Templates.Formats;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Format
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class FormatBuilderTest
	{
		[Test]
		public void ShouldBuildBooleanFormat()
		{
			// Given

			var template = new BooleanFormat
							   {
								   FalseText = "Неправда",
								   TrueText = "Правда"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new BooleanFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.BooleanFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual("Неправда", format.FalseText);
			Assert.AreEqual("Правда", format.TrueText);
		}

		[Test]
		public void ShouldBuildNumberFormat()
		{
			// Given

			var template = new NumberFormat
							   {
								   DecimalDigits = 5,
								   GroupSeparator = "'",
								   DecimalSeparator = "."
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new NumberFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.NumberFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual(5, format.DecimalDigits);
			Assert.AreEqual("'", format.GroupSeparator);
			Assert.AreEqual(".", format.DecimalSeparator);
		}

		[Test]
		public void ShouldBuildPercentFormat()
		{
			// Given

			var template = new PercentFormat
							   {
								   DecimalDigits = 5,
								   GroupSeparator = "'",
								   DecimalSeparator = ".",
								   PercentSymbol = "процентов"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new PercentFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.PercentFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual(5, format.DecimalDigits);
			Assert.AreEqual("'", format.GroupSeparator);
			Assert.AreEqual(".", format.DecimalSeparator);
			Assert.AreEqual("процентов", format.PercentSymbol);
		}

		[Test]
		public void ShouldBuildCurrencyFormat()
		{
			// Given

			var template = new CurrencyFormat
							   {
								   DecimalDigits = 5,
								   GroupSeparator = "'",
								   DecimalSeparator = ".",
								   CurrencySymbol = "РУБ"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new CurrencyFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.CurrencyFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual(5, format.DecimalDigits);
			Assert.AreEqual("'", format.GroupSeparator);
			Assert.AreEqual(".", format.DecimalSeparator);
			Assert.AreEqual("РУБ", format.CurrencySymbol);
		}

		[Test]
		public void ShouldBuildCustomFormat()
		{
			// Given

			var template = new CustomFormat
							   {
								   Format = "N2"
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new CustomFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.CustomFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual("N2", format.Format);
		}

		[Test]
		[TestCase(DateFormats.Default, "d")]
		[TestCase(DateFormats.ShortDate, "d")]
		[TestCase(DateFormats.LongDate, "D")]
		public void ShouldBuildDateFormat(DateFormats dateFormat, string expectedDateFormat)
		{
			// Given

			var template = new DateFormat
							   {
								   Format = dateFormat
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new DateFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.DateFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual(expectedDateFormat, format.Format);
		}

		[Test]
		[TestCase(TimeFormats.Default, "t")]
		[TestCase(TimeFormats.ShortTime, "t")]
		[TestCase(TimeFormats.LongTime, "T")]
		public void ShouldBuildTimeFormat(TimeFormats timeFormat, string expectedTimeFormat)
		{
			// Given

			var template = new TimeFormat
							   {
								   Format = timeFormat
							   };

			var context = new FrReportObjectBuilderContext();
			var target = new TimeFormatBuilder();
			var parent = new TextObject();

			// When
			target.BuildObject(context, template, parent);

			// Then
			var format = parent.Format as global::FastReport.Format.TimeFormat;
			Assert.IsNotNull(format);
			Assert.AreEqual(expectedTimeFormat, format.Format);
		}
	}
}