using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class DateFormatTemplateBuilder : IReportObjectTemplateBuilder<DateFormat>
	{
		public DateFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.DateFormat)reportObject;

			var result = new DateFormat();

			switch (format.Format)
			{
				case "D":
					result.Format = DateFormats.LongDate;
					break;
				case "d":
					result.Format = DateFormats.ShortDate;
					break;
				default:
					result.Format = DateFormats.Default;
					break;
			}

			return result;
		}
	}
}