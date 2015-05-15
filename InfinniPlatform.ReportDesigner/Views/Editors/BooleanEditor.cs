using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
	sealed class BooleanEditor : EditorBase
	{
		public BooleanEditor(string valueFormat = null, bool defaultValue = false)
			: base(typeof(bool), valueFormat, defaultValue)
		{
			_editControl = CreateEdit<CheckBox>();
			_editControl.Checked = defaultValue;
		}


		private readonly CheckBox _editControl;


		public override object Value
		{
			get { return _editControl.Checked; }
			set { _editControl.Checked = (bool)CastObjectValue(value); }
		}

		public override string Text
		{
			get { return base.Text; }
			set { }
		}
	}
}