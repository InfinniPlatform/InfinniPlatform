using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.DesignControls.Layout;
using InfinniPlatform.DesignControls.ObjectInspector;
using InfinniPlatform.DesignControls.PropertyDesigner;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.DesignControls
{
    /// <summary>
    ///     Форма дизайнера выступает в качестве медиатора между инспектором объектов, панелью размещения контролов и
    ///     репозиторием контролов
    /// </summary>
    public partial class DesignerControl : UserControl
    {
        private IFocusControl _focusedControl;
        private ObjectInspectorPopupMenu _objectInspectorPopupMenu;

        public DesignerControl()
        {
            InitializeComponent();

            if (!DesignMode)
            {
                InitObjectInspectorTree();
            }
        }

        protected new bool DesignMode
        {
            get
            {
                if (base.DesignMode)
                    return true;

                return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
            }
        }

        public Action<dynamic> OnLayoutChanged { get; set; }
        public dynamic LayoutView { get; private set; }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            ControlRepository.RemoveInstance(ObjectInspector);
        }

        private void InitObjectInspectorTree()
        {
            var controlRepository = ControlRepository.Instance(ObjectInspector);

            _objectInspectorPopupMenu = new ObjectInspectorPopupMenu();
            _objectInspectorPopupMenu.ControlRepository = controlRepository;
            _objectInspectorPopupMenu.Form = FindForm();
            _objectInspectorPopupMenu.SetItemLinks();

            var rootNodeSettings = new PropertiesNode(PanelContainer);
            rootNodeSettings.EnabledLayoutTypes = new List<EnabledItems> {EnabledItems.Layout};

            ObjectInspector.PropertiesRootNode = rootNodeSettings;
            ObjectInspector.ObjectInspectorPopupMenu = _objectInspectorPopupMenu;
            ObjectInspector.ControlRepository = controlRepository;
            ObjectInspector.OnSetFocus = DoSetFocus;
            ObjectInspector.DataSources = DataSourceSurface.DataSources;
            ObjectInspector.Scripts = ScriptSurface.Scripts;
            ObjectInspector.ChildViews = ChildViewSurface.ChildViews;
            PanelContainer.ObjectInspector = ObjectInspector;
            rootNodeSettings.ObjectInspector = ObjectInspector;

            //циклическая ссылка - плохо
            DataSourceSurface.ObjectInspector = ObjectInspector;
            ScriptSurface.ObjectInspector = ObjectInspector;
            ChildViewSurface.ObjectInspector = ObjectInspector;
        }

        private void DoSetFocus(Control focusedControl)
        {
            if (_focusedControl != null)
            {
                _focusedControl.ClearFocus();
            }

            _focusedControl = focusedControl as IFocusControl;
            if (_focusedControl != null)
            {
                _focusedControl.ShowFocus();
            }
        }

        private void ButtonGetLayout_Click(object sender, EventArgs e)
        {
            dynamic layout = GetLayout();
            var valueEdit = new ValueEdit();
            valueEdit.Value = layout.ToString();
            valueEdit.ShowDialog();
        }

        private dynamic GetLayout()
        {
            var layoutProvider = PanelContainer as ILayoutProvider;
            dynamic layout = new DynamicWrapper();

            if (layoutProvider != null)
            {
                dynamic layoutProviderLayout = layoutProvider.GetLayout();
                IEnumerable<dynamic> items = layoutProviderLayout.Items;
                layout.LayoutPanel = new DynamicWrapper();
                if (items.Any())
                {
                    var firstItem = (layoutProviderLayout.Items.First() as IEnumerable).Cast<dynamic>().First();
                    ObjectHelper.SetProperty(layout.LayoutPanel, firstItem.Key, firstItem.Value);
                }

                ObjectHelper.SetProperty(layout, "Caption", layoutProviderLayout.Caption);
                ObjectHelper.SetProperty(layout, "Text", layoutProviderLayout.Text);
                ObjectHelper.SetProperty(layout, "DataSources", DataSourceSurface.GetLayout());
                ObjectHelper.SetProperty(layout, "Scripts", ScriptSurface.GetLayout());
                ObjectHelper.SetProperty(layout, "Parameters", ParametersSurface.GetLayout());
                ObjectHelper.SetProperty(layout, "ChildViews", ChildViewSurface.GetLayout());
                LayoutExtensions.RemoveEmptyEntries(layout);
            }
            return layout;
        }

        private void SetLayoutButton_Click(object sender, EventArgs e)
        {
            var valueEdit = new ValueEdit();
            valueEdit.ReadOnly = false;
            if (valueEdit.ShowDialog() == DialogResult.OK)
            {
                ProcessJson(valueEdit.Value);
            }
        }

        private void ClearLayoutButton_Click(object sender, EventArgs e)
        {
            if (
                MessageBox.Show("Clear all layout?", "Need confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.OK)
            {
                ClearControls();
            }
        }

        private void ClearControls()
        {
            ObjectInspector.RemoveAllElements();
            ObjectInspector.FocusedPropertiesNode = ObjectInspector.PropertiesRootNode;
            DataSourceSurface.Clear();
            ScriptSurface.Clear();
            ParametersSurface.Clear();
            ChildViewSurface.Clear();
        }

        public void ProcessJson(dynamic instance)
        {
            ClearControls();

            var panel = new Panel();
            panel.Dock = DockStyle.Fill;
            ScrollableControl.Controls.Add(panel);
            panel.BringToFront();
            var watch = Stopwatch.StartNew();

            ObjectInspector.BeginUpdate();
            try
            {
                var layoutPanel = instance.LayoutPanel;
                if (layoutPanel != null)
                {
                    var layoutProvider = PanelContainer as ILayoutProvider;
                    var propertiesProvider = PanelContainer as IPropertiesProvider;

                    propertiesProvider.LoadProperties(instance);

                    if (layoutProvider != null)
                    {
                        dynamic instanceLayout = new DynamicWrapper();
                        instanceLayout.Items = new List<dynamic>();
                        instanceLayout.Items.Add(layoutPanel);


                        layoutProvider.SetLayout(instanceLayout);
                        ((IAlignment) layoutProvider).AlignControls();
                    }
                }
                var dataSources = instance.DataSources;
                if (dataSources != null)
                {
                    DataSourceSurface.ProcessJson(dataSources);
                }
                var scripts = instance.Scripts;
                if (scripts != null)
                {
                    ScriptSurface.ProcessJson(scripts);
                }
                var parameters = instance.Parameters;
                if (parameters != null)
                {
                    ParametersSurface.ProcessJson(parameters);
                }
                var childViews = instance.ChildViews;
                if (childViews != null)
                {
                    ChildViewSurface.ProcessJson(childViews);
                }
            }
            finally
            {
                ObjectInspector.EndUpdate();
                ScrollableControl.Controls.Remove(panel);

                watch.Stop();
                LabelRendered.Text = string.Format("Rendered in {0}", watch.ElapsedMilliseconds);
            }
        }

        private void ProcessJson(string value)
        {
            dynamic instance = null;
            try
            {
                instance = value.ToDynamic();
            }
            catch
            {
                MessageBox.Show("Can't parse json object");
                return;
            }

            ProcessJson(instance);
        }

        private void SetSizeButton_Click(object sender, EventArgs e)
        {
            ControlRepository.RemoveInstance(ObjectInspector);
            var controlRepository = ControlRepository.Instance(ObjectInspector);
            _objectInspectorPopupMenu.ControlRepository = controlRepository;
            ObjectInspector.ControlRepository = controlRepository;

            PanelContainer.Width = Convert.ToInt32(TextEditWidth.EditValue);
            PanelContainer.Height = Convert.ToInt32(TextEditHeight.EditValue);


            dynamic layout = GetLayout();
            ProcessJson(layout.ToString());
        }

        private void PanelContainer_Scroll(object sender, ScrollEventArgs e)
        {
            PanelContainer.Refresh();
        }

        private void GenerateViewButton_Click(object sender, EventArgs e)
        {
            LayoutView = GetLayout();
            if (OnLayoutChanged != null)
            {
                OnLayoutChanged(LayoutView);
            }
        }
    }
}