using FastReport;
using FastReport.Utils;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Bands
{
	sealed class ReportBandBuilder : IReportObjectBuilder<ReportBand>
	{
		public void BuildObject(IReportObjectBuilderContext context, ReportBand template, object parent)
		{
			var band = (BandBase)parent;

			// Установка наименование блока
			if (string.IsNullOrWhiteSpace(template.Name) == false)
			{
				band.Name = template.Name;
			}

			// Настройка общих свойств блока
			band.Height = template.Height * Units.Millimeters;
			band.CanShrink = template.CanShrink;
			band.CanGrow = template.CanGrow;
			band.CanBreak = true;

			// Настройка границ блока
			context.BuildObject(template.Border, band);

			// Настройка свойств печати блока
			context.BuildObject(template.PrintSetup, band);

			// Построение вложенных элементов блока
			context.BuildObjects(template.Elements, band);
		}
	}
}