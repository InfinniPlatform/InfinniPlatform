using System;

using FastReport;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Data
{
	sealed class DataBindTemplateBuilder : IReportObjectTemplateBuilder<IDataBind>
	{
		public IDataBind BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			// Todo: Этот класс потребует доработки синтаксического анализа входящего выражения

			IDataBind result;

			var report = (Report)context.Report;
			var expression = (string)reportObject ?? string.Empty;

			try
			{
				if (expression.StartsWith("[") && expression.EndsWith("]"))
				{
					var expressionBody = expression.Substring(1, expression.Length - 2);

					// Если найдена ссылка на параметр отчета
					if (report.Dictionary.Parameters.FindByName(expressionBody) != null)
					{
						result = context.BuildTemplate<ParameterBind>(reportObject);
					}
					// Если найдена ссылка на итог отчета
					else if (report.Dictionary.Totals.FindByName(expressionBody) != null)
					{
						result = context.BuildTemplate<TotalBind>(reportObject);
					}
					// Попытка найти ссылку на свойство источника данных
					else
					{
						result = context.BuildTemplate<PropertyBind>(reportObject);
					}
				}
				else
				{
					// Во всех непонятных ситуациях считаем, что дано выражение
					result = context.BuildTemplate<ConstantBind>(reportObject);
				}
			}
			catch (Exception)
			{
				// Во всех непонятных ситуациях считаем, что дано выражение
				result = context.BuildTemplate<ConstantBind>(reportObject);
			}

			return result;
		}
	}
}