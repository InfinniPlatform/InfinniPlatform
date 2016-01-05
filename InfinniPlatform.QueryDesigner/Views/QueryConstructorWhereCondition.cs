using System;
using System.Collections.Generic;
using System.Windows.Forms;

using InfinniPlatform.Core.Schema;
using InfinniPlatform.QueryDesigner.Contracts;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.QueryDesigner.Views
{
    public partial class QueryConstructorWhereCondition : UserControl
    {
        public QueryConstructorWhereCondition()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get { return FieldPart.Caption; }
            set { FieldPart.Caption = value; }
        }

        public CriteriaType CriteriaType
        {
            get { return ConditionPart.Value; }
            set { ConditionPart.Value = value; }
        }

        public object Value
        {
            get { return ValuePart.EditValue; }
            set { ValuePart.EditValue = value; }
        }

        public string Property
        {
            get { return FieldPart.Path; }
        }

        public IDataProvider DataProvider
        {
            get { return FieldPart.DataProvider; }
            set { FieldPart.DataProvider = value; }
        }

        public Action<Control> OnQueryDelete { get; set; }

        public PathResolveType PathResolveType
        {
            get { return FieldPart.PathResolveType; }
            set { FieldPart.PathResolveType = value; }
        }

        public void SetPathItems(IEnumerable<dynamic> pathItems)
        {
            FieldPart.SetPathItems(pathItems);
        }

        private void ButtonRemoveClick(object sender, EventArgs e)
        {
            if (OnQueryDelete != null)
            {
                OnQueryDelete(this);
            }
        }
    }
}