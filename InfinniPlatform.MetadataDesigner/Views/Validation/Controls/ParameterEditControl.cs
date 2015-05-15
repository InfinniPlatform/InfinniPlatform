using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InfinniPlatform.MetadataDesigner.Views.Validation.Controls
{
    public partial class ParameterEditControl : UserControl
    {
        private readonly string _parameterName;
        private readonly bool _isOptional;
        private readonly bool _isCollection;
        private readonly bool _isDescriptionMessage;

        public ParameterEditControl(
            string parameterName, 
            bool isOptional = false, 
            bool isCollection = false,
            bool isDataTypeEditable = false,
            bool isDescriptionMessage = false)
        {
            _parameterName = parameterName;
            _isOptional = isOptional;
            _isCollection = isCollection;
            _isDescriptionMessage = isDescriptionMessage;

            InitializeComponent();

            parameterLabel.Text = "Parameter '" + parameterName + "':";

            dontUseParameter.Visible = _isOptional;
            CommaPromptLabel.Visible = _isCollection;

            if (isDataTypeEditable)
            {
                Height = 60;
                DataTypeLabel.Visible = true;
                DataTypeComboBox.Visible = true;
            }
            else
            {
                Height = 30;
                DataTypeLabel.Visible = false;
                DataTypeComboBox.Visible = false;
            }

            if (_isDescriptionMessage)
            {
                Height = 60;
                parameterValueEdit.Visible = false;
                MessageMemoEdit.Visible = true;
            }
            else
            {
                parameterValueEdit.Visible = true;
                MessageMemoEdit.Visible = false;
            }
        }

        public bool HasValue
        {
            get
            {
                if (_isOptional && dontUseParameter.Checked)
                {
                    return false;
                }
                return true;
            }
        }

        public string ParameterName
        {
            get
            {
                return _parameterName.First().ToString().ToUpper() + _parameterName.Substring(1);
            }
        }

        public object GetUserInput
        {
            get
            {
                object result;

                if (_isCollection)
                {
                    result = parameterValueEdit.Text.Split(',');

                    if (DataTypeComboBox.Text == "Integer")
                    {
                        var typefiedResult = new List<int>();

                        foreach (var item in parameterValueEdit.Text.Split(','))
                        {
                            int outResult;

                            if (!int.TryParse(item, out outResult))
                            {
                                MessageBox.Show("Error :" + item);
                            }
                            else
                            {
                                typefiedResult.Add(outResult);
                            }
                        }

                        result = typefiedResult.ToArray();
                    }
                    else if (DataTypeComboBox.Text == "Bool")
                    {
                        var typefiedResult = new List<bool>();

                        foreach (var item in parameterValueEdit.Text.Split(','))
                        {
                            bool outResult;

                            if (!bool.TryParse(item, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + item);
                            }
                            else
                            {
                                typefiedResult.Add(outResult);
                            }
                        }

                        result = typefiedResult.ToArray();
                    }
                    else if (DataTypeComboBox.Text == "DateTime")
                    {
                        var typefiedResult = new List<DateTime>();

                        foreach (var item in parameterValueEdit.Text.Split(','))
                        {
                            DateTime outResult;

                            if (!DateTime.TryParse(item, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + item);
                            }
                            else
                            {
                                typefiedResult.Add(outResult);
                            }
                        }

                        result = typefiedResult.ToArray();
                    }
                    else if (DataTypeComboBox.Text == "Float")
                    {
                        var typefiedResult = new List<double>();

                        foreach (var item in parameterValueEdit.Text.Split(','))
                        {
                            double outResult;

                            if (!double.TryParse(item, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + item);
                            }
                            else
                            {
                                typefiedResult.Add(outResult);
                            }
                        }

                        result = typefiedResult.ToArray();
                    }
                }
                else
                {
                    if (_isDescriptionMessage)
                    {
                        result = MessageMemoEdit.Text;
                    }
                    else
                    {
                        result = parameterValueEdit.Text;

                        if (DataTypeComboBox.Text == "Integer")
                        {
                            int outResult;

                            if (!int.TryParse(parameterValueEdit.Text, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + parameterValueEdit.Text);
                            }
                            else
                            {
                                result = outResult;
                            }
                        }
                        else if (DataTypeComboBox.Text == "Bool")
                        {
                            bool outResult;

                            if (!bool.TryParse(parameterValueEdit.Text, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + parameterValueEdit.Text);
                            }
                            else
                            {
                                result = outResult;
                            }
                        }
                        else if (DataTypeComboBox.Text == "DateTime")
                        {
                            DateTime outResult;

                            if (!DateTime.TryParse(parameterValueEdit.Text, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + parameterValueEdit.Text);
                            }
                            else
                            {
                                result = outResult;
                            }
                        }
                        else if (DataTypeComboBox.Text == "Float")
                        {
                            double outResult;

                            if (!double.TryParse(parameterValueEdit.Text, out outResult))
                            {
                                MessageBox.Show("Invalid parameter value: " + parameterValueEdit.Text);
                            }
                            else
                            {
                                result = outResult;
                            }
                        }
                    }
                }

                return result;
            }
        }

        private void dontUseParameter_CheckedChanged(object sender, EventArgs e)
        {
            parameterValueEdit.Enabled = !dontUseParameter.Checked;
        }
    }
}
