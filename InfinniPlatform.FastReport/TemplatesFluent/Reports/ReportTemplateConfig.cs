using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Print;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.FastReport.TemplatesFluent.Bands;
using InfinniPlatform.FastReport.TemplatesFluent.Data;
using InfinniPlatform.FastReport.TemplatesFluent.Print;

namespace InfinniPlatform.FastReport.TemplatesFluent.Reports
{
	/// <summary>
	/// Интерфейс для настройки шаблона отчета.
	/// </summary>
	public sealed class ReportTemplateConfig
	{
		internal ReportTemplateConfig(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			_report = new ReportTemplate
				          {
					          Info = new ReportInfo
						                 {
							                 Name = name
						                 }
				          };
		}


		private readonly ReportTemplate _report;


		/// <summary>
		/// Информация об отчете.
		/// </summary>
		public ReportTemplateConfig Info(Action<ReportInfoConfig> action)
		{
			var configuration = new ReportInfoConfig(_report.Info);
			action(configuration);

			return this;
		}


		/// <summary>
		/// Параметры отчета.
		/// </summary>
		public ReportTemplateConfig Parameters(Action<ParametersConfig> action)
		{
			if (_report.Parameters == null)
			{
				_report.Parameters = new List<ParameterInfo>();
			}

			var configuration = new ParametersConfig(_report.Parameters);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Источники данных.
		/// </summary>
		public ReportTemplateConfig DataSources(Action<DataSourcesConfig> action)
		{
			if (_report.DataSources == null)
			{
				_report.DataSources = new List<DataSourceInfo>();
			}

			var configuration = new DataSourcesConfig(_report.DataSources);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Итоги отчета.
		/// </summary>
		public ReportTemplateConfig Totals(string dataBandName, Action<TotalsConfig> action)
		{
			if (_report.Totals == null)
			{
				_report.Totals = new List<TotalInfo>();
			}

			var configuration = new TotalsConfig(dataBandName, _report.Totals);
			action(configuration);

			return this;
		}


		/// <summary>
		/// Настройки печати.
		/// </summary>
		public ReportTemplateConfig PrintSetup(Action<PrintSetupConfig> action)
		{
			if (_report.PrintSetup == null)
			{
				_report.PrintSetup = new PrintSetup();
			}

			var configuration = new PrintSetupConfig(_report.PrintSetup);
			action(configuration);

			return this;
		}


		/// <summary>
		/// Страница отчета
		/// </summary>
		public ReportTemplateConfig Page(Action<ReportPageBandConfig> action)
		{
			if (_report.Page == null)
			{
				_report.Page = new ReportPageBand();
			}

			var configuration = new ReportPageBandConfig(_report.Page);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Блок данных.
		/// </summary>
		public ReportTemplateConfig Data(string dataSourceName, Action<ReportDataBandConfig> action)
		{
			return Data(dataSourceName, string.Empty, action);
		}

		/// <summary>
		/// Блок данных.
		/// </summary>
		public ReportTemplateConfig Data(string dataSourceName, string collectionPath, Action<ReportDataBandConfig> action)
		{
			if (_report.Data == null)
			{
				_report.Data = new ReportDataBand();
			}

			var configuration = new ReportDataBandConfig(dataSourceName, collectionPath, _report.Data);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Заголовок отчета.
		/// </summary>
		public ReportTemplateConfig Title(Action<ReportBandConfig> action)
		{
			if (_report.Title == null)
			{
				_report.Title = new ReportBand();
			}

			var configuration = new ReportBandConfig(_report.Title);
			action(configuration);

			return this;
		}

		/// <summary>
		/// Итоги отчета.
		/// </summary>
		public ReportTemplateConfig Summary(Action<ReportBandConfig> action)
		{
			if (_report.Summary == null)
			{
				_report.Summary = new ReportBand();
			}

			var configuration = new ReportBandConfig(_report.Summary);
			action(configuration);

			return this;
		}


		/// <summary>
		/// Построить шаблон отчета.
		/// </summary>
		public ReportTemplate BuildTemplate()
		{
			return _report;
		}
	}
}