using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Format;
using InfinniPlatform.FastReport.Templates.Formats;

using NUnit.Framework;

namespace InfinniPlatform.FastReport.Tests.ReportTemplateBuilders.Format
{
	[TestFixture]
	[Category(TestCategories.UnitTest)]
	public sealed class FormatTemplateBuilderTest
	{
		private readonly FrReportObjectTemplateBuilderContext _context = new FrReportObjectTemplateBuilderContext();


		[Test]
		public void ShouldBuildBooleanFormatTemplate()
		{
			// Given
			var reportObject = new global::FastReport.Format.BooleanFormat
								   {
									   FalseText = "Неправда",
									   TrueText = "Правда"
								   };

			// When
			var target = new BooleanFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("Неправда", result.FalseText);
			Assert.AreEqual("Правда", result.TrueText);
		}

		[Test]
		public void ShouldBuildNumberFormatTemplate()
		{
			// Given
			var reportObject = new global::FastReport.Format.NumberFormat
								   {
									   DecimalDigits = 5,
									   GroupSeparator = "'",
									   DecimalSeparator = "."
								   };

			// When
			var target = new NumberFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(5, result.DecimalDigits);
			Assert.AreEqual("'", result.GroupSeparator);
			Assert.AreEqual(".", result.DecimalSeparator);
		}

		[Test]
		public void ShouldBuildPercentFormatTemplate()
		{
			// Given
			var reportObject = new global::FastReport.Format.PercentFormat
								   {
									   DecimalDigits = 5,
									   GroupSeparator = "'",
									   DecimalSeparator = ".",
									   PercentSymbol = "процентов"
								   };

			// When
			var target = new PercentFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(5, result.DecimalDigits);
			Assert.AreEqual("'", result.GroupSeparator);
			Assert.AreEqual(".", result.DecimalSeparator);
			Assert.AreEqual("процентов", result.PercentSymbol);
		}

		[Test]
		public void ShouldBuildCurrencyFormatTemplate()
		{
			// Given
			var reportObject = new global::FastReport.Format.CurrencyFormat
								   {
									   DecimalDigits = 5,
									   GroupSeparator = "'",
									   DecimalSeparator = ".",
									   CurrencySymbol = "РУБ"
								   };

			// When
			var target = new CurrencyFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(5, result.DecimalDigits);
			Assert.AreEqual("'", result.GroupSeparator);
			Assert.AreEqual(".", result.DecimalSeparator);
			Assert.AreEqual("РУБ", result.CurrencySymbol);
		}

		[Test]
		public void ShouldBuildCustomFormatTemplate()
		{
			// Given
			var reportObject = new global::FastReport.Format.CustomFormat
								   {
									   Format = "N2"
								   };

			// When
			var target = new CustomFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual("N2", result.Format);
		}

		[Test]
		[TestCase("", DateFormats.Default)]
		[TestCase("d", DateFormats.ShortDate)]
		[TestCase("D", DateFormats.LongDate)]
		public void ShouldBuildDateFormatTemplate(string dateFormat, DateFormats expectedDateFormat)
		{
			// Given
			var reportObject = new global::FastReport.Format.DateFormat
								   {
									   Format = dateFormat
								   };

			// When
			var target = new DateFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedDateFormat, result.Format);
		}

		[Test]
		[TestCase("", TimeFormats.Default)]
		[TestCase("t", TimeFormats.ShortTime)]
		[TestCase("T", TimeFormats.LongTime)]
		public void ShouldBuildTimeFormatTemplate(string timeFormat, TimeFormats expectedTimeFormat)
		{
			// Given
			var reportObject = new global::FastReport.Format.TimeFormat
								   {
									   Format = timeFormat
								   };

			// When
			var target = new TimeFormatTemplateBuilder();
			var result = target.BuildTemplate(_context, reportObject);

			// Then
			Assert.IsNotNull(result);
			Assert.AreEqual(expectedTimeFormat, result.Format);
		}

		[Test]
		public void ShouldBuildFormatTemplate()
		{
			// Given

			_context.RegisterBuilder(new BooleanFormatTemplateBuilder());
			_context.RegisterBuilder(new NumberFormatTemplateBuilder());
			_context.RegisterBuilder(new PercentFormatTemplateBuilder());
			_context.RegisterBuilder(new CurrencyFormatTemplateBuilder());
			_context.RegisterBuilder(new CustomFormatTemplateBuilder());
			_context.RegisterBuilder(new DateFormatTemplateBuilder());
			_context.RegisterBuilder(new TimeFormatTemplateBuilder());

			var reportObject1 = new global::FastReport.Format.BooleanFormat();
			var reportObject2 = new global::FastReport.Format.NumberFormat();
			var reportObject3 = new global::FastReport.Format.PercentFormat();
			var reportObject4 = new global::FastReport.Format.CurrencyFormat();
			var reportObject5 = new global::FastReport.Format.CustomFormat();
			var reportObject6 = new global::FastReport.Format.DateFormat();
			var reportObject7 = new global::FastReport.Format.TimeFormat();

			// When
			var target = new FormatTemplateBuilder();
			var result1 = target.BuildTemplate(_context, reportObject1);
			var result2 = target.BuildTemplate(_context, reportObject2);
			var result3 = target.BuildTemplate(_context, reportObject3);
			var result4 = target.BuildTemplate(_context, reportObject4);
			var result5 = target.BuildTemplate(_context, reportObject5);
			var result6 = target.BuildTemplate(_context, reportObject6);
			var result7 = target.BuildTemplate(_context, reportObject7);

			// Then
			Assert.IsNotNull(result1);
			Assert.IsNotNull(result2);
			Assert.IsNotNull(result3);
			Assert.IsNotNull(result4);
			Assert.IsNotNull(result5);
			Assert.IsNotNull(result6);
			Assert.IsNotNull(result7);
		}
	}
}