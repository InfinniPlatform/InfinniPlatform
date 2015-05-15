using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class PercentFormatTemplateBuilder : IReportObjectTemplateBuilder<PercentFormat>
	{
		public PercentFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.PercentFormat)reportObject;

			return new PercentFormat
					   {
						   DecimalDigits = format.DecimalDigits,
						   GroupSeparator = format.GroupSeparator,
						   DecimalSeparator = format.DecimalSeparator,
						   PercentSymbol = format.PercentSymbol
					   };
		}
	}
}