using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Reporting;
using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Reports;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Reporting.ParameterValueProviders;
using InfinniPlatform.Reporting.Properties;

namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Сервис для работы с подсистемой отчетов.
	/// </summary>
	sealed class ReportService : IReportService
	{
		public ReportService(IReportBuilder reportBuilder, IDataSource reportDataSource, IParameterValueProvider parameterValueProvider)
		{
			if (reportBuilder == null)
			{
				throw new ArgumentNullException("reportBuilder");
			}

			if (reportDataSource == null)
			{
				throw new ArgumentNullException("reportDataSource");
			}

			if (parameterValueProvider == null)
			{
				throw new ArgumentNullException("parameterValueProvider");
			}



			_reportBuilder = reportBuilder;
			_reportDataSource = reportDataSource;
			_parameterValueProvider = parameterValueProvider;
		}


		private readonly IReportBuilder _reportBuilder;
		private readonly IDataSource _reportDataSource;
		private readonly IParameterValueProvider _parameterValueProvider;


		/// <summary>
		/// Получить значения параметра отчета.
		/// </summary>
		/// <param name="template">Шаблон отчета.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns>Значения параметров отчета.</returns>
		public IDictionary<string, ParameterValues> GetParameterValues(ReportTemplate template)
		{
			if (template == null)
			{
				throw new ArgumentNullException("template");
			}

			var result = new Dictionary<string, ParameterValues>();

			if (template.Parameters != null)
			{
				var dataSources = template.DataSources;

				foreach (var parameterInfo in template.Parameters)
				{
					var parameterName = parameterInfo.Name;
					var parameterValues = new ParameterValues();

					if (parameterInfo.AvailableValues != null)
					{
						parameterValues.AvailableValues = _parameterValueProvider.GetParameterValues(parameterInfo.AvailableValues, dataSources);
					}

					if (parameterInfo.DefaultValues != null)
					{
						parameterValues.DefaultValues = _parameterValueProvider.GetParameterValues(parameterInfo.DefaultValues, dataSources);
					}

					result.Add(parameterName, parameterValues);
				}
			}

			return result;
		}




		/// <summary>
		/// Создать файл отчета.
		/// </summary>
		/// <param name="template">Шаблон отчета.</param>
		/// <param name="parameterValues">Значения параметров отчета.</param>
		/// <param name="fileFormat">Формат файла отчета.</param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <returns>Файл отчета.</returns>
		public byte[] CreateReportFile(ReportTemplate template, IDictionary<string, object> parameterValues = null, ReportFileFormat fileFormat = ReportFileFormat.Pdf)
		{
			if (template == null)
			{
				throw new ArgumentNullException("template");
			}

			using (var report = _reportBuilder.Build(template))
			{
				SetParameters(report, template, parameterValues);
				SetDataSources(report, template, parameterValues);

				return report.CreateFile(fileFormat);
			}
		}


		private static void SetParameters(IReport report, ReportTemplate template, IDictionary<string, object> parameterValues)
		{
			if (template.Parameters != null)
			{
				foreach (var parameterInfo in template.Parameters)
				{
					var parameterName = parameterInfo.Name;

					object parameterValue = null;

					if (parameterValues != null)
					{
						parameterValues.TryGetValue(parameterName, out parameterValue);
					}

					report.SetParameter(parameterName, parameterValue);
				}
			}
		}

		private void SetDataSources(IReport report, ReportTemplate template, IDictionary<string, object> parameterValues)
		{
			if (template.DataSources != null)
			{
				foreach (var dataSourceInfo in template.DataSources)
				{
					var data = _reportDataSource.GetData(dataSourceInfo, template.Parameters, parameterValues);

					report.SetDataSource(dataSourceInfo.Name, data);
				}
			}
		}
	}
}