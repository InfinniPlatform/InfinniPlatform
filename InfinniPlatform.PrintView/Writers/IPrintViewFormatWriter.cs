using System.IO;
using System.Threading.Tasks;

using InfinniPlatform.PrintView.Model.Views;

namespace InfinniPlatform.PrintView.Writers
{
    /// <summary>
    /// Преобразовывает документ в файл определенного формата.
    /// </summary>
    internal interface IPrintViewFormatWriter
    {
        Task Write(Stream stream, PrintViewDocument document);
    }
}