using System.IO;

using InfinniPlatform.PrintView.Model.Views;

namespace InfinniPlatform.PrintView.Writers
{
    /// <summary>
    /// Преобразовывает документ в файл определенного формата.
    /// </summary>
    internal interface IPrintViewFormatWriter
    {
        void Write(Stream stream, PrintViewDocument document);
    }
}