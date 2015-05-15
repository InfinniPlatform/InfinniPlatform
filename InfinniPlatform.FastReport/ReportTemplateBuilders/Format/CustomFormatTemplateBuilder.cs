using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class CustomFormatTemplateBuilder : IReportObjectTemplateBuilder<CustomFormat>
	{
		public CustomFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var format = (global::FastReport.Format.CustomFormat)reportObject;

			return new CustomFormat
					   {
						   Format = format.Format
					   };
		}
	}
}