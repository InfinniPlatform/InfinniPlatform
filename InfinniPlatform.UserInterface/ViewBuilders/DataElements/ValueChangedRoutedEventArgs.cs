using System.Windows;

namespace InfinniPlatform.UserInterface.ViewBuilders.DataElements
{
    /// <summary>
    ///     Аргумент события окончания изменений значения.
    /// </summary>
    public sealed class ValueChangedRoutedEventArgs : RoutedEventArgs
    {
        public ValueChangedRoutedEventArgs(RoutedEvent routedEvent)
            : base(routedEvent)
        {
        }

        public object OldValue { get; set; }
        public object NewValue { get; set; }
    }
}