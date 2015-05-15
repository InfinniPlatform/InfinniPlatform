using System;

using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.TemplatesFluent.Reports
{
	/// <summary>
	/// Интерфейс для настройки информации об отчете.
	/// </summary>
	public sealed class ReportInfoConfig
	{
		internal ReportInfoConfig(ReportInfo reportInfo)
		{
			if (reportInfo == null)
			{
				throw new ArgumentNullException("reportInfo");
			}

			_reportInfo = reportInfo;
		}


		private readonly ReportInfo _reportInfo;


		/// <summary>
		/// Наименование отчета.
		/// </summary>
		public ReportInfoConfig Name(string value)
		{
			_reportInfo.Name = value;

			return this;
		}

		/// <summary>
		/// Заголовок отчета.
		/// </summary>
		public ReportInfoConfig Caption(string value)
		{
			_reportInfo.Caption = value;

			return this;
		}

		/// <summary>
		/// Описание отчета.
		/// </summary>
		public ReportInfoConfig Description(string value)
		{
			_reportInfo.Description = value;

			return this;
		}
	}
}