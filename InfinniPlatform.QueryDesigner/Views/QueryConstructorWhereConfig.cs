using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorWhereConfig : UserControl, IQueryBlockProvider
    {
        private static int itemsCounter;
        private dynamic _fromSettings = new DynamicWrapper();
        private List<dynamic> _joinSettings = new List<dynamic>();

        public QueryConstructorWhereConfig()
        {
            InitializeComponent();

            QueryConstructorWhere.ControlType = typeof (QueryConstructorWhereCondition);
            QueryConstructorWhere.OnItemAdded = control =>
            {
                //var propInfo = control.GetType().GetProperty("DataProvider");
                //if (propInfo != null)
                //{
                //    IDataProvider dp = new DataProviderStandard();
                //    propInfo.SetValue(control, dp, null);
                //}
            };
            itemsCounter++;
            Name = string.Format("{0}{1}", Name, itemsCounter);
        }

        public ConstructOrder GetConstructOrder()
        {
            return ConstructOrder.ConstructWhere;
        }

        public void ProcessQuery(dynamic query)
        {
            if (query.Where == null)
            {
                query.Where = new List<dynamic>();
            }

            foreach (QueryConstructorWhereCondition queryConstructorWhereCondition in QueryConstructorWhere.Items)
            {
                dynamic filter = new DynamicWrapper();
                filter.CriteriaType = queryConstructorWhereCondition.CriteriaType;
                filter.Value = queryConstructorWhereCondition.Value;
                filter.Property = queryConstructorWhereCondition.Property;
                query.Where.Add(filter);
            }
        }

        public bool DefinitionCompleted()
        {
            foreach (QueryConstructorWhereCondition queryConstructorWhereCondition in QueryConstructorWhere.Items)
            {
                if (queryConstructorWhereCondition.Value == null || queryConstructorWhereCondition.Property == null)
                {
                    return false;
                }
            }
            return true;
        }

        public string GetErrorMessage()
        {
            return "Block WHERE same fields are unsettled";
        }

        private void ReloadWherePaths(IEnumerable<Control> items)
        {
            foreach (var whereConditionConstructor in items.Cast<QueryConstructorWhereCondition>().ToList())
            {
                var pathItems = new[] {_fromSettings};
                whereConditionConstructor.SetPathItems(pathItems);
            }
        }

        public void NotifyFromConfigurationChanged(string configuration)
        {
            _fromSettings = new DynamicWrapper();
            _fromSettings.Configuration = configuration;
            ReloadWherePaths(QueryConstructorWhere.Items.ToList());
        }

        public void NotifyFromDocumentChanged(string document)
        {
            _fromSettings.Document = document;
            ReloadWherePaths(QueryConstructorWhere.Items.ToList());
        }

        private void OnDeleteQuery(Control control)
        {
            QueryConstructorWhere.DeleteItem(control);
        }

        private void AddConditionButtonClick(object sender, EventArgs e)
        {
            var item = (QueryConstructorWhereCondition) QueryConstructorWhere.AddItem();
            item.OnQueryDelete = OnDeleteQuery;
            item.PathResolveType = PathResolveType.Where;

            item.Caption = "Document field (FROM/JOIN)";
            ReloadWherePaths(new[] {item});
        }
    }
}