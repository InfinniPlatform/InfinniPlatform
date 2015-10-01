using InfinniPlatform.Sdk.Environment;

namespace InfinniPlatform.Sdk.ContextComponents
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