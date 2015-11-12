using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using InfinniPlatform.Api.ContextTypes;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.SchemaReaders;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;
using InfinniPlatform.Api.Metadata.ConfigurationManagers.Standard.Factories;
using InfinniPlatform.Api.RestApi.Auth;
using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.MetadataDesigner.Views.ProcessTemplates
{
	public partial class CustomProcessTemplate : UserControl, IProcessTemplate
	{
		public CustomProcessTemplate()
		{
			InitializeComponent();
		}

		private void DeleteButtonClick(object sender, ButtonPressedEventArgs e)
		{
			if (e.Button.Kind == ButtonPredefines.Delete)
			{
				((BaseEdit)sender).EditValue = null;
			}
		}

		private string _selectedTransitionName;

		private dynamic _process;
		private EditMode _editMode;

		private DataTable LoadTransitionStates()
		{
            if (_process == null || _process.Transitions == null)
			{
				return new DataTable();
			}
			return ViewModelExtension.BuildStateTransitions(_process.Transitions);
		}

        private IEnumerable<string> LoadPropertiesNames()
        {
            var document = new ManagerFactoryConfiguration(ConfigId).BuildDocumentMetadataReader().GetItem(DocumentId);
            
            var properiesNames = new List<string>();

            var schemaIterator = new SchemaIterator(new SchemaReaderManager())
            {
                OnObjectProperty = schemaObject => properiesNames.Add(schemaObject.Name),
                OnPrimitiveProperty = schemaObject => properiesNames.Add(schemaObject.Name)
            };

            schemaIterator.ProcessSchema(Version, document.Schema);
            
            return properiesNames;
        }


		void FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
		{
			var focusedRow = GridViewTransitionStates.GetFocusedDataRow();
			if (focusedRow != null)
			{
				_selectedTransitionName = focusedRow.Field<string>("Name");
			}
			else
			{
				_selectedTransitionName = null;
			}
		}

		void CheckEditWithState_EditValueChanged(object sender, EventArgs e)
		{
			bool withState = ((bool)CheckEditWithState.EditValue);
			colStateFrom.Visible = withState;
			LabelFromState.Visible = withState;
			TextEditFromState.Visible = withState;
		}


		private void ReloadFailHandlers(IEnumerable<HandlerDescription> actionHandlers)
		{

			ComboBoxFailHandler.Properties.Items.Clear();
			ComboBoxFailHandler.Properties.Items.AddRange(actionHandlers.BuildImageComboBoxItems().ToList());
		}

		private void ReloadSuccessHandlers(IEnumerable<HandlerDescription> actionHandlers)
		{

			ComboBoxSuccessHandler.Properties.Items.Clear();
			ComboBoxSuccessHandler.Properties.Items.AddRange(actionHandlers.BuildImageComboBoxItems().ToList());
		}

		private void ReloadActionHandlers(IEnumerable<HandlerDescription> actionHandlers)
		{
			ComboBoxActionHandler.Properties.Items.Clear();
			ComboBoxActionHandler.Properties.Items.AddRange(actionHandlers.BuildImageComboBoxItems().ToList());

			CustomProcessCredentialsAction.Properties.Items.Clear();
			CustomProcessCredentialsAction.Properties.Items.AddRange(actionHandlers.BuildImageComboBoxItems().ToList());
		}

		private void ReloadValidationWarningHandlers(IEnumerable<HandlerDescription> validationHandlers)
		{
			ComboBoxValidationWarningHandler.Properties.Items.Clear();
			ComboBoxValidationWarningHandler.Properties.Items.AddRange(validationHandlers.BuildImageComboBoxItems().ToList());
		}

		private void ReloadValidationErrorHandlers(IEnumerable<HandlerDescription> validationHandlers)
		{
			ComboBoxValidationErrorHandler.Properties.Items.Clear();
			ComboBoxValidationErrorHandler.Properties.Items.AddRange(validationHandlers.BuildImageComboBoxItems().ToList());
		}


		private void ReloadValidationRuleWarningHandlers(IEnumerable<string> validationRuleHandlers)
		{
			ComboBoxValidationRuleWarningHandler.Properties.Items.Clear();
			ComboBoxValidationRuleWarningHandler.Properties.Items.AddRange(validationRuleHandlers.BuildImageComboBoxItemsString().ToList());
		}

		private void ReloadValidationRuleErrorHandlers(IEnumerable<string> validationRuleHandlers)
		{
			ComboBoxValidationRuleErrorHandler.Properties.Items.Clear();
			ComboBoxValidationRuleErrorHandler.Properties.Items.AddRange(validationRuleHandlers.BuildImageComboBoxItemsString().ToList());
		}

        private void ReloadDeleteDocumentValidationRuleHandlers(IEnumerable<HandlerDescription> validationRuleHandlers)
        {
            ComboBoxDeleteDocumentValidationRuleHandler.Properties.Items.Clear();
            ComboBoxDeleteDocumentValidationRuleHandler.Properties.Items.AddRange(validationRuleHandlers.BuildImageComboBoxItems().ToList());
        }

	    private void ButtonAddTransitionClick(object sender, EventArgs e)
	    {
	        ProcessBuilder.AddTransition(TransitionName, StateFrom, ValidationPointError, ValidationPointWarning,
	            ActionPoint, SuccessPoint,null, FailPoint, null, ValidationRuleWarning, ValidationRuleError, DeletingDocumentValidationRuleError, null,CredentialsType, CredentialsPoint);

	        ClearValues();
	    }

	    void ButtonDeleteTransitionClick(object sender, EventArgs e)
		{
			if (_selectedTransitionName != null)
			{
				ProcessBuilder.DeleteTransition(_selectedTransitionName);
			}
		}

		private void CreateProcessFromControls()
		{
			ProcessBuilder.BuildProcess(Guid.NewGuid().ToString(), TextEditProcessName.Text, TextEditProcessCaption.Text,
						 (bool)CheckEditWithState.EditValue
							 ? (int)WorkflowTypes.WithState
							 : (int)WorkflowTypes.WithoutState);
		}


		private void ClearValues()
		{
			TextEditFromState.EditValue = null;
			ComboBoxFailHandler.EditValue = null;
			ComboBoxSuccessHandler.EditValue = null;
			ComboBoxValidationWarningHandler.EditValue = null;
			ComboBoxValidationErrorHandler.EditValue = null;
			ComboBoxActionHandler.EditValue = null;
			ComboBoxValidationRuleWarningHandler.EditValue = null;
			ComboBoxValidationRuleErrorHandler.EditValue = null;
		    ComboBoxDeleteDocumentValidationRuleHandler.EditValue = null;
		}


		public void OnInitTemplate()
		{
			ReloadValidationWarningHandlers(ValidationHandlers);

			ReloadValidationErrorHandlers(ValidationHandlers);

			ReloadActionHandlers(ActionHandlers);

			ReloadSuccessHandlers(ActionHandlers);

			ReloadFailHandlers(ActionHandlers);

			ReloadValidationRuleWarningHandlers(ValidationWarnings);			

			ReloadValidationRuleErrorHandlers(ValidationErrors);

            ReloadDeleteDocumentValidationRuleHandlers(ValidationHandlers);
           
		}

		public IEnumerable<HandlerDescription> ActionHandlers { get; set; }
		public IEnumerable<HandlerDescription> ValidationHandlers { get; set; }
		public IEnumerable<string> ValidationWarnings { get; set; }
		public IEnumerable<string> ValidationErrors { get; set; }
        public IEnumerable<string> DeleteDocumentValidationErrors { get; set; }
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
			var sourceTransitions = LoadTransitionStates();
			sourceTransitionStates.DataSource = sourceTransitions;

			TextEditProcessName.EditValue = _process != null ? _process.Name : string.Empty;
			TextEditProcessCaption.EditValue = _process != null ? _process.Caption : string.Empty;

		}

		public string ProcessName
		{
			get { return TextEditProcessName.EditValue != null ? TextEditProcessName.EditValue.ToString() : ""; }
		}


		public string TransitionName
		{
			get
			{
				var stateFrom = TextEditFromState.EditValue;

				string transitionName = null;
				if (stateFrom != null )
				{
					transitionName = string.Format("From state {0}", stateFrom);
				}
				return transitionName ?? TextEditProcessName.EditValue.ToString();
			}
		}

		public string StateFrom
		{
			get
			{
                var stateFrom = TextEditFromState.EditValue;
				return stateFrom != null ? stateFrom.ToString() : null;
			}
		}


		public dynamic ValidationPointWarning
		{
			get
			{
				if (ComboBoxValidationWarningHandler.EditValue != null)
				{
					return ViewModelExtension.BuildValidationPointFromString(
												   ((HandlerDescription)ComboBoxValidationWarningHandler.EditValue).HandlerId);
				}
				return null;
			}
		}

		public dynamic ValidationPointError
		{
			get
			{
				if (ComboBoxValidationErrorHandler.EditValue != null)
				{
					return ViewModelExtension.BuildValidationPointFromString(
												   ((HandlerDescription)ComboBoxValidationErrorHandler.EditValue).HandlerId);
				}
				return null;
			}
		}

		public dynamic ActionPoint
		{
			get
			{
				if (ComboBoxActionHandler.EditValue != null)
				{
					return ViewModelExtension.BuildActionPointFromString(
													((HandlerDescription)ComboBoxActionHandler.EditValue).HandlerId);
				}
				return null;
			}
		}

		public dynamic CredentialsPoint
		{
			get
			{
				if (CustomProcessCredentialsAction.EditValue != null)
				{
					return ViewModelExtension.BuildActionPointFromString(
													((HandlerDescription)CustomProcessCredentialsAction.EditValue).HandlerId);
				}
				return null;
			}
		}

		public string CredentialsType
		{
			get
			{
				return CustomProcessCredentialsEditor.EditValue as string;
			}
		}

		public string ValidationRuleWarning
		{
			get
			{
				if (ComboBoxValidationRuleWarningHandler.EditValue != null)
				{
					return ComboBoxValidationRuleWarningHandler.EditValue.ToString();
				}
				return null;
			}
		}


		public string ValidationRuleError
		{
			get
			{
				if (ComboBoxValidationRuleErrorHandler.EditValue != null)
				{
					return ComboBoxValidationRuleErrorHandler.EditValue.ToString();
				}
				return null;
			}
		}

        public dynamic DeletingDocumentValidationRuleError
        {
            get
            {
                if (ComboBoxDeleteDocumentValidationRuleHandler.EditValue != null)
                {
                    return ViewModelExtension.BuildValidationPointFromString(((HandlerDescription)ComboBoxDeleteDocumentValidationRuleHandler.EditValue).HandlerId);
                }
                return null;
            }
        }

		public dynamic SuccessPoint
		{
			get
			{
				if (ComboBoxSuccessHandler.EditValue != null)
				{
					return ViewModelExtension.BuildSuccessPointFromString(
												 ((HandlerDescription)ComboBoxSuccessHandler.EditValue).HandlerId);
				}
				return null;
			}
		}

		public dynamic FailPoint
		{
			get
			{
				if (ComboBoxFailHandler.EditValue != null)
				{
					return ViewModelExtension.BuildFailPointFromString(
												 ((HandlerDescription)ComboBoxFailHandler.EditValue).HandlerId);
				}
				return null;
			}
		}

		private void ButtonSaveProcessClick(object sender, EventArgs e)
		{
			CreateProcessFromControls();
			PanelTransitions.Visible = true;
		}


		public EditMode EditMode
		{
			get { return _editMode; }
			set
			{
				_editMode = value;
				PanelTransitions.Visible = _editMode != EditMode.ModeCreate;
				ButtonSaveProcess.Visible = _editMode == EditMode.ModeCreate;
			}
		}

		private void CustomProcessCredentialsEditor_Properties_EditValueChanged(object sender, EventArgs e)
		{
			var isCustomCredentials = CustomProcessCredentialsEditor.EditValue.ToString() == AuthorizationStorageExtensions.CustomCredentials;

			LabelCredentialsAction.Visible = isCustomCredentials;
			CustomProcessCredentialsAction.Visible = isCustomCredentials;
		}
	}
}
