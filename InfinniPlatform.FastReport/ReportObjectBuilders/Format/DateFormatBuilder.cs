using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class DateFormatBuilder : IReportObjectBuilder<DateFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, DateFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			var format = new global::FastReport.Format.DateFormat();

			switch (template.Format)
			{
				case DateFormats.LongDate:
					format.Format = "D";
					break;
				case DateFormats.ShortDate:
					format.Format = "d";
					break;
				default:
					format.Format = "d";
					break;
			}

			element.Format = format;
		}
	}
}