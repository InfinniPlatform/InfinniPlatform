using System;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Format
{
	sealed class FormatTemplateBuilder : IReportObjectTemplateBuilder<IFormat>
	{
		public IFormat BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			if (reportObject is global::FastReport.Format.BooleanFormat)
			{
				return context.BuildTemplate<BooleanFormat>(reportObject);
			}

			if (reportObject is global::FastReport.Format.NumberFormat)
			{
				return context.BuildTemplate<NumberFormat>(reportObject);
			}

			if (reportObject is global::FastReport.Format.PercentFormat)
			{
				return context.BuildTemplate<PercentFormat>(reportObject);
			}

			if (reportObject is global::FastReport.Format.CurrencyFormat)
			{
				return context.BuildTemplate<CurrencyFormat>(reportObject);
			}

			if (reportObject is global::FastReport.Format.DateFormat)
			{
				return context.BuildTemplate<DateFormat>(reportObject);
			}

			if (reportObject is global::FastReport.Format.TimeFormat)
			{
				return context.BuildTemplate<TimeFormat>(reportObject);
			}

			if (reportObject is global::FastReport.Format.CustomFormat)
			{
				return context.BuildTemplate<CustomFormat>(reportObject);
			}

			return null;
		}
	}
}