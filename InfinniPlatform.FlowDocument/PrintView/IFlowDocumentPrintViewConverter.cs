using System.IO;

using InfinniPlatform.FlowDocument.Model.Views;
using InfinniPlatform.Sdk.PrintView;

namespace InfinniPlatform.FlowDocument.PrintView
{
	/// <summary>
	/// Преобразовывает печатное представление в файл определенного формата.
	/// </summary>
	public interface IFlowDocumentPrintViewConverter
	{
		/// <summary>
		/// Осуществляет преобразование документа печатного представления в файл указанного формата.
		/// </summary>
		/// <param name="printView">Документ печатного представления.</param>
		/// <param name="printViewStream">Поток файла печатного представления.</param>
		/// <param name="printViewFileFormat">Формат файла печатного представления.</param>
		void Convert(PrintViewDocument printView, Stream printViewStream, PrintViewFileFormat printViewFileFormat);
	}
}