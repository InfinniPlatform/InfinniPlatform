using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.JsonEditor
{
	public partial class CheckedBody : Form
	{
		public CheckedBody()
		{
			InitializeComponent();
		}

		public string JsonBody
		{
			get { return MemoJSONBody.Text; }
		}
	}
}
