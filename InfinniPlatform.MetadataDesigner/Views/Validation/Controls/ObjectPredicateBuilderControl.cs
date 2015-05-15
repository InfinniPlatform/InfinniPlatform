using InfinniPlatform.MetadataDesigner.Views.Validation.Builders;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    public partial class ObjectPredicateBuilderControl : UserControl, IPredicateBuilderControl
    {
        public ObjectPredicateBuilderControl()
        {
            InitializeComponent();

            // Добавляем 4 стандартных метода валидации объекта (не из расширений)
            operationComboBox.Properties.Items.Add("Or");
            operationComboBox.Properties.Items.Add("And");
            operationComboBox.Properties.Items.Add("Property");
            operationComboBox.Properties.Items.Add("Collection");

            var extensionMethods = 
                typeof (ObjectValidationBuilder).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(m => m.Name);  

            operationComboBox.Properties.Items.AddRange(extensionMethods.Distinct().ToArray());

            parametersPanel.Controls.Clear();

            operationComboBox.SelectedIndex = 20;
        }

        public PredicateDescriptionNode GetPredicateDescription()
        {
            var validationType = PredicateDescriptionType.Object;

            if (operationComboBox.SelectedIndex < 4)
            {
                validationType = PredicateDescriptionType.ObjectBasePredicate;
            }

            var parameterNames = new List<string>();
            var parameters = new List<object>();

            foreach (ParameterEditControl editControl in parametersPanel.Controls)
            {
                if (editControl.HasValue)
                {
                    parameterNames.Add(editControl.ParameterName);
                    parameters.Add(editControl.GetUserInput);
                }
            }

            return new PredicateDescriptionNode(validationType, operationComboBox.Text, parameterNames.ToArray(), parameters.ToArray());
        }

        public PredicateDescriptionType GetNextControlType()
        {
            if (operationComboBox.Text == "Property")
            {
                return PredicateDescriptionType.ObjectComposite;
            }

            if (operationComboBox.Text == "Collection")
            {
                return PredicateDescriptionType.CollectionComposite;
            }

            return PredicateDescriptionType.ObjectBasePredicate;
        }

        private void operationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            parametersPanel.Controls.Clear();

            if (operationComboBox.SelectedIndex < 4)
            {
                if (operationComboBox.Text == "Property" ||
                    operationComboBox.Text == "Collection")
                {
                    parametersPanel.Controls.Add(new ParameterEditControl("property"));
                }
            }
            else
            {
                var extensionMethods =
                    typeof (ObjectValidationBuilder).GetMethods(BindingFlags.Public | BindingFlags.Static)
                    .Where(m => m.Name == operationComboBox.Text && !m.IsGenericMethod).ToArray();

                var hasOptionalParameterProperty = false;

                MethodInfo selectedMethodInfo = null;

                if (extensionMethods.Length == 1)
                {
                    selectedMethodInfo = extensionMethods[0];
                }
                else if (extensionMethods.Length == 2 &&
                    Math.Abs(extensionMethods[0].GetParameters().Count() - extensionMethods[1].GetParameters().Count()) == 1)
                {
                    // Это значит было найдено две перегрузки - в одной из которых дополнительный параметр

                    hasOptionalParameterProperty = true;
                    selectedMethodInfo = extensionMethods[0].GetParameters().Count() > extensionMethods[1].GetParameters().Count() ? extensionMethods[1] : extensionMethods[0];
                }

                int yIndent = 0;

                if (hasOptionalParameterProperty)
                {
                    var control = new ParameterEditControl("property", true) {Location = new Point(0, yIndent)};

                    parametersPanel.Controls.Add(control);

                    yIndent += control.Height;
                }

                if (selectedMethodInfo != null)
                {
                    foreach (var parameter in selectedMethodInfo.GetParameters().Skip(1))
                    {
                        var isDatatypeEditable = false;

                        if (parameter.Name == "value")
                        {
                            if (selectedMethodInfo.Name.Contains("Equal"))
                            {
                                isDatatypeEditable = true;
                            }
                        }
                        else if (parameter.Name == "items")
                        {
                            isDatatypeEditable = true;
                        }

                        var control = new ParameterEditControl(
                            parameter.Name, 
                            false,
                            parameter.ParameterType.Name == "IEnumerable",
                            isDatatypeEditable,
                            parameter.Name == "message")
                        {
                            Location = new Point(0, yIndent)
                        };

                        parametersPanel.Controls.Add(control);

                        yIndent += control.Height;
                    }
                }
            }
        }
    }
}
