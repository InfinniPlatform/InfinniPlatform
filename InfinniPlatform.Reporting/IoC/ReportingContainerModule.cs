using InfinniPlatform.ContextComponents;
using InfinniPlatform.Factories;
using InfinniPlatform.Reporting.PrintView;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Reporting.IoC
{
    internal sealed class ReportingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<FlowDocumentPrintViewBuilderFactory>()
                   .As<IPrintViewBuilderFactory>()
                   .SingleInstance();

            builder.RegisterType<PrintViewComponent>()
                   .As<IPrintViewComponent>()
                   .SingleInstance();
        }
    }
}