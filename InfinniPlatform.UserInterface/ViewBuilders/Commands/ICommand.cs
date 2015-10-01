using System.Windows.Input;

namespace InfinniPlatform.UserInterface.ViewBuilders.Commands
{
    public interface ICommand<in T> : ICommand
    {
        bool CanExecute(T parameter);
        void Execute(T parameter);
    }
}