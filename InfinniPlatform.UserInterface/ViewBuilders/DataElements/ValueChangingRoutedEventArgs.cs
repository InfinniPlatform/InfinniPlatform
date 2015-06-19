using System.Windows;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements
{
    /// <summary>
    ///     Аргумент события начала изменений значения.
    /// </summary>
    public sealed class ValueChangingRoutedEventArgs : RoutedEventArgs
    {
        public ValueChangingRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        public bool IsCancel { get; set; }
        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}