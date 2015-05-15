using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class TotalBindBuilder : IReportObjectBuilder<TotalBind>
	{
		public void BuildObject(IReportObjectBuilderContext context, TotalBind template, object parent)
		{
			var expression = string.Format("[{0}]", template.Total);

			DataBindHelper.SetDataBind(parent, expression);
		}
	}
}