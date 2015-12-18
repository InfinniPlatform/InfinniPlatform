using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Reporting.PrintView;
using InfinniPlatform.Reporting.Services;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Reporting.IoC
{
    internal sealed class ReportingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // PrintView (wkhtmltopdf)

            builder.RegisterType<FlowDocumentPrintViewBuilderFactory>()
                   .As<IPrintViewBuilderFactory>()
                   .SingleInstance();

            builder.RegisterType<PrintViewComponent>()
                   .As<IPrintViewComponent>()
                   .SingleInstance();

            // Report (FastReport)

            builder.RegisterType<SqlDataSource>()
                   .As<IDataSource>()
                   .SingleInstance();

            builder.RegisterType<RestDataSource>()
                   .As<IDataSource>()
                   .SingleInstance();

            builder.RegisterType<RegisterDataSource>()
                   .As<IDataSource>()
                   .SingleInstance();

            builder.RegisterType<ReportServiceFactory>()
                   .As<IReportServiceFactory>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IReportServiceFactory>().CreateReportService())
                   .As<IReportService>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<IReportServiceFactory>().CreateReportTemplateRepository())
                   .As<IReportTemplateRepository>()
                   .SingleInstance();
        }
    }
}