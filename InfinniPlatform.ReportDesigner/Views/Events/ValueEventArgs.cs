using System;

namespace InfinniPlatform.ReportDesigner.Views.Events
{
    internal sealed class ValueEventArgs<T> : EventArgs
    {
        public ValueEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}