using InfinniPlatform.FastReport.ReportObjectBuilders.Bands;
using InfinniPlatform.FastReport.ReportObjectBuilders.Borders;
using InfinniPlatform.FastReport.ReportObjectBuilders.Data;
using InfinniPlatform.FastReport.ReportObjectBuilders.Elements;
using InfinniPlatform.FastReport.ReportObjectBuilders.Format;
using InfinniPlatform.FastReport.ReportObjectBuilders.Print;
using InfinniPlatform.FastReport.ReportObjectBuilders.Reports;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Фабрика по умолчанию для создания контекста построителя объекта отчета FastReport.
	/// </summary>
	public sealed class FrReportObjectBuilderContextFactory : IReportObjectBuilderContextFactory
	{
		public IReportObjectBuilderContext CreateContext()
		{
			// Создание контекста

			var context = new FrReportObjectBuilderContext();

			// Регистрация построителей

			context.RegisterBuilder(new ReportBandBuilder());
			context.RegisterBuilder(new ReportBandPrintSetupBuilder());
			context.RegisterBuilder(new ReportPageBandBuilder());
			context.RegisterBuilder(new ReportDataBandBuilder());
			context.RegisterBuilder(new ReportGroupBandBuilder());

			context.RegisterBuilder(new BorderBuilder());

			context.RegisterBuilder(new ParameterInfoBuilder());
			context.RegisterBuilder(new DataSourceInfoBuilder());
			context.RegisterBuilder(new TotalInfoBuilder());

			context.RegisterBuilder(new ConstantBindBuilder());
			context.RegisterBuilder(new ParameterBindBuilder());
			context.RegisterBuilder(new TotalBindBuilder());
			context.RegisterBuilder(new PropertyBindBuilder());
			context.RegisterBuilder(new CollectionBindBuilder());

			context.RegisterBuilder(new ElementLayoutBuilder());
			context.RegisterBuilder(new TextElementStyleBuilder());
			context.RegisterBuilder(new TextElementBuilder());
			context.RegisterBuilder(new TableElementBuilder());

			context.RegisterBuilder(new BooleanFormatBuilder());
			context.RegisterBuilder(new NumberFormatBuilder());
			context.RegisterBuilder(new CurrencyFormatBuilder());
			context.RegisterBuilder(new PercentFormatBuilder());
			context.RegisterBuilder(new DateFormatBuilder());
			context.RegisterBuilder(new TimeFormatBuilder());
			context.RegisterBuilder(new CustomFormatBuilder());

			context.RegisterBuilder(new PrintSetupBuilder());

			context.RegisterBuilder(new ReportObjectBuilder());

			return context;
		}
	}
}