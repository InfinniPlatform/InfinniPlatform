using System.IO;
using System.Windows;
using System.Windows.Documents;

namespace InfinniPlatform.FlowDocument.Converters
{
	sealed class FlowDocumentRtfConverter : IFlowDocumentConverter
	{
		public void Convert(System.Windows.Documents.FlowDocument document, Stream documentStream)
		{
			var textRange = new TextRange(document.ContentStart, document.ContentEnd);
			textRange.Save(documentStream, DataFormats.Rtf);
		}
	}
}