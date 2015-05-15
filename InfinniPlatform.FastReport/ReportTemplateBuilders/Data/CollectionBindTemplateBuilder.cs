using System;
using System.Collections.Generic;

using FastReport;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Data;

using SortOrder = InfinniPlatform.FastReport.Templates.Data.SortOrder;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	sealed class CollectionBindTemplateBuilder : IReportObjectTemplateBuilder<CollectionBind>
	{
		public CollectionBind BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var dataBand = (DataBand)reportObject;

			if (dataBand.DataSource == null)
			{
				throw new InvalidOperationException(Resources.DataBandShouldReferencedOnDataSource);
			}

			var result = DataSourceBindHelper.GetCollectionBind(dataBand.DataSource);

			if (dataBand.Sort != null)
			{
				var sortFields = new List<SortField>();

				foreach (Sort sort in dataBand.Sort)
				{
					var property = DataSourceBindHelper.GetPropertyBind((Report)context.Report, sort.Expression);

					if (property.DataSource != result.DataSource)
					{
						throw new InvalidOperationException(string.Format(Resources.SortFieldShouldReferencedOnDataSource, sort.Expression, result.DataSource));
					}

					sortFields.Add(new SortField
									   {
										   Property = property.Property,
										   SortOrder = sort.Descending ? SortOrder.Descending : SortOrder.Ascending
									   });
				}

				result.SortFields = sortFields;
			}

			return result;
		}
	}
}