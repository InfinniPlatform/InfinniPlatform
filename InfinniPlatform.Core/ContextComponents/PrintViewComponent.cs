using InfinniPlatform.Api.PrintView;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    ///     Компонент для получения печатных форм из контекста
    /// </summary>
    public sealed class PrintViewComponent : IPrintViewComponent
    {
        private readonly IPrintViewBuilder _printViewBuilder;

        public PrintViewComponent(IPrintViewBuilder printViewBuilder)
        {
            _printViewBuilder = printViewBuilder;
        }

        public byte[] BuildPrintView(object printView, object printViewSource,
            PrintViewFileFormat printViewFileFormat = PrintViewFileFormat.Pdf)
        {
            return _printViewBuilder.BuildFile(printView, printViewSource, printViewFileFormat);
        }
    }
}