using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;

using InfinniPlatform.MetadataDesigner.Views.Status;
using InfinniPlatform.MetadataDesigner.Views.ViewModel;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views
{
	/// <summary>
	/// Элемент управления для проверки конфигурации.
	/// </summary>
	public sealed partial class ConfigVerifyDesignerView : UserControl
	{
		public ConfigVerifyDesignerView()
		{
			InitializeComponent();
		}

        public dynamic Value { get; set; }

        public string Version { get; set; }


        private void ConfigVerifyDesignerView_Load(object sender, EventArgs e)
        {
			MigrationsComboBox.Properties.Items.AddRange(ViewModelExtension.BuildMigrations());
            VerificationsComboBox.Properties.Items.AddRange(ViewModelExtension.BuildVerifications());
        }

        /// <summary>
        /// Нажатие на кнопку применения миграции
        /// </summary>
        private void UpButton_Click(object sender, EventArgs e)
        {
            var lines = new List<string>(MigrationMemoEdit.Lines)
            {
                string.Format("Migration {0} started.", MigrationsComboBox.Text)
            };

            string resultString = null;
            
            var process = new StatusProcess();
            process.StartOperation(() =>
            {
                resultString = ViewModelExtension.RunMigration(Version, Value.Name.ToString(), MigrationsComboBox.Text, GetParameterValues());
            });
            process.EndOperation();
            
            lines.AddRange(resultString
                .Replace("\\r", "\r")
                .Replace("\\n", "\n")
                .Replace("\"", "") 
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));

            MigrationMemoEdit.Lines = lines.ToArray();

            MigrationMemoEdit.SelectionStart = MigrationMemoEdit.Text.Length;
            MigrationMemoEdit.ScrollToCaret();
        }

        /// <summary>
        /// Нажатие на кнопку отката миграции
        /// </summary>
        private void DownButton_Click(object sender, EventArgs e)
        {
            var lines = new List<string>(MigrationMemoEdit.Lines)
            {
                string.Format("Migration {0} revert started.", MigrationsComboBox.Text)
            };

            string resultString = null;

            var process = new StatusProcess();
            process.StartOperation(() =>
            {
                resultString = ViewModelExtension.RevertMigration(Version, Value.Name.ToString(), MigrationsComboBox.Text, GetParameterValues());
            });
            process.EndOperation();

            lines.AddRange(resultString
                .Replace("\\r", "\r")
                .Replace("\\n", "\n")
                .Replace("\"", "")
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));

            MigrationMemoEdit.Lines = lines.ToArray();

            MigrationMemoEdit.SelectionStart = MigrationMemoEdit.Text.Length;
            MigrationMemoEdit.ScrollToCaret();
        }
        
        /// <summary>
        /// Нажатие на кнопку запуска верификации
        /// </summary>
        private void CheckButton_Click(object sender, EventArgs e)
        {
            var lines = new List<string>(VerificationMemoEdit.Lines)
            {
                string.Format("Verification {0} started.", VerificationsComboBox.Text)
            };

            string resultString = null;

            var process = new StatusProcess();
            process.StartOperation(() =>
            {
                resultString = ViewModelExtension.RunVerification(Version, Value.Name.ToString(), VerificationsComboBox.Text);
            });
            process.EndOperation();

            lines.AddRange(resultString
                .Replace("\\r", "\r")
                .Replace("\\n", "\n")
                .Replace("\"", "")
                .Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries));

            VerificationMemoEdit.Lines = lines.ToArray();

            VerificationMemoEdit.SelectionStart = VerificationMemoEdit.Text.Length;
            VerificationMemoEdit.ScrollToCaret();
        }

        /// <summary>
        /// Нажатие на кнопку запуска всех верификаций
        /// </summary>
        private void RunAllChecksButton_Click(object sender, EventArgs e)
        {
            var lines = new List<string>
            {
                "Full verification started."
            };

            var process = new StatusProcess();
            process.StartOperation(() =>
            {
                foreach (string verification in ViewModelExtension.BuildVerifications())
                {
                    string resultString = ViewModelExtension.RunVerification(Version, Value.Name.ToString(), verification);

                    lines.Add(string.Format("Verification {0} started.", verification));

                    lines.AddRange(resultString
                        .Replace("\\r", "\r")
                        .Replace("\\n", "\n")
                        .Replace("\"", "")
                        .Split(new[] {"\r\n", "\n"}, StringSplitOptions.RemoveEmptyEntries));
                }
            });
            process.EndOperation();

            VerificationMemoEdit.Lines = lines.ToArray();

            VerificationMemoEdit.SelectionStart = VerificationMemoEdit.Text.Length;
            VerificationMemoEdit.ScrollToCaret();
        }

        /// <summary>
        /// Получение значений параметров миграции, введенных пользователем
        /// </summary>
        private object[] GetParameterValues()
        {
            var parameters = new List<object>();

            // Получение параметров миграции
            foreach (var control in ParametersPanelControl.Controls)
            {
                var comboBox = control as ComboBoxEdit;
                if (comboBox != null)
                {
                    parameters.Add(comboBox.Text);
                }
                else
                {
                    var textEdit = control as TextEdit;
                    if (textEdit != null)
                    {
                        parameters.Add(textEdit.Text);
                    }

                    var checkBoxEdit = control as CheckBox;
                    if (checkBoxEdit != null)
                    {
                        parameters.Add(checkBoxEdit.Checked);
                    }
                }
            }
            return parameters.ToArray();
        }

        /// <summary>
        /// Выбрана другая миграция - необходимо обновить контролы для ввода параметров
        /// </summary>
        private void MigrationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedMigration = MigrationsComboBox.Text;
            dynamic migration = null;

            var process = new StatusProcess();
            process.StartOperation(() =>
            {
                migration = ViewModelExtension.BuildMigrationDetails(Version, Value.Name.ToString(), selectedMigration);
            });
            process.EndOperation();

            if (migration == null) return;

            MigrationDescriptionLabelControl.Text = migration.Description;
            DownButton.Visible = migration.IsUndoable;

            if (migration.Parameters.Length == 0)
            {
                ParametersPanelControl.Visible = false;
                MigrationParametersControl.Visible = false;
            }
            else
            {
                ParametersPanelControl.Visible = true;
                MigrationParametersControl.Visible = true;
                ParametersPanelControl.Controls.Clear();
            }

            var counter = 0;
            foreach (var parameter in migration.Parameters)
            {
                if (parameter.PossibleValues != null && parameter.PossibleValues.Count > 0)
                {
                    AddComboBoxForParameter(parameter.Caption.ToString(), parameter.PossibleValues, counter++);
                    continue;
                }

                if (parameter.InitialValue is bool)
                {
                    AddCheckBoxForParameter(parameter.Caption.ToString(), parameter.InitialValue, counter++);
                }
                else
                {
                    AddTextEditForParameter(parameter.Caption.ToString(), counter++);
                }

            }
        }

        private void VerificationsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedVerification = VerificationsComboBox.Text;

            var verification = ViewModelExtension.BuildVerificationDetails(selectedVerification);

            if (verification != null)
            {
                VerificationDescriptionLabelControl.Text = verification.Description;
            }
        }

        private void AddComboBoxForParameter(string parameterName, dynamic rawPossibleValues, int order)
        {
            var labelControl = new LabelControl
            {
                Text = parameterName + ":", 
                Location = new Point(10, 12 + order*24),
                AutoSize = true
            };

            var parameterControl = new ComboBoxEdit
            {
                Location = new Point(140, 10 + order*24),
                Size = new Size(210, 20)
            };
	        parameterControl.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            parameterControl.Properties.DropDownRows = 25;
            parameterControl.Properties.Sorted = true;

            var possibleValues = new List<string>();
            foreach (var possibleValue in rawPossibleValues)
            {
                string value = possibleValue.ToString();

                if (value.Length > 100)
                {
                    value = value.Substring(0, 100) + "...";
                }

                possibleValues.Add(value);
            }
            
            parameterControl.Properties.Items.AddRange(possibleValues);
            parameterControl.SelectedIndex = 0;

            ParametersPanelControl.Controls.Add(parameterControl);
            ParametersPanelControl.Controls.Add(labelControl);
	    }

        private void AddTextEditForParameter(string parameterName, int order)
        {
            var labelControl = new LabelControl
            {
                Text = parameterName + ":",
                Location = new Point(10, 12 + order * 24),
                AutoSize = true
            };

            var parameterControl = new TextEdit
            {
                Location = new Point(140, 10 + order * 24),
                Size = new Size(190, 20)
            };

            ParametersPanelControl.Controls.Add(parameterControl);
            ParametersPanelControl.Controls.Add(labelControl);
        }

        private void AddCheckBoxForParameter(string parameterName, bool initialValue, int order)
        {
            var control = new CheckBox
            {
                Text = parameterName,
                Location = new Point(10, 12 + order * 24),
                AutoSize = true,
                Checked = initialValue
            };

            ParametersPanelControl.Controls.Add(control);
        }
	}
}