using InfinniPlatform.UserInterface.ViewBuilders.Views;

namespace InfinniPlatform.UserInterface.ViewBuilders
{
	/// <summary>
	/// Предоставляет интерфейс для построения объекта по метаданным.
	/// </summary>
	interface IObjectBuilder
	{
		object Build(ObjectBuilderContext context, View parent, dynamic metadata);
	}
}