using InfinniPlatform.Core.PrintView;
using InfinniPlatform.FlowDocument;
using InfinniPlatform.FlowDocument.PrintView;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Reporting.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Reporting.IoC
{
    internal sealed class ReportingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // PrintView (wkhtmltopdf)

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<PrintViewSettings>(PrintViewSettings.SectionName))
                   .As<PrintViewSettings>()
                   .SingleInstance();

            builder.RegisterType<FlowDocumentPrintViewConverter>()
                   .As<IFlowDocumentPrintViewConverter>()
                   .SingleInstance();

            builder.RegisterType<FlowDocumentPrintViewFactory>()
                   .As<IFlowDocumentPrintViewFactory>()
                   .SingleInstance();

            builder.RegisterType<FlowDocumentPrintViewBuilder>()
                   .As<IPrintViewBuilder>()
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