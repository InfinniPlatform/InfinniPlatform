using System;
using System.Windows.Forms;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для редактирования статусов.
	/// </summary>
	public sealed partial class StatusDesignerView : UserControl
	{
		private dynamic _status;

        public StatusDesignerView()
		{
			InitializeComponent();
		}

		public Func<string> ConfigId { get; set; }

		public Func<string> DocumentId { get; set; }

		public object Value
		{
            get { return _status; }
			set
			{
                _status = value.ToDynamic();
                if (!string.IsNullOrEmpty(_status.Name))
				{
                    NameEditor.EditValue = _status.Name;
                    CaptionEditor.EditValue = _status.Caption;
                    DescriptionEditor.EditValue = _status.Description;
					NameEditor.Enabled = false;
				}
				
			}
		}


		public event EventHandler OnValueChanged;

		private void CreateButton_Click(object sender, EventArgs e)
		{
            if (NameEditor.EditValue == null)
            {
                MessageBox.Show("Необходимо указать наименование");
                return;
            }

            _status.Id = Guid.NewGuid().ToString();
            _status.Name = NameEditor.EditValue.ToString();
            _status.Caption = CaptionEditor.EditValue != null ? CaptionEditor.EditValue.ToString() : "";
            _status.Description = DescriptionEditor.EditValue != null ? DescriptionEditor.EditValue.ToString() : "";

            OnValueChanged(_status, new EventArgs());

		}

	}
}