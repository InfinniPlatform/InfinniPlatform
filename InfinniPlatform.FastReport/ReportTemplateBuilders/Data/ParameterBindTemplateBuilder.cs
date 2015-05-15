using System;

using FastReport;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	sealed class ParameterBindTemplateBuilder : IReportObjectTemplateBuilder<ParameterBind>
	{
		public ParameterBind BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var report = (Report)context.Report;
			var expression = (string)reportObject ?? string.Empty;

			var parameter = expression.TrimStart('[').TrimEnd(']');

			if (report.Dictionary.Parameters.FindByName(parameter) == null)
			{
				throw new InvalidOperationException(string.Format(Resources.ExpressionReferencedOnUnknownParameter, expression, parameter));
			}

			return new ParameterBind
					   {
						   Parameter = parameter
					   };
		}
	}
}