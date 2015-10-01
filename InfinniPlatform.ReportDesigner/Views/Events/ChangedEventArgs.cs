using System;

namespace InfinniPlatform.ReportDesigner.Views.Events
{
    internal sealed class ChangedEventArgs<T> : EventArgs
    {
        public readonly T NewValue;
        public readonly T OldValue;

        public ChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}