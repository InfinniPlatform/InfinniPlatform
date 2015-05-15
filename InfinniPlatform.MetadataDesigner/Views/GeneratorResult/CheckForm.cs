using System;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.GeneratorResult
{
	public partial class CheckForm : Form
	{
		public CheckForm()
		{
			InitializeComponent();
		}

		public string MemoText
		{
			get { return Memo.Text; }
			set { Memo.Text = value; }
		}

		public string BodyText
		{
			get { return BodyMemo.Text; }
			set { BodyMemo.Text = value; }
		}

		public string UrlText
		{
			get { return UrlEdit.Text; }
			set { UrlEdit.Text = value; }
		}

		private void ButtonOK_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
		}
	}
}
