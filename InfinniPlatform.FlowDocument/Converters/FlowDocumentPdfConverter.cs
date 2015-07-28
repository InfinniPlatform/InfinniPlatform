using System.IO;

using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters
{
    sealed class FlowDocumentPdfConverter : IFlowDocumentConverter
    {
        public void Convert(ViewDocument document, Stream documentStream)
        {
            //using (var xpsDocumentStream = new MemoryStream())
            //{
            //    using (var package = Package.Open(xpsDocumentStream, FileMode.Create, FileAccess.ReadWrite))
            //    {
            //        using (var xpsDocument = new XpsDocument(package, CompressionOption.Maximum))
            //        {
            //            var serializer = new XpsSerializationManager(new XpsPackagingPolicy(xpsDocument), false);
            //            var paginator = ((IDocumentPaginatorSource)document).DocumentPaginator;
            //            serializer.SaveAsXaml(paginator);
            //            serializer.Commit();
            //        }
            //    }

            //    xpsDocumentStream.Position = 0;

            //    XpsConverter.Convert(xpsDocumentStream, documentStream);
            //}
        }
    }
}