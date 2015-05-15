using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class PercentFormatBuilder : IReportObjectBuilder<PercentFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, PercentFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			element.Format = new global::FastReport.Format.PercentFormat
								 {
									 DecimalDigits = template.DecimalDigits,
									 GroupSeparator = template.GroupSeparator,
									 DecimalSeparator = template.DecimalSeparator,
									 PercentSymbol = template.PercentSymbol
								 };
		}
	}
}