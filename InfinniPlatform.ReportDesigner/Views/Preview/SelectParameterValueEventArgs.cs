using System;

namespace InfinniPlatform.ReportDesigner.Views.Preview
{
	sealed class SelectParameterValueEventArgs : EventArgs
	{
		public SelectParameterValueEventArgs(object value)
		{
			Value = value;
		}

		public object Value { get; set; }

		public string Label { get; set; }

		public bool Cancel { get; set; }
	}
}