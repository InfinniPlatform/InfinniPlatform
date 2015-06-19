using System.IO;

namespace InfinniPlatform.FlowDocument.Converters
{
    /// <summary>
    ///     Преобразовывает документ в файл определенного формата.
    /// </summary>
    internal interface IFlowDocumentConverter
    {
        void Convert(System.Windows.Documents.FlowDocument document, Stream documentStream);
    }
}