using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.Sdk.Application.Dynamic;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorSelectConfig : UserControl, IQueryBlockProvider
    {
        private static int itemsCounter;
        private dynamic _fromSettings = new DynamicWrapper();
        private readonly List<dynamic> _joinSettings = new List<dynamic>();

        public QueryConstructorSelectConfig()
        {
            InitializeComponent();

            SelectPart.ControlType = typeof (QueryConstructorSelectProperty);
            SelectPart.OnItemAdded = control =>
            {
                var propInfo = control.GetType().GetProperty("DataProvider");
                if (propInfo != null)
                {
                    //IDataProvider dp = new DataProviderStandard();
                    //propInfo.SetValue(control, dp, null);
                }
            };

            itemsCounter++;
            Name = string.Format("{0}{1}", Name, itemsCounter);
        }

        public IEnumerable<SchemaObject> SelectedObjects
        {
            get
            {
                return SelectPart.Items.Cast<QueryConstructorSelectProperty>().Select(p => p.SelectedObject).ToList();
            }
        }

        public ConstructOrder GetConstructOrder()
        {
            return ConstructOrder.ConstructSelect;
        }

        public void ProcessQuery(dynamic query)
        {
            if (query.Select == null)
            {
                query.Select = new List<dynamic>();
            }

            foreach (QueryConstructorSelectProperty querySelectCondition in SelectPart.Items)
            {
                dynamic select = querySelectCondition.SelectValue;

                query.Select.Add(select);
            }
        }

        public bool DefinitionCompleted()
        {
            foreach (QueryConstructorSelectProperty queryConstructorSelectProperty in SelectPart.Items.ToList())
            {
                if (queryConstructorSelectProperty.SelectValue == null)
                {
                    return false;
                }
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return "Some of select field are unsettled";
        }

        private void ReloadWherePaths(IEnumerable<Control> items)
        {
            foreach (var selectItem in items.Cast<QueryConstructorSelectProperty>().ToList())
            {
                var pathItems = _joinSettings.Concat(new[] {_fromSettings});
                selectItem.SetPathItems(pathItems);
            }
        }

        public void NotifyFromConfigurationChanged(string configuration)
        {
            _fromSettings = new DynamicWrapper();
            _fromSettings.Configuration = configuration;
            ReloadWherePaths(SelectPart.Items.ToList());
        }

        public void NotifyFromDocumentChanged(string document)
        {
            _fromSettings.Document = document;
            ReloadWherePaths(SelectPart.Items.ToList());
        }

        public void NotifyFromAliasChanged(string alias)
        {
            _fromSettings.Alias = alias;
            ReloadWherePaths(SelectPart.Items.ToList());
        }

        public void NotifyJoinConfigurationChanged(Control joinPart, string config)
        {
            var existingPart = FindOrCreateJoinPart(joinPart);
            existingPart.Configuration = config;
        }

        public void NotifyJoinDocumentChanged(Control joinPart, string document)
        {
            var existingPart = FindOrCreateJoinPart(joinPart);
            existingPart.Document = document;
            ReloadWherePaths(SelectPart.Items.ToList());
        }

        public void NotifyJoinAliasChanged(Control joinPart, string alias)
        {
            var existingPart = FindOrCreateJoinPart(joinPart);
            existingPart.Alias = alias;
            ReloadWherePaths(SelectPart.Items.ToList());
        }

        private dynamic FindOrCreateJoinPart(Control joinPart)
        {
            var existingPart = _joinSettings.FirstOrDefault(d => d.ControlName == joinPart.Name);
            if (existingPart == null)
            {
                existingPart = new DynamicWrapper();
                existingPart.ControlName = joinPart.Name;
                _joinSettings.Add(existingPart);
            }
            return existingPart;
        }

        private void OnDeleteQuery(Control control)
        {
            SelectPart.DeleteItem(control);
        }

        private void AddConditionButtonClick(object sender, EventArgs e)
        {
            var item = (QueryConstructorSelectProperty) SelectPart.AddItem();
            item.OnDeleteItem = OnDeleteQuery;
            item.PathResolveType = PathResolveType.Select;

            item.Caption = "Field to Select";
            ReloadWherePaths(new[] {item});
        }
    }
}