using InfinniPlatform.Api.PrintView;

namespace InfinniPlatform.Api.ContextComponents
{
    /// <summary>
    ///     Контрак для получения печатной формы документов из контекста
    /// </summary>
    public interface IPrintViewComponent
    {
        byte[] BuildPrintView(object printView, object printViewSource,
            PrintViewFileFormat printViewFileFormat = PrintViewFileFormat.Pdf);
    }
}