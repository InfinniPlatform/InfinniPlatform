using System.Collections.Generic;
using System.Linq;

using FastReport;
using FastReport.Data;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Data
{
	sealed class TotalInfoBuilder : IReportObjectBuilder<TotalInfo>
	{
		public void BuildObject(IReportObjectBuilderContext context, TotalInfo template, object parent)
		{
			var report = (Report)parent;
			var reportPage = (ReportPage)report.Pages[0];

			var dataBand = FindDataBand(reportPage, template.DataBand);
			var printBand = FindPrintBand(reportPage, template.PrintBand);

			var total = new Total
			{
				PrintOn = printBand,
				Evaluator = dataBand,
				Name = template.Name,
				TotalType = TotalFunctions[template.TotalFunc]
			};

			report.Dictionary.Totals.Add(total);

			context.BuildObject(template.Expression, total);
		}

		private static DataBand FindDataBand(ReportPage reportPage, string dataBandName)
		{
			return reportPage.AllObjects.OfType<DataBand>().FirstOrDefault(i => i.Name == dataBandName);
		}

		private static BandBase FindPrintBand(ReportPage reportPage, string printBandName)
		{
			return reportPage.AllObjects.OfType<BandBase>().FirstOrDefault(i => i.Name == printBandName);
		}

		private static readonly Dictionary<TotalFunc, TotalType> TotalFunctions
			= new Dictionary<TotalFunc, TotalType>
				  {
					  { TotalFunc.Sum, TotalType.Sum },
					  { TotalFunc.Min, TotalType.Min },
					  { TotalFunc.Max, TotalType.Max },
					  { TotalFunc.Avg, TotalType.Avg },
					  { TotalFunc.Count, TotalType.Count },
				  };
	}
}