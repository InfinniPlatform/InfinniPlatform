using InfinniPlatform.ReportDesigner.Views.Controls;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
	sealed class IntegerEditor : EditorBase
	{
		public IntegerEditor(string valueFormat = null, int defaultValue = 0)
			: base(typeof(int), valueFormat, defaultValue)
		{
			_editControl = CreateEdit<NumericEditor>();
			_editControl.IsRealNumber = false;
		}


		private readonly NumericEditor _editControl;


		public override object Value
		{
			get { return _editControl.Value; }
			set { _editControl.Value = (int)CastObjectValue(value); }
		}

		public override string Text
		{
			get { return base.Text; }
			set { }
		}
	}
}