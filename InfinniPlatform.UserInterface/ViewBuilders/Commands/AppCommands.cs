using System.Windows.Input;

namespace InfinniPlatform.UserInterface.ViewBuilders.Commands
{
	public static class AppCommands
	{
		public static readonly ICommand Refresh = new RoutedCommand();
		public static readonly ICommand Cut = new RoutedCommand();
		public static readonly ICommand Copy = new RoutedCommand();
		public static readonly ICommand Paste = new RoutedCommand();
		public static readonly ICommand Add = new RoutedCommand();
		public static readonly ICommand Edit = new RoutedCommand();
		public static readonly ICommand Delete = new RoutedCommand();
		public static readonly ICommand MoveUp = new RoutedCommand();
		public static readonly ICommand MoveDown = new RoutedCommand();
	}
}