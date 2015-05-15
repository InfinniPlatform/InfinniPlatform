using System;
using System.Windows.Input;

using InfinniPlatform.UserInterface.ViewBuilders.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree.Commands
{
	abstract class CommandBase<T> : ICommand<T>
	{
		public string Text { get; set; }

		public object Image { get; set; }

		public abstract bool CanExecute(T parameter);

		public abstract void Execute(T parameter);

		bool ICommand.CanExecute(object parameter)
		{
			return CanExecute((T)parameter);
		}

		void ICommand.Execute(object parameter)
		{
			Execute((T)parameter);
		}

		public event EventHandler CanExecuteChanged;
	}
}