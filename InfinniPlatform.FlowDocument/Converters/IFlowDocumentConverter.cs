namespace InfinniPlatform.FlowDocument.Converters
{
	/// <summary>
	/// Преобразовывает документ в файл определенного формата.
	/// </summary>
	interface IFlowDocumentConverter
	{
		void Convert(System.Windows.Documents.FlowDocument document, System.IO.Stream documentStream);
	}
}