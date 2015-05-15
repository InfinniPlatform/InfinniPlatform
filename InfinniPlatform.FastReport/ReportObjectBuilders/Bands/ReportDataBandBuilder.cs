using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Bands
{
	sealed class ReportDataBandBuilder : IReportObjectBuilder<ReportDataBand>
	{
		public void BuildObject(IReportObjectBuilderContext context, ReportDataBand template, object parent)
		{
			var reportPage = (IParent)parent;

			// Создание блока данных
			var dataBand = context.CreateObject<DataBand>();
			reportPage.AddChild(dataBand);

			// Установка источника данных
			context.BuildObject(template.DataBind, dataBand);

			// Настройка отображения блока данных
			context.BuildObject(template.Content, dataBand);

			// Построение вложенного блока данных
			context.BuildObject(template.Details, dataBand);

			// Группировки блока данных в порядке объявления групп
			context.BuildObjects(template.Groups, dataBand);
		}
	}
}