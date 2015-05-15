using FastReport;
using FastReport.Utils;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Bands
{
	sealed class ReportBandTemplateBuilder : IReportObjectTemplateBuilder<ReportBand>
	{
		public ReportBand BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var bandObject = (BandBase)reportObject;

			return new ReportBand
					   {
						   Name = bandObject.Name,
						   Height = bandObject.Height / Units.Millimeters,
						   CanGrow = bandObject.CanGrow,
						   CanShrink = bandObject.CanShrink,

						   Border = context.BuildTemplate<Templates.Borders.Border>(bandObject.Border),
						   PrintSetup = context.BuildTemplate<ReportBandPrintSetup>(bandObject),
						   Elements = context.BuildTemplates<IElement>(bandObject.ChildObjects)
					   };
		}
	}
}