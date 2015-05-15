using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Bands
{
	sealed class ReportBandPrintSetupTemplateBuilder : IReportObjectTemplateBuilder<ReportBandPrintSetup>
	{
		public ReportBandPrintSetup BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var bandObject = (BandBase)reportObject;

			var printTargets = PrintTargets.None;

			if (bandObject.PrintOn.HasFlag(PrintOn.FirstPage))
			{
				printTargets |= PrintTargets.FirstPage;
			}

			if (bandObject.PrintOn.HasFlag(PrintOn.LastPage))
			{
				printTargets |= PrintTargets.LastPage;
			}

			if (bandObject.PrintOn.HasFlag(PrintOn.OddPages))
			{
				printTargets |= PrintTargets.OddPages;
			}

			if (bandObject.PrintOn.HasFlag(PrintOn.EvenPages))
			{
				printTargets |= PrintTargets.EvenPages;
			}

			return new ReportBandPrintSetup
					   {
						   IsStartNewPage = bandObject.StartNewPage,
						   PrintTargets = printTargets
					   };
		}
	}
}