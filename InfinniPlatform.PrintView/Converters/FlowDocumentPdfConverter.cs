using System.IO;
using System.IO.Packaging;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using PdfSharp.Xps;

namespace InfinniPlatform.FlowDocument.Converters
{
    internal sealed class FlowDocumentPdfConverter : IFlowDocumentConverter
    {
        public void Convert(System.Windows.Documents.FlowDocument document, Stream documentStream)
        {
            using (var xpsDocumentStream = new MemoryStream())
            {
                using (var package = Package.Open(xpsDocumentStream, FileMode.Create, FileAccess.ReadWrite))
                {
                    using (var xpsDocument = new XpsDocument(package, CompressionOption.Maximum))
                    {
                        var serializer = new XpsSerializationManager(new XpsPackagingPolicy(xpsDocument), false);
                        var paginator = ((IDocumentPaginatorSource) document).DocumentPaginator;
                        serializer.SaveAsXaml(paginator);
                        serializer.Commit();
                    }
                }

                xpsDocumentStream.Position = 0;

                XpsConverter.Convert(xpsDocumentStream, documentStream);
            }
        }
    }
}