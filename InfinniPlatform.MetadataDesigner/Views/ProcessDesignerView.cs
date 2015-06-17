using DevExpress.XtraEditors.Controls;

using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.MetadataDesigner.Views.ProcessTemplates;
using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;

using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.Api.Deprecated;

namespace InfinniPlatform.MetadataDesigner.Views
{

	public enum EditMode { ModeCreate, ModeUpdate }

	/// <summary>
	/// Элемент управления для редактирования процессов.
	/// </summary>
	public sealed partial class ProcessDesignerView : UserControl, IProcessBuilder
	{


		public ProcessDesignerView()
		{
			InitializeComponent();

			ProcessTemplateEditor.Properties.Items.AddRange(new ProcessTemplateSelector().GetTemplateList().Select(i => new ImageComboBoxItem(i, i)).ToList());
		}

		private IProcessTemplate _selectedTemplate;

		private void ReloadExistingHandlers()
		{
			IEnumerable<HandlerDescription> actionHandlers = null;
			IEnumerable<HandlerDescription> validationHandlers = null;
			IEnumerable<string> validationRuleWarningHandlers = null;
			IEnumerable<string> validationRuleErrorHandlers = null;
			IEnumerable<string> states = null;

			actionHandlers = ViewModelExtension.BuildActionHandlerDescriptions(Version(), ConfigId(), DocumentId());
            validationHandlers = ViewModelExtension.BuildValidationHandlerDescriptions(Version(), ConfigId(), DocumentId());
            validationRuleWarningHandlers = ViewModelExtension.BuildValidationRuleWarningDescriptions(Version(), ConfigId(), DocumentId());
            validationRuleErrorHandlers = ViewModelExtension.BuildValidationRuleErrorDescriptions(Version(), ConfigId(), DocumentId());
			
			_selectedTemplate.ActionHandlers = actionHandlers;
			_selectedTemplate.ValidationErrors = validationRuleErrorHandlers;
			_selectedTemplate.ValidationWarnings = validationRuleWarningHandlers;
			_selectedTemplate.ValidationHandlers = validationHandlers;
			_selectedTemplate.DocumentStates = states;
		}

		private dynamic _process;

		public void AddTransition(
			string transitionName, string stateFrom, object validationPointError,
			object validationPointWarning, object actionPoint, object successPoint, object registerPoint, object failPoint, object deletePoint,
			string validationRuleWarning, string validationRuleError, string defaultValuesSchema, string credentialsType, object credentialsPoint)
		{
			object transition = null;
			try
			{
				transition = new DynamicWrapper().BuildId(Guid.NewGuid().ToString())
					.BuildName(transitionName);

				if (stateFrom != null)
				{
                    //TODO Убрать комбо-бокс для статусов
                    //transition.BuildProperty("StateFrom",
                    //    ViewModelExtension.BuildStatusByName(ConfigId(), DocumentId(), stateFrom));
				}

				if (validationPointError != null)
				{
					transition.BuildProperty("ValidationPointError", validationPointError);
				}

				if (validationPointWarning != null)
				{
					transition.BuildProperty("ValidationPointWarning", validationPointWarning);
				}


				if (actionPoint != null)
				{
					transition.BuildProperty("ActionPoint", actionPoint);
				}

				if (successPoint != null)
				{
					transition.BuildProperty("SuccessPoint", successPoint);
				}

				if (registerPoint != null)
				{
					transition.BuildProperty("RegisterPoint", registerPoint);
				}

				if (failPoint != null)
				{
					transition.BuildProperty("FailPoint", failPoint);
				}

				if (deletePoint != null)
				{
					transition.BuildProperty("DeletePoint", deletePoint);
				}

				if (validationRuleWarning != null)
				{
					transition.BuildProperty("ValidationRuleWarning", validationRuleWarning);
				}

				if (validationRuleError != null)
				{
					transition.BuildProperty("ValidationRuleError", validationRuleError);
				}

				if (defaultValuesSchema != null)
				{
					transition.BuildProperty("SchemaPrefill", defaultValuesSchema);
				}

				if (credentialsPoint != null)
				{
					transition.BuildProperty("CredentialsPoint", credentialsPoint);
				}

				if (credentialsType != null)
				{
					transition.BuildProperty("CredentialsType", credentialsType);
				}

				_process.Transitions.Add(transition);
				OnValueChanged(_process, new EventArgs());
				_selectedTemplate.Process = _process;

			}
			catch
			{
				MessageBox.Show("Ошибка создания перехода");
			}
		}

		public void DeleteTransition(string transitionName)
		{
			IEnumerable<dynamic> transitions = _process.Transitions;

			if (transitions == null)
			{
				_process.Transitions = new List<dynamic>();
			}
			else
			{
				_process.Transitions =
					transitions.Where(tr => tr.Name != transitionName).ToList();
			}

			OnValueChanged(_process, new EventArgs());
			_selectedTemplate.Process = _process;
		}

		public void EditTransition(dynamic transition)
		{
			IEnumerable<dynamic> transitions = _process.Transitions;
			_process.Transitions = transitions.Where(tr => tr.Name != transition.Name).ToList();
			_process.Transitions.Add(transition);
			OnValueChanged(_process, new EventArgs());
			_selectedTemplate.Process = _process;
		}

		public void BuildProcess(string id, string name, string caption, int workflowType)
		{
			_process = new DynamicWrapper().BuildId(id)
										   .BuildName(name)
										   .BuildCaption(caption)
										   .BuildProperty("Type", workflowType);
			_process.Transitions = new List<dynamic>();
			_process.SettingsType = ProcessTemplateEditor.EditValue;

			OnValueChanged(_process, new EventArgs());
			_selectedTemplate.Process = _process;
		}



		private void ProcessTemplateEditorEditValueChanged(object sender, EventArgs e)
		{
			if (ProcessTemplateEditor.EditValue != null)
			{
				var control = new ProcessTemplateSelector().GetTemplate(ProcessTemplateEditor.EditValue.ToString());

				PanelBusinessProcessTemplate.Controls.Clear();

				if (control != null)
				{
					PanelBusinessProcessTemplate.Controls.Add(control);
					control.Dock = DockStyle.Fill;
					//var initialized = (IInitialized) control;
					//initialized.Init();

					_selectedTemplate = control as IProcessTemplate;
					if (_selectedTemplate != null)
					{
						_selectedTemplate.EditMode = EditMode;
						_selectedTemplate.ConfigId = ConfigId();
						_selectedTemplate.DocumentId = DocumentId();
						_selectedTemplate.ProcessBuilder = this;
						ReloadExistingHandlers();
						_selectedTemplate.OnInitTemplate();
						_selectedTemplate.Process = _process;
					}
				}
			}

		}

		public Func<string> ConfigId { get; set; }

		public Func<string> DocumentId { get; set; }

        public Func<string> Version { get; set; } 

		public EditMode EditMode { get; set; }

		public object Value
		{
			get { return _process; }
			set
			{
				dynamic process = value.ToDynamic();

				if (process == null ||
					string.IsNullOrEmpty(process.Name))
				{
					EditMode = EditMode.ModeCreate;
					ProcessTemplateEditor.EditValue = new ProcessTemplateSelector().GetDefaultTemplate();
				}
				else
				{
					_process = process;
					EditMode = EditMode.ModeUpdate;

					// Если раскомментировать следующие строчки,
					// выбор шаблона процесса блокируется
					ProcessTemplateEditor.EditValue = _process.SettingsType;
					ProcessTemplateEditor.Properties.ReadOnly = true;
				}
			}
		}

		public event EventHandler OnValueChanged;
	}
}