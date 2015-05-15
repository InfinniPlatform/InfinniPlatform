using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class ParameterInfoBuilder : IReportObjectBuilder<ParameterInfo>
	{
		public void BuildObject(IReportObjectBuilderContext context, ParameterInfo template, object parent)
		{
			var report = (Report)parent;

			report.Parameters.Add(new Parameter(template.Name)
									  {
										  Description = template.Caption,
										  DataType = template.Type.ToClrType()
									  });
		}
	}
}