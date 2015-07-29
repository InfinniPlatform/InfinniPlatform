using System.IO;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters
{
	/// <summary>
	/// Преобразовывает документ в файл определенного формата.
	/// </summary>
	interface IFlowDocumentConverter
	{
		void Convert(PrintViewDocument document, Stream documentStream);
	}
}