using System.IO;
using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters
{
	/// <summary>
	/// Преобразовывает документ в файл определенного формата.
	/// </summary>
	interface IFlowDocumentConverter
	{
		void Convert(ViewDocument document, Stream documentStream);
	}
}