using System.Collections.Generic;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Reporting.ParameterValueProviders;

namespace InfinniPlatform.Reporting.Services
{
    /// <summary>
    /// Фабрика для создания инфраструктурных сервисов подсистемы отчетов.
    /// </summary>
    internal sealed class ReportServiceFactory : IReportServiceFactory
    {
        public ReportServiceFactory(IEnumerable<IDataSource> dataSources)
        {
            var reportBuilder = new FrReportBuilder();
            var reportDataSource = new GenericDataSource();
            var parameterValueProvider = new GenericParameterValueProvider();

            foreach (var dataSource in dataSources)
            {
                reportDataSource.RegisterDataSource(dataSource);
            }

            parameterValueProvider.RegisterProvider<ParameterConstantValueProviderInfo>(new ParameterConstantValueProvider());
            parameterValueProvider.RegisterProvider<ParameterDataSourceValueProviderInfo>(new ParameterDataSourceValueProvider(reportDataSource));

            _reportService = new ReportService(reportBuilder, reportDataSource, parameterValueProvider);
        }

        private readonly IReportService _reportService;

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
            return null;
        }
    }
}