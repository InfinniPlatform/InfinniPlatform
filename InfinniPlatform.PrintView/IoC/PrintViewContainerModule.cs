using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Serialization;
using InfinniPlatform.Core.Abstractions.Settings;
using InfinniPlatform.PrintView.Contract;
using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Writers;
using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Pdf;

namespace InfinniPlatform.PrintView.IoC
{
    internal class PrintViewContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<HtmlToPdfSettings>(HtmlToPdfSettings.SectionName))
                   .As<HtmlToPdfSettings>()
                   .SingleInstance();

            builder.RegisterType<PrintViewKnownTypesSource>()
                   .As<IKnownTypesSource>()
                   .SingleInstance();

            builder.RegisterType<HtmlPrintDocumentWriter>()
                   .As<IPrintDocumentWriter>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<PdfPrintDocumentWriter>()
                   .As<IPrintDocumentWriter>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(CreatePrintViewWriter)
                   .As<IPrintViewWriter>()
                   .SingleInstance();

            builder.RegisterType<PrintDocumentBuilder>()
                   .As<IPrintDocumentBuilder>()
                   .SingleInstance();

            builder.RegisterType<PrintViewBuilder>()
                   .As<IPrintViewBuilder>()
                   .SingleInstance();
        }


        private static PrintViewWriter CreatePrintViewWriter(IContainerResolver resolver)
        {
            var writer = new PrintViewWriter();

            var htmlWriter = resolver.Resolve<HtmlPrintDocumentWriter>();
            var pdfWriter = resolver.Resolve<PdfPrintDocumentWriter>();

            writer.RegisterWriter(PrintViewFileFormat.Html, htmlWriter);
            writer.RegisterWriter(PrintViewFileFormat.Pdf, pdfWriter);

            return writer;
        }
    }
}