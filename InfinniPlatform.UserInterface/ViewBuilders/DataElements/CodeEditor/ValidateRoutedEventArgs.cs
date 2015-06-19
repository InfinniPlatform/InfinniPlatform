using System.Windows;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements.CodeEditor
{
    /// <summary>
    ///     Аргумент события проверки текста.
    /// </summary>
    public sealed class ValidateRoutedEventArgs : RoutedEventArgs
    {
        public ValidateRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        public string Text { get; set; }
        public string Error { get; set; }
    }
}