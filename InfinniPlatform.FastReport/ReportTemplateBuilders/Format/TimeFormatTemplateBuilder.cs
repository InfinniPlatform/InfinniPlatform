using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class TimeFormatTemplateBuilder : IReportObjectTemplateBuilder<TimeFormat>
	{
		public TimeFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.TimeFormat)reportObject;

			var result = new TimeFormat();

			switch (format.Format)
			{
				case "T":
					result.Format = TimeFormats.LongTime;
					break;
				case "t":
					result.Format = TimeFormats.ShortTime;
					break;
				default:
					result.Format = TimeFormats.Default;
					break;
			}

			return result;
		}
	}
}