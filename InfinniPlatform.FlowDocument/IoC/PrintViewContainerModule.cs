using InfinniPlatform.Core.PrintView;
using InfinniPlatform.FlowDocument.PrintView;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.PrintView;

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
        }
    }
}