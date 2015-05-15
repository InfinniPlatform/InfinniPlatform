using System;

using InfinniPlatform.Factories;
using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Reporting.ParameterValueProviders;

namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Фабрика для создания инфраструктурных сервисов подсистемы отчетов.
	/// </summary>
	public sealed class ReportServiceFactory : IReportServiceFactory
	{
		public ReportServiceFactory()
		{
			var reportBuilder = new FrReportBuilder();
			var reportDataSource = new GenericDataSource();
			var parameterValueProvider = new GenericParameterValueProvider();

			reportDataSource.RegisterDataSource<SqlDataProviderInfo>(new SqlDataSource());
			reportDataSource.RegisterDataSource<RestDataProviderInfo>(new RestDataSource());
			reportDataSource.RegisterDataSource<RegisterDataProviderInfo>(new RegisterDataSource());

			parameterValueProvider.RegisterProvider<ParameterConstantValueProviderInfo>(new ParameterConstantValueProvider());
			parameterValueProvider.RegisterProvider<ParameterDataSourceValueProviderInfo>(new ParameterDataSourceValueProvider(reportDataSource));

			_reportService = new ReportService(reportBuilder, reportDataSource, parameterValueProvider);
		}


		private readonly IReportService _reportService;
		private readonly IReportTemplateRepository _reportTemplateRepository;


		/// <summary>
		/// Создать сервис для работы с подсистемой отчетов.
		/// </summary>
		public IReportService CreateReportService()
		{
			return _reportService;
		}

		/// <summary>
		/// Создать сервис для работы с хранилищем шаблонов отчетов.
		/// </summary>
		public IReportTemplateRepository CreateReportTemplateRepository()
		{
			return _reportTemplateRepository;
		}
	}
}