using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraVerticalGrid.Rows;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.MetadataDesigner.Views.ProcessTemplates
{
	public partial class DefaultProcessTemplate : UserControl, IProcessTemplate
	{
		private IEnumerable<dynamic> _prefillItems;

		private dynamic _process;
		private EditMode _editMode;

		public DefaultProcessTemplate()
		{
			InitializeComponent();

			TextEditProcessName.EditValue = "Default";
			TextEditProcessCaption.EditValue = "Default business process";
		}

		private dynamic _schemaPrefill;

		private void ButtonReloadSchemaClick(object sender, EventArgs e)
		{
			ReloadSchema();
		}

		private void ReloadSchema()
		{
			_schemaPrefill = new MetadataApi().GetDocumentSchema(Version, ConfigId, DocumentId);
			FillPropertiesBySchema();
		}

		private IEnumerable<string> LoadPropertiesNames()
		{
            var schema = new MetadataApi().GetDocumentSchema(Version,ConfigId, DocumentId);

			var properiesNames = new List<string>();

			var schemaIterator = new SchemaIterator(new SchemaReaderManager())
			{
				OnObjectProperty = schemaObject => properiesNames.Add(schemaObject.Name),
				OnPrimitiveProperty = schemaObject => properiesNames.Add(schemaObject.Name)
			};

			schemaIterator.ProcessSchema(Version, schema);

			return properiesNames;
		}

		private void FillControlsItems()
		{
            //_prefillItems = RestQueryApi.QueryPostJsonRaw("systemconfig", "prefill", "getfillitems", null, null).ToDynamicList();
		    _prefillItems = new dynamic[]{};
            ComplexPrefillEditor.Properties.Items.Clear();
			ComplexPrefillEditor.Properties.Items.AddRange(ActionHandlers.BuildImageComboBoxItems().ToList());

			ComplexValidationErrorEditor.Properties.Items.Clear();
			ComplexValidationErrorEditor.Properties.Items.AddRange(ValidationHandlers.BuildImageComboBoxItems().ToList());

			ComplexValidationWarningEditor.Properties.Items.Clear();
			ComplexValidationWarningEditor.Properties.Items.AddRange(ValidationHandlers.BuildImageComboBoxItems().ToList());

			SimpleValidationWarningEditor.Properties.Items.Clear();
			SimpleValidationWarningEditor.Properties.Items.AddRange(ValidationWarnings.BuildImageComboBoxItemsString().ToList());

			SimpleValidationErrorEditor.Properties.Items.Clear();
			SimpleValidationErrorEditor.Properties.Items.AddRange(ValidationErrors.BuildImageComboBoxItemsString().ToList());

            DeletingDocumentValidationErrorEditor.Properties.Items.Clear();
            DeletingDocumentValidationErrorEditor.Properties.Items.AddRange(ValidationHandlers.BuildImageComboBoxItems().ToList());

			SuccessSaveEditor.Properties.Items.Clear();
			SuccessSaveEditor.Properties.Items.AddRange(ActionHandlers.BuildImageComboBoxItems().ToList());

			FailSaveEditor.Properties.Items.Clear();
			FailSaveEditor.Properties.Items.AddRange(ActionHandlers.BuildImageComboBoxItems().ToList());

			DeleteDocumentEditor.Properties.Items.Clear();
			DeleteDocumentEditor.Properties.Items.AddRange(ActionHandlers.BuildImageComboBoxItems().ToList());

			DefaultProcessCredentialsAction.Properties.Items.Clear();
			DefaultProcessCredentialsAction.Properties.Items.AddRange(ActionHandlers.BuildImageComboBoxItems().ToList());
		}



		private void FillRepositoryEditItems()
		{
			RepositoryItemComboBoxEdit.Items.AddRange(
				 _prefillItems.Select(m => new ImageComboBoxItem(m, m)).ToList());

		}

		private void FillPropertiesBySchema()
		{
			var schemaIterator = new SchemaIterator(new SchemaReaderManager());
			PropertyGridControl.Rows.Clear();
			schemaIterator.OnObjectProperty = schemaObject => FillNewRow(schemaObject, obj =>
																						   {
																							   if (!string.IsNullOrEmpty(obj.ParentPath))
																							   {
																								   return null;
																							   }

																							   var row = new EditorRow()
																							   {
																								   Name = obj.Name,
																							   };
																							   row.Properties.Caption = string.Format("{0} ({1})", obj.Caption, obj.Name);
																							   row.Properties.RowEdit = RepositoryItemComboBoxEdit;
																							   row.Properties.Value = obj.Value.DefaultValue;
																							   return row;
																						   });
			schemaIterator.OnPrimitiveProperty = schemaObject => FillNewRow(schemaObject, obj =>
																							{
																								if (!string.IsNullOrEmpty(obj.ParentPath))
																								{
																									return null;
																								}


																								var row = new EditorRow()
																								{
																									Name = obj.Name,
																								};
																								row.Properties.Caption = string.Format("{0} ({1})", obj.Caption, obj.Name);
																								row.Properties.RowEdit = RepositoryItemComboBoxEdit;
																								row.Properties.Value = obj.Value.DefaultValue;
																								return row;
																							});
			schemaIterator.ProcessSchema(Version,_schemaPrefill);


		}

		private void FillNewRow(SchemaObject schemaObject, Func<SchemaObject, BaseRow> rowConstructor)
		{
			CategoryRow categoryRow = null;
			if (!string.IsNullOrEmpty(schemaObject.ParentPath))
			{
				categoryRow =
					PropertyGridControl.GetRowByName(schemaObject.ParentPath) as CategoryRow;
			}
			if (categoryRow != null)
			{
				var row = rowConstructor(schemaObject);
				if (row != null)
				{
					categoryRow.ChildRows.Add(row);
				}
			}
			else
			{
				var row = rowConstructor(schemaObject);
				if (row != null)
				{
					PropertyGridControl.Rows.Add(row);
				}
			}
		}

		public dynamic PrefillSchema
		{
			get { return CreatePrefilledSchema(); }
		}

		private dynamic CreatePrefilledSchema()
		{
			var schemaIterator = new SchemaIterator(new SchemaReaderManager());
			Action<SchemaObject> action = schemaObject =>
			{
				var row = PropertyGridControl.GetRowByName(schemaObject.Name);
				if (row != null)
				{
					schemaObject.Value.DefaultValue = row.Properties.Value;
				}
			};

			schemaIterator.OnPrimitiveProperty = action;
			schemaIterator.OnObjectProperty = action;

			schemaIterator.ProcessSchema(Version,_schemaPrefill);
			return _schemaPrefill;
		}


		public void OnInitTemplate()
		{
			FillControlsItems();

			FillRepositoryEditItems();
		}

		public IEnumerable<HandlerDescription> ActionHandlers { get; set; }
		public IEnumerable<HandlerDescription> ValidationHandlers { get; set; }
		public IEnumerable<string> ValidationWarnings { get; set; }
		public IEnumerable<string> ValidationErrors { get; set; }
		public string ConfigId { get; set; }
		public string DocumentId { get; set; }
        public string Version { get; set; }

	    public IEnumerable<string> DocumentStates { get; set; }
		public IProcessBuilder ProcessBuilder { get; set; }


		public dynamic Process
		{
			get { return _process; }
			set
			{
				_process = value;
				RefreshBindings();
			}
		}

		private void RefreshBindings()
		{
			if (_process == null)
			{
				return;
			}
			//ищем схему предзаполнения документа
			if (_process.Transitions != null && _process.Transitions.Count > 0)
			{
				var schemaPrefillString = _process.Transitions[0].SchemaPrefill;
				if (schemaPrefillString != null)
				{
					_schemaPrefill = DynamicWrapperExtensions.ToDynamic(JObject.Parse(schemaPrefillString));
				}


				if (_process.Transitions[0].ComplexPrefill != null)
				{
					ComplexPrefillEditor.EditValue = ComplexPrefillEditor.Properties.Items.Cast<ImageComboBoxItem>()
																				 .FirstOrDefault(i => _process.Transitions[0].ComplexPrefill.ScenarioId == ((dynamic)i.Value).HandlerId);
				}
				if (_process.Transitions[0].ValidationPointError != null)
				{
					ComplexValidationErrorEditor.EditValue = ComplexValidationErrorEditor.Properties.Items.Cast<ImageComboBoxItem>()
																						 .FirstOrDefault(
																							 i => _process.Transitions[0].ValidationPointError.ScenarioId == ((dynamic)i.Value).HandlerId);
				}

				if (_process.Transitions[0].ValidationPointWarning != null)
				{
					ComplexValidationWarningEditor.EditValue = ComplexValidationWarningEditor.Properties.Items.Cast<ImageComboBoxItem>()
																						 .FirstOrDefault(
																							 i => _process.Transitions[0].ValidationPointWarning.ScenarioId == ((dynamic)i.Value).HandlerId);
				}

				if (_process.Transitions[0].SuccessPoint != null)
				{
					SuccessSaveEditor.EditValue = SuccessSaveEditor.Properties.Items.Cast<ImageComboBoxItem>().FirstOrDefault(
						i => _process.Transitions[0].SuccessPoint.ScenarioId == ((dynamic)i.Value).HandlerId);

				}

				if (_process.Transitions[0].FailPoint != null)
				{
					FailSaveEditor.EditValue = FailSaveEditor.Properties.Items.Cast<ImageComboBoxItem>().FirstOrDefault(
						i => _process.Transitions[0].FailPoint.ScenarioId == ((dynamic)i.Value).HandlerId);

				}

				if (_process.Transitions[0].DeletePoint != null)
				{
					DeleteDocumentEditor.EditValue = DeleteDocumentEditor.Properties.Items.Cast<ImageComboBoxItem>().FirstOrDefault(
						i => _process.Transitions[0].DeletePoint.ScenarioId == ((dynamic)i.Value).HandlerId);

				}

                if (_process.Transitions[0].DeletingDocumentValidationPoint != null)
                {
                    DeletingDocumentValidationErrorEditor.EditValue = DeletingDocumentValidationErrorEditor.Properties.Items.Cast<ImageComboBoxItem>().FirstOrDefault(
                        i => _process.Transitions[0].DeletingDocumentValidationPoint.ScenarioId == ((dynamic)i.Value).HandlerId);

                }

				if (_process.Transitions[0].CredentialsPoint != null && _process.Transitions[0].CredentialsType == AuthorizationStorageExtensions.CustomCredentials)
				{
					DefaultProcessCredentialsAction.EditValue = DefaultProcessCredentialsAction.Properties.Items.Cast<ImageComboBoxItem>().FirstOrDefault(
						i => _process.Transitions[0].CredentialsPoint.ScenarioId == ((dynamic)i.Value).HandlerId);
				}

				DefaultProcessCredentialsType.EditValue = _process.Transitions[0].CredentialsType;

				SimpleValidationErrorEditor.EditValue = _process.Transitions[0].ValidationRuleError;

                DeletingDocumentValidationErrorEditor.EditValue = _process.Transitions[0].DeletingDocumentValidationRuleError;

				SimpleValidationWarningEditor.EditValue = _process.Transitions[0].ValidationRuleWarning;


			}

			//если схема предзаполнения документа не найдена, создаем новую схему
			if (_schemaPrefill == null)
			{
				ReloadSchema();
			}

			FillPropertiesBySchema();

			TextEditProcessName.EditValue = _process.Name;
			TextEditProcessCaption.EditValue = _process.Caption;

		}


		public EditMode EditMode
		{
			get { return _editMode; }
			set
			{
				_editMode = value;
				GroupControlOnOpen.Visible = _editMode == EditMode.ModeUpdate;
				GroupControlValidationOnSave.Visible = _editMode == EditMode.ModeUpdate;
				GroupControlActionOnSave.Visible = _editMode == EditMode.ModeUpdate;
				ButtonSaveProcess.Visible = _editMode == EditMode.ModeCreate;
				PanelCreateProcess.Visible = _editMode == EditMode.ModeUpdate;
			}
		}


		//private void simpleButton1_Click(object sender, EventArgs e)
		//{
		//	dynamic instance = CreatePrefilledSchema();
		//	memoEdit1.Text = instance.ToString();
		//}

		private void ButtonSaveProcessClick(object sender, EventArgs e)
		{
			if (TextEditProcessCaption.EditValue != null && TextEditProcessName.EditValue != null)
			{
				ProcessBuilder.BuildProcess(Guid.NewGuid().ToString(), TextEditProcessName.EditValue.ToString(), TextEditProcessCaption.EditValue.ToString(),
					(int)WorkflowTypes.WithoutState);

				EditMode = EditMode.ModeUpdate;

			}
			else
			{
				MessageBox.Show("Process caption and name should not be empty", "Warning", MessageBoxButtons.OK,
								MessageBoxIcon.Exclamation);
			}
		}


		private void ButtonCreateMetadata_Click(object sender, EventArgs e)
		{
			ProcessBuilder.EditTransition(GetTransition());

			var processManager = new ManagerFactoryDocument(ConfigId, DocumentId).BuildProcessManager();
			processManager.DeleteItem(_process);
			processManager.MergeItem(_process);

			MessageBox.Show("Process metadata created successfully.");
		}


		private dynamic GetTransition()
		{
			var prefillSchema = CreatePrefilledSchema();

			ProcessBuilder.DeleteTransition("DefaultTransition");
			ProcessBuilder.AddTransition("DefaultTransition", null, ValidationPointError, ValidationPointWarning,
				ComplexPrefillPoint, SuccessPoint, null, FailPoint, DeletePoint, ValidationRuleWarning, ValidationRuleError,DeletingDocumentValidationRuleError,
				prefillSchema != null ? prefillSchema.ToString() : null, CredentialsType, CredentialsPoint);
			return _process.Transitions[0];
		}

		private void RepositoryItemComboBoxEditButtonClick(object sender, ButtonPressedEventArgs e)
		{
			if (e.Button.Kind == ButtonPredefines.Delete)
			{
				((BaseEdit)sender).EditValue = null;
			}
		}

		public dynamic ComplexPrefillPoint
		{
			get
			{
				if (ComplexPrefillEditor.EditValue != null)
				{
					return
						ViewModelExtension.BuildActionPointFromString(((HandlerDescription)ComplexPrefillEditor.EditValue).HandlerId);
				}
				return null;
			}
		}


		public dynamic ValidationPointError
		{
			get
			{
				if (ComplexValidationErrorEditor.EditValue != null)
				{
					return ViewModelExtension.BuildValidationPointFromString(
												   ((HandlerDescription)ComplexValidationErrorEditor.EditValue).HandlerId);
				}
				return null;
			}
		}

		public dynamic ValidationPointWarning
		{
			get
			{
				if (ComplexValidationWarningEditor.EditValue != null)
				{
					return ViewModelExtension.BuildValidationPointFromString(
												   ((HandlerDescription)ComplexValidationWarningEditor.EditValue).HandlerId);
				}
				return null;
			}
		}


		public dynamic SuccessPoint
		{
			get
			{
				if (SuccessSaveEditor.EditValue != null)
				{
					return ViewModelExtension.BuildSuccessPointFromString(
												   ((HandlerDescription)SuccessSaveEditor.EditValue).HandlerId);

				}
				return null;
			}
		}

		public dynamic FailPoint
		{
			get
			{
				if (FailSaveEditor.EditValue != null)
				{
					return
						ViewModelExtension.BuildFailPointFromString(
							((HandlerDescription)FailSaveEditor.EditValue).HandlerId);
				}
				return null;
			}
		}

		public dynamic DeletePoint
		{
			get
			{
				if (DeleteDocumentEditor.EditValue != null)
				{
					return
						ViewModelExtension.BuildDeletePointFromString(
							((HandlerDescription)DeleteDocumentEditor.EditValue).HandlerId);
				}
				return null;
			}
		}

		public string CredentialsType
		{
			get
			{
				return DefaultProcessCredentialsType.EditValue as string;
			}
		}

		public dynamic CredentialsPoint
		{
			get
			{
				if (DefaultProcessCredentialsType.EditValue != null && DefaultProcessCredentialsType.EditValue == AuthorizationStorageExtensions.CustomCredentials)
				{
					if (DefaultProcessCredentialsAction.EditValue == null)
					{
						MessageBox.Show("Credentials action should be selected for custom credentials type!");
						return null;
					}
					return
						ViewModelExtension.BuildActionPointFromString(
							((HandlerDescription)DefaultProcessCredentialsAction.EditValue).HandlerId);
				}
				return null;
			}
		}

		public string ValidationRuleWarning
		{
			get
			{
				if (SimpleValidationWarningEditor.EditValue != null)
				{
					return SimpleValidationWarningEditor.EditValue.ToString();
				}
				return null;
			}
		}


		public string ValidationRuleError
		{
			get
			{
				if (SimpleValidationErrorEditor.EditValue != null)
				{
					return SimpleValidationErrorEditor.EditValue.ToString();
				}
				return null;
			}
		}

        public dynamic DeletingDocumentValidationRuleError
        {
            get
            {
                if (DeletingDocumentValidationErrorEditor.EditValue != null)
                {
                    return ViewModelExtension.BuildValidationPointFromString(
                                                   ((HandlerDescription)DeletingDocumentValidationErrorEditor.EditValue).HandlerId);
                }
                return null;
            }
        }

		private void EditorDeleteButtonClick(object sender, ButtonPressedEventArgs e)
		{
			if (e.Button.Kind == ButtonPredefines.Delete)
			{
				((BaseEdit)sender).EditValue = null;
			}
		}


		private void ProcessCredentialsEditor_Properties_EditValueChanged(object sender, EventArgs e)
		{
			var customCredentialsSelected = DefaultProcessCredentialsType.EditValue.ToString() ==
											AuthorizationStorageExtensions.CustomCredentials;

			CredentialsLabel.Visible = customCredentialsSelected;
			DefaultProcessCredentialsAction.Visible = customCredentialsSelected;
		}






	}
}
