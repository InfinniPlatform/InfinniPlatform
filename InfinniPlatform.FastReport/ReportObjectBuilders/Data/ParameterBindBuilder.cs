using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class ParameterBindBuilder : IReportObjectBuilder<ParameterBind>
	{
		public void BuildObject(IReportObjectBuilderContext context, ParameterBind template, object parent)
		{
			var expression = string.Format("[{0}]", template.Parameter);

			DataBindHelper.SetDataBind(parent, expression);
		}
	}
}