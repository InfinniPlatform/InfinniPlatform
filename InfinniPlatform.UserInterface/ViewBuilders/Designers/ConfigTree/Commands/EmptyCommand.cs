using InfinniPlatform.UserInterface.ViewBuilders.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
	sealed class EmptyCommand<T> : CommandBase<T>
	{
		public static readonly ICommand<T> Instance = new EmptyCommand<T>();

		private EmptyCommand()
		{
		}

		public override bool CanExecute(T parameter)
		{
			return false;
		}

		public override void Execute(T parameter)
		{
		}
	}
}