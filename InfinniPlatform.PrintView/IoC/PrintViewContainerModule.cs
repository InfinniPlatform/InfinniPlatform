using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Writers;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.PrintView.IoC
{
    internal class PrintViewContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<PrintViewBuilder>()
                   .As<IPrintViewBuilder>()
                   .SingleInstance();

            // PrintView (wkhtmltopdf)

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<PrintViewSettings>(PrintViewSettings.SectionName))
                   .As<PrintViewSettings>()
                   .SingleInstance();

            builder.RegisterType<PrintViewWriter>()
                   .As<IPrintViewWriter>()
                   .SingleInstance();

            builder.RegisterType<PrintViewFactory>()
                   .As<IPrintViewFactory>()
                   .SingleInstance();

            builder.RegisterType<PrintViewBuilder>()
                   .As<IPrintViewBuilder>()
                   .SingleInstance();
        }
    }
}