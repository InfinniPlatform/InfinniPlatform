using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.QueryDesigner.Forms;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class DesignControl : UserControl
    {
        private readonly RunTimeController _controller;
        private readonly QueryConstructorFromConfig _fromPart = new QueryConstructorFromConfig();
        private readonly QueryConstructorSelectConfig _selectPart = new QueryConstructorSelectConfig();
        private readonly QueryConstructorWhereConfig _wherePart = new QueryConstructorWhereConfig();

        public DesignControl()
        {
            InitializeComponent();

            _controller = new RunTimeController(Controls.Cast<Control>().ToList());
            _controller.RegisterControl(_fromPart);
            _fromPart.ShowAlias = false;
            _controller.RegisterControl(_selectPart);
            _controller.RegisterControl(_wherePart);
        }

        private void ButtonCreateQueryClick(object sender, EventArgs e)
        {
            CreateQuery();
        }

        public string CreateQuery()
        {
            var queryInstance = new DynamicWrapper();

            var providers = _controller.GetQueryPartProviders();
            foreach (var queryBlockProvider in providers.OrderBy(p => p.GetConstructOrder()).ToList())
            {
                if (!queryBlockProvider.DefinitionCompleted())
                {
                    MessageBox.Show(queryBlockProvider.GetErrorMessage());
                    break;
                }
                queryBlockProvider.ProcessQuery(queryInstance);
            }
            return queryInstance.ToString();
        }

        private static HostingConfig GetHostingConfig()
        {
            return HostingConfig.Default;
        }

        private void OnLoadDesigner(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }


            InitSyntaxTree();

            Initialize(null);
        }

        private void InitSyntaxTree()
        {
            _fromPart.OnConfigurationValueChanged += _wherePart.NotifyFromConfigurationChanged;
            _fromPart.OnDocumentValueChanged += _wherePart.NotifyFromDocumentChanged;

            _fromPart.OnConfigurationValueChanged += _selectPart.NotifyFromConfigurationChanged;
            _fromPart.OnDocumentValueChanged += _selectPart.NotifyFromDocumentChanged;
            _fromPart.OnAliasValueChanged += _selectPart.NotifyFromAliasChanged;

            SyntaxTree.OnAddFromControl = link => _fromPart;
            SyntaxTree.OnAddJoinControl = link =>
            {
                var joinPart = new QueryConstructorJoinConfig();

                Initialize(joinPart);

                _fromPart.OnConfigurationValueChanged += joinPart.NotifyFromConfigurationChanged;
                _fromPart.OnDocumentValueChanged += joinPart.NotifyFromDocumentChanged;

                //устанавливаем нотификацию на при изменении join'ов для частей select и where
                //============================

                joinPart.OnConfigurationValueChanged +=
                    config => _selectPart.NotifyJoinConfigurationChanged(joinPart, config);
                joinPart.OnDocumentValueChanged += document => _selectPart.NotifyJoinDocumentChanged(joinPart, document);
                joinPart.OnAliasValueChanged += alias => _selectPart.NotifyJoinAliasChanged(joinPart, alias);
                //=============================

                joinPart.NotifyFromConfigurationChanged(_fromPart.Configuration);
                joinPart.NotifyFromDocumentChanged(_fromPart.Document);

                _controller.RegisterControl(joinPart);
                ShowControl(joinPart);

                return joinPart;
            };
            SyntaxTree.OnRemoveJoinControl = control =>
            {
                _controller.UnregisterControl(control);
                ShowControl(_fromPart);
            };
            SyntaxTree.OnAddSelectControl = link => _selectPart;
            SyntaxTree.OnAddWhereControl = link => _wherePart;

            SyntaxTree.OnPressFromSection = ShowControl;
            SyntaxTree.OnPressJoinAction = ShowControl;
            SyntaxTree.OnPressSelectSection = ShowControl;
            SyntaxTree.OnPressWhereAction = ShowControl;
        }

        private void ShowControl(Control control)
        {
            PanelSectionConstructor.Controls.Cast<Control>().ToList().ForEach(c => c.Parent = null);

            control.Dock = DockStyle.Fill;
            control.Parent = PanelSectionConstructor;
            control.Visible = true;
        }

        private void Initialize(Control control)
        {
            foreach (var requestExecutor in _controller.GetProviderList<IRequestExecutor>(control))
            {
                InvokeEvents(requestExecutor);
            }


            foreach (var provider in _controller.GetProviderList<Control>(control))
            {
                InvokeEvents(provider);
            }

            foreach (var loadInitializer in _controller.GetProviderList<IInitializedOnLoad>(control))
            {
                InvokeEvents(loadInitializer);
            }
        }

        private void InvokeEvents(object argument)
        {
            var loadInitializer = argument as IInitializedOnLoad;

            //Control control = argument as Control;
            //if (control != null)
            //{
            //    var propInfo = control.GetType().GetProperty("DataProvider");
            //    if (propInfo != null)
            //    {
            //        IDataProvider dp = new DataProviderStandard();
            //        propInfo.SetValue(control, dp, null);
            //    }
            //}

            var requestExecutor = argument as IRequestExecutor;

            if (requestExecutor != null)
            {
                requestExecutor.InitRouting(GetHostingConfig());
            }

            if (loadInitializer != null)
            {
                loadInitializer.OnLoad();
            }
        }

        private void CreateDatabaseButtonClick(object sender, EventArgs e)
        {
        }

        private void ButtonExecuteQueryClick(object sender, EventArgs e)
        {
            var queryExecutor = new QueryExecutorForm {QueryText = JsonQueryEditor.JsonQueryText};
            queryExecutor.ShowDialog();
        }

        public IEnumerable<SchemaObject> GetSelectObjects()
        {
            return _selectPart.SelectedObjects;
        }

        public string GetQuery()
        {
            return JsonQueryEditor.JsonQueryText;
        }
    }
}