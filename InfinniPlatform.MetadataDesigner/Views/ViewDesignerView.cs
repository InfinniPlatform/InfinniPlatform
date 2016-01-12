using System;
using System.Linq;
using System.Windows.Forms;

using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Core.Properties;
using InfinniPlatform.DesignControls;
using InfinniPlatform.MetadataDesigner.Views.GeneratorResult;
using InfinniPlatform.MetadataDesigner.Views.JsonEditor;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для редактирования представлений.
	/// </summary>
	public sealed partial class ViewDesignerView : UserControl
	{
		private dynamic _view;
		private readonly DesignerControl _designerControl;

		public ViewDesignerView()
		{
			InitializeComponent();

			_designerControl = new DesignerControl();
			_designerControl.Dock = DockStyle.Fill;
			
			TabPageDesigner.Controls.Add(_designerControl);
			_designerControl.OnLayoutChanged += s =>
				                                   {
					                                   if (MessageBox.Show(
														   @"Create new form layout? All existing layout will be replaced.",
						                                   Resources.NeedConfirm, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
					                                       DialogResult.OK)
					                                   {
						                                   RefreshView(s);
					                                   }
				                                   };
		}

		private void RefreshView(dynamic layout)
		{
			string viewId = _view != null ? _view.Id : Guid.NewGuid().ToString();
			_view = layout;
			_view.Id = viewId;
			_view.Name = NameEditor.EditValue;
			_view.Caption = CaptionEditor.EditValue;
			_view.Description = DescriptionEditor.EditValue;
			_view.MetadataType = ComboBoxSelectViewType.EditValue;

			OnValueChanged(_view, new EventArgs());
			
		}

		public Func<string> ConfigId { get; set; }

		public Func<string> DocumentId { get; set; }

        public Func<string> Version { get; set; }


		private bool _isVisible;
		private bool _pendingRenderValue;

		public bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			set
			{
				if (_isVisible != value)
				{
					_isVisible = value;

					if (value)
					{
						RenderValue();
					}
				}
			}
		}

		public object Value
		{
			get
			{
				return _view;
			}
			set
			{
				_view = value.ToDynamic();

				_pendingRenderValue = true;

				if (IsVisible)
				{
					RenderValue();
				}
			}
		}

		private void RenderValue()
		{
		}

		public event EventHandler OnValueChanged;

		private void CreateButton_Click(object sender, EventArgs e)
		{


			if (NameEditor.EditValue == null)
			{
				MessageBox.Show("Необходимо указать наименование");
				return;				
			}

			if (ComboBoxSelectViewType.EditValue == null)
			{
				MessageBox.Show("Необходимо указать тип представления");
				return;								
			}



			dynamic viewByString = null;
			if (MetadataGeneratorSelect.EditValue != null)
			{

				dynamic item = MetadataGeneratorSelect.EditValue;
				viewByString = CreateGeneratedView(item.DocumentId , item.Name, ComboBoxSelectViewType.EditValue.ToString());
			}

			_view = viewByString != null ? DynamicWrapperExtensions.ToDynamic((string)viewByString) : new DynamicWrapper();
			_view.Id = Guid.NewGuid().ToString();
			_view.Name = NameEditor.EditValue.ToString();
			_view.Caption = CaptionEditor.EditValue != null ? CaptionEditor.EditValue.ToString() : "";
			_view.Description = DescriptionEditor.EditValue != null ? DescriptionEditor.EditValue.ToString() : "";
			_view.MetadataType = ComboBoxSelectViewType.EditValue;

			MessageBox.Show("View metadata created successfully.");

			OnValueChanged(_view, new EventArgs());

		}

		private string CreateGeneratedView(string documentId, string generatorName, string viewType)
		{
			var form = new CheckedBody();

			string jsonParams = string.Empty;
			if (form.ShowDialog() == DialogResult.OK)
			{
				jsonParams = form.JsonBody;
			}

			try
			{
				return ViewModelExtension.CheckGetView(Version(), ConfigId(), documentId, generatorName,viewType, jsonParams);
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
				return null;
			}
			

		}

		private void ButtonCheckGenerator_Click(object sender, EventArgs e)
		{
			if (ComboBoxSelectViewType.EditValue == null)
			{
				MessageBox.Show("Необходимо указать тип представления");
				return;
			}

			var form = new CheckedBody();

			string jsonParams = string.Empty;
			if (form.ShowDialog() == DialogResult.OK)
			{
				jsonParams = form.JsonBody;

			}

			dynamic item = MetadataGeneratorSelect.EditValue;
			var result = ViewModelExtension.CheckGetView(Version(), ConfigId(), item.DocumentId, NameEditor.Text,ComboBoxSelectViewType.EditValue.ToString(), jsonParams);
			var checkForm = new CheckForm();
			checkForm.MemoText = result;
			checkForm.BodyText = "";
			checkForm.UrlText = "";
			checkForm.ShowDialog();
		}

		private void TabControlViewDesigner_TabIndexChanged(object sender, EventArgs e)
		{
			if (TabControlViewDesigner.SelectedTab == TabPageDesigner)
			{
				_designerControl.ProcessJson(_view);
			}
		}

		private void TabControlViewDesigner_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (TabControlViewDesigner.SelectedTab == TabPageDesigner)
			{
				_designerControl.ProcessJson(_view);
			}
		}
	}
}