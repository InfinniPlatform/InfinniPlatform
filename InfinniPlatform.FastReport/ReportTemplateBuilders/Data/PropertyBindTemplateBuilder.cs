using FastReport;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	sealed class PropertyBindTemplateBuilder : IReportObjectTemplateBuilder<PropertyBind>
	{
		public PropertyBind BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			return DataSourceBindHelper.GetPropertyBind((Report)context.Report, (string)reportObject);
		}
	}
}