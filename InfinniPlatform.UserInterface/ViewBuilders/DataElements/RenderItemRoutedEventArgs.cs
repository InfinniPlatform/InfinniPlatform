using System.Windows;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements
{
	/// <summary>
	/// Аргумент события отрисовки элемента.
	/// </summary>
	public sealed class RenderItemRoutedEventArgs : RoutedEventArgs
	{
		public RenderItemRoutedEventArgs(RoutedEvent routedEvent)
			: base(routedEvent)
		{
		}

		public object Item { get; set; }
		public object Value { get; set; }
		public object Display { get; set; }
	}
}