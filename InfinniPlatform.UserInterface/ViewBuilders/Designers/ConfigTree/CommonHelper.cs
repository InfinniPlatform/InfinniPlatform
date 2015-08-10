using System;
using System.Windows;
using InfinniPlatform.UserInterface.Properties;
using InfinniPlatform.UserInterface.ViewBuilders.Commands;

namespace InfinniPlatform.UserInterface.ViewBuilders.Designers.ConfigTree
{
    internal static class CommonHelper
    {
        public static bool TryCanExecute<T>(this ICommand<T> command, T parameter = default(T))
        {
            return (command != null && command.CanExecute(parameter));
        }

        public static void TryExecute<T>(this ICommand<T> command, T parameter = default(T))
        {
            if (command != null)
            {
                //try
                //{
                    if (command.CanExecute(parameter))
                    {
                        command.Execute(parameter);
                    }
                //}
                //catch (Exception error)
                //{
                //    ShowErrorMessage(error.Message);
                //}
            }
        }

        public static void ShowErrorMessage(string message, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Resources.AppViewText, MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public static void ShowWarningMessage(string message, params object[] args)
        {
            MessageBox.Show(string.Format(message, args), Resources.AppViewText, MessageBoxButton.OK,
                MessageBoxImage.Warning);
        }

        public static bool AcceptQuestionMessage(string message, params object[] args)
        {
            return
                MessageBox.Show(string.Format(message, args), Resources.AppViewText, MessageBoxButton.YesNo,
                    MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes;
        }
    }
}