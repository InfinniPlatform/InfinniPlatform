using InfinniPlatform.UserInterface.ViewBuilders.LinkViews;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers
{
	/// <summary>
	/// Описание редактора элемента конфигурации.
	/// </summary>
	sealed class ItemEditor
	{
		public string Text { get; set; }
		public string Image { get; set; }
		public string Container { get; set; }
		public string MetadataType { get; set; }
		public LinkView LinkView { get; set; }
	}
}