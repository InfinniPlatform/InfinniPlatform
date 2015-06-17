using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.MetadataDesigner.Views
{
    /// <summary>
    /// Элемент управления для редактирования сервисов.
    /// </summary>
    public sealed partial class ServiceDesignerView : UserControl
    {
        private object _service;

        public ServiceDesignerView()
        {
            InitializeComponent();
        }


        private List<dynamic> _extensionPoints = new List<dynamic>();
        private dynamic _selectedExtensionPoint;

        void ButtonAddExtensionPoint_Click(object sender, EventArgs e)
        {
            dynamic point = null;
            try
            {
                point = new DynamicWrapper().BuildProperty("TypeName", ComboBoxExtensionPoint.EditValue)
                                             .BuildProperty("ScenarioId", ((ProcessDescription)ComboBoxScenarioId.EditValue).Name);
                _extensionPoints.Add(point);
            }
            catch
            {
                MessageBox.Show("Ошибка создания точки расширения");
            }

            sourceExtensionPoints.DataSource = _extensionPoints.ToDataTable();

        }

        void ButtonDeleteExtensionPoint_Click(object sender, EventArgs e)
        {
            if (_selectedExtensionPoint != null)
            {
                _extensionPoints =
                    _extensionPoints.ToList().Where(a => a.TypeName.Name != _selectedExtensionPoint.Name).ToList();
            }
            sourceExtensionPoints.DataSource = _extensionPoints.ToDataTable();
        }

        public void ExtensionPointFocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            var focusedRow = GridViewExtensionPoints.GetFocusedDataRow();
            if (focusedRow != null)
            {
	            string typeName = focusedRow.Field<string>("TypeName");
                _selectedExtensionPoint = DynamicWrapperExtensions.ToDynamic((string)typeName);
            }
            else
            {
                _selectedExtensionPoint = null;
            }
        }

        void ComboBoxServiceType_EditValueChanged(object sender, EventArgs e)
        {
            ComboBoxExtensionPoint.Properties.Items.Clear();
            if (ComboBoxServiceType.EditValue == null)
            {
                return;
            }

            ComboBoxExtensionPoint.Properties.Items.AddRange(ViewModelExtension.BuildServiceTypeExtensionPointList((string)ComboBoxServiceType.EditValue).BuildImageComboBoxItems().ToList());

        }

        void ButtonCreateService_Click(object sender, EventArgs e)
        {

            try
            {
	            dynamic service = _service;
	            string serviceId = null;
				if (service != null && service.Id != null)
				{
					serviceId = service.Id;
				}
	            _service = new DynamicWrapper().BuildId(serviceId ?? Guid.NewGuid().ToString())
                                                .BuildName(TextEditServiceName.Text)
                                                .BuildProperty("Type", ViewModelExtension.BuildCompleteServiceType(ComboBoxServiceType.EditValue.ToString()))
                                                .BuildCaption(TextEditServiceCaption.Text);
                foreach (var extensionPoint in _extensionPoints)
                {
                    _service.BuildCollectionItem("ExtensionPoints", (object)extensionPoint);
                }

				MessageBox.Show("Service metadata created successfully.");
                OnValueChanged(_service, new EventArgs());
            }
            catch
            {
                MessageBox.Show("Ошибка создания сервиса");
                return;
            }



        }

        public Func<string> Version { get; set; } 

        public Func<string> ConfigId { get; set; }

        public Func<string> DocumentId { get; set; }

        public object Value
        {
            get { return _service; }
            set
            {
                ComboBoxServiceType.Properties.Items.Clear();
                ComboBoxServiceType.Properties.Items.AddRange(ViewModelExtension.BuildServiceTypesHeaders().BuildImageComboBoxItemsString().ToList());

                IEnumerable<ProcessDescription> descriptions = ViewModelExtension.GetDocumentProcessesList(Version(), ConfigId(), DocumentId());


                ComboBoxScenarioId.Properties.Items.Clear();
                ComboBoxScenarioId.Properties.Items.AddRange(descriptions.BuildImageComboBoxItems().ToList());

                dynamic service = value.ToDynamic();

                if (service != null && !string.IsNullOrEmpty(service.Name))
                {
                    _service = service;
                    TextEditServiceName.EditValue = service.Name;
                    TextEditServiceCaption.EditValue = service.Caption;

                    object serviceType = null;

                    if (service.Type != null)
                    {
                        serviceType = ComboBoxServiceType.Properties.Items.Cast<ImageComboBoxItem>().Where(
                            c => c.Description == service.Type.Name).Select(c => c.Value).FirstOrDefault();
                    }

                    ComboBoxServiceType.EditValue = serviceType;

                    ComboBoxServiceType.Enabled = false;

                    if (service.ExtensionPoints != null)
                    {
                        _extensionPoints = service.ExtensionPoints;
                        sourceExtensionPoints.DataSource = _extensionPoints.ToDataTable();
                    }
                }
            }
        }

        public event EventHandler OnValueChanged;

        private void GridViewExtensionPoints_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "Name")
            {
                var item = GridViewExtensionPoints.GetDataRow(e.RowHandle);
	            var typeName = item.Field<string>("TypeName");
                e.DisplayText = DynamicWrapperExtensions.ToDynamic((string)typeName).Name;
            }
        }
    }
}