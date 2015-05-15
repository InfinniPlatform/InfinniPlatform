using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class BooleanFormatTemplateBuilder : IReportObjectTemplateBuilder<BooleanFormat>
	{
		public BooleanFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.BooleanFormat)reportObject;

			return new BooleanFormat
					   {
						   FalseText = format.FalseText,
						   TrueText = format.TrueText
					   };
		}
	}
}