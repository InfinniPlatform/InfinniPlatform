using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Bands
{
	sealed class ReportGroupBandBuilder : IReportObjectBuilder<ReportGroupBand>
	{
		public void BuildObject(IReportObjectBuilderContext context, ReportGroupBand template, object parent)
		{
			var dataBand = (DataBand)parent;

			// Создание блоков с заголовком и итогами группы
			var groupHeaderBand = context.CreateObject<GroupHeaderBand>();
			var groupFooterBand = context.CreateObject<GroupFooterBand>();
			groupHeaderBand.GroupFooter = groupFooterBand;

			// Заворачивание блока данных в группу
			var dataBandContainer = dataBand.Parent;
			dataBand.Parent = groupHeaderBand;
			groupHeaderBand.Parent = dataBandContainer;

			// Настройка условия группировки
			context.BuildObject(template.DataBind, groupHeaderBand);

			// Настройка отображения блоков группы
			context.BuildObject(template.Header, groupHeaderBand);
			context.BuildObject(template.Footer, groupFooterBand);
		}
	}
}