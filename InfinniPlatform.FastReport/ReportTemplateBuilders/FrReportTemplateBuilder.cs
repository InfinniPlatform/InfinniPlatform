using InfinniPlatform.FastReport.ReportTemplateBuilders.Bands;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Borders;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Data;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Elements;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Format;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Print;
using InfinniPlatform.FastReport.ReportTemplateBuilders.Reports;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders
{
	/// <summary>
	/// Предоставляет методы для формирования шаблона отчета по объекту отчета FastReport.
	/// </summary>
	public sealed class FrReportTemplateBuilder : IReportTemplateBuilder
	{
		public ReportTemplate Build(object reportObject)
		{
			// Создание контекста

			var context = new FrReportObjectTemplateBuilderContext
							  {
								  Report = reportObject
							  };

			// Регистрация построителей

			context.RegisterBuilder(new ReportBandTemplateBuilder());
			context.RegisterBuilder(new ReportBandPrintSetupTemplateBuilder());
			context.RegisterBuilder(new ReportPageBandTemplateBuilder());
			context.RegisterBuilder(new ReportDataBandTemplateBuilder());
			context.RegisterBuilder(new ReportGroupBandTemplateBuilder());

			context.RegisterBuilder(new BorderTemplateBuilder());

			context.RegisterBuilder(new ConstantBindTemplateBuilder());
			context.RegisterBuilder(new ParameterBindTemplateBuilder());
			context.RegisterBuilder(new TotalBindTemplateBuilder());
			context.RegisterBuilder(new PropertyBindTemplateBuilder());
			context.RegisterBuilder(new CollectionBindTemplateBuilder());
			context.RegisterBuilder(new DataBindTemplateBuilder());

			context.RegisterBuilder(new ElementLayoutTemplateBuilder());
			context.RegisterBuilder(new TextElementStyleTemplateBuilder());
			context.RegisterBuilder(new TextElementTemplateBuilder());
			context.RegisterBuilder(new TableElementTemplateBuilder());
			context.RegisterBuilder(new ElementTemplateBuilder());

			context.RegisterBuilder(new BooleanFormatTemplateBuilder());
			context.RegisterBuilder(new NumberFormatTemplateBuilder());
			context.RegisterBuilder(new PercentFormatTemplateBuilder());
			context.RegisterBuilder(new CurrencyFormatTemplateBuilder());
			context.RegisterBuilder(new DateFormatTemplateBuilder());
			context.RegisterBuilder(new TimeFormatTemplateBuilder());
			context.RegisterBuilder(new CustomFormatTemplateBuilder());
			context.RegisterBuilder(new FormatTemplateBuilder());

			context.RegisterBuilder(new PrintSetupTemplateBuilder());

			context.RegisterBuilder(new ReportTemplateBuilder());

			// Построение шаблона отчета

			return context.BuildTemplate<ReportTemplate>(reportObject);
		}
	}
}