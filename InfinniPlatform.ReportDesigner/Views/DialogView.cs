using System;
using System.Drawing;
using System.Windows.Forms;

namespace InfinniPlatform.ReportDesigner.Views
{
	/// <summary>
	/// Представление для отображения формы диалога.
	/// </summary>
	sealed partial class DialogView<TView> : Form where TView : ContainerControl, new()
	{
		public DialogView()
			: this(new TView())
		{
		}

		public DialogView(TView view)
		{
			InitializeComponent();

			View = view;
		}


		private TView _view;

		public TView View
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value;

				if (value != null && value.Parent != MainPanel)
				{
					value.Dock = DockStyle.Fill;

					Text = value.Text;
					Size = new Size(Size.Width - MainPanel.Width + value.Width, Size.Height - MainPanel.Height + value.Height);

					MainPanel.Controls.Clear();
					MainPanel.Controls.Add(value);
				}
			}
		}


		protected override void OnLoad(EventArgs e)
		{
			View = View;
			base.OnLoad(e);
		}

		private void OnSave(object sender, EventArgs e)
		{
			if (View == null || View.ValidateChildren())
			{
				DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void OnCancel(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}
	}
}