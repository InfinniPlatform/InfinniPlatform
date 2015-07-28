using InfinniPlatform.Api.Registers;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MetadataDesigner.Views
{
    public partial class RegisterDesignerView : UserControl
    {
        /// <summary>
        /// Редактируемый объект
        /// </summary>
        private dynamic _register;

        /// <summary>
        /// Закэшированное значение схемы документа регистра
        /// </summary>
        private dynamic _documentSchema;

        /// <summary>
        /// Закэшированное значение схемы документа с итогами
        /// </summary>
        private dynamic _documentTotalSchema;

        public RegisterDesignerView()
        {
            InitializeComponent();
        }

        public Func<string> ConfigId { get; set; }

        public Func<string> DocumentId { get; set; }

        public Func<string> Version { get; set; } 
        
        public object Value
        {
            get
            {
                return _register;
            }
            set
            {
                RegisterTypeEditor.Properties.Items.AddRange(
                    ViewModelExtension.BuildRegisterTypes().BuildImageComboBoxItemsString().ToList());

                PeriodComboBoxEdit.Properties.Items.AddRange(
                    ViewModelExtension.BuildRegisterPeriods().BuildImageComboBoxItemsString().ToList());

                DataTypeComboBoxEdit.Properties.Items.AddRange(
                    ViewModelExtension.BuildRegisterPropertyDataTypes().BuildImageComboBoxItemsString().ToList());

                PropertyTypeComboBoxEdit.Properties.Items.AddRange(
                    ViewModelExtension.BuildRegisterPropertyTypes().BuildImageComboBoxItemsString().ToList());
                
                _register = value.ToDynamic();

                if (!string.IsNullOrEmpty(_register.Name))
                {
                    // Заполняем элементы управления  контрола свойствами регистра

                    PropertiesPanelControl.Visible = true;
                    RegisterNameTextEdit.Enabled = false;
                    
                    RegisterNameTextEdit.Text = _register.Name;
                    RegisterTypeEditor.Text = _register.Type;
                    PeriodComboBoxEdit.Text = _register.Period;
                    
                    if (_register.Asynchronous != null && _register.Asynchronous == true)
                    {
                        AsynchronousCheckBox.Checked = true;
                    }

                    // Словарь registerProperties содержит типы полей (ресурс, реквизит или измерения)
                    var registerProperties = new Dictionary<string, string>();
                    if (_register.Properties != null)
                    {
                        foreach (var property in _register.Properties)
                        {
                            registerProperties.Add(property.Property.ToString(), property.Type.ToString());
                        }
                    }

                    _documentSchema = ViewModelExtension.GetRegisterDocumentSchema(Version(), ConfigId(), _register.Name);

                    PropertiesListBoxControl.Items.Clear();

                    if (_documentSchema != null)
                    {
                        foreach (var property in _documentSchema.Properties)
                        {
                            string propertyName = property.Key;
                            string propertyDataType = property.Value.Type;
                            string propertyType = string.Empty;
                            if (registerProperties.ContainsKey(propertyName))
                            {
                                propertyType = registerProperties[propertyName];
                            }

                            PropertiesListBoxControl.Items.Add(
                                new RegisterProperty(
                                    propertyName,
                                    propertyDataType,
                                    propertyType));
                        }
                    }
                }
                else
                {
                    PropertiesPanelControl.Visible = false;
                }
            }
        }

        public event EventHandler OnValueChanged;

        /// <summary>
        /// Создание нового регистра
        /// </summary>
        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RegisterNameTextEdit.Text))
            {
                MessageBox.Show("Specify register name");
                return;
            }

            _register.Name = RegisterNameTextEdit.Text;
            _register.Caption = RegisterNameTextEdit.Text;
            _register.Id = Guid.NewGuid().ToString();

            _register.Properties = new List<dynamic>();
            _register.Properties.Add(new { Property = RegisterConstants.RegistrarProperty, Type = RegisterPropertyType.Info });
            _register.Properties.Add(new { Property = RegisterConstants.RegistrarTypeProperty, Type = RegisterPropertyType.Info });
            _register.Properties.Add(new { Property = RegisterConstants.DocumentDateProperty, Type = RegisterPropertyType.Info });
            _register.Properties.Add(new { Property = RegisterConstants.EntryTypeProperty, Type = RegisterPropertyType.Info });
            
            PropertiesPanelControl.Visible = true;

            _documentSchema = ViewModelExtension.GetRegisterDocumentSchema(Version(), ConfigId(), _register.Name);
            if (_documentSchema == null)
            {
                var proc = new StatusProcess();
                proc.StartOperation(
                    delegate
                    {
                        _register.DocumentId = ViewModelExtension.CreateRegisterDocuments(Version(), ConfigId(), _register.Name);
                    });
                proc.EndOperation();
            }

            if (OnValueChanged != null)
            {
                OnValueChanged(_register, new EventArgs());
            }
        }

        private void RegisterTypeEditor_SelectedIndexChanged(object sender, EventArgs e)
        {
            _register.Name = RegisterNameTextEdit.Text;
            _register.Caption = RegisterNameTextEdit.Text;
            _register.Type = RegisterTypeEditor.Text;

            if (OnValueChanged != null)
            {
                OnValueChanged(_register, new EventArgs());
            }
        }

        private void PeriodComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            _register.Name = RegisterNameTextEdit.Text;
            _register.Caption = RegisterNameTextEdit.Text;
            _register.Period = PeriodComboBoxEdit.Text;

            if (OnValueChanged != null)
            {
                OnValueChanged(_register, new EventArgs());
            }
        }

        private void AsynchronousCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _register.Name = RegisterNameTextEdit.Text;
            _register.Caption = RegisterNameTextEdit.Text;
            _register.Asynchronous = AsynchronousCheckBox.Checked;

            if (OnValueChanged != null)
            {
                OnValueChanged(_register, new EventArgs());
            }
        }

        /// <summary>
        /// Добавление свойства регистра.
        /// Применение изменений схемы данных регистра производится сразу,
        /// так как при добавлении нового свойства должны быть добавлены 
        /// соответствующие свойства различных служебных документов 
        /// (например, документа, хранящего данные регистра и документа, хранящего рассчитанные итоги)
        /// </summary>
        private void AddPropertyButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PropertyNameTextEdit.Text))
            {
                MessageBox.Show("Specify property name");
                return;
            }

            if (string.IsNullOrEmpty(DataTypeComboBoxEdit.Text))
            {
                MessageBox.Show("Specify property data type");
                return;
            }

            if (string.IsNullOrEmpty(PropertyTypeComboBoxEdit.Text))
            {
                MessageBox.Show("Specify property type");
                return;
            }

            var newProperty = new RegisterProperty(
                PropertyNameTextEdit.Text,
                DataTypeComboBoxEdit.Text,
                PropertyTypeComboBoxEdit.Text);

            PropertiesListBoxControl.Items.Add(newProperty);

            if (_register.Properties == null)
            {
                _register.Properties = new List<dynamic>();
            }

            dynamic newRegisterProperty = new DynamicWrapper();
            newRegisterProperty.Property = newProperty.Name;
            newRegisterProperty.Type = newProperty.Type;

            _register.Properties.Add(newRegisterProperty);

            // Обновляем схему документа, связанного с регистром

            if (_documentSchema == null)
            {
                _documentSchema = ViewModelExtension.GetRegisterDocumentSchema(Version(), ConfigId(), _register.Name);
            }

            if (_documentSchema == null)
            {
                var proc = new StatusProcess();
                proc.StartOperation(
                    delegate
                    {
                        _register.DocumentId = ViewModelExtension.CreateRegisterDocuments(Version(), ConfigId(), _register.Name);
                    });
                proc.EndOperation();

                _documentSchema = ViewModelExtension.GetRegisterDocumentSchema(Version(), ConfigId(), _register.Name);
            }

            _documentSchema.Properties[newProperty.Name] =
                new
                {
                    Type = DataTypeComboBoxEdit.Text
                }.ToDynamic();


            var process = new StatusProcess();
            process.StartOperation(
                () => ViewModelExtension.UpdateRegisterDocumentSchema(Version(), ConfigId(), _register.Name, _documentSchema));
            process.EndOperation();

            // Обновляем схему документа, рассчитываеющего промежуточные итоги по регистру
            // (поля реквизиты добавлять в схему итогов не нужно)
            if (newProperty.Type != RegisterPropertyType.Info)
            {
                if (_documentTotalSchema == null)
                {
                    _documentTotalSchema = ViewModelExtension.GetRegisterDocumentTotalSchema(Version(), ConfigId(), _register.Name);
                }

                _documentTotalSchema.Properties[newProperty.Name] =
                    new
                    {
                        Type = DataTypeComboBoxEdit.Text
                    }.ToDynamic();

                process = new StatusProcess();
                process.StartOperation(
                    () =>
                        ViewModelExtension.UpdateRegisterDocumentTotalSchema(Version(), ConfigId(), _register.Name,
                            _documentTotalSchema));
                process.EndOperation();
            }

            if (OnValueChanged != null)
            {
                OnValueChanged(_register, new EventArgs());
            }

            PropertyNameTextEdit.Text = string.Empty;
        }

        /// <summary>
        /// Удаление свойства регистра
        /// </summary>
        private void DeletePropertyButton_Click(object sender, EventArgs e)
        {
            if (PropertiesListBoxControl.SelectedIndex > -1)
            {
                DeletePropertyButton.Enabled = true;

                var propertyToDelete = (RegisterProperty) PropertiesListBoxControl.SelectedItem;
                
                var propertiesArray = new List<dynamic>();

                if (_register.Properties != null)
                {
                    foreach (var property in _register.Properties)
                    {
                        if (property.Property != null &&
                            property.Property.ToString() != propertyToDelete.Name)
                        {
                            propertiesArray.Add(property);
                        }
                    }
                }

                _register.Properties = propertiesArray;
                
                if (OnValueChanged != null)
                {
                    OnValueChanged(_register, new EventArgs());
                }

                if (_documentSchema == null)
                {
                    _documentSchema = ViewModelExtension.GetRegisterDocumentSchema(Version(), ConfigId(), _register.Name);
                }

                _documentSchema.Properties[propertyToDelete.Name] = null;
                
                var process = new StatusProcess();
                process.StartOperation(
                    () => ViewModelExtension.UpdateRegisterDocumentSchema(Version(), ConfigId(), _register.Name, _documentSchema));
                process.EndOperation();

                // Обновляем схему документа, рассчитываеющего промежуточные итоги по регистру
                // (поля реквизиты добавлять в схему итогов не нужно)
                if (propertyToDelete.Type != RegisterPropertyType.Info)
                {
                    if (_documentTotalSchema == null)
                    {
                        _documentTotalSchema = ViewModelExtension.GetRegisterDocumentTotalSchema(Version(), ConfigId(), _register.Name);
                    }

                    _documentTotalSchema.Properties[propertyToDelete.Name] = null;

                    process = new StatusProcess();
                    process.StartOperation(
                        () =>
                            ViewModelExtension.UpdateRegisterDocumentTotalSchema(Version(), ConfigId(), _register.Name,
                                _documentTotalSchema));
                    process.EndOperation();
                }
                
                PropertiesListBoxControl.Items.Remove(propertyToDelete);
            }
            else
            {
                MessageBox.Show("Specify property");
            }
        }
        
        private class RegisterProperty
        {
            public RegisterProperty(string name, string dataType, string type)
            {
                Type = type;
                DataType = dataType;
                Name = name;
            }

            public string Name { get; private set; }

            public string DataType { get; private set; }

            public string Type { get; private set; }

            public override string ToString()
            {
                return string.Format("{0} ('{1}' '{2}' property)", Name, Type, DataType);
            }
        }
    }
}
