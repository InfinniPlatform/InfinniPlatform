using System;

using FastReport;
using FastReport.Table;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Elements
{
	sealed class ElementTemplateBuilder : IReportObjectTemplateBuilder<IElement>
	{
		public IElement BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			if (reportObject is TextObject)
			{
				return context.BuildTemplate<TextElement>(reportObject);
			}

			if (reportObject is TableObject)
			{
				return context.BuildTemplate<TableElement>(reportObject);
			}

			if (reportObject is BandBase == false)
			{
				throw new NotSupportedException(string.Format(Resources.ReportObjectIsNotSupported, reportObject.GetType().FullName));
			}

			return null;
		}
	}
}