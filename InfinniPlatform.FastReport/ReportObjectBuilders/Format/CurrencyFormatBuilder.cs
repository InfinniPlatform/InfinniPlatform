using FastReport;

using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Format
{
	sealed class CurrencyFormatBuilder : IReportObjectBuilder<CurrencyFormat>
	{
		public void BuildObject(IReportObjectBuilderContext context, CurrencyFormat template, object parent)
		{
			var element = (TextObjectBase)parent;

			element.Format = new global::FastReport.Format.CurrencyFormat
								 {
									 DecimalDigits = template.DecimalDigits,
									 GroupSeparator = template.GroupSeparator,
									 DecimalSeparator = template.DecimalSeparator,
									 CurrencySymbol = template.CurrencySymbol
								 };
		}
	}
}