using System;

namespace InfinniPlatform.ReportDesigner.Views.Events
{
	sealed class ChangedEventArgs<T> : EventArgs
	{
		public ChangedEventArgs(T oldValue, T newValue)
		{
			OldValue = oldValue;
			NewValue = newValue;
		}

		public readonly T OldValue;
		public readonly T NewValue;
	}
}