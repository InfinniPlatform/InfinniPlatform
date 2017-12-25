using InfinniPlatform.IoC;
using InfinniPlatform.PrintView.Factories;
using InfinniPlatform.PrintView.Writers.Html;
using InfinniPlatform.PrintView.Writers.Pdf;
using InfinniPlatform.Serialization;

namespace InfinniPlatform.PrintView.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.PrintView" />.
    /// </summary>
    public class PrintViewContainerModule : IContainerModule
    {
        /// <summary>
        /// Creates new instance of <see cref="PrintViewContainerModule"/>.
        /// </summary>
        public PrintViewContainerModule(PrintViewOptions options)
        {
            _options = options;
        }

        private readonly PrintViewOptions _options;

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

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