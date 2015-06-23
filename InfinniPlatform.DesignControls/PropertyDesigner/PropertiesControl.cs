using System;
using System.Drawing;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public partial class PropertiesControl : UserControl, ILayoutProvider, ILayoutContainer, IAlignment,
        IClientHeightProvider, IInspectedItem, IFocusControl
    {
        private Control _control;

        public PropertiesControl()
        {
            InitializeComponent();
        }

        public Control Control
        {
            get { return _control; }
            set
            {
                _control = value;
                var userControl = _control;
                if (userControl != null)
                {
                    PanelDataControl.Controls.Add(userControl);


                    userControl.Dock = DockStyle.Fill;

                    var propertiesProvider = _control as IPropertiesProvider;
                    if (propertiesProvider == null)
                    {
                        ButtonProperties.Visible = false;
                        ControlNameLabel.Visible = false;
                    }
                }
            }
        }

        public void AlignControls()
        {
            var alignment = Control as IAlignment;
            if (alignment != null)
            {
                alignment.AlignControls();
            }
        }

        public int GetClientHeight()
        {
            var clientProvider = Control as IClientHeightProvider;
            if (clientProvider != null)
            {
                return clientProvider.GetClientHeight() + 12; // + padding of propertiescontrol
            }
            return 0;
        }

        public bool IsFixedHeight()
        {
            var clientProvider = Control as IClientHeightProvider;
            if (clientProvider != null)
            {
                return clientProvider.IsFixedHeight();
            }
            return true;
        }

        public void ShowFocus()
        {
            PanelDataControl.Appearance.BackColor = Color.LightSkyBlue;
        }

        public void ClearFocus()
        {
            PanelDataControl.Appearance.BackColor = Color.White;
        }

        public ObjectInspectorTree ObjectInspector { get; set; }

        public void InsertLayout(dynamic layout)
        {
            var layoutContainer = Control as ILayoutContainer;
            if (layoutContainer != null)
            {
                layoutContainer.InsertLayout(layout);
            }
        }

        public dynamic GetLayout()
        {
            var layoutProvider = Control as ILayoutProvider;
            if (layoutProvider != null)
            {
                return layoutProvider.GetLayout();
            }
            return new DynamicWrapper();
        }

        public void SetLayout(dynamic value)
        {
            var layoutProvider = Control as ILayoutProvider;
            if (layoutProvider != null)
            {
                layoutProvider.SetLayout(value);
            }
        }

        public string GetPropertyName()
        {
            var layoutProvider = Control as ILayoutProvider;
            if (layoutProvider != null)
            {
                return layoutProvider.GetPropertyName();
            }
            return string.Empty;
        }

        private void ButtonProperties_Click(object sender, EventArgs e)
        {
            if (ObjectInspector != null)
            {
                ObjectInspector.SelectElement(_control);
                var showingNode = ObjectInspector.FocusedPropertiesNode;
                ObjectInspector.ShowPropertyGrid(_control, showingNode);
            }
        }

        public void SetSimpleProperty(string propertyName, object propertyValue)
        {
            var propertiesProvider = _control as IPropertiesProvider;
            if (propertiesProvider != null)
            {
                var props = propertiesProvider.GetSimpleProperties();
                if (props.ContainsKey(propertyName))
                {
                    props[propertyName].Value = propertyValue;
                }
            }
        }

        public void SetControlCaption(string controlCaption)
        {
            ControlNameLabel.Text = controlCaption;
        }
    }
}