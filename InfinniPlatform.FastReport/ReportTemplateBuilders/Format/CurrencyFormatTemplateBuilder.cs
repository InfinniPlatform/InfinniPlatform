using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class CurrencyFormatTemplateBuilder : IReportObjectTemplateBuilder<CurrencyFormat>
	{
		public CurrencyFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.CurrencyFormat)reportObject;

			return new CurrencyFormat
					   {
						   DecimalDigits = format.DecimalDigits,
						   GroupSeparator = format.GroupSeparator,
						   DecimalSeparator = format.DecimalSeparator,
						   CurrencySymbol = format.CurrencySymbol
					   };
		}
	}
}