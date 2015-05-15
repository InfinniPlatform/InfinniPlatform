namespace InfinniPlatform.FlowDocument.Builders
{
	/// <summary>
	/// Фабрика для создания элемента печатного представления на основе метаданных.
	/// </summary>
	interface IPrintElementFactory
	{
		object Create(PrintElementBuildContext buildContext, dynamic elementMetadata);
	}
}