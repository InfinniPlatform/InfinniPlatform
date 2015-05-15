using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class NumberFormatTemplateBuilder : IReportObjectTemplateBuilder<NumberFormat>
	{
		public NumberFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.NumberFormat)reportObject;

			return new NumberFormat
					   {
						   DecimalDigits = format.DecimalDigits,
						   GroupSeparator = format.GroupSeparator,
						   DecimalSeparator = format.DecimalSeparator
					   };
		}
	}
}