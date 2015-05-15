using FastReport;

using InfinniPlatform.FastReport.Templates.Data;

using SortOrder = InfinniPlatform.FastReport.Templates.Data.SortOrder;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class CollectionBindBuilder : IReportObjectBuilder<CollectionBind>
	{
		public void BuildObject(IReportObjectBuilderContext context, CollectionBind template, object parent)
		{
			var dataBand = (DataBand)parent;

			dataBand.DataSource = PropertyMapper.FindDataSource(dataBand.Report, template.DataSource, template.Property);

			if (dataBand.DataSource != null && template.SortFields != null)
			{
				foreach (var sortField in template.SortFields)
				{
					var expression = PropertyMapper.GetPropertyExpression(template.DataSource, sortField.Property);
					dataBand.Sort.Add(new Sort(expression, sortField.SortOrder == SortOrder.Descending));
				}
			}
		}
	}
}