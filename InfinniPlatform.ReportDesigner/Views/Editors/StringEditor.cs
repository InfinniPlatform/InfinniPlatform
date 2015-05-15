using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views.Editors
{
	sealed class StringEditor : EditorBase
	{
		public StringEditor(string valueFormat = null, string defaultValue = null)
			: base(typeof(string), valueFormat, defaultValue)
		{
			_editControl = CreateEdit<TextBox>();
			_editControl.MaxLength = 1024;
		}


		private readonly TextBox _editControl;


		public override object Value
		{
			get { return _editControl.Text; }
			set { _editControl.Text = (string)CastObjectValue(value); }
		}

		public override string Text
		{
			get { return base.Text; }
			set { }
		}
	}
}