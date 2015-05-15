using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class TimeFormatBuilder : IReportObjectBuilder<TimeFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, TimeFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			var format = new global::FastReport.Format.TimeFormat();

			switch (template.Format)
			{
				case TimeFormats.LongTime:
					format.Format = "T";
					break;
				case TimeFormats.ShortTime:
					format.Format = "t";
					break;
				default:
					format.Format = "t";
					break;
			}

			element.Format = format;
		}
	}
}