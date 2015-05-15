using System;

using FastReport;

using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Предоставляет методы для формирования отчета FastReport.
	/// </summary>
	public sealed class FrReportBuilder : IReportBuilder
	{
		public FrReportBuilder()
			: this(new FrReportObjectBuilderContextFactory())
		{
		}

		public FrReportBuilder(IReportObjectBuilderContextFactory contextFactory)
		{
			if (contextFactory == null)
			{
				throw new ArgumentNullException("contextFactory");
			}

			_contextFactory = contextFactory;
		}


		private readonly IReportObjectBuilderContextFactory _contextFactory;


		public IReport Build(ReportTemplate template)
		{
			var context = _contextFactory.CreateContext();

			context.BuildObject(template, null);

			return new FrReport((Report)context.Report);
		}
	}
}