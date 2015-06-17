﻿using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.Deprecated;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.MetadataManagers;
using InfinniPlatform.Api.Settings;
using InfinniPlatform.MetadataDesigner.Views.Exchange;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для редактирования сценариев.
	/// </summary>
	public sealed partial class ScenarioDesignerView : UserControl
	{
		private dynamic _scenario;


		public ScenarioDesignerView()
		{
			InitializeComponent();

			ComboBoxScenarioIdentifier.EditValueChanged += ComboBoxScenarioIdentifier_EditValueChanged;

			

			
		}


		private void InitEditor()
		{			
			ComboBoxScenarioContextType.Properties.Items.Clear();
			ComboBoxScenarioActionType.Properties.Items.Clear();

			ComboBoxScenarioContextType.Properties.Items.AddRange(ViewModelExtension.BuildContextTypes().BuildImageComboBoxItems().ToList());
			ComboBoxScenarioActionType.Properties.Items.AddRange(ViewModelExtension.BuildScriptUnitTypes().BuildImageComboBoxItems().ToList());

			if (_scenario != null)
			{

				var selectedContextType =
					ComboBoxScenarioContextType.Properties.Items.Cast<ImageComboBoxItem>()
					                           .FirstOrDefault(c => (int) c.Value == _scenario.ContextType);
				var selectedScriptType =
					ComboBoxScenarioActionType.Properties.Items.Cast<ImageComboBoxItem>()
					                          .FirstOrDefault(c => (int) c.Value == _scenario.ScriptUnitType);

				ComboBoxScenarioActionType.EditValue = selectedScriptType;
				ComboBoxScenarioContextType.EditValue = selectedContextType;
			}

		}



		void ComboBoxScenarioIdentifier_EditValueChanged(object sender, EventArgs e)
		{
			if (ComboBoxScenarioIdentifier.EditValue == null)
			{
				TextEditScenarioCaption.EditValue = null;
				ComboBoxScenarioContextType.EditValue = null;
				ComboBoxScenarioActionType.EditValue = null;
				return;
			}
			var descriptionItem = ((ScriptDescription)ComboBoxScenarioIdentifier.EditValue);
			var typeName = descriptionItem.TypeName;
			TextEditScenarioCaption.EditValue = typeName;

			ComboBoxScenarioContextType.EditValue = descriptionItem.ContextTypeCode;

			if (descriptionItem.MethodName == "Action")
			{
				ComboBoxScenarioActionType.EditValue = ScriptUnitType.Action;
			}
			else
			{
				ComboBoxScenarioActionType.EditValue = ScriptUnitType.Validator;
			}
		}

		private void RefreshUnits()
		{
			var process = new StatusProcess();
			bool discoverResult = false;
			var discovery = new AssemblyDiscovery();
			process.StartOperation(() =>
			{
				discoverResult = discovery.DiscoverAppliedAssemblies(Version(), ConfigId());
			});
			process.EndOperation();
			if (!discoverResult)
			{
				MessageBox.Show(string.Format("Не найдены прикладные сборки.\n\rКорректно укажите в App.config параметр AppliedAssemblies для указания местоположения прикладных сборок: \n\rТекущее значение параметра: {0}", AppSettings.GetValue("AppliedAssemblies")));
				return;
			}

			ComboBoxScenarioIdentifier.Properties.Items.Clear();
			ComboBoxScenarioIdentifier.Properties.Items.AddRange(
				ViewModelExtension.BuildScripts(discovery.SourceAssemblyList)
								 .BuildImageComboBoxItems()
								 .ToList());

		}

		public void UpdateEditorState()
		{
			sourceScenario.DataSource = LoadScenarios();
			RefreshUnits();
		}


		private DataTable LoadScenarios()
		{
			DataTable result = null;
			var process = new StatusProcess();
			process.StartOperation(() =>
			{
				result = ViewModelExtension.BuildDocumentScenarios(Version(), ConfigId(), DocumentId());
			});
			process.EndOperation();
			return result;

		}

		private void CreateScenarioButton_Click(object sender, EventArgs e)
		{
			var scriptDescription = ((ScriptDescription)ComboBoxScenarioIdentifier.EditValue);
			_scenario = new DynamicWrapper().BuildId(Guid.NewGuid().ToString())
									 .BuildName(scriptDescription.TypeName)
									 .BuildCaption(TextEditScenarioCaption.Text)
									 .BuildDescription(TextEditScenarioDescription.Text)
									 .BuildProperty("ScenarioId", scriptDescription.TypeName)
									 .BuildProperty("ContextType",
													(int)ComboBoxScenarioContextType.EditValue)
									 .BuildProperty("ScriptUnitType",
													(int)ComboBoxScenarioActionType.EditValue);
			MessageBox.Show("Scenario metadata created successfully.");

			OnValueChanged(_scenario, new EventArgs());
		}

		public Func<string> ConfigId { get; set; }

        public Func<string> Version { get; set; }

		public Func<string> DocumentId { get; set; }

		public object Value
		{
			get { return _scenario; }
			set
			{
                _scenario = value.ToDynamic();

			    if (_scenario != null && !string.IsNullOrEmpty(_scenario.Name))
			    {
			        ComboBoxScenarioIdentifier.Properties.Items.Add(new ImageComboBoxItem(_scenario.Name, new ScriptDescription()
			        {
			            TypeName = _scenario.Name
			        }));

			        ComboBoxScenarioIdentifier.Enabled = false;
			        ComboBoxScenarioIdentifier.EditValue =
			            ComboBoxScenarioIdentifier.Properties.Items.Cast<ImageComboBoxItem>().FirstOrDefault();
			        TextEditScenarioCaption.EditValue = _scenario.Caption;
			        TextEditScenarioDescription.EditValue = _scenario.Description;
			        DisableRefresh();
			    }
			    else
			    {
			        RefreshUnits();
			    }
			    InitEditor();
			}
		}

		private void DisableRefresh()
		{
			RefreshButton.Enabled = false;
		}


		public event EventHandler OnValueChanged;

		private void RefreshButton_Click(object sender, EventArgs e)
		{
			UpdateEditorState();
		}


	}
}