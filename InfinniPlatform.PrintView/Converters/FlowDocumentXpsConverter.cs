using System.IO;
using System.IO.Packaging;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;

namespace InfinniPlatform.FlowDocument.Converters
{
    internal sealed class FlowDocumentXpsConverter : IFlowDocumentConverter
    {
        public void Convert(System.Windows.Documents.FlowDocument document, Stream documentStream)
        {
            using (var package = Package.Open(documentStream, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var xpsDocument = new XpsDocument(package, CompressionOption.Maximum))
                {
                    var serializer = new XpsSerializationManager(new XpsPackagingPolicy(xpsDocument), false);
                    var paginator = ((IDocumentPaginatorSource) document).DocumentPaginator;
                    serializer.SaveAsXaml(paginator);
                    serializer.Commit();
                }
            }
        }
    }
}