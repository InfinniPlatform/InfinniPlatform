using InfinniPlatform.MetadataDesigner.Views.Validation;
using System;
using System.Windows.Forms;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для редактирования представлений.
	/// </summary>
	public sealed partial class ValidationErrorDesignerView : UserControl
	{
		private dynamic _validation;

		public ValidationErrorDesignerView()
		{
			InitializeComponent();
		}

		public Func<string> ConfigId { get; set; }

		public Func<string> DocumentId { get; set; }

        public Func<string> Version { get; set; } 

		public object Value
		{
			get { return _validation; }
			set
			{
				_validation = value.ToDynamic();
			    if (!string.IsNullOrEmpty(_validation.Name))
			    {
			        NameEditor.EditValue = _validation.Name;
			        CaptionEditor.EditValue = _validation.Caption;
			        DescriptionEditor.EditValue = _validation.Description;
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

		    var validationBuilder = new ValidationConstructor();

            var form = new ValidationConstructorForm(validationBuilder);

            try
            {
                _validation = validationBuilder.BuildValidationStatement();

                if (_validation == null)
                {
                    MessageBox.Show("Выражения для валидации не сформировано.");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка формирования выражения валидации: " + ex.Message);
            }
            
            _validation.Id = Guid.NewGuid().ToString();
            _validation.Name = NameEditor.EditValue.ToString();
            _validation.Caption = CaptionEditor.EditValue != null ? CaptionEditor.EditValue.ToString() : "";
            _validation.Description = DescriptionEditor.EditValue != null ? DescriptionEditor.EditValue.ToString() : "";
		    

			OnValueChanged(_validation, new EventArgs());

		}

	}
}