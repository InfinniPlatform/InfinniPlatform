using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class ConstantBindBuilder : IReportObjectBuilder<ConstantBind>
	{
		public void BuildObject(IReportObjectBuilderContext context, ConstantBind template, object parent)
		{
			string expression = null;

			if (template.Value != null)
			{
				var stringValue = template.Value as string;

				expression = stringValue ?? string.Format("[{0}]", template.Value);
			}

			DataBindHelper.SetDataBind(parent, expression);
		}
	}
}