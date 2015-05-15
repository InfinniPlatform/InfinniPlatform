using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class PropertyBindBuilder : IReportObjectBuilder<PropertyBind>
	{
		public void BuildObject(IReportObjectBuilderContext context, PropertyBind template, object parent)
		{
			var expression = PropertyMapper.GetPropertyExpression(template.DataSource, template.Property);

			DataBindHelper.SetDataBind(parent, expression);
		}
	}
}