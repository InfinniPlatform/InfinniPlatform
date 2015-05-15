using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	sealed class ConstantBindTemplateBuilder : IReportObjectTemplateBuilder<ConstantBind>
	{
		public ConstantBind BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			return new ConstantBind
					   {
						   Value = reportObject
					   };
		}
	}
}