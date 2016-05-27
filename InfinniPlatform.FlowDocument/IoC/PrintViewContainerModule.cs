using InfinniPlatform.Core.PrintView;
using InfinniPlatform.FlowDocument.PrintView;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PrintView;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.FlowDocument.IoC
{
    internal sealed class PrintViewContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<PrintViewApi>()
                   .As<IPrintViewApi>()
                   .SingleInstance();

            builder.RegisterType<FlowDocumentPrintViewBuilder>()
                   .As<IPrintViewBuilder>()
                   .SingleInstance();

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
        }
    }
}