using System;
using System.Linq;
using System.Windows.Forms;

using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.RestApi.CommonApi.RouteTraces;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.GeneratorResult;
using InfinniPlatform.MetadataDesigner.Views.JsonEditor;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.Update;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для редактирования генераторов.
	/// </summary>
	public sealed partial class GeneratorDesignerView : UserControl
	{
		private string _configurationName;
		private string _documentName;

		private dynamic _generator;
		private readonly AssemblyDiscovery _assemblyDiscovery;

		public GeneratorDesignerView()
		{
			InitializeComponent();

			_assemblyDiscovery = new AssemblyDiscovery();

			ButtonCreateGenerator.Click += ButtonCreateGenerator_Click;
			ButtonCheckGenerator.Click += ButtonCheckGenerator_Click;
			ButtonRefreshConfig.Click += ButtonRefreshConfig_Click;


		}

		public void InitEditor()
		{
			_configurationName = ConfigId();
			_documentName = DocumentId();


			RefreshUnits();

		}

		private void RefreshUnits()
		{
			var process = new StatusProcess();
			bool discoverResult = false;
			process.StartOperation(() =>
			{
				discoverResult = _assemblyDiscovery.DiscoverAppliedAssemblies(_configurationName);
			});
			process.EndOperation();
			if (!discoverResult)
			{
				MessageBox.Show(
					string.Format(
						"Не найдены прикладные сборки.\n\rКорректно укажите в App.config параметр AppliedAssemblies для указания местоположения прикладных сборок: \n\rТекущее значение параметра: {0}",
						AppSettings.GetValue("AppliedAssemblies")));
			}
			else
			{
				ComboBoxSelectGeneratorScenario.Properties.Items.Clear();
				ComboBoxSelectGeneratorScenario.Properties.Items.AddRange(ViewModelExtension.BuildGeneratorScripts(_assemblyDiscovery.SourceAssemblyList).BuildImageComboBoxItems().ToList());

			}
		}

		void ButtonCreateGenerator_Click(object sender, EventArgs e)
		{

			if (string.IsNullOrEmpty(TextEditGeneratorName.Text))
			{
				MessageBox.Show(@"Необходимо указать наименование генератора.");
				return;
			}

			if (ComboBoxSelectGeneratorScenario.EditValue == null)
			{
				MessageBox.Show(@"Необходимо указать сценарий для генерации метаданных.");
				return;
			}

			if (ComboBoxSelectViewType.EditValue == null)
			{
				MessageBox.Show(@"Необходимо указать тип представления генератора метаданных.");
				return;
			}

			var process = new StatusProcess();
			process.StartOperation(() =>
			{
				var generatorBroker = new GeneratorBroker(_configurationName, _documentName);

				var generator = new
									{
										GeneratorName = TextEditGeneratorName.Text,
										ActionUnit = ((ScriptDescription)ComboBoxSelectGeneratorScenario.EditValue).TypeName,
										MetadataType = ComboBoxSelectViewType.EditValue.ToString(),
									};


				generatorBroker.CreateGenerator(generator);

				var manager =
					new ManagerFactoryDocument(_configurationName, _documentName).BuildManagerByType(MetadataType.Generator);

				_generator = manager.MetadataReader.GetItem(TextEditGeneratorName.Text);
				

			});
			process.EndOperation();
			OnValueChanged(_generator, new EventArgs());
		}

		void ButtonRefreshConfig_Click(object sender, EventArgs e)
		{
			var process = new StatusProcess();
			process.StartOperation(() => new ExchangeDirector(new ExchangeLocalHost(), _configurationName).UpdateConfigurationMetadataFromSelf());
			process.EndOperation();
		}

		void ButtonCheckGenerator_Click(object sender, EventArgs e)
		{
			var form = new CheckedBody();

			string jsonParams = string.Empty;
			if (form.ShowDialog() == DialogResult.OK)
			{
				jsonParams = form.JsonBody;

			}

			var tracer = new RouteTraceSaveQueryLog();

			var result = ViewModelExtension.CheckGetView(ConfigId(), DocumentId(), "", ComboBoxSelectViewType.EditValue.ToString(), jsonParams);


			var checkForm = new CheckForm
							{
								MemoText = result,
								BodyText = tracer.GetCatchedData().Select(r => r.Body).FirstOrDefault(),
								UrlText = tracer.GetCatchedData().Select(r => r.Url).FirstOrDefault()
							};

			checkForm.ShowDialog();
		}


		public Func<string> ConfigId { get; set; }

		public Func<string> DocumentId { get; set; }

		public object Value
		{
			get { return _generator; }
			set
			{
				ComboBoxSelectGeneratorScenario.Properties.Items.Clear();
				ComboBoxSelectGeneratorScenario.Properties.Items.AddRange(ViewModelExtension.BuildGeneratorScripts(_assemblyDiscovery.SourceAssemblyList).BuildImageComboBoxItems().ToList());

				ComboBoxSelectViewType.Properties.Items.Clear();
				ComboBoxSelectViewType.Properties.Items.AddRange(ViewModelExtension.BuildViewTypes().BuildImageComboBoxItemsString().ToList());

				dynamic item = value.ToDynamic();
				if (!string.IsNullOrEmpty(item.Name))
				{
					_generator = value.ToDynamic();
					TextEditGeneratorName.EditValue = _generator.Name;
					var comboBoxItem = new ImageComboBoxItem(_generator.ActionUnit, _generator.ActionUnit);
					ComboBoxSelectGeneratorScenario.Properties.Items.Add(comboBoxItem);

					ComboBoxSelectGeneratorScenario.EditValue = _generator.ActionUnit;
					ComboBoxSelectGeneratorScenario.Enabled = false;

					ComboBoxSelectViewType.EditValue = _generator.MetadataType;
					ComboBoxSelectViewType.Enabled = false;

					DisableCreate();

				}
				InitEditor();
			}
		}


		private void DisableCreate()
		{
			ButtonCreateGenerator.Enabled = false;
		}

		public event EventHandler OnValueChanged;

		private void ButtonRefreshScenario_Click(object sender, EventArgs e)
		{
			RefreshUnits();
		}
	}
}