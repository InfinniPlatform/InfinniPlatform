using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class NumberFormatBuilder : IReportObjectBuilder<NumberFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, NumberFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			element.Format = new global::FastReport.Format.NumberFormat
								 {
									 DecimalDigits = template.DecimalDigits,
									 GroupSeparator = template.GroupSeparator,
									 DecimalSeparator = template.DecimalSeparator
								 };
		}
	}
}