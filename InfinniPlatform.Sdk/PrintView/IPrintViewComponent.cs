namespace InfinniPlatform.Sdk.PrintView
{
    /// <summary>
    /// Контракт для получения печатной формы документов из контекста
    /// </summary>
    public interface IPrintViewComponent
    {
        byte[] BuildPrintView(object printView, object printViewSource, PrintViewFileFormat printViewFileFormat = PrintViewFileFormat.Pdf);
    }
}