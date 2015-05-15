using System;

using FastReport;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	sealed class TotalBindTemplateBuilder : IReportObjectTemplateBuilder<TotalBind>
	{
		public TotalBind BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var report = (Report)context.Report;
			var expression = (string)reportObject ?? string.Empty;

			var total = expression.TrimStart('[').TrimEnd(']');

			if (report.Dictionary.Totals.FindByName(total) == null)
			{
				throw new InvalidOperationException(string.Format(Resources.ExpressionReferencedOnUnknownTotal, expression, total));
			}

			return new TotalBind
					   {
						   Total = total
					   };
		}
	}
}