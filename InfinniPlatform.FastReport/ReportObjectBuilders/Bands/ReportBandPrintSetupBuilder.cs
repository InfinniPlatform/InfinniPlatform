using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Bands
{
	sealed class ReportBandPrintSetupBuilder : IReportObjectBuilder<ReportBandPrintSetup>
	{
		public void BuildObject(IReportObjectBuilderContext context, ReportBandPrintSetup template, object parent)
		{
			var band = (BandBase)parent;

			band.StartNewPage = template.IsStartNewPage;
			band.PrintOn = PrintOn.None;

			if (template.PrintTargets.HasFlag(PrintTargets.FirstPage))
			{
				band.PrintOn |= PrintOn.FirstPage;
			}

			if (template.PrintTargets.HasFlag(PrintTargets.LastPage))
			{
				band.PrintOn |= PrintOn.LastPage;
			}

			if (template.PrintTargets.HasFlag(PrintTargets.OddPages))
			{
				band.PrintOn |= PrintOn.OddPages;
			}

			if (template.PrintTargets.HasFlag(PrintTargets.EvenPages))
			{
				band.PrintOn |= PrintOn.EvenPages;
			}
		}
	}
}