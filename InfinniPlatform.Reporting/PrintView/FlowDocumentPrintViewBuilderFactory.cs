using InfinniPlatform.Api.PrintView;
using InfinniPlatform.Factories;
using InfinniPlatform.FlowDocument.PrintView;

namespace InfinniPlatform.Reporting.PrintView
{
	/// <summary>
	/// Фабрика для создания построителя печатного представления на основе System.Windows.Documents.FlowDocument.
	/// </summary>
	public sealed class FlowDocumentPrintViewBuilderFactory : IPrintViewBuilderFactory
	{
		private IPrintViewBuilder _printViewBuilder;

		public IPrintViewBuilder CreatePrintViewBuilder()
		{
			return _printViewBuilder ?? (_printViewBuilder = new FlowDocumentPrintViewBuilder(new FlowDocumentPrintViewFactory(), new FlowDocumentPrintViewConverter()));
		}
	}
}